using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Harness;

/// <summary>Result serialization, verification comparison and actual optimization decisions.</summary>
public sealed class ResultTests
{
    internal static VerificationFile Verified(string target, string hash = "abc") => new()
    {
        Model = "sql", Target = target, DatasetManifestSha256 = hash, Seed = 42,
        Operations = [new("op", new(SupportLevel.Native), Enumerable.Repeat(new RowsDigest(1, "digest"), 100).ToArray())],
    };
    internal static BenchmarkResult Result(string target, double p50, bool ranked = true) => new()
    {
        RunId = "test-" + target, Model = "sql", Target = new(target, target, ranked, target, "engine"), Suite = "latency",
        DatasetManifestSha256 = "abc", Seed = 42, Scale = .01, Engine = new("1", new Dictionary<string, string>()),
        Environment = new(), Verification = Verified(target), Repetition = 1,
        Operations = [new() { Operation = "op", Support = new(SupportLevel.Native), Clients = 1, Successes = 10, P50Us = (long)p50, Seconds = 1 }],
    };
    [Fact]
    public void Serialize_ResultWithHistogramAndSupport_RoundTripsGeneratedContext()
    {
        var h = new LatencyHistogram(); h.Record(123);
        var source = Result("cohesion-sql-tcp", 123) with
        { Operations = [new() { Operation = "op", Support = new(SupportLevel.Emulated, "Sort selected rows"), Histogram = h.Encode(), Successes = 1 }] };
        string json = JsonSerializer.Serialize(source, ResultJsonContext.Default.BenchmarkResult);
        var copy = JsonSerializer.Deserialize(json, ResultJsonContext.Default.BenchmarkResult)!;
        copy.SchemaVersion.ShouldBe(1);
        copy.Operations[0].Support.Level.ShouldBe(SupportLevel.Emulated);
        LatencyHistogram.Decode(copy.Operations[0].Histogram!).Count.ShouldBe(1);
        copy.Verification!.Operations[0].Cases.Count.ShouldBe(100);
    }
    [Fact]
    public void Compare_SupportedMismatchesAndUnsupported_ReportsOnlySupportedDifferences()
    {
        var left = Verified("a"); var right = Verified("b");
        VerificationRunner.Compare([left, right]).Count.ShouldBe(0);
        right = right with { Operations = [new("op", new(SupportLevel.Native), Enumerable.Repeat(new RowsDigest(2, "wrong"), 100).ToArray())] };
        VerificationRunner.Compare([left, right]).Count.ShouldBe(100);
        right = right with { Operations = [new("op", new(SupportLevel.Unsupported, "No feature", "error"), [])] };
        VerificationRunner.Compare([left, right]).Count.ShouldBe(0);
    }
    [Fact]
    public async Task VerifyAsync_FixedParameters_ExecutesExactlyOneHundredCases()
    {
        var fake = new FakeWorkload(new FakeClock());
        var result = await VerificationRunner.VerifyAsync(fake, fake, FakeWorkload.Manifest, "abc", fake.Operations);
        fake.Calls.ShouldBe(100);
        result.Operations[0].Cases.Count.ShouldBe(100);
        VerificationRunner.Compare([result]).Count.ShouldBe(0);
    }
    [Fact]
    public void RequireOptimized_DebugOrMissingEvidence_RefusesOfficialRun()
    {
        Should.Throw<InvalidOperationException>(() => OptimizationGuard.RequireOptimized([]));
        Should.Throw<InvalidOperationException>(() => OptimizationGuard.RequireOptimized([new("engine", false, "1", null)]));
        Should.NotThrow(() => OptimizationGuard.RequireOptimized([new("engine", true, "1+abc", null)]));
    }
}
