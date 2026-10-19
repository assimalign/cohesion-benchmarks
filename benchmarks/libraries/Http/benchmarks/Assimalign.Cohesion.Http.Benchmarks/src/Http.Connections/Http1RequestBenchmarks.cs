using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

using Assimalign.Cohesion.Connections;
using Assimalign.Cohesion.Connections.InMemory;
using Assimalign.Cohesion.Http.Connections;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Http.Benchmarks.Http.Connections;

/// <summary>
/// One HTTP/1.1 request cycle on a kept-alive connection through the public transport: the client writes a request,
/// the server reads its head (and dispatches it), answers 200 with a two-byte body, and the client reads the
/// response. The connection is an in-memory duplex pipe, so this is the transport's own cost per request.
/// </summary>
/// <remarks>
/// <c>Header</c> varies the size of one request header the way authentication does: none, an HS256 bearer token
/// (a 307-byte <c>Authorization</c> line), a data-protected cookie (484 bytes). Until 2026-10 the head was read one
/// octet at a time, so those lines cost 24 µs and 37 µs per request (results/2026-10-18-auth-triage in the Web
/// area); this benchmark is the guard against that coming back. There is no counterpart: Kestrel's in-memory
/// transport is not public.
/// </remarks>
public class Http1RequestBenchmarks
{
    private InMemoryConnectionListener _driver = null!;
    private HttpConnectionListener _listener = null!;
    private IDuplexConnection _client = null!;
    private IHttpConnection _connection = null!;
    private IHttpConnectionContext _server = null!;
    private IAsyncEnumerator<IHttpContext> _requests = null!;
    private byte[] _request = null!;

    private static readonly byte[] ResponseBody = "OK"u8.ToArray();

    [Params("none", "bearer", "cookie")]
    public string Header { get; set; } = "none";

    [GlobalSetup]
    public async Task Setup()
    {
        string header = Header switch
        {
            // The same lengths as the Web harness's credentials.
            "bearer" => "Authorization: Bearer " + new string('t', 283) + "\r\n",
            "cookie" => "Cookie: .Cohesion.Cookies=" + new string('c', 456) + "\r\n",
            _ => string.Empty,
        };
        _request = Encoding.ASCII.GetBytes("GET /me HTTP/1.1\r\nHost: bench.local\r\n" + header + "\r\n");

        _driver = new InMemoryConnectionListener();
        await _driver.BindAsync();
        _listener = HttpConnectionListener.Create(options => options.UseHttp1(_driver));

        _client = await _driver.CreateFactory().ConnectAsync(_driver.EndPoint);
        _connection = await _listener.AcceptAsync();
        _server = await _connection.OpenAsync();
        _requests = _server.ReceiveAsync().GetAsyncEnumerator();
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _client.DisposeAsync();
        await _requests.DisposeAsync();
        await _connection.DisposeAsync();
        await _listener.DisposeAsync();
        await _driver.DisposeAsync();
    }

    [Benchmark]
    public async Task<long> ServeRequest()
    {
        await _client.Output.WriteAsync(_request);

        if (!await _requests.MoveNextAsync())
        {
            throw new InvalidOperationException("The connection closed.");
        }

        IHttpContext context = _requests.Current;
        context.Response.StatusCode = HttpStatusCode.Ok;
        context.Response.Headers[HttpHeaderKey.ContentLength] = "2";
        await context.Response.Body.WriteAsync(ResponseBody);
        await _server.SendAsync(context);
        await context.DisposeAsync();

        return await ReadResponseAsync(_client.Input);
    }

    // Reads one response: the head, then its two-byte body.
    private static async ValueTask<long> ReadResponseAsync(PipeReader input)
    {
        while (true)
        {
            ReadResult result = await input.ReadAsync();
            ReadOnlySequence<byte> buffer = result.Buffer;
            if (TryFindHeadEnd(buffer, out long headEnd) && buffer.Length >= headEnd + ResponseBody.Length)
            {
                long length = headEnd + ResponseBody.Length;
                input.AdvanceTo(buffer.GetPosition(length));
                return length;
            }

            if (result.IsCompleted)
            {
                throw new InvalidOperationException("The server closed the connection mid-response.");
            }

            input.AdvanceTo(buffer.Start, buffer.End);
        }
    }

    private static bool TryFindHeadEnd(ReadOnlySequence<byte> buffer, out long headEnd)
    {
        SequenceReader<byte> reader = new(buffer);
        if (reader.TryReadTo(out ReadOnlySequence<byte> _, "\r\n\r\n"u8, advancePastDelimiter: true))
        {
            headEnd = reader.Consumed;
            return true;
        }

        headEnd = 0;
        return false;
    }
}
