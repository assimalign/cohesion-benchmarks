using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

using Assimalign.Cohesion.Connections.InMemory;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Connections.Benchmarks.Connections.Tls;

/// <summary>
/// Per-operation ALPN dispatch through the optional TLS feature. Fixed negotiated facts are registered
/// on a real in-memory connection; no certificate or handshake is involved. This covers the Tls
/// contract lookup, not Security's encryption layer. Pipelines exposes no equivalent TLS feature.
/// </summary>
public class TlsFeatureLookupBenchmarks
{
    private IConnection _connection = null!;
    private DuplexConnection _peer = null!;

    [Params(false, true)]
    public bool FeaturePresent { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        (DuplexConnection client, _peer) = InMemoryConnectionPair.Create();
        _connection = client;
        if (FeaturePresent) _connection.Features.Set(new NegotiatedFacts());
        if (SelectHttp2() != (FeaturePresent ? 32 : 0))
            throw new InvalidOperationException("Unexpected ALPN routing decision.");
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _connection.DisposeAsync();
        await _peer.DisposeAsync();
    }

    [Benchmark(OperationsPerInvoke = 32)]
    public int SelectHttp2()
    {
        int selected = 0;
        for (int i = 0; i < 32; i++)
        {
            ITlsConnectionInfo? tls = _connection.Tls;
            if (tls is not null && tls.ApplicationProtocol == SslApplicationProtocol.Http2) selected++;
        }
        return selected;
    }

    private sealed class NegotiatedFacts : ITlsConnectionInfo
    {
        public string Name => ITlsConnectionInfo.FeatureName;
        public SslApplicationProtocol ApplicationProtocol => SslApplicationProtocol.Http2;
        public SslProtocols TlsProtocol => SslProtocols.Tls13;
        public TlsCipherSuite CipherSuite => TlsCipherSuite.TLS_AES_128_GCM_SHA256;
        public X509Certificate2? RemoteCertificate => null;
    }
}
