using System;

using Assimalign.Cohesion.Scheduler.Cron;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Cronos;

namespace Assimalign.Cohesion.Scheduler.Benchmarks.Scheduler.Cron;

/// <summary>
/// The two operations a cron schedule runs: parsing the five-field expression (once per schedule), and finding the
/// next occurrence after an instant (once per occurrence, by the provider deciding how long to sleep). A frequent
/// expression finds its occurrence within the hour; the weekday one, evaluated from a Friday morning, has to skip
/// the weekend. Cronos is the baseline, so the Ratio column reads "Cohesion relative to the counterpart".
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class CrontabBenchmarks
{
    private static readonly DateTime From = new(2026, 10, 16, 10, 0, 30, DateTimeKind.Utc);

    private Crontab _cohesion;
    private CronExpression _cronos = null!;

    [Params("*/5 * * * *", "0 9 * * 1-5")]
    public string Expression { get; set; } = "";

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = Crontab.Parse(Expression);
        _cronos = CronExpression.Parse(Expression);

        DateTime cohesionNext = _cohesion.GetDateTime(From);
        DateTime? cronosNext = _cronos.GetNextOccurrence(From);
        if (cohesionNext != cronosNext)
        {
            throw new InvalidOperationException(
                $"The implementations disagree on the next occurrence of '{Expression}': {cohesionNext:O} and {cronosNext:O}.");
        }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Parse")]
    public CronExpression Parse_Cronos() => CronExpression.Parse(Expression);

    [Benchmark]
    [BenchmarkCategory("Parse")]
    public Crontab Parse() => Crontab.Parse(Expression);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("NextOccurrence")]
    public DateTime? NextOccurrence_Cronos() => _cronos.GetNextOccurrence(From);

    [Benchmark]
    [BenchmarkCategory("NextOccurrence")]
    public DateTime NextOccurrence() => _cohesion.GetDateTime(From);
}
