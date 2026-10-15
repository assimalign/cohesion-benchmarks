using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>
/// Describes a generated dataset: what produced it and every file it contains. Stored as <c>manifest.json</c>
/// in the dataset directory. Contains no timestamps, so the same seed and scale always yield the same manifest.
/// </summary>
public sealed class DatasetManifest
{
    /// <summary>The generator's format version; bump it whenever generated values change.</summary>
    public required int GeneratorVersion { get; init; }

    /// <summary>The model.</summary>
    public required DatasetModel Model { get; init; }

    /// <summary>The dataset name: <c>commerce</c>, <c>marketplace</c> or <c>social</c>.</summary>
    public required string Name { get; init; }

    /// <summary>The scale factor.</summary>
    public required double Scale { get; init; }

    /// <summary>The seed.</summary>
    public required ulong Seed { get; init; }

    /// <summary>SHA-256 of binary part hashes concatenated in table and part order.</summary>
    public string Fingerprint { get; init; } = string.Empty;

    /// <summary>Every table, collection or graph file, in load order.</summary>
    public required IReadOnlyList<DatasetTableManifest> Tables { get; init; }
}

