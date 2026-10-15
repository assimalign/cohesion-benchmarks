using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Npgsql;
using WireCommand = Assimalign.Cohesion.Database.Sql.Client.SqlCommand;
using TsqlCommand = Microsoft.Data.SqlClient.SqlCommand;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

/// <summary>Commands and bindings constructed by the parameter stream, before the operation timer.</summary>
public sealed class SqlStatement : IDisposable
{
    internal string Text { get; }
    internal IReadOnlyDictionary<string, object?> Parameters { get; }
    internal NpgsqlCommand PostgreSql { get; }
    internal TsqlCommand SqlServer { get; }
    internal WireCommand Cohesion { get; }

    public SqlStatement(string text, IReadOnlyDictionary<string, object?>? parameters = null, string? tsql = null, string? postgresql = null)
    {
        Text = text;
        Parameters = parameters ?? Empty;
        PostgreSql = new(postgresql ?? text) { CommandTimeout = 600 };
        SqlServer = new(tsql ?? text) { CommandTimeout = 600 };
        Cohesion = new(text);
        foreach (var pair in Parameters)
        {
            Add(PostgreSql, pair.Key, pair.Value);
            Add(SqlServer, pair.Key, pair.Value);
            Cohesion.WithParameter(pair.Key, pair.Value);
        }
    }

    private static readonly Dictionary<string, object?> Empty = new();
    private static void Add(DbCommand command, string name, object? value)
    {
        DbParameter parameter = command.CreateParameter();
        parameter.ParameterName = name;
        // SQL TIMESTAMP is UTC without a zone on all three targets.
        parameter.Value = value is DateTime time ? DateTime.SpecifyKind(time, DateTimeKind.Unspecified) : value ?? DBNull.Value;
        if (value is string) parameter.DbType = DbType.AnsiString;
        command.Parameters.Add(parameter);
    }

    public void Dispose() { PostgreSql.Dispose(); SqlServer.Dispose(); }
}
