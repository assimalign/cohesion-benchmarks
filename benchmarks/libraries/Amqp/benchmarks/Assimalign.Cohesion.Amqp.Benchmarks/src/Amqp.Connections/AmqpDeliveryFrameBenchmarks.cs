using System;
using System.Collections.Generic;

using Assimalign.Cohesion.Amqp.Connections;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Amqp.Benchmarks.Amqp.Connections;

/// <summary>
/// Repeated delivery control traffic: replenish credit, drain a link, and settle a delivery range.
/// Measures full frame encoding/decoding including performative field dispatch and described delivery state.
/// AMQPNetLite's full frame codec is internal; its public performative encoder omits frame headers and
/// frame validation, so it is not used as a cheaper baseline. No connection or broker is involved.
/// </summary>
public class AmqpDeliveryFrameBenchmarks
{
    private AmqpFrame _frame = null!;
    private byte[] _wire = null!;

    [Params("Credit", "Drain", "Disposition")]
    public string Kind { get; set; } = "";

    [GlobalSetup]
    public void Setup()
    {
        AmqpPerformative performative = Kind switch
        {
            "Credit" => new AmqpFlowPerformative
            {
                NextIncomingId = 1024, IncomingWindow = 2048, NextOutgoingId = 4096, OutgoingWindow = 2048,
                Handle = 1, DeliveryCount = 1024, LinkCredit = 500, Available = 100,
            },
            "Drain" => new AmqpFlowPerformative
            {
                NextIncomingId = 1024, IncomingWindow = 2048, NextOutgoingId = 4096, OutgoingWindow = 2048,
                Handle = 1, DeliveryCount = 1024, LinkCredit = 500, Drain = true, Echo = true,
                Properties = new Dictionary<AmqpSymbol, object?> { ["reason"] = "idle", ["generation"] = 3u },
            },
            "Disposition" => new AmqpDispositionPerformative
            {
                Role = true, First = 1024, Last = 1123, Settled = true, State = AmqpDescribedValue.Accepted(), Batchable = true,
            },
            _ => throw new InvalidOperationException("Unknown delivery control frame."),
        };
        _frame = new AmqpFrame(1, performative, ReadOnlyMemory<byte>.Empty);
        _wire = Encode();
        AmqpFrame decoded = Decode();
        if (decoded.Channel != 1 || decoded.Performative?.GetType() != performative.GetType() ||
            !_wire.AsSpan().SequenceEqual(AmqpFrameCodec.Encode(decoded)))
        {
            throw new InvalidOperationException("Delivery control frame did not round-trip.");
        }
    }

    [Benchmark] public byte[] Encode() => AmqpFrameCodec.Encode(_frame);
    [Benchmark] public AmqpFrame Decode() => AmqpFrameCodec.Decode(_wire);
}
