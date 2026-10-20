using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Logging.Benchmarks.Logging.Console;

/// <summary>
/// Synchronous console write/flush to TextWriter.Null, with formatting isolated as a separate operation.
/// Formatter-only intentionally binds the internal ConsoleLogFormatter once in setup: the question is
/// its rendering cost without logger dispatch/locking. All provider operations use public APIs.
/// Microsoft console logging queues writes asynchronously and uses a different output schema, so it
/// is not a same-work baseline for these operations.
/// </summary>
public class ConsoleProviderBenchmarks
{
    private ConsoleLoggerProvider _provider = null!;
    private ILogger _logger = null!;
    private ConsoleLoggerOptions _options = null!;
    private LoggerEntry _entry = null!;
    private LoggerEntry _error = null!;
    private Action<ILoggerEntry, TextWriter, ConsoleLoggerOptions> _format = null!;

    [Params(false, true)] public bool IncludeAttributes { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _options = new ConsoleLoggerOptions { Output = TextWriter.Null, ErrorOutput = TextWriter.Null, IncludeAttributes = IncludeAttributes };
        _provider = new ConsoleLoggerProvider(_options);
        _logger = _provider.Create("Orders.Api");
        var attributes = new Dictionary<string, object?> { ["OrderId"] = 42, ["Elapsed"] = 12.5 };
        _entry = new LoggerEntry(LogLevel.Information, "Orders.Api", "Order processed", attributes: attributes, timestamp: DateTimeOffset.UnixEpoch);
        _error = new LoggerEntry(LogLevel.Error, "Orders.Api", "Order failed", new InvalidOperationException("Order rejected"), attributes, timestamp: DateTimeOffset.UnixEpoch);
        Type formatter = typeof(ConsoleLoggerProvider).Assembly.GetType("Assimalign.Cohesion.Logging.Internal.ConsoleLogFormatter", throwOnError: true)!;
        _format = formatter.GetMethod("Write", BindingFlags.Public | BindingFlags.Static)!
            .CreateDelegate<Action<ILoggerEntry, TextWriter, ConsoleLoggerOptions>>();

        using var output = new StringWriter();
        using var errors = new StringWriter();
        using var formatted = new StringWriter();
        _options.Output = output;
        _options.ErrorOutput = errors;
        Write(); WriteError();
        _format(_entry, formatted, _options);
        if (output.ToString() != formatted.ToString() || !errors.ToString().Contains("Order rejected", StringComparison.Ordinal) ||
            output.ToString().Contains("OrderId=42", StringComparison.Ordinal) != IncludeAttributes)
            throw new InvalidOperationException("Console output mismatch.");
        _options.Output = TextWriter.Null;
        _options.ErrorOutput = TextWriter.Null;
    }

    [GlobalCleanup] public void Cleanup() => _provider.Dispose();
    [Benchmark] public void Write() => _logger.Log(_entry);
    [Benchmark] public void WriteError() => _logger.Log(_error);
    [Benchmark] public void Format() => _format(_entry, TextWriter.Null, _options);
}
