using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using CP = Assimalign.Cohesion.ObjectPool;
using MP = Microsoft.Extensions.ObjectPool;

namespace Assimalign.Cohesion.ObjectPool.Benchmarks.ObjectPool;

/// <summary>
/// Same object factory, no-op return policy and explicit retention on both sides. RoundTrip uses a warm fast
/// slot; RetainedBurst drains the retained slots; Exhaustion rents 32 objects before returning any, exceeding
/// either retention limit. Buffers holding outstanding rentals are allocated in setup. Every invocation restores
/// the pool to a full state, so repeated measurements exercise the same retained/allocation split.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class PoolWorkloadBenchmarks
{
    private CP.ObjectPool<Item> _cohesion = null!;
    private MP.ObjectPool<Item> _extensions = null!;
    private readonly Item[] _items = new Item[32];

    [Params(1, 16)]
    public int Retained { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new CP.DefaultObjectPool<Item>(new Factory(), Retained);
        _extensions = new MP.DefaultObjectPool<Item>(new Policy(), Retained);
        if (Exhaustion() != 32 || Exhaustion_MicrosoftExtensions() != 32)
        {
            throw new InvalidOperationException("Pool burst failed.");
        }
    }

    [Benchmark, BenchmarkCategory("RoundTrip")]
    public Item RoundTrip()
    {
        Item item = _cohesion.Rent();
        _cohesion.Return(item);
        return item;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("RoundTrip")]
    public Item RoundTrip_MicrosoftExtensions()
    {
        Item item = _extensions.Get();
        _extensions.Return(item);
        return item;
    }

    [Benchmark, BenchmarkCategory("RetainedBurst")]
    public int RetainedBurst() => Burst(Retained);

    [Benchmark(Baseline = true), BenchmarkCategory("RetainedBurst")]
    public int RetainedBurst_MicrosoftExtensions() => BurstMicrosoft(Retained);

    [Benchmark, BenchmarkCategory("Exhaustion")]
    public int Exhaustion() => Burst(_items.Length);

    [Benchmark(Baseline = true), BenchmarkCategory("Exhaustion")]
    public int Exhaustion_MicrosoftExtensions() => BurstMicrosoft(_items.Length);

    private int Burst(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _items[i] = _cohesion.Rent();
        }
        for (int i = 0; i < count; i++)
        {
            _cohesion.Return(_items[i]);
            _items[i] = null!;
        }
        return count;
    }

    private int BurstMicrosoft(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _items[i] = _extensions.Get();
        }
        for (int i = 0; i < count; i++)
        {
            _extensions.Return(_items[i]);
            _items[i] = null!;
        }
        return count;
    }

    public sealed class Item;
    private sealed class Factory : CP.ObjectPoolFactory<Item>
    {
        public override Item Create() => new();
    }
    private sealed class Policy : MP.PooledObjectPolicy<Item>
    {
        public override Item Create() => new();
        public override bool Return(Item obj) => true;
    }
}
