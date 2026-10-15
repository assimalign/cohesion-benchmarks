namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Observed optimization flags and the informational version of an actually loaded Cohesion assembly.</summary>
public sealed record OptimizationAssembly(string Name, bool Optimized, string InformationalVersion, string? Location);
