using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Database.Graph;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

/// <summary>The shared SocialSchema mapping. All temporal values are signed epoch milliseconds.</summary>
public static class GraphMapping
{
    public static object? ReadValue(CsvReader reader, int ordinal, SchemaColumn column)
    {
        if (reader.IsNull(ordinal)) return null;
        var value = reader.GetField(ordinal);
        return column.Type switch
        {
            LogicalType.BigInt or LogicalType.Integer or LogicalType.SmallInt => long.Parse(value, CultureInfo.InvariantCulture),
            LogicalType.Date or LogicalType.Timestamp => new DateTimeOffset(DateTime.Parse(value, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal)).ToUnixTimeMilliseconds(),
            LogicalType.Boolean => bool.Parse(value),
            LogicalType.Numeric => decimal.Parse(value, CultureInfo.InvariantCulture),
            _ => value.ToString()
        };
    }

    public static Dictionary<string, object?> ReadProperties(CsvReader reader, IReadOnlyList<SchemaColumn> columns, int offset = 0)
    {
        var properties = new Dictionary<string, object?>(columns.Count, StringComparer.Ordinal);
        for (int i = 0; i < columns.Count; i++)
        {
            object? value = ReadValue(reader, i + offset, columns[i]);
            // A missing graph property is the portable representation of NULL.
            if (value is not null) properties.Add(columns[i].Name, value);
        }
        return properties;
    }

    public static string[] Neo4jHeader(SchemaNode node)
    {
        var header = new string[node.Properties.Count];
        for (int i = 0; i < header.Length; i++) header[i] = node.Properties[i].Name == "id"
            ? "id:ID(" + node.Label + "){id-type:long}" : TypedHeader(node.Properties[i]);
        return header;
    }

    public static string[] Neo4jHeader(SchemaRelationship relationship)
    {
        var header = new string[relationship.Properties.Count + 2];
        header[0] = ":START_ID(" + relationship.SourceLabel + ")";
        header[1] = ":END_ID(" + relationship.DestinationLabel + ")";
        for (int i = 0; i < relationship.Properties.Count; i++) header[i + 2] = TypedHeader(relationship.Properties[i]);
        return header;
    }

    private static string TypedHeader(SchemaColumn column) => column.Name + ":" + (column.Type switch
    {
        LogicalType.BigInt or LogicalType.Integer or LogicalType.SmallInt or LogicalType.Date or LogicalType.Timestamp => "long",
        LogicalType.Boolean => "boolean", LogicalType.Numeric => "double", _ => "string"
    });

    public static IEnumerable<string> Neo4jIndexes()
    {
        foreach (var node in SocialSchema.Nodes)
        foreach (var index in node.Indexes)
        {
            if (index.Columns.Count != 1) throw new NotSupportedException("Graph indexes must have one property.");
            yield return index.Unique
                ? $"CREATE CONSTRAINT {index.Name} IF NOT EXISTS FOR (n:{node.Label}) REQUIRE n.{index.Columns[0]} IS UNIQUE"
                : $"CREATE INDEX {index.Name} IF NOT EXISTS FOR (n:{node.Label}) ON (n.{index.Columns[0]})";
        }
    }

    public static async ValueTask CreateCohesionIndexesAsync(GraphDatabase database, GraphDatabaseSession session, CancellationToken token)
    {
        var schema = GraphSchema.Open(database, session);
        foreach (var node in SocialSchema.Nodes)
        {
            var existing = await schema.GetIndexesAsync(node.Label, token).ConfigureAwait(false);
            foreach (var index in node.Indexes)
            {
                bool found = false;
                foreach (var item in existing) if (item.Name == index.Name) { found = true; break; }
                if (!found) await schema.CreateIndexAsync(node.Label, index.Name, index.Columns[0], token).ConfigureAwait(false);
            }
        }
    }

    public static string Properties(IReadOnlyDictionary<string, object?> properties)
    {
        var text = new StringBuilder("{");
        foreach (var pair in properties)
        {
            if (text.Length > 1) text.Append(',');
            text.Append('`').Append(pair.Key.Replace("`", "``", StringComparison.Ordinal)).Append("`:").Append(Literal(pair.Value));
        }
        return text.Append('}').ToString();
    }

    public static string Literal(object? value) => value switch
    {
        null => "NULL",
        string s => "'" + s.Replace("'", "''", StringComparison.Ordinal) + "'",
        bool b => b ? "TRUE" : "FALSE",
        IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
        _ => throw new ArgumentException("Unsupported graph literal.", nameof(value))
    };
}
