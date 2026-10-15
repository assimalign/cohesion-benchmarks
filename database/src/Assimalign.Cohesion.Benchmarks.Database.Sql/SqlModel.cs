using System;
using System.Collections.Generic;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

/// <summary>Commerce operations, deterministic parameters, target identities and evidence-backed support.</summary>
public sealed class SqlModel : IBenchmarkModel
{
    public string Id => "sql";
    public DatasetModel DatasetModel => DatasetModel.Sql;
    public IReadOnlyList<TargetDescriptor> Targets { get; } = new TargetDescriptor[]
    {
        new("cohesion-sql-embedded", "Cohesion SQL embedded", true, "in-process", "Cohesion SQL"),
        new("cohesion-sql-tcp", "Cohesion SQL TCP", true, "TCP", "Cohesion SQL", "cohesion-sql-embedded"),
        new("postgresql", "PostgreSQL", true, "Npgsql", "PostgreSQL"),
        new("sqlserver", "SQL Server engine via Azure SQL Edge", true, "Azure SQL Edge ARM64", "SQL Server engine via Azure SQL Edge")
    };
    public IReadOnlyList<OperationDescriptor> Operations { get; } = new OperationDescriptor[]
    {
        new("sql.customer-by-id", "point read", true, TimeoutClass.Point, 20, 10),
        new("sql.customer-by-email", "point read", true, TimeoutClass.Point, 10, 5),
        new("sql.order-lines", "join read", true, TimeoutClass.Point, 10, 5),
        new("sql.order-detail", "join read", true, TimeoutClass.Point, 10, 5),
        new("sql.customer-recent-orders", "range read", true, TimeoutClass.Point, 15, 10),
        new("sql.product-review-summary", "aggregate read", true, TimeoutClass.Point, 10, 5),
        new("sql.product-latest-reviews", "top-N read", true, TimeoutClass.Point, 10, 5),
        new("sql.inventory-by-product", "range read", true, TimeoutClass.Point, 5, 5),
        new("sql.tx.place-order", "write txn", true, TimeoutClass.Point, 6, 30),
        new("sql.tx.ship-order", "write txn", true, TimeoutClass.Point, 3, 15),
        new("sql.update-customer", "write", true, TimeoutClass.Point, 1, 5),
        new("sql.a.revenue-by-channel", "analytics", true, TimeoutClass.Analytics),
        new("sql.a.revenue-by-month", "analytics", true, TimeoutClass.Analytics),
        new("sql.a.top-products-in-category", "analytics", true, TimeoutClass.Analytics),
        new("sql.a.repeat-customers", "analytics", true, TimeoutClass.Analytics),
        new("sql.a.reorder-report", "analytics", true, TimeoutClass.Analytics),
        new("sql.a.brand-ratings", "analytics", true, TimeoutClass.Analytics),
        new("sql.a.unshipped-orders", "analytics", true, TimeoutClass.Analytics),
        new("sql.a.orders-full-aggregate", "scan", true, TimeoutClass.Analytics),
        new("sql.a.product-name-like", "scan", true, TimeoutClass.Analytics),
        new("sql.a.country-category-revenue", "analytics", true, TimeoutClass.Analytics),
        new("sql.a.category-rank-window", "analytics", true, TimeoutClass.Analytics),
        new("sql.a.above-country-average", "analytics", true, TimeoutClass.Analytics),
        new("sql.a.monthly-growth-cte", "analytics", true, TimeoutClass.Analytics)
    };

    public IBenchmarkTarget CreateTarget(string id, TargetOptions options)
    {
        foreach (TargetDescriptor target in Targets) if (target.Id == id) return new SqlTarget(target, options, Supports(id));
        throw new ArgumentException("Unknown SQL target: " + id);
    }

    public IReadOnlyDictionary<string, OperationSupport> Supports(string target)
    {
        var result = new Dictionary<string, OperationSupport>(StringComparer.Ordinal);
        foreach (OperationDescriptor operation in Operations)
        {
            OperationSupport support = new(SupportLevel.Native);
            if (target.StartsWith("cohesion-", StringComparison.Ordinal)) support = operation.Id switch
            {
                "sql.order-detail" or "sql.a.top-products-in-category" or "sql.a.country-category-revenue" => new(SupportLevel.Unsupported,
                    "More than two joined relations; client joins are forbidden.", "Sql.Language/docs/DIALECT.md:55,616-618; COHDBL001: The SQL JOIN surface supports exactly two tables; additional JOIN clauses are not supported."),
                "sql.a.category-rank-window" => new(SupportLevel.Unsupported, "Window ranking and CTEs are not executable.", "Sql.Language/docs/DIALECT.md:61-63; COHDBL001: The OVER clause is not supported by the SQL surface of this database model."),
                "sql.a.above-country-average" => new(SupportLevel.Unsupported, "Correlated subquery; a client join of country aggregates is forbidden.", "Sql.Language/docs/DIALECT.md:971-990; COHDBL001"),
                "sql.a.revenue-by-month" or "sql.a.monthly-growth-cte" => new(SupportLevel.Emulated,
                    "Engine selects the half-open year range; client groups selected rows into at most 48 month/channel groups and computes growth.", "Sql.Language/docs/DIALECT.md:853-860,1572-1581; EXTRACT is not executable; DESIGN section 6 permits selected-row aggregation."),
                _ => support
            };
            result.Add(operation.Id, support);
        }
        return result;
    }

    public IParameterStream CreateParameterStream(DatasetManifest manifest, ulong seed, string operation, int clientIndex, ParameterStreamContext context)
        => new SqlParameterStream(manifest, seed, operation, clientIndex, context);
}
