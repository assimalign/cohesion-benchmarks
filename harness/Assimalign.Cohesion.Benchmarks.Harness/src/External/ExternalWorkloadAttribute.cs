using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;

namespace Assimalign.Cohesion.Benchmarks.External;

/// <summary>
/// Marks a benchmark class whose operation is macro-sized: launching a process or container, waiting for a server to
/// be ready, running a command to completion. It switches every job of the active profile to BenchmarkDotNet's
/// <see cref="RunStrategy.Monitoring"/> strategy (no pilot stage, one invocation per iteration, no overhead
/// subtraction), keeping the profile's warmup and iteration counts.
/// </summary>
/// <remarks>
/// Per-request benchmarks against an already-running target (a request to a server started in
/// <c>[GlobalSetup]</c>) do not need this: they are ordinary microsecond-scale benchmarks.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
public sealed class ExternalWorkloadAttribute : JobMutatorConfigBaseAttribute
{
    /// <summary>Creates the attribute.</summary>
    public ExternalWorkloadAttribute()
        : base(Job.Default.WithStrategy(RunStrategy.Monitoring))
    {
    }
}
