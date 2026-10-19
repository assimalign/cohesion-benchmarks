using System.Collections.Generic;

using Assimalign.Cohesion.Database.Language;
using Assimalign.Cohesion.Database.Sql.Language;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Assimalign.Cohesion.Database.Benchmarks.Database.Sql.Language;

/// <summary>
/// Parsing SQL text into the statement tree, the first step of every ad-hoc statement the SQL engine executes:
/// a parameterized point lookup, a parameterized insert, and a filtered, ordered two-table join.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[BenchmarkCategory("SqlParse")]
public class SqlQueryParserBenchmarks
{
    private static readonly Dictionary<string, string> Statements = new()
    {
        ["PointSelect"] = "SELECT id, name, email FROM users WHERE id = @id;",
        ["Insert"] = "INSERT INTO users (id, name, email, age) VALUES (@id, @name, @email, @age);",
        ["Join"] = "SELECT users.name, orders.total FROM users INNER JOIN orders ON orders.user_id = users.id " +
            "WHERE users.age > 30 AND orders.total >= 100 ORDER BY orders.total DESC LIMIT 10;",
    };

    private readonly SqlQueryParser _parser = new();
    private string _sql = null!;

    [Params("PointSelect", "Insert", "Join")]
    public string Statement { get; set; } = "";

    [GlobalSetup]
    public void Setup()
    {
        _sql = Statements[Statement];

        // Fails the run up front if the dialect stops accepting the statement.
        _parser.Parse(_sql);
    }

    [Benchmark]
    public QueryStatement Parse() => _parser.Parse(_sql);
}
