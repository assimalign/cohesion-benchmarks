using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Raven.Client.Json;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed partial class RavenDocumentsTarget
{
    public override async ValueTask<IReadOnlyList<LoadMetric>> LoadAsync(DatasetManifest manifest, string directory,
        IProgress<LoadProgress>? progress, CancellationToken cancellationToken)
    {
        var metrics = new List<LoadMetric>();
        foreach (var table in manifest.Tables)
        {
            var watch = Stopwatch.StartNew();
            await Parallel.ForEachAsync(DatasetFiles.EnumerateParts(directory, manifest, table.Name),
                new ParallelOptions { MaxDegreeOfParallelism = 4, CancellationToken = cancellationToken }, async (path, token) =>
                {
                    // One stream per worker; each store call finishes using the raw JSON before advancing the reader.
                    await using var bulk = store.BulkInsert(Options.Database, token: token);
                    using var reader = new JsonLinesReader(File.OpenRead(path));
                    var metadata = new MetadataAsDictionary { ["@collection"] = table.Name };
                    while (reader.Read())
                    {
                        token.ThrowIfCancellationRequested();
                        string id = DocumentJson.Key(reader.Current);
                        string raw = Encoding.UTF8.GetString(DocumentJson.Rewrite(reader.Current, ravenCollection: table.Name));
                        await bulk.StoreAsync(raw, id, metadata);
                    }
                });
            metrics.Add(new(table.Name, "ingest", "RavenDB.Client BulkInsert; raw JSON serializer; 4 part workers", table.Rows, table.Bytes, watch.Elapsed.TotalSeconds));
            progress?.Report(new(table.Name, table.Rows, table.Bytes));
        }
        return metrics;
    }
}
