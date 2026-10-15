using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Assimalign.Cohesion.Database.Graph.Client;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal static class TcpIngest
{
    internal static async Task<List<LoadMetric>> SampleAsync(GraphConnection connection, DatasetManifest manifest, string directory, int batchSize, CancellationToken token)
    {
        var metrics = new List<LoadMetric>();
        foreach (var node in SocialSchema.Nodes)
        {
            if (node.Label != "Post") continue;
            var watch = Stopwatch.StartNew();
            long count = 0;
            var text = new StringBuilder("INSERT ");
            int pending = 0;
            foreach (var properties in GraphData.Nodes(manifest, directory, node, GraphData.SamplePosts, token))
            {
                if (pending++ != 0) text.Append(',');
                text.Append("(:Post ").Append(GraphMapping.Properties(properties)).Append(')');
                if (pending == batchSize) await FlushAsync();
            }
            if (pending != 0) await FlushAsync();
            metrics.Add(new(node.File, "tcp-ingest", "inlined multi-path INSERT; isolated sample database", count, 0, watch.Elapsed.TotalSeconds, true));
            async Task FlushAsync()
            {
                await connection.ExecuteAsync(text.ToString(), cancellationToken: token);
                count += pending; pending = 0; text.Clear().Append("INSERT ");
            }
        }
        foreach (var relation in SocialSchema.Relationships)
        {
            if (!GraphData.SampleRelationship(relation)) continue;
            var watch = Stopwatch.StartNew();
            long count = 0;
            var rows = new List<GraphData.Edge>(batchSize);
            foreach (var edge in GraphData.Edges(manifest, directory, relation, true, token))
            {
                rows.Add(edge);
                if (rows.Count == batchSize) await FlushAsync();
            }
            if (rows.Count != 0) await FlushAsync();
            metrics.Add(new(relation.File, "tcp-ingest", "inlined MATCH / multi-path INSERT; isolated sample database", count, 0, watch.Elapsed.TotalSeconds, true));
            async Task FlushAsync()
            {
                await connection.ExecuteAsync(CohesionIngest.EdgeStatement(relation, rows), cancellationToken: token);
                count += rows.Count; rows.Clear();
            }
        }
        return metrics;
    }
}
