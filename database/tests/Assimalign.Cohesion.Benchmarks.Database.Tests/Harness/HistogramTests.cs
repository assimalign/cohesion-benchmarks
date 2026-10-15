using System;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Harness;

/// <summary>Numerical accuracy against known distributions, boundaries, merge and persisted counts.</summary>
public sealed class HistogramTests
{
    [Fact]
    public void Percentile_UniformDistribution_MatchesQuantilesWithinHdrPrecision()
    {
        var h = new LatencyHistogram();
        for (int i = 1; i <= 100_000; i++) h.Record(i);
        foreach (double p in new[] { 50d, 90, 95, 99, 99.9 })
        {
            double expected = 1000 * p;
            h.Percentile(p).ShouldBeInRange((long)expected, (long)(expected * 1.001));
        }
        h.Count.ShouldBe(100_000);
        h.Maximum.ShouldBe(100_000);
    }
    [Fact]
    public void Merge_EncodedBuckets_PreservesTailsAndCounts()
    {
        var left = new LatencyHistogram(); var right = new LatencyHistogram();
        for (int i = 0; i < 999; i++) left.Record(1);
        right.Record(LatencyHistogram.HighestTrackableValue);
        left.Merge(LatencyHistogram.Decode(right.Encode()));
        var actual = LatencyHistogram.Decode(left.Encode());
        actual.Count.ShouldBe(1000);
        actual.Percentile(99.9).ShouldBe(1);
        actual.Percentile(100).ShouldBe(LatencyHistogram.HighestTrackableValue);
    }
    [Fact]
    public void Record_OutOfBounds_RejectsInsteadOfClamping()
    {
        var h = new LatencyHistogram();
        Should.Throw<ArgumentOutOfRangeException>(() => h.Record(0));
        Should.Throw<ArgumentOutOfRangeException>(() => h.Record(LatencyHistogram.HighestTrackableValue + 1));
        for (int power = 1; power <= 31; power++)
        {
            long v = 1L << power;
            h.Record(v - 1); h.Record(v); h.Record(v + 1);
        }
        h.Count.ShouldBe(93);
    }
}
