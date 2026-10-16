using System;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Suite controls. Null latency overrides select the design's separate point and analytics defaults.</summary>
public sealed record RunnerOptions
{
    public string Suite { get; init; } = "latency";
    public string Mix { get; init; } = "read-heavy";
    public int[] Clients { get; init; } = [1, 8, 32];
    public int Repeat { get; init; } = 1;
    public ulong Seed { get; init; } = 20261015;
    public TimeSpan? Warmup { get; init; }
    public TimeSpan? Duration { get; init; }
    public int? Iterations { get; init; }
    public int AnalyticsWarmupIterations { get; init; } = 1;
    public int AnalyticsIterations { get; init; } = 3;
    public TimeSpan PointTimeout { get; init; } = TimeSpan.FromSeconds(30);
    public TimeSpan AnalyticsTimeout { get; init; } = TimeSpan.FromSeconds(600);
    public string? Container { get; init; }
    public void Validate()
    {
        if (Suite is not ("latency" or "throughput" or "mixed") || Mix is not ("read-heavy" or "balanced"))
            throw new ArgumentException("Unknown suite or mix.");
        if (Repeat is < 1 or > 100 || Clients.Length == 0 || Array.Exists(Clients, n => n is < 1 or > 1024) ||
            Warmup < TimeSpan.Zero || Duration <= TimeSpan.Zero || Iterations < 1 ||
            AnalyticsWarmupIterations < 0 || AnalyticsIterations < 1 ||
            PointTimeout <= TimeSpan.Zero || AnalyticsTimeout <= TimeSpan.Zero)
            throw new ArgumentException("Invalid runner limits.");
        if (Suite != "latency" && Iterations != null) throw new ArgumentException("Iteration limits apply only to latency.");
    }
}
