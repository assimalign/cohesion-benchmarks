using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Assimalign.Cohesion.Database.Documents;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed partial class CohesionDocumentsTarget
{
    private static IEnumerable<byte[][]> Batches(IEnumerable<string> paths, int batchSize)
    {
        var batch = new List<byte[]>(batchSize);
        foreach (string path in paths)
        {
            using var reader = new JsonLinesReader(File.OpenRead(path));
            while (reader.Read())
            {
                batch.Add(reader.Current.ToArray());
                if (batch.Count == batchSize) { yield return batch.ToArray(); batch.Clear(); }
            }
        }
        if (batch.Count != 0) yield return batch.ToArray();
    }
    private static async Task PutBatchesAsync(DocumentDatabase db, string collection, IEnumerable<byte[][]> batches,
        int workers, CancellationToken token)
    {
        // Dynamic partitioning keeps at most workers batches resident. Each batch owns one session/transaction.
        await Parallel.ForEachAsync(batches, new ParallelOptions { MaxDegreeOfParallelism = workers, CancellationToken = token }, async (batch, ct) =>
        {
            await using var session = await db.CreateSessionAsync(ct);
            var target = await session.GetCollectionAsync(collection, ct) ?? throw new InvalidOperationException("Collection missing.");
            await using var transaction = await session.BeginTransactionAsync(cancellationToken: ct);
            foreach (byte[] bytes in batch) await target.PutAsync(session, DocumentJson.Key(bytes), bytes, cancellationToken: ct);
            await transaction.CommitAsync(ct);
        });
    }
    public override async ValueTask<IReadOnlyList<LoadMetric>> LoadAsync(DatasetManifest manifest, string directory,
        IProgress<LoadProgress>? progress, CancellationToken cancellationToken)
    {
        var metrics = new List<LoadMetric>();
        var sample = new List<byte[][]>();
        foreach (byte[][] batch in Batches(DatasetFiles.EnumerateParts(directory, manifest, "customers"), Options.BatchSize))
        { sample.Add(batch); if (sample.Count == 8) break; }
        int selected = 1; double best = double.MaxValue;
        foreach (int workers in new[] { 1, 4 })
        {
            string name = Options.Database + "_load_probe";
            var probe = await engine.CreateDatabaseAsync(name, cancellationToken);
            try
            {
                await using (var session = await probe.CreateSessionAsync(cancellationToken)) await session.CreateCollectionAsync("customers", cancellationToken);
                var watch = Stopwatch.StartNew();
                await PutBatchesAsync(probe, "customers", sample, workers, cancellationToken);
                double elapsed = watch.Elapsed.TotalSeconds;
                long rows = 0, bytes = 0; foreach (var batch in sample) foreach (var doc in batch) { rows++; bytes += doc.Length; }
                metrics.Add(new("customers", "loader-probe", $"{workers} concurrent sessions; explicit transactions of {Options.BatchSize}; selection only", rows, bytes, elapsed, IsSample: true));
                if (elapsed < best) { selected = workers; best = elapsed; }
            }
            finally { await engine.DropDatabaseAsync(name, CancellationToken.None); }
        }
        var db = await OpenAsync(cancellationToken);
        foreach (var table in manifest.Tables)
        {
            var watch = Stopwatch.StartNew();
            await PutBatchesAsync(db, table.Name, Batches(DatasetFiles.EnumerateParts(directory, manifest, table.Name), Options.BatchSize), selected, cancellationToken);
            metrics.Add(new(table.Name, "ingest", $"PutAsync; explicit transactions of {Options.BatchSize}; {selected} selected sessions; one database writer", table.Rows, table.Bytes, watch.Elapsed.TotalSeconds));
            progress?.Report(new(table.Name, table.Rows, table.Bytes));
        }
        return metrics;
    }
}
