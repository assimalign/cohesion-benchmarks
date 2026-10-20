using System;

using Assimalign.Cohesion.Amqp.Connections;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using LiteByteBuffer = global::Amqp.ByteBuffer;
using LiteData = global::Amqp.Framing.Data;
using LiteMessage = global::Amqp.Message;

namespace Assimalign.Cohesion.Amqp.Benchmarks.Amqp.Connections;

/// <summary>
/// Binary message bodies below and above the binary8/binary32 boundary, including buffer growth/copying.
/// Both decoders consume the same packet and return an owned payload. Accessing AMQPNetLite's Body forces
/// its otherwise deferred payload copy, matching Cohesion's eager data materialization. No broker or link.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class AmqpDataMessageBenchmarks
{
    private AmqpMessage _message = null!;
    private LiteMessage _liteMessage = null!;
    private byte[] _wire = null!;

    [Params(64, 16384)]
    public int PayloadSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        byte[] payload = new byte[PayloadSize];
        new Random(42).NextBytes(payload);
        _message = new AmqpMessage { DataSections = [payload] };
        _liteMessage = new LiteMessage { BodySection = new LiteData { Binary = payload } };
        _wire = Encode();
        LiteByteBuffer lite = Encode_AmqpNetLite();
        if (!Decode().Span.SequenceEqual(payload) || !Decode_AmqpNetLite().Span.SequenceEqual(payload) ||
            !AmqpMessageCodec.Decode(lite.Buffer.AsSpan(lite.Offset, lite.Length)).DataSections![0].Span.SequenceEqual(payload))
        {
            throw new InvalidOperationException("The binary messages did not round-trip across both codecs.");
        }
    }

    [Benchmark]
    [BenchmarkCategory("Encode")]
    public byte[] Encode() => AmqpMessageCodec.Encode(_message);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Encode")]
    public LiteByteBuffer Encode_AmqpNetLite() => _liteMessage.Encode();

    [Benchmark]
    [BenchmarkCategory("Decode")]
    public ReadOnlyMemory<byte> Decode() => AmqpMessageCodec.Decode(_wire).DataSections![0];

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Decode")]
    public ReadOnlyMemory<byte> Decode_AmqpNetLite()
        => (byte[])LiteMessage.Decode(new LiteByteBuffer(_wire, 0, _wire.Length, _wire.Length)).Body;
}
