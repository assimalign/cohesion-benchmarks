using Assimalign.Cohesion.Caching;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Microsoft.Extensions.Caching.Memory;

using CohesionCache = Assimalign.Cohesion.Caching.InMemory.MemoryCache;
using ExtensionsCache = Microsoft.Extensions.Caching.Memory.MemoryCache;

namespace Assimalign.Cohesion.Cache.Benchmarks.Caching;

/// <summary>
/// GetOrCreate for a cold factory population. Both factories return the same prepared value, without
/// expiration or callback work. Cold keys are removed outside measurement each iteration; one invocation
/// keeps the cold case from silently turning into a hit in longer profiles.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[InvocationCount(1)]
public class GetOrCreateBenchmarks
{
    private const string ColdKey = "product:cold";
    private static readonly object Value = new();
    private ICache _cohesion = null!;
    private IMemoryCache _extensions = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new CohesionCache();
        _extensions = new ExtensionsCache(new MemoryCacheOptions());
    }

    [IterationSetup(Targets = new[] { nameof(Create), nameof(Create_MicrosoftExtensions) })]
    public void ResetColdKey()
    {
        _cohesion.Remove(ColdKey);
        _extensions.Remove(ColdKey);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _cohesion.Dispose();
        _extensions.Dispose();
    }

    [Benchmark, BenchmarkCategory("Create")]
    public object? Create() => _cohesion.GetOrCreate(ColdKey, static _ => Value);

    [Benchmark(Baseline = true), BenchmarkCategory("Create")]
    public object? Create_MicrosoftExtensions() => _extensions.GetOrCreate(ColdKey, static _ => Value);
}
