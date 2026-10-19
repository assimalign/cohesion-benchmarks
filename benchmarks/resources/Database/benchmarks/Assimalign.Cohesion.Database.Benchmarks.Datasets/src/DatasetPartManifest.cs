using System.Collections.Generic;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

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
