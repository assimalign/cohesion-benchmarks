using System.Collections.Generic;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>An ordered logical index, including uniqueness and array expansion semantics.</summary>
public sealed record SchemaIndex(string Name, IReadOnlyList<string> Columns, bool Unique = false);
