using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.OpenTelemetry.Benchmarks.OpenTelemetry;

internal static class ExporterFixture
{
    internal static IOtlpLogExporter Create(CollectorHandler handler, int queueLength = 4096, int batchSize = 512)
        => OtlpExporter.CreateLogExporter(new OtlpExporterOptions
        {
            Endpoint = new Uri("http://collector.invalid:4318"),
            FlushInterval = TimeSpan.FromDays(1),
            MaxQueueLength = queueLength,
            MaxBatchSize = batchSize,
            HandlerFactory = () => handler,
            ResourceAttributes = new Dictionary<string, string>
            {
                ["service.name"] = "orders-api", ["service.instance.id"] = "orders-api-0",
            },
            Headers = new Dictionary<string, string> { ["X-Tenant"] = "benchmark" },
        });

    internal static OtlpLogRecord[] Records(int count, bool nested = false, int scopes = 1)
    {
        var records = new OtlpLogRecord[count];
        DateTimeOffset timestamp = new(2026, 10, 18, 12, 0, 0, TimeSpan.Zero);
        for (int index = 0; index < count; index++)
        {
            IReadOnlyDictionary<string, object?> attributes = nested
                ? new Dictionary<string, object?>
                {
                    ["order"] = new Dictionary<string, object?>
                    {
                        ["id"] = 42L,
                        ["items"] = new object?[] { "book", 3, true, null },
                        ["price"] = 19.95m,
                    },
                    ["durations"] = new double[] { 1.25, 2.5, 5.75 },
                    ["optional"] = null,
                }
                : new Dictionary<string, object?>
                {
                    ["http.route"] = "/orders/{id}", ["http.status_code"] = 200,
                    ["duration"] = 3.25, ["cache.hit"] = true,
                };
            records[index] = new OtlpLogRecord(timestamp.AddMilliseconds(index), 9, "INFO",
                "Order request completed", "Orders.Scope" + index % scopes, attributes,
                "4bf92f3577b34da6a3ce929d0e0e4736", "00f067aa0ba902b7");
        }

        return records;
    }
}

/// <summary>Consumes each encoded request without retaining it, avoiding network and unbounded capture state.</summary>
internal sealed class CollectorHandler(HttpStatusCode status = HttpStatusCode.OK, string response = "") : HttpMessageHandler
{
    private readonly byte[] _response = Encoding.UTF8.GetBytes(response);

    internal int Requests { get; private set; }
    internal long PayloadBytes { get; private set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests++;
        PayloadBytes += (await request.Content!.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false)).Length;
        return new HttpResponseMessage(status)
        {
            Content = new ByteArrayContent(_response), RequestMessage = request,
        };
    }
}
