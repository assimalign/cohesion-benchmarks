using Assimalign.Cohesion.Caching;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Microsoft.Extensions.Caching.Memory;

using CohesionCache = Assimalign.Cohesion.Caching.InMemory.MemoryCache;
using ExtensionsCache = Microsoft.Extensions.Caching.Memory.MemoryCache;

namespace Assimalign.Cohesion.Cache.Benchmarks.Caching;

/// <summary>
/// Public typed extensions over the cache interfaces, for a matching reference type or a missing key.
/// Both implementations return the same value/default. Incompatible cached types are excluded because
/// Microsoft's Get throws for those while Cohesion returns default, so that comparison would be misleading.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class TypedCacheBenchmarks
{
    private ICache _cohesion = null!;
    private IMemoryCache _extensions = null!;
    private string _key = null!;

    [Params(true, false)]
    public bool Hit { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new CohesionCache();
        _extensions = new ExtensionsCache(new MemoryCacheOptions());
        var value = new Product(42, "Benchmark product");
        _cohesion.Set("product:42", value);
        _extensions.Set("product:42", value);
        _key = Hit ? "product:42" : "product:missing";
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _cohesion.Dispose();
        _extensions.Dispose();
    }

    [Benchmark, BenchmarkCategory("GetTyped")]
    public Product? GetTyped() => _cohesion.Get<Product>(_key);

    [Benchmark(Baseline = true), BenchmarkCategory("GetTyped")]
    public Product? GetTyped_MicrosoftExtensions() => _extensions.Get<Product>(_key);

    [Benchmark, BenchmarkCategory("TryGetTyped")]
    public Product? TryGetTyped()
    {
        _cohesion.TryGetValue<Product>(_key, out var value);
        return value;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("TryGetTyped")]
    public Product? TryGetTyped_MicrosoftExtensions()
    {
        _extensions.TryGetValue<Product>(_key, out var value);
        return value;
    }

    public sealed record Product(int Id, string Name);
}
