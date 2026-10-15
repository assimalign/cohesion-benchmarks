using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>Marketplace collections in load order and equivalent indexes for every document engine.</summary>
public static class MarketplaceSchema
{
    public static IReadOnlyList<SchemaCollection> Collections { get; } = new SchemaCollection[]
    {
        new("customers","customers/<n>",[]),
        new("products","products/<n>",[
            new("ix_products_category_price",["category","variants[].price"]),
            new("ix_products_name",["name"])]),
        new("orders","orders/<n>",[
            new("ix_orders_customer_date",["customer.id","placedAt"]),
            new("ix_orders_status_date",["status","placedAt"])]),
        new("reviews","reviews/<n>",[new("ix_reviews_product_date",["productId","createdAt"])])
    };
    public static IReadOnlyList<string> LoadOrder { get; } = new[] { "customers","products","orders","reviews" };
}
