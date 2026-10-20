using System;
using System.Threading.Tasks;

using Assimalign.Cohesion.Connections.InMemory;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Connections.Benchmarks.Connections.StreamAbort;

/// <summary>
/// Failure-path reads after a directional abort: local STOP_SENDING rejects buffered input and
/// peer RESET_STREAM preserves its application error code. Abort and stream setup happen once
/// outside measurement; the live successful read/write guards are covered by multiplexed echo.
/// Raw pipe completion cannot provide the same directional/coded semantics, so no baseline is used.
/// </summary>
public class StreamAbortReadBenchmarks
{
    private MultiplexedConnection _client = null!;
    private MultiplexedConnection _server = null!;
    private MultiplexedConnectionStream _local = null!;
    private MultiplexedConnectionStream _localPeer = null!;
    private MultiplexedConnectionStream _sender = null!;
    private MultiplexedConnectionStream _receiver = null!;

    [GlobalSetup]
    public async Task Setup()
    {
        (_client, _server) = InMemoryMultiplexedConnectionPair.Create();
        _local = await _client.OpenStreamAsync();
        _localPeer = await _server.AcceptStreamAsync();
        _sender = await _client.OpenStreamAsync();
        _receiver = await _server.AcceptStreamAsync();

        // Buffered bytes make an incorrectly accepted read fail verification without waiting forever.
        await _localPeer.Output.WriteAsync(new byte[] { 1 });
        await _sender.Output.WriteAsync(new byte[] { 1 });
        (_local.DirectionalAbort ?? throw new InvalidOperationException("Missing abort feature.")).AbortRead(41);
        (_sender.DirectionalAbort ?? throw new InvalidOperationException("Missing abort feature.")).AbortWrite(42);
        if (!await ReadAfterLocalAbort() || await ReadAfterPeerReset() != 42)
            throw new InvalidOperationException("Directional abort did not preserve its guarantees.");
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _local.DisposeAsync();
        await _localPeer.DisposeAsync();
        await _sender.DisposeAsync();
        await _receiver.DisposeAsync();
        await _client.DisposeAsync();
        await _server.DisposeAsync();
    }

    [Benchmark]
    public async Task<bool> ReadAfterLocalAbort()
    {
        try
        {
            await _local.Input.ReadAsync();
        }
        catch (ConnectionAbortedException)
        {
            return true;
        }
        throw new InvalidOperationException("Local aborted read unexpectedly succeeded.");
    }

    [Benchmark]
    public async Task<long> ReadAfterPeerReset()
    {
        try
        {
            await _receiver.Input.ReadAsync();
        }
        catch (ConnectionResetException exception) when (exception.ApplicationErrorCode == 42)
        {
            return exception.ApplicationErrorCode.Value;
        }
        throw new InvalidOperationException("Peer reset read unexpectedly succeeded.");
    }
}
