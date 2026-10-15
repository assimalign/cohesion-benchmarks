namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Portable semantics and mix weights. Each mix's read/write weights must total the specified ratio.</summary>
public sealed record OperationDescriptor(string Id, string Category, bool Ordered, TimeoutClass TimeoutClass,
    int ReadHeavyWeight = 0, int BalancedWeight = 0);
