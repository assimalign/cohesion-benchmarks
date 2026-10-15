using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Neo4j.Driver;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal static class Neo4jIngest
{
    internal static async Task<List<LoadMetric>> SampleAsync(IDriver driver, string database, DatasetManifest manifest,
        string directory, int batchSize, CancellationToken token)
    {
        var metrics = new List<LoadMetric>();
        // PrepareAsync established an empty database. Keep every source property unchanged.
        try
        {
            foreach (var node in SocialSchema.Nodes)
            {
                if (node.Label is not ("Person" or "Tag" or "Post")) continue;
                var watch = Stopwatch.StartNew();
                long count = 0;
                var rows = new List<Dictionary<string, object?>>(batchSize);
                foreach (var properties in GraphData.Nodes(manifest, directory, node, node.Label == "Post" ? GraphData.SamplePosts : long.MaxValue, token))
                {
                    rows.Add(properties);
                    if (rows.Count == batchSize) await FlushAsync();
                }
                if (rows.Count > 0) await FlushAsync();
                if (node.Label == "Post") metrics.Add(new(node.File, "online-ingest", "UNWIND $rows CREATE; isolated pre-import sample", count, GraphData.SourceBytes(manifest, node.File, count), watch.Elapsed.TotalSeconds, true));
                async Task FlushAsync()
                {
                    await RunAsync(driver, database, $"UNWIND $rows AS row CREATE (n:{node.Label}) SET n = row", new() { ["rows"] = rows }, token);
                    count += rows.Count; rows.Clear();
                }
            }
            foreach (var relation in SocialSchema.Relationships)
            {
                if (!GraphData.SampleRelationship(relation)) continue;
                var watch = Stopwatch.StartNew();
                long count = 0;
                var rows = new List<Dictionary<string, object?>>(batchSize);
                foreach (var edge in GraphData.Edges(manifest, directory, relation, true, token))
                {
                    rows.Add(new() { ["src"] = edge.Source, ["dst"] = edge.Target, ["properties"] = edge.Properties });
                    if (rows.Count == batchSize) await FlushAsync();
                }
                if (rows.Count > 0) await FlushAsync();
                metrics.Add(new(relation.File, "online-ingest", "UNWIND $rows MATCH endpoints CREATE", count, GraphData.SourceBytes(manifest, relation.File, count), watch.Elapsed.TotalSeconds, true));
                async Task FlushAsync()
                {
                    await RunAsync(driver, database, $"UNWIND $rows AS row MATCH (a:{relation.SourceLabel} {{id:row.src}}), (b:{relation.DestinationLabel} {{id:row.dst}}) CREATE (a)-[r:{relation.Type}]->(b) SET r = row.properties", new() { ["rows"] = rows }, token);
                    count += rows.Count; rows.Clear();
                }
            }
        }
        finally
        {
            // Only the sample exists in this initially empty database. Cleanup is untimed.
            foreach (string label in new[] { "Post", "Person", "Tag" })
            {
                while (true)
                {
                    var result = await driver.ExecutableQuery($"MATCH (n:{label}) WITH n LIMIT 1000 DETACH DELETE n RETURN count(*) AS removed")
                        .WithConfig(new QueryConfig(database: database)).ExecuteAsync(CancellationToken.None);
                    if (result.Result[0]["removed"].As<long>() == 0) break;
                }
            }
        }
        return metrics;
    }

    internal static async Task RunAsync(IDriver driver, string database, string query, Dictionary<string, object?>? parameters, CancellationToken token)
    {
        await driver.ExecutableQuery(query).WithParameters(parameters ?? new()).WithConfig(new QueryConfig(database: database)).ExecuteAsync(token);
    }
}
