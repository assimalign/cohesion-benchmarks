using System;
using System.Collections.Generic;
using System.Text;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Http.Benchmarks.Http.Sessions;

/// <summary>
/// Encoding the session dictionary for a store, and decoding it when a request loads the session.
/// ASP.NET Core's session serializer is internal and its framing differs; a general serializer
/// would produce a different wire contract, so these track Cohesion alone.
/// </summary>
public class HttpSessionSerializerBenchmarks
{
    private Dictionary<string, byte[]> _values = null!;
    private byte[] _frame = null!;

    [Params(4, 32)]
    public int Entries { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _values = new Dictionary<string, byte[]>(StringComparer.Ordinal);
        for (int i = 0; i < Entries; i++)
        {
            _values.Add($"session-key-{i}", Encoding.UTF8.GetBytes(new string((char)('a' + i % 26), 128)));
        }
        _frame = Serialize();
        Dictionary<string, byte[]> decoded = Deserialize();
        foreach (KeyValuePair<string, byte[]> pair in _values)
        {
            if (!decoded[pair.Key].AsSpan().SequenceEqual(pair.Value))
            {
                throw new InvalidOperationException("Session values did not round-trip.");
            }
        }
    }

    [Benchmark]
    public byte[] Serialize() => HttpSessionSerializer.Serialize(_values);

    [Benchmark]
    public Dictionary<string, byte[]> Deserialize()
        => HttpSessionSerializer.TryDeserialize(_frame, out Dictionary<string, byte[]>? values)
            ? values : throw new InvalidOperationException("Invalid session frame.");
}
