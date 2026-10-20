using System;
using System.Buffers;
using System.Buffers.Text;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using Assimalign.Cohesion.IdentityHub.Hosting;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.IdentityHub.Benchmarks.IdentityHub.Hosting;

/// <summary>
/// Measures IdentityHub's actual token-issuance and JWKS serialization helper. It is internal and
/// the public facade always starts a socket listener, so setup binds closed typed delegates to the
/// packaged helper; measured calls do not use reflection. Key generation, fixtures and signature
/// verification stay in setup. This is the hub's descriptor construction, claims, signing lock and
/// ES256 writer together, rather than a comparison with a lower-level JWT writer doing less work.
/// </summary>
public class IdentitySigningBenchmarks
{
    private static readonly DateTimeOffset Now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private static readonly TimeSpan Lifetime = TimeSpan.FromHours(1);
    private readonly ArrayBufferWriter<byte> _buffer = new();
    private IDisposable _signingKey = null!;
    private IssueToken _issue = null!;
    private Action<Utf8JsonWriter> _writeJwks = null!;

    private delegate string IssueToken(string issuer, string subject, string audience, string clientId,
        string? scope, TimeSpan lifetime, DateTimeOffset now, bool identityToken);

    [GlobalSetup]
    public void Setup()
    {
        Type type = typeof(IdentityHubApplication).Assembly.GetType(
            "Assimalign.Cohesion.IdentityHub.Hosting.Internal.IdentitySigningKey", throwOnError: true)!;
        ECDsa key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        try
        {
            _signingKey = (IDisposable)type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                binder: null, [typeof(ECDsa)], modifiers: null)!.Invoke([key]);
        }
        catch
        {
            key.Dispose();
            throw;
        }

        _issue = type.GetMethod("Issue", BindingFlags.Instance | BindingFlags.NonPublic)!
            .CreateDelegate<IssueToken>(_signingKey);
        _writeJwks = type.GetMethod("WriteJwks", BindingFlags.Instance | BindingFlags.NonPublic)!
            .CreateDelegate<Action<Utf8JsonWriter>>(_signingKey);

        Verify(key, IssueAccessToken(), "at+jwt", "access");
        Verify(key, IssueIdentityToken(), "JWT", "id");
        WriteJwks();
        using JsonDocument jwks = JsonDocument.Parse(_buffer.WrittenMemory);
        if (jwks.RootElement.GetProperty("keys")[0].GetProperty("alg").GetString() != "ES256")
        {
            throw new InvalidOperationException("Expected an ES256 JWKS key.");
        }
    }

    [Benchmark]
    public string IssueAccessToken() => _issue("https://identity.example", "worker", "orders",
        "worker", "orders.read", Lifetime, Now, false);

    [Benchmark]
    public string IssueIdentityToken() => _issue("https://identity.example", "user-1", "worker",
        "worker", "openid", Lifetime, Now, true);

    [Benchmark]
    public int WriteJwks()
    {
        _buffer.Clear();
        using Utf8JsonWriter writer = new(_buffer);
        _writeJwks(writer);
        writer.Flush();
        return _buffer.WrittenCount;
    }

    [GlobalCleanup]
    public void Cleanup() => _signingKey.Dispose();

    private static void Verify(ECDsa key, string token, string tokenType, string tokenUse)
    {
        string[] parts = token.Split('.');
        if (parts.Length != 3 || !key.VerifyData(Encoding.ASCII.GetBytes(parts[0] + "." + parts[1]),
            Base64Url.DecodeFromChars(parts[2]), HashAlgorithmName.SHA256,
            DSASignatureFormat.IeeeP1363FixedFieldConcatenation))
        {
            throw new InvalidOperationException("Token signature verification failed.");
        }

        using JsonDocument header = JsonDocument.Parse(Base64Url.DecodeFromChars(parts[0]));
        using JsonDocument payload = JsonDocument.Parse(Base64Url.DecodeFromChars(parts[1]));
        if (header.RootElement.GetProperty("typ").GetString() != tokenType ||
            payload.RootElement.GetProperty("token_use").GetString() != tokenUse ||
            payload.RootElement.GetProperty("exp").GetInt64() != Now.Add(Lifetime).ToUnixTimeSeconds())
        {
            throw new InvalidOperationException("Token claims did not match the fixed fixture.");
        }
    }
}
