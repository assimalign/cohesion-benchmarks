using System;

using Assimalign.Cohesion.Amqp.Connections;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Amqp.Benchmarks.Amqp.Connections;

/// <summary>
/// Encoding and decoding a <c>transfer</c> frame carrying a 512-byte message payload: the frame every delivery
/// travels in, so its codec runs once per message per direction on top of the message codec. AMQPNetLite's frame
/// codec is internal to its connection, so there is no counterpart to compare against; these track Cohesion
/// against itself.
/// </summary>
public class AmqpFrameBenchmarks
{
    private AmqpFrame _frame = null!;
    private byte[] _encoded = null!;

    [GlobalSetup]
    public void Setup()
    {
        byte[] payload = new byte[512];
        new Random(42).NextBytes(payload);

        _frame = new AmqpFrame(
            channel: 1,
            new AmqpTransferPerformative
            {
                Handle = 0,
                DeliveryId = 42,
                DeliveryTag = new byte[] { 0, 0, 0, 42 },
                MessageFormat = 0,
                Settled = false,
            },
            payload);

        _encoded = AmqpFrameCodec.Encode(_frame);
        if (DecodeTransferFrame().Payload.Length != payload.Length)
        {
            throw new InvalidOperationException("The transfer frame did not round-trip.");
        }
    }

    [Benchmark]
    public byte[] EncodeTransferFrame() => AmqpFrameCodec.Encode(_frame);

    [Benchmark]
    public AmqpFrame DecodeTransferFrame() => AmqpFrameCodec.Decode(_encoded);
}
