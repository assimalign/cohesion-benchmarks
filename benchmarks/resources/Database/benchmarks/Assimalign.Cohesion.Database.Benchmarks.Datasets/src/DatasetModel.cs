namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>
/// The database model a dataset is generated for.
/// </summary>
public enum DatasetModel
{
    /// <summary>The relational "commerce" dataset (CSV tables).</summary>
    Sql,

    /// <summary>The document "marketplace" dataset (JSON Lines collections).</summary>
    Documents,

    /// <summary>The graph "social network" dataset (CSV node and relationship files).</summary>
    Graph,
}
