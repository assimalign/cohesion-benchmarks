namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Per-table ingest, finalization or fixed TCP sample. Sample metrics are excluded from full-load ratios.</summary>
public sealed record LoadMetric(string Table, string Phase, string Path, long Rows, long Bytes,
    double Seconds, bool IsSample = false)
{
    public double RowsPerSecond => Seconds > 0 ? Rows / Seconds : 0;
    public double MegabytesPerSecond => Seconds > 0 ? Bytes / 1_000_000d / Seconds : 0;
}
