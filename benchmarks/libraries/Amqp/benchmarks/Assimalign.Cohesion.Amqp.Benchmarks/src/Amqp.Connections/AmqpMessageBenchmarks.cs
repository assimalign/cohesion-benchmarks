using System;
using System.Collections.Generic;

using Assimalign.Cohesion.Amqp.Connections;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using LiteApplicationProperties = global::Amqp.Framing.ApplicationProperties;
using LiteByteBuffer = global::Amqp.ByteBuffer;
using LiteData = global::Amqp.Framing.Data;
using LiteHeader = global::Amqp.Framing.Header;
using LiteMessage = global::Amqp.Message;
using LiteProperties = global::Amqp.Framing.Properties;

namespace Assimalign.Cohesion.Amqp.Benchmarks.Amqp.Connections;

/// <summary>
/// Encoding and decoding an AMQP 1.0 bare message (header, properties, three application properties and a
/// 256-byte data section): the per-message cost on every send and every receive. The baseline is AMQPNetLite,
/// the reference .NET AMQP 1.0 client (<c>Message.Encode</c> / <c>Message.Decode</c>), with the same sections.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class AmqpMessageBenchmarks
{
    private const string encodeCategory = "Encode";
    private const string decodeCategory = "Decode";

    private static readonly DateTimeOffset _created = new(2026, 10, 18, 12, 0, 0, TimeSpan.Zero);

    private AmqpMessage _message = null!;
    private LiteMessage _liteMessage = null!;
    private byte[] _encoded = null!;
    private byte[] _liteEncoded = null!;

    [GlobalSetup]
    public void Setup()
    {
        byte[] body = new byte[256];
        new Random(42).NextBytes(body);

        _message = new AmqpMessage
        {
            Durable = true,
            Priority = 4,
            MessageId = "order-7f3a9c2e",
            To = "orders",
            Subject = "order.created",
            ContentType = "application/json",
            CreationTime = _created,
            ApplicationProperties = new Dictionary<string, object?>
            {
                ["tenant"] = "contoso",
                ["attempt"] = 1,
                ["priority-lane"] = true,
            },
            DataSections = [body],
        };

        LiteApplicationProperties applicationProperties = new();
        applicationProperties["tenant"] = "contoso";
        applicationProperties["attempt"] = 1;
        applicationProperties["priority-lane"] = true;
        _liteMessage = new LiteMessage
        {
            Header = new LiteHeader { Durable = true, Priority = 4 },
            Properties = new LiteProperties
            {
                MessageId = "order-7f3a9c2e",
                To = "orders",
                Subject = "order.created",
                ContentType = "application/json",
                CreationTime = _created.UtcDateTime,
            },
            ApplicationProperties = applicationProperties,
            BodySection = new LiteData { Binary = body },
        };

        _encoded = AmqpMessageCodec.Encode(_message);
        LiteByteBuffer liteBuffer = _liteMessage.Encode();
        _liteEncoded = liteBuffer.Buffer.AsSpan(liteBuffer.Offset, liteBuffer.Length).ToArray();

        if (Decode().Subject != "order.created" || Decode_AmqpNetLite().Properties.Subject != "order.created")
        {
            throw new InvalidOperationException("The message did not round-trip.");
        }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(encodeCategory)]
    public LiteByteBuffer Encode_AmqpNetLite() => _liteMessage.Encode();

    [Benchmark]
    [BenchmarkCategory(encodeCategory)]
    public byte[] Encode() => AmqpMessageCodec.Encode(_message);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(decodeCategory)]
    public LiteMessage Decode_AmqpNetLite()
        => LiteMessage.Decode(new LiteByteBuffer(_liteEncoded, 0, _liteEncoded.Length, _liteEncoded.Length));

    [Benchmark]
    [BenchmarkCategory(decodeCategory)]
    public AmqpMessage Decode() => AmqpMessageCodec.Decode(_encoded);
}
