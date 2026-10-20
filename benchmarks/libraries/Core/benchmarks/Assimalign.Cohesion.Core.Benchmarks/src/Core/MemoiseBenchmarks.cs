using System;
using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Core.Benchmarks.Core;

/// <summary>
/// Repeated cached transformations over a warmed, bounded key set. The baseline uses the same
/// TryGetValue hit path; GetOrAdd's atomic factory guarantees would not be an honest miss comparison.
/// </summary>
public class MemoiseBenchmarks
{
    private Func<int, string> _memo = null!;
    private ConcurrentDictionary<int, string> _cache = null!;

    [GlobalSetup]
    public void Setup()
    {
        _memo = Memoise<int, string>.Invoke(static key => $"route/{key}");
        _cache = new ConcurrentDictionary<int, string>();
        for (int i = 0; i < 64; i++) _cache[i] = _memo(i);
    }

    [Benchmark]
    public int CachedTransform()
    {
        int length = 0;
        for (int i = 0; i < 64; i++) length += _memo(i).Length;
        return length;
    }

    [Benchmark(Baseline = true)]
    public int CachedTransform_ConcurrentDictionary()
    {
        int length = 0;
        for (int i = 0; i < 64; i++)
            if (_cache.TryGetValue(i, out string? value)) length += value.Length;
        return length;
    }
}
