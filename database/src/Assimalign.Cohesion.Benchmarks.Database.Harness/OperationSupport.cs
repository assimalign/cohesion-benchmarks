using System;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Explicit support, including bounded client completion or an unsupported reason and evidence.</summary>
public sealed record OperationSupport(SupportLevel Level, string? Reason = null, string? Evidence = null)
{
    public void Validate()
    {
        if (!Enum.IsDefined(Level)) throw new ArgumentException("Unknown support level.");
        if (Level != SupportLevel.Native && string.IsNullOrWhiteSpace(Reason))
            throw new ArgumentException("Emulated and unsupported operations need a reason.");
        if (Level == SupportLevel.Unsupported && string.IsNullOrWhiteSpace(Evidence))
            throw new ArgumentException("Unsupported operations need evidence.");
    }
}
