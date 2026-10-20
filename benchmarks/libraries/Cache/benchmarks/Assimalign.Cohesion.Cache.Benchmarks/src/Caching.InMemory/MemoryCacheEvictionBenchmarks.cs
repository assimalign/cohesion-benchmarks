using System;

using Assimalign.Cohesion.Caching;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Microsoft.Extensions.Caching.Memory;

using CohesionCache = Assimalign.Cohesion.Caching.InMemory.MemoryCache;
using ExtensionsCache = Microsoft.Extensions.Caching.Memory.MemoryCache;

namespace Assimalign.Cohesion.Cache.Benchmarks.Caching.InMemory;

/// <summary>
/// Explicitly evict a populated working set, one key at a time or via Clear. Both caches hold the same
/// keys and values without callbacks. Each measurement is a whole EntryCount-entry batch. Iteration setup
/// repopulates the caches outside measurement; single invocation prevents repeated empty-cache measurements.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[InvocationCount(1)]
public class MemoryCacheEvictionBenchmarks
{
    private readonly object _value = new();
    private object[] _keys = null!;
    private CohesionCache _cohesion = null!;
    private ExtensionsCache _extensions = null!;

    [Params(32, 1024)]
    public int EntryCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new();
        _extensions = new(new MemoryCacheOptions());
        _keys = new object[EntryCount];
        for (int i = 0; i < EntryCount; i++) _keys[i] = $"product:{i}";
    }

    [IterationSetup]
    public void Populate()
    {
        _cohesion.Clear();
        _extensions.Clear();
        foreach (object key in _keys)
        {
            _cohesion.Set(key, _value);
            _extensions.Set(key, _value);
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _cohesion.Dispose();
        _extensions.Dispose();
    }

    [IterationCleanup(Targets = new[] { nameof(Remove), nameof(Clear) })]
    public void VerifyCohesionEvicted()
    {
        if (_cohesion.Count != 0) throw new InvalidOperationException("Cohesion entries were not evicted.");
    }

    [IterationCleanup(Targets = new[] { nameof(Remove_MicrosoftExtensions), nameof(Clear_MicrosoftExtensions) })]
    public void VerifyExtensionsEvicted()
    {
        if (_extensions.Count != 0) throw new InvalidOperationException("Microsoft entries were not evicted.");
    }

    [Benchmark, BenchmarkCategory("Remove")]
    public object Remove()
    {
        foreach (object key in _keys) _cohesion.Remove(key);
        return _cohesion;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Remove")]
    public object Remove_MicrosoftExtensions()
    {
        foreach (object key in _keys) _extensions.Remove(key);
        return _extensions;
    }

    [Benchmark, BenchmarkCategory("Clear")]
    public object Clear()
    {
        _cohesion.Clear();
        return _cohesion;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Clear")]
    public object Clear_MicrosoftExtensions()
    {
        _extensions.Clear();
        return _extensions;
    }
}
