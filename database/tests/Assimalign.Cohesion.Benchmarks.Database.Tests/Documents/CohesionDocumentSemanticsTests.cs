using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assimalign.Cohesion.Database;
using Assimalign.Cohesion.Database.Documents;
using Assimalign.Cohesion.Database.Execution;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Documents;

/// <summary>Exercises the pinned engine's ordinal index semantics and unsupported language surface.</summary>
public sealed class CohesionDocumentSemanticsTests
{
    [Fact]
    public async Task IndexedPrefix_MixedCaseNames_ReturnsOnlyOrdinalMatchesInTotalOrder()
    {
        await using var engine = DocumentDatabaseEngine.Create("prefix-test", new());
        var database = await engine.CreateDatabaseAsync("test");
        await using var session = await database.CreateSessionAsync();
        var products = await session.CreateCollectionAsync("products");
        string[] names = ["alpha", "Alpha", "ALPHA", "Alpha one", "Alpine", "Beta"];
        for (int i = 0; i < names.Length; i++)
            await products.PutAsync(session, "products/" + i, System.Text.Encoding.UTF8.GetBytes("{\"id\":\"products/" + i + "\",\"name\":\"" + names[i] + "\"}"));
        await session.ExecuteAsync("CREATE INDEX by_name ON products (name)");
        await using var result = (QueryResultSet)await session.ExecuteAsync("SELECT d.id, d.name FROM products d WHERE d.name >= @prefix AND d.name < @upper ORDER BY d.name, d.id",
            new Dictionary<string, object?> { ["prefix"] = "Alp", ["upper"] = "Alp\uffff" });
        var actual = new List<string>();
        await foreach (var row in result.GetRowsAsync()) actual.Add(row.GetString(1)!);
        actual.ShouldBe(["Alpha", "Alpha one", "Alpine"]);
    }
    [Theory]
    [InlineData("SELECT * FROM products LIMIT 1")]
    [InlineData("SELECT * FROM products p WHERE ANY v IN p.variants SATISFIES v.price <= 10 END")]
    public void Oql_UnsupportedPagingAndArrays_ReportsCapabilityDiagnostic(string statement)
    {
        var exception = Should.Throw<DatabaseParseException>(() => DocumentQueryRequest.FromOql(statement));
        exception.Message.ShouldContain("COHDBL001");
    }
    [Fact]
    public async Task Put_StaleExpectedVersion_RefusesLostUpdate()
    {
        await using var engine = DocumentDatabaseEngine.Create("version-test", new());
        var database = await engine.CreateDatabaseAsync("test");
        await using var session = await database.CreateSessionAsync();
        var collection = await session.CreateCollectionAsync("products");
        var original = await collection.PutAsync(session, "products/1", "{\"stock\":1}"u8.ToArray());
        await collection.PutAsync(session, "products/1", "{\"stock\":2}"u8.ToArray(), original.Version);
        await Should.ThrowAsync<DatabaseException>(async () => await collection.PutAsync(session, "products/1", "{\"stock\":3}"u8.ToArray(), original.Version));
    }
}
