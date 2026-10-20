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
/// Public client request construction, bearer headers, command serialization and response decoding,
/// using a caller-owned in-process message handler. No socket, TLS or server work is included.
/// The handler consumes already-encoded request bodies and returns prebuilt response bytes wrapped
/// in a fresh disposable HTTP response. Standalone: raw JSON serialization does not include this
/// protocol work, validation, immutable results or command observation semantics.
/// </summary>
public class ClientBenchmarks
{
    private ProtocolHandler _handler = null!;
    private HttpMessageInvoker _transport = null!;
    private HttpMessageInvoker _rejectedTransport = null!;
    private HttpMessageInvoker _emptyTransport = null!;
    private IConfigurationStoreClient _client = null!;
    private IConfigurationStoreClient _rejected = null!;
    private IConfigurationStoreClient _empty = null!;
    private ResourceCommand _command = null!;

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
        byte[] payload = JsonSerializer.SerializeToUtf8Bytes(new { name = "settings", seed = values });
        _command = new ResourceCommand("benchmark-command", "configurationstore.add-namespace",
            "benchmarks", "settings", payload);
        _handler = new ProtocolHandler(readResponse, listResponse, HttpStatusCode.OK, false);
        _transport = new HttpMessageInvoker(_handler);
        _rejectedTransport = new HttpMessageInvoker(new ProtocolHandler(readResponse, listResponse, HttpStatusCode.Conflict, false));
        _emptyTransport = new HttpMessageInvoker(new ProtocolHandler(readResponse, listResponse, HttpStatusCode.NoContent, true));
        var credential = new ClientCredential("benchmark-opaque-token");
        var endpoint = new Uri("https://store.invalid/custom/control");
        _client = ConfigurationStoreClient.Create(endpoint, credential, _transport);
        _rejected = ConfigurationStoreClient.CreateForControlPlane(endpoint, credential, _rejectedTransport);
        _empty = ConfigurationStoreClient.CreateForControlPlane(endpoint, credential, _emptyTransport);

        IReadOnlyDictionary<string, string?> decoded = await _client.GetNamespaceAsync("settings/\u03bb");
        if (decoded.Count != EntryCount || decoded["Key0000"] is not null
            || decoded["Key0001"] != values["Key0001"] || (await _client.ListNamespacesAsync()).Count != EntryCount)
        {
            throw new InvalidOperationException("Client read fixtures did not round-trip.");
        }
        await _client.SendCommandAsync(_command);
        using (JsonDocument document = JsonDocument.Parse(_handler.LastBody))
        {
            if (!document.RootElement.GetProperty("payload").GetBytesFromBase64().AsSpan().SequenceEqual(payload)
                || _handler.LastAuthorization != "Bearer benchmark-opaque-token"
                || _handler.LastUri?.AbsolutePath != "/custom/control/cohesion/v1/commands")
            {
                throw new InvalidOperationException("Client command encoding changed.");
            }
        }
        if ((await _client.ObserveCommandAsync(_command)).Status != "Applied"
            || (await _rejected.ObserveCommandAsync(_command)).Status != "Rejected"
            || (await _client.DeleteCommandAsync(_command)).Status != "Deleted"
            || (await _empty.ObserveCommandAsync(_command)).Status != "Applied")
        {
            throw new InvalidOperationException("Command observations changed.");
        }
    }

    [Benchmark] public Task<IReadOnlyDictionary<string, string?>> GetNamespace() => _client.GetNamespaceAsync("settings/\u03bb");
    [Benchmark] public Task<IReadOnlyList<string>> ListNamespaces() => _client.ListNamespacesAsync();
    [Benchmark] public Task SendCommand() => _client.SendCommandAsync(_command);
    [Benchmark] public ValueTask<ResourceCommandObservation> ObserveAppliedCommand() => _client.ObserveCommandAsync(_command);
    [Benchmark] public ValueTask<ResourceCommandObservation> ObserveRejectedCommand() => _rejected.ObserveCommandAsync(_command);
    [Benchmark] public ValueTask<ResourceCommandObservation> DeleteCommand() => _client.DeleteCommandAsync(_command);

    [GlobalCleanup]
    public void Cleanup()
    {
        _transport?.Dispose();
        _rejectedTransport?.Dispose();
        _emptyTransport?.Dispose();
    }

    private sealed class ProtocolHandler(byte[] readResponse, byte[] listResponse, HttpStatusCode commandStatus, bool empty) : HttpMessageHandler
    {
        private static readonly byte[] Applied = """{"status":"Applied","detail":"declaration persisted"}"""u8.ToArray();
        private static readonly byte[] Deleted = """{"status":"Deleted","detail":"declaration removed"}"""u8.ToArray();
        private static readonly byte[] Rejected = """{"status":"Rejected","detail":"the declaration belongs to another owner"}"""u8.ToArray();
        internal byte[] LastBody { get; private set; } = Array.Empty<byte>();
        internal string? LastAuthorization { get; private set; }
        internal Uri? LastUri { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastUri = request.RequestUri;
            LastAuthorization = request.Headers.Authorization?.ToString();
            bool command = request.Method == HttpMethod.Post || request.Method == HttpMethod.Delete;
            if (request.Content is not null)
            {
                LastBody = await request.Content.ReadAsByteArrayAsync(cancellationToken);
            }
            byte[] bytes;
            string mediaType;
            if (command)
            {
                bytes = empty ? Array.Empty<byte>() : commandStatus == HttpStatusCode.Conflict ? Rejected
                    : request.Method == HttpMethod.Delete ? Deleted : Applied;
                mediaType = "application/json";
            }
            else
            {
                bytes = string.IsNullOrEmpty(request.RequestUri!.Query) ? listResponse : readResponse;
                mediaType = "application/json";
            }
            var response = new HttpResponseMessage(command ? commandStatus : HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return response;
        }
    }
}
