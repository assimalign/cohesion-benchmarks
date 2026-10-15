using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>A typed relationship file. Its first two fields reference the indicated source and destination labels.</summary>
public sealed record SchemaRelationship(string Type,string File,string SourceLabel,string DestinationLabel,IReadOnlyList<SchemaColumn> Properties);
