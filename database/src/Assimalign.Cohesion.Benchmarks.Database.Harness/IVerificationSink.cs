using System;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Receives complete logical rows, in the descriptor's total order when ordered. No calls in measured runs.</summary>
public interface IVerificationSink
{
    void AddRow(ReadOnlySpan<object?> values);
}
