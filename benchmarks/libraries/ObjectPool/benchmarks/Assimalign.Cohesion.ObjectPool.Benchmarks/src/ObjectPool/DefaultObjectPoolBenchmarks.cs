using System.Text;

using BenchmarkDotNet.Attributes;

using CohesionPools = Assimalign.Cohesion.ObjectPool;
using ExtensionsPools = Microsoft.Extensions.ObjectPool;

namespace Assimalign.Cohesion.ObjectPool.Benchmarks.ObjectPool;

/// <summary>
/// Renting and returning from the default pool, against Microsoft.Extensions.ObjectPool (the baseline, so the
/// Ratio column reads "Cohesion relative to the counterpart").
/// </summary>
public class DefaultObjectPoolBenchmarks
{
    private CohesionPools.ObjectPool<StringBuilder> _cohesion = null!;
    private ExtensionsPools.ObjectPool<StringBuilder> _extensions = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = CohesionPools.ObjectPool<StringBuilder>.Create();
        _extensions = new ExtensionsPools.DefaultObjectPoolProvider().Create<StringBuilder>();
    }

    [Benchmark(Baseline = true)]
    public StringBuilder RentReturn_MicrosoftExtensions()
    {
        StringBuilder instance = _extensions.Get();
        _extensions.Return(instance);
        return instance;
    }

    [Benchmark]
    public StringBuilder RentReturn()
    {
        StringBuilder instance = _cohesion.Rent();
        _cohesion.Return(instance);
        return instance;
    }

    /// <summary>Two rentals outstanding at once: the second misses the pool's fast slot.</summary>
    [Benchmark]
    public StringBuilder RentTwoReturnTwo()
    {
        StringBuilder first = _cohesion.Rent();
        StringBuilder second = _cohesion.Rent();
        _cohesion.Return(second);
        _cohesion.Return(first);
        return first;
    }
}
