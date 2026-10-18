using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading.Tasks;

using Assimalign.Cohesion.Connections.InMemory;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Connections.Benchmarks.Connections.InMemory;

/// <summary>
/// One request/response round trip over a live connection pair: the client writes and flushes a payload, the
/// server reads it and echoes it back, the client reads the echo. This is the byte path every protocol above the
/// connection layer (HTTP/1.1, AMQP) drives, and the in-memory driver is the one the HTTP and Web test hosts run
/// on. The baseline is the same exchange over two raw <see cref="Pipe"/>s with the driver's pipe options, so the
/// Ratio column is the cost of the connection wrapper (state tracking, signaling reader/writer).
/// </summary>
public class InMemoryConnectionBenchmarks
{
    // The in-memory driver's own pipe options: non-pausing, no synchronization context.
    private static readonly PipeOptions _pipeOptions = new(
        pauseWriterThreshold: 0,
        resumeWriterThreshold: 0,
        useSynchronizationContext: false);

    private DuplexConnection _client = null!;
    private DuplexConnection _server = null!;
    private Pipe _clientToServer = null!;
    private Pipe _serverToClient = null!;
    private byte[] _payload = null!;

    /// <summary>A small control message, and a page-sized body.</summary>
    [Params(64, 4096)]
    public int PayloadSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _payload = new byte[PayloadSize];
        new Random(42).NextBytes(_payload);

        (_client, _server) = InMemoryConnectionPair.Create();
        _clientToServer = new Pipe(_pipeOptions);
        _serverToClient = new Pipe(_pipeOptions);
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _client.DisposeAsync();
        await _server.DisposeAsync();
    }

    [Benchmark(Baseline = true)]
    public Task<long> RoundTrip_SystemIOPipelines()
        => RoundTripAsync(_clientToServer.Writer, _clientToServer.Reader, _serverToClient.Writer, _serverToClient.Reader, _payload);

    [Benchmark]
    public Task<long> RoundTrip()
        => RoundTripAsync(_client.Output, _server.Input, _server.Output, _client.Input, _payload);

    private static async Task<long> RoundTripAsync(
        PipeWriter clientOutput,
        PipeReader serverInput,
        PipeWriter serverOutput,
        PipeReader clientInput,
        byte[] payload)
    {
        await clientOutput.WriteAsync(payload);
        await CopyAsync(serverInput, serverOutput, payload.Length);
        return await DrainAsync(clientInput, payload.Length);
    }

    // The server side: read exactly one message and echo it back in one flush.
    private static async Task CopyAsync(PipeReader input, PipeWriter output, int length)
    {
        int remaining = length;
        while (remaining > 0)
        {
            ReadResult result = await input.ReadAsync();
            ReadOnlySequence<byte> message = result.Buffer.Slice(0, Math.Min(result.Buffer.Length, remaining));
            foreach (ReadOnlyMemory<byte> segment in message)
            {
                output.Write(segment.Span);
            }

            remaining -= (int)message.Length;
            input.AdvanceTo(message.End);
        }

        await output.FlushAsync();
    }

    // The client side: consume the echo.
    private static async Task<long> DrainAsync(PipeReader input, int length)
    {
        long received = 0;
        while (received < length)
        {
            ReadResult result = await input.ReadAsync();
            ReadOnlySequence<byte> message = result.Buffer.Slice(0, Math.Min(result.Buffer.Length, length - received));
            received += message.Length;
            input.AdvanceTo(message.End);
        }

        return received;
    }
}
