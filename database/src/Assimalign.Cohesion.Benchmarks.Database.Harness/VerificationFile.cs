using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>One target's fixed 100-case verification pass.</summary>
public sealed record VerificationFile
{
    public int SchemaVersion { get; init; } = 1;
    public required string Model { get; init; }
    public required string Target { get; init; }
    public required string DatasetManifestSha256 { get; init; }
    public ulong Seed { get; init; }
    public int ParameterCount { get; init; } = 100;
    public required IReadOnlyList<VerificationOperation> Operations { get; init; }
}
