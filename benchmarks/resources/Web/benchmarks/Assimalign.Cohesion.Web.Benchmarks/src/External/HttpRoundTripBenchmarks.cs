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
/// </remarks>
public class HttpRoundTripBenchmarks
{
    private static readonly byte[] Request = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: localhost\r\n\r\n");

    private ExternalTargetInstance _target = null!;
    private Socket _socket = null!;
    private readonly byte[] _buffer = new byte[4096];

    public IEnumerable<string> Targets => WebTargets.Catalog.AvailableIds(ExternalTargetKind.Process)
        .Concat(WebTargets.Catalog.AvailableIds(ExternalTargetKind.Container));

    [ParamsSource(nameof(Targets))]
    public string Target { get; set; } = string.Empty;

    [GlobalSetup]
    public void Start()
    {
        _target = WebTargets.Catalog.Start(Target);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
        _socket.Connect(_target.IPEndPoint);
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
        _socket.Send(Request);

        // Read until the response head and its Content-Length body have arrived.
        int received = 0;
        while (true)
        {
            int read = _socket.Receive(_buffer, received, _buffer.Length - received, SocketFlags.None);
            if (read == 0)
            {
                throw new InvalidOperationException($"'{Target}' closed the connection.");
            }

            received += read;
            int headEnd = _buffer.AsSpan(0, received).IndexOf("\r\n\r\n"u8);
            if (headEnd >= 0 && received >= headEnd + 4 + ContentLength(_buffer.AsSpan(0, headEnd)))
            {
                return received;
            }
        }
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
