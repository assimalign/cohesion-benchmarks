using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed partial class RavenDocumentsTarget : DocumentsTarget
{
    private readonly DocumentStore store;
    private readonly HttpClient http;
    private readonly string databasePath;
    internal RavenDocumentsTarget(TargetDescriptor descriptor, TargetOptions options) : base(descriptor, options)
    {
        store = new DocumentStore { Urls = [options.Endpoint!], Database = options.Database };
        store.Conventions.BulkInsert.TrySerializeEntityToJsonStream = (entity, metadata, writer) =>
        {
            if (entity is not string raw) return false;
            writer.Write(raw);
            return true;
        };
        store.Initialize();
        http = new HttpClient { BaseAddress = new Uri(options.Endpoint!), Timeout = Timeout.InfiniteTimeSpan };
        databasePath = "/databases/" + Uri.EscapeDataString(options.Database);
    }
    private async Task<JsonDocument> SendAsync(HttpMethod method, string path, byte[]? body, CancellationToken token)
    {
        using var request = new HttpRequestMessage(method, path);
        if (body != null) { request.Content = new ByteArrayContent(body); request.Content.Headers.ContentType = new("application/json"); }
        using var response = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode) throw new HttpRequestException($"RavenDB {(int)response.StatusCode}: {await response.Content.ReadAsStringAsync(token)}");
        await using var stream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);
        return await JsonDocument.ParseAsync(stream, cancellationToken: token).ConfigureAwait(false);
    }
    public override async ValueTask PrepareAsync(DatasetManifest manifest, CancellationToken cancellationToken)
    {
        using (var response = await http.GetAsync(databasePath + "/stats", cancellationToken))
            if (response.IsSuccessStatusCode)
                await store.Maintenance.Server.SendAsync(new DeleteDatabasesOperation(Options.Database, hardDelete: true), cancellationToken);
            else if (response.StatusCode != HttpStatusCode.NotFound) response.EnsureSuccessStatusCode();
        await store.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(Options.Database)), cancellationToken);
    }
    private static IndexDefinition Index(string collection, string suffix, string map, params string[] exact)
    {
        var fields = new Dictionary<string, IndexFieldOptions>();
        foreach (string field in exact) fields[field] = new() { Indexing = FieldIndexing.Exact };
        return new IndexDefinition { Name = "Bench/" + collection + "/" + suffix,
            Maps = new HashSet<string> { "from d in docs." + collection + " select new { " + map + " }" }, Fields = fields };
    }
    public override async ValueTask<IReadOnlyList<LoadMetric>> FinalizeLoadAsync(CancellationToken cancellationToken)
    {
        var watch = Stopwatch.StartNew();
        var definitions = new List<IndexDefinition>();
        foreach (string collection in DocumentWorkload.Collections)
        {
            switch (collection.Replace("write_", "", StringComparison.Ordinal))
            {
                case "orders":
                    definitions.Add(Index(collection, "CustomerDate", "Customer = d.customer.id, PlacedAt = d.placedAt, Key = d.id", "Customer", "Key"));
                    definitions.Add(Index(collection, "StatusDate", "Status = d.status, PlacedAt = d.placedAt, Key = d.id", "Status", "Key"));
                    definitions.Add(Index(collection, "Date", "PlacedAt = d.placedAt, Key = d.id", "Key")); break;
                case "products":
                    definitions.Add(Index(collection, "CategoryPrice", "Category = d.category, Prices = d.variants.Select(v => v.price), Key = d.id", "Category", "Key"));
                    definitions.Add(Index(collection, "Name", "Name = d.name, Key = d.id", "Name", "Key")); break;
                case "reviews": definitions.Add(Index(collection, "ProductDate", "Product = d.productId, CreatedAt = d.createdAt, Key = d.id", "Product", "Key")); break;
            }
        }
        await store.Maintenance.SendAsync(new PutIndexesOperation(definitions.ToArray()), cancellationToken);
        double submitted = watch.Elapsed.TotalSeconds;
        await WaitForIndexesAsync(cancellationToken);
        return new[] { new LoadMetric("*", "indexes-submit", "Explicit static indexes; exact string fields", 0, 0, submitted),
            new LoadMetric("*", "indexing-wait", "Wait until every static index is non-stale; reject index errors/auto-indexes", 0, 0, watch.Elapsed.TotalSeconds - submitted) };
    }
    private async Task WaitForIndexesAsync(CancellationToken token)
    {
        using var deadline = CancellationTokenSource.CreateLinkedTokenSource(token);
        deadline.CancelAfter(TimeSpan.FromMinutes(10));
        while (true)
        {
            var statistics = await store.Maintenance.SendAsync(new GetStatisticsOperation(), deadline.Token);
            foreach (var index in statistics.Indexes)
                if (index.Name.StartsWith("Auto/", StringComparison.Ordinal)) throw new InvalidOperationException("Unexpected auto-index: " + index.Name);
            var errors = await store.Maintenance.SendAsync(new GetIndexErrorsOperation(), deadline.Token);
            foreach (var index in errors) if (index.Errors.Length != 0) throw new InvalidOperationException("Index failed: " + index.Name + ": " + index.Errors[0].Error);
            if (statistics.StaleIndexes.Length == 0) return;
            await Task.Delay(100, deadline.Token);
        }
    }
    public override async ValueTask<EngineDescription> DescribeAsync(CancellationToken cancellationToken)
    {
        using var version = await SendAsync(HttpMethod.Get, "/build/version", null, cancellationToken);
        return new(version.RootElement.GetProperty("FullVersion").GetString()!, new Dictionary<string, string>
        { ["indexes"] = "Explicit static indexes; exact strings; WaitForNonStaleResults", ["storage"] = "RavenDB durable defaults",
            ["mutationIsolation"] = "write_* collections in the same database", ["client"] = "RavenDB.Client 7.1.12 BulkInsert and maintenance; raw RQL HTTP without response caching" });
    }
    public override async ValueTask<long> MeasureStorageAsync(CancellationToken cancellationToken)
    {
        using var stats = await SendAsync(HttpMethod.Get, databasePath + "/stats", null, cancellationToken);
        return stats.RootElement.GetProperty("SizeOnDisk").GetProperty("SizeInBytes").GetInt64();
    }
    public override async ValueTask PingAsync(CancellationToken cancellationToken)
    { using var version = await SendAsync(HttpMethod.Get, "/build/version", null, cancellationToken); }
    public override ValueTask DisposeAsync() { http.Dispose(); store.Dispose(); return ValueTask.CompletedTask; }
}
