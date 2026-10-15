using System;
using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Machine, provenance, container metadata and the optimization evidence used by an official run.</summary>
public sealed record EnvironmentSnapshot
{
    public int SchemaVersion { get; init; } = 1;
    public DateTimeOffset CapturedUtc { get; init; }
    public string OperatingSystem { get; init; } = "";
    public string Architecture { get; init; } = "";
    public string Cpu { get; init; } = "";
    public int LogicalProcessors { get; init; }
    public long? PhysicalMemoryBytes { get; init; }
    public string? BenchmarksCommit { get; init; }
    public string? CohesionCheckoutCommit { get; init; }
    public IReadOnlyList<string> CohesionBuiltCommits { get; init; } = [];
    public IReadOnlyList<OptimizationAssembly> Optimization { get; init; } = [];
    public string? Container { get; init; }
    public string? ContainerInspectJson { get; init; }
    public string? ImageDigest { get; init; }
    public string? CgroupMemoryMax { get; init; }
    public string? CgroupCpuMax { get; init; }
    public string? Diagnostics { get; init; }
}
