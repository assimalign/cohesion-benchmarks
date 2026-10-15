using System;
using System.Text;
using System.Threading.Tasks;
using Assimalign.Cohesion.Database.Execution;
using Assimalign.Cohesion.Database.Sql;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Sql;

public sealed class CohesionCapabilityTests(ITestOutputHelper output)
{
    [Fact]
    public async Task Execute_CapabilityBoundaries_ReturnsExplicitEvidence()
    {
        var builder = SqlDatabaseEngine.CreateBuilder("sql-capability-test");
        builder.AddDatabase("benchmark");
        await using var engine = await builder.BuildAsync();
        engine.TryGetDatabase("benchmark", out SqlDatabase? database).ShouldBeTrue();
        await using var session = await database!.CreateSessionAsync();
        foreach (string name in new[] { "a", "b", "c" })
        {
            (await session.ExecuteAsync($"CREATE TABLE {name} (id BIGINT PRIMARY KEY, happened TIMESTAMP)")).Status.ShouldBe(QueryResultStatus.Success);
            (await session.ExecuteAsync($"INSERT INTO {name} VALUES (1,'2025-01-01T00:00:00')")).Status.ShouldBe(QueryResultStatus.Success);
        }
        foreach (string sql in new[]
        {
            "SELECT a.id FROM a INNER JOIN b ON a.id = b.id INNER JOIN c ON a.id = c.id",
            "SELECT EXTRACT(YEAR FROM happened) FROM a",
            "SELECT id FROM a WHERE id > (SELECT AVG(b.id) FROM b WHERE b.id = a.id)",
            "SELECT RANK() OVER (PARTITION BY id ORDER BY happened) FROM a",
            "WITH x AS (SELECT id FROM a) SELECT id FROM x"
        })
        {
            string evidence;
            try
            {
                var result = await session.ExecuteAsync(sql);
                var text = new StringBuilder();
                if (result.Diagnostics != null) foreach (var diagnostic in result.Diagnostics) text.Append(diagnostic.Code).Append(": ").AppendLine(diagnostic.Message);
                if (result is QueryResultSet rows)
                {
                    await using (rows) { await foreach (var row in rows.GetRowsAsync()) _ = row.GetValue(0); }
                }
                evidence = text.ToString();
            }
            catch (Exception ex) { evidence = ex.GetType().Name + ": " + ex.Message; }
            output.WriteLine(sql + "\n" + evidence);
            evidence.ShouldNotBeNullOrWhiteSpace();
        }
        await using var uncorrelated = (QueryResultSet)await session.ExecuteAsync("SELECT id FROM a WHERE id NOT IN (SELECT id FROM b) ORDER BY id");
        int count = 0;
        await foreach (var row in uncorrelated.GetRowsAsync()) count++;
        count.ShouldBe(0);
    }
}
