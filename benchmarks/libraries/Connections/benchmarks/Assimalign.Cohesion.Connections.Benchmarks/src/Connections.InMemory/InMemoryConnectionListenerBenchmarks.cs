using System.Threading.Tasks;

using Assimalign.Cohesion.Connections.InMemory;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Connections.Benchmarks.Connections.InMemory;

/// <summary>
/// Establishing and tearing down one connection through the listener/factory contracts: dial, accept (which
/// tracks the connection in the listener's live set), then dispose both ends (which untracks it). This is the
/// per-connection overhead a server's accept loop and a test host pay before any byte moves. There is no .NET
/// counterpart for the contract layer, so it tracks Cohesion against itself.
/// </summary>
public class InMemoryConnectionListenerBenchmarks
{
    private InMemoryConnectionListener _listener = null!;
    private InMemoryConnectionFactory _factory = null!;

    [GlobalSetup]
    public async Task Setup()
    {
        _listener = new InMemoryConnectionListener();
        await _listener.BindAsync();
        _factory = _listener.CreateFactory();
    }

    [GlobalCleanup]
    public ValueTask Cleanup() => _listener.DisposeAsync();

    [Benchmark]
    public async Task<ConnectionId> ConnectAccept()
    {
        IDuplexConnection client = await _factory.ConnectAsync(_listener.EndPoint);
        IDuplexConnection server = await _listener.AcceptAsync();

        await client.DisposeAsync();
        await server.DisposeAsync();

        return server.Id;
    }
}
