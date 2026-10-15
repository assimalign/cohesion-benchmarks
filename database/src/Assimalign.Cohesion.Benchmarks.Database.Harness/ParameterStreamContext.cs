using System;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Deterministic mutation namespace, separate from the read-parameter seed. Adapters use WriteId for inserted keys.</summary>
public sealed record ParameterStreamContext(string Suite, int Repetition, int Concurrency, bool Warmup)
{
    /// <summary>Reserves a billion keys per logical client and scope, above the generated maximum.
    /// Independent of execution order and target speed. Reusing a complete run requires restoring the baseline.</summary>
    public long WriteId(long datasetMaximum, int clientIndex, long sequence)
    {
        if (Repetition is < 1 or > 100 || Concurrency is < 1 or > 1024 || clientIndex < 0 || clientIndex >= Concurrency ||
            sequence is < 0 or >= 1_000_000_000 || datasetMaximum < 0) throw new ArgumentOutOfRangeException(nameof(sequence));
        int suite = Suite switch
        {
            "verify" => 0, "latency" => 1, "throughput" => 2, "mixed/read-heavy" => 3, "mixed/balanced" => 4,
            _ => throw new ArgumentException("Unknown parameter scope."),
        };
        long partition = (((suite * 100L + Repetition - 1) * 1024 + Concurrency - 1) * 2 + (Warmup ? 1 : 0)) * 1024 + clientIndex;
        return checked(datasetMaximum + 1 + partition * 1_000_000_000 + sequence);
    }
}
