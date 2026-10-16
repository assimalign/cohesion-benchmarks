using System;
using System.Collections.Generic;
using System.Globalization;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

internal sealed class SqlParameterStream : IParameterStream
{
    private readonly CommerceDescriptor commerce;
    private readonly string operation;
    private readonly int client;
    private readonly ParameterStreamContext context;
    private readonly Dictionary<string, long> maximum = new(StringComparer.Ordinal);
    private readonly ZipfDistribution customerZipf, productZipf;
    private readonly IdPermutation customers, products;
    private EntityRandom random;
    private long sequence;

    internal SqlParameterStream(DatasetManifest manifest, ulong seed, string operation, int client, ParameterStreamContext context)
    {
        commerce = new(manifest); this.operation = operation; this.client = client; this.context = context;
        random = new(seed, "sql.parameters." + operation, client);
        customerZipf = new(commerce.Customers); productZipf = new(commerce.Products);
        customers = new(commerce.Customers, manifest.Seed, CounterRandom.Stream("commerce.customer.popularity"));
        products = new(commerce.Products, manifest.Seed, CounterRandom.Stream("commerce.product.popularity"));
        foreach (DatasetTableManifest table in manifest.Tables) maximum.Add(table.Name, table.MaximumId);
    }

    public object Next()
    {
        long seq = sequence++;
        if (operation == "sql.tx.place-order") return PlaceOrder(seq);
        if (operation == "sql.tx.ship-order") return ShipOrder(seq);
        if (operation == "sql.update-customer") return UpdateCustomer();
        long customer = customers.Permute(customerZipf.Sample(ref random));
        long product = products.Permute(productZipf.Sample(ref random));
        long order = random.Long(1, commerce.Orders);
        int year = random.Int(2021, 2025), quarter = random.Int(0, 3);
        bool quarterly = operation is "sql.a.revenue-by-channel" or "sql.a.top-products-in-category";
        DateTime start = new(year, quarterly ? quarter * 3 + 1 : 1, 1, 0, 0, 0, DateTimeKind.Utc);
        long id = operation switch
        {
            "sql.order-lines" or "sql.order-detail" => order,
            "sql.product-review-summary" or "sql.product-latest-reviews" or "sql.inventory-by-product" => product,
            _ => customer
        };
        string name = commerce.Product(product).Name;
        var parameters = P(("id", id), ("email", commerce.CustomerEmail(customer)), ("start", start),
            ("end", quarterly ? start.AddMonths(3) : start.AddYears(1)), ("category", (long)random.Int(71, 400)),
            ("warehouse", (long)random.Int(1, 20)), ("term", "%" + name[(name.LastIndexOf(' ') + 1)..] + "%"));
        return new SqlWorkloadParameters
        {
            Statements = [new(SqlQueries.Text(operation, SqlDialect.Cohesion), parameters,
                SqlQueries.Text(operation, SqlDialect.SqlServer), SqlQueries.Text(operation, SqlDialect.PostgreSql))],
            Monthly = operation is "sql.a.revenue-by-month" or "sql.a.monthly-growth-cte" ? new(year, operation == "sql.a.monthly-growth-cte") : null
        };
    }

    private long Partitioned(long count)
    {
        if (count < context.Concurrency) throw new InvalidOperationException("Write workload needs at least one entity per client.");
        long slots = (count - 1 - client) / context.Concurrency + 1;
        return client + 1 + context.Concurrency * random.Long(0, slots - 1);
    }

    private SqlWorkloadParameters UpdateCustomer()
    {
        long id = Partitioned(commerce.Customers);
        var parameters = P(("id", id), ("city", "Benchmark City " + random.Int(1, 20)));
        return new()
        {
            Statements = [new("UPDATE customers SET city = @city WHERE customer_id = @id", parameters)],
            Verification = [new("SELECT customer_id, city FROM customers WHERE customer_id = @id", parameters)]
        };
    }

    private SqlWorkloadParameters ShipOrder(long seq)
    {
        // Disjoint client stripes avoid making lock collisions an accidental workload parameter.
        if (commerce.Orders < context.Concurrency) throw new InvalidOperationException("Write workload needs one order per client.");
        long slots = (commerce.Orders - 1 - client) / context.Concurrency + 1;
        long order = client + 1 + context.Concurrency * (seq % slots);
        long shipment = context.WriteId(maximum["shipments"], client, seq);
        long history = context.WriteId(5, client, seq);
        var parameters = P(("id", order), ("shipment", shipment), ("seq", history), ("warehouse", (long)random.Int(1, 20)),
            ("time", DatasetTime.End.AddDays(1).AddSeconds(seq % 86400)),
            ("tracking", "BENCH-" + shipment.ToString(CultureInfo.InvariantCulture)));
        return new()
        {
            Transaction = true,
            Statements =
            [
                new("UPDATE orders SET status = 'shipped' WHERE order_id = @id", parameters),
                new("INSERT INTO shipments (shipment_id,order_id,warehouse_id,carrier,tracking_no,shipped_at,delivered_at) VALUES (@shipment,@id,@warehouse,'Benchmark Carrier',@tracking,@time,NULL)", parameters),
                new("INSERT INTO order_status_history (order_id,seq,status,occurred_at,note) VALUES (@id,@seq,'shipped',@time,NULL)", parameters)
            ],
            Verification =
            [
                new("SELECT order_id, status FROM orders WHERE order_id = @id", parameters),
                new("SELECT * FROM shipments WHERE shipment_id = @shipment", parameters),
                new("SELECT * FROM order_status_history WHERE order_id = @id AND seq = @seq", parameters)
            ]
        };
    }

    private SqlWorkloadParameters PlaceOrder(long seq)
    {
        long id = context.WriteId(maximum["orders"], client, seq);
        long payment = context.WriteId(maximum["payments"], client, seq);
        long customer = customers.Permute(customerZipf.Sample(ref random));
        DateTime time = DatasetTime.End.AddDays(1).AddSeconds(seq % 86400);
        int lines = random.Int(1, 6);
        decimal subtotal = 0;
        var inserts = new List<SqlStatement>();
        var updates = new List<SqlStatement>();
        for (short line = 1; line <= lines; line++)
        {
            long product = Partitioned(commerce.Products), warehouse = 1 + product % 20;
            int quantity = random.Int(1, 4);
            decimal price = commerce.Product(product).Price, total = quantity * price;
            subtotal += total;
            var item = P(("id", id), ("line", line), ("product", product), ("quantity", quantity), ("price", price), ("total", total), ("warehouse", warehouse));
            inserts.Add(new("INSERT INTO order_items (order_id,line_no,product_id,quantity,unit_price,discount,line_total) VALUES (@id,@line,@product,@quantity,@price,0,@total)", item));
            updates.Add(new("UPDATE inventory SET quantity_on_hand = quantity_on_hand - @quantity WHERE warehouse_id = @warehouse AND product_id = @product", item));
        }
        decimal tax = decimal.Round(subtotal * 0.07m, 2, MidpointRounding.AwayFromZero);
        var p = P(("id", id), ("customer", customer), ("time", time), ("subtotal", subtotal), ("tax", tax),
            ("total", subtotal + tax), ("payment", payment), ("reference", "BENCH-" + payment.ToString(CultureInfo.InvariantCulture)));
        var statements = new List<SqlStatement>
        {
            new("INSERT INTO orders (order_id,customer_id,order_date,status,channel,shipping_address_id,currency,subtotal,tax,shipping_fee,total,promo_code) VALUES (@id,@customer,@time,'paid','web',@customer,'USD',@subtotal,@tax,0,@total,NULL)", p)
        };
        statements.AddRange(inserts);
        statements.Add(new("INSERT INTO payments (payment_id,order_id,method,amount,status,paid_at,provider_ref) VALUES (@payment,@id,'card',@total,'succeeded',@time,@reference)", p));
        statements.Add(new("INSERT INTO order_status_history (order_id,seq,status,occurred_at,note) VALUES (@id,1,'paid',@time,NULL)", p));
        statements.AddRange(updates);
        return new()
        {
            Transaction = true, Statements = statements.ToArray(),
            Verification =
            [
                new("SELECT * FROM orders WHERE order_id = @id", p),
                new("SELECT * FROM order_items WHERE order_id = @id ORDER BY line_no", p),
                new("SELECT * FROM payments WHERE order_id = @id ORDER BY payment_id", p),
                new("SELECT * FROM order_status_history WHERE order_id = @id ORDER BY seq", p),
                new("SELECT i.warehouse_id, i.product_id, i.quantity_on_hand FROM inventory i INNER JOIN order_items l ON i.product_id = l.product_id WHERE l.order_id = @id ORDER BY i.warehouse_id, i.product_id, l.line_no", p)
            ]
        };
    }

    private static Dictionary<string, object?> P(params (string Name, object? Value)[] values)
    {
        var result = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var value in values) result.Add(value.Name, value.Value);
        return result;
    }
}
