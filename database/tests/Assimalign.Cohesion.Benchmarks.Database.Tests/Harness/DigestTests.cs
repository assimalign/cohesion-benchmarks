using System;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Harness;

/// <summary>Cross-adapter normalization and unambiguous row framing.</summary>
public sealed class DigestTests
{
    private static RowsDigest Digest(bool ordered, params object?[][] rows)
    {
        using var digest = new VerificationDigest(ordered);
        foreach (var row in rows) digest.AddRow(row);
        return digest.Complete();
    }
    [Fact]
    public void Complete_UnorderedRows_SortsButPreservesMultiplicity()
    {
        Digest(false, [1], [2]).ShouldBe(Digest(false, [2], [1]));
        Digest(true, [1], [2]).ShouldNotBe(Digest(true, [2], [1]));
        Digest(false, [1], [1]).ShouldNotBe(Digest(false, [1]));
    }
    [Fact]
    public void Complete_DecimalsAndFloatingAverages_NormalizesSpecifiedPrecision()
    {
        Digest(true, [1.234m, 123.45671d]).ShouldBe(Digest(true, [1.23m, 123.45674d]));
        Digest(true, [1.235m]).ShouldBe(Digest(true, [1.24m]));
        Digest(true, [1.225m]).ShouldBe(Digest(true, [1.22m]));
        Digest(true, [-0.0d, 1L]).ShouldBe(Digest(true, [0d, 1]));
        Digest(true, [123.4567d]).ShouldNotBe(Digest(true, [123.4577d]));
    }
    [Fact]
    public void Complete_Timestamps_UsesUtcIncludingUnspecifiedSqlTime()
    {
        var utc = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        Digest(true, [utc]).ShouldBe(Digest(true, [new DateTimeOffset(2026, 1, 1, 22, 4, 5, TimeSpan.FromHours(-5))]));
        Digest(true, [DateTime.SpecifyKind(utc, DateTimeKind.Unspecified)]).ShouldBe(Digest(true, [utc]));
    }
    [Fact]
    public void Complete_NullsStringsAndBoundaries_NeverConflatesDifferentRows()
    {
        Digest(true, new object?[] { null }).ShouldBe(Digest(true, [DBNull.Value]));
        Digest(true, new object?[] { null }).ShouldNotBe(Digest(true, [""]));
        Digest(true, ["é"]).ShouldNotBe(Digest(true, ["e\u0301"]));
        Digest(true, ["a", "bc"]).ShouldNotBe(Digest(true, ["ab", "c"]));
        Digest(true, ["a\nb"]).ShouldNotBe(Digest(true, ["a"], ["b"]));
        Digest(true, new object?[0]).RowCount.ShouldBe(1);
        Digest(true).RowCount.ShouldBe(0);
    }
}
