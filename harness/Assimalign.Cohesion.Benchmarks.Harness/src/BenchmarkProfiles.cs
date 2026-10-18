using System;
using System.IO;
using System.Reflection;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace Assimalign.Cohesion.Benchmarks;

/// <summary>
/// The run profiles. A profile picks the measurement job; every profile measures allocations and writes the
/// GitHub markdown table plus the full JSON report that CI turns into history.
/// </summary>
/// <remarks>
/// <para>
/// Suites run <b>in-process</b> by default (BenchmarkDotNet's <see cref="InProcessEmitToolchain"/>: the same
/// emitted measurement harness, in the already-built benchmark process). BenchmarkDotNet's default toolchain
/// rebuilds the suite in a generated child project, and cohesion's project graph does not rebuild reliably there:
/// its analyzer and shared-source references evaluate some projects more than once under different global
/// properties, and those instances race on shared output files. In-process also skips a multi-minute rebuild per
/// suite, which matters in CI.
/// </para>
/// <para>
/// <c>--out-of-process</c> restores the default toolchain for a suite whose graph rebuilds cleanly (for example
/// to compare GC modes in separate processes); the measured cohesion checkout is then forwarded to the child build.
/// </para>
/// </remarks>
public static class BenchmarkProfiles
{
    /// <summary>BenchmarkDotNet's default job: as many iterations as it takes to be stable. For investigations.</summary>
    public const string Local = "local";

    /// <summary>
    /// A fixed, bounded job for CI history: one launch, 5 warmups, 20 measured iterations. Fixed counts keep run
    /// time predictable and make consecutive runs comparable.
    /// </summary>
    public const string Ci = "ci";

    /// <summary>One invocation per benchmark: proves every benchmark builds and runs. Numbers are meaningless.</summary>
    public const string Dry = "dry";

    /// <summary>Creates the configuration for <paramref name="profile"/>.</summary>
    /// <param name="profile">One of <see cref="Local"/>, <see cref="Ci"/>, <see cref="Dry"/>.</param>
    /// <param name="assembly">The benchmark assembly.</param>
    /// <param name="addProfileJob">False when the command line already chose a job (<c>--job</c>, <c>--inProcess</c>).</param>
    /// <param name="outOfProcess">True to rebuild and run the suite in a child process (BenchmarkDotNet's default toolchain).</param>
    /// <returns>The configuration.</returns>
    public static IConfig CreateConfig(string profile, Assembly assembly, bool addProfileJob = true, bool outOfProcess = false)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(assembly);

        string artifacts = Path.Combine(
            BenchmarkEnvironment.GetBenchmarksRoot(assembly), ".artifacts", "results", assembly.GetName().Name!);

        ManualConfig config = ManualConfig.Create(DefaultConfig.Instance)
            .WithArtifactsPath(artifacts)
            .AddDiagnoser(MemoryDiagnoser.Default)
            .AddExporter(JsonExporter.Full)
            .AddExporter(MarkdownExporter.GitHub)
            .WithOptions(ConfigOptions.DisableLogFile);

        if (addProfileJob)
        {
            config.AddJob(CreateJob(profile, assembly, outOfProcess));
        }

        return config;
    }

    /// <summary>Creates the job for <paramref name="profile"/>.</summary>
    /// <param name="profile">The profile name.</param>
    /// <param name="assembly">The benchmark assembly.</param>
    /// <param name="outOfProcess">True to use BenchmarkDotNet's default (child process) toolchain.</param>
    /// <returns>The job.</returns>
    public static Job CreateJob(string profile, Assembly assembly, bool outOfProcess = false)
    {
        Job job = profile.ToLowerInvariant() switch
        {
            Local => Job.Default,
            Ci => Job.Default.WithLaunchCount(1).WithWarmupCount(5).WithIterationCount(20),
            Dry => Job.Dry,
            _ => throw new ArgumentException($"Unknown benchmark profile '{profile}'. Use {Local}, {Ci} or {Dry}.", nameof(profile)),
        };

        job = job.WithId(profile.ToLowerInvariant());

        if (!outOfProcess)
        {
            return job.WithToolchain(InProcessEmitToolchain.Instance);
        }

        // The child project rebuilds the suite: forward the measured checkout, or it would measure the default
        // sibling checkout instead, and build on one node to keep duplicate project instances from racing.
        return job.WithArguments(
        [
            new MsBuildArgument($"/p:CohesionRoot={BenchmarkEnvironment.GetCohesionRoot(assembly)}"),
            new MsBuildArgument("/m:1"),
        ]);
    }
}
