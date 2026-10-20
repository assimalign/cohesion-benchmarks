using System;
using System.Collections;
using System.IO;
using System.Text.Json;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.LogSpace.Benchmarks.LogSpace.Hosting;

/// <summary>
/// Actual internal OTLP AnyValue conversion, timestamp validation, attribute merging and identity
/// checks. This is the ingest core, which has no public API without the HTTPS listener. Standalone:
/// generic JSON deserialization does not implement these validation and normalization guarantees.
/// </summary>
public class IngestionBenchmarks
{
    private byte[] _payload = null!;
    private JsonDocument _document = null!;
    private Func<JsonElement, string?, IList> _read = null!;

    [Params(1, 128)]
    public int Records { get; set; }

    [Params(false, true)]
    public bool Structured { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _read = LogSpaceFixture.BindReader();
        _payload = LogSpaceFixture.Payload(Records, Structured);
        _document = JsonDocument.Parse(_payload);
        if (_read(_document.RootElement, "service-a").Count != Records) throw new InvalidOperationException();
    }

    [Benchmark]
    public IList ReadParsedOtlp() => _read(_document.RootElement, "service-a");

    [Benchmark]
    public IList ParseAndReadOtlp()
    {
        using var document = JsonDocument.Parse(_payload);
        return _read(document.RootElement, "service-a");
    }

    [GlobalCleanup]
    public void Cleanup() => _document.Dispose();
}

/// <summary>
/// Bounded-channel admission and full parse/normalize/admit ingestion, using internal packaged
/// methods because no public in-process ingestion seam exists. Fresh queues per iteration prevent
/// saturation from changing the measured work. No flush, disk I/O or receiver authentication here.
/// </summary>
[InvocationCount(1)]
public class QueueBenchmarks
{
    private byte[] _payload = null!;
    private IList _records = null!;
    private Func<JsonElement, string?, IList> _read = null!;
    private LogSpaceFixture.StoreAccess _store = null!;

    [Params(1, 128)]
    public int Records { get; set; }

    [Params(false, true)]
    public bool Structured { get; set; }

    [GlobalSetup]
    public void CreateInputs()
    {
        _read = LogSpaceFixture.BindReader();
        _payload = LogSpaceFixture.Payload(Records, Structured);
        using var document = JsonDocument.Parse(_payload);
        _records = _read(document.RootElement, "service-a");
    }

    [IterationSetup]
    public void Setup() => _store = LogSpaceFixture.CreateStore(Path.Combine(Path.GetTempPath(), "cohesion-logspace-queue-" + Guid.NewGuid().ToString("N")));

    [Benchmark]
    public int EnqueueBatch() => Admit(_records);

    [Benchmark]
    public int IngestBatch()
    {
        using var document = JsonDocument.Parse(_payload);
        return Admit(_read(document.RootElement, "service-a"));
    }

    private int Admit(IList records)
    {
        int admitted = 0;
        foreach (object record in records)
        {
            if (!_store.Enqueue(record)) throw new InvalidOperationException("Unexpected queue rejection.");
            admitted++;
        }
        return admitted;
    }
}
