using Assimalign.Cohesion.Caching;

using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.Caching.Memory;

using CohesionCache = Assimalign.Cohesion.Caching.InMemory.MemoryCache;
using ExtensionsCache = Microsoft.Extensions.Caching.Memory.MemoryCache;

namespace Assimalign.Cohesion.Cache.Benchmarks.Caching;

/// <summary>
/// GetOrCreate's correctly typed hit fast path: both caches return the prepared value without invoking
/// the factory. Kept separate from cold population so normal profiles can measure repeated hot reads.
/// </summary>
public class GetOrCreateHitBenchmarks
{
    private const string Key = "product:42";
    private static readonly object Value = new();
    private ICache _cohesion = null!;
    private IMemoryCache _extensions = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new CohesionCache();
        _extensions = new ExtensionsCache(new MemoryCacheOptions());
        _cohesion.Set(Key, Value);
        _extensions.Set(Key, Value);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _cohesion.Dispose();
        _extensions.Dispose();
    }

    [Benchmark]
    public object? Hit() => _cohesion.GetOrCreate(Key, static _ => Value);

    [Benchmark(Baseline = true)]
    public object? Hit_MicrosoftExtensions() => _extensions.GetOrCreate(Key, static _ => Value);
}
