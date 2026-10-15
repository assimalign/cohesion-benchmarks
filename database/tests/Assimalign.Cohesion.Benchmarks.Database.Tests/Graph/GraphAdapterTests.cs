using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Graph;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Assimalign.Cohesion.Database;
using Assimalign.Cohesion.Database.Execution;
using Assimalign.Cohesion.Database.Graph;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Graph;

/// <summary>Semantic adapter checks against the pinned real embedded engine, plus portable mapping invariants.</summary>
public sealed class GraphAdapterTests
{
    private readonly ITestOutputHelper output;
    public GraphAdapterTests(ITestOutputHelper output) => this.output = output;
    private const string Seed = "INSERT (c:Place {id:1,name:'City',type:'City'})-[:IS_PART_OF]->(:Place {id:2,name:'Country',type:'Country'}), " +
        "(a:Person {id:1,firstName:'Alice',lastName:'A'})-[:IS_LOCATED_IN]->(c), " +
        "(b:Person {id:2,firstName:'Bob',lastName:'B'})-[:IS_LOCATED_IN]->(c), " +
        "(d:Person {id:3,firstName:'Bob',lastName:'C'})-[:IS_LOCATED_IN]->(c), " +
        "(a)-[:KNOWS {creationDate:10}]->(b),(b)-[:KNOWS {creationDate:20}]->(d), " +
        "(m:Post {id:1,creationDate:100,content:'post'})-[:HAS_CREATOR]->(b), " +
        "(q:Comment {id:1,creationDate:100,content:'comment'})-[:HAS_CREATOR]->(b), " +
        "(n:Post {id:2,creationDate:200,content:'newer'})-[:HAS_CREATOR]->(d), " +
        "(t:Tag {id:1,name:'tag1'}),(u:Tag {id:2,name:'tag2'}),(m)-[:HAS_TAG]->(t),(m)-[:HAS_TAG]->(u)";

    [Fact]
    public void ReadValue_TemporalProperties_UsesIdenticalEpochMilliseconds()
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("birthday,creationDate\n1969-12-31,2026-01-01 00:00:00\n"));
        using var reader = new CsvReader(stream);
        reader.Read().ShouldBeTrue();
        GraphMapping.ReadValue(reader, 0, new("birthday", LogicalType.Date)).ShouldBe(-86_400_000L);
        GraphMapping.ReadValue(reader, 1, new("creationDate", LogicalType.Timestamp)).ShouldBe(1_767_225_600_000L);
        foreach (var node in SocialSchema.Nodes)
        {
            GraphMapping.Neo4jHeader(node)[0].ShouldContain("id:ID(" + node.Label + ")");
            foreach (var header in GraphMapping.Neo4jHeader(node)) header.ShouldNotContain(":date");
        }
        GraphMapping.Neo4jIndexes().Count().ShouldBe(11);
        GraphMapping.Neo4jIndexes().Count(s => s.Contains("CONSTRAINT", StringComparison.Ordinal)).ShouldBe(8);
    }

    [Fact]
    public async Task Literal_QuotesUnicodeAndBackslashes_RoundTripsThroughEngine()
    {
        await using var engine = GraphDatabaseEngine.Create("literal-test", new());
        var db = await engine.CreateDatabaseAsync("test");
        await using var session = await db.CreateSessionAsync();
        const string input = "O'Brien \\東京\n\"quoted\"; MATCH (n) DELETE n";
        await session.ExecuteAsync("INSERT (:Literal " + GraphMapping.Properties(new Dictionary<string, object?> { ["text"] = input }) + ")");
        var rows = await RowsAsync(session, "MATCH (n:Literal) RETURN n.text");
        rows.Single()[0].ShouldBe(input);
    }

    [Theory]
    [InlineData("MATCH (n) RETURN n.id ORDER BY n.id", "COHDBL001")]
    [InlineData("MATCH (n) RETURN n.id LIMIT 10", "COHDBL001")]
    [InlineData("MATCH (n) RETURN count(n)", "COHDBL001")]
    [InlineData("MATCH (n {id:$id}) RETURN n.id", "COHDBL001")]
    [InlineData("MATCH (n)-[:KNOWS*1..3]-(m) RETURN m.id", "COHDBL001")]
    [InlineData("MATCH ANY SHORTEST (n)-[:KNOWS]-(m) RETURN n", "COHDBL001")]
    [InlineData("MATCH (n),(m) WHERE NOT EXISTS { MATCH (n)-[:KNOWS]-(m) } RETURN m.id", "COHDBL001")]
    public async Task Execute_UnsupportedLanguage_ReturnsActualCapabilityError(string query, string code)
    {
        await using var engine = GraphDatabaseEngine.Create("capability-test", new());
        var db = await engine.CreateDatabaseAsync("test");
        await using var session = await db.CreateSessionAsync();
        var error = await Should.ThrowAsync<DatabaseParseException>(async () => await session.ExecuteAsync(query));
        error.Message.ShouldContain(code);
        output.WriteLine(error.Message);
    }

    [Fact]
    public async Task Traverse_DifferentShortestDistances_HasIndistinguishableNodeProjection()
    {
        async Task<List<long>> Traverse(bool star)
        {
            await using var engine = GraphDatabaseEngine.Create("traversal-test", new());
            var database = await engine.CreateDatabaseAsync("test");
            await using var session = await database.CreateSessionAsync();
            var a = await database.CreateNodeAsync(session, ["Person"], new Dictionary<string, object?> { ["id"] = 1L });
            var b = await database.CreateNodeAsync(session, ["Person"], new Dictionary<string, object?> { ["id"] = 2L });
            var c = await database.CreateNodeAsync(session, ["Person"], new Dictionary<string, object?> { ["id"] = 3L });
            await database.CreateRelationshipAsync(session, a.Id, b.Id, "KNOWS");
            await database.CreateRelationshipAsync(session, star ? a.Id : b.Id, c.Id, "KNOWS");
            var ids = new List<long>();
            await foreach (var node in database.TraverseAsync(session, new(a.Id, GraphDirection.Both, "KNOWS", 3))) ids.Add((long)node.Properties["id"]!);
            return ids;
        }
        // Distance from 1 to 3 is one in the star and two in the chain. The public result loses that fact.
        (await Traverse(true)).ShouldBe(new long[] { 2, 3 });
        (await Traverse(false)).ShouldBe(new long[] { 2, 3 });
    }

    [Fact]
    public async Task Execute_SelectedPathsAndAggregates_PreservesExactSemantics()
    {
        await using var engine = GraphDatabaseEngine.Create("semantic-test", new());
        var db = await engine.CreateDatabaseAsync("test");
        await using (var setup = await db.CreateSessionAsync()) await setup.ExecuteAsync(Seed);
        var model = ModelRegistration.Create();
        await using var client = new CohesionBenchmarkClient(await db.CreateSessionAsync());

        async Task<List<object?[]>> Execute(string operation, long person = 1)
        {
            var sink = new Rows();
            var descriptor = model.Operations.Single(o => o.Id == operation);
            await client.ExecuteAsync(descriptor, GraphQueries.Create(operation, person, 2, 1, 1, "Bob", 999), sink, CancellationToken.None);
            return sink.Values;
        }

        (await Execute("graph.person-profile")).Single()[10].ShouldBe(1L);
        (await Execute("graph.person-friends")).Single().ShouldBe(new object?[] { 2L, "Bob", "B", 10L });
        var recent = await Execute("graph.person-recent-messages", 2);
        recent.Count.ShouldBe(2);
        recent[0].ShouldBe(new object?[] { "Comment", 1L, 100L, 2L, "comment" });
        recent[1].ShouldBe(new object?[] { "Post", 1L, 100L, 2L, "post" });
        (await Execute("graph.friends-recent-posts")).Single()[1].ShouldBe(1L);
        var fof = await Execute("graph.fof-recent-messages");
        fof.Count.ShouldBe(3); fof[0][1].ShouldBe(2L);
        var names = await Execute("graph.friends-within-3-by-name");
        names.Count.ShouldBe(2); names[0][3].ShouldBe(1L); names[1][3].ShouldBe(2L);
        (await Execute("graph.tag-cooccurrence")).Single().ShouldBe(new object?[] { 2L, "tag2", 1L });
        (await Execute("graph.country-person-count")).Single().ShouldBe(new object?[] { 2L, "Country", 3L });
    }

    [Theory]
    [InlineData("graph.add-post")]
    [InlineData("graph.add-like")]
    [InlineData("graph.add-friendship")]
    public async Task Execute_Write_ReturnsPersistedValuesAndRestoresBaseline(string operation)
    {
        await using var engine = GraphDatabaseEngine.Create("write-test", new());
        var db = await engine.CreateDatabaseAsync("test");
        await using var setup = await db.CreateSessionAsync();
        await setup.ExecuteAsync(Seed);
        string baseline = "MATCH (p:Person)-[r:KNOWS]-(f:Person) RETURN p.id,f.id,r.creationDate";
        int before = (await RowsAsync(setup, baseline)).Count;
        var sink = new Rows();
        await using (var client = new CohesionBenchmarkClient(await db.CreateSessionAsync()))
        {
            var model = ModelRegistration.Create();
            await client.ExecuteAsync(model.Operations.Single(o => o.Id == operation), GraphQueries.Create(operation, 1, 3, 1, 1, "Bob", 999), sink, CancellationToken.None);
            sink.Values.Count.ShouldBe(1);
        }
        (await RowsAsync(setup, "MATCH (n:Post {id:999}) RETURN n.id")).ShouldBeEmpty();
        (await RowsAsync(setup, "MATCH (p:Person)-[r:LIKES]->(m:Post) RETURN r.creationDate")).ShouldBeEmpty();
        (await RowsAsync(setup, baseline)).Count.ShouldBe(before);
    }

    [Fact]
    public void CreateParameterStream_SameSeedAndScope_IsDeterministicAndPartitionsWrites()
    {
        var manifest = new DatasetManifest { Model = DatasetModel.Graph, Name = "social", GeneratorVersion = 1, Scale = 0.01, Seed = 20261015, Tables = [] };
        var model = ModelRegistration.Create();
        var a = model.CreateParameterStream(manifest, manifest.Seed, "graph.add-post", 0, new("verify", 1, 1, false));
        var b = model.CreateParameterStream(manifest, manifest.Seed, "graph.add-post", 0, new("verify", 1, 1, false));
        var c = model.CreateParameterStream(manifest, manifest.Seed, "graph.add-post", 0, new("latency", 1, 1, false));
        var first = (GraphParameters)a.Next();
        ((GraphParameters)b.Next()).Gql.ShouldBe(first.Gql);
        ((GraphParameters)c.Next()).Values["id"].ShouldNotBe(first.Values["id"]);
        foreach (var target in model.Targets)
        {
            var instance = model.CreateTarget(target.Id, new());
            instance.Support.Count.ShouldBe(13);
            foreach (var entry in instance.Support.Values) entry.Validate();
        }
    }

    private static async Task<List<object?[]>> RowsAsync(GraphDatabaseSession session, string text)
    {
        await using var result = (QueryResultSet)await session.ExecuteAsync(text);
        var rows = new List<object?[]>();
        await foreach (var row in result.GetRowsAsync())
        {
            var values = new object?[row.FieldCount];
            for (int i = 0; i < values.Length; i++) values[i] = row.GetValue(i);
            rows.Add(values);
        }
        return rows;
    }
    private sealed class Rows : IVerificationSink
    {
        internal List<object?[]> Values { get; } = new();
        public void AddRow(ReadOnlySpan<object?> values) => Values.Add(values.ToArray());
    }
}
