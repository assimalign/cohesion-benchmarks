using System;
using System.Threading.Tasks;
using Assimalign.Cohesion.Scheduler;
using Assimalign.Cohesion.Scheduler.Hosting;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Scheduler.Benchmarks.Scheduler.Hosting;

/// <summary>
/// Repeated registration of an existing job scans the real hosting registry and checks identity.
/// Batch declaration measures distinct jobs and their duplicate-ID checks, not host startup.
/// Inputs and fresh builders are prepared outside measurement. Standalone scheduler semantics.
/// </summary>
[InvocationCount(1)]
public class RegistryBenchmarks
{
    private SchedulerApplicationBuilder _populated = null!;
    private SchedulerApplicationBuilder _empty = null!;
    private IScheduleJob[] _jobs = null!;

    [Params(1, 64)]
    public int Jobs { get; set; }

    [GlobalSetup]
    public void CreateInputs()
    {
        var builder = new RecordingBuilder();
        _jobs = new IScheduleJob[Jobs];
        for (int i = 0; i < Jobs; i++)
            _jobs[i] = builder.AddJob($"job-{i}", static (_, _) => ValueTask.CompletedTask);
    }

    [IterationSetup]
    public void Setup()
    {
        _populated = SchedulerApplication.CreateBuilder([]);
        _empty = SchedulerApplication.CreateBuilder([]);
        foreach (var job in _jobs) _populated.AddJob(job);
    }

    [Benchmark]
    public SchedulerApplicationBuilder RegisterExisting() => _populated.AddJob(_jobs[^1]);

    [Benchmark]
    public SchedulerApplicationBuilder RegisterBatch()
    {
        foreach (var job in _jobs) _empty.AddJob(job);
        return _empty;
    }
}
