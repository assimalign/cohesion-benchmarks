using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Assimalign.Cohesion.Security.Benchmarks.Security;

/// <summary>
/// Loads the same generated certificate with or without its private key from PKCS#12 bytes, a PKCS#12 file,
/// and PEM files. The BCL loaders are precisely the operations the Cohesion manager wraps. Disposal is included
/// on both sides to bound native handle lifetime. All files are generated in a unique temporary directory;
/// file benchmarks include warm filesystem reads, with no certificate-store or external-file dependency.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class CertificateLoadingBenchmarks
{
    private const string Password = "benchmark-password";
    private ICertificateManager _manager = null!;
    private byte[] _pfx = null!;
    private string _directory = null!;
    private string _pfxPath = null!;
    private string _certificatePath = null!;
    private string? _keyPath;

    [Params(false, true)]
    public bool WithPrivateKey { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _manager = CertificateManagerFactory.Create();
        _directory = Path.Combine(Path.GetTempPath(), "cohesion-security-benchmarks-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_directory);
        _pfxPath = Path.Combine(_directory, "certificate.pfx");
        _certificatePath = Path.Combine(_directory, "certificate.pem");
        using RSA key = RSA.Create(2048);
        var request = new CertificateRequest("CN=benchmark.example.com", key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        using var certificate = request.CreateSelfSigned(
            new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2036, 1, 1, 0, 0, 0, TimeSpan.Zero));
        using var publicCertificate = X509CertificateLoader.LoadCertificate(certificate.Export(X509ContentType.Cert));
        _pfx = (WithPrivateKey ? certificate : publicCertificate).Export(X509ContentType.Pkcs12, Password);
        File.WriteAllBytes(_pfxPath, _pfx);
        File.WriteAllText(_certificatePath, certificate.ExportCertificatePem());
        if (WithPrivateKey)
        {
            _keyPath = Path.Combine(_directory, "private-key.pem");
            File.WriteAllText(_keyPath, key.ExportPkcs8PrivateKeyPem());
        }

        using var cohesion = _manager.LoadPkcs12(_pfx, Password);
        using var bcl = X509CertificateLoader.LoadPkcs12(_pfx, Password);
        if (cohesion.Thumbprint != certificate.Thumbprint || bcl.Thumbprint != certificate.Thumbprint ||
            cohesion.HasPrivateKey != WithPrivateKey || bcl.HasPrivateKey != WithPrivateKey ||
            LoadPem() != WithPrivateKey || LoadPem_SystemSecurityCryptography() != WithPrivateKey)
        {
            throw new InvalidOperationException("Certificate loaders must preserve the certificate and requested key.");
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        File.Delete(_pfxPath);
        File.Delete(_certificatePath);
        if (_keyPath is not null)
        {
            File.Delete(_keyPath);
        }
        Directory.Delete(_directory);
    }

    [Benchmark]
    [BenchmarkCategory("LoadPkcs12")]
    public bool LoadPkcs12()
    {
        using var certificate = _manager.LoadPkcs12(_pfx, Password);
        return certificate.HasPrivateKey;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("LoadPkcs12")]
    public bool LoadPkcs12_SystemSecurityCryptography()
    {
        using var certificate = X509CertificateLoader.LoadPkcs12(_pfx, Password);
        return certificate.HasPrivateKey;
    }

    [Benchmark]
    [BenchmarkCategory("LoadPkcs12File")]
    public bool LoadPkcs12File()
    {
        using var certificate = _manager.LoadPkcs12FromFile(_pfxPath, Password);
        return certificate.HasPrivateKey;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("LoadPkcs12File")]
    public bool LoadPkcs12File_SystemSecurityCryptography()
    {
        using var certificate = X509CertificateLoader.LoadPkcs12FromFile(_pfxPath, Password);
        return certificate.HasPrivateKey;
    }

    [Benchmark]
    [BenchmarkCategory("LoadPem")]
    public bool LoadPem()
    {
        using var certificate = _manager.LoadPemFromFile(_certificatePath, _keyPath);
        return certificate.HasPrivateKey;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("LoadPem")]
    public bool LoadPem_SystemSecurityCryptography()
    {
        using var certificate = _keyPath is null
            ? X509Certificate2.CreateFromPem(File.ReadAllText(_certificatePath))
            : X509Certificate2.CreateFromPemFile(_certificatePath, _keyPath);
        return certificate.HasPrivateKey;
    }
}
