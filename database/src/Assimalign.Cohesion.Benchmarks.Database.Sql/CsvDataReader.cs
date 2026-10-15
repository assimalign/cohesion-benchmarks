using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;
using System.IO;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

/// <summary>Streaming typed IDataReader for SqlBulkCopy; preserves CSV NULL versus empty string.</summary>
public sealed class CsvDataReader : DbDataReader
{
    private readonly CsvReader csv;
    private readonly SchemaTable table;
    private bool closed;
    public CsvDataReader(Stream stream, SchemaTable table)
    {
        csv = new(stream); this.table = table;
        if (csv.Headers.Length != table.Columns.Count) throw new InvalidDataException("Wrong CSV column count.");
        for (int i = 0; i < csv.Headers.Length; i++) if (csv.Headers[i] != table.Columns[i].Name) throw new InvalidDataException("Wrong CSV column order.");
    }
    public override object GetValue(int ordinal)
    {
        if (csv.IsNull(ordinal)) return DBNull.Value;
        ReadOnlySpan<char> value = csv.GetField(ordinal);
        return table.Columns[ordinal].Type switch
        {
            LogicalType.BigInt => long.Parse(value, CultureInfo.InvariantCulture),
            LogicalType.Integer => int.Parse(value, CultureInfo.InvariantCulture),
            LogicalType.SmallInt => short.Parse(value, CultureInfo.InvariantCulture),
            LogicalType.Numeric => decimal.Parse(value, CultureInfo.InvariantCulture),
            LogicalType.Boolean => bool.Parse(value),
            LogicalType.Date or LogicalType.Timestamp => DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.None),
            _ => value.ToString()
        };
    }
    public override bool Read() => csv.Read();
    public override bool NextResult() => false;
    public override int FieldCount => table.Columns.Count;
    public override bool HasRows => true;
    public override bool IsClosed => closed;
    public override int RecordsAffected => -1;
    public override int Depth => 0;
    public override object this[int ordinal] => GetValue(ordinal);
    public override object this[string name] => GetValue(GetOrdinal(name));
    public override int GetOrdinal(string name) => csv.GetOrdinal(name);
    public override string GetName(int ordinal) => table.Columns[ordinal].Name;
    public override bool IsDBNull(int ordinal) => csv.IsNull(ordinal);
    public override Type GetFieldType(int ordinal) => table.Columns[ordinal].Type switch
    {
        LogicalType.BigInt => typeof(long), LogicalType.Integer => typeof(int), LogicalType.SmallInt => typeof(short),
        LogicalType.Numeric => typeof(decimal), LogicalType.Boolean => typeof(bool),
        LogicalType.Date or LogicalType.Timestamp => typeof(DateTime), _ => typeof(string)
    };
    public override string GetDataTypeName(int ordinal) => GetFieldType(ordinal).Name;
    public override int GetValues(object[] values) { int n = Math.Min(values.Length, FieldCount); for (int i = 0; i < n; i++) values[i] = GetValue(i); return n; }
    public override bool GetBoolean(int ordinal) => (bool)GetValue(ordinal);
    public override byte GetByte(int ordinal) => (byte)GetValue(ordinal);
    public override char GetChar(int ordinal) => (char)GetValue(ordinal);
    public override DateTime GetDateTime(int ordinal) => (DateTime)GetValue(ordinal);
    public override decimal GetDecimal(int ordinal) => (decimal)GetValue(ordinal);
    public override double GetDouble(int ordinal) => (double)GetValue(ordinal);
    public override float GetFloat(int ordinal) => (float)GetValue(ordinal);
    public override Guid GetGuid(int ordinal) => (Guid)GetValue(ordinal);
    public override short GetInt16(int ordinal) => (short)GetValue(ordinal);
    public override int GetInt32(int ordinal) => (int)GetValue(ordinal);
    public override long GetInt64(int ordinal) => (long)GetValue(ordinal);
    public override string GetString(int ordinal) => (string)GetValue(ordinal);
    public override long GetBytes(int ordinal, long offset, byte[]? buffer, int bufferOffset, int length) => throw new NotSupportedException();
    public override long GetChars(int ordinal, long offset, char[]? buffer, int bufferOffset, int length) => throw new NotSupportedException();
    public override IEnumerator GetEnumerator() => new DbEnumerator(this, false);
    public override void Close() { if (!closed) csv.Dispose(); closed = true; }
    protected override void Dispose(bool disposing) { if (disposing) Close(); base.Dispose(disposing); }
}
