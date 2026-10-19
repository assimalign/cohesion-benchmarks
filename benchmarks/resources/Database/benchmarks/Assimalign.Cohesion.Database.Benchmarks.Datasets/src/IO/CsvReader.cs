using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>Streaming, header-aware CSV decoder. Field spans and null flags remain valid until the next Read.</summary>
public sealed class CsvReader : IDisposable
{
    private readonly StreamReader reader;
    private char[] buffer = new char[4096];
    private int[] starts = new int[32];
    private int[] lengths = new int[32];
    private bool[] nulls = new bool[32];
    private int used;
    private readonly Dictionary<string, int> ordinals = new(StringComparer.Ordinal);
    public string[] Headers { get; }
    public int FieldCount { get; private set; }

    public CsvReader(Stream stream, bool leaveOpen = false)
    {
        reader = new StreamReader(stream, new UTF8Encoding(false, true), false, 65536, leaveOpen);
        if (!ReadRecord()) throw new InvalidDataException("CSV has no header.");
        Headers = new string[FieldCount];
        for (int i = 0; i < FieldCount; i++) { Headers[i] = GetField(i).ToString(); ordinals.Add(Headers[i], i); }
    }

    public int GetOrdinal(string name) => ordinals[name];
    public bool IsNull(int ordinal) => nulls[Check(ordinal)];
    public bool IsNull(string name) => IsNull(GetOrdinal(name));
    public ReadOnlySpan<char> GetField(int ordinal) { Check(ordinal); return buffer.AsSpan(starts[ordinal], lengths[ordinal]); }
    public ReadOnlySpan<char> GetField(string name) => GetField(GetOrdinal(name));
    public string? GetString(string name) => IsNull(name) ? null : GetField(name).ToString();
    private int Check(int ordinal) => (uint)ordinal < (uint)FieldCount ? ordinal : throw new ArgumentOutOfRangeException(nameof(ordinal));

    public bool Read()
    {
        if (!ReadRecord()) return false;
        if (FieldCount != Headers.Length) throw new InvalidDataException("CSV record has a different field count from its header.");
        return true;
    }

    private void Append(char value)
    {
        if (used == buffer.Length) Array.Resize(ref buffer, checked(buffer.Length * 2));
        buffer[used++] = value;
    }

    private bool ReadRecord()
    {
        used = 0; FieldCount = 0;
        if (reader.Peek() < 0) return false;
        while (true)
        {
            if (FieldCount == starts.Length)
            {
                Array.Resize(ref starts, starts.Length * 2);
                Array.Resize(ref lengths, lengths.Length * 2);
                Array.Resize(ref nulls, nulls.Length * 2);
            }
            int start = used;
            bool quoted = reader.Peek() == '"';
            if (quoted)
            {
                reader.Read();
                while (true)
                {
                    int ch = reader.Read();
                    if (ch < 0) throw new InvalidDataException("Unterminated quoted CSV field.");
                    if (ch != '"') { Append((char)ch); continue; }
                    if (reader.Peek() == '"') { reader.Read(); Append('"'); continue; }
                    break;
                }
            }
            else
            {
                while (reader.Peek() is int ch && ch >= 0 && ch != ',' && ch != '\r' && ch != '\n')
                {
                    if (ch == '"') throw new InvalidDataException("Quote inside unquoted CSV field.");
                    Append((char)reader.Read());
                }
            }
            starts[FieldCount] = start; lengths[FieldCount] = used - start; nulls[FieldCount] = !quoted && used == start;
            FieldCount++;
            int separator = reader.Read();
            if (separator == ',') continue;
            if (separator == '\r') { if (reader.Peek() == '\n') reader.Read(); return true; }
            if (separator is '\n' or -1) return true;
            throw new InvalidDataException("Unexpected character after a quoted CSV field.");
        }
    }

    public void Dispose() => reader.Dispose();
}
