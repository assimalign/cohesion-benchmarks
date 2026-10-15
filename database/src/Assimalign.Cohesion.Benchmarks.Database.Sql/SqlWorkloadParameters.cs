using System;
using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

/// <summary>Complete prebuilt requests and post-pass restoration commands for one logical operation.</summary>
public sealed class SqlWorkloadParameters : IDisposable
{
    public required SqlStatement[] Statements { get; init; }
    public bool Transaction { get; init; }
    public SqlStatement[] Verification { get; init; } = [];
    public SqlStatement[] Restore { get; init; } = [];
    internal SqlWorkloadParameters? PreviousMutation { get; set; }
    internal MonthlyAggregation? Monthly { get; init; }
    public void Dispose()
    {
        foreach (SqlStatement statement in Statements) statement.Dispose();
        foreach (SqlStatement statement in Verification) statement.Dispose();
        foreach (SqlStatement statement in Restore) statement.Dispose();
    }
}
