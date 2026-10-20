using System;
using System.Buffers;
using System.Text;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Http.Benchmarks.Http.ServerSentEvents;

/// <summary>
/// UTF-8 SSE framing into either an owned array or a caller-owned buffer. Single-line JSON and
/// multiline Unicode data exercise different splitting/encoding work. These buffer/array APIs
/// have no direct BCL equivalent; ServerSentEventStreamBenchmarks adapts both libraries to the
/// BCL's stream/enumerable contract for a separate comparison.
/// </summary>
public class ServerSentEventBenchmarks
{
    private ServerSentEvent _event;
    private ArrayBufferWriter<byte> _writer = null!;

    [Params(false, true)]
    public bool Multiline { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _event = new ServerSentEvent(Multiline ? "first café\r\nsecond 東京\nthird\rfourth" : "{\"count\":42}")
        {
            EventType = "update",
            Id = "42",
            Retry = TimeSpan.FromSeconds(3),
        };
        _writer = new ArrayBufferWriter<byte>(1024);
        byte[] formatted = Format();
        Write();
        if (!formatted.AsSpan().SequenceEqual(_writer.WrittenSpan)
            || !Encoding.UTF8.GetString(formatted).EndsWith("\n\n", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("SSE formatting paths disagree.");
        }
    }

    [Benchmark]
    public byte[] Format() => ServerSentEventFormatter.Format(_event);

    [Benchmark]
    public int Write()
    {
        _writer.Clear();
        ServerSentEventFormatter.Write(_event, _writer);
        return _writer.WrittenCount;
    }
}
