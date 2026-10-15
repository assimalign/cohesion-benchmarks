using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Microsoft.Azure.Cosmos;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed partial class CosmosDocumentsTarget : DocumentsTarget
{
    private readonly CosmosClient client;
    private readonly CosmosClient bulkClient;
    private readonly Microsoft.Azure.Cosmos.Database database;
    private readonly Dictionary<string, Container> containers = new(StringComparer.Ordinal);
    internal CosmosDocumentsTarget(TargetDescriptor descriptor, TargetOptions options) : base(descriptor, options)
    {
        client = CreateClient(options, bulk: false);
        bulkClient = CreateClient(options, bulk: true);
        database = client.GetDatabase(options.Database);
        foreach (string name in DocumentWorkload.Collections) containers[name] = database.GetContainer(name);
    }
    private static CosmosClient CreateClient(TargetOptions options, bool bulk)
        => new(options.Endpoint, options.Settings["accountKey"], new CosmosClientOptions
        {
            ConnectionMode = ConnectionMode.Gateway, AllowBulkExecution = bulk, LimitToEndpoint = true,
            // This target is exclusively the local, unranked emulator with its self-signed development certificate.
            HttpClientFactory = () => new HttpClient(new HttpClientHandler
            { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator })
        });
    public override async ValueTask PrepareAsync(DatasetManifest manifest, CancellationToken cancellationToken)
    {
        try { await database.DeleteAsync(cancellationToken: cancellationToken); }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound) { }
        await client.CreateDatabaseAsync(Options.Database, cancellationToken: cancellationToken);
        foreach (string collection in DocumentWorkload.Collections)
        {
            var properties = new ContainerProperties(collection, "/id")
            { IndexingPolicy = new IndexingPolicy { IndexingMode = IndexingMode.None, Automatic = false } };
            await database.CreateContainerAsync(properties, cancellationToken: cancellationToken);
        }
    }
    private static Collection<CompositePath> Composite(params (string Path, CompositePathSortOrder Order)[] paths)
    {
        var result = new Collection<CompositePath>();
        foreach (var path in paths) result.Add(new() { Path = path.Path, Order = path.Order });
        return result;
    }
    private static IndexingPolicy Policy(string collection)
    {
        var policy = new IndexingPolicy { IndexingMode = IndexingMode.Consistent, Automatic = true };
        policy.ExcludedPaths.Add(new() { Path = "/*" });
        policy.IncludedPaths.Add(new() { Path = "/key/?" });
        // Identity already has the service's built-in index. The pinned emulator rejects an explicit /id/? path.
        const CompositePathSortOrder asc = CompositePathSortOrder.Ascending, desc = CompositePathSortOrder.Descending;
        string[] paths = collection.Replace("write_", "", StringComparison.Ordinal) switch
        {
            "orders" => ["/customer/id/?", "/placedAt/?", "/status/?"],
            "products" => ["/category/?", "/variants/[]/price/?", "/name/?"],
            "reviews" => ["/productId/?", "/createdAt/?"], _ => []
        };
        foreach (string path in paths) policy.IncludedPaths.Add(new() { Path = path });
        switch (collection.Replace("write_", "", StringComparison.Ordinal))
        {
            case "orders":
                policy.CompositeIndexes.Add(Composite(("/customer/id", asc), ("/placedAt", desc), ("/key", asc)));
                policy.CompositeIndexes.Add(Composite(("/status", asc), ("/placedAt", asc), ("/key", asc)));
                policy.CompositeIndexes.Add(Composite(("/placedAt", desc), ("/key", asc)));
                policy.CompositeIndexes.Add(Composite(("/placedAt", asc), ("/key", asc))); break;
            case "products": policy.CompositeIndexes.Add(Composite(("/name", asc), ("/key", asc))); break;
            case "reviews": policy.CompositeIndexes.Add(Composite(("/productId", asc), ("/createdAt", asc))); break;
        }
        return policy;
    }
    public override async ValueTask<IReadOnlyList<LoadMetric>> FinalizeLoadAsync(CancellationToken cancellationToken)
    {
        var watch = Stopwatch.StartNew();
        var observed = new List<string>();
        foreach (string collection in DocumentWorkload.Collections)
        {
            var container = database.GetContainer(collection);
            await container.ReplaceContainerAsync(new ContainerProperties(collection, "/id") { IndexingPolicy = Policy(collection) }, cancellationToken: cancellationToken);
            using var deadline = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            deadline.CancelAfter(TimeSpan.FromMinutes(10));
            while (true)
            {
                var response = await container.ReadContainerAsync(new ContainerRequestOptions { PopulateQuotaInfo = true }, deadline.Token);
                string progress = response.Headers["x-ms-documentdb-collection-index-transformation-progress"];
                if (string.IsNullOrEmpty(progress) || progress == "100")
                {
                    observed.Add(collection + ": returned mode=" + response.Resource.IndexingPolicy.IndexingMode + ", progress=" + (string.IsNullOrEmpty(progress) ? "not reported" : progress));
                    break;
                }
                await Task.Delay(200, deadline.Token);
            }
        }
        return new[] { new LoadMetric("*", "index-policy-request", "Requested consistent scalar/array/composite paths; emulator may accept updates as no-ops. " + string.Join("; ", observed), 0, 0, watch.Elapsed.TotalSeconds) };
    }
    public override async ValueTask<EngineDescription> DescribeAsync(CancellationToken cancellationToken)
    {
        var account = await client.ReadAccountAsync(); cancellationToken.ThrowIfCancellationRequested();
        return new("Cosmos emulator vnext-preview / SDK " + typeof(CosmosClient).Assembly.GetName().Version,
            new Dictionary<string, string> { ["ConnectionMode"] = "Gateway", ["AllowBulkExecution"] = "load only; false for individual measured operations",
                ["partitionKey"] = "/id (one logical partition per encoded document key)", ["consistency"] = account.Consistency.DefaultConsistencyLevel.ToString(),
                ["idEncoding"] = "id = base64url(logical key); key retains original collection/n", ["ranked"] = "false",
                ["indexPolicyCapability"] = "Emulator custom range/composite policies may be accepted without execution; see observed finalization metrics and vendor emulator feature matrix",
                ["storageCapability"] = "Pinned emulator reports zero size and zero count for populated containers; storage is unavailable",
                ["categoryPriceIndex"] = "Separate category and multivalue price paths; Cosmos composite indexes cannot contain array paths" });
    }
    // ReadContainer(PopulateQuotaInfo=true) reports documentsSize=0 and documentsCount=0 after a full load.
    // Preserve unknown storage in the result instead of publishing the emulator's false zero as a measurement.
    public override ValueTask<long> MeasureStorageAsync(CancellationToken cancellationToken)
    { cancellationToken.ThrowIfCancellationRequested(); return ValueTask.FromResult(-1L); }
    public override async ValueTask PingAsync(CancellationToken cancellationToken)
    { _ = await client.ReadAccountAsync(); cancellationToken.ThrowIfCancellationRequested(); }
    public override ValueTask DisposeAsync() { bulkClient.Dispose(); client.Dispose(); return ValueTask.CompletedTask; }
}
