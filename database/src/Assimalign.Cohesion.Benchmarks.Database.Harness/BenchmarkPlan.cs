using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Compiled model descriptors consumed by orchestration without duplicating the operation catalog.</summary>
public sealed record BenchmarkPlan(string Model, IReadOnlyList<TargetDescriptor> Targets,
    IReadOnlyList<OperationDescriptor> Operations,
    IReadOnlyDictionary<string, IReadOnlyDictionary<string, OperationSupport>> Support);
