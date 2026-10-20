using System;
using System.Net;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.OpenTelemetry.Benchmarks.OpenTelemetry;

/// <summary>
/// Measures complete export attempts with JSON acknowledgement parsing, partial rejection, and permanent
/// HTTP rejection. Every response is deterministic and immediate; none retries or waits on a timer.
/// Standalone because the OpenTelemetry SDK exporter has a different wire encoding and processing contract.
/// </summary>
public class OtlpCollectorResponseBenchmarks
{
    private IOtlpLogExporter _accepted = null!;
    private IOtlpLogExporter _partial = null!;
    private IOtlpLogExporter _rejected = null!;
    private OtlpLogRecord[] _records = null!;

    [Params(1, 64)]
    public int BatchSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _records = ExporterFixture.Records(BatchSize);
        _accepted = ExporterFixture.Create(new CollectorHandler(response: "{\"partialSuccess\":{\"rejectedLogRecords\":\"0\"}}"));
        _partial = ExporterFixture.Create(new CollectorHandler(response: "{\"partialSuccess\":{\"rejectedLogRecords\":\"1\",\"errorMessage\":\"quota\"}}"));
        _rejected = ExporterFixture.Create(new CollectorHandler(HttpStatusCode.BadRequest));
        if (!ExportAcknowledged().AsTask().GetAwaiter().GetResult() ||
            ExportPartiallyRejected().AsTask().GetAwaiter().GetResult() ||
            ExportPermanentlyRejected().AsTask().GetAwaiter().GetResult() ||
            _partial.FailedExportCount != 1 || _rejected.FailedExportCount != 1)
        {
            throw new InvalidOperationException("Unexpected collector response handling.");
        }
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _accepted.DisposeAsync().ConfigureAwait(false);
        await _partial.DisposeAsync().ConfigureAwait(false);
        await _rejected.DisposeAsync().ConfigureAwait(false);
    }

    [Benchmark]
    public ValueTask<bool> ExportAcknowledged() => _accepted.ExportAsync(_records);

    [Benchmark]
    public ValueTask<bool> ExportPartiallyRejected() => _partial.ExportAsync(_records);

    [Benchmark]
    public ValueTask<bool> ExportPermanentlyRejected() => _rejected.ExportAsync(_records);
}
