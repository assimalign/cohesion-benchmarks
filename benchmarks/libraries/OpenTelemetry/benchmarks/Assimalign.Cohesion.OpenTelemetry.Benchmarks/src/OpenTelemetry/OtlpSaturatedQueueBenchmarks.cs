using System;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.OpenTelemetry.Benchmarks.OpenTelemetry;

/// <summary>
/// Producer writes under sustained queue pressure: each invocation evicts one old record per write.
/// The queue is filled in setup, has no consumer during measurement, and is drained only in cleanup.
/// Burst size changes the measured work; transport batching is irrelevant to this producer-only path.
/// Standalone: this is Cohesion's specific bounded drop-oldest contract, not SDK recording or processing.
/// </summary>
public class OtlpSaturatedQueueBenchmarks
{
    private IOtlpLogExporter _exporter = null!;
    private OtlpLogRecord[] _records = null!;

    [Params(16, 128)]
    public int BurstSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _records = ExporterFixture.Records(BurstSize);
        _exporter = ExporterFixture.Create(new CollectorHandler(), queueLength: 64);
        for (int index = 0; index < 64; index++)
        {
            _exporter.TryEnqueue(_records[index % BurstSize]);
        }

        if (EnqueueFullQueue() != BurstSize || _exporter.DroppedCount != BurstSize)
        {
            throw new InvalidOperationException("The full queue did not evict one record per write.");
        }
    }

    [GlobalCleanup]
    public void Cleanup() => _exporter.DisposeAsync().AsTask().GetAwaiter().GetResult();

    [Benchmark]
    public int EnqueueFullQueue()
    {
        int accepted = 0;
        foreach (OtlpLogRecord record in _records)
        {
            if (_exporter.TryEnqueue(record))
            {
                accepted++;
            }
        }

        return accepted;
    }
}
