using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Assimalign.Cohesion.IdentityModel.Token;
using Assimalign.Cohesion.IdentityModel.Token.JsonWebToken;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using CohesionJsonWebToken = Assimalign.Cohesion.IdentityModel.Token.JsonWebToken.JsonWebToken;
using MicrosoftEcdsaSecurityKey = Microsoft.IdentityModel.Tokens.ECDsaSecurityKey;
using MicrosoftJsonWebToken = Microsoft.IdentityModel.JsonWebTokens.JsonWebToken;
using MicrosoftJsonWebTokenHandler = Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler;
using MicrosoftSecurityAlgorithms = Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using MicrosoftSecurityTokenDescriptor = Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor;
using MicrosoftSigningCredentials = Microsoft.IdentityModel.Tokens.SigningCredentials;
using MicrosoftTokenValidationParameters = Microsoft.IdentityModel.Tokens.TokenValidationParameters;
using MicrosoftTokenValidationResult = Microsoft.IdentityModel.Tokens.TokenValidationResult;

namespace Assimalign.Cohesion.IdentityModel.Benchmarks.IdentityModel.Token.JsonWebToken;

/// <summary>
/// The three JWT paths a service pays for: parsing a compact token, validating a bearer token on every
/// authenticated request (ES256 signature, issuer, audience and lifetime), and issuing one. The baseline is
/// Microsoft.IdentityModel.JsonWebTokens (<c>JsonWebToken</c>, <c>JsonWebTokenHandler.ValidateTokenAsync</c>,
/// <c>JsonWebTokenHandler.CreateToken</c>), the stack ASP.NET Core's JWT bearer handler runs.
/// </summary>
/// <remarks>
/// Parse and validate read the same token (written by Cohesion, accepted by both libraries), so they compare the
/// same bytes. Each library issues its own token with the same header, registered claims and two custom claims.
/// Validation uses a fixed instant on both sides. Microsoft's lifetime callback also applies Cohesion's
/// required-claim and maximum-lifetime rules; Cohesion explicitly checks audience membership after validation.
/// Both return parsed-token results; the Microsoft result's lazy ClaimsIdentity is not forced.
/// </remarks>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class JsonWebTokenBenchmarks
{
    private const string parseCategory = "Parse";
    private const string validateCategory = "Validate";
    private const string writeCategory = "Write";

    private const string issuer = "https://identity.example.com";
    private const string audience = "orders-api";
    private const string subject = "user-42";
    private const string tokenId = "4f1c9a7e2b3d4c5e";
    private const string keyId = "signing-key-1";

    private ECDsa _signingKey = null!;
    private ECDsa _verificationKey = null!;

    private JsonWebTokenWriter _writer = null!;
    private JsonWebTokenValidator _validator = null!;
    private JsonWebTokenDescriptor _descriptor = null!;

    private MicrosoftJsonWebTokenHandler _handler = null!;
    private MicrosoftSecurityTokenDescriptor _microsoftDescriptor = null!;
    private MicrosoftTokenValidationParameters _validationParameters = null!;

    private string _token = null!;
    private DateTimeOffset _now;

    [GlobalSetup]
    public async Task Setup()
    {
        _signingKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        ECParameters publicParameters = _signingKey.ExportParameters(includePrivateParameters: false);
        _verificationKey = ECDsa.Create(publicParameters);

        DateTimeOffset now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        _now = now;

        // Cohesion: the issuer's public key arrives as a JWK, as it would from a JWKS document.
        string jwk =
            "{\"kty\":\"EC\",\"crv\":\"P-256\"," +
            "\"x\":\"" + Base64Url.EncodeToString(publicParameters.Q.X) + "\"," +
            "\"y\":\"" + Base64Url.EncodeToString(publicParameters.Q.Y) + "\"," +
            "\"kid\":\"" + keyId + "\",\"alg\":\"ES256\",\"use\":\"sig\"}";
        if (!JsonWebKey.TryParse(Encoding.UTF8.GetBytes(jwk), out JsonWebKey? publicKey))
        {
            throw new InvalidOperationException("The verification JWK did not parse.");
        }

        _writer = JsonWebTokenWriter.CreateEs256(_signingKey, keyId);
        _validator = JsonWebTokenValidator.CreateEs256(
            new JsonWebTokenValidationProfile(issuer, new JsonWebKeySet(publicKey), TimeSpan.FromHours(24)));
        _descriptor = new JsonWebTokenDescriptor
        {
            Id = tokenId,
            Issuer = issuer,
            Subject = new SubjectIdentifier(subject),
            TokenType = "JWT",
            IssuedAt = now,
            NotBefore = now,
            ExpiresAt = now.AddHours(1),
        };
        _descriptor.Audiences.Add(audience);
        _descriptor.Claims.Add(new IdentityClaim("scope", "orders.read orders.write"));
        _descriptor.Claims.Add(new IdentityClaim("tenant", "contoso"));

        // Microsoft.IdentityModel: the same key, claims and lifetime.
        _handler = new MicrosoftJsonWebTokenHandler();
        _microsoftDescriptor = new MicrosoftSecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            TokenType = "JWT",
            IssuedAt = now.UtcDateTime,
            NotBefore = now.UtcDateTime,
            Expires = now.AddHours(1).UtcDateTime,
            Claims = new Dictionary<string, object>
            {
                ["sub"] = subject,
                ["jti"] = tokenId,
                ["scope"] = "orders.read orders.write",
                ["tenant"] = "contoso",
            },
            SigningCredentials = new MicrosoftSigningCredentials(
                new MicrosoftEcdsaSecurityKey(_signingKey) { KeyId = keyId },
                MicrosoftSecurityAlgorithms.EcdsaSha256),
        };
        _validationParameters = new MicrosoftTokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidAudience = audience,
            ValidAlgorithms = [MicrosoftSecurityAlgorithms.EcdsaSha256],
            IssuerSigningKey = new MicrosoftEcdsaSecurityKey(_verificationKey) { KeyId = keyId },
            LifetimeValidator = (notBefore, expires, securityToken, parameters) =>
                securityToken is MicrosoftJsonWebToken jwt &&
                notBefore.HasValue && expires.HasValue &&
                jwt.TryGetPayloadValue<long>("iat", out long issuedAt) &&
                jwt.TryGetPayloadValue<string>("sub", out string sub) && !string.IsNullOrEmpty(sub) &&
                jwt.TryGetPayloadValue<string>("jti", out string id) && !string.IsNullOrWhiteSpace(id) &&
                jwt.TryGetPayloadValue<object>("aud", out _) &&
                notBefore.Value <= now.UtcDateTime + parameters.ClockSkew &&
                expires.Value > now.UtcDateTime - parameters.ClockSkew &&
                issuedAt <= now.ToUnixTimeSeconds() + (long)parameters.ClockSkew.TotalSeconds &&
                expires.Value > DateTimeOffset.FromUnixTimeSeconds(issuedAt).UtcDateTime &&
                expires.Value > notBefore.Value &&
                expires.Value - DateTimeOffset.FromUnixTimeSeconds(issuedAt).UtcDateTime <= TimeSpan.FromHours(24),
        };

        _token = _writer.Write(_descriptor);

        if (Validate() is null)
        {
            throw new InvalidOperationException("Cohesion rejected the benchmark token.");
        }

        MicrosoftTokenValidationResult result = await Validate_MicrosoftIdentityModel();
        if (!result.IsValid)
        {
            throw new InvalidOperationException("Microsoft.IdentityModel rejected the benchmark token.", result.Exception);
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _signingKey.Dispose();
        _verificationKey.Dispose();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(parseCategory)]
    public MicrosoftJsonWebToken Parse_MicrosoftIdentityModel() => new(_token);

    [Benchmark]
    [BenchmarkCategory(parseCategory)]
    public CohesionJsonWebToken Parse() => CohesionJsonWebToken.Parse(_token);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(validateCategory)]
    public Task<MicrosoftTokenValidationResult> Validate_MicrosoftIdentityModel()
        => _handler.ValidateTokenAsync(_token, _validationParameters);

    [Benchmark]
    [BenchmarkCategory(validateCategory)]
    public CohesionJsonWebToken? Validate()
    {
        return _validator.TryValidate(_token, _now, out CohesionJsonWebToken? token) && token.HasAudience(audience)
            ? token
            : null;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(writeCategory)]
    public string Write_MicrosoftIdentityModel() => _handler.CreateToken(_microsoftDescriptor);

    [Benchmark]
    [BenchmarkCategory(writeCategory)]
    public string Write() => _writer.Write(_descriptor);
}
