using System;
using System.Buffers;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Core.Benchmarks.Core;

/// <summary>
/// Rent, fill and return a byte buffer through the same IMemoryOwner contract. Only the requested
/// slice is used: retention policies and excess capacity are intentionally outside this comparison.
/// </summary>
public class MemoryPoolBenchmarks
{
    private AdaptiveMemoryPool _pool = null!;

    [Params(256, 4096)] public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _pool = new AdaptiveMemoryPool(new AdaptiveMemoryPoolOptions { TrimInterval = TimeSpan.Zero });
        RentAndReturn();
        RentAndReturn_MemoryPool();
    }

    [GlobalCleanup] public void Cleanup() => _pool.Dispose();

    [Benchmark]
    public byte RentAndReturn()
    {
        using IMemoryOwner<byte> owner = _pool.Rent(Length);
        owner.Memory.Span[..Length].Fill(42);
        return owner.Memory.Span[Length - 1];
    }

    [Benchmark(Baseline = true)]
    public byte RentAndReturn_MemoryPool()
    {
        using IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(Length);
        owner.Memory.Span[..Length].Fill(42);
        return owner.Memory.Span[Length - 1];
    }
}
