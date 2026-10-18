using System;

using Assimalign.Cohesion.Caching;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Microsoft.Extensions.Caching.Memory;

using CohesionCaching = Assimalign.Cohesion.Caching.InMemory;
using ExtensionsCaching = Microsoft.Extensions.Caching.Memory;

namespace Assimalign.Cohesion.Cache.Benchmarks.Caching.InMemory;

/// <summary>
/// The two calls every cache-aside caller makes on the in-process cache: a read that hits, and a write that
/// replaces an existing entry with a time-to-live. Microsoft.Extensions.Caching.Memory is the baseline, so the
/// Ratio column reads "Cohesion relative to the counterpart".
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class MemoryCacheBenchmarks
{
    private const string Key = "catalog:product:42";
    private static readonly TimeSpan TimeToLive = TimeSpan.FromMinutes(5);

    private readonly object _value = new();
    private CohesionCaching.MemoryCache _cohesion = null!;
    private ExtensionsCaching.MemoryCache _extensions = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new CohesionCaching.MemoryCache();
        _extensions = new ExtensionsCaching.MemoryCache(new ExtensionsCaching.MemoryCacheOptions());

        _cohesion.Set(Key, _value, TimeToLive);
        _extensions.Set(Key, _value, TimeToLive);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _cohesion.Dispose();
        _extensions.Dispose();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Get")]
    public object? TryGetValue_MicrosoftExtensions()
    {
        _extensions.TryGetValue(Key, out object? value);
        return value;
    }

    [Benchmark]
    [BenchmarkCategory("Get")]
    public object? TryGetValue()
    {
        _cohesion.TryGetValue(Key, out object? value);
        return value;
    }

    /// <summary>Replaces the existing entry: allocates the entry, resolves the expiration, swaps the stored value.</summary>
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Set")]
    public object Set_MicrosoftExtensions() => _extensions.Set(Key, _value, TimeToLive);

    [Benchmark]
    [BenchmarkCategory("Set")]
    public object Set() => _cohesion.Set(Key, _value, TimeToLive);
}
