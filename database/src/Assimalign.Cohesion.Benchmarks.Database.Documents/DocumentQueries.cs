using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Azure.Cosmos;
using MongoDB.Bson;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal static class DocumentQueries
{
    private const string OrderOql = "d.id AS id, d.customer.id AS customerId, d.placedAt AS placedAt, d.status AS status, d.total AS total";
    private const string ProductOql = "d.id AS id, d.name AS name, d.category AS category, d.variants[0].price AS price";
    private const string OrderRql = "{ id: id(d), customerId: d.customer.id, placedAt: d.placedAt, status: d.status, total: d.total }";
    private const string ProductRql = "{ id: id(d), name: d.name, category: d.category, price: d.variants[0].price }";
    private const string OrderCosmos = "c.key AS id, c.customer.id AS customerId, c.placedAt, c.status, c.total";
    private const string ProductCosmos = "c.key AS id, c.name, c.category, c.variants[0].price AS price";
    private static BsonDocument OrderMongo() => new() { ["_id"] = 0, ["id"] = 1, ["customerId"] = "$customer.id", ["placedAt"] = 1, ["status"] = 1, ["total"] = 1 };
    private static BsonDocument ProductMongo() => new() { ["_id"] = 0, ["id"] = 1, ["name"] = 1, ["category"] = 1, ["price"] = new BsonDocument("$arrayElemAt", new BsonArray { "$variants.price", 0 }) };
    internal static void Build(string operation, DocumentParameters p)
    {
        p.Values = new() { ["key"] = p.Key, ["customer"] = p.Customer, ["category"] = p.Category, ["status"] = p.Status,
            ["start"] = p.Start, ["end"] = p.End, ["prefix"] = p.Prefix, ["upper"] = p.Prefix + '\uffff', ["price"] = p.Price, ["ids"] = p.Keys };
        var filter = new BsonDocument(); var pipeline = new List<BsonDocument>();
        string? rql = null, cosmos = null;
        switch (operation)
        {
            case "doc.order-by-id":
                p.RavenPath = "/docs?id=" + Uri.EscapeDataString(p.Key); break;
            case "doc.products-multiget":
                filter["_id"] = new BsonDocument("$in", new BsonArray(p.Keys));
                pipeline.Add(new("$match", filter)); pipeline.Add(new("$sort", new BsonDocument("id", 1))); pipeline.Add(new("$project", ProductMongo()));
                p.RavenPath = "/docs?";
                var names = new string[p.Keys.Length];
                for (int i = 0; i < p.Keys.Length; i++)
                { p.RavenPath += (i == 0 ? "" : "&") + "id=" + Uri.EscapeDataString(p.Keys[i]); names[i] = "@id" + i; p.Values["id" + i] = p.Keys[i]; }
                cosmos = "SELECT " + ProductCosmos + " FROM c WHERE c.key IN (" + string.Join(',', names) + ") ORDER BY c.key"; break;
            case "doc.customer-recent-orders":
                filter["customer.id"] = p.Customer;
                pipeline.Add(new("$match", filter)); pipeline.Add(new("$sort", new BsonDocument { ["placedAt"] = -1, ["id"] = 1 }));
                pipeline.Add(new("$limit", 20)); pipeline.Add(new("$project", OrderMongo()));
                p.Oql = "SELECT " + OrderOql + " FROM orders d WHERE d.customer.id = @customer ORDER BY d.placedAt DESC, d.id";
                rql = "from index 'Bench/orders/CustomerDate' as d where exact(Customer = $customer) order by PlacedAt desc, Key select " + OrderRql + " limit 20";
                cosmos = "SELECT " + OrderCosmos + " FROM c WHERE c.customer.id = @customer ORDER BY c.placedAt DESC, c.key ASC OFFSET 0 LIMIT 20"; break;
            case "doc.products-by-category-price":
                filter["category"] = p.Category; filter["variants.price"] = new BsonDocument("$lte", new BsonDecimal128(p.Price));
                pipeline.Add(new("$match", filter)); pipeline.Add(new("$sort", new BsonDocument("id", 1)));
                pipeline.Add(new("$limit", 50)); pipeline.Add(new("$project", ProductMongo()));
                rql = "from index 'Bench/products/CategoryPrice' as d where exact(Category = $category) and Prices <= $price order by Key select " + ProductRql + " limit 50";
                cosmos = "SELECT " + ProductCosmos + " FROM c WHERE c.category = @category AND EXISTS(SELECT VALUE v FROM v IN c.variants WHERE v.price <= @price) ORDER BY c.key OFFSET 0 LIMIT 50"; break;
            case "doc.orders-status-range":
                filter["status"] = p.Status; filter["placedAt"] = new BsonDocument { ["$gte"] = p.Start, ["$lt"] = p.End };
                pipeline.Add(new("$match", filter));
                pipeline.Add(new("$facet", new BsonDocument
                {
                    ["count"] = new BsonArray { new BsonDocument("$count", "value") },
                    ["page"] = new BsonArray { new BsonDocument("$sort", new BsonDocument { ["placedAt"] = 1, ["id"] = 1 }), new BsonDocument("$limit", 25), new BsonDocument("$project", OrderMongo()) }
                }));
                p.Oql = "SELECT " + OrderOql + " FROM orders d WHERE d.status = @status AND d.placedAt >= @start AND d.placedAt < @end ORDER BY d.placedAt, d.id";
                rql = "from index 'Bench/orders/StatusDate' as d where exact(Status = $status) and PlacedAt >= $start and PlacedAt < $end order by PlacedAt, Key select " + OrderRql + " limit 25";
                cosmos = "SELECT " + OrderCosmos + " FROM c WHERE c.status = @status AND c.placedAt >= @start AND c.placedAt < @end ORDER BY c.placedAt, c.key"; break;
            case "doc.products-name-prefix":
                filter["name"] = new BsonRegularExpression("^" + Regex.Escape(p.Prefix));
                pipeline.Add(new("$match", filter)); pipeline.Add(new("$sort", new BsonDocument { ["name"] = 1, ["id"] = 1 }));
                pipeline.Add(new("$limit", 50)); pipeline.Add(new("$project", ProductMongo()));
                p.Oql = "SELECT " + ProductOql + " FROM products d WHERE d.name >= @prefix AND d.name < @upper ORDER BY d.name, d.id";
                rql = "from index 'Bench/products/Name' as d where exact(startsWith(Name, $prefix)) order by Name, Key select " + ProductRql + " limit 50";
                cosmos = "SELECT " + ProductCosmos + " FROM c WHERE STARTSWITH(c.name, @prefix, false) ORDER BY c.name, c.key OFFSET 0 LIMIT 50"; break;
            case "doc.revenue-by-category-30d":
                filter["placedAt"] = new BsonDocument { ["$gte"] = p.Start, ["$lt"] = p.End };
                pipeline.Add(new("$match", filter)); pipeline.Add(new("$unwind", "$items"));
                pipeline.Add(new("$group", new BsonDocument { ["_id"] = "$items.category", ["revenue"] = new BsonDocument("$sum", new BsonDocument("$multiply", new BsonArray { "$items.quantity", "$items.price" })) }));
                pipeline.Add(new("$sort", new BsonDocument("_id", 1)));
                pipeline.Add(new("$project", new BsonDocument { ["_id"] = 0, ["category"] = "$_id", ["revenue"] = 1 }));
                p.Oql = "SELECT d.id AS id, d.items AS items FROM orders d WHERE d.placedAt >= @start AND d.placedAt < @end";
                rql = "from index 'Bench/orders/Date' as d where PlacedAt >= $start and PlacedAt < $end select { id: id(d), items: d.items } limit 2147483647";
                cosmos = "SELECT i.category, SUM(i.quantity * i.price) AS revenue FROM c JOIN i IN c.items WHERE c.placedAt >= @start AND c.placedAt < @end GROUP BY i.category"; break;
            case "doc.insert-order":
            case "doc.replace-customer":
                p.MongoJson = DocumentJson.Mongo(p.Json);
                p.RavenJson = DocumentJson.Rewrite(p.Json, ravenCollection: p.Collection);
                p.CosmosJson = DocumentJson.Rewrite(p.Json, cosmos: true); break;
            case "doc.patch-order-status":
                var history = new Dictionary<string, object> { ["status"] = p.Status, ["occurredAt"] = p.OccurredAt };
                p.MongoUpdate = new() { ["$set"] = new BsonDocument("status", p.Status), ["$push"] = new BsonDocument("statusHistory", new BsonDocument { ["status"] = p.Status, ["occurredAt"] = p.OccurredAt }) };
                p.CosmosPatch = new[] { PatchOperation.Set("/status", p.Status), PatchOperation.Add("/statusHistory/-", history) };
                p.RavenPatch = JsonSerializer.SerializeToUtf8Bytes(new { Patch = new { Script = "this.status = args.status; this.statusHistory.push({status: args.status, occurredAt: args.at});", Values = new { status = p.Status, at = p.OccurredAt } } }); break;
            case "doc.adjust-variant-stock":
                p.MongoUpdate = new("$inc", new BsonDocument("variants.0.stock", p.Delta));
                p.CosmosPatch = new[] { PatchOperation.Increment("/variants/0/stock", p.Delta) };
                p.RavenPatch = JsonSerializer.SerializeToUtf8Bytes(new { Patch = new { Script = "this.variants[0].stock += args.delta;", Values = new { delta = p.Delta } } }); break;
            default: throw new ArgumentException("Unknown document operation: " + operation);
        }
        p.MongoFilter = new("_id", p.Key); p.MongoPipeline = pipeline.ToArray(); p.CosmosKey = DocumentJson.CosmosId(p.Key);
        if (rql != null) p.RavenQuery = JsonSerializer.SerializeToUtf8Bytes(new { Query = rql, QueryParameters = p.Values,
            WaitForNonStaleResults = true, WaitForNonStaleResultsTimeout = "00:05:00", DisableCaching = true });
        if (cosmos != null)
        {
            p.CosmosQuery = new QueryDefinition(cosmos);
            foreach (var pair in p.Values) p.CosmosQuery.WithParameter("@" + pair.Key, pair.Value);
        }
        if (p.RavenPath.Length == 0) p.RavenPath = "/docs?id=" + Uri.EscapeDataString(p.Key);
    }
}
