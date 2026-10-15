using System;
using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Versioned result for one target, suite and repetition. Verification travels with measured evidence.</summary>
public sealed record BenchmarkResult
{
    public int SchemaVersion { get; init; } = 1;
    public required string RunId { get; init; }
    public required string Model { get; init; }
    public required TargetDescriptor Target { get; init; }
    public required string Suite { get; init; }
    public string? Mix { get; init; }
    public int Repetition { get; init; }
    public bool Official { get; init; }
    public DateTimeOffset StartedUtc { get; init; }
    public DateTimeOffset FinishedUtc { get; init; }
    public required EngineDescription Engine { get; init; }
    public required EnvironmentSnapshot Environment { get; init; }
    public required string DatasetManifestSha256 { get; init; }
    public double Scale { get; init; }
    public ulong Seed { get; init; }
    public ResourceBudget Budget { get; init; } = new();
    public RunnerOptions? Runner { get; init; }
    public IReadOnlyList<LoadMetric> Load { get; init; } = [];
    public long? StorageBytes { get; init; }
    public IReadOnlyList<OperationMeasurement> Operations { get; init; } = [];
    public IReadOnlyList<ResourceSample> Samples { get; init; } = [];
    public VerificationFile? Verification { get; init; }
}
