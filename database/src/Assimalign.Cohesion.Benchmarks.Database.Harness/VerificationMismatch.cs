namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>A blocking verification failure with the target pair and precise evidence.</summary>
public sealed record VerificationMismatch(string Model, string Operation, string LeftTarget, string RightTarget, string Evidence);
