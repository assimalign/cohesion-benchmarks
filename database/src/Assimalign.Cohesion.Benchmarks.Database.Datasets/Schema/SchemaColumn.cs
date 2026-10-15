namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>A logical column with portable size and nullability metadata.</summary>
public sealed record SchemaColumn(string Name, LogicalType Type, bool Nullable = false, int? Length = null, int? Precision = null, int? Scale = null);
