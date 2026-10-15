using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Length-framed canonical scalar rows. Ordered results preserve the declared total order; unordered rows sort bytewise.</summary>
public sealed class VerificationDigest(bool ordered) : IVerificationSink, IDisposable
{
    private readonly IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
    private readonly List<byte[]> rows = [];
    private long count;
    private bool completed;
    public void AddRow(ReadOnlySpan<object?> values)
    {
        if (completed) throw new InvalidOperationException("Digest already completed.");
        using var buffer = new MemoryStream();
        using (var writer = new BinaryWriter(buffer, Encoding.UTF8, true))
        {
            writer.Write(values.Length);
            foreach (object? value in values)
            {
                // String framing is binary UTF-8 length + bytes; delimiters and NULL spellings cannot collide.
                var (kind, text) = Normalize(value);
                writer.Write(kind);
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                writer.Write(bytes.Length);
                writer.Write(bytes);
            }
        }
        byte[] row = buffer.ToArray();
        if (ordered) Append(row); else rows.Add(row);
        count++;
    }
    public RowsDigest Complete()
    {
        if (completed) throw new InvalidOperationException("Digest already completed.");
        completed = true;
        if (!ordered)
        {
            rows.Sort(static (a, b) => a.AsSpan().SequenceCompareTo(b));
            foreach (byte[] row in rows) Append(row);
        }
        Span<byte> tail = stackalloc byte[8];
        BinaryPrimitives.WriteInt64LittleEndian(tail, count);
        hash.AppendData(tail);
        return new(count, Convert.ToHexStringLower(hash.GetHashAndReset()));
    }
    private void Append(byte[] row)
    {
        Span<byte> length = stackalloc byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(length, row.Length);
        hash.AppendData(length);
        hash.AppendData(row);
    }
    private static (byte Kind, string Text) Normalize(object? value) => value switch
    {
        null or DBNull => (0, ""),
        string s => (1, s),
        char c => (1, c.ToString()),
        bool b => (2, b ? "true" : "false"),
        decimal d => (3, decimal.Round(d, 2, MidpointRounding.ToEven).ToString("G29", CultureInfo.InvariantCulture)),
        double d => (3, Floating(d)),
        float f => (3, Floating(f)),
        byte or sbyte or short or ushort or int or uint or long or ulong => (3, Convert.ToString(value, CultureInfo.InvariantCulture)!),
        DateTimeOffset d => (4, d.UtcDateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'", CultureInfo.InvariantCulture)),
        DateTime d => (4, (d.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(d, DateTimeKind.Utc) : d.ToUniversalTime())
            .ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'", CultureInfo.InvariantCulture)),
        DateOnly d => (5, d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
        TimeOnly t => (6, t.ToString("HH:mm:ss.fffffff", CultureInfo.InvariantCulture)),
        byte[] b => (7, Convert.ToHexStringLower(b)),
        Guid g => (8, g.ToString("D")),
        _ => throw new ArgumentException($"Adapter must project scalar verification values; unsupported type {value.GetType()}."),
    };
    private static string Floating(double value)
        => value == 0 ? "0" : value.ToString("G6", CultureInfo.InvariantCulture);
    public void Dispose() => hash.Dispose();
}
