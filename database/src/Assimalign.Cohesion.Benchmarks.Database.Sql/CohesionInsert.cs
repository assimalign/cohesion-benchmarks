using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

internal static class CohesionInsert
{
    // Deliberately far below the 16 MiB wire frame; one serialized row must fit the 8 KiB page.
    internal const int MaximumStatementBytes = 1024 * 1024;
    internal static IEnumerable<(string Text, int Rows)> Statements(string path, SchemaTable table, string name, int batch, long maximum = long.MaxValue)
    {
        using var csv = new CsvReader(File.OpenRead(path));
        string prefix = $"INSERT INTO {SqlSchema.Identifier(name)} ({string.Join(",", table.Header())}) VALUES ";
        var text = new StringBuilder(prefix);
        int count = 0, bytes = Encoding.UTF8.GetByteCount(prefix);
        long read = 0;
        while (read < maximum && csv.Read())
        {
            var row = new StringBuilder("(");
            int storageBytes = 128;
            for (int c = 0; c < table.Columns.Count; c++)
            {
                if (c != 0) row.Append(',');
                if (csv.IsNull(c)) { row.Append("NULL"); continue; }
                ReadOnlySpan<char> field = csv.GetField(c);
                storageBytes += 16 + Encoding.UTF8.GetByteCount(field);
                switch (table.Columns[c].Type)
                {
                    case LogicalType.VarChar: case LogicalType.Text: case LogicalType.Date: case LogicalType.Timestamp:
                        row.Append('\'').Append(field.ToString().Replace("'", "''", StringComparison.Ordinal)).Append('\''); break;
                    case LogicalType.Boolean: row.Append(bool.Parse(field) ? "TRUE" : "FALSE"); break;
                    default: row.Append(field); break;
                }
            }
            if (storageBytes > 8000) throw new InvalidDataException("Commerce row exceeds the Cohesion 8 KiB record budget.");
            row.Append(')');
            int size = Encoding.UTF8.GetByteCount(row.ToString()) + 1;
            if (count > 0 && (count >= batch || bytes + size > MaximumStatementBytes))
            {
                yield return (text.ToString(), count);
                text.Clear().Append(prefix); count = 0; bytes = Encoding.UTF8.GetByteCount(prefix);
            }
            if (count != 0) text.Append(',');
            text.Append(row); count++; bytes += size; read++;
        }
        if (count > 0) yield return (text.ToString(), count);
    }
}
