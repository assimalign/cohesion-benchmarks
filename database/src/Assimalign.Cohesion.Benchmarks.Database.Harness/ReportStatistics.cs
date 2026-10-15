using System;
using System.Collections.Generic;
using System.Linq;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Pure repetition aggregation and inclusive deficit thresholds.</summary>
public static class ReportStatistics
{
    public static double Median(IEnumerable<double> values)
    {
        double[] sorted = values.Order().ToArray();
        if (sorted.Length == 0) throw new ArgumentException("No observations.");
        return sorted.Length % 2 == 1 ? sorted[sorted.Length / 2] :
            (sorted[sorted.Length / 2 - 1] + sorted[sorted.Length / 2]) / 2;
    }
    public static double Ratio(double value, double best, bool higherIsBetter = false)
        => higherIsBetter ? (value > 0 ? best / value : double.PositiveInfinity) :
            (best > 0 ? value / best : double.PositiveInfinity);
    public static bool IsDeficit(double ratio, bool load = false) => ratio >= (load ? 1.5 : 2);
}
