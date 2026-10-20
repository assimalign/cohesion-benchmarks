using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading.Tasks;

using Assimalign.Cohesion.Connections;
using Assimalign.Cohesion.Connections.InMemory;
using Assimalign.Cohesion.Http.Connections;

namespace Assimalign.Cohesion.Http.Benchmarks.Fixtures;

// Keeps one real HTTP/1.1 transport open across invocations. No sockets or connection setup
// are measured. Responses have a declared length so the client can consume exactly one exchange.
internal sealed class InMemoryHttpExchange : IAsyncDisposable
{
    private InMemoryConnectionListener _driver = null!;
    private HttpConnectionListener _listener = null!;
    private IDuplexConnection _client = null!;
    private IHttpConnection _connection = null!;
    private IHttpConnectionContext _server = null!;
    private IAsyncEnumerator<IHttpContext> _requests = null!;

    public async Task StartAsync(Action<HttpConnectionListenerOptions> configure)
    {
        _driver = new InMemoryConnectionListener();
        await _driver.BindAsync();
        _listener = HttpConnectionListener.Create(options =>
        {
            configure(options);
            options.UseHttp1(_driver);
        });
        _client = await _driver.CreateFactory().ConnectAsync(_driver.EndPoint);
        _connection = await _listener.AcceptAsync();
        _server = await _connection.OpenAsync();
        _requests = _server.ReceiveAsync().GetAsyncEnumerator();
    }

    public async Task<IHttpContext> ReceiveAsync(byte[] request)
    {
        await _client.Output.WriteAsync(request);
        if (!await _requests.MoveNextAsync()) throw new InvalidOperationException("The connection closed.");
        return _requests.Current;
    }

    public async Task<long> SendAsync(IHttpContext context, int bodyLength)
    {
        await _server.SendAsync(context);
        await context.DisposeAsync();
        return await ReadResponseAsync(bodyLength);
    }

    private async Task<long> ReadResponseAsync(int bodyLength)
    {
        while (true)
        {
            ReadResult result = await _client.Input.ReadAsync();
            ReadOnlySequence<byte> buffer = result.Buffer;
            if (TryFindHeadEnd(buffer, out long headEnd) && buffer.Length >= headEnd + bodyLength)
            {
                long length = headEnd + bodyLength;
                _client.Input.AdvanceTo(buffer.GetPosition(length));
                return length;
            }
            if (result.IsCompleted) throw new InvalidOperationException("The response was truncated.");
            _client.Input.AdvanceTo(buffer.Start, buffer.End);
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

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
        await _requests.DisposeAsync();
        await _connection.DisposeAsync();
        await _listener.DisposeAsync();
        await _driver.DisposeAsync();
    }
}
