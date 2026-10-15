using System;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>Fixed UTC calendar with seasonal, weekday and hourly weights; strict, bounded event ordering.</summary>
public static class DatasetTime
{
    public static readonly DateTime Start = new(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static readonly DateTime End = new(2025, 12, 31, 23, 59, 59, DateTimeKind.Utc);
    private static readonly WeightedCategorical Days = BuildDays();
    private static readonly WeightedCategorical Hours = new(1, 1, 1, 1, 1, 2, 3, 5, 7, 8, 8, 8, 9, 8, 8, 9, 11, 13, 15, 16, 15, 12, 8, 4);

    private static WeightedCategorical BuildDays()
    {
        var weights = new double[(End.Date - Start).Days + 1];
        for (int i = 0; i < weights.Length; i++)
        {
            DateTime day = Start.AddDays(i);
            weights[i] = (day.Month >= 11 ? 2.6 : 1.0) * (day.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday ? 0.8 : 1.2);
        }
        return new WeightedCategorical(weights);
    }

    public static DateTime Sample(ref EntityRandom random, int reserveSeconds = 0)
    {
        DateTime value = Start.AddDays(Days.Sample(ref random)).AddHours(Hours.Sample(ref random)).AddSeconds(random.Int(0, 3599));
        return value > End.AddSeconds(-reserveSeconds) ? End.AddSeconds(-reserveSeconds) : value;
    }

    /// <summary>Returns a strictly later timestamp, reserving room for subsequent events before the fixed horizon.</summary>
    public static DateTime After(DateTime parent, ref EntityRandom random, int maximumDelaySeconds = 86400, int remainingEvents = 0)
    {
        long available = (long)(End - parent).TotalSeconds - remainingEvents;
        if (available < 1) throw new ArgumentOutOfRangeException(nameof(parent), "No room remains in the dataset horizon.");
        return parent.AddSeconds(random.Long(1, Math.Min(maximumDelaySeconds, available)));
    }
}
