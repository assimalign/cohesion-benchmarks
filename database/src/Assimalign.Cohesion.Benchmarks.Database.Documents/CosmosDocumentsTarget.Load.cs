using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Microsoft.Azure.Cosmos;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed partial class CosmosDocumentsTarget
{
    private static IEnumerable<byte[]> Lines(IEnumerable<string> paths)
    {
        foreach (string path in paths)
        { using var reader = new JsonLinesReader(File.OpenRead(path)); while (reader.Read()) yield return reader.Current.ToArray(); }
    }
    public override async ValueTask<IReadOnlyList<LoadMetric>> LoadAsync(DatasetManifest manifest, string directory,
        IProgress<LoadProgress>? progress, CancellationToken cancellationToken)
    {
        var metrics = new List<LoadMetric>();
        foreach (var table in manifest.Tables)
        {
            var watch = Stopwatch.StartNew();
            var container = bulkClient.GetContainer(Options.Database, table.Name);
            await Parallel.ForEachAsync(Lines(DatasetFiles.EnumerateParts(directory, manifest, table.Name)),
                new ParallelOptions { MaxDegreeOfParallelism = 16, CancellationToken = cancellationToken }, async (bytes, token) =>
                {
                    string id = DocumentJson.CosmosId(DocumentJson.Key(bytes));
                    using var stream = new MemoryStream(DocumentJson.Rewrite(bytes, cosmos: true), writable: false);
                    using var response = await container.CreateItemStreamAsync(stream, new PartitionKey(id), cancellationToken: token);
                    response.EnsureSuccessStatusCode();
                });
            metrics.Add(new(table.Name, "ingest", "Bulk mode; 16 concurrent CreateItemStreamAsync; base64url transport ids", table.Rows, table.Bytes, watch.Elapsed.TotalSeconds));
            progress?.Report(new(table.Name, table.Rows, table.Bytes));
        }
        return metrics;
    }
}
