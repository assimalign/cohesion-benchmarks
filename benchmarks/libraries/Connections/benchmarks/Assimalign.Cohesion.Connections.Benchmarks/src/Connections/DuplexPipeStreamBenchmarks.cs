using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Assimalign.Cohesion.Connections.Benchmarks.Connections;

/// <summary>
/// Repeated stream reads and writes through the public duplex adapter used by stream-based layers.
/// BCL PipeReader.AsStream and PipeWriter.AsStream perform the same copy and flush over identical
/// non-pausing pipes. Feeder writes and receiver copies are included equally on both sides. The
/// larger payload spans pipe segments; each read copies into a destination large enough for the message.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class DuplexPipeStreamBenchmarks
{
    private Pipe _input = null!;
    private Pipe _output = null!;
    private Stream _duplex = null!;
    private Stream _reader = null!;
    private Stream _writer = null!;
    private byte[] _payload = null!;
    private byte[] _received = null!;

    [Params(64, 16384)]
    public int PayloadSize { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        var options = new PipeOptions(pauseWriterThreshold: 0, resumeWriterThreshold: 0, useSynchronizationContext: false);
        _input = new Pipe(options);
        _output = new Pipe(options);
        _duplex = new DuplexPipeStream(_input.Reader, _output.Writer);
        _reader = _input.Reader.AsStream(leaveOpen: true);
        _writer = _output.Writer.AsStream(leaveOpen: true);
        _payload = new byte[PayloadSize];
        new Random(42).NextBytes(_payload);
        _received = new byte[PayloadSize];

        Validate(await Read());
        Validate(await Read_SystemIOPipelines());
        Validate(await Write());
        Validate(await Write_SystemIOPipelines());
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _duplex.Dispose();
        _reader.Dispose();
        _writer.Dispose();
        _input.Writer.Complete();
        _input.Reader.Complete();
        _output.Writer.Complete();
        _output.Reader.Complete();
    }

    [Benchmark, BenchmarkCategory("Read")]
    public Task<int> Read() => ReadAsync(_duplex);

    [Benchmark(Baseline = true), BenchmarkCategory("Read")]
    public Task<int> Read_SystemIOPipelines() => ReadAsync(_reader);

    [Benchmark, BenchmarkCategory("Write")]
    public Task<int> Write() => WriteAsync(_duplex);

    [Benchmark(Baseline = true), BenchmarkCategory("Write")]
    public Task<int> Write_SystemIOPipelines() => WriteAsync(_writer);

    private async Task<int> ReadAsync(Stream stream)
    {
        for (int offset = 0; offset < _payload.Length; offset += 4096)
        {
            _input.Writer.Write(_payload.AsSpan(offset, Math.Min(4096, _payload.Length - offset)));
        }
        await _input.Writer.FlushAsync();
        int received = 0;
        while (received < _received.Length)
        {
            int count = await stream.ReadAsync(_received.AsMemory(received));
            if (count == 0) throw new InvalidOperationException("Unexpected end of message.");
            received += count;
        }
        return received;
    }

    private async Task<int> WriteAsync(Stream stream)
    {
        await stream.WriteAsync(_payload.AsMemory());
        ReadResult result = await _output.Reader.ReadAsync();
        int length = checked((int)result.Buffer.Length);
        result.Buffer.CopyTo(_received);
        _output.Reader.AdvanceTo(result.Buffer.End);
        return length;
    }

    private void Validate(int length)
    {
        if (length != PayloadSize || !_payload.AsSpan().SequenceEqual(_received))
            throw new InvalidOperationException("Stream adapter changed the payload.");
    }
}
