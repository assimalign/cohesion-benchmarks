using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed partial class MongoDocumentsTarget : DocumentsTarget
{
    private readonly MongoClient client;
    private readonly IMongoDatabase database;
    internal MongoDocumentsTarget(TargetDescriptor descriptor, TargetOptions options) : base(descriptor, options)
    {
        var settings = MongoClientSettings.FromConnectionString(options.Endpoint);
        if (options.Settings.TryGetValue("username", out string? username))
            settings.Credential = MongoCredential.CreateCredential("admin", username, options.Settings["password"]);
        settings.WriteConcern = WriteConcern.W1.With(journal: true);
        settings.ReadPreference = ReadPreference.Primary;
        client = new MongoClient(settings); database = client.GetDatabase(options.Database);
    }
    public override async ValueTask PrepareAsync(DatasetManifest manifest, CancellationToken cancellationToken)
    {
        await client.DropDatabaseAsync(Options.Database, cancellationToken);
        foreach (string collection in DocumentWorkload.Collections)
            await database.CreateCollectionAsync(collection, new CreateCollectionOptions { Collation = Collation.Simple }, cancellationToken);
    }
    public override async ValueTask<IReadOnlyList<LoadMetric>> FinalizeLoadAsync(CancellationToken cancellationToken)
    {
        var watch = Stopwatch.StartNew();
        foreach (string collection in DocumentWorkload.Collections)
        {
            var indexes = new List<CreateIndexModel<BsonDocument>>();
            var keys = Builders<BsonDocument>.IndexKeys;
            switch (collection.Replace("write_", "", StringComparison.Ordinal))
            {
                case "orders":
                    indexes.Add(new(keys.Ascending("customer.id").Descending("placedAt")));
                    indexes.Add(new(keys.Ascending("status").Ascending("placedAt")));
                    indexes.Add(new(keys.Ascending("placedAt"))); break;
                case "products":
                    indexes.Add(new(keys.Ascending("category").Ascending("variants.price")));
                    indexes.Add(new(keys.Ascending("name"))); break;
                case "reviews": indexes.Add(new(keys.Ascending("productId").Ascending("createdAt"))); break;
            }
            if (indexes.Count != 0) await database.GetCollection<BsonDocument>(collection).Indexes.CreateManyAsync(indexes, cancellationToken);
        }
        return new[] { new LoadMetric("*", "indexes", "CreateMany: compound/multikey + shared standalone placedAt range index", 0, 0, watch.Elapsed.TotalSeconds) };
    }
    public override async ValueTask<EngineDescription> DescribeAsync(CancellationToken cancellationToken)
    {
        var build = await database.RunCommandAsync<BsonDocument>(new BsonDocument("buildInfo", 1), cancellationToken: cancellationToken);
        var options = await client.GetDatabase("admin").RunCommandAsync<BsonDocument>(new BsonDocument("getCmdLineOpts", 1), cancellationToken: cancellationToken);
        return new(build["version"].AsString, new Dictionary<string, string>
        { ["writeConcern"] = database.Settings.WriteConcern.ToString(), ["serverOptions"] = options["parsed"].ToJson(),
            ["collation"] = "simple", ["mutationIsolation"] = "write_* collections, identical indexes" });
    }
    public override async ValueTask<long> MeasureStorageAsync(CancellationToken cancellationToken)
    {
        var stats = await database.RunCommandAsync<BsonDocument>(new BsonDocument("dbStats", 1), cancellationToken: cancellationToken);
        return stats["storageSize"].ToInt64() + stats["indexSize"].ToInt64();
    }
    public override async ValueTask PingAsync(CancellationToken cancellationToken) =>
        _ = await database.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1), cancellationToken: cancellationToken);
    public override ValueTask DisposeAsync() { client.Dispose(); return ValueTask.CompletedTask; }
}
