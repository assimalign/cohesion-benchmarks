using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.OpenTelemetry.Benchmarks.OpenTelemetry;

/// <summary>
/// The OTLP/HTTP JSON log exporter's two costs: exporting a batch (OTLP JSON encoding plus one HTTP POST), and the
/// application-side path of enqueuing records and draining them in one flush. The collector is an in-process
/// handler that accepts every request with an empty body, so no network enters the measurement; the background
/// flush interval is pushed out of the way so the benchmark controls every drain.
/// </summary>
public class OtlpLogExporterBenchmarks
{
    private IOtlpLogExporter _exporter = null!;
    private OtlpLogRecord[] _batch = null!;

    [Params(1, 64)]
    public int BatchSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _exporter = OtlpExporter.CreateLogExporter(new OtlpExporterOptions
        {
            Endpoint = new Uri("http://collector.invalid:4318"),
            ResourceAttributes = new Dictionary<string, string>
            {
                ["service.name"] = "appa-api",
                ["service.instance.id"] = "appa-api-0",
            },
            FlushInterval = TimeSpan.FromDays(1),
            MaxQueueLength = 4096,
            MaxBatchSize = 512,
            HandlerFactory = static () => new AcceptingCollectorHandler(),
        });

        DateTimeOffset timestamp = new(2026, 10, 18, 12, 0, 0, TimeSpan.Zero);
        _batch = new OtlpLogRecord[BatchSize];
        for (int index = 0; index < _batch.Length; index++)
        {
            _batch[index] = new OtlpLogRecord(
                timestamp.AddMilliseconds(index),
                SeverityNumber: 9,
                SeverityText: "INFO",
                Body: "Request GET /api/orders/42 completed with 200 in 3.2 ms",
                Category: index % 2 == 0 ? "Appa.Api.Orders" : "Appa.Api.Http",
                Attributes: new Dictionary<string, object?>
                {
                    ["http.method"] = "GET",
                    ["http.status_code"] = 200,
                    ["http.route"] = "/api/orders/{id}",
                    ["cache.hit"] = true,
                },
                TraceId: "4bf92f3577b34da6a3ce929d0e0e4736",
                SpanId: "00f067aa0ba902b7");
        }
    }

    [GlobalCleanup]
    public void Cleanup() => _exporter.DisposeAsync().AsTask().GetAwaiter().GetResult();

    /// <summary>Encodes the batch as one OTLP JSON request and posts it.</summary>
    [Benchmark]
    public ValueTask<bool> ExportAsync() => _exporter.ExportAsync(_batch);

    /// <summary>What a logging burst costs end to end: each record enqueued by the application, then one drain.</summary>
    [Benchmark]
    public async Task EnqueueThenFlush()
    {
        OtlpLogRecord[] batch = _batch;
        for (int index = 0; index < batch.Length; index++)
        {
            _exporter.TryEnqueue(batch[index]);
        }

        await _exporter.FlushAsync().ConfigureAwait(false);
    }

    private sealed class AcceptingCollectorHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent([]),
                RequestMessage = request,
            });
    }
}
