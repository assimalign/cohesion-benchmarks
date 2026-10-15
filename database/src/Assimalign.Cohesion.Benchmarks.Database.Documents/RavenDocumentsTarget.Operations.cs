using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed partial class RavenDocumentsTarget
{
    public override async ValueTask<IBenchmarkClient> OpenClientAsync(int clientIndex, CancellationToken cancellationToken)
    {
        foreach (string name in new[] { "orders", "products", "customers" })
        foreach (bool warmup in new[] { false, true })
        {
            using var source = await SendAsync(HttpMethod.Get, databasePath + "/docs?id=" + Uri.EscapeDataString(name + "/1"), null, cancellationToken);
            string key = DocumentParameterStream.Fixture("write_" + name, clientIndex, warmup);
            byte[] bytes = DocumentJson.Rewrite(Encoding.UTF8.GetBytes(source.RootElement.GetProperty("Results")[0].GetRawText()), key, "write_" + name);
            using var response = await SendAsync(HttpMethod.Put, databasePath + "/docs?id=" + Uri.EscapeDataString(key), bytes, cancellationToken);
        }
        await WaitForIndexesAsync(cancellationToken);
        return new Client(this);
    }
    private sealed class Client(RavenDocumentsTarget target) : IBenchmarkClient
    {
        public async ValueTask ExecuteAsync(OperationDescriptor operation, object parameters, IVerificationSink? sink, CancellationToken cancellationToken)
        {
            var p = (DocumentParameters)parameters;
            if (operation.Category == "write")
            {
                bool put = operation.Id is "doc.insert-order" or "doc.replace-customer";
                using var response = await target.SendAsync(put ? HttpMethod.Put : HttpMethod.Patch,
                    target.databasePath + p.RavenPath, put ? p.RavenJson : p.RavenPatch, cancellationToken);
                if (sink != null)
                {
                    using var stored = await target.SendAsync(HttpMethod.Get, target.databasePath + p.RavenPath, null, cancellationToken);
                    DocumentRows.Mutation(operation.Id, stored.RootElement.GetProperty("Results")[0], sink);
                }
                return;
            }
            bool query = p.RavenQuery.Length != 0;
            using var result = await target.SendAsync(query ? HttpMethod.Post : HttpMethod.Get,
                target.databasePath + (query ? "/queries" : p.RavenPath), query ? p.RavenQuery : null, cancellationToken);
            if (operation.Id == "doc.orders-status-range" && sink != null)
                sink.AddRow(["count", result.RootElement.GetProperty("TotalResults").GetInt64()]);
            var totals = operation.Id == "doc.revenue-by-category-30d" ? new SortedDictionary<string, decimal>(StringComparer.Ordinal) : null;
            foreach (var row in result.RootElement.GetProperty("Results").EnumerateArray())
            {
                if (totals != null) DocumentRows.AddItems(row.GetProperty("items"), totals);
                else if (sink != null) { if (p.Collection == "products") DocumentRows.Product(row, sink); else DocumentRows.Order(row, sink); }
            }
            if (totals != null) DocumentRows.Revenue(totals, sink);
        }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
