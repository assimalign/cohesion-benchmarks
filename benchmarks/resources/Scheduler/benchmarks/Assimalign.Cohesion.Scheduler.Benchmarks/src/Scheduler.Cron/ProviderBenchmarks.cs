using System;
using System.Linq;
using System.Threading.Tasks;
using Assimalign.Cohesion.Scheduler;
using Assimalign.Cohesion.Scheduler.Cron;
using Assimalign.Cohesion.Scheduler.Timer;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Scheduler.Benchmarks.Scheduler.Cron;

/// <summary>
/// The real feature providers' schedule lookup and job enablement validation, through the public
/// composition extensions. Each provider owns one schedule and one job, as these extensions create.
/// Standalone: Cronos does not implement a job registry or dispatch provider.
/// </summary>
public class ProviderBenchmarks
{
    private IScheduleProvider _provider = null!;
    private ISchedule _schedule = null!;
    private IScheduleJob _job = null!;

    [Params("Cron", "Timer")]
    public string Trigger { get; set; } = "";

    [GlobalSetup]
    public void Setup()
    {
        var builder = new RecordingBuilder();
        _job = builder.AddJob("work", static (_, _) => ValueTask.CompletedTask);
        if (Trigger == "Cron") builder.AddCronSchedule("*/5 * * * *", _job);
        else builder.AddTimerSchedule(TimeSpan.FromMinutes(5), _job);
        _provider = builder.Provider;
        _schedule = _provider.GetSchedules().Single();
        if (!_provider.IsJobEnabled(_schedule.Id, _job.Id)) throw new InvalidOperationException();
    }

    [Benchmark]
    public ISchedule Lookup() => _provider.GetSchedule(_schedule.Id);

    [Benchmark]
    public bool ValidateEnabledJob() => _provider.IsJobEnabled(_schedule.Id, _job.Id);

    [Benchmark]
    public bool DisableAndEnable()
    {
        _provider.DisableJob(_schedule.Id, _job.Id);
        bool disabled = !_provider.IsJobEnabled(_schedule.Id, _job.Id);
        _provider.EnableJob(_schedule.Id, _job.Id);
        return disabled;
    }
}
