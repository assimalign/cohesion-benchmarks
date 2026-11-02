using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.ConfigurationStore.Client;

namespace Assimalign.Cohesion.ConfigurationStore.Benchmarks;

/// <summary>
/// Public client request construction, bearer headers and response decoding, using a caller-owned
/// in-process message handler. No socket, TLS or server work is included. The handler returns
/// prebuilt response bytes wrapped in a fresh disposable HTTP response. Standalone: raw JSON
/// deserialization does not include this protocol work, validation or the read-only results.
/// </summary>
public class ClientBenchmarks
{
    private ProtocolHandler _handler = null!;
    private HttpMessageInvoker _transport = null!;
    private IConfigurationStoreClient _client = null!;

    [Params(8, 128)]
    public int EntryCount { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        var values = new Dictionary<string, string?>(StringComparer.Ordinal);
        var names = new string[EntryCount];
        for (int i = 0; i < EntryCount; i++)
        {
            values.Add($"Key{i:D4}", i % 4 == 0 ? null : $"value-{i:D4}-\u03bb");
            names[i] = $"namespace-{i:D4}";
        }
        byte[] readResponse = JsonSerializer.SerializeToUtf8Bytes(values);
        byte[] listResponse = JsonSerializer.SerializeToUtf8Bytes(names);
        _handler = new ProtocolHandler(readResponse, listResponse);
        _transport = new HttpMessageInvoker(_handler);
        var credential = new ClientCredential("benchmark-opaque-token");
        var endpoint = new Uri("https://store.invalid/custom/store");
        _client = ConfigurationStoreClient.Create(endpoint, credential, _transport);

        IReadOnlyDictionary<string, string?> decoded = await _client.GetNamespaceAsync("settings/\u03bb");
        if (decoded.Count != EntryCount || decoded["Key0000"] is not null
            || decoded["Key0001"] != values["Key0001"]
            || _handler.LastAuthorization != "Bearer benchmark-opaque-token"
            || (await _client.ListNamespacesAsync()).Count != EntryCount)
        {
            throw new InvalidOperationException("Client read fixtures did not round-trip.");
        }
    }

    [Benchmark] public Task<IReadOnlyDictionary<string, string?>> GetNamespace() => _client.GetNamespaceAsync("settings/\u03bb");
    [Benchmark] public Task<IReadOnlyList<string>> ListNamespaces() => _client.ListNamespacesAsync();

    [GlobalCleanup]
    public void Cleanup() => _transport?.Dispose();

    private sealed class ProtocolHandler(byte[] readResponse, byte[] listResponse) : HttpMessageHandler
    {
        internal string? LastAuthorization { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastAuthorization = request.Headers.Authorization?.ToString();
            byte[] bytes = string.IsNullOrEmpty(request.RequestUri!.Query) ? listResponse : readResponse;
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Task.FromResult(response);
        }
    }
}
