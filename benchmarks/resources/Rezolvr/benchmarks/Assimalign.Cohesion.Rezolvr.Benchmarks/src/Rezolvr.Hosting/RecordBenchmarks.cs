using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Assimalign.Cohesion.Hosting.Resources;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Rezolvr.Benchmarks.Rezolvr.Hosting;

/// <summary>
/// Implemented DNS declaration storage, NOT DNS resolution (the pinned resolver endpoint is a TODO).
/// Measures internal cached snapshots and the real command handler's validated apply/delete paths:
/// owner/conflict checks, dictionary copy, canonical JSON and atomic replacement. No public storage
/// seam exists; only construction/binding uses reflection. Standalone, with no network or server.
/// </summary>
public class RecordBenchmarks
{
    private string _directory = null!;
    private RecordFixture.Access _repository = null!;
    private ResourceCommand _existing = null!;
    private ResourceCommand _missing = null!;

    [Params("A", "CNAME")]
    public string Type { get; set; } = "";

    [Params(16, 256)]
    public int Records { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        _directory = Path.Combine(Path.GetTempPath(), "cohesion-rezolvr-records-" + Guid.NewGuid().ToString("N"));
        RecordFixture.Seed(_directory, Type, Records);
        _repository = RecordFixture.Open(_directory, Type);
        _existing = RecordFixture.Command(Type, Records - 1);
        _missing = RecordFixture.Command(Type, Records);
        if ((await _repository.Read(default)).Count != Records) throw new InvalidOperationException();
        await _repository.Handler.ExecuteAsync(_existing);
        await _repository.Handler.DeleteAsync(_existing);
        if ((await _repository.Read(default)).Count != Records - 1) throw new InvalidOperationException();
        await _repository.Handler.ExecuteAsync(_existing);
    }

    [Benchmark]
    public Task<IReadOnlyList<ResourceCommand>> ReadSnapshot() => _repository.Read(default);

    [Benchmark]
    public ValueTask<ReadOnlyMemory<byte>> ReapplyRecord() => _repository.Handler.ExecuteAsync(_existing);

    [Benchmark]
    public async ValueTask<ReadOnlyMemory<byte>> DeleteAndRestore()
    {
        await _repository.Handler.DeleteAsync(_existing);
        return await _repository.Handler.ExecuteAsync(_existing);
    }

    [Benchmark]
    public ValueTask<ReadOnlyMemory<byte>> DeleteMissingRecord() => _repository.Handler.DeleteAsync(_missing);

    [GlobalCleanup]
    public async Task Cleanup()
    {
        try
        {
            var restored = await RecordFixture.Open(_directory, Type).Read(default);
            if (restored.Count != Records) throw new InvalidOperationException("Persistent record count changed.");
        }
        finally { Directory.Delete(_directory, recursive: true); }
    }
}

/// <summary>
/// First snapshot after repository activation: file read, generated JSON deserialization and
/// case-insensitive record dictionary creation. Fresh repository per sample; OS cache is not reset.
/// This is record recovery, not resolution or a DNS cache. Internal seam as above.
/// </summary>
[InvocationCount(1)]
public class RecoveryBenchmarks
{
    private string _directory = null!;
    private RecordFixture.Access _repository = null!;

    [Params("A", "CNAME")]
    public string Type { get; set; } = "";

    [Params(16, 256)]
    public int Records { get; set; }

    [GlobalSetup]
    public void CreateInputs()
    {
        _directory = Path.Combine(Path.GetTempPath(), "cohesion-rezolvr-recovery-" + Guid.NewGuid().ToString("N"));
        RecordFixture.Seed(_directory, Type, Records);
    }

    [IterationSetup]
    public void Setup() => _repository = RecordFixture.Open(_directory, Type);

    [Benchmark]
    public Task<IReadOnlyList<ResourceCommand>> LoadSnapshot() => _repository.Read(default);

    [IterationCleanup]
    public void Verify()
    {
        if (_repository.Read(default).GetAwaiter().GetResult().Count != Records) throw new InvalidOperationException();
    }

    [GlobalCleanup]
    public void Cleanup() => Directory.Delete(_directory, recursive: true);
}
