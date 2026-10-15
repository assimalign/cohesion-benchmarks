namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>An actionable Cohesion deficit with its triggering numbers or support/verification evidence.</summary>
public sealed record Finding(string Model, string Cohort, string Target, string Kind, string Operation,
    string Evidence, double? Value = null, double? Best = null, double? Ratio = null, string? BestTarget = null);
