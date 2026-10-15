namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Identity and ranking policy; ServedDataTarget identifies an embedded target whose files this target serves.</summary>
public sealed record TargetDescriptor(string Id, string DisplayName, bool Ranked, string Label,
    string Engine, string? ServedDataTarget = null);
