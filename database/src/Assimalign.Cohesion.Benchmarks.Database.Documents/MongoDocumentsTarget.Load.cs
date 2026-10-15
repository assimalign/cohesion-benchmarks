using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed partial class MongoDocumentsTarget
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
                    using var reader = new JsonLinesReader(File.OpenRead(path));
                    var batch = new List<BsonDocument>(Options.BatchSize);
                    var collection = database.GetCollection<BsonDocument>(table.Name);
                    while (reader.Read())
                    {
                        token.ThrowIfCancellationRequested(); batch.Add(DocumentJson.Mongo(reader.Current));
                        if (batch.Count == Options.BatchSize)
                        { await collection.InsertManyAsync(batch, new InsertManyOptions { IsOrdered = false }, token); batch.Clear(); }
                    }
                    if (batch.Count != 0) await collection.InsertManyAsync(batch, new InsertManyOptions { IsOrdered = false }, token);
                });
            metrics.Add(new(table.Name, "ingest", "JSONL UTF-8 -> BSON (decimal128 money); unordered InsertMany; 4 part workers", table.Rows, table.Bytes, watch.Elapsed.TotalSeconds));
            progress?.Report(new(table.Name, table.Rows, table.Bytes));
        }
        return metrics;
    }
}
