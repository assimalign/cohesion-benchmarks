using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.OpenTelemetry.Benchmarks.OpenTelemetry;

/// <summary>
/// Exports structured attributes (arrays, maps, numeric and null values) and many instrumentation scopes
/// through the public log exporter. The OpenTelemetry SDK's OTLP exporter supports gRPC/HTTP protobuf,
/// not Cohesion's HTTP JSON wire contract, and the SDK adds recording/processing work absent here.
/// No SDK baseline is paired with this transport-only workload. Inputs are prebuilt; JSON encoding,
/// resource/scope grouping, request submission and acknowledgement handling remain measured.
/// </summary>
public class OtlpStructuredExportBenchmarks
{
    private IOtlpLogExporter _exporter = null!;
    private OtlpLogRecord[] _nested = null!;
    private OtlpLogRecord[] _scopes = null!;

    [Params(16, 256)]
    public int BatchSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var handler = new CollectorHandler();
        _exporter = ExporterFixture.Create(handler);
        _nested = ExporterFixture.Records(BatchSize, nested: true);
        _scopes = ExporterFixture.Records(BatchSize, scopes: 16);
        bool nested = ExportNestedAttributes().AsTask().GetAwaiter().GetResult();
        bool scopes = ExportManyScopes().AsTask().GetAwaiter().GetResult();
        if (!nested || !scopes || handler.Requests != 2 || handler.PayloadBytes == 0 || _exporter.FailedExportCount != 0)
        {
            throw new InvalidOperationException("The structured log batches were not exported.");
        }
    }

    [GlobalCleanup]
    public void Cleanup() => _exporter.DisposeAsync().AsTask().GetAwaiter().GetResult();

    [Benchmark]
    public ValueTask<bool> ExportNestedAttributes() => _exporter.ExportAsync(_nested);

    [Benchmark]
    public ValueTask<bool> ExportManyScopes() => _exporter.ExportAsync(_scopes);
}
