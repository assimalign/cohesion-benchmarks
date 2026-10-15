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

    /// <summary>Every table, collection or graph file, in load order.</summary>
    public required IReadOnlyList<DatasetTableManifest> Tables { get; init; }
}

/// <summary>
/// One table, collection or graph file of a dataset.
/// </summary>
public sealed class DatasetTableManifest
{
    /// <summary>The table name (for example <c>orders</c>, <c>products</c>, <c>person_knows_person</c>).</summary>
    public required string Name { get; init; }

    /// <summary>Total rows, documents, nodes or relationships.</summary>
    public required long Rows { get; init; }

    /// <summary>Total bytes across all parts.</summary>
    public required long Bytes { get; init; }

    /// <summary>The part files, in order.</summary>
    public required IReadOnlyList<DatasetPartManifest> Parts { get; init; }
}

/// <summary>
/// One part file of a table.
/// </summary>
public sealed class DatasetPartManifest
{
    /// <summary>The path relative to the dataset directory, with forward slashes.</summary>
    public required string Path { get; init; }

    /// <summary>Rows in this part.</summary>
    public required long Rows { get; init; }

    /// <summary>Bytes in this part.</summary>
    public required long Bytes { get; init; }

    /// <summary>Lower-case hexadecimal SHA-256 of the part's bytes.</summary>
    public required string Sha256 { get; init; }
}
