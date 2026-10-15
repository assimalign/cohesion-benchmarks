using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Assimalign.Cohesion.Database.Graph;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal static class CohesionIngest
{
    internal static async Task<List<LoadMetric>> LoadAsync(GraphDatabase db, DatasetManifest manifest, string directory,
        int batchSize, bool sample, bool gql, bool prerequisitesOnly, IProgress<LoadProgress>? progress, CancellationToken token)
    {
        var metrics = new List<LoadMetric>();
        var ids = new Dictionary<(string Label, long Id), GraphNodeId>();
        await using var session = await db.CreateSessionAsync(token);
        foreach (var node in SocialSchema.Nodes)
        {
            bool prerequisite = node.Label is "Person" or "Tag";
            if (sample && !prerequisite && (prerequisitesOnly || node.Label != "Post")) continue;
            long rows = 0;
            var watch = Stopwatch.StartNew();
            var batch = new List<Dictionary<string, object?>>(batchSize);
            foreach (var properties in GraphData.Nodes(manifest, directory, node, sample && node.Label == "Post" ? GraphData.SamplePosts : long.MaxValue, token))
            {
                batch.Add(properties);
                if (batch.Count == batchSize) await FlushNodesAsync();
            }
            if (batch.Count != 0) await FlushNodesAsync();
            watch.Stop();
            if (!sample || !prerequisite) metrics.Add(new(node.File, sample ? "candidate-ingest" : "ingest",
                gql ? "multi-path INSERT; explicit transactions" : "CreateNodeAsync; explicit transactions", rows, GraphData.SourceBytes(manifest, node.File, rows), watch.Elapsed.TotalSeconds, sample));
            progress?.Report(new(node.File, rows, 0));

            async Task FlushNodesAsync()
            {
                await using var transaction = await session.BeginTransactionAsync(token);
                if (gql && (!sample || !prerequisite))
                {
                    var statement = new StringBuilder("INSERT ");
                    for (int i = 0; i < batch.Count; i++)
                    {
                        if (i != 0) statement.Append(',');
                        statement.Append("(:").Append(node.Label).Append(' ').Append(GraphMapping.Properties(batch[i])).Append(')');
                    }
                    await session.ExecuteAsync(statement.ToString(), cancellationToken: token);
                }
                else foreach (var properties in batch)
                {
                    var created = await db.CreateNodeAsync(session, [node.Label], properties, token);
                    ids.Add((node.Label, (long)properties["id"]!), created.Id);
                }
                await transaction.CommitAsync(token);
                rows += batch.Count;
                batch.Clear();
            }
        }
        if (gql && !sample)
        {
            var watch = Stopwatch.StartNew();
            foreach (var node in SocialSchema.Nodes)
                await GraphSchema.Open(db, session).CreateIndexAsync(node.Label, node.Indexes[0].Name, "id", token);
            metrics.Add(new("social", "index", "C# equality id indexes before GQL endpoint matches", SocialSchema.Nodes.Count, 0, watch.Elapsed.TotalSeconds));
        }
        if (sample)
        {
            // Sample setup is excluded for both candidate paths. Identical lookup indexes are present.
            var schema = GraphSchema.Open(db, session);
            await schema.CreateIndexAsync("Person", "sample_person_id", "id", token);
            await schema.CreateIndexAsync("Person", "sample_person_name", "firstName", token);
            await schema.CreateIndexAsync("Tag", "sample_tag_id", "id", token);
            await schema.CreateIndexAsync("Tag", "sample_tag_name", "name", token);
            if (prerequisitesOnly) return metrics;
            await schema.CreateIndexAsync("Post", "sample_post_id", "id", token);
        }
        foreach (var relation in SocialSchema.Relationships)
        {
            if (sample && !GraphData.SampleRelationship(relation)) continue;
            long rows = 0;
            var watch = Stopwatch.StartNew();
            var batch = new List<GraphData.Edge>(batchSize);
            foreach (var edge in GraphData.Edges(manifest, directory, relation, sample, token))
            {
                batch.Add(edge);
                if (batch.Count == batchSize) await FlushEdgesAsync();
            }
            if (batch.Count != 0) await FlushEdgesAsync();
            watch.Stop();
            metrics.Add(new(relation.File, sample ? "candidate-ingest" : "ingest",
                gql ? "MATCH / multi-path INSERT; explicit transactions" : "CreateRelationshipAsync; explicit transactions", rows, GraphData.SourceBytes(manifest, relation.File, rows), watch.Elapsed.TotalSeconds, sample));
            progress?.Report(new(relation.File, rows, 0));

            async Task FlushEdgesAsync()
            {
                await using var transaction = await session.BeginTransactionAsync(token);
                if (gql)
                {
                    // Every endpoint is selected by its indexed id in the engine, with no driver join.
                    await session.ExecuteAsync(EdgeStatement(relation, batch), cancellationToken: token);
                }
                else foreach (var edge in batch)
                    await db.CreateRelationshipAsync(session, ids[(relation.SourceLabel, edge.Source)], ids[(relation.DestinationLabel, edge.Target)], relation.Type, edge.Properties, token);
                await transaction.CommitAsync(token);
                rows += batch.Count;
                batch.Clear();
            }
        }
        return metrics;
    }

    internal static string EdgeStatement(SchemaRelationship relation, IReadOnlyList<GraphData.Edge> edges)
    {
        var text = new StringBuilder("MATCH ");
        var names = new Dictionary<(string, long), string>();
        foreach (var edge in edges) { Node(relation.SourceLabel, edge.Source); Node(relation.DestinationLabel, edge.Target); }
        text.Append(" INSERT ");
        for (int i = 0; i < edges.Count; i++)
        {
            if (i != 0) text.Append(',');
            var edge = edges[i];
            text.Append('(').Append(names[(relation.SourceLabel, edge.Source)]).Append(")-[:").Append(relation.Type)
                .Append(' ').Append(GraphMapping.Properties(edge.Properties)).Append("]->(").Append(names[(relation.DestinationLabel, edge.Target)]).Append(')');
        }
        return text.ToString();
        void Node(string label, long id)
        {
            if (names.ContainsKey((label, id))) return;
            if (names.Count != 0) text.Append(',');
            string name = "n" + names.Count;
            names.Add((label, id), name);
            text.Append('(').Append(name).Append(':').Append(label).Append(" {id:").Append(GraphMapping.Literal(id)).Append("})");
        }
    }
}
