using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.SecretStore.Client;

namespace Assimalign.Cohesion.SecretStore.Benchmarks;

/// <summary>
/// Public client request construction, bearer headers and response decoding, using a caller-owned
/// in-process message handler. No socket, TLS or server work is included. The handler returns
/// prebuilt response bytes wrapped in a fresh disposable HTTP response. Standalone: a raw HTTP call
/// does not include this protocol work, path validation or result handling.
/// </summary>
public class ClientBenchmarks
{
    private const string SecretPath = "orders/api key/\u03bb";
    private ProtocolHandler _handler = null!;
    private HttpMessageInvoker _transport = null!;
    private ISecretStoreClient _client = null!;
    private SecretStoreCertificateReference _certificate;

    [Params(32, 4096)]
    public int ValueBytes { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        byte[] secret = new byte[ValueBytes];
        for (int i = 0; i < secret.Length; i++) { secret[i] = (byte)(i * 31); }
        string certificate = "-----BEGIN CERTIFICATE-----\n" + Convert.ToBase64String(secret) + "\n-----END CERTIFICATE-----";
        _certificate = "certs/api";
        _handler = new ProtocolHandler(secret, Encoding.UTF8.GetBytes(certificate));
        _transport = new HttpMessageInvoker(_handler);
        var credential = new ClientCredential("benchmark-opaque-token");
        var endpoint = new Uri("https://store.invalid/custom/control");
        _client = SecretStoreClient.Create(endpoint, credential, _transport);

        if (!(await _client.GetSecretAsync(SecretPath)).Span.SequenceEqual(secret)
            || _handler.LastAuthorization != "Bearer benchmark-opaque-token"
            || await _client.GetCertificateAsync(_certificate) != certificate)
        {
            throw new InvalidOperationException("Client read fixtures did not round-trip.");
        }
    }

    [Benchmark] public Task<ReadOnlyMemory<byte>> GetSecret() => _client.GetSecretAsync(SecretPath);
    [Benchmark] public Task<string> GetCertificate() => _client.GetCertificateAsync(_certificate);

    [GlobalCleanup]
    public void Cleanup() => _transport?.Dispose();

    private sealed class ProtocolHandler(byte[] readResponse, byte[] certificateResponse) : HttpMessageHandler
    {
        internal string? LastAuthorization { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastAuthorization = request.Headers.Authorization?.ToString();
            bool certificate = request.RequestUri!.AbsolutePath.EndsWith("/certificates", StringComparison.Ordinal);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(certificate ? certificateResponse : readResponse)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(
                certificate ? "application/x-pem-file" : "application/octet-stream");
            return Task.FromResult(response);
        }
    }
}
