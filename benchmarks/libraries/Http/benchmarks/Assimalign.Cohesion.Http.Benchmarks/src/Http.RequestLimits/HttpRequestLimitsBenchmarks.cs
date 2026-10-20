using System;
using System.Text;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http.Benchmarks.Fixtures;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Http.Benchmarks.Http.RequestLimits;

/// <summary>
/// Per-request cap selection and enforcement while consuming a chunked body over a kept-alive
/// in-memory HTTP/1.1 connection. The 1 KiB body arrives in four chunks; the cap is either exactly
/// 1 KiB or unbounded. This measures the accepted request path, including transport framing and
/// feature attachment, not just a limit property access. Rejection closes this connection and
/// is deliberately excluded, as are time-based rate limits. Kestrel has no equivalent public
/// in-memory transport API, so there is no baseline.
/// </summary>
public class HttpRequestLimitsBenchmarks
{
    private InMemoryHttpExchange _exchange = null!;
    private byte[] _request = null!;
    private byte[] _buffer = null!;
    private byte[] _response = null!;
    private bool _frozen;

    [Params(false, true)]
    public bool Bounded { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        StringBuilder request = new("POST /upload HTTP/1.1\r\nHost: bench.local\r\nTransfer-Encoding: chunked\r\n\r\n");
        for (int i = 0; i < 4; i++) request.Append("100\r\n").Append(new string('x', 256)).Append("\r\n");
        request.Append("0\r\n\r\n");
        _request = Encoding.ASCII.GetBytes(request.ToString());
        _buffer = new byte[256];
        _response = "OK"u8.ToArray();
        _exchange = new InMemoryHttpExchange();
        await _exchange.StartAsync(options => options.Interceptors.Add(new HttpRequestLimitsInterceptor()));
        if (await ReadLimitedBody() != 1024 || !_frozen)
            throw new InvalidOperationException("Request body or limit lifecycle was incorrect.");
    }

    [GlobalCleanup]
    public ValueTask Cleanup() => _exchange.DisposeAsync();

    [Benchmark]
    public async Task<int> ReadLimitedBody()
    {
        IHttpContext context = await _exchange.ReceiveAsync(_request);
        IHttpMaxRequestBodySizeFeature feature = context.MaxRequestBodySize
            ?? throw new InvalidOperationException("Request limits feature missing.");
        feature.MaxRequestBodySize = Bounded ? 1024 : null;
        int total = 0;
        int read;
        while ((read = await context.Request.Body.ReadAsync(_buffer.AsMemory())) != 0) total += read;
        _frozen = feature.IsReadOnly;
        context.Response.Headers[HttpHeaderKey.ContentLength] = "2";
        await context.Response.Body.WriteAsync(_response);
        await _exchange.SendAsync(context, _response.Length);
        return total;
    }
}
