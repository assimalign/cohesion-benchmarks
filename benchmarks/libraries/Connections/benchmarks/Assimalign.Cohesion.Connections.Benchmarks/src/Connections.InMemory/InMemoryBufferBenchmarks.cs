using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading.Tasks;

using Assimalign.Cohesion.Connections.InMemory;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Assimalign.Cohesion.Connections.Benchmarks.Connections.InMemory;

/// <summary>
/// Buffer acquisition/advance, a single batched flush, and segmented consumption on a live byte channel.
/// Both implementations write the same 4 KiB chunks and copy every received byte. Async read and
/// TryRead are separate groups; the raw pipes use the transport's options and measure the same live
/// data path, without claiming to reproduce Cohesion's connection lifecycle guarantees.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class InMemoryBufferBenchmarks
{
    private DuplexConnection _client = null!;
    private DuplexConnection _server = null!;
    private Pipe _pipe = null!;
    private byte[] _payload = null!;
    private byte[] _received = null!;

    [Params(64, 16384)]
    public int PayloadSize { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        (_client, _server) = InMemoryConnectionPair.Create();
        _pipe = new Pipe(new PipeOptions(pauseWriterThreshold: 0, resumeWriterThreshold: 0, useSynchronizationContext: false));
        _payload = new byte[PayloadSize];
        new Random(42).NextBytes(_payload);
        _received = new byte[PayloadSize];
        await ValidateBufferShapeAsync(_client.Output, _server.Input);
        await ValidateBufferShapeAsync(_pipe.Writer, _pipe.Reader);
        Validate(await Transfer());
        Validate(await Transfer_SystemIOPipelines());
        Validate(await TryReadTransfer());
        Validate(await TryReadTransfer_SystemIOPipelines());
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _client.DisposeAsync();
        await _server.DisposeAsync();
        _pipe.Writer.Complete();
        _pipe.Reader.Complete();
    }

    [Benchmark, BenchmarkCategory("ReadAsync")]
    public Task<int> Transfer() => TransferAsync(_client.Output, _server.Input, false);

    [Benchmark(Baseline = true), BenchmarkCategory("ReadAsync")]
    public Task<int> Transfer_SystemIOPipelines() => TransferAsync(_pipe.Writer, _pipe.Reader, false);

    [Benchmark, BenchmarkCategory("TryRead")]
    public Task<int> TryReadTransfer() => TransferAsync(_client.Output, _server.Input, true);

    [Benchmark(Baseline = true), BenchmarkCategory("TryRead")]
    public Task<int> TryReadTransfer_SystemIOPipelines() => TransferAsync(_pipe.Writer, _pipe.Reader, true);

    private async Task<int> TransferAsync(PipeWriter writer, PipeReader reader, bool tryRead)
    {
        Fill(writer);
        await writer.FlushAsync();

        ReadResult result;
        if (tryRead)
        {
            if (!reader.TryRead(out result)) throw new InvalidOperationException("Flushed bytes were unavailable.");
        }
        else
        {
            result = await reader.ReadAsync();
        }

        int copied = 0;
        foreach (ReadOnlyMemory<byte> segment in result.Buffer)
        {
            segment.CopyTo(_received.AsMemory(copied));
            copied += segment.Length;
        }
        reader.AdvanceTo(result.Buffer.End);
        return copied;
    }

    private void Fill(PipeWriter writer)
    {
        for (int offset = 0; offset < _payload.Length; offset += 4096)
        {
            int count = Math.Min(4096, _payload.Length - offset);
            _payload.AsMemory(offset, count).CopyTo(writer.GetMemory(count));
            writer.Advance(count);
        }
    }

    private async Task ValidateBufferShapeAsync(PipeWriter writer, PipeReader reader)
    {
        Fill(writer);
        await writer.FlushAsync();
        ReadResult result = await reader.ReadAsync();
        bool segmented = !result.Buffer.IsSingleSegment;
        reader.AdvanceTo(result.Buffer.End);
        if (PayloadSize > 4096 && !segmented)
            throw new InvalidOperationException("Large-buffer fixture must exercise multiple pipe segments.");
    }

    private void Validate(int length)
    {
        if (length != PayloadSize || !_payload.AsSpan().SequenceEqual(_received))
            throw new InvalidOperationException("Buffer transfer changed the payload.");
    }
}
