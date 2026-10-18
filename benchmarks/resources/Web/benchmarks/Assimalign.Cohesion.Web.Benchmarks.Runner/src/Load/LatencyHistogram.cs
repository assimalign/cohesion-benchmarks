using System;
using System.Diagnostics;
using System.Numerics;

namespace Assimalign.Cohesion.Web.Benchmarks.Runner.Load;

/// <summary>
/// A log-linear histogram of Stopwatch ticks: exact below 256 ticks, then 128 sub-buckets per power of
/// two, so any recorded value is reported within 0.8% of itself. Recording is allocation-free; each
/// connection owns one and they are merged after the run.
/// </summary>
internal sealed class LatencyHistogram
{
    private const int SubBucketBits = 7;
    private const int SubBucketCount = 1 << SubBucketBits;      // 128
    private const int LinearLimit = SubBucketCount * 2;         // 256: exact range
    private const int Levels = 48;

    private readonly long[] _counts = new long[LinearLimit + Levels * SubBucketCount];

    public long Count { get; private set; }

    public long Sum { get; private set; }

    public long Max { get; private set; }

    public void Record(long ticks)
    {
        if (ticks < 0)
        {
            ticks = 0;
        }

        _counts[IndexOf(ticks)]++;
        Count++;
        Sum += ticks;
        if (ticks > Max)
        {
            Max = ticks;
        }
    }

    public void Add(LatencyHistogram other)
    {
        for (int i = 0; i < _counts.Length; i++)
        {
            _counts[i] += other._counts[i];
        }
        Count += other.Count;
        Sum += other.Sum;
        Max = Math.Max(Max, other.Max);
    }

    /// <summary>Gets the value at a percentile (0-100), in milliseconds.</summary>
    public double PercentileMilliseconds(double percentile)
    {
        if (Count == 0)
        {
            return 0;
        }

        long target = Math.Max(1, (long)Math.Ceiling(percentile / 100.0 * Count));
        long seen = 0;
        for (int i = 0; i < _counts.Length; i++)
        {
            seen += _counts[i];
            if (seen >= target)
            {
                return ToMilliseconds(Math.Min(MidpointOf(i), Max));
            }
        }

        return ToMilliseconds(Max);
    }

    public double MeanMilliseconds => Count == 0 ? 0 : ToMilliseconds(Sum / (double)Count);

    public double MaxMilliseconds => ToMilliseconds(Max);

    private static int IndexOf(long value)
    {
        if (value < LinearLimit)
        {
            return (int)value;
        }

        int msb = 63 - BitOperations.LeadingZeroCount((ulong)value);
        int shift = msb - SubBucketBits;                         // >= 1 here
        int level = Math.Min(shift, Levels);
        long sub = (value >> level) - SubBucketCount;             // [0, 128)
        if (sub >= SubBucketCount)
        {
            sub = SubBucketCount - 1;                            // clamped at the top level
        }
        return LinearLimit + (level - 1) * SubBucketCount + (int)sub;
    }

    private static double MidpointOf(int index)
    {
        if (index < LinearLimit)
        {
            return index;
        }

        int level = (index - LinearLimit) / SubBucketCount + 1;
        long sub = (index - LinearLimit) % SubBucketCount + SubBucketCount;
        long lower = sub << level;
        long upper = ((sub + 1) << level) - 1;
        return (lower + upper) / 2.0;
    }

    private static double ToMilliseconds(double ticks) => ticks * 1000.0 / Stopwatch.Frequency;
}
