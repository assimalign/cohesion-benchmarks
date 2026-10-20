using System;

using Assimalign.Cohesion.Amqp.Connections;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Amqp.Benchmarks.Amqp.Connections;

/// <summary>
/// Transfer framing with small and larger binary payloads, covering frame size checks, allocation/copying,
/// delivery tag and flags. Complements the existing fixed 512-byte history series without changing it.
/// No AMQPNetLite counterpart because its full frame codec is internal. Payloads are prebuilt in setup.
/// </summary>
public class AmqpTransferPayloadBenchmarks
{
    private AmqpFrame _frame = null!;
    private byte[] _wire = null!;

    [Params(64, 16384)]
    public int PayloadSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        byte[] payload = new byte[PayloadSize];
        new Random(42).NextBytes(payload);
        _frame = new AmqpFrame(1, new AmqpTransferPerformative
        {
            Handle = 1, DeliveryId = 1024, DeliveryTag = new byte[] { 0, 0, 4, 0 }, MessageFormat = 0,
            Settled = false, More = true,
        }, payload);
        _wire = Encode();
        AmqpFrame decoded = Decode();
        if (!decoded.Payload.Span.SequenceEqual(payload) || decoded.Performative is not AmqpTransferPerformative transfer ||
            transfer.DeliveryId != 1024 || transfer.More != true || !_wire.AsSpan().SequenceEqual(AmqpFrameCodec.Encode(decoded)))
        {
            throw new InvalidOperationException("The transfer fragment did not round-trip.");
        }
    }

    [Benchmark] public byte[] Encode() => AmqpFrameCodec.Encode(_frame);
    [Benchmark] public AmqpFrame Decode() => AmqpFrameCodec.Decode(_wire);
}
