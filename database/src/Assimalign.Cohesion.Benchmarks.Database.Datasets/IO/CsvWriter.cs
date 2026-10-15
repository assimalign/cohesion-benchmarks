using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>RFC 4180 UTF-8 writer with LF records and distinct null and empty-string representations.</summary>
public sealed class CsvWriter : IDisposable
{
    private readonly StreamWriter writer;
    public long Rows { get; private set; }

    public CsvWriter(Stream stream, ReadOnlySpan<string> headers, bool leaveOpen = false)
    {
        writer = new StreamWriter(stream, new UTF8Encoding(false, true), 65536, leaveOpen) { NewLine = "\n" };
        for (int i = 0; i < headers.Length; i++) { if (i != 0) writer.Write(','); Field(headers[i]); }
        writer.Write('\n');
    }

    public void WriteRow(params ReadOnlySpan<object?> values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (i != 0) writer.Write(',');
            switch (values[i])
            {
                case null: break;
                case string text: Field(text); break;
                case decimal number: writer.Write(number.ToString("F2", CultureInfo.InvariantCulture)); break;
                case DateOnly date: writer.Write(date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)); break;
                case DateTime timestamp:
                    if (timestamp.Kind != DateTimeKind.Utc) throw new ArgumentException("Timestamps must be UTC.");
                    writer.Write(timestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)); break;
                case bool flag: writer.Write(flag ? "true" : "false"); break;
                case IFormattable value: writer.Write(value.ToString(null, CultureInfo.InvariantCulture)); break;
                default: throw new ArgumentException("Unsupported CSV field type.");
            }
        }
        writer.Write('\n');
        Rows++;
    }

    private void Field(string value)
    {
        bool quote = value.Length == 0 || value.AsSpan().IndexOfAny(",\"\r\n") >= 0;
        if (!quote) { writer.Write(value); return; }
        writer.Write('"');
        foreach (char ch in value) { if (ch == '"') writer.Write('"'); writer.Write(ch); }
        writer.Write('"');
    }

    public void Flush() => writer.Flush();
    public void Dispose() => writer.Dispose();
}
