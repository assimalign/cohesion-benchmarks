using System;

using Assimalign.Cohesion.Caching;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Microsoft.Extensions.Caching.Memory;

using CohesionCaching = Assimalign.Cohesion.Caching.InMemory;
using ExtensionsCaching = Microsoft.Extensions.Caching.Memory;

namespace Assimalign.Cohesion.Cache.Benchmarks.Caching.InMemory;

/// <summary>
/// Untyped miss, sliding-expiration hit, and replacing entries without expiration or with size accounting.
/// Each paired operation uses identical metadata and returns the same prepared object. Sized writes stay
/// below the capacity limit so eviction policy differences cannot affect this comparison.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class MemoryCacheAccessBenchmarks
{
    private static readonly object Value = new();
    private CohesionCaching.MemoryCache _cohesion = null!;
    private ExtensionsCaching.MemoryCache _extensions = null!;
    private CohesionCaching.MemoryCache _cohesionSized = null!;
    private ExtensionsCaching.MemoryCache _extensionsSized = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new();
        _extensions = new(new ExtensionsCaching.MemoryCacheOptions());
        _cohesionSized = new(new CohesionCaching.MemoryCacheOptions { SizeLimit = 1024 });
        _extensionsSized = new(new ExtensionsCaching.MemoryCacheOptions { SizeLimit = 1024 });
        using (var entry = _cohesion.CreateEntry("sliding"))
        {
            entry.Value = Value;
            entry.SlidingExpiration = TimeSpan.FromDays(1);
        }
        _extensions.Set("sliding", Value, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(1) });
        _cohesion.Set("replace", Value);
        _extensions.Set("replace", Value);
        SetSized();
        SetSized_MicrosoftExtensions();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _cohesion.Dispose();
        _extensions.Dispose();
        _cohesionSized.Dispose();
        _extensionsSized.Dispose();
    }

    [Benchmark, BenchmarkCategory("Miss")]
    public bool Miss() => _cohesion.TryGetValue("missing", out _);

    [Benchmark(Baseline = true), BenchmarkCategory("Miss")]
    public bool Miss_MicrosoftExtensions() => _extensions.TryGetValue("missing", out _);

    [Benchmark, BenchmarkCategory("SlidingHit")]
    public object? SlidingHit()
    {
        _cohesion.TryGetValue("sliding", out var value);
        return value;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("SlidingHit")]
    public object? SlidingHit_MicrosoftExtensions()
    {
        _extensions.TryGetValue("sliding", out var value);
        return value;
    }

    [Benchmark, BenchmarkCategory("SetWithoutExpiration")]
    public object SetWithoutExpiration() => _cohesion.Set("replace", Value);

    [Benchmark(Baseline = true), BenchmarkCategory("SetWithoutExpiration")]
    public object SetWithoutExpiration_MicrosoftExtensions() => _extensions.Set("replace", Value);

    [Benchmark, BenchmarkCategory("SetSized")]
    public object SetSized()
    {
        using var entry = _cohesionSized.CreateEntry("sized");
        entry.Value = Value;
        entry.Size = 1;
        return Value;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("SetSized")]
    public object SetSized_MicrosoftExtensions()
    {
        using var entry = _extensionsSized.CreateEntry("sized");
        entry.Value = Value;
        entry.Size = 1;
        return Value;
    }
}
