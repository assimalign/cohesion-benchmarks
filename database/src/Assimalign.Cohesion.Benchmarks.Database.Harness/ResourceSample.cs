using System;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>One-second observation; null means unavailable, never zero usage. CPU is percent of all host CPUs.</summary>
public sealed record ResourceSample(DateTimeOffset Timestamp, double? HostCpuPercent,
    double? ContainerCpuPercent, string? ContainerMemory, string? Diagnostic);
