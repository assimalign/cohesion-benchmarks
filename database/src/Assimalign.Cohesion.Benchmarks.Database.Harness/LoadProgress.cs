namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Out-of-band table progress for bulk ingestion.</summary>
public readonly record struct LoadProgress(string Table, long Rows, long Bytes);
