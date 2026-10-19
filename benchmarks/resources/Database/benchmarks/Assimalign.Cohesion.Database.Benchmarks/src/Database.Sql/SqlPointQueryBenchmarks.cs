using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Assimalign.Cohesion.Database.Execution;
using Assimalign.Cohesion.Database.Sql;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Microsoft.Data.Sqlite;

namespace Assimalign.Cohesion.Database.Benchmarks.Database.Sql;

/// <summary>
/// A parameterized primary-key lookup through an embedded, in-memory SQL engine, as an application issues it: SQL
/// text in, one row out (parse, bind, plan, execute, read the row). The table holds 1,000 rows. SQLite through
/// Microsoft.Data.Sqlite, the usual in-process SQL engine for .NET, runs the same statement against the same data
/// and is the baseline, so the Ratio column reads "Cohesion relative to the counterpart".
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[BenchmarkCategory("SqlPointQuery")]
public class SqlPointQueryBenchmarks
{
    private const string Query = "SELECT name FROM users WHERE id = @id;";
    private const int RowCount = 1000;
    private const long LookupId = 500;

    private SqlDatabaseEngine _engine = null!;
    private SqlDatabaseSession _session = null!;
    private Dictionary<string, object?> _parameters = null!;
    private SqliteConnection _sqlite = null!;

    [GlobalSetup]
    public void Setup()
    {
        _engine = SqlDatabaseEngine.Create("benchmarks", new SqlDatabaseEngineOptions());
        SqlDatabase database = _engine.CreateDatabaseAsync("benchmarks").AsTask().GetAwaiter().GetResult();
        _session = database.CreateSessionAsync().AsTask().GetAwaiter().GetResult();
        _parameters = new Dictionary<string, object?> { ["id"] = LookupId };

        _sqlite = new SqliteConnection("Data Source=:memory:");
        _sqlite.Open();

        Execute("CREATE TABLE users (id BIGINT PRIMARY KEY, name VARCHAR(100), age INT);");
        for (int start = 1; start <= RowCount; start += 100)
        {
            StringBuilder insert = new("INSERT INTO users (id, name, age) VALUES ");
            for (int id = start; id < start + 100; id++)
            {
                if (id != start)
                {
                    insert.Append(", ");
                }

                insert.Append('(').Append(id).Append(", 'user-").Append(id).Append("', ").Append(20 + (id % 50)).Append(')');
            }

            Execute(insert.Append(';').ToString());
        }

        string expected = $"user-{LookupId}";
        if (SelectById().AsTask().GetAwaiter().GetResult() != expected || SelectById_Sqlite() != expected)
        {
            throw new InvalidOperationException($"The seeded row '{expected}' was not found.");
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _sqlite.Dispose();
        _session.DisposeAsync().AsTask().GetAwaiter().GetResult();
        _engine.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    [Benchmark(Baseline = true)]
    public string? SelectById_Sqlite()
    {
        using SqliteCommand command = _sqlite.CreateCommand();
        command.CommandText = Query;
        command.Parameters.AddWithValue("@id", LookupId);
        using SqliteDataReader reader = command.ExecuteReader();
        return reader.Read() ? reader.GetString(0) : null;
    }

    [Benchmark]
    public async ValueTask<string?> SelectById()
    {
        DatabaseResult result = await _session.ExecuteAsync(Query, _parameters).ConfigureAwait(false);
        await using DatabaseResultSet rows = (DatabaseResultSet)result;
        await foreach (DatabaseRow row in rows.GetRowsAsync().ConfigureAwait(false))
        {
            return row.GetString(0);
        }

        return null;
    }

    private void Execute(string sql)
    {
        _session.ExecuteAsync(sql).AsTask().GetAwaiter().GetResult();

        using SqliteCommand command = _sqlite.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }
}
