using System.Collections.Generic;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>A relational table in load order. Foreign keys are intentionally not physical constraints.</summary>
public sealed record SchemaTable(string Name, IReadOnlyList<SchemaColumn> Columns, IReadOnlyList<string> PrimaryKey, IReadOnlyList<SchemaIndex> Indexes)
{
    public string[] Header()
    {
        var result = new string[Columns.Count];
        for (int i = 0; i < result.Length; i++) result[i] = Columns[i].Name;
        return result;
    }
}
