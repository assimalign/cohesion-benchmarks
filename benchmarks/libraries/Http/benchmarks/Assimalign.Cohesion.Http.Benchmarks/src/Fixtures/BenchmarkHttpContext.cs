using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Http.Benchmarks.Fixtures;

// An in-memory request/response carrier for the public feature APIs. It does no parsing itself.
internal sealed class BenchmarkHttpContext : HttpContext
{
    public BenchmarkHttpContext()
    {
        Request = new BenchmarkHttpRequest(this);
        Response = new BenchmarkHttpResponse(this);
    }

    public override HttpVersion Version => HttpVersion.Http11;
    public override BenchmarkHttpRequest Request { get; }
    public override BenchmarkHttpResponse Response { get; }
    public override HttpConnectionInfo ConnectionInfo => HttpConnectionInfo.Empty;
    public override HttpFeatureCollection Features { get; } = new();
    public override IDictionary<string, object?> Items { get; } = new Dictionary<string, object?>();
    public override CancellationToken RequestCancelled => CancellationToken.None;
    public override void Cancel() { }
    public override Task CancelAsync() => Task.CompletedTask;
    public override ValueTask DisposeAsync()
    {
        Request.Body.Dispose();
        Response.Body.Dispose();
        return ValueTask.CompletedTask;
    }
}

internal sealed class BenchmarkHttpRequest(HttpContext context) : HttpRequest
{
    public override HttpHost Host { get; set; } = HttpHost.Empty;
    public override HttpPath Path { get; set; } = HttpPath.Root;
    public override HttpMethod Method { get; set; } = HttpMethod.Post;
    public override HttpScheme Scheme { get; set; } = HttpScheme.Http;
    public override HttpQueryCollection Query { get; } = new();
    public override HttpHeaderCollection Headers { get; } = new();
    public override Stream Body { get; set; } = Stream.Null;
    public override HttpContext HttpContext => context;
}

internal sealed class BenchmarkHttpResponse(HttpContext context) : HttpResponse
{
    public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Ok;
    public override HttpHeaderCollection Headers { get; } = new();
    public override Stream Body { get; set; } = Stream.Null;
    public override HttpContext HttpContext => context;
}
