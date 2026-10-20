using System;

using Assimalign.Cohesion.IdentityModel.Token;
using Assimalign.Cohesion.IdentityModel.Token.JsonWebToken;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.IdentityModel.Benchmarks.IdentityModel.Token;

/// <summary>
/// Exercises the neutral token base's issuer, audience and temporal validation through a concrete public JWT.
/// No signature verification or JWT-specific rules are requested here. Third-party full validators provide
/// different guarantees, so this data-rule path is measured alone, including diagnostic allocation on failure.
/// </summary>
public class TokenValidationBenchmarks
{
    private IdentityToken _token = null!;
    private IdentityTokenValidationOptions _options = null!;

    [Params(false, true)]
    public bool Expired { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var instant = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var descriptor = new JsonWebTokenDescriptor
        {
            Id = "token-42",
            Issuer = "https://identity.example.com",
            Subject = new SubjectIdentifier("user-42"),
            NotBefore = instant.AddHours(-1),
            ExpiresAt = instant.AddMinutes(Expired ? -30 : 30),
        };
        descriptor.Audiences.Add("orders-api");
        descriptor.Audiences.Add("billing-api");
        _token = new global::Assimalign.Cohesion.IdentityModel.Token.JsonWebToken.JsonWebToken(descriptor);
        _options = new IdentityTokenValidationOptions(instant)
        {
            ExpectedIssuer = descriptor.Issuer,
            ExpectedAudience = "orders-api",
            ClockSkew = TimeSpan.Zero,
        };
        if (Validate().Succeeded == Expired)
        {
            throw new InvalidOperationException("The expiry fixture produced the wrong validation outcome.");
        }
    }

    [Benchmark]
    public TokenValidationResult Validate() => _token.Validate(_options);
}
