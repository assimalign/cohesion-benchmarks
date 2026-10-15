using System;
using System.Globalization;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>Compact UTF-8 documents; the caller writes properties in schema order.</summary>
public sealed class JsonLinesWriter : IDisposable
{
    private readonly Stream stream;
    private readonly Utf8JsonWriter writer;
    private readonly bool leaveOpen;
    public long Rows { get; private set; }

    public JsonLinesWriter(Stream stream, bool leaveOpen = false)
    {
        this.stream = stream; this.leaveOpen = leaveOpen;
        writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = false, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
    }

    public Utf8JsonWriter BeginDocument() { writer.Reset(stream); writer.WriteStartObject(); return writer; }
    public void EndDocument() { writer.WriteEndObject(); writer.Flush(); stream.WriteByte((byte)'\n'); Rows++; }

    public static void Timestamp(Utf8JsonWriter writer, string name, DateTime value)
    {
        if (value.Kind != DateTimeKind.Utc) throw new ArgumentException("Timestamps must be UTC.");
        writer.WriteString(name, value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
    }

    public static void Money(Utf8JsonWriter writer, string name, decimal value)
    {
        writer.WritePropertyName(name);
        Span<char> chars = stackalloc char[64];
        value.TryFormat(chars, out int written, "F2", CultureInfo.InvariantCulture);
        writer.WriteRawValue(chars[..written]);
    }

    public void Dispose() { writer.Dispose(); if (!leaveOpen) stream.Dispose(); }
}
