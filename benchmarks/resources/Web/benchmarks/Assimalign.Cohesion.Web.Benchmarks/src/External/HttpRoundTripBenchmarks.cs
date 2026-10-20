using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

using Assimalign.Cohesion.Benchmarks.External;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Web.Benchmarks.External;

/// <summary>
/// One HTTP/1.1 request and response over a kept-alive connection to a server running in another process: the
/// published Cohesion and ASP.NET Core servers, Node.js, Python and nginx (whichever this machine can start).
/// </summary>
/// <remarks>
/// This is unloaded round-trip latency (client, loopback and server for one request), not throughput: concurrent
/// load belongs to the end-to-end runner. The client is a raw socket writing a fixed request, so the client side
/// costs the same for every target.
/// <para>
/// A server may end a kept-alive connection: nginx closes it after 1,000 requests by default and says so with
/// <c>Connection: close</c>, and an idle timeout closes it without a response. The client then reconnects, so an
/// occasional operation includes a loopback connect.
/// </para>
/// </remarks>
public class HttpRoundTripBenchmarks
{
    private static readonly byte[] Request = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: localhost\r\n\r\n");

    private ExternalTargetInstance _target = null!;
    private Socket _socket = null!;
    private readonly byte[] _buffer = new byte[4096];

    public IEnumerable<string> Targets => WebTargets.AvailableIds(ExternalTargetKind.Process)
        .Concat(WebTargets.AvailableIds(ExternalTargetKind.Container));

    [ParamsSource(nameof(Targets))]
    public string Target { get; set; } = string.Empty;

    [GlobalSetup]
    public void Start()
    {
        _target = WebTargets.Catalog.Start(Target);
        _socket = Connect();
    }

    [GlobalCleanup]
    public void Stop()
    {
        _socket.Dispose();
        _target.Dispose();
    }

    [Benchmark]
    public int Get()
    {
        int received = Exchange();
        if (received < 0)
        {
            // The server closed the kept-alive connection before answering (an idle timeout): retry once on a new one.
            Reconnect();
            received = Exchange();
            if (received < 0)
            {
                throw new InvalidOperationException($"'{Target}' closed a new connection without answering.");
            }
        }

        return received;
    }

    // Sends the request and reads the response head and its Content-Length body. Returns -1 when the connection
    // closed before any of the response arrived.
    private int Exchange()
    {
        _socket.Send(Request);

        int received = 0;
        while (true)
        {
            int read = _socket.Receive(_buffer, received, _buffer.Length - received, SocketFlags.None);
            if (read == 0)
            {
                return received == 0
                    ? -1
                    : throw new InvalidOperationException($"'{Target}' closed the connection in the middle of a response.");
            }

            received += read;
            int headEnd = _buffer.AsSpan(0, received).IndexOf("\r\n\r\n"u8);
            if (headEnd >= 0 && received >= headEnd + 4 + ContentLength(_buffer.AsSpan(0, headEnd)))
            {
                if (ClosesConnection(_buffer.AsSpan(0, headEnd)))
                {
                    Reconnect();
                }

                return received;
            }
        }
    }

    private Socket Connect()
    {
        Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
        socket.Connect(_target.IPEndPoint);
        return socket;
    }

    private void Reconnect()
    {
        _socket.Dispose();
        _socket = Connect();
    }

    private static bool ClosesConnection(ReadOnlySpan<byte> head)
    {
        while (!head.IsEmpty)
        {
            int end = head.IndexOf("\r\n"u8);
            ReadOnlySpan<byte> line = end < 0 ? head : head[..end];
            head = end < 0 ? default : head[(end + 2)..];

            if (line.Length > 11 && Ascii.EqualsIgnoreCase(line[..11], "connection:"u8))
            {
                ReadOnlySpan<byte> value = line[11..];
                return Ascii.EqualsIgnoreCase(value[Ascii.Trim(value)], "close"u8);
            }
        }

        return false;
    }

    private static int ContentLength(ReadOnlySpan<byte> head)
    {
        int index = head.IndexOf("Content-Length:"u8);
        if (index < 0)
        {
            index = head.IndexOf("content-length:"u8);
        }

        if (index < 0)
        {
            return 0;
        }

        ReadOnlySpan<byte> value = head[(index + 15)..];
        int end = value.IndexOf("\r\n"u8);
        return int.Parse(Encoding.ASCII.GetString(end < 0 ? value : value[..end]).Trim());
    }
}
