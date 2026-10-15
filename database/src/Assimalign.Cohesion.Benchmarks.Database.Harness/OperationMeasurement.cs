using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Successful samples and separate warm-up/measured failure accounting.</summary>
public sealed record OperationMeasurement
{
    public required string Operation { get; init; }
    public required OperationSupport Support { get; init; }
    public int Clients { get; init; }
    public long Successes { get; init; }
    public long Errors { get; init; }
    public long Timeouts { get; init; }
    public long WarmupAttempts { get; init; }
    public long WarmupErrors { get; init; }
    public double Seconds { get; init; }
    public double OperationsPerSecond => Seconds > 0 ? Successes / Seconds : 0;
    public long P50Us { get; init; }
    public long P90Us { get; init; }
    public long P95Us { get; init; }
    public long P99Us { get; init; }
    public long P999Us { get; init; }
    public long MaxUs { get; init; }
    public string HistogramEncoding { get; init; } = "dbbench-hdr-v1";
    public string? Histogram { get; init; }
    public IReadOnlyList<string> ErrorExamples { get; init; } = [];
}
