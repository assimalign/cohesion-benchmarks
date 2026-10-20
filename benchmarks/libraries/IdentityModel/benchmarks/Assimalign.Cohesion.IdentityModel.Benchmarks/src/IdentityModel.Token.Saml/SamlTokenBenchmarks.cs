using System;

using Assimalign.Cohesion.IdentityModel.Token;
using Assimalign.Cohesion.IdentityModel.Token.Saml;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.IdentityModel.Benchmarks.IdentityModel.Token.Saml;

/// <summary>
/// Normalizes a typed assertion into a token (including one claim per attribute value), and validates issuer,
/// AND-across audience restrictions, lifetime and bearer recipient/correlation. The pinned package has no XML
/// reader or signature validator; a third-party signed XML token handler would do more work and is not compared.
/// </summary>
public class SamlTokenBenchmarks
{
    private SamlTokenDescriptor _descriptor = null!;
    private SamlToken _token = null!;
    private SamlTokenValidationOptions _options = null!;

    [Params(8, 32)]
    public int AttributeCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var instant = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        _descriptor = new SamlTokenDescriptor
        {
            AssertionId = "_assertion-42",
            Version = "2.0",
            Issuer = "https://identity.example.com",
            IssuedAt = instant.AddMinutes(-1),
            NameId = new SamlNameId("user-42", spNameQualifier: "orders-api"),
            Conditions = new SamlConditions(
                notBefore: instant.AddMinutes(-1),
                notOnOrAfter: instant.AddMinutes(5),
                audienceRestrictions: new[] { new[] { "orders-api", "billing-api" }, new[] { "orders-api" } }),
        };
        _descriptor.SubjectConfirmations.Add(new SamlSubjectConfirmation(
            SamlConfirmationMethods.Bearer,
            data: new SamlSubjectConfirmationData(
                recipient: "https://orders.example.com/acs", notOnOrAfter: instant.AddMinutes(5), inResponseTo: "_request-42")));
        for (int index = 0; index < AttributeCount; index++)
        {
            _descriptor.Attributes.Add(new IdentityAttribute("attribute-" + index,
                new[] { IdentityClaimValue.FromString("value-" + index), IdentityClaimValue.FromString("other-" + index) }));
        }
        _options = new SamlTokenValidationOptions(instant)
        {
            ExpectedIssuer = _descriptor.Issuer,
            ExpectedAudience = "orders-api",
            ExpectedRecipient = "https://orders.example.com/acs",
            ExpectedInResponseTo = "_request-42",
            RequireBearerConfirmation = true,
        };
        _token = Materialize();
        if (!Validate().Succeeded || _token.Claims.Count != 1 + 2 * AttributeCount)
        {
            throw new InvalidOperationException("The SAML token must normalize and validate successfully.");
        }
    }

    [Benchmark]
    public SamlToken Materialize() => new(_descriptor);

    [Benchmark]
    public TokenValidationResult Validate() => _token.Validate(_options);
}
