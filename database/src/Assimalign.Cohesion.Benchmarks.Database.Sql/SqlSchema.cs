using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

/// <summary>Renders the shared commerce schema without foreign keys or load-time secondary indexes.</summary>
public static class SqlSchema
{
    public static string CreateTable(SchemaTable table, SqlDialect dialect, string? name = null)
    {
        var sql = new StringBuilder("CREATE TABLE ").Append(Identifier(name ?? table.Name)).Append(" (");
        foreach (SchemaColumn column in table.Columns)
        {
            // Persistent write passes need a full deterministic namespace for shipment history.
            // Widen identically on all engines; generated SMALLINT values still load unchanged.
            string type = table.Name == "order_status_history" && column.Name == "seq" ? "BIGINT" : Type(column, dialect);
            sql.Append(column.Name).Append(' ').Append(type);
            if (column.Type is LogicalType.VarChar or LogicalType.Text)
                sql.Append(dialect switch { SqlDialect.PostgreSql => " COLLATE \"C\"", SqlDialect.SqlServer => " COLLATE Latin1_General_100_BIN2_UTF8", _ => " COLLATE binary" });
            sql.Append(column.Nullable ? " NULL, " : " NOT NULL, ");
        }
        return sql.Append("PRIMARY KEY (").AppendJoin(", ", table.PrimaryKey).Append("))").ToString();
    }

    public static IEnumerable<string> CreateIndexes(SchemaTable table)
    {
        foreach (SchemaIndex index in table.Indexes)
            yield return $"CREATE {(index.Unique ? "UNIQUE " : "")}INDEX {index.Name} ON {table.Name} ({string.Join(", ", index.Columns)})";
    }

    public static string CreateDatabase(string name, SqlDialect dialect) => dialect switch
    {
        SqlDialect.PostgreSql => $"CREATE DATABASE {Identifier(name)} TEMPLATE template0 ENCODING 'UTF8' LC_COLLATE 'C' LC_CTYPE 'C'",
        SqlDialect.SqlServer => $"CREATE DATABASE {Identifier(name)} COLLATE Latin1_General_100_BIN2_UTF8",
        _ => throw new NotSupportedException("Cohesion databases are created through the public engine builder.")
    };

    public static string Identifier(string name)
    {
        if (name.Length == 0 || !(char.IsAsciiLetter(name[0]) || name[0] == '_')) throw new ArgumentException("Invalid SQL identifier.");
        foreach (char c in name) if (!(char.IsAsciiLetterOrDigit(c) || c == '_')) throw new ArgumentException("Invalid SQL identifier.");
        return name;
    }

    private static string Type(SchemaColumn column, SqlDialect dialect) => column.Type switch
    {
        LogicalType.BigInt => "BIGINT", LogicalType.Integer => "INTEGER", LogicalType.SmallInt => "SMALLINT",
        LogicalType.Numeric => string.Create(CultureInfo.InvariantCulture, $"NUMERIC({column.Precision},{column.Scale})"),
        LogicalType.VarChar => string.Create(CultureInfo.InvariantCulture, $"VARCHAR({column.Length})"),
        LogicalType.Text => dialect == SqlDialect.SqlServer ? "VARCHAR(MAX)" : "TEXT",
        LogicalType.Boolean => dialect == SqlDialect.SqlServer ? "BIT" : "BOOLEAN",
        LogicalType.Date => "DATE", LogicalType.Timestamp => dialect == SqlDialect.SqlServer ? "DATETIME2(0)" : "TIMESTAMP",
        _ => throw new NotSupportedException(column.Type.ToString())
    };
}
