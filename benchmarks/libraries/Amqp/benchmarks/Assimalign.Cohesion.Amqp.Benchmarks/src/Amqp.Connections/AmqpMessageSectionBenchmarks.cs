using System;
using System.Collections;
using System.Collections.Generic;

using Assimalign.Cohesion.Amqp.Connections;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using LiteByteBuffer = global::Amqp.ByteBuffer;
using LiteDeliveryAnnotations = global::Amqp.Framing.DeliveryAnnotations;
using LiteFooter = global::Amqp.Framing.Footer;
using LiteList = global::Amqp.Types.List;
using LiteMessage = global::Amqp.Message;
using LiteMessageAnnotations = global::Amqp.Framing.MessageAnnotations;
using LiteSequence = global::Amqp.Framing.AmqpSequence;
using LiteSymbol = global::Amqp.Types.Symbol;

namespace Assimalign.Cohesion.Amqp.Benchmarks.Amqp.Connections;

/// <summary>
/// Structured sequence bodies and the delivery/message annotation plus footer sections used by brokers.
/// Both libraries encode equivalent sections and decode the same packet into materialized values;
/// neither performs transport work. Annotations are standalone sections to isolate their map codecs.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class AmqpMessageSectionBenchmarks
{
    private AmqpMessage _message = null!;
    private LiteMessage _liteMessage = null!;
    private byte[] _wire = null!;

    [Params("Sequence", "Annotations")]
    public string Sections { get; set; } = "";

    [GlobalSetup]
    public void Setup()
    {
        if (Sections == "Sequence")
        {
            object?[] values = ["order-42", 1000, true, 123.5d, new byte[] { 1, 2, 3, 4 }];
            _message = new AmqpMessage { SequenceSections = [values] };
            LiteList list = new();
            foreach (object? value in values)
            {
                list.Add(value);
            }
            _liteMessage = new LiteMessage { BodySection = new LiteSequence { List = list } };
        }
        else
        {
            _message = new AmqpMessage
            {
                DeliveryAnnotations = new Dictionary<AmqpSymbol, object?> { ["route"] = "orders", ["attempt"] = 1000 },
                MessageAnnotations = new Dictionary<AmqpSymbol, object?> { ["partition"] = 42u, ["trace-id"] = "0123456789abcdef" },
                Footer = new Dictionary<AmqpSymbol, object?> { ["checksum"] = new byte[] { 1, 2, 3, 4 } },
            };
            LiteDeliveryAnnotations delivery = new();
            delivery[new LiteSymbol("route")] = "orders";
            delivery[new LiteSymbol("attempt")] = 1000;
            LiteMessageAnnotations annotations = new();
            annotations[new LiteSymbol("partition")] = 42u;
            annotations[new LiteSymbol("trace-id")] = "0123456789abcdef";
            LiteFooter footer = new();
            footer[new LiteSymbol("checksum")] = new byte[] { 1, 2, 3, 4 };
            _liteMessage = new LiteMessage { DeliveryAnnotations = delivery, MessageAnnotations = annotations, Footer = footer };
        }
        _wire = Encode();
        Validate(Decode(), Decode_AmqpNetLite());
        LiteByteBuffer lite = Encode_AmqpNetLite();
        Validate(AmqpMessageCodec.Decode(lite.Buffer.AsSpan(lite.Offset, lite.Length)),
            LiteMessage.Decode(new LiteByteBuffer(lite.Buffer, lite.Offset, lite.Length, lite.Length)));
    }

    [Benchmark]
    [BenchmarkCategory("Encode")]
    public byte[] Encode() => AmqpMessageCodec.Encode(_message);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Encode")]
    public LiteByteBuffer Encode_AmqpNetLite() => _liteMessage.Encode();

    [Benchmark]
    [BenchmarkCategory("Decode")]
    public AmqpMessage Decode() => AmqpMessageCodec.Decode(_wire);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Decode")]
    public LiteMessage Decode_AmqpNetLite()
    {
        LiteMessage message = LiteMessage.Decode(new LiteByteBuffer(_wire, 0, _wire.Length, _wire.Length));
        if (message.BodySection is not null)
        {
            GC.KeepAlive(message.Body);
        }
        return message;
    }

    private void Validate(AmqpMessage cohesion, LiteMessage lite)
    {
        if (Sections == "Sequence")
        {
            IReadOnlyList<object?> left = cohesion.SequenceSections![0];
            IList right = (IList)lite.Body;
            if (left.Count != 5 || right.Count != 5 || !Equals(left[0], "order-42") || !Equals(right[0], left[0]) ||
                !Equals(left[1], 1000) || !Equals(right[1], left[1]) || !Equals(left[2], true) || !Equals(right[2], left[2]) ||
                !Equals(left[3], 123.5d) || !Equals(right[3], left[3]) ||
                !((byte[])left[4]!).AsSpan().SequenceEqual((byte[])right[4]!))
            {
                throw new InvalidOperationException("The sequence did not round-trip across both codecs.");
            }
        }
        else if (!Equals(cohesion.DeliveryAnnotations!["route"], "orders") ||
            !Equals(lite.DeliveryAnnotations[new LiteSymbol("route")], "orders") ||
            !Equals(cohesion.DeliveryAnnotations["attempt"], 1000) ||
            !Equals(lite.DeliveryAnnotations[new LiteSymbol("attempt")], 1000) ||
            !Equals(cohesion.MessageAnnotations!["partition"], 42u) ||
            !Equals(lite.MessageAnnotations[new LiteSymbol("partition")], 42u) ||
            !Equals(cohesion.MessageAnnotations["trace-id"], "0123456789abcdef") ||
            !Equals(lite.MessageAnnotations[new LiteSymbol("trace-id")], "0123456789abcdef") ||
            !((byte[])cohesion.Footer!["checksum"]!).AsSpan().SequenceEqual((byte[])lite.Footer[new LiteSymbol("checksum")]))
        {
            throw new InvalidOperationException("The annotations did not round-trip across both codecs.");
        }
    }
}
