using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading.Tasks;

using Assimalign.Cohesion.Connections.InMemory;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Connections.Benchmarks.Connections.InMemory;

/// <summary>
/// Steady-state request/echo on an already opened multiplexed stream. This exercises the read-side
/// abort/loss guards as well as writer guards, unlike the single-stream transport's raw reader.
/// The baseline exchanges identical bytes over non-pausing pipes; stream creation and feature
/// discovery are outside both measurements. Only successful I/O is compared, not abort semantics.
/// </summary>
public class InMemoryMultiplexedStreamBenchmarks
{
    private MultiplexedConnection _client = null!;
    private MultiplexedConnection _server = null!;
    private MultiplexedConnectionStream _outbound = null!;
    private MultiplexedConnectionStream _inbound = null!;
    private Pipe _request = null!;
    private Pipe _response = null!;
    private byte[] _payload = null!;
    private byte[] _received = null!;

    [Params(64, 4096)]
    public int PayloadSize { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        (_client, _server) = InMemoryMultiplexedConnectionPair.Create();
        _outbound = await _client.OpenStreamAsync();
        _inbound = await _server.AcceptStreamAsync();
        var options = new PipeOptions(pauseWriterThreshold: 0, resumeWriterThreshold: 0, useSynchronizationContext: false);
        _request = new Pipe(options);
        _response = new Pipe(options);
        _payload = new byte[PayloadSize];
        new Random(42).NextBytes(_payload);
        _received = new byte[PayloadSize];
        Validate(await RoundTrip());
        Validate(await RoundTrip_SystemIOPipelines());
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _outbound.DisposeAsync();
        await _inbound.DisposeAsync();
        await _client.DisposeAsync();
        await _server.DisposeAsync();
        _request.Writer.Complete();
        _request.Reader.Complete();
        _response.Writer.Complete();
        _response.Reader.Complete();
    }

    [Benchmark]
    public Task<int> RoundTrip() => ExchangeAsync(_outbound.Output, _inbound.Input, _inbound.Output, _outbound.Input);

    [Benchmark(Baseline = true)]
    public Task<int> RoundTrip_SystemIOPipelines() => ExchangeAsync(_request.Writer, _request.Reader, _response.Writer, _response.Reader);

    private async Task<int> ExchangeAsync(PipeWriter request, PipeReader server, PipeWriter response, PipeReader client)
    {
        await request.WriteAsync(_payload);
        ReadResult input = await server.ReadAsync();
        foreach (ReadOnlyMemory<byte> segment in input.Buffer) response.Write(segment.Span);
        server.AdvanceTo(input.Buffer.End);
        await response.FlushAsync();
        ReadResult echo = await client.ReadAsync();
        int length = checked((int)echo.Buffer.Length);
        echo.Buffer.CopyTo(_received);
        client.AdvanceTo(echo.Buffer.End);
        return length;
    }

    private void Validate(int length)
    {
        if (length != PayloadSize || !_payload.AsSpan().SequenceEqual(_received))
            throw new InvalidOperationException("Multiplexed echo changed the payload.");
    }
}
