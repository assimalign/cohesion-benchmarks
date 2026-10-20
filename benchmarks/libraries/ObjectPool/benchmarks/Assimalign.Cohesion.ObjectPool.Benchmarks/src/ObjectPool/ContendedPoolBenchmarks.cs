using System;
using System.Threading;

using BenchmarkDotNet.Attributes;

using CP = Assimalign.Cohesion.ObjectPool;
using MP = Microsoft.Extensions.ObjectPool;

namespace Assimalign.Cohesion.ObjectPool.Benchmarks.ObjectPool;

/// <summary>
/// Persistent dedicated workers share one pool and each perform 1024 rent/use/return cycles per invocation.
/// Thread creation is outside measurement; both sides include the same start/end barriers. Results are batch
/// latency (Workers * 1024 round trips), including synchronization. Both retain 16 objects with no reset policy.
/// </summary>
public class ContendedPoolBenchmarks
{
    private const int Rounds = 1024;
    private CP.ObjectPool<Item> _cohesion = null!;
    private MP.ObjectPool<Item> _extensions = null!;
    private Barrier _start = null!;
    private Barrier _finish = null!;
    private Thread[] _threads = [];
    private int[] _counts = [];
    private bool _stop;
    private bool _useCohesion;

    [Params(2, 4)]
    public int Workers { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new CP.DefaultObjectPool<Item>(new Factory(), 16);
        _extensions = new MP.DefaultObjectPool<Item>(new Policy(), 16);
        _stop = false;
        _start = new Barrier(Workers + 1);
        _finish = new Barrier(Workers + 1);
        _counts = new int[Workers];
        _threads = new Thread[Workers];
        for (int i = 0; i < Workers; i++)
        {
            int index = i;
            _threads[i] = new Thread(() => Work(index)) { IsBackground = true };
            _threads[i].Start();
        }
        if (RentReturn() != Workers * Rounds || RentReturn_MicrosoftExtensions() != Workers * Rounds)
        {
            throw new InvalidOperationException("Workers did not complete every round trip.");
        }
    }

    private void Work(int index)
    {
        while (true)
        {
            _start.SignalAndWait();
            if (_stop) return;
            int count = 0;
            for (int i = 0; i < Rounds; i++)
            {
                Item item = _useCohesion ? _cohesion.Rent() : _extensions.Get();
                item.Value = 1;
                count += item.Value;
                if (_useCohesion) _cohesion.Return(item);
                else _extensions.Return(item);
            }
            _counts[index] = count;
            _finish.SignalAndWait();
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _stop = true;
        _start.SignalAndWait();
        foreach (Thread thread in _threads) thread.Join();
        _start.Dispose();
        _finish.Dispose();
    }

    private int Run(bool cohesion)
    {
        _useCohesion = cohesion;
        _start.SignalAndWait();
        _finish.SignalAndWait();
        int count = 0;
        foreach (int value in _counts) count += value;
        return count;
    }

    [Benchmark]
    public int RentReturn() => Run(true);

    [Benchmark(Baseline = true)]
    public int RentReturn_MicrosoftExtensions() => Run(false);

    public sealed class Item
    {
        public int Value;
    }
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
