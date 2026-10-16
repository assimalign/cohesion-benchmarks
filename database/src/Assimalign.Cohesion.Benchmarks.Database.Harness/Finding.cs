namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>An actionable deficit or orchestration failure with its triggering numbers and evidence.</summary>
public sealed record Finding(string Model, string Cohort, string Target, string Kind, string Operation,
    string Evidence, double? Value = null, double? Best = null, double? Ratio = null, string? BestTarget = null);
