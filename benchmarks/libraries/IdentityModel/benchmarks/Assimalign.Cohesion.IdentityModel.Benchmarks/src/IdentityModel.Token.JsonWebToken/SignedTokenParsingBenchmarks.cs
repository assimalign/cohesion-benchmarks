using System;
using System.Collections.Generic;
using System.Security.Cryptography;

using BenchmarkDotNet.Attributes;

using CohesionToken = Assimalign.Cohesion.IdentityModel.Token.JsonWebToken.JsonWebToken;
using MicrosoftToken = Microsoft.IdentityModel.JsonWebTokens.JsonWebToken;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Assimalign.Cohesion.IdentityModel.Benchmarks.IdentityModel.Token.JsonWebToken;

/// <summary>
/// Parses identical HS256 and RS256 compact tokens without verifying signatures on either side.
/// These are read paths only: Cohesion does not ship an HMAC verifier or a full RSA validator at this pin.
/// </summary>
public class SignedTokenParsingBenchmarks
{
    private string _token = null!;

    [Params("HS256", "RS256")]
    public string Algorithm { get; set; } = "HS256";

    [GlobalSetup]
    public void Setup()
    {
        using RSA rsa = RSA.Create(2048);
        SecurityKey key = Algorithm == "HS256"
            ? new SymmetricSecurityKey(RandomNumberGenerator.GetBytes(32))
            : new RsaSecurityKey(rsa);
        var instant = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        _token = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = "https://identity.example.com",
            Audience = "orders-api",
            IssuedAt = instant,
            NotBefore = instant,
            Expires = instant.AddHours(1),
            Claims = new Dictionary<string, object>
            {
                ["sub"] = "user-42",
                ["scope"] = "orders.read orders.write",
                ["roles"] = new[] { "reader", "writer", "admin" },
            },
            SigningCredentials = new SigningCredentials(key, Algorithm),
        });
        if (Parse().Subject?.Value != Parse_MicrosoftIdentityModel().Subject)
        {
            throw new InvalidOperationException("Both readers must recover the same subject.");
        }
    }

    [Benchmark]
    public CohesionToken Parse() => CohesionToken.Parse(_token);

    [Benchmark(Baseline = true)]
    public MicrosoftToken Parse_MicrosoftIdentityModel() => new(_token);
}
