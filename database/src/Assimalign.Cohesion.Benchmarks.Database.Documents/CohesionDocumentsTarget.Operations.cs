using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Assimalign.Cohesion.Database.Documents;
using Assimalign.Cohesion.Database.Execution;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed partial class CohesionDocumentsTarget
{
    public override async ValueTask<IBenchmarkClient> OpenClientAsync(int clientIndex, CancellationToken cancellationToken)
    {
        var session = await (await OpenAsync(cancellationToken)).CreateSessionAsync(cancellationToken);
        try
        {
            var collections = new Dictionary<string, DocumentCollection>(StringComparer.Ordinal);
            foreach (string name in DocumentWorkload.Collections)
                collections[name] = await session.GetCollectionAsync(name, cancellationToken) ?? throw new InvalidOperationException("Missing " + name);
            foreach (string name in new[] { "orders", "products", "customers" })
            {
                var source = await collections[name].GetAsync(session, name + "/1", cancellationToken) ?? throw new InvalidOperationException("Missing fixture source.");
                string key = DocumentParameterStream.Fixture("write_" + name, clientIndex);
                await collections["write_" + name].PutAsync(session, key, DocumentJson.Rewrite(source.Content, key), cancellationToken: cancellationToken);
            }
            return new Client(session, collections);
        }
        catch { await session.DisposeAsync(); throw; }
    }
    private sealed class Client(DocumentDatabaseSession session, Dictionary<string, DocumentCollection> collections) : IBenchmarkClient
    {
        public async ValueTask ExecuteAsync(OperationDescriptor operation, object parameters, IVerificationSink? sink, CancellationToken cancellationToken)
        {
            var p = (DocumentParameters)parameters;
            var collection = collections[p.Collection];
            if (operation.Id == "doc.order-by-id")
            {
                var document = await collection.GetAsync(session, p.Key, cancellationToken);
                if (document != null && sink != null) { using var json = JsonDocument.Parse(document.Value.Content); DocumentRows.Order(json.RootElement, sink); }
                return;
            }
            if (operation.Id == "doc.products-multiget")
            {
                foreach (string key in p.Keys)
                {
                    var doc = await collection.GetAsync(session, key, cancellationToken);
                    if (doc != null && sink != null) { using var json = JsonDocument.Parse(doc.Value.Content); DocumentRows.Product(json.RootElement, sink); }
                }
                return;
            }
            if (operation.Category == "write")
            {
                if (operation.Id is "doc.insert-order" or "doc.replace-customer")
                    await collection.PutAsync(session, p.Key, p.Json, cancellationToken: cancellationToken);
                else
                {
                    var previous = await collection.GetAsync(session, p.Key, cancellationToken) ?? throw new InvalidOperationException("Mutation fixture missing.");
                    var json = JsonNode.Parse(previous.Content.Span)!.AsObject();
                    if (operation.Id == "doc.patch-order-status")
                    { json["status"] = p.Status; json["statusHistory"]!.AsArray().Add(new JsonObject { ["status"] = p.Status, ["occurredAt"] = p.OccurredAt }); }
                    else { var variant = json["variants"]![0]!; variant["stock"] = variant["stock"]!.GetValue<long>() + p.Delta; }
                    await collection.PutAsync(session, p.Key, JsonSerializer.SerializeToUtf8Bytes(json), previous.Version, cancellationToken);
                }
                if (sink != null)
                {
                    var stored = await collection.GetAsync(session, p.Key, cancellationToken) ?? throw new InvalidOperationException("Write did not persist.");
                    using var json = JsonDocument.Parse(stored.Content); DocumentRows.Mutation(operation.Id, json.RootElement, sink);
                }
                return;
            }
            if (operation.Id == "doc.products-by-category-price") throw new NotSupportedException("OQL array predicates are unavailable.");
            await using var result = (QueryResultSet)await session.ExecuteAsync(p.Oql, p.Values, cancellationToken);
            var totals = operation.Id == "doc.revenue-by-category-30d" ? new SortedDictionary<string, decimal>(StringComparer.Ordinal) : null;
            int limit = operation.Id switch { "doc.customer-recent-orders" => 20, "doc.products-name-prefix" => 50, "doc.orders-status-range" => 25, _ => int.MaxValue };
            long count = 0;
            var page = new List<object?[]>();
            await foreach (var row in result.GetRowsAsync(cancellationToken))
            {
                if (totals != null) { DocumentRows.AddItems((JsonElement)row.GetValue(1)!, totals); continue; }
                if (sink != null && count < limit)
                {
                    object?[] values = p.Collection == "products"
                        ? [row.GetString(0), row.GetString(1), row.GetString(2), (decimal)row.GetValue(3)!]
                        : [row.GetString(0), row.GetString(1), DocumentRows.Date(row.GetString(2)!), row.GetString(3), (decimal)row.GetValue(4)!];
                    if (operation.Id == "doc.orders-status-range") page.Add(values); else sink.AddRow(values);
                }
                count++;
            }
            if (operation.Id == "doc.orders-status-range" && sink != null)
            { sink.AddRow(["count", count]); foreach (var row in page) sink.AddRow(row); }
            if (totals != null) DocumentRows.Revenue(totals, sink);
        }
        public ValueTask DisposeAsync() => session.DisposeAsync();
    }
}
