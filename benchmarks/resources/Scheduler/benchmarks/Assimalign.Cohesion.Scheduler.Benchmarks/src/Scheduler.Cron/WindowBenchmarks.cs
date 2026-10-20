using System;
using Assimalign.Cohesion.Scheduler.Cron;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Cronos;

namespace Assimalign.Cohesion.Scheduler.Benchmarks.Scheduler.Cron;

/// <summary>
/// Counts occurrences in a fixed UTC window, either by advancing to each occurrence or by evaluating
/// each minute. Cronos has no match predicate; its inclusive next-occurrence lookup implements the
/// same predicate. Only five-field expressions with identical semantics are compared. Setup checks
/// every returned instant, including the exclusive window end, against Cronos.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class WindowBenchmarks
{
    private static readonly DateTime Start = new(2026, 10, 16, 0, 0, 0, DateTimeKind.Utc);
    private Crontab _schedule;
    private CronExpression _cronos = null!;
    private DateTime _end;

    [Params("*/5 * * * *", "0 9 * * 1-5")]
    public string Expression { get; set; } = "";

    [Params(1, 7)]
    public int Days { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _schedule = Crontab.Parse(Expression);
        _cronos = CronExpression.Parse(Expression);
        _end = Start.AddDays(Days);
        DateTime previous = Start.AddTicks(-1);
        while (true)
        {
            DateTime next = _schedule.GetDateTime(previous);
            if (next != _cronos.GetNextOccurrence(previous))
                throw new InvalidOperationException("Occurrence sequences disagree.");
            if (next >= _end) break;
            previous = next;
        }
        if (Evaluate() != Evaluate_Cronos() || Occurrences() != Evaluate())
            throw new InvalidOperationException("Window evaluation disagrees.");
    }

    [Benchmark]
    [BenchmarkCategory("Occurrences")]
    public int Occurrences()
    {
        int count = 0;
        for (DateTime next = _schedule.GetDateTime(Start.AddTicks(-1)); next < _end; next = _schedule.GetDateTime(next)) count++;
        return count;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Occurrences")]
    public int Occurrences_Cronos()
    {
        int count = 0;
        for (DateTime? next = _cronos.GetNextOccurrence(Start.AddTicks(-1)); next < _end; next = _cronos.GetNextOccurrence(next!.Value)) count++;
        return count;
    }

    [Benchmark]
    [BenchmarkCategory("Evaluate")]
    public int Evaluate()
    {
        int count = 0;
        for (DateTime instant = Start; instant < _end; instant = instant.AddMinutes(1))
            if (_schedule.IsMatch(instant)) count++;
        return count;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Evaluate")]
    public int Evaluate_Cronos()
    {
        int count = 0;
        for (DateTime instant = Start; instant < _end; instant = instant.AddMinutes(1))
            if (_cronos.GetNextOccurrence(instant, inclusive: true) == instant) count++;
        return count;
    }
}
