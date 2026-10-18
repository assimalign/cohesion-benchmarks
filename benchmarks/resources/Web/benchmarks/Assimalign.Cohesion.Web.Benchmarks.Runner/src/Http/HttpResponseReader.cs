using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Web.Benchmarks.Runner.Http;

/// <summary>
/// Reads HTTP/1.1 responses off a keep-alive socket with as little work as the framing allows: it
/// locates the head, reads the status code and the three framing headers, and skips the body by
/// Content-Length or chunked framing. Nothing is allocated per response unless a capture is supplied.
/// </summary>
internal sealed class HttpResponseReader
{
    private static ReadOnlySpan<byte> CrLf => "\r\n"u8;
    private static ReadOnlySpan<byte> CrLfCrLf => "\r\n\r\n"u8;

    private readonly Socket _socket;
    private byte[] _buffer;
    private int _start;
    private int _end;

    public HttpResponseReader(Socket socket, int bufferSize = 64 * 1024)
    {
        _socket = socket;
        _buffer = new byte[bufferSize];
    }

    /// <summary>Gets the total number of bytes received on the socket.</summary>
    public long BytesReceived { get; private set; }

    /// <summary>Gets whether the last response asked for the connection to close.</summary>
    public bool CloseRequested { get; private set; }

    /// <summary>Reads one complete response and returns its status code.</summary>
    [AsyncMethodBuilder(typeof(PoolingAsyncValueTaskMethodBuilder<>))]
    public async ValueTask<int> ReadAsync(bool headRequest, ResponseCapture? capture, CancellationToken cancellationToken)
    {
        while (true)
        {
            int headLength;
            while ((headLength = FindHeadLength()) < 0)
            {
                await FillAsync(cancellationToken).ConfigureAwait(false);
            }

            Head head = ParseHead(_buffer.AsSpan(_start, headLength));
            capture?.SetHead(_buffer, _start, headLength);
            _start += headLength;
            CloseRequested = head.Close;

            // Interim responses (100 Continue, 103 Early Hints) precede the final one.
            if (head.Status is >= 100 and < 200 && head.Status != 101)
            {
                continue;
            }

            if (headRequest || head.Status is 101 or 204 or 304)
            {
                return head.Status;
            }

            if (head.Chunked)
            {
                await SkipChunkedAsync(capture, cancellationToken).ConfigureAwait(false);
            }
            else if (head.ContentLength > 0)
            {
                await SkipAsync(head.ContentLength, capture, cancellationToken).ConfigureAwait(false);
            }
            else if (head.ContentLength < 0)
            {
                // No framing: the body runs to the end of the connection.
                await SkipToEndAsync(capture, cancellationToken).ConfigureAwait(false);
                CloseRequested = true;
            }

            return head.Status;
        }
    }

    private int FindHeadLength()
    {
        int index = _buffer.AsSpan(_start, _end - _start).IndexOf(CrLfCrLf);
        return index < 0 ? -1 : index + CrLfCrLf.Length;
    }

    [AsyncMethodBuilder(typeof(PoolingAsyncValueTaskMethodBuilder))]
    private async ValueTask FillAsync(CancellationToken cancellationToken)
    {
        if (!await TryFillAsync(cancellationToken).ConfigureAwait(false))
        {
            throw new EndOfStreamException("The server closed the connection mid-response.");
        }
    }

    [AsyncMethodBuilder(typeof(PoolingAsyncValueTaskMethodBuilder<>))]
    private async ValueTask<bool> TryFillAsync(CancellationToken cancellationToken)
    {
        if (_start == _end)
        {
            _start = 0;
            _end = 0;
        }
        else if (_end == _buffer.Length)
        {
            if (_start > 0)
            {
                Buffer.BlockCopy(_buffer, _start, _buffer, 0, _end - _start);
                _end -= _start;
                _start = 0;
            }
            else
            {
                Array.Resize(ref _buffer, _buffer.Length * 2);
            }
        }

        int received = await _socket.ReceiveAsync(_buffer.AsMemory(_end), SocketFlags.None, cancellationToken).ConfigureAwait(false);
        if (received == 0)
        {
            return false;
        }

        _end += received;
        BytesReceived += received;
        return true;
    }

    [AsyncMethodBuilder(typeof(PoolingAsyncValueTaskMethodBuilder))]
    private async ValueTask SkipAsync(long count, ResponseCapture? capture, CancellationToken cancellationToken)
    {
        while (count > 0)
        {
            if (_start == _end)
            {
                await FillAsync(cancellationToken).ConfigureAwait(false);
            }

            int take = (int)Math.Min(count, _end - _start);
            capture?.AppendBody(_buffer, _start, take);
            _start += take;
            count -= take;
        }
    }

    private async ValueTask SkipToEndAsync(ResponseCapture? capture, CancellationToken cancellationToken)
    {
        while (true)
        {
            if (_start < _end)
            {
                capture?.AppendBody(_buffer, _start, _end - _start);
                _start = _end;
            }

            if (!await TryFillAsync(cancellationToken).ConfigureAwait(false))
            {
                return;
            }
        }
    }

    [AsyncMethodBuilder(typeof(PoolingAsyncValueTaskMethodBuilder))]
    private async ValueTask SkipChunkedAsync(ResponseCapture? capture, CancellationToken cancellationToken)
    {
        while (true)
        {
            int lineLength;
            while ((lineLength = FindLineLength()) < 0)
            {
                await FillAsync(cancellationToken).ConfigureAwait(false);
            }

            long size = ParseChunkSize(_buffer.AsSpan(_start, lineLength - CrLf.Length));
            _start += lineLength;

            if (size == 0)
            {
                // Trailer section: header lines until an empty one.
                while (true)
                {
                    while ((lineLength = FindLineLength()) < 0)
                    {
                        await FillAsync(cancellationToken).ConfigureAwait(false);
                    }

                    _start += lineLength;
                    if (lineLength == CrLf.Length)
                    {
                        return;
                    }
                }
            }

            await SkipAsync(size, capture, cancellationToken).ConfigureAwait(false);
            await SkipAsync(CrLf.Length, null, cancellationToken).ConfigureAwait(false);
        }
    }

    private int FindLineLength()
    {
        int index = _buffer.AsSpan(_start, _end - _start).IndexOf(CrLf);
        return index < 0 ? -1 : index + CrLf.Length;
    }

    private static long ParseChunkSize(ReadOnlySpan<byte> line)
    {
        long size = 0;
        foreach (byte b in line)
        {
            int digit = b switch
            {
                >= (byte)'0' and <= (byte)'9' => b - '0',
                >= (byte)'a' and <= (byte)'f' => b - 'a' + 10,
                >= (byte)'A' and <= (byte)'F' => b - 'A' + 10,
                _ => -1,
            };
            if (digit < 0)
            {
                break; // chunk extensions (';') or whitespace end the size
            }
            size = (size << 4) | (uint)digit;
        }
        return size;
    }

    private static Head ParseHead(ReadOnlySpan<byte> head)
    {
        // "HTTP/1.1 200 OK"
        if (head.Length < 12 || !head.StartsWith("HTTP/1."u8))
        {
            throw new InvalidDataException("Malformed status line: " + Encoding.ASCII.GetString(head[..Math.Min(head.Length, 64)]));
        }

        int status = (head[9] - '0') * 100 + (head[10] - '0') * 10 + (head[11] - '0');
        long contentLength = -1;
        bool chunked = false;
        bool close = head[7] == (byte)'0'; // HTTP/1.0 closes unless told otherwise

        int lineStart = head.IndexOf(CrLf) + CrLf.Length;
        while (lineStart < head.Length)
        {
            int lineLength = head[lineStart..].IndexOf(CrLf);
            if (lineLength <= 0)
            {
                break;
            }

            ReadOnlySpan<byte> line = head.Slice(lineStart, lineLength);
            lineStart += lineLength + CrLf.Length;

            int colon = line.IndexOf((byte)':');
            if (colon <= 0)
            {
                continue;
            }

            ReadOnlySpan<byte> name = line[..colon];
            ReadOnlySpan<byte> value = line[(colon + 1)..].Trim((byte)' ');
            if (System.Text.Ascii.EqualsIgnoreCase(name, "content-length"u8))
            {
                contentLength = 0;
                foreach (byte b in value)
                {
                    contentLength = contentLength * 10 + (b - '0');
                }
            }
            else if (System.Text.Ascii.EqualsIgnoreCase(name, "transfer-encoding"u8))
            {
                chunked = ContainsIgnoreCase(value, "chunked"u8);
            }
            else if (System.Text.Ascii.EqualsIgnoreCase(name, "connection"u8))
            {
                close = ContainsIgnoreCase(value, "close"u8);
            }
        }

        if (chunked)
        {
            contentLength = -1;
        }

        return new Head(status, contentLength, chunked, close);
    }

    private static bool ContainsIgnoreCase(ReadOnlySpan<byte> value, ReadOnlySpan<byte> token)
    {
        for (int i = 0; i + token.Length <= value.Length; i++)
        {
            if (System.Text.Ascii.EqualsIgnoreCase(value.Slice(i, token.Length), token))
            {
                return true;
            }
        }
        return false;
    }

    private readonly record struct Head(int Status, long ContentLength, bool Chunked, bool Close);
}

/// <summary>Keeps one response's head and body for validation and setup steps.</summary>
internal sealed class ResponseCapture
{
    private readonly MemoryStream _body = new();

    public string Head { get; private set; } = string.Empty;

    public byte[] Body => _body.ToArray();

    public string BodyText => Encoding.UTF8.GetString(_body.GetBuffer(), 0, (int)_body.Length);

    public long BodyLength => _body.Length;

    public int HeadLength { get; private set; }

    public void SetHead(byte[] buffer, int offset, int count)
    {
        Head = Encoding.Latin1.GetString(buffer, offset, count);
        HeadLength = count;
    }

    public void AppendBody(byte[] buffer, int offset, int count) => _body.Write(buffer, offset, count);

    public int Status => Head.Length >= 12 ? int.Parse(Head.AsSpan(9, 3)) : 0;

    /// <summary>Gets every value of a header, in order.</summary>
    public string[] GetHeaders(string name)
    {
        var values = new System.Collections.Generic.List<string>();
        foreach (string line in Head.Split("\r\n"))
        {
            int colon = line.IndexOf(':');
            if (colon > 0 && line.AsSpan(0, colon).Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                values.Add(line[(colon + 1)..].Trim());
            }
        }
        return [.. values];
    }

    public string? GetHeader(string name)
    {
        string[] values = GetHeaders(name);
        return values.Length == 0 ? null : values[0];
    }
}
