using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed partial class MongoDocumentsTarget
{
    public override async ValueTask<IBenchmarkClient> OpenClientAsync(int clientIndex, CancellationToken cancellationToken)
    {
        foreach (string name in new[] { "orders", "products", "customers" })
        foreach (bool warmup in new[] { false, true })
        {
            var source = await database.GetCollection<BsonDocument>(name).Find(new BsonDocument("_id", name + "/1")).SingleAsync(cancellationToken);
            string key = DocumentParameterStream.Fixture("write_" + name, clientIndex, warmup);
            source["_id"] = key; source["id"] = key;
            await database.GetCollection<BsonDocument>("write_" + name).ReplaceOneAsync(new BsonDocument("_id", key), source, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        return new Client(collections);
    }
    private sealed class Client(Dictionary<string, IMongoCollection<BsonDocument>> collections) : IBenchmarkClient
    {
        public async ValueTask ExecuteAsync(OperationDescriptor operation, object parameters, IVerificationSink? sink, CancellationToken cancellationToken)
        {
            var p = (DocumentParameters)parameters; var collection = collections[p.Collection];
            if (operation.Id == "doc.order-by-id")
            {
                using var cursor = await collection.FindAsync(p.MongoFilter, cancellationToken: cancellationToken);
                while (await cursor.MoveNextAsync(cancellationToken)) foreach (var doc in cursor.Current)
                    if (sink != null) { using var json = DocumentRows.MongoJson(doc); DocumentRows.Order(json.RootElement, sink); }
                return;
            }
            if (operation.Category == "write")
            {
                if (operation.Id == "doc.insert-order") await collection.InsertOneAsync(p.MongoJson, cancellationToken: cancellationToken);
                else if (operation.Id == "doc.replace-customer")
                { var r = await collection.ReplaceOneAsync(p.MongoFilter, p.MongoJson, cancellationToken: cancellationToken); if (r.MatchedCount != 1) throw new InvalidOperationException("Replacement missed fixture."); }
                else
                { var r = await collection.UpdateOneAsync(p.MongoFilter, p.MongoUpdate, cancellationToken: cancellationToken); if (r.MatchedCount != 1) throw new InvalidOperationException("Patch missed fixture."); }
                if (sink != null)
                {
                    var stored = await collection.Find(p.MongoFilter).SingleAsync(cancellationToken);
                    using var json = DocumentRows.MongoJson(stored); DocumentRows.Mutation(operation.Id, json.RootElement, sink);
                }
                return;
            }
            using var results = await collection.AggregateAsync<BsonDocument>(p.MongoPipeline, cancellationToken: cancellationToken);
            while (await results.MoveNextAsync(cancellationToken)) foreach (var doc in results.Current)
            {
                if (sink == null) continue;
                if (operation.Id == "doc.orders-status-range")
                {
                    var count = doc["count"].AsBsonArray;
                    sink.AddRow(["count", count.Count == 0 ? 0L : count[0]["value"].ToInt64()]);
                    foreach (var row in doc["page"].AsBsonArray) { using var json = DocumentRows.MongoJson(row.AsBsonDocument); DocumentRows.Order(json.RootElement, sink); }
                }
                else if (operation.Id == "doc.revenue-by-category-30d") sink.AddRow([doc["category"].AsString, Decimal128.ToDecimal(doc["revenue"].AsDecimal128)]);
                else { using var json = DocumentRows.MongoJson(doc); if (p.Collection == "products") DocumentRows.Product(json.RootElement, sink); else DocumentRows.Order(json.RootElement, sink); }
            }
        }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
