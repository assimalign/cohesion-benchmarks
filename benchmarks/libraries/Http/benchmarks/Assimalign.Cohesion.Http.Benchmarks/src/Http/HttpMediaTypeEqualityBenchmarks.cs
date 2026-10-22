using System;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Http.Benchmarks.Http;

/// <summary>Ordinary unique-name equality controls for the repeated-name correctness fix.</summary>
public class HttpMediaTypeEqualityBenchmarks
{
    private HttpMediaType _left;
    private HttpMediaType _right;

    [Params(1, 2, 65, 4097)]
    public int Parameters { get; set; }

    [Params(false, true)]
    public bool Reversed { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var entries = new string[Parameters];
        for (int i = 0; i < entries.Length; i++)
        {
            entries[i] = $"p{i}=v{i}";
        }
        _left = HttpMediaType.Parse("text/plain; " + string.Join(';', entries));
        if (Reversed)
        {
            Array.Reverse(entries);
        }
        _right = HttpMediaType.Parse("text/plain; " + string.Join(';', entries));
        if (!EqualsMediaTypes())
        {
            throw new InvalidOperationException("Unique-name parameter equality changed.");
        }
    }

    [Benchmark]
    public bool EqualsMediaTypes() => _left.Equals(_right);
}
