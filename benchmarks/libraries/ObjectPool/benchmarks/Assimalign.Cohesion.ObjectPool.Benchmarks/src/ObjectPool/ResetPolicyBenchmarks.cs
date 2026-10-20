using System;
using System.Text;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using CP = Assimalign.Cohesion.ObjectPool;
using MP = Microsoft.Extensions.ObjectPool;

namespace Assimalign.Cohesion.ObjectPool.Benchmarks.ObjectPool;

/// <summary>
/// Both policies clear StringBuilder state on successful return and reject capacity above 8192. Reset measures
/// reuse of a pre-sized buffer; Reject measures an oversized buffer being dropped and allocated again.
/// Append input is prepared in setup; growing the buffer is part of rejection cost. Both use identical policies.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ResetPolicyBenchmarks
{
    private CP.ObjectPool<StringBuilder> _cohesion = null!;
    private MP.ObjectPool<StringBuilder> _extensions = null!;
    private string _text = "";

    [Params(64, 4096)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _text = new string('x', Length);
        _cohesion = new CP.DefaultObjectPool<StringBuilder>(new Factory(), new CohesionPolicy(), 16);
        _extensions = new MP.DefaultObjectPool<StringBuilder>(new MicrosoftPolicy(), 16);
        if (Reset() != Length || Reset_MicrosoftExtensions() != Length
            || Reject() != Length || Reject_MicrosoftExtensions() != Length)
        {
            throw new InvalidOperationException("Pool reset workload differs.");
        }
        StringBuilder first = _cohesion.Rent();
        StringBuilder second = _extensions.Get();
        if (first.Length != 0 || second.Length != 0)
        {
            throw new InvalidOperationException("Policy did not clear state.");
        }
        _cohesion.Return(first);
        _extensions.Return(second);
    }

    [Benchmark, BenchmarkCategory("Reset")]
    public int Reset()
    {
        StringBuilder item = _cohesion.Rent();
        item.Append(_text);
        int length = item.Length;
        _cohesion.Return(item);
        return length;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Reset")]
    public int Reset_MicrosoftExtensions()
    {
        StringBuilder item = _extensions.Get();
        item.Append(_text);
        int length = item.Length;
        _extensions.Return(item);
        return length;
    }

    [Benchmark, BenchmarkCategory("Reject")]
    public int Reject()
    {
        StringBuilder item = _cohesion.Rent();
        item.Append(_text);
        item.EnsureCapacity(16384);
        int length = item.Length;
        _cohesion.Return(item);
        return length;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Reject")]
    public int Reject_MicrosoftExtensions()
    {
        StringBuilder item = _extensions.Get();
        item.Append(_text);
        item.EnsureCapacity(16384);
        int length = item.Length;
        _extensions.Return(item);
        return length;
    }

    private sealed class Factory : CP.ObjectPoolFactory<StringBuilder>
    {
        public override StringBuilder Create() => new(4096);
    }

    private sealed class CohesionPolicy : CP.ObjectPoolPolicy<StringBuilder>
    {
        public override bool CanReturn(StringBuilder instance)
        {
            if (instance.Capacity > 8192) return false;
            instance.Clear();
            return true;
        }
    }

    private sealed class MicrosoftPolicy : MP.PooledObjectPolicy<StringBuilder>
    {
        public override StringBuilder Create() => new(4096);
        public override bool Return(StringBuilder obj)
        {
            if (obj.Capacity > 8192) return false;
            obj.Clear();
            return true;
        }
    }
}
