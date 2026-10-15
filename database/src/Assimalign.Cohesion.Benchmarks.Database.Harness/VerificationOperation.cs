using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Fixed-parameter digests in invocation order; errors invalidate verification.</summary>
public sealed record VerificationOperation(string Operation, OperationSupport Support,
    IReadOnlyList<RowsDigest> Cases, string? Error = null);
