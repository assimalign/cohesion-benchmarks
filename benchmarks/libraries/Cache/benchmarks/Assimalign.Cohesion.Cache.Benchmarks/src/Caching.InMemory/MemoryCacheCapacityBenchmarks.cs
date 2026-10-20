using System;

using Assimalign.Cohesion.Caching;

using BenchmarkDotNet.Attributes;

using CohesionCaching = Assimalign.Cohesion.Caching.InMemory;

namespace Assimalign.Cohesion.Cache.Benchmarks.Caching.InMemory;

/// <summary>
/// Commit one entry into a full size-limited cache, paying for synchronous priority/LRU capacity eviction.
/// No Microsoft baseline: its full-cache insertion can reject the new entry and schedule compaction in the
/// background, while Cohesion admits it and evicts synchronously. These are different completion guarantees.
/// Population and key allocation stay outside measurement; the cache is full before every invocation.
/// </summary>
[InvocationCount(1)]
public class MemoryCacheCapacityBenchmarks
{
    private readonly object _value = new();
    private CohesionCaching.MemoryCache _cache = null!;
    private object[] _keys = null!;

    [Params(32, 1024)]
    public int EntryCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _cache = new(new CohesionCaching.MemoryCacheOptions { SizeLimit = EntryCount, CompactionPercentage = 0.25 });
        _keys = new object[EntryCount];
        for (int i = 0; i < EntryCount; i++) _keys[i] = $"product:{i}";
    }

    [IterationSetup]
    public void Populate()
    {
        _cache.Clear();
        for (int i = 0; i < _keys.Length; i++)
        {
            using var entry = _cache.CreateEntry(_keys[i]);
            entry.Value = _value;
            entry.Size = 1;
            entry.Priority = i % 2 == 0 ? CacheEntryPriority.Low : CacheEntryPriority.Normal;
        }
    }

    [GlobalCleanup]
    public void Cleanup() => _cache.Dispose();

    [IterationCleanup]
    public void VerifyEviction()
    {
        if (_cache.Count >= EntryCount || !_cache.TryGetValue("product:new", out _))
            throw new InvalidOperationException("Capacity eviction did not admit the new entry and reclaim space.");
    }

    [Benchmark]
    public object InsertWithEviction()
    {
        using var entry = _cache.CreateEntry("product:new");
        entry.Value = _value;
        entry.Size = 1;
        entry.Priority = CacheEntryPriority.High;
        return _value;
    }
}
