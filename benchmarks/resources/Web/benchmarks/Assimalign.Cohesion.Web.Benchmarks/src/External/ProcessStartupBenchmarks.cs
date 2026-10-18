using System.Collections.Generic;

using Assimalign.Cohesion.Benchmarks.External;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Web.Benchmarks.External;

/// <summary>
/// Macro-sized out-of-process work: how long a server takes from launch to answering its first request, and how
/// long a command takes to run to exit. Each iteration starts a new process (<see cref="ExternalWorkloadAttribute"/>).
/// </summary>
[ExternalWorkload]
public class ProcessStartupBenchmarks
{
    public IEnumerable<string> Servers => WebTargets.Catalog.AvailableIds(ExternalTargetKind.Process);

    public IEnumerable<string> Commands => WebTargets.Catalog.AvailableIds(ExternalTargetKind.Command);

    /// <summary>Launch to the first successful HTTP response (the target's readiness probe), then stop.</summary>
    [Benchmark]
    [ArgumentsSource(nameof(Servers))]
    public double StartUntilReady(string server)
    {
        using ExternalTargetInstance instance = WebTargets.Catalog.Start(server);
        return instance.StartupTime.TotalMilliseconds;
    }

    /// <summary>Run a command target to exit.</summary>
    [Benchmark]
    [ArgumentsSource(nameof(Commands))]
    public int RunToExit(string command) => WebTargets.Catalog.Run(command);
}
