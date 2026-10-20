using System;
using System.Threading.Tasks;

using Assimalign.Cohesion.Connections.InMemory;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Connections.Benchmarks.Connections.StreamLimits;

/// <summary>
/// Protocol-side stream ordinal checks including optional-feature discovery on each operation.
/// InMemory does not enforce a transport credit limit: the fixture registers only a fixed allowance
/// via the public contract. This measures the StreamLimits extension and consumer check, not QUIC
/// flow control. Pipelines has no cumulative peer stream allowance to compare fairly.
/// </summary>
public class StreamLimitLookupBenchmarks
{
    private IMultiplexedConnection _connection = null!;
    private MultiplexedConnection _peer = null!;
    private long[] _ordinals = null!;

    [Params(false, true)]
    public bool FeaturePresent { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        (MultiplexedConnection client, _peer) = InMemoryMultiplexedConnectionPair.Create();
        _connection = client;
        if (FeaturePresent) _connection.Features.Set(new FixedAllowance());
        _ordinals = new long[32];
        for (int i = 0; i < _ordinals.Length; i++) _ordinals[i] = i;
        if (CheckStreamOrdinals() != (FeaturePresent ? 16 : 32))
            throw new InvalidOperationException("Unexpected stream allowance decision.");
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _connection.DisposeAsync();
        await _peer.DisposeAsync();
    }

    [Benchmark(OperationsPerInvoke = 32)]
    public int CheckStreamOrdinals()
    {
        int accepted = 0;
        foreach (long ordinal in _ordinals)
        {
            IMultiplexedConnectionStreamLimits? limits = _connection.StreamLimits;
            if (limits is null || ordinal < limits.InboundBidirectionalStreamLimit) accepted++;
        }
        return accepted;
    }

    private sealed class FixedAllowance : IMultiplexedConnectionStreamLimits
    {
        public string Name => IMultiplexedConnectionStreamLimits.FeatureName;
        public long InboundBidirectionalStreamLimit => 16;
    }
}
