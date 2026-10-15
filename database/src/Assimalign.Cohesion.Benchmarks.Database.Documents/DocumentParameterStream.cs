using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed class DocumentParameterStream : IParameterStream
{
    private readonly MarketplaceDescriptor data;
    private readonly DeterministicSequence random;
    private readonly string operation;
    private readonly int client;
    private readonly ParameterStreamContext context;
    private long sequence;
    internal DocumentParameterStream(DatasetManifest manifest, ulong seed, string operation, int client, ParameterStreamContext context)
    { data = new(manifest); random = new(seed, operation, client); this.operation = operation; this.client = client; this.context = context; }
    private long Pick(long count) => 1 + (long)(random.Next() % (ulong)count);
    internal static string Fixture(string collection, int client, bool warmup = false) => collection + "/fixture/" + (warmup ? "warmup/" : "") + client.ToString(CultureInfo.InvariantCulture);
    internal static string Timestamp(DateTime value) => value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
    public object Next()
    {
        long step = sequence++;
        long order = Pick(data.Orders), product = Pick(data.Products);
        var orderInfo = data.Order(order); var productInfo = data.Product(product);
        var p = new DocumentParameters
        {
            Key = MarketplaceDescriptor.Key("orders", order), Collection = "orders",
            Customer = MarketplaceDescriptor.Key("customers", orderInfo.Customer), Category = productInfo.Category,
            Status = orderInfo.Status, Price = data.Variant(product, 0).Price,
            Prefix = productInfo.Name[..Math.Min(3, productInfo.Name.Length)],
            Start = Timestamp(orderInfo.Date.Date), End = Timestamp(orderInfo.Date.Date.AddDays(30)),
            OccurredAt = Timestamp(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(step)),
            Delta = step % 2 == 0 ? 1 : -1
        };
        // Empty, case-mismatched predicates are intentional parity checks, not a different collation.
        if (step % 4 == 1) p.Prefix = p.Prefix.ToUpperInvariant();
        if (step % 10 == 1) { p.Category = p.Category.ToUpperInvariant(); p.Status = p.Status.ToUpperInvariant(); }
        if (operation == "doc.revenue-by-category-30d")
        {
            var end = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(-(int)(step % 12) * 30);
            p.End = Timestamp(end); p.Start = Timestamp(end.AddDays(-30));
        }
        if (operation == "doc.products-multiget")
        {
            var unique = new HashSet<string>(StringComparer.Ordinal);
            while (unique.Count < Math.Min(10, data.Products)) unique.Add(MarketplaceDescriptor.Key("products", Pick(data.Products)));
            p.Keys = new string[unique.Count]; unique.CopyTo(p.Keys); Array.Sort(p.Keys, StringComparer.Ordinal); p.Collection = "products";
        }
        if (operation is "doc.products-name-prefix" or "doc.products-by-category-price") p.Collection = "products";
        if (operation == "doc.insert-order")
        {
            p.Collection = "write_orders"; p.Key = MarketplaceDescriptor.Key("orders", context.WriteId(data.Orders, client, step));
            p.Json = NewOrder(p, product, orderInfo.Customer);
        }
        if (operation == "doc.patch-order-status") { p.Collection = "write_orders"; p.Key = Fixture(p.Collection, client, context.Warmup); p.Status = step % 2 == 0 ? "paid" : "shipped"; }
        if (operation == "doc.adjust-variant-stock") { p.Collection = "write_products"; p.Key = Fixture(p.Collection, client, context.Warmup); }
        if (operation == "doc.replace-customer")
        {
            p.Collection = "write_customers"; p.Key = Fixture(p.Collection, client, context.Warmup);
            var identity = data.Customer(Pick(data.Customers));
            var root = new JsonObject { ["id"] = p.Key, ["firstName"] = identity.First, ["lastName"] = identity.Last,
                ["email"] = identity.Email, ["createdAt"] = "2021-01-01T00:00:00Z", ["phone"] = "+1-555-1234567",
                ["addresses"] = new JsonArray(new JsonObject { ["kind"] = "home", ["line1"] = "42 Market Street", ["city"] = identity.City.Name, ["country"] = identity.City.CountryCode, ["isDefault"] = true }),
                ["preferences"] = new JsonObject { ["locale"] = "en", ["currency"] = "USD", ["emailReceipts"] = true, ["categories"] = new JsonArray("books", "electronics") },
                ["devices"] = new JsonArray(new JsonObject { ["id"] = "benchmark-phone", ["type"] = "phone", ["platform"] = "browser", ["lastSeenAt"] = p.OccurredAt }),
                ["loyalty"] = new JsonObject { ["tier"] = step % 2 == 0 ? "gold" : "standard", ["points"] = step % 20000, ["lifetimeValue"] = 1234.56m },
                ["consent"] = new JsonObject { ["marketing"] = step % 2 == 0, ["analytics"] = true, ["personalization"] = true } };
            p.Json = JsonSerializer.SerializeToUtf8Bytes(root);
        }
        DocumentQueries.Build(operation, p);
        return p;
    }
    private byte[] NewOrder(DocumentParameters p, long product, long customer)
    {
        var identity = data.Customer(customer); var info = data.Product(product); var variant = data.Variant(product, 0);
        decimal total = variant.Price * 2;
        var root = new JsonObject { ["id"] = p.Key, ["customer"] = new JsonObject { ["id"] = p.Customer, ["firstName"] = identity.First, ["lastName"] = identity.Last, ["email"] = identity.Email },
            ["shippingAddress"] = new JsonObject { ["kind"] = "home", ["line1"] = "42 Market Street", ["city"] = identity.City.Name, ["country"] = identity.City.CountryCode, ["isDefault"] = true },
            ["items"] = new JsonArray(new JsonObject { ["productId"] = MarketplaceDescriptor.Key("products", product), ["name"] = info.Name, ["category"] = info.Category, ["variant"] = variant.Sku, ["quantity"] = 2, ["price"] = variant.Price, ["lineTotal"] = total }),
            ["payment"] = new JsonObject { ["method"] = "card", ["status"] = "pending", ["amount"] = total },
            ["statusHistory"] = new JsonArray(new JsonObject { ["status"] = "placed", ["occurredAt"] = p.OccurredAt }),
            ["currency"] = "USD", ["subtotal"] = total, ["tax"] = 0m, ["shippingFee"] = 0m, ["total"] = total,
            ["placedAt"] = p.OccurredAt, ["status"] = "placed" };
        return JsonSerializer.SerializeToUtf8Bytes(root);
    }
}
