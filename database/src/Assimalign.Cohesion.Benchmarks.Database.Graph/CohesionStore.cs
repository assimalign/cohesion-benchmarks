using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Assimalign.Cohesion.Database.Graph;
using Assimalign.Cohesion.Database.Storage;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal sealed class CohesionStore : IAsyncDisposable
{
    internal GraphDatabaseEngine Engine { get; }
    internal GraphDatabase Database { get; }
    internal long OpenWorkingSet { get; }
    internal long OpenManagedBytes { get; }
    internal double OpenSeconds { get; }
    private readonly TargetOptions options;

    private CohesionStore(GraphDatabaseEngine engine, GraphDatabase database, TargetOptions options, double seconds)
    {
        Engine = engine; Database = database; this.options = options; OpenSeconds = seconds;
        OpenWorkingSet = Process.GetCurrentProcess().WorkingSet64;
        OpenManagedBytes = GC.GetTotalMemory(false);
    }

    internal static async Task<CohesionStore> OpenAsync(TargetOptions options, CancellationToken token)
    {
        var watch = Stopwatch.StartNew();
        var builder = GraphDatabaseEngine.CreateBuilder("benchmark-graph");
        builder.Options.RootPath = options.StorageDirectory;
        builder.Options.BufferPoolCapacity = options.BufferPoolBytes;
        builder.Options.Durability = StorageCommitDurability.Synchronous;
        builder.AddDatabase(options.Database);
        var engine = await builder.BuildAsync(token);
        return new(engine, await engine.OpenDatabaseAsync(options.Database, token), options, watch.Elapsed.TotalSeconds);
    }

    internal async Task<List<LoadMetric>> LoadAsync(DatasetManifest manifest, string directory, IProgress<LoadProgress>? progress, CancellationToken token)
    {
        var metrics = new List<LoadMetric>();
        var api = await CandidateAsync("api", false);
        var gql = await CandidateAsync("gql", true);
        metrics.AddRange(api); metrics.AddRange(gql);
        double apiSeconds = Seconds(api), gqlSeconds = Seconds(gql);
        bool useGql = gqlSeconds < apiSeconds;
        Console.WriteLine($"Cohesion load candidates: API={apiSeconds.ToString("F3", CultureInfo.InvariantCulture)}s GQL={gqlSeconds.ToString("F3", CultureInfo.InvariantCulture)}s selected={(useGql ? "GQL" : "API")}");
        metrics.AddRange(await CohesionIngest.LoadAsync(Database, manifest, directory, options.BatchSize, false, useGql, false, progress, token));
        // The TCP sample opens this separate database through the server's lazy-open path.
        var sample = await Engine.CreateDatabaseAsync(options.Database + "-tcp-ingest", token);
        await CohesionIngest.LoadAsync(sample, manifest, directory, options.BatchSize, true, false, true, null, token);
        await using (var session = await sample.CreateSessionAsync(token))
        {
            var node = await sample.CreateNodeAsync(session, ["Post"], new Dictionary<string, object?> { ["id"] = 0L }, token);
            await sample.DeleteNodeAsync(session, node.Id, token);
            await GraphSchema.Open(sample, session).CreateIndexAsync("Post", "sample_post_id", "id", token);
            await GraphSchema.Open(sample, session).CreateIndexAsync("Post", "sample_post_created", "creationDate", token);
        }
        await sample.DisposeAsync();
        return metrics;

        async Task<List<LoadMetric>> CandidateAsync(string name, bool candidateGql)
        {
            string databaseName = options.Database + "-candidate-" + name;
            var candidate = await Engine.CreateDatabaseAsync(databaseName, token);
            try { return await CohesionIngest.LoadAsync(candidate, manifest, directory, options.BatchSize, true, candidateGql, false, null, token); }
            finally { await Engine.DropDatabaseAsync(databaseName, CancellationToken.None); }
        }
    }

    private static double Seconds(List<LoadMetric> metrics)
    {
        double total = 0; foreach (var metric in metrics) total += metric.Seconds; return total;
    }
    public ValueTask DisposeAsync() => Engine.DisposeAsync();
}
