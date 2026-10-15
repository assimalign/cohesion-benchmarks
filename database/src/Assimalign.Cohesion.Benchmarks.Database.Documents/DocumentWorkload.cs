using System;
using System.Collections.Generic;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal static class DocumentWorkload
{
    internal static readonly OperationDescriptor[] Operations =
    [
        new("doc.order-by-id", "point read", true, TimeoutClass.Point, 25, 15),
        new("doc.products-multiget", "point read", true, TimeoutClass.Point, 20, 10),
        new("doc.customer-recent-orders", "index query", true, TimeoutClass.Point, 15, 8),
        new("doc.products-by-category-price", "multi-value query", true, TimeoutClass.Point, 10, 7),
        new("doc.orders-status-range", "index query", true, TimeoutClass.Point, 10, 5),
        new("doc.products-name-prefix", "index query", true, TimeoutClass.Point, 8, 4),
        new("doc.revenue-by-category-30d", "aggregation", true, TimeoutClass.Analytics, 2, 1),
        new("doc.insert-order", "write", true, TimeoutClass.Point, 4, 20),
        new("doc.patch-order-status", "write", true, TimeoutClass.Point, 3, 15),
        new("doc.adjust-variant-stock", "write", true, TimeoutClass.Point, 2, 10),
        new("doc.replace-customer", "write", true, TimeoutClass.Point, 1, 5)
    ];
    internal static readonly string[] Collections = ["customers", "products", "orders", "reviews", "write_customers", "write_products", "write_orders"];
    internal static Dictionary<string, OperationSupport> Support(string target)
    {
        var result = new Dictionary<string, OperationSupport>(StringComparer.Ordinal);
        foreach (var operation in Operations) result[operation.Id] = new(SupportLevel.Native);
        void Emulated(string id, string reason, string evidence) => result["doc." + id] = new(SupportLevel.Emulated, reason, evidence);
        const string language = "Assimalign.Cohesion.Database.Documents.Language/docs/DESIGN.md:48-62";
        const string api = "Assimalign.Cohesion.Database.Documents/src/DocumentCollection.cs:75-154";
        if (target == "cohesion-documents-embedded")
        {
            Emulated("products-multiget", "Ten point reads; no batch API.", api);
            Emulated("customer-recent-orders", "Engine filters and orders; client takes 20 (no LIMIT).", language);
            Emulated("orders-status-range", "Engine filters and orders; client counts selected rows and takes 25 (no LIMIT).", language);
            Emulated("products-name-prefix", "Ordinal indexed range rewrite; client takes 50 (no LIMIT).", language);
            result["doc.products-by-category-price"] = new(SupportLevel.Unsupported,
                "Array existential predicate cannot run in OQL; client filtering is forbidden.", language);
            Emulated("revenue-by-category-30d", "Indexed date range selects orders; client unwinds items and aggregates.", language);
            Emulated("patch-order-status", "Read-modify-write using PutAsync with the expected version.", api);
            Emulated("adjust-variant-stock", "Read-modify-write using PutAsync with the expected version.", api);
        }
        if (target == "ravendb")
            Emulated("revenue-by-category-30d", "Static date index selects orders; client unwinds items and aggregates.", "RQL selects the bounded 30-day range; database/docs/DOCUMENTS.md");
        if (target == "cosmos-emulator")
        {
            Emulated("orders-status-range", "Engine selects and orders range; client counts and takes 25.", "Cosmos SQL has no count-plus-page result operator; database/docs/DOCUMENTS.md");
            Emulated("revenue-by-category-30d", "Engine groups selected items; client orders category totals.", "Cosmos SQL GROUP BY cannot be combined with ORDER BY; database/docs/DOCUMENTS.md");
        }
        return result;
    }
}
