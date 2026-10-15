using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

/// <summary>Immutable workload choices and fully constructed query text, made before an operation is timed.</summary>
public sealed record GraphParameters(string Cypher, Dictionary<string, object?> Values, string[] Gql,
    bool PathRows = false, string? CleanupCypher = null, string? CleanupGql = null);
