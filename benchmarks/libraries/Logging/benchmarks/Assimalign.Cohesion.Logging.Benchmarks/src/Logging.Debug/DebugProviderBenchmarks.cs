using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Logging.Benchmarks.Logging.Debug;

/// <summary>
/// Debug rendering through its public writer callback, independent of debugger attachment.
/// The sink consumes line lengths. Microsoft Debug has no equivalent injectable output path and
/// its debugger gate would measure a disabled operation on CI, so there is no baseline here.
/// </summary>
public class DebugProviderBenchmarks
{
    private DebugLoggerProvider _provider = null!;
    private ILogger _logger = null!;
    private LoggerEntry _entry = null!;
    private LoggerEntry _error = null!;
    private int _characters;

    [Params(false, true)] public bool IncludeAttributes { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _provider = new DebugLoggerProvider(new DebugLoggerOptions
        {
            EmitOnlyWhenDebuggerAttached = false, IncludeAttributes = IncludeAttributes,
            Writer = line => _characters = unchecked(_characters + line.Length),
        });
        _logger = _provider.Create("Orders.Api");
        var attributes = new Dictionary<string, object?> { ["OrderId"] = 42, ["Elapsed"] = 12.5 };
        _entry = new LoggerEntry(LogLevel.Information, "Orders.Api", "Order processed", attributes: attributes, timestamp: DateTimeOffset.UnixEpoch);
        _error = new LoggerEntry(LogLevel.Error, "Orders.Api", "Order failed", new InvalidOperationException("Order rejected"), attributes, timestamp: DateTimeOffset.UnixEpoch);
        if (Write() == 0 || WriteError() <= _entry.Message.Length)
            throw new InvalidOperationException("Debug writer did not emit.");
    }

    [GlobalCleanup] public void Cleanup() => _provider.Dispose();
    [Benchmark] public int Write() { _logger.Log(_entry); return _characters; }
    [Benchmark] public int WriteError() { _logger.Log(_error); return _characters; }
}
