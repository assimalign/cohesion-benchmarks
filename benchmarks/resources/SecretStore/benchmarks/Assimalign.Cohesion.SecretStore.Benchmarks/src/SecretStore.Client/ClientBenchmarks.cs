using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.SecretStore.Client;

namespace Assimalign.Cohesion.SecretStore.Benchmarks;

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
    private ISecretStoreClient _client = null!;
    private ISecretStoreClient _rejected = null!;
    private ISecretStoreClient _empty = null!;
    private ResourceCommand _command = null!;
    private SecretStoreCertificateReference _certificate;

    [Params(32, 4096)]
    public int ValueBytes { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        byte[] secret = new byte[ValueBytes];
        for (int i = 0; i < secret.Length; i++) { secret[i] = (byte)(i * 31); }
        byte[] readResponse = secret;
        string certificate = "-----BEGIN CERTIFICATE-----\n" + Convert.ToBase64String(secret) + "\n-----END CERTIFICATE-----";
        byte[] certificateResponse = Encoding.UTF8.GetBytes(certificate);
        byte[] payload = JsonSerializer.SerializeToUtf8Bytes(new
        {
            path = "orders/api key/\u03bb",
            source = "parameter:credential",
            resolvedValue = Convert.ToBase64String(secret)
        });
        _certificate = "certs/api";
        _command = new ResourceCommand("benchmark-command", "secretstore.add-secret",
            "benchmarks", "orders/api key/\u03bb", payload);
        _handler = new ProtocolHandler(readResponse, certificateResponse, HttpStatusCode.OK, false);
        _transport = new HttpMessageInvoker(_handler);
        _rejectedTransport = new HttpMessageInvoker(new ProtocolHandler(readResponse, certificateResponse, HttpStatusCode.Conflict, false));
        _emptyTransport = new HttpMessageInvoker(new ProtocolHandler(readResponse, certificateResponse, HttpStatusCode.NoContent, true));
        var credential = new ClientCredential("benchmark-opaque-token");
        var endpoint = new Uri("https://store.invalid/custom/control");
        _client = SecretStoreClient.Create(endpoint, credential, _transport);
        _rejected = SecretStoreClient.CreateForControlPlane(endpoint, credential, _rejectedTransport);
        _empty = SecretStoreClient.CreateForControlPlane(endpoint, credential, _emptyTransport);

        if (!(await _client.GetSecretAsync(_command.Key)).Span.SequenceEqual(secret)
            || await _client.GetCertificateAsync(_certificate) != certificate)
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

    [Benchmark] public Task<ReadOnlyMemory<byte>> GetSecret() => _client.GetSecretAsync(_command.Key);
    [Benchmark] public Task<string> GetCertificate() => _client.GetCertificateAsync(_certificate);
    [Benchmark] public Task SendCommand() => _client.SendCommandAsync(_command);
    [Benchmark] public ValueTask<ResourceCommandObservation> ObserveAppliedCommand() => _client.ObserveCommandAsync(_command);
    [Benchmark] public ValueTask<ResourceCommandObservation> ObserveRejectedCommand() => _rejected.ObserveCommandAsync(_command);
    [Benchmark] public ValueTask<ResourceCommandObservation> DeleteCommand() => _client.DeleteCommandAsync(_command);
    [Benchmark] public ValueTask<ResourceCommandObservation> ObserveEmptyResponse() => _empty.ObserveCommandAsync(_command);

    [GlobalCleanup]
    public void Cleanup()
    {
        _transport?.Dispose();
        _rejectedTransport?.Dispose();
        _emptyTransport?.Dispose();
    }

    private sealed class ProtocolHandler(byte[] readResponse, byte[] certificateResponse, HttpStatusCode commandStatus, bool empty) : HttpMessageHandler
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
            else if (request.RequestUri!.AbsolutePath.EndsWith("/certificates", StringComparison.Ordinal))
            {
                bytes = certificateResponse;
                mediaType = "application/x-pem-file";
            }
            else
            {
                bytes = readResponse;
                mediaType = "application/octet-stream";
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
