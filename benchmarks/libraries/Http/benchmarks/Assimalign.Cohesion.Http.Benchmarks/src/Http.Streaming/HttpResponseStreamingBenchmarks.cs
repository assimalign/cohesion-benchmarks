using System;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http.Benchmarks.Fixtures;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Http.Benchmarks.Http.Streaming;

/// <summary>
/// Streaming a 4 KiB response in one or sixteen flushed writes through the public streaming
/// feature over a kept-alive in-memory HTTP/1.1 connection. Includes head commitment, body
/// transport and completion. A declared length keeps client consumption deterministic. No
/// baseline: DefaultHttpContext's body stream does not do HTTP framing or commit a wire head,
/// and Kestrel's equivalent in-memory transport is not public.
/// </summary>
public class HttpResponseStreamingBenchmarks
{
    private InMemoryHttpExchange _exchange = null!;
    private byte[] _request = null!;
    private byte[] _body = null!;

    [Params(1, 16)]
    public int Writes { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        _request = "GET /events HTTP/1.1\r\nHost: bench.local\r\n\r\n"u8.ToArray();
        _body = new byte[4096];
        Array.Fill(_body, (byte)'x');
        _exchange = new InMemoryHttpExchange();
        await _exchange.StartAsync(options => options.Interceptors.Add(new HttpResponseStreamingInterceptor()));
        if (await StreamResponse() <= _body.Length)
            throw new InvalidOperationException("The response did not include a head and the entire body.");
    }

    [GlobalCleanup]
    public ValueTask Cleanup() => _exchange.DisposeAsync();

    [Benchmark]
    public async Task<long> StreamResponse()
    {
        IHttpContext context = await _exchange.ReceiveAsync(_request);
        context.Response.Headers[HttpHeaderKey.ContentLength] = "4096";
        IHttpResponseStreamingFeature streaming = context.Response.Streaming;
        int size = _body.Length / Writes;
        for (int i = 0; i < Writes; i++)
        {
            await streaming.WriteAsync(_body.AsMemory(i * size, size));
            await streaming.FlushAsync();
        }
        await streaming.CompleteAsync();
        return await _exchange.SendAsync(context, _body.Length);
    }
}
