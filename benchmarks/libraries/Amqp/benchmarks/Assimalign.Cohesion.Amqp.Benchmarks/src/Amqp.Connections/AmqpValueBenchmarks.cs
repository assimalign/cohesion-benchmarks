using System;
using System.Collections;
using System.Collections.Generic;

using Assimalign.Cohesion.Amqp.Connections;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using LiteByteBuffer = global::Amqp.ByteBuffer;
using LiteList = global::Amqp.Types.List;
using LiteMap = global::Amqp.Types.Map;
using LiteMessage = global::Amqp.Message;

namespace Assimalign.Cohesion.Amqp.Benchmarks.Amqp.Connections;

/// <summary>
/// Primitive type read/write and recursive list/map dispatch through a single public AMQP value section.
/// Cohesion's primitive codec is private, so both libraries pay the same minimal message envelope cost.
/// AMQPNetLite's lazy value must be accessed inside Decode to compare fully materialized values.
/// Numeric includes compact/full-width integers and floating point; TextBinary crosses 255-byte lengths;
/// Nested covers nulls and recursive maps/lists. Library-specific lists/maps represent the same AMQP types.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class AmqpValueBenchmarks
{
    private AmqpMessage _message = null!;
    private LiteMessage _liteMessage = null!;
    private byte[] _wire = null!;
    private object _expected = null!;

    [Params("Numeric", "TextBinary", "Nested")]
    public string Shape { get; set; } = "";

    [GlobalSetup]
    public void Setup()
    {
        object?[] numeric = [true, false, (byte)255, (sbyte)-10, (ushort)65535, (short)-1234,
            0u, 42u, 100000u, -100000, 0ul, 42ul, ulong.MaxValue, long.MinValue, 1.25f, 123.5d];
        byte[] binary = new byte[512];
        new Random(42).NextBytes(binary);
        object?[] text = ["orders", new string('x', 512), "注文 café", new byte[] { 0, 127, 255 }, binary];

        _expected = Shape switch
        {
            "Numeric" => numeric,
            "TextBinary" => text,
            "Nested" => new Dictionary<object, object?>
            {
                ["tenant"] = "contoso",
                ["values"] = new object?[] { null, numeric, text },
                ["options"] = new Dictionary<object, object?> { ["enabled"] = true, ["attempt"] = 1000 },
            },
            _ => throw new InvalidOperationException("Unknown value shape."),
        };
        _message = new AmqpMessage { Value = _expected };
        _liteMessage = new LiteMessage(ToLite(_expected));
        _wire = Encode();
        AssertEquivalent(_expected, Decode());
        AssertEquivalent(_expected, Decode_AmqpNetLite());
        LiteByteBuffer lite = Encode_AmqpNetLite();
        AssertEquivalent(_expected, AmqpMessageCodec.Decode(lite.Buffer.AsSpan(lite.Offset, lite.Length)).Value);
    }

    [Benchmark]
    [BenchmarkCategory("Encode")]
    public byte[] Encode() => AmqpMessageCodec.Encode(_message);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Encode")]
    public LiteByteBuffer Encode_AmqpNetLite() => _liteMessage.Encode();

    [Benchmark]
    [BenchmarkCategory("Decode")]
    public object? Decode() => AmqpMessageCodec.Decode(_wire).Value;

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Decode")]
    public object Decode_AmqpNetLite()
        => LiteMessage.Decode(new LiteByteBuffer(_wire, 0, _wire.Length, _wire.Length)).Body;

    private static object? ToLite(object? value)
    {
        if (value is object?[] array)
        {
            LiteList list = new();
            foreach (object? item in array)
            {
                list.Add(ToLite(item));
            }
            return list;
        }
        if (value is Dictionary<object, object?> dictionary)
        {
            LiteMap map = new();
            foreach (KeyValuePair<object, object?> entry in dictionary)
            {
                map[entry.Key] = ToLite(entry.Value);
            }
            return map;
        }
        return value;
    }

    private static void AssertEquivalent(object? expected, object? actual)
    {
        if (expected is byte[] bytes && actual is byte[] actualBytes && bytes.AsSpan().SequenceEqual(actualBytes))
        {
            return;
        }
        if (expected is object?[] list && actual is IList actualList && list.Length == actualList.Count)
        {
            for (int i = 0; i < list.Length; i++)
            {
                AssertEquivalent(list[i], actualList[i]);
            }
            return;
        }
        if (expected is IDictionary map && actual is IDictionary actualMap && map.Count == actualMap.Count)
        {
            foreach (DictionaryEntry entry in map)
            {
                if (!actualMap.Contains(entry.Key))
                {
                    throw new InvalidOperationException("A decoded map key is missing.");
                }
                AssertEquivalent(entry.Value, actualMap[entry.Key]);
            }
            return;
        }
        if (!Equals(expected, actual))
        {
            throw new InvalidOperationException($"Value mismatch: expected {expected} ({expected?.GetType()}), actual {actual} ({actual?.GetType()}).");
        }
    }
}
