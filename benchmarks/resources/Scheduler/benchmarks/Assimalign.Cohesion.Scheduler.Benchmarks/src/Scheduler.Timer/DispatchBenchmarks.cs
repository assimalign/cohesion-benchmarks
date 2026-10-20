using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Scheduler;
using Assimalign.Cohesion.Scheduler.Cron;
using Assimalign.Cohesion.Scheduler.Timer;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Scheduler.Benchmarks.Scheduler.Timer;

/// <summary>
/// Runs the packaged Cron/Timer loop, clock evaluation, timer continuation, provider validation,
/// context creation and delegate dispatch for a bounded number of occurrences. An advancing UTC
/// clock removes wall-clock waits. Includes cancellation/drain of that loop, with fresh state per
/// iteration. No server, application startup, or counterpart (Cronos only calculates dates).
/// </summary>
[InvocationCount(1)]
public class DispatchBenchmarks
{
    private ISchedule _schedule = null!;
    private CancellationTokenSource _stop = null!;
    private int _executed;
    private DateTime _last;

    [Params("Cron", "Timer")]
    public string Trigger { get; set; } = "";

    [Params(1, 16)]
    public int Occurrences { get; set; }

    [IterationSetup]
    public void Setup()
    {
        _executed = 0;
        _stop = new CancellationTokenSource();
        var builder = new RecordingBuilder();
        var job = builder.AddJob("work", (context, _) =>
        {
            _last = context.ScheduledTime;
            if (++_executed == Occurrences) _stop.Cancel();
            return ValueTask.CompletedTask;
        });
        var clock = new AdvancingClock();
        if (Trigger == "Cron") builder.AddCronSchedule("work", Crontab.Parse("* * * * *"), job, clock);
        else builder.AddTimerSchedule("work", TimeSpan.Zero, TimeSpan.FromMinutes(1), job, clock);
        _schedule = builder.Provider.GetSchedules().Single();
    }

    [Benchmark]
    public async Task<DateTime> Dispatch()
    {
        await _schedule.RunAsync(_stop.Token);
        return _last;
    }

    [IterationCleanup]
    public void Cleanup()
    {
        _stop.Dispose();
        if (_executed != Occurrences || _schedule.Status != ScheduleStatus.Stopped)
            throw new InvalidOperationException("Dispatch did not drain the requested occurrences.");
    }
}
