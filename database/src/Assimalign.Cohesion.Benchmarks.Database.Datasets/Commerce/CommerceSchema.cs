using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>The portable commerce schema, in dependency/load order, with the DESIGN §5.2 secondary indexes.</summary>
public static class CommerceSchema
{
    private static SchemaColumn B(string n) => new(n, LogicalType.BigInt);
    private static SchemaColumn I(string n) => new(n, LogicalType.Integer);
    private static SchemaColumn S(string n) => new(n, LogicalType.SmallInt);
    private static SchemaColumn V(string n, int length, bool nullable = false) => new(n, LogicalType.VarChar, nullable, length);
    private static SchemaColumn M(string n) => new(n, LogicalType.Numeric, Precision: 12, Scale: 2);
    private static SchemaColumn T(string n, bool nullable = false) => new(n, LogicalType.Timestamp, nullable);
    private static SchemaColumn F(string n) => new(n, LogicalType.Boolean);
    private static SchemaIndex X(string table, bool unique, params string[] columns) => new("ix_" + table + "_" + string.Join("_", columns), columns, unique);

    public static IReadOnlyList<SchemaTable> Tables { get; } = new SchemaTable[]
    {
        new("warehouses", [B("warehouse_id"), V("name",100), V("region",100), V("country_code",2)], ["warehouse_id"], []),
        new("categories", [B("category_id"), new("parent_id",LogicalType.BigInt,true), V("name",100)], ["category_id"], []),
        new("suppliers", [B("supplier_id"), V("name",160), V("country_code",2), M("rating")], ["supplier_id"], []),
        new("customers", [B("customer_id"), V("email",254), V("first_name",80), V("last_name",80), V("phone",30,true),
            new("birth_date",LogicalType.Date), V("country_code",2), V("city",100), T("created_at"), V("loyalty_tier",20),
            F("marketing_opt_in"), M("lifetime_value")], ["customer_id"], [X("customers",true,"email")]),
        new("customer_addresses", [B("address_id"), B("customer_id"), V("kind",20), V("line1",200), V("line2",200,true),
            V("city",100), V("region",100), V("postal_code",20), V("country_code",2), F("is_default")], ["address_id"], [X("customer_addresses",false,"customer_id")]),
        new("products", [B("product_id"), V("sku",40), V("name",200), new("description",LogicalType.Text), B("category_id"),
            B("supplier_id"), V("brand",100), M("unit_price"), M("cost"), I("weight_grams"), T("created_at"), F("is_active")],
            ["product_id"], [X("products",true,"sku"), X("products",false,"category_id"), X("products",false,"supplier_id")]),
        new("inventory", [B("warehouse_id"), B("product_id"), I("quantity_on_hand"), I("reorder_point"), T("updated_at")],
            ["warehouse_id","product_id"], [X("inventory",false,"product_id")]),
        new("orders", [B("order_id"), B("customer_id"), T("order_date"), V("status",20), V("channel",20), B("shipping_address_id"),
            V("currency",3), M("subtotal"), M("tax"), M("shipping_fee"), M("total"), V("promo_code",40,true)], ["order_id"],
            [X("orders",false,"customer_id","order_date"), X("orders",false,"order_date"), X("orders",false,"status","order_date")]),
        new("order_items", [B("order_id"), S("line_no"), B("product_id"), I("quantity"), M("unit_price"), M("discount"), M("line_total")],
            ["order_id","line_no"], [X("order_items",false,"product_id")]),
        new("payments", [B("payment_id"), B("order_id"), V("method",30), M("amount"), V("status",20), T("paid_at"), V("provider_ref",80)],
            ["payment_id"], [X("payments",false,"order_id")]),
        new("shipments", [B("shipment_id"), B("order_id"), B("warehouse_id"), V("carrier",40), V("tracking_no",80), T("shipped_at"), T("delivered_at",true)],
            ["shipment_id"], [X("shipments",false,"order_id")]),
        new("reviews", [B("review_id"), B("product_id"), B("customer_id"), S("rating"), V("title",200), new("body",LogicalType.Text),
            T("created_at"), I("helpful_votes")], ["review_id"], [X("reviews",false,"product_id","created_at"), X("reviews",false,"customer_id")]),
        new("order_status_history", [B("order_id"), S("seq"), V("status",20), T("occurred_at"), V("note",500,true)], ["order_id","seq"], [])
    };

    public static SchemaTable Table(string name)
    {
        foreach (SchemaTable table in Tables) if (table.Name == name) return table;
        throw new KeyNotFoundException(name);
    }

    public static IReadOnlyList<string> LoadOrder { get; } = BuildOrder();
    private static string[] BuildOrder()
    {
        var names = new string[Tables.Count];
        for (int i = 0; i < names.Length; i++) names[i] = Tables[i].Name;
        return names;
    }
}
