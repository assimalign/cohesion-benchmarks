using System;
using System.Text;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Http.Benchmarks.Http.DigestFields;

/// <summary>
/// RFC 9530 digest parse/format, request-body verification and response digest generation.
/// Neither ASP.NET Core's typed headers nor System.Net.Http implements these structured digest
/// dictionaries. A bare BCL hash omits field construction and registry handling, so no baseline is used.
/// </summary>
public class HttpDigestFieldBenchmarks
{
    private byte[] _content = null!;
    private string _header = null!;
    private string _wantHeader = null!;
    private HttpDigestField _field;

    [GlobalSetup]
    public void Setup()
    {
        _content = Encoding.UTF8.GetBytes(new string('x', 4096));
        _field = HttpDigestField.ForContent(_content, HttpDigestAlgorithm.Sha256, HttpDigestAlgorithm.Sha512);
        _header = _field.Serialize();
        _wantHeader = "sha-256=5, sha-512=10";
        if (Parse().Entries.Count != 2 || Verify() != HttpDigestVerificationResult.Matched
            || !ParsePreferences().TrySelectPreferred(out HttpDigestAlgorithm selected)
            || selected != HttpDigestAlgorithm.Sha512)
        {
            throw new InvalidOperationException("Digest fixtures failed to round-trip or select SHA-512.");
        }
    }

    [Benchmark]
    public HttpDigestField Parse() => HttpDigestField.Parse(_header);

    [Benchmark]
    public string Format() => _field.Serialize();

    [Benchmark]
    public HttpWantDigestField ParsePreferences() => HttpWantDigestField.Parse(_wantHeader);

    [Benchmark]
    public HttpDigestVerificationResult Verify() => _field.VerifyContent(_content);

    [Benchmark]
    public HttpDigestField Compute() => HttpDigestField.ForContent(_content, HttpDigestAlgorithm.Sha256);
}
