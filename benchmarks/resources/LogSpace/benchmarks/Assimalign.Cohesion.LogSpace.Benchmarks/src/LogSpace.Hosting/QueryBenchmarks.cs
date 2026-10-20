using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.LogSpace.Benchmarks.LogSpace.Hosting;

/// <summary>
/// Internal segment queries: deserialize NDJSON, filter by resource/time, and return bounded pages
/// with filter-bound cursors. The public product only exposes this through an authenticated server.
/// Deterministic stored-format fixtures occupy two segments in a private temp directory. Standalone;
/// an in-memory filter would omit file scanning, validation, cursor and output construction.
/// </summary>
public class QueryBenchmarks
{
    private string _directory = null!;
    private LogSpaceFixture.StoreAccess _store = null!;
    private string _cursor = null!;
    private DateTimeOffset _since;

    [Params(64, 1024)]
    public int Records { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _directory = Path.Combine(Path.GetTempPath(), "cohesion-logspace-query-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_directory);
        var lines = Enumerable.Range(0, Records).Select(i => JsonSerializer.Serialize(new
        {
            t = LogSpaceFixture.Nanoseconds(LogSpaceFixture.Start.AddSeconds(i)), sev = 9, sevText = "INFO",
            svc = i % 2 == 0 ? "service-a" : "service-b", cat = "Orders", body = "GET /orders completed successfully",
            attrs = new { status = 200, cached = false }
        })).ToArray();
        File.WriteAllLines(Path.Combine(_directory, "logs-20261016-0000.ndjson"), lines.Take(Records / 2));
        File.WriteAllLines(Path.Combine(_directory, "logs-20261016-0001.ndjson"), lines.Skip(Records / 2));
        _store = LogSpaceFixture.CreateStore(_directory);
        _since = LogSpaceFixture.Start.AddSeconds(Records / 2);
        _cursor = _store.Query(null, null, 32, null).Cursor ?? throw new InvalidOperationException();
        Verify(All().Body, Records);
        Verify(FilterResource().Body, Records / 2);
        Verify(FilterTime().Body, Records / 2);
        Verify(MissingResource().Body, 0);
        Verify(ResumePage().Body, 32);
        string? cursor = null;
        int total = 0;
        do
        {
            var page = _store.Query(null, null, 32, cursor);
            total += page.Body.Split('\n', StringSplitOptions.RemoveEmptyEntries).Length;
            cursor = page.Cursor;
        } while (cursor is not null);
        if (total != Records) throw new InvalidOperationException("Pagination lost records.");
    }

    private static void Verify(string body, int expected)
    {
        var lines = body.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length != expected) throw new InvalidOperationException("Incorrect query count.");
        foreach (string line in lines) { using var document = JsonDocument.Parse(line); }
    }

    [Benchmark]
    public (string Body, string? Cursor) All() => _store.Query(null, null, Records + 1, null);

    [Benchmark]
    public (string Body, string? Cursor) FilterResource() => _store.Query("service-a", null, Records + 1, null);

    [Benchmark]
    public (string Body, string? Cursor) FilterTime() => _store.Query(null, _since, Records + 1, null);

    [Benchmark]
    public (string Body, string? Cursor) MissingResource() => _store.Query("missing", null, 32, null);

    [Benchmark]
    public (string Body, string? Cursor) ResumePage() => _store.Query(null, null, 32, _cursor);

    [GlobalCleanup]
    public void Cleanup() => Directory.Delete(_directory, recursive: true);
}
