using System;
using System.Buffers;
using System.IO;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>Streaming JSONL bytes, with pooled storage valid until the next Read. Does not deserialize documents.</summary>
public sealed class JsonLinesReader : IDisposable
{
    private readonly Stream stream;
    private readonly bool leaveOpen;
    private readonly byte[] input = ArrayPool<byte>.Shared.Rent(65536);
    private byte[] line = ArrayPool<byte>.Shared.Rent(4096);
    private int position, available, length;
    private bool disposed;
    public ReadOnlyMemory<byte> Current => line.AsMemory(0, length);

    public JsonLinesReader(Stream stream, bool leaveOpen = false) { this.stream = stream; this.leaveOpen = leaveOpen; }

    public bool Read()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        length = 0;
        while (true)
        {
            if (position == available)
            {
                available = stream.Read(input); position = 0;
                if (available == 0) return length != 0;
            }
            ReadOnlySpan<byte> rest = input.AsSpan(position, available - position);
            int newline = rest.IndexOf((byte)'\n');
            int take = newline < 0 ? rest.Length : newline;
            if (length + take > line.Length)
            {
                byte[] larger = ArrayPool<byte>.Shared.Rent(Math.Max(line.Length * 2, length + take));
                line.AsSpan(0, length).CopyTo(larger);
                ArrayPool<byte>.Shared.Return(line); line = larger;
            }
            rest[..take].CopyTo(line.AsSpan(length)); length += take; position += take;
            if (newline < 0) continue;
            position++;
            if (length > 0 && line[length - 1] == '\r') length--;
            if (length == 0) throw new InvalidDataException("Empty JSON Lines record.");
            return true;
        }
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        ArrayPool<byte>.Shared.Return(input); ArrayPool<byte>.Shared.Return(line);
        if (!leaveOpen) stream.Dispose();
    }
}
