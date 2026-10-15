using System;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

/// <summary>Exact read projections and total orders. Date ranges are always half-open bound parameters.</summary>
public static class SqlQueries
{
    public static string Text(string operation, SqlDialect dialect)
    {
        string year = dialect == SqlDialect.SqlServer ? "DATEPART(year, order_date)" : "EXTRACT(YEAR FROM order_date)";
        string month = dialect == SqlDialect.SqlServer ? "DATEPART(month, order_date)" : "EXTRACT(MONTH FROM order_date)";
        string sql = operation switch
        {
            "sql.customer-by-id" => "SELECT * FROM customers WHERE customer_id = @id",
            "sql.customer-by-email" => "SELECT * FROM customers WHERE email = @email",
            "sql.order-lines" => "SELECT o.order_id, o.customer_id, o.order_date, o.status, i.line_no, i.product_id, i.quantity, i.unit_price, i.discount, i.line_total FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id WHERE o.order_id = @id ORDER BY i.line_no",
            "sql.order-detail" => "SELECT o.order_id, o.customer_id, o.order_date, o.status, i.line_no, i.product_id, p.name, i.quantity, i.unit_price, i.discount, i.line_total FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id INNER JOIN products p ON i.product_id = p.product_id WHERE o.order_id = @id ORDER BY i.line_no",
            "sql.customer-recent-orders" => Limit("SELECT * FROM orders WHERE customer_id = @id ORDER BY order_date DESC, order_id DESC", 20, dialect),
            "sql.product-review-summary" => "SELECT COUNT(*), CAST(AVG(CAST(rating AS NUMERIC(18,6))) AS NUMERIC(18,6)) FROM reviews WHERE product_id = @id",
            "sql.product-latest-reviews" => Limit("SELECT * FROM reviews WHERE product_id = @id ORDER BY created_at DESC, review_id DESC", 5, dialect),
            "sql.inventory-by-product" => "SELECT warehouse_id, product_id, quantity_on_hand, reorder_point, updated_at FROM inventory WHERE product_id = @id ORDER BY warehouse_id",
            "sql.a.revenue-by-channel" => "SELECT channel, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end AND status <> 'cancelled' GROUP BY channel ORDER BY channel",
            "sql.a.revenue-by-month" when dialect == SqlDialect.Cohesion => "SELECT order_date, channel, total FROM orders WHERE order_date >= @start AND order_date < @end",
            "sql.a.revenue-by-month" => $"SELECT CAST({year} AS INTEGER) AS year_no, CAST({month} AS INTEGER) AS month_no, channel, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end GROUP BY {year}, {month}, channel ORDER BY year_no, month_no, channel",
            "sql.a.top-products-in-category" => Limit("SELECT p.product_id, p.name, SUM(i.line_total) AS revenue FROM products p INNER JOIN order_items i ON p.product_id = i.product_id INNER JOIN orders o ON i.order_id = o.order_id WHERE p.category_id = @category AND o.order_date >= @start AND o.order_date < @end AND o.status <> 'cancelled' GROUP BY p.product_id, p.name ORDER BY revenue DESC, p.product_id", 20, dialect),
            "sql.a.repeat-customers" => Limit("SELECT customer_id, COUNT(*) AS order_count, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end GROUP BY customer_id HAVING COUNT(*) >= 5 ORDER BY order_count DESC, customer_id", 100, dialect),
            "sql.a.reorder-report" => "SELECT i.warehouse_id, i.product_id, p.name, i.quantity_on_hand, i.reorder_point FROM inventory i INNER JOIN products p ON i.product_id = p.product_id WHERE i.warehouse_id = @warehouse AND i.quantity_on_hand < i.reorder_point ORDER BY i.product_id",
            "sql.a.brand-ratings" => "SELECT p.brand, COUNT(*) AS review_count, CAST(AVG(CAST(r.rating AS NUMERIC(18,6))) AS NUMERIC(18,6)) AS average_rating FROM products p INNER JOIN reviews r ON p.product_id = r.product_id GROUP BY p.brand HAVING COUNT(*) >= 100 ORDER BY p.brand",
            "sql.a.unshipped-orders" => "SELECT order_id, customer_id, order_date, total FROM orders WHERE status = 'paid' AND order_date < @start AND order_id NOT IN (SELECT order_id FROM shipments) ORDER BY order_date, order_id",
            "sql.a.orders-full-aggregate" => "SELECT COUNT(*), SUM(total), CAST(AVG(total) AS NUMERIC(18,6)) FROM orders",
            "sql.a.product-name-like" => Limit("SELECT product_id, name FROM products WHERE name LIKE @term ORDER BY product_id", 50, dialect),
            // Five joined relations. The category hierarchy is resolved from stored parent rows in the CTE.
            "sql.a.country-category-revenue" => "WITH category_roots AS (SELECT leaf.category_id, root.category_id AS root_id, root.name AS root_name FROM categories leaf INNER JOIN categories parent ON leaf.parent_id = parent.category_id INNER JOIN categories root ON parent.parent_id = root.category_id) SELECT c.country_code, k.root_id, k.root_name, SUM(i.line_total) AS revenue FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id INNER JOIN products p ON p.product_id = i.product_id INNER JOIN customers c ON c.customer_id = o.customer_id INNER JOIN category_roots k ON k.category_id = p.category_id WHERE o.status = 'delivered' AND o.order_date >= @start AND o.order_date < @end GROUP BY c.country_code, k.root_id, k.root_name ORDER BY c.country_code, k.root_id",
            "sql.a.category-rank-window" => "WITH revenues AS (SELECT p.category_id, p.product_id, SUM(i.line_total) AS revenue FROM products p INNER JOIN order_items i ON p.product_id = i.product_id GROUP BY p.category_id, p.product_id), ranked AS (SELECT category_id, product_id, revenue, RANK() OVER (PARTITION BY category_id ORDER BY revenue DESC) AS revenue_rank FROM revenues) SELECT category_id, product_id, revenue, revenue_rank FROM ranked WHERE revenue_rank <= 3 ORDER BY category_id, revenue_rank, product_id",
            "sql.a.above-country-average" => "SELECT c.customer_id, c.country_code, " + (dialect == SqlDialect.SqlServer ? "c.first_name + ' ' + c.last_name" : "c.first_name || ' ' || c.last_name") + " AS full_name, c.lifetime_value FROM customers c WHERE c.lifetime_value > (SELECT AVG(x.lifetime_value) FROM customers x WHERE x.country_code = c.country_code) ORDER BY c.country_code, c.customer_id",
            "sql.a.monthly-growth-cte" when dialect == SqlDialect.Cohesion => "SELECT order_date, channel, total FROM orders WHERE order_date >= @start AND order_date < @end",
            "sql.a.monthly-growth-cte" => $"WITH monthly AS (SELECT CAST({year} AS INTEGER) AS year_no, CAST({month} AS INTEGER) AS month_no, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end GROUP BY {year}, {month}), growth AS (SELECT year_no, month_no, revenue, LAG(revenue) OVER (ORDER BY year_no, month_no) AS previous_revenue FROM monthly) SELECT year_no, month_no, revenue, previous_revenue, revenue - previous_revenue AS growth FROM growth ORDER BY year_no, month_no",
            _ => throw new ArgumentException("Not a SQL read operation: " + operation)
        };
        return sql;
    }

    private static string Limit(string sql, int count, SqlDialect dialect) => dialect == SqlDialect.SqlServer
        ? sql + " OFFSET 0 ROWS FETCH NEXT " + count + " ROWS ONLY" : sql + " LIMIT " + count;
}
