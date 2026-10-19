using System.Collections.Generic;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>A document collection's stable key format and logical secondary index paths.</summary>
public sealed record SchemaCollection(string Name, string KeyFormat, IReadOnlyList<SchemaIndex> Indexes);
