using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Microsoft.Azure.Cosmos;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed partial class CosmosDocumentsTarget
{
    private async Task<JsonDocument> ReadAsync(string collection, string id, CancellationToken token)
    {
        using var response = await containers[collection].ReadItemStreamAsync(id, new PartitionKey(id), cancellationToken: token);
        response.EnsureSuccessStatusCode();
        return await JsonDocument.ParseAsync(response.Content, cancellationToken: token);
    }
    public override async ValueTask<IBenchmarkClient> OpenClientAsync(int clientIndex, CancellationToken cancellationToken)
    {
        foreach (string name in new[] { "orders", "products", "customers" })
        foreach (bool warmup in new[] { false, true })
        {
            using var source = await ReadAsync(name, DocumentJson.CosmosId(name + "/1"), cancellationToken);
            string key = DocumentParameterStream.Fixture("write_" + name, clientIndex, warmup);
            using var stream = new MemoryStream(DocumentJson.Rewrite(System.Text.Encoding.UTF8.GetBytes(source.RootElement.GetRawText()), key, cosmos: true), writable: false);
            using var response = await database.GetContainer("write_" + name).UpsertItemStreamAsync(stream, new PartitionKey(DocumentJson.CosmosId(key)), cancellationToken: cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        return new Client(this);
    }
    private sealed class Client(CosmosDocumentsTarget target) : IBenchmarkClient
    {
        private readonly QueryRequestOptions queryOptions = new() { MaxItemCount = 1000 };
        public async ValueTask ExecuteAsync(OperationDescriptor operation, object parameters, IVerificationSink? sink, CancellationToken cancellationToken)
        {
            var p = (DocumentParameters)parameters;
            var container = target.containers[p.Collection];
            if (operation.Id == "doc.order-by-id")
            {
                using var result = await target.ReadAsync(p.Collection, p.CosmosKey, cancellationToken);
                if (sink != null) DocumentRows.Order(result.RootElement, sink); return;
            }
            if (operation.Category == "write")
            {
                using var stream = new MemoryStream(p.CosmosJson, writable: false);
                using var response = operation.Id switch
                {
                    "doc.insert-order" => await container.CreateItemStreamAsync(stream, new PartitionKey(p.CosmosKey), cancellationToken: cancellationToken),
                    "doc.replace-customer" => await container.ReplaceItemStreamAsync(stream, p.CosmosKey, new PartitionKey(p.CosmosKey), cancellationToken: cancellationToken),
                    _ => await container.PatchItemStreamAsync(p.CosmosKey, new PartitionKey(p.CosmosKey), p.CosmosPatch, cancellationToken: cancellationToken)
                };
                response.EnsureSuccessStatusCode();
                // Consume the response even when verification is disabled.
                if (response.Content != null) await response.Content.CopyToAsync(Stream.Null, cancellationToken);
                if (sink != null) { using var stored = await target.ReadAsync(p.Collection, p.CosmosKey, cancellationToken); DocumentRows.Mutation(operation.Id, stored.RootElement, sink); }
                return;
            }
            using var iterator = container.GetItemQueryStreamIterator(p.CosmosQuery, requestOptions: queryOptions);
            var totals = operation.Id == "doc.revenue-by-category-30d" ? new SortedDictionary<string, decimal>(StringComparer.Ordinal) : null;
            long count = 0; var page = new List<JsonElement>();
            while (iterator.HasMoreResults)
            {
                using var response = await iterator.ReadNextAsync(cancellationToken); response.EnsureSuccessStatusCode();
                using var result = await JsonDocument.ParseAsync(response.Content, cancellationToken: cancellationToken);
                foreach (var row in result.RootElement.GetProperty("Documents").EnumerateArray())
                {
                    if (totals != null) totals.Add(row.GetProperty("category").GetString()!, row.GetProperty("revenue").GetDecimal());
                    else if (operation.Id == "doc.orders-status-range") { if (sink != null && count < 25) page.Add(row.Clone()); count++; }
                    else if (sink != null) { if (p.Collection == "products") DocumentRows.Product(row, sink); else DocumentRows.Order(row, sink); }
                }
            }
            if (operation.Id == "doc.orders-status-range" && sink != null)
            { sink.AddRow(["count", count]); foreach (var row in page) DocumentRows.Order(row, sink); }
            if (totals != null) DocumentRows.Revenue(totals, sink);
        }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
