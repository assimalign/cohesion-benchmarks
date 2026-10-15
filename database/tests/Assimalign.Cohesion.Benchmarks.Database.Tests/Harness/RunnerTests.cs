using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Harness;

/// <summary>Fake-clock arithmetic proves warm-up isolation, parameter timing, deadlines and repetition counts.</summary>
public sealed class RunnerTests
{
    [Fact]
    public void WriteId_WorkloadScopes_NeverCollidesAcrossPhasesAndRepetitions()
    {
        var ids = new HashSet<long>();
        foreach (string suite in new[] { "verify", "latency", "throughput", "mixed/read-heavy", "mixed/balanced" })
        foreach (int repetition in new[] { 1, 2, 100 })
        foreach (int concurrency in new[] { 1, 8, 32 })
        foreach (bool warmup in new[] { false, true })
        for (int client = 0; client < concurrency; client++)
            ids.Add(new ParameterStreamContext(suite, repetition, concurrency, warmup).WriteId(1000, client, 0)).ShouldBeTrue();
    }
    [Fact]
    public async Task RunAsync_UnsupportedMixedMember_ReturnsBlockedEvidenceWithoutChangingMix()
    {
        var clock = new FakeClock(); var fake = new FakeWorkload(clock)
        { Support = new Dictionary<string, OperationSupport> { ["op"] = new(SupportLevel.Unsupported, "No feature", "source.cs:1") } };
        var pass = (await new BenchmarkRunner(clock).RunAsync(fake, fake, FakeWorkload.Manifest, fake.Operations,
            new() { Suite = "mixed", Repeat = 1 }))[0];
        pass.BlockReason.ShouldNotBeNull();
        pass.Operations[0].Support.Level.ShouldBe(SupportLevel.Unsupported);
        fake.Calls.ShouldBe(0);
    }
    [Fact]
    public async Task RunAsync_WarmupAndParameters_RecordsOnlyExecution()
    {
        var clock = new FakeClock();
        var fake = new FakeWorkload(clock) { ParameterCost = TimeSpan.FromMilliseconds(1) };
        var passes = await new BenchmarkRunner(clock).RunAsync(fake, fake, FakeWorkload.Manifest, fake.Operations,
            new() { Warmup = TimeSpan.FromMilliseconds(4), Duration = TimeSpan.FromMilliseconds(6), Repeat = 2 });
        passes.Count.ShouldBe(2);
        foreach (var pass in passes)
        {
            var op = pass.Operations[0];
            op.WarmupAttempts.ShouldBe(2);
            op.Successes.ShouldBe(3);
            op.P50Us.ShouldBe(1000);
            op.Seconds.ShouldBe(.006, .000001);
            op.OperationsPerSecond.ShouldBe(500, .0001);
        }
        fake.Calls.ShouldBe(10);
    }
    [Fact]
    public async Task RunAsync_AnalyticsDefaults_PerformsOneWarmupAndFiveSamples()
    {
        var clock = new FakeClock(); var fake = new FakeWorkload(clock);
        var passes = await new BenchmarkRunner(clock).RunAsync(fake, fake, FakeWorkload.Manifest,
            [new("op", "analytics", true, TimeoutClass.Analytics)], new() { Repeat = 1 });
        passes[0].Operations[0].WarmupAttempts.ShouldBe(1);
        passes[0].Operations[0].Successes.ShouldBe(5);
    }
    [Fact]
    public async Task RunAsync_TimeoutsAndFailures_NeverRecordsSamples()
    {
        var clock = new FakeClock(); var fake = new FakeWorkload(clock) { ExecutionCost = TimeSpan.FromMilliseconds(2) };
        var options = new RunnerOptions { Repeat = 1, Warmup = TimeSpan.Zero, Iterations = 3, PointTimeout = TimeSpan.FromMilliseconds(1) };
        var pass = (await new BenchmarkRunner(clock).RunAsync(fake, fake, FakeWorkload.Manifest, fake.Operations, options))[0];
        pass.Operations[0].Errors.ShouldBe(3);
        pass.Operations[0].Timeouts.ShouldBe(3);
        pass.Operations[0].Successes.ShouldBe(0);
        fake.Fail = true;
        pass = (await new BenchmarkRunner(clock).RunAsync(fake, fake, FakeWorkload.Manifest, fake.Operations,
            options with { PointTimeout = TimeSpan.FromSeconds(30) }))[0];
        pass.Operations[0].Errors.ShouldBe(3);
        pass.Operations[0].Timeouts.ShouldBe(0);
    }
    [Fact]
    public async Task RunAsync_Throughput_UsesDurationWithoutLatencyIterationCap()
    {
        var clock = new FakeClock(); var fake = new FakeWorkload(clock);
        var pass = (await new BenchmarkRunner(clock).RunAsync(fake, fake, FakeWorkload.Manifest, fake.Operations,
            new() { Suite = "throughput", Clients = [1], Repeat = 1, Warmup = TimeSpan.Zero, Duration = TimeSpan.FromMilliseconds(7) }))[0];
        pass.Operations[0].Successes.ShouldBe(7);
        pass.Operations[0].OperationsPerSecond.ShouldBe(1000, .001);
    }
    [Fact]
    public async Task RunAsync_UnsupportedAndCancellation_PreservesOutcomeWithoutExecuting()
    {
        var clock = new FakeClock(); var fake = new FakeWorkload(clock)
        { Support = new Dictionary<string, OperationSupport> { ["op"] = new(SupportLevel.Unsupported, "No feature", "source.cs:1") } };
        var pass = (await new BenchmarkRunner(clock).RunAsync(fake, fake, FakeWorkload.Manifest, fake.Operations, new() { Repeat = 1 }))[0];
        pass.Operations[0].Support.Evidence.ShouldBe("source.cs:1");
        fake.Calls.ShouldBe(0);
        await Should.ThrowAsync<OperationCanceledException>(() => new BenchmarkRunner(clock).RunAsync(fake, fake, FakeWorkload.Manifest,
            fake.Operations, new(), new CancellationToken(true)));
    }
    [Fact]
    public void Next_IdenticalKeys_ReproducesIndependentStreams()
    {
        var a = new DeterministicSequence(42, "op", 2); var b = new DeterministicSequence(42, "op", 2);
        var c = new DeterministicSequence(42, "op", 3);
        a.Next().ShouldNotBe(c.Next());
        b.Next();
        for (int i = 0; i < 100; i++) a.Next().ShouldBe(b.Next());
    }
}
