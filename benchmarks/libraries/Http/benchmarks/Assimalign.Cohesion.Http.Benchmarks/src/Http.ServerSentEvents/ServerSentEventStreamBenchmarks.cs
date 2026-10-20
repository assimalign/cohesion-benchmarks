using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.ServerSentEvents;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Http.Benchmarks.Http.ServerSentEvents;

/// <summary>
/// Formats eight events from an async enumerable into a stream, the contract of the BCL's
/// System.Net.ServerSentEvents.SseFormatter. Both sides enumerate prepared typed events,
/// format UTF-8 with multiline data, id, type and retry, and write each event to a reusable
/// MemoryStream. Cohesion reuses one buffer within the batch, as the BCL does internally.
/// Field order differs but the parsed events are identical; comments are excluded because
/// the BCL event model does not carry them. Neither side measures HTTP transport or flushing.
/// </summary>
public class ServerSentEventStreamBenchmarks
{
    private IAsyncEnumerable<ServerSentEvent> _events = null!;
    private IAsyncEnumerable<SseItem<string>> _bclEvents = null!;
    private MemoryStream _stream = null!;
    private MemoryStream _bclStream = null!;

    [GlobalSetup]
    public async Task Setup()
    {
        ServerSentEvent[] events = new ServerSentEvent[8];
        SseItem<string>[] bclEvents = new SseItem<string>[8];
        for (int i = 0; i < events.Length; i++)
        {
            string data = $"café update {i}\r\n東京\ncomplete";
            string id = i.ToString(System.Globalization.CultureInfo.InvariantCulture);
            events[i] = new ServerSentEvent(data) { EventType = "update", Id = id, Retry = TimeSpan.FromSeconds(3) };
            bclEvents[i] = new SseItem<string>(data, "update") { EventId = id, ReconnectionInterval = TimeSpan.FromSeconds(3) };
        }
        _events = Enumerate(events);
        _bclEvents = Enumerate(bclEvents);
        _stream = new MemoryStream(4096);
        _bclStream = new MemoryStream(4096);
        await FormatBatch();
        await FormatBatch_SystemNetServerSentEvents();
        _stream.Position = 0;
        _bclStream.Position = 0;
        SseItem<string>[] actual = SseParser.Create(_stream).Enumerate().ToArray();
        SseItem<string>[] expected = SseParser.Create(_bclStream).Enumerate().ToArray();
        if (actual.Length != 8 || expected.Length != 8)
            throw new InvalidOperationException("SSE event count differs.");
        for (int i = 0; i < actual.Length; i++)
        {
            if (actual[i].Data != expected[i].Data || actual[i].EventType != expected[i].EventType
                || actual[i].EventId != expected[i].EventId
                || actual[i].ReconnectionInterval != expected[i].ReconnectionInterval)
                throw new InvalidOperationException("SSE events differ after parsing.");
        }
    }

    private static async IAsyncEnumerable<T> Enumerate<T>(T[] values)
    {
        foreach (T value in values) yield return value;
        await Task.CompletedTask;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _stream.Dispose();
        _bclStream.Dispose();
    }

    [Benchmark]
    public async Task<long> FormatBatch()
    {
        _stream.SetLength(0);
        ArrayBufferWriter<byte> writer = new();
        await foreach (ServerSentEvent item in _events)
        {
            writer.Clear();
            ServerSentEventFormatter.Write(item, writer);
            await _stream.WriteAsync(writer.WrittenMemory);
        }
        return _stream.Length;
    }

    [Benchmark(Baseline = true)]
    public async Task<long> FormatBatch_SystemNetServerSentEvents()
    {
        _bclStream.SetLength(0);
        await SseFormatter.WriteAsync(_bclEvents, _bclStream);
        return _bclStream.Length;
    }
}
