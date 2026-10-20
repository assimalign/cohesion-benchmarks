using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http;

namespace Assimalign.Cohesion.Web.Benchmarks.Fixtures;

/// <summary>
/// A minimal request context without a server or transport, for in-process handlers, middleware and policies.
/// Adapted from the Web.Authentication.Bearer tests' TestHttpContext.
/// </summary>
internal sealed class BenchmarkHttpContext : HttpContext
{
    private BenchmarkHttpContext(BenchmarkHttpRequest request, BenchmarkHttpResponse response, int featureCapacity)
    {
        Version = HttpVersion.Http11;
        Request = request;
        Response = response;
        ConnectionInfo = HttpConnectionInfo.Empty;
        Features = new HttpFeatureCollection(featureCapacity);
        Items = new Dictionary<string, object?>(StringComparer.Ordinal);
        RequestCancelled = CancellationToken.None;

        request.AttachContext(this);
        response.AttachContext(this);
    }

    public override HttpVersion Version { get; }

    public override BenchmarkHttpRequest Request { get; }

    public override BenchmarkHttpResponse Response { get; }

    public override HttpConnectionInfo ConnectionInfo { get; }

    public override HttpFeatureCollection Features { get; }

    public override IDictionary<string, object?> Items { get; }

    public override CancellationToken RequestCancelled { get; }

    public static BenchmarkHttpContext Create() => Create(featureCapacity: 0);

    public static BenchmarkHttpContext Create(int featureCapacity)
        => new(new BenchmarkHttpRequest(), new BenchmarkHttpResponse(), featureCapacity);

    public void SetAuthorization(string value) => Request.Headers[HttpHeaderKey.Authorization] = value;

    public override void Cancel()
    {
    }

    public override Task CancelAsync() => Task.CompletedTask;

    public override ValueTask DisposeAsync() => ValueTask.CompletedTask;
}

internal sealed class BenchmarkHttpRequest : HttpRequest
{
    private HttpContext? _httpContext;

    public override HttpHost Host { get; set; } = HttpHost.Empty;

    public override HttpPath Path { get; set; } = HttpPath.Root;

    public override HttpMethod Method { get; set; } = HttpMethod.Get;

    public override HttpScheme Scheme { get; set; } = HttpScheme.Http;

    public override HttpQueryCollection Query { get; } = new HttpQueryCollection();

    public override HttpHeaderCollection Headers { get; } = new HttpHeaderCollection();

    public override Stream Body { get; set; } = Stream.Null;

    public override HttpContext HttpContext => _httpContext
        ?? throw new InvalidOperationException("The HttpContext back-reference has not been attached.");

    internal void AttachContext(HttpContext context) => _httpContext ??= context;
}

internal sealed class BenchmarkHttpResponse : HttpResponse
{
    private HttpContext? _httpContext;

    public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Ok;

    public override HttpHeaderCollection Headers { get; } = new HttpHeaderCollection();

    public override Stream Body { get; set; } = new MemoryStream();

    public override HttpContext HttpContext => _httpContext
        ?? throw new InvalidOperationException("The HttpContext back-reference has not been attached.");

    internal void AttachContext(HttpContext context) => _httpContext ??= context;
}
