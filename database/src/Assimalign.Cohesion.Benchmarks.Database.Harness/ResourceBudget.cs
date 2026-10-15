namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Resource budget attached to a run; observed container limits are captured independently.</summary>
public sealed record ResourceBudget(double Cpus = 3, long MemoryBytes = 6L * 1024 * 1024 * 1024,
    long DriverMemoryBytes = 3L * 1024 * 1024 * 1024);
