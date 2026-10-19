using System.Collections.Generic;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

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

    /// <summary>Largest generated entity id (or owning entity id for composite-key child tables).</summary>
    public long MaximumId { get; init; }

    /// <summary>The part files, in order.</summary>
    public required IReadOnlyList<DatasetPartManifest> Parts { get; init; }
}
