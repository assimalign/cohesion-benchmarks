using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using M = Microsoft.Extensions.Logging;

namespace Assimalign.Cohesion.Logging.Benchmarks.Logging;

/// <summary>
/// Cheap disabled calls, scope push/pop, and fan-out to one/four sinks. Dispatch and scope use the
/// same prepared entry as state on both sides to isolate the pipeline from payload/metadata creation.
/// Sinks do not format. The existing LoggerBenchmarks separately measures per-call structured state creation.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class PipelineBenchmarks
{
    private ILoggerFactory _cohesionFactory = null!;
    private M.LoggerFactory _extensionsFactory = null!;
    private ILogger _cohesion = null!;
    private M.ILogger _extensions = null!;
    private LoggerEntry _entry = null!;

    [Params(1, 4)] public int Providers { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        ILoggerFactoryBuilder builder = new LoggerFactoryBuilder().SetMinimumLevel(LogLevel.Information);
        var providers = new M.ILoggerProvider[Providers];
        for (int i = 0; i < Providers; i++)
        {
            builder.AddProvider(new NullSinks.CohesionProvider($"sink-{i}"));
            providers[i] = new NullSinks.ExtensionsProvider();
        }
        _cohesionFactory = builder.Build();
        _extensionsFactory = new M.LoggerFactory(providers, new M.LoggerFilterOptions { MinLevel = M.LogLevel.Information });
        _cohesion = _cohesionFactory.Create("Orders.Api");
        _extensions = _extensionsFactory.CreateLogger("Orders.Api");
        _entry = new LoggerEntry(LogLevel.Information, "Orders.Api", "Order processed", timestamp: DateTimeOffset.UnixEpoch);
        if (_cohesion.IsEnabled(LogLevel.Debug) || _extensions.IsEnabled(M.LogLevel.Debug))
            throw new InvalidOperationException("Disabled call is enabled.");
    }

    [GlobalCleanup] public void Cleanup() { _cohesionFactory.Dispose(); _extensionsFactory.Dispose(); }

    [Benchmark, BenchmarkCategory("Disabled")]
    public void LogDisabled() => _cohesion.LogDebug("Orders.Api", "Cache miss");
    [Benchmark(Baseline = true), BenchmarkCategory("Disabled")]
    public void LogDisabled_MicrosoftExtensions() => M.LoggerExtensions.LogDebug(_extensions, "Cache miss");

    [Benchmark, BenchmarkCategory("Dispatch")]
    public void Dispatch() => _cohesion.Log(_entry);
    [Benchmark(Baseline = true), BenchmarkCategory("Dispatch")]
    public void Dispatch_MicrosoftExtensions() => _extensions.Log(M.LogLevel.Information, default, _entry, null, static (entry, _) => entry.Message);

    [Benchmark, BenchmarkCategory("Scope")]
    public void PushAndPopScope() { using IScopedLogger scope = _cohesion.BeginScope(_entry); }
    [Benchmark(Baseline = true), BenchmarkCategory("Scope")]
    public void PushAndPopScope_MicrosoftExtensions() { using IDisposable? scope = _extensions.BeginScope(_entry); }
}
