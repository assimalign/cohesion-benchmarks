using System;

using Assimalign.Cohesion.Caching;

using BenchmarkDotNet.Attributes;

using CohesionCaching = Assimalign.Cohesion.Caching.InMemory;

namespace Assimalign.Cohesion.Cache.Benchmarks.Caching.InMemory;

/// <summary>
/// Expiration scans over a live or wholly expired working set via public Compact. A manually advanced
/// clock makes expiration deterministic without sleeping. No Microsoft baseline: its Compact also builds
/// priority/LRU eviction buckets; Cohesion's Compact only scans expiration, a different workload.
/// Refill and time advancement happen outside the single measured invocation.
/// </summary>
[InvocationCount(1)]
public class MemoryCacheExpirationBenchmarks
{
    private readonly ManualTimeProvider _clock = new();
    private readonly object _value = new();
    private CohesionCaching.MemoryCache _cache = null!;
    private object[] _keys = null!;
    private int _expectedCount;

    [Params(32, 1024)]
    public int EntryCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _cache = new(new CohesionCaching.MemoryCacheOptions { TimeProvider = _clock });
        _keys = new object[EntryCount];
        for (int i = 0; i < EntryCount; i++) _keys[i] = $"product:{i}";
    }

    [IterationSetup(Target = nameof(ScanLive))]
    public void PopulateLive() => Populate(false);

    [IterationSetup(Target = nameof(ScanExpired))]
    public void PopulateExpired() => Populate(true);

    private void Populate(bool expired)
    {
        _cache.Clear();
        _clock.UtcNow = DateTimeOffset.UnixEpoch;
        foreach (object key in _keys) _cache.Set(key, _value, TimeSpan.FromMinutes(1));
        if (expired) _clock.UtcNow += TimeSpan.FromMinutes(2);
        _expectedCount = expired ? 0 : EntryCount;
    }

    [IterationCleanup]
    public void VerifyScan()
    {
        if (_cache.Count != _expectedCount) throw new InvalidOperationException("Expiration scan left an unexpected entry count.");
    }

    [GlobalCleanup]
    public void Cleanup() => _cache.Dispose();

    [Benchmark]
    public object ScanLive()
    {
        _cache.Compact();
        return _cache;
    }

    [Benchmark]
    public object ScanExpired()
    {
        _cache.Compact();
        return _cache;
    }

    private sealed class ManualTimeProvider : TimeProvider
    {
        public DateTimeOffset UtcNow { get; set; } = DateTimeOffset.UnixEpoch;
        public override DateTimeOffset GetUtcNow() => UtcNow;
    }
}
