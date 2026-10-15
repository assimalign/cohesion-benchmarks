using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Neo4j.Driver;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal sealed class Neo4jTarget : IBenchmarkTarget
{
    private readonly TargetOptions options;
    private readonly string database;
    private IDriver? driver;
    public TargetDescriptor Descriptor { get; }
    public IReadOnlyDictionary<string, OperationSupport> Support { get; }
    internal Neo4jTarget(GraphModel model, TargetDescriptor descriptor, TargetOptions options)
    {
        this.options = options; Descriptor = descriptor; Support = model.Support(true, false);
        database = options.Database == "benchmark" ? "neo4j" : options.Database;
    }
    private IDriver Driver => driver ??= GraphDatabase.Driver(options.Endpoint ?? throw new ArgumentException("Neo4j endpoint required."),
        AuthTokens.Basic(options.Settings["username"], options.Settings["password"]),
        config => config.WithMaxTransactionRetryTime(TimeSpan.Zero).WithMaxConnectionPoolSize(64));
    public async ValueTask PrepareAsync(DatasetManifest manifest, CancellationToken cancellationToken)
    {
        var result = await Driver.ExecutableQuery("MATCH (n) RETURN n LIMIT 1").WithConfig(new QueryConfig(database: database)).ExecuteAsync(cancellationToken);
        if (result.Result.Count != 0) throw new InvalidOperationException("Offline import requires an empty Neo4j volume.");
    }
    public async ValueTask<IReadOnlyList<LoadMetric>> LoadAsync(DatasetManifest manifest, string directory, IProgress<LoadProgress>? progress, CancellationToken cancellationToken)
    {
        // Community has one user database. Measure the unchanged sample on the empty
        // database first; offline import then replaces its stores with the full baseline.
        await FinalizeLoadAsync(cancellationToken);
        var metrics = await Neo4jIngest.SampleAsync(Driver, database, manifest, directory, options.BatchSize, cancellationToken);
        if (driver is not null) { await driver.DisposeAsync(); driver = null; }
        metrics.AddRange(await Neo4jOfflineImport.RunAsync(options, manifest, directory, cancellationToken));
        indexesBuilt = false;
        Exception? last = null;
        for (int i = 0; i < 90; i++)
        {
            try { await PingAsync(cancellationToken); last = null; break; }
            catch (Exception ex) when (!cancellationToken.IsCancellationRequested) { last = ex; await Task.Delay(1000, cancellationToken); }
        }
        if (last is not null) throw new InvalidOperationException("Neo4j did not restart after import.", last);
        metrics.AddRange(await FinalizeLoadAsync(cancellationToken));
        return metrics;
    }
    private bool indexesBuilt;
    public async ValueTask<IReadOnlyList<LoadMetric>> FinalizeLoadAsync(CancellationToken cancellationToken)
    {
        if (indexesBuilt) return Array.Empty<LoadMetric>();
        var watch = Stopwatch.StartNew();
        int count = 0;
        foreach (string statement in GraphMapping.Neo4jIndexes()) { await Neo4jIngest.RunAsync(Driver, database, statement, null, cancellationToken); count++; }
        await Neo4jIngest.RunAsync(Driver, database, "CALL db.awaitIndexes(600)", null, cancellationToken);
        indexesBuilt = true;
        return new[] { new LoadMetric("social", "index", "id uniqueness constraints and range indexes; awaitIndexes", count, 0, watch.Elapsed.TotalSeconds) };
    }
    public async ValueTask<EngineDescription> DescribeAsync(CancellationToken cancellationToken)
    {
        var result = await Driver.ExecutableQuery("CALL dbms.components() YIELD versions RETURN versions[0] AS version").WithConfig(new QueryConfig(database: database)).ExecuteAsync(cancellationToken);
        var settings = new Dictionary<string, string> { ["database"] = database, ["temporalRepresentation"] = "int64 epoch milliseconds UTC", ["retries"] = "disabled", ["mutationIsolation"] = "client cleanup outside timed operation" };
        var observed = await Driver.ExecutableQuery("SHOW SETTINGS YIELD name,value WHERE name IN ['server.memory.heap.initial_size','server.memory.heap.max_size','server.memory.pagecache.size','db.tx_log.preallocate'] RETURN name,value")
            .WithConfig(new QueryConfig(database: database)).ExecuteAsync(cancellationToken);
        foreach (var record in observed.Result) settings.Add(record["name"].As<string>(), record["value"].As<string>());
        return new(result.Result[0]["version"].As<string>(), settings);
    }
    public async ValueTask<long> MeasureStorageAsync(CancellationToken cancellationToken)
    {
        if (!OperatingSystem.IsWindows() || options.Container is null) return 0;
        var start = new ProcessStartInfo("podman") { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true };
        foreach (string value in new[] { "exec", options.Container, "du", "-sb", "/data" }) start.ArgumentList.Add(value);
        using var process = Process.Start(start)!;
        var output = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var error = process.StandardError.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);
        string text = await output;
        if (process.ExitCode != 0) throw new InvalidOperationException(await error);
        return long.Parse(text.Split('\t')[0], CultureInfo.InvariantCulture);
    }
    public async ValueTask<IBenchmarkClient> OpenClientAsync(int clientIndex, CancellationToken cancellationToken)
    {
        await PingAsync(cancellationToken);
        return new Neo4jBenchmarkClient(Driver, database);
    }
    public async ValueTask PingAsync(CancellationToken cancellationToken)
        => await Neo4jIngest.RunAsync(Driver, database, "RETURN 1", null, cancellationToken);
    public async ValueTask DisposeAsync() { if (driver is not null) await driver.DisposeAsync(); }
}
