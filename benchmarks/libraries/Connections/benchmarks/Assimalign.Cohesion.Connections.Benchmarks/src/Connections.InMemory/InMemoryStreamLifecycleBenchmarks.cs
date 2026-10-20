using System.Threading.Tasks;

using Assimalign.Cohesion.Connections.InMemory;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Connections.Benchmarks.Connections.InMemory;

/// <summary>
/// Per-request stream open, accept, and disposal over an established multiplexed connection: live-set
/// tracking, direction selection and channel dispatch. Raw pipelines have no stream routing or
/// multiplexed lifecycle contract, so there is no equivalent counterpart for this operation.
/// </summary>
public class InMemoryStreamLifecycleBenchmarks
{
    private MultiplexedConnection _client = null!;
    private MultiplexedConnection _server = null!;

    [Params(ConnectionDirection.Bidirectional, ConnectionDirection.WriteOnly)]
    public ConnectionDirection Direction { get; set; }

    [GlobalSetup]
    public void Setup() => (_client, _server) = InMemoryMultiplexedConnectionPair.Create();

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _client.DisposeAsync();
        await _server.DisposeAsync();
    }

    [Benchmark]
    public async Task<ConnectionDirection> OpenAccept()
    {
        MultiplexedConnectionStream outbound = await _client.OpenStreamAsync(Direction);
        MultiplexedConnectionStream inbound = await _server.AcceptStreamAsync();
        ConnectionDirection direction = inbound.Direction;
        await outbound.DisposeAsync();
        await inbound.DisposeAsync();
        return direction;
    }
}
