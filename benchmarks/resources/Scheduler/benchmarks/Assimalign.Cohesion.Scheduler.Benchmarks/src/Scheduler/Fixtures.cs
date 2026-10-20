using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Scheduler;

namespace Assimalign.Cohesion.Scheduler.Benchmarks;

// Composition-only fixture: feature extensions still construct the real packaged providers,
// schedules and delegate jobs. No application is started and no endpoint is opened.
internal sealed class RecordingBuilder : ISchedulerApplicationBuilder
{
    internal IScheduleProvider Provider { get; private set; } = null!;
    public ISchedulerApplicationBuilder AddJob(IScheduleJob job) => this;
    public ISchedulerApplicationBuilder AddScheduleProvider(IScheduleProvider provider)
    {
        Provider = provider;
        return this;
    }
    public ISchedulerApplication Build() => throw new NotSupportedException();
}

internal sealed class AdvancingClock : TimeProvider
{
    private DateTimeOffset _now = new(2026, 10, 16, 0, 0, 30, TimeSpan.Zero);
    public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
    public override DateTimeOffset GetUtcNow() => _now;
    public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
        _now += dueTime;
        ThreadPool.QueueUserWorkItem(_ => callback(state));
        return new ImmediateTimer();
    }
    private sealed class ImmediateTimer : ITimer
    {
        public bool Change(TimeSpan dueTime, TimeSpan period) => true;
        public void Dispose() { }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
