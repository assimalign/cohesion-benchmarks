using System.Collections.Generic;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>A graph node label and its CSV properties, with id uniqueness represented explicitly.</summary>
public sealed record SchemaNode(string Label,string File,IReadOnlyList<SchemaColumn> Properties,IReadOnlyList<SchemaIndex> Indexes);
