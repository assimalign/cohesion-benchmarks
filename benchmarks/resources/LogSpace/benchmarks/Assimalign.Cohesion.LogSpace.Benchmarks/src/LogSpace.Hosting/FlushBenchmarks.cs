using System;
using System.IO;
using System.Text.Json;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.LogSpace.Benchmarks.LogSpace.Hosting;

/// <summary>
/// Internal durable flush: JSON serialization, append, per-record flush-to-disk, and sidecar index
/// writes. The store is internal and no public in-process persistence API exists. Setup queues the
/// records; cleanup verifies persisted output. One invocation keeps every sample a real write.
/// Standalone: generic logging sinks do not implement the same durability and index guarantees.
/// </summary>
[InvocationCount(1)]
public class FlushBenchmarks
{
    private string _directory = null!;
    private LogSpaceFixture.StoreAccess _store = null!;
    private System.Collections.IList _records = null!;

    [Params(1, 128)]
    public int Records { get; set; }

    [GlobalSetup]
    public void CreateInputs()
    {
        using var document = JsonDocument.Parse(LogSpaceFixture.Payload(Records, false));
        _records = LogSpaceFixture.BindReader()(document.RootElement, "service-a");
    }

    [IterationSetup]
    public void Setup()
    {
        _directory = Path.Combine(Path.GetTempPath(), "cohesion-logspace-flush-" + Guid.NewGuid().ToString("N"));
        _store = LogSpaceFixture.CreateStore(_directory);
        foreach (object record in _records)
            if (!_store.Enqueue(record)) throw new InvalidOperationException();
    }

    [Benchmark]
    public void FlushBatch() => _store.Flush(false);

    [IterationCleanup]
    public void Cleanup()
    {
        try
        {
            if (_store.Query(null, null, Records + 1, null).Body.Split('\n', StringSplitOptions.RemoveEmptyEntries).Length != Records)
                throw new InvalidOperationException("Flush did not persist the batch.");
        }
        finally { Directory.Delete(_directory, recursive: true); }
    }
}
