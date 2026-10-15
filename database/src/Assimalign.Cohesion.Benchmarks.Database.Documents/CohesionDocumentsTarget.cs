using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Assimalign.Cohesion.Database;
using Assimalign.Cohesion.Database.Documents;
using Assimalign.Cohesion.Database.Storage;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed partial class CohesionDocumentsTarget : DocumentsTarget
{
    private readonly DocumentDatabaseEngine engine;
    private readonly SemaphoreSlim openGate = new(1);
    private DocumentDatabase? database;
    internal CohesionDocumentsTarget(TargetDescriptor descriptor, TargetOptions options) : base(descriptor, options)
    {
        Directory.CreateDirectory(options.StorageDirectory);
        engine = DocumentDatabaseEngine.Create("dbbench-documents-r3", new()
        {
            RootPath = Path.GetFullPath(options.StorageDirectory),
            BufferPoolCapacity = options.BufferPoolBytes,
            Durability = StorageCommitDurability.Synchronous
        });
    }
    private async ValueTask<DocumentDatabase> OpenAsync(CancellationToken token)
    {
        await openGate.WaitAsync(token).ConfigureAwait(false);
        try { return database ??= await engine.OpenDatabaseAsync(Options.Database, token).ConfigureAwait(false); }
        finally { openGate.Release(); }
    }
    public override async ValueTask PrepareAsync(DatasetManifest manifest, CancellationToken cancellationToken)
    {
        try { await engine.DropDatabaseAsync(Options.Database, cancellationToken); }
        catch (DatabaseNotFoundException) { }
        database = await engine.CreateDatabaseAsync(Options.Database, cancellationToken);
        await using var session = await database.CreateSessionAsync(cancellationToken);
        foreach (string collection in DocumentWorkload.Collections) await session.CreateCollectionAsync(collection, cancellationToken);
    }
    public override async ValueTask<IReadOnlyList<LoadMetric>> FinalizeLoadAsync(CancellationToken cancellationToken)
    {
        var watch = Stopwatch.StartNew();
        await using var session = await (await OpenAsync(cancellationToken)).CreateSessionAsync(cancellationToken);
        foreach (string collection in DocumentWorkload.Collections)
        {
            string logical = collection.Replace("write_", "", StringComparison.Ordinal);
            string[] paths = logical switch
            {
                "orders" => ["customer.id", "placedAt", "status"],
                "products" => ["category", "name"],
                "reviews" => ["productId", "createdAt"], _ => []
            };
            foreach (string path in paths)
                await session.ExecuteAsync($"CREATE INDEX ix_{path.Replace('.', '_')} ON {collection} ({path})", cancellationToken: cancellationToken);
        }
        return new[] { new LoadMetric("*", "indexes", "OQL CREATE INDEX; separate scalar paths; no multikey", 0, 0, watch.Elapsed.TotalSeconds) };
    }
    public override ValueTask<EngineDescription> DescribeAsync(CancellationToken cancellationToken) => ValueTask.FromResult(new EngineDescription(
        typeof(DocumentDatabaseEngine).Assembly.GetName().Version?.ToString() ?? "unknown",
        new Dictionary<string, string> { ["BufferPoolCapacity"] = Options.BufferPoolBytes.ToString(CultureInfo.InvariantCulture),
            ["Durability"] = "Synchronous", ["RootPath"] = Path.GetFullPath(Options.StorageDirectory),
            ["indexes"] = "single-path non-unique; no compound or multikey", ["mutationIsolation"] = "write_* collections, identical supported indexes" }));
    public override ValueTask<long> MeasureStorageAsync(CancellationToken cancellationToken)
    {
        long bytes = 0;
        foreach (string path in Directory.EnumerateFiles(Options.StorageDirectory, "*", SearchOption.AllDirectories))
        { cancellationToken.ThrowIfCancellationRequested(); bytes += new FileInfo(path).Length; }
        return ValueTask.FromResult(bytes);
    }
    public override async ValueTask PingAsync(CancellationToken cancellationToken)
    { await using var session = await (await OpenAsync(cancellationToken)).CreateSessionAsync(cancellationToken); }
    public override async ValueTask DisposeAsync() { await engine.DisposeAsync(); openGate.Dispose(); }
}
