using System;

using Assimalign.Cohesion.IdentityModel.Protocols;
using Assimalign.Cohesion.IdentityModel.Protocols.Saml;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.IdentityModel.Benchmarks.IdentityModel.Protocols.Saml;

/// <summary>
/// Materializes the protocol assertion with attribute claims and checks the Web Browser SSO data rules on
/// accepted and expired assertions. This is not XML parsing, signature validation or replay prevention;
/// the pinned package exposes those as deferred seams. No signed-token-handler comparison is made.
/// </summary>
public class SamlAssertionBenchmarks
{
    private SamlAssertionDescriptor _descriptor = null!;
    private SamlAssertion _assertion = null!;
    private SamlAssertionValidationOptions _options = null!;

    [Params(false, true)]
    public bool Expired { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var instant = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        _descriptor = new SamlAssertionDescriptor
        {
            Id = "_assertion-42",
            Version = "2.0",
            IssueInstant = instant.AddHours(-1),
            Issuer = new SamlNameId("https://identity.example.com"),
            Subject = new SamlSubject(new SamlNameId("user-42"), new[]
            {
                new SamlSubjectConfirmation(SamlConfirmationMethods.Bearer,
                    data: new SamlSubjectConfirmationData(recipient: "https://orders.example.com/acs",
                        notOnOrAfter: instant.AddMinutes(5), inResponseTo: "_request-42")),
            }),
            Conditions = new SamlConditions(notBefore: instant.AddHours(-1),
                notOnOrAfter: instant.AddMinutes(Expired ? -30 : 5),
                audienceRestrictions: new[] { new[] { "orders-api" } }),
        };
        _descriptor.AuthnStatements.Add(new SamlAuthnStatement(
            new SamlAuthnContext(classReference: "urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport"),
            authnInstant: instant.AddMinutes(-1), sessionIndex: "session-42"));
        _descriptor.AttributeStatements.Add(new SamlAttributeStatement(new[]
        {
            new IdentityAttribute("urn:oid:0.9.2342.19200300.100.1.3", new[] { IdentityClaimValue.FromString("user@example.com") }),
            new IdentityAttribute("groups", new[] { IdentityClaimValue.FromString("staff"), IdentityClaimValue.FromString("admin") }),
        }));
        _options = new SamlAssertionValidationOptions(instant)
        {
            ExpectedIssuer = "https://identity.example.com",
            ExpectedAudience = "orders-api",
            ExpectedRecipient = "https://orders.example.com/acs",
            ExpectedInResponseTo = "_request-42",
        };
        _assertion = Materialize();
        if (Validate().Succeeded == Expired || _assertion.Claims.Count != 4)
        {
            throw new InvalidOperationException("The SAML assertion fixture produced the wrong outcome.");
        }
    }

    [Benchmark]
    public SamlAssertion Materialize() => new(_descriptor);

    [Benchmark]
    public ProtocolValidationResult Validate() => _assertion.Validate(_options);
}
