using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Http;
using Sparrow.Json;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

// Use the RavenDB.Client transport and error handling without POCO materialization or response caching.
internal sealed class RavenJsonCommand : RavenCommand<JsonDocument>
{
    private readonly HttpMethod method;
    private readonly string path;
    private readonly byte[]? body;
    private readonly CancellationToken token;
    internal RavenJsonCommand(HttpMethod method, string path, byte[]? body, CancellationToken token)
    {
        this.method = method; this.path = path; this.body = body; this.token = token;
        ResponseType = RavenCommandResponseType.Raw; CanCache = false; CanCacheAggressively = false;
    }
    public override bool IsReadRequest => method == HttpMethod.Get || path.EndsWith("/queries", System.StringComparison.Ordinal);
    public override HttpRequestMessage CreateRequest(JsonOperationContext ctx, ServerNode node, out string url)
    {
        url = node.Url.TrimEnd('/') + path;
        var request = new HttpRequestMessage(method, url);
        if (body != null) { request.Content = new ByteArrayContent(body); request.Content.Headers.ContentType = new("application/json"); }
        return request;
    }
    public override async Task SetResponseRawAsync(HttpResponseMessage response, Stream stream, JsonOperationContext context)
    { Result = await JsonDocument.ParseAsync(stream, cancellationToken: token).ConfigureAwait(false); }
}
