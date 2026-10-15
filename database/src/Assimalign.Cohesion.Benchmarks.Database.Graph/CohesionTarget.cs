using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Assimalign.Cohesion.Connections.Tcp;
using Assimalign.Cohesion.Database.Client;
using Assimalign.Cohesion.Database.Execution;
using Assimalign.Cohesion.Database.Graph;
using Assimalign.Cohesion.Database.Graph.Client;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal sealed class CohesionTarget : IBenchmarkTarget
{
    private readonly TargetOptions options;
    private readonly bool network;
    private readonly SemaphoreSlim initialize = new(1);
    private CohesionStore? store;
    private GraphClient? tcp;
    public TargetDescriptor Descriptor { get; }
    public IReadOnlyDictionary<string, OperationSupport> Support { get; }
    internal CohesionTarget(GraphModel model, TargetDescriptor descriptor, TargetOptions options)
    {
        this.options = options; Descriptor = descriptor; network = descriptor.ServedDataTarget is not null;
        Support = model.Support(false, network);
    }
    private GraphClient CreateTcp(string database)
    {
        var settings = DatabaseConnectionSettings.For(new Uri(options.Endpoint ?? throw new ArgumentException("TCP endpoint required.")), database,
            options.Settings.TryGetValue("principal", out string? principal) ? principal : "dbbench");
        settings.MaxPoolSize = 64;
        return GraphClient.Create(new() { Settings = settings, ConnectionFactory = new TcpConnectionFactory() });
    }
    private async ValueTask EnsureAsync(CancellationToken token)
    {
        await initialize.WaitAsync(token);
        try
        {
            if (network) tcp ??= CreateTcp(options.Database);
            else store ??= await CohesionStore.OpenAsync(options, token);
        }
        finally { initialize.Release(); }
    }
    public async ValueTask PrepareAsync(DatasetManifest manifest, CancellationToken cancellationToken)
    {
        await EnsureAsync(cancellationToken);
        if (network) return;
        await using var session = await store!.Database.CreateSessionAsync(cancellationToken);
        await using var result = (QueryResultSet)await session.ExecuteAsync("MATCH (p:Person) RETURN p.id", cancellationToken: cancellationToken);
        await foreach (var _ in result.GetRowsAsync(cancellationToken)) throw new InvalidOperationException("Full load requires an empty graph volume.");
    }
    public async ValueTask<IReadOnlyList<LoadMetric>> LoadAsync(DatasetManifest manifest, string directory, IProgress<LoadProgress>? progress, CancellationToken cancellationToken)
    {
        await EnsureAsync(cancellationToken);
        if (!network) return await store!.LoadAsync(manifest, directory, progress, cancellationToken);
        await using var sample = CreateTcp(options.Database + "-tcp-ingest");
        await using var connection = await sample.ConnectAsync(cancellationToken);
        var existing = await connection.QueryAsync("MATCH (p:Post) RETURN p.id", cancellationToken: cancellationToken);
        if (existing.Count != 0) throw new InvalidOperationException("TCP sample already loaded; restore the embedded-prepared sample before another load.");
        return await TcpIngest.SampleAsync(connection, manifest, directory, options.BatchSize, cancellationToken);
    }
    public async ValueTask<IReadOnlyList<LoadMetric>> FinalizeLoadAsync(CancellationToken cancellationToken)
    {
        if (network) return Array.Empty<LoadMetric>();
        await EnsureAsync(cancellationToken);
        await using var session = await store!.Database.CreateSessionAsync(cancellationToken);
        var watch = Stopwatch.StartNew();
        await GraphMapping.CreateCohesionIndexesAsync(store.Database, session, cancellationToken);
        return new[] { new LoadMetric("social", "index", "GraphSchema.CreateIndexAsync; equality/non-unique", 11, 0, watch.Elapsed.TotalSeconds) };
    }
    public async ValueTask<EngineDescription> DescribeAsync(CancellationToken cancellationToken)
    {
        await EnsureAsync(cancellationToken);
        var settings = new Dictionary<string, string> { ["temporalRepresentation"] = "int64 epoch milliseconds UTC",
            ["indexes"] = "equality only; id uniqueness not enforced", ["Durability"] = "Synchronous", ["mutationIsolation"] = "client cleanup of uniquely identified writes outside timed operation" };
        if (!network)
        {
            settings["BufferPoolCapacity"] = options.BufferPoolBytes.ToString(CultureInfo.InvariantCulture);
            settings["openWorkingSetBytes"] = store!.OpenWorkingSet.ToString(CultureInfo.InvariantCulture);
            settings["openManagedBytes"] = store.OpenManagedBytes.ToString(CultureInfo.InvariantCulture);
            settings["openSeconds"] = store.OpenSeconds.ToString("R", CultureInfo.InvariantCulture);
        }
        else
        {
            await using var connection = await tcp!.ConnectAsync(cancellationToken);
            var indexes = await connection.QueryAsync("SHOW INDEXES", cancellationToken: cancellationToken);
            settings["observedIndexCount"] = indexes.Count.ToString(CultureInfo.InvariantCulture);
            settings["wireTransactions"] = "unavailable";
        }
        return new(typeof(GraphDatabaseEngine).Assembly.GetName().Version?.ToString() ?? "unknown", settings);
    }
    public ValueTask<long> MeasureStorageAsync(CancellationToken cancellationToken)
    {
        long bytes = 0;
        if (Directory.Exists(options.StorageDirectory))
        foreach (string file in Directory.EnumerateFiles(options.StorageDirectory, "*", SearchOption.AllDirectories))
        { cancellationToken.ThrowIfCancellationRequested(); bytes += new FileInfo(file).Length; }
        return ValueTask.FromResult(bytes);
    }
    public async ValueTask<IBenchmarkClient> OpenClientAsync(int clientIndex, CancellationToken cancellationToken)
    {
        await EnsureAsync(cancellationToken);
        return network ? new CohesionBenchmarkClient(tcp!, await tcp!.ConnectAsync(cancellationToken))
            : new CohesionBenchmarkClient(await store!.Database.CreateSessionAsync(cancellationToken));
    }
    public async ValueTask PingAsync(CancellationToken cancellationToken)
    {
        await EnsureAsync(cancellationToken);
        if (network) { await using var connection = await tcp!.ConnectAsync(cancellationToken); await connection.QueryAsync("SHOW INDEXES", cancellationToken: cancellationToken); }
        else { await using var session = await store!.Database.CreateSessionAsync(cancellationToken); await using var result = (QueryResultSet)await session.ExecuteAsync("SHOW INDEXES", cancellationToken: cancellationToken); await foreach (var _ in result.GetRowsAsync(cancellationToken)) { } }
    }
    public async ValueTask DisposeAsync()
    {
        if (tcp is not null) await tcp.DisposeAsync();
        if (store is not null) await store.DisposeAsync();
        initialize.Dispose();
    }
}
