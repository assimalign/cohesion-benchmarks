using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.OpenTelemetry.Benchmarks.OpenTelemetry;

/// <summary>
/// A 128-record application burst drained in one or several HTTP requests, the same burst overflowing
/// a 64-record drop-oldest queue. Separate exporters keep state independent.
/// Background flushes are delayed one day. No SDK pairing: queue loss policies and
/// wire encodings differ, and Cohesion has no activity/metric recorder to compare with the SDK.
/// </summary>
public class OtlpQueueBenchmarks
{
    private IOtlpLogExporter _normal = null!;
    private IOtlpLogExporter _overflow = null!;
    private OtlpLogRecord[] _records = null!;

    [Params(16, 128)]
    public int MaxBatchSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _records = ExporterFixture.Records(128);
        var normalHandler = new CollectorHandler();
        var overflowHandler = new CollectorHandler();
        _normal = ExporterFixture.Create(normalHandler, queueLength: 256, batchSize: MaxBatchSize);
        _overflow = ExporterFixture.Create(overflowHandler, queueLength: 64, batchSize: MaxBatchSize);
        EnqueueThenDrain().GetAwaiter().GetResult();
        OverflowThenDrain().GetAwaiter().GetResult();
        if (_normal.DroppedCount != 0 || _overflow.DroppedCount != 64 ||
            normalHandler.Requests != 128 / MaxBatchSize ||
            overflowHandler.Requests != 64 / Math.Min(64, MaxBatchSize) ||
            _normal.FailedExportCount != 0 || _overflow.FailedExportCount != 0)
        {
            throw new InvalidOperationException("Queue loss or drain batching was unexpected.");
        }
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _normal.DisposeAsync().ConfigureAwait(false);
        await _overflow.DisposeAsync().ConfigureAwait(false);
    }

    [Benchmark]
    public async Task<long> EnqueueThenDrain()
    {
        foreach (OtlpLogRecord record in _records)
        {
            _normal.TryEnqueue(record);
        }

        await _normal.FlushAsync().ConfigureAwait(false);
        return _normal.FailedExportCount;
    }

    [Benchmark]
    public async Task<long> OverflowThenDrain()
    {
        foreach (OtlpLogRecord record in _records)
        {
            _overflow.TryEnqueue(record);
        }

        await _overflow.FlushAsync().ConfigureAwait(false);
        return _overflow.DroppedCount;
    }

}
