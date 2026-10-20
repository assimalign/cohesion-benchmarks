using System;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using CohesionKey = Assimalign.Cohesion.IdentityModel.Token.JsonWebToken.JsonWebKey;
using MicrosoftKey = Microsoft.IdentityModel.Tokens.JsonWebKey;

namespace Assimalign.Cohesion.IdentityModel.Benchmarks.IdentityModel.Token.JsonWebToken;

/// <summary>
/// Reads the same public EC JWK and computes its RFC 7638 SHA-256 thumbprint. Parsing is structural on both
/// paths; neither imports a key nor validates its signing policy. Thumbprints return base64url on both sides.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class JsonWebKeyBenchmarks
{
    private string _json = null!;
    private byte[] _utf8 = null!;
    private CohesionKey _cohesion = null!;
    private MicrosoftKey _microsoft = null!;

    [GlobalSetup]
    public void Setup()
    {
        using ECDsa key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        ECParameters parameters = key.ExportParameters(false);
        _json = "{\"kty\":\"EC\",\"crv\":\"P-256\",\"x\":\"" + Base64Url.EncodeToString(parameters.Q.X) +
            "\",\"y\":\"" + Base64Url.EncodeToString(parameters.Q.Y) + "\",\"kid\":\"key-1\",\"alg\":\"ES256\",\"use\":\"sig\"}";
        _utf8 = Encoding.UTF8.GetBytes(_json);
        _cohesion = Parse();
        _microsoft = Parse_MicrosoftIdentityModel();
        if (Thumbprint() != Thumbprint_MicrosoftIdentityModel())
        {
            throw new InvalidOperationException("RFC 7638 thumbprints must agree.");
        }
    }

    [Benchmark]
    [BenchmarkCategory("Parse")]
    public CohesionKey Parse()
    {
        return CohesionKey.TryParse(_utf8, out var key) ? key : throw new InvalidOperationException("Invalid JWK.");
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Parse")]
    public MicrosoftKey Parse_MicrosoftIdentityModel() => new(_json);

    [Benchmark]
    [BenchmarkCategory("Thumbprint")]
    public string Thumbprint() => _cohesion.ComputeThumbprint();

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Thumbprint")]
    public string Thumbprint_MicrosoftIdentityModel() => Base64Url.EncodeToString(_microsoft.ComputeJwkThumbprint());
}
