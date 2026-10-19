using System.Collections.Generic;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using ExtensionsLogging = Microsoft.Extensions.Logging;

namespace Assimalign.Cohesion.Logging.Benchmarks.Logging;

/// <summary>
/// Writing through a factory-created logger to one sink that discards entries, against
/// Microsoft.Extensions.Logging (the baseline in each category): an enabled entry with two structured values, an
/// entry below the minimum level, and a scope with one entry inside it. This is the per-call cost every
/// instrumented code path pays before any transport runs.
/// </summary>
/// <remarks>
/// Each side uses its ordinary call shape for structured values: Cohesion takes an attribute dictionary, which the
/// caller builds before the call; Microsoft.Extensions.Logging takes a message template and a <c>params</c> array.
/// Both are built even when the level is disabled, so the <c>Disabled</c> category measures that too.
/// </remarks>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class LoggerBenchmarks
{
    private const string Category = "Orders.Api";

    private ILoggerFactory _cohesionFactory = null!;
    private ILogger _cohesion = null!;
    private ExtensionsLogging.LoggerFactory _extensionsFactory = null!;
    private ExtensionsLogging.ILogger _extensions = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesionFactory = new LoggerFactoryBuilder()
            .AddProvider(new NullSinks.CohesionProvider())
            .SetMinimumLevel(LogLevel.Information)
            .Build();
        _cohesion = _cohesionFactory.Create(Category);

        _extensionsFactory = new ExtensionsLogging.LoggerFactory(
            [new NullSinks.ExtensionsProvider()],
            new ExtensionsLogging.LoggerFilterOptions { MinLevel = ExtensionsLogging.LogLevel.Information });
        _extensions = _extensionsFactory.CreateLogger(Category);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _cohesionFactory.Dispose();
        _extensionsFactory.Dispose();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Enabled")]
    public void LogEnabled_MicrosoftExtensions()
        => ExtensionsLogging.LoggerExtensions.LogInformation(_extensions, "Order {OrderId} processed in {Elapsed} ms", 42, 12.5);

    [Benchmark]
    [BenchmarkCategory("Enabled")]
    public void LogEnabled()
        => _cohesion.LogInformation(Category, "Order processed", new Dictionary<string, object?>
        {
            ["OrderId"] = 42,
            ["Elapsed"] = 12.5,
        });

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Disabled")]
    public void LogDisabled_MicrosoftExtensions()
        => ExtensionsLogging.LoggerExtensions.LogDebug(_extensions, "Cache miss for {Key}", "orders:42");

    [Benchmark]
    [BenchmarkCategory("Disabled")]
    public void LogDisabled()
        => _cohesion.LogDebug(Category, "Cache miss", new Dictionary<string, object?> { ["Key"] = "orders:42" });

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Scope")]
    public void Scope_MicrosoftExtensions()
    {
        using (_extensions.BeginScope("Request"))
        {
            ExtensionsLogging.LoggerExtensions.LogInformation(_extensions, "Request handled");
        }
    }

    [Benchmark]
    [BenchmarkCategory("Scope")]
    public void Scope()
    {
        using IScopedLogger scope = _cohesion.BeginScope(Category, "Request");
        scope.LogInformation(Category, "Request handled");
    }
}
