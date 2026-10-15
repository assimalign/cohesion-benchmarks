namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>SHA-256 of canonical rows plus their count.</summary>
public sealed record RowsDigest(long RowCount, string Sha256);
