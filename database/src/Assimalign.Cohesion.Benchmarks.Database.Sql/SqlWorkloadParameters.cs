using System;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

/// <summary>Complete prebuilt requests and verification reads for one logical operation.</summary>
public sealed class SqlWorkloadParameters : IDisposable
{
    public required SqlStatement[] Statements { get; init; }
    public bool Transaction { get; init; }
    public SqlStatement[] Verification { get; init; } = [];
    internal MonthlyAggregation? Monthly { get; init; }
    public void Dispose()
    {
        foreach (SqlStatement statement in Statements) statement.Dispose();
        foreach (SqlStatement statement in Verification) statement.Dispose();
    }
}
