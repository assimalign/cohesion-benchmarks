using System;
using System.Security.Cryptography;
using System.Text;

using Assimalign.Cohesion.IdentityModel.Token.JsonWebToken;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.IdentityModel.Benchmarks.IdentityModel.Token.JsonWebToken;

/// <summary>
/// Verifies the same JOSE signing input and signature through the public verifier and the BCL primitive.
/// This isolates signature verification from parsing and claim validation. RSA covers both PKCS#1 and PSS;
/// ECDSA uses the JOSE P1363 representation. The pinned package has no HMAC signature verifier.
/// </summary>
public class SignatureVerificationBenchmarks
{
    private RSA? _rsa;
    private ECDsa? _ecdsa;
    private IJsonWebTokenSignatureVerifier _verifier = null!;
    private RSASignaturePadding _padding = null!;
    private byte[] _input = null!;
    private byte[] _signature = null!;

    [Params("RS256", "PS256", "ES256")]
    public string Algorithm { get; set; } = "RS256";

    [GlobalSetup]
    public void Setup()
    {
        _input = Encoding.ASCII.GetBytes("eyJ0eXAiOiJKV1QifQ." + new string('a', 512));
        if (Algorithm == "ES256")
        {
            using ECDsa signer = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            _signature = signer.SignData(_input, HashAlgorithmName.SHA256, DSASignatureFormat.IeeeP1363FixedFieldConcatenation);
            _ecdsa = ECDsa.Create(signer.ExportParameters(false));
            _verifier = EcdsaJsonWebTokenSignatureVerifier.Create(_ecdsa);
        }
        else
        {
            using RSA signer = RSA.Create(2048);
            _padding = Algorithm == "PS256" ? RSASignaturePadding.Pss : RSASignaturePadding.Pkcs1;
            _signature = signer.SignData(_input, HashAlgorithmName.SHA256, _padding);
            _rsa = RSA.Create(signer.ExportParameters(false));
            _verifier = RsaJsonWebTokenSignatureVerifier.Create(_rsa);
        }

        if (!Verify() || !Verify_SystemSecurityCryptography())
        {
            throw new InvalidOperationException("The signature fixture must verify on both paths.");
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _rsa?.Dispose();
        _ecdsa?.Dispose();
    }

    [Benchmark]
    public bool Verify() => _verifier.Verify(Algorithm, _input, _signature);

    [Benchmark(Baseline = true)]
    public bool Verify_SystemSecurityCryptography() => Algorithm == "ES256"
        ? _ecdsa!.VerifyData(_input, _signature, HashAlgorithmName.SHA256, DSASignatureFormat.IeeeP1363FixedFieldConcatenation)
        : _rsa!.VerifyData(_input, _signature, HashAlgorithmName.SHA256, _padding);
}
