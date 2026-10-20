using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.Hosting.Telemetry;
using Assimalign.Cohesion.Logging;

namespace Assimalign.Cohesion.Hosting.Benchmarks.Hosting.Telemetry;

/// <summary>
/// Measures Hosting.Telemetry's JSON console formatter itself, paid for each emitted console record: attribute
/// conversion, escaping, timestamp formatting and serialization. The formatter is internal and its public caller
/// also exports OTLP; a delegate bound once in setup isolates this specific formatter without network or console I/O.
/// Microsoft.Extensions.Logging.Console has a different JSON schema and scope contract, so it is not a counterpart.
/// </summary>
public class JsonConsoleFormattingBenchmarks
{
    private Action<ILoggerEntry, TextWriter> _format = null!;
    private LoggerEntry _entry = null!;
    private StringBuilder _buffer = null!;
    private StringWriter _writer = null!;

    [Params(0, 8)]
    public int AttributeCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Type formatter = typeof(ResourceTelemetry).Assembly.GetType(
            "Assimalign.Cohesion.Hosting.Telemetry.Internal.JsonConsoleFormatter", throwOnError: true)!;
        _format = formatter.GetMethod("Write", BindingFlags.Static | BindingFlags.NonPublic)!
            .CreateDelegate<Action<ILoggerEntry, TextWriter>>();
        Dictionary<string, object?> attributes = new();
        for (int index = 0; index < AttributeCount; index++)
        {
            attributes[$"attribute{index}"] = index;
        }

        _entry = new LoggerEntry(LogLevel.Information, "Orders", "Processed \"order\"\nwith details",
            attributes: attributes, timestamp: new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));
        _buffer = new StringBuilder(2048);
        _writer = new StringWriter(_buffer, CultureInfo.InvariantCulture);
        Format();
        using JsonDocument document = JsonDocument.Parse(_buffer.ToString());
        if (document.RootElement.GetProperty("message").GetString() != _entry.Message
            || document.RootElement.GetProperty("attributes").EnumerateObject().Count() != AttributeCount)
        {
            throw new InvalidOperationException("The formatter did not preserve the log record.");
        }
    }

    [GlobalCleanup]
    public void Cleanup() => _writer.Dispose();

    [Benchmark]
    public int Format()
    {
        _buffer.Clear();
        _format(_entry, _writer);
        return _buffer.Length;
    }
}
