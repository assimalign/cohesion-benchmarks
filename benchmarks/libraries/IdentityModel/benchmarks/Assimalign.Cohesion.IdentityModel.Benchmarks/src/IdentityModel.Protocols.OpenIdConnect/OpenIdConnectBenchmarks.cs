using System;

using Assimalign.Cohesion.IdentityModel.Protocols;
using Assimalign.Cohesion.IdentityModel.Protocols.OpenIdConnect;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.IdentityModel.Benchmarks.IdentityModel.Protocols.OpenIdConnect;

/// <summary>
/// Snapshots discovery metadata and a code/PKCE authorization message, then applies their protocol rules.
/// The pinned package provides descriptors, not JSON/query parsers. These are the public materialization and
/// validation paths only; Microsoft OIDC configuration/message readers do different work and are not baselines.
/// </summary>
public class OpenIdConnectBenchmarks
{
    private const string Issuer = "https://identity.example.com";
    private OpenIdConnectProviderMetadataDescriptor _metadataDescriptor = null!;
    private OpenIdConnectAuthorizationRequestDescriptor _requestDescriptor = null!;
    private OpenIdConnectProviderMetadata _metadata = null!;
    private OpenIdConnectAuthorizationRequest _request = null!;

    [GlobalSetup]
    public void Setup()
    {
        _metadataDescriptor = new OpenIdConnectProviderMetadataDescriptor
        {
            Issuer = Issuer,
            AuthorizationEndpoint = Issuer + "/authorize",
            TokenEndpoint = Issuer + "/token",
            UserInfoEndpoint = Issuer + "/userinfo",
            JwksUri = Issuer + "/jwks",
            RegistrationEndpoint = Issuer + "/register",
            EndSessionEndpoint = Issuer + "/logout",
        };
        _metadataDescriptor.ScopesSupported.Add("openid");
        _metadataDescriptor.ScopesSupported.Add("profile");
        _metadataDescriptor.ResponseTypesSupported.Add("code");
        _metadataDescriptor.SubjectTypesSupported.Add("public");
        _metadataDescriptor.IdTokenSigningAlgValuesSupported.Add("ES256");
        _metadataDescriptor.IdTokenSigningAlgValuesSupported.Add("RS256");
        _metadataDescriptor.ClaimsSupported.Add("sub");
        _metadataDescriptor.ClaimsSupported.Add("name");
        _metadataDescriptor.CodeChallengeMethodsSupported.Add("S256");

        _requestDescriptor = new OpenIdConnectAuthorizationRequestDescriptor
        {
            ClientId = "orders-client",
            ResponseType = "code",
            RedirectUri = "https://orders.example.com/callback",
            CorrelationState = "request-42",
            Nonce = "nonce-42",
            CodeChallenge = "E9Melhoa2OwvFrEMTJguCHaoeK1t8URWbuGJSstw-cM",
            CodeChallengeMethod = "S256",
        };
        _requestDescriptor.Scopes.Add("openid");
        _requestDescriptor.Scopes.Add("profile");
        _metadata = MaterializeMetadata();
        _request = MaterializeAuthorizationRequest();
        if (!ValidateMetadata().Succeeded || !ValidateAuthorizationRequest().Succeeded)
        {
            throw new InvalidOperationException("OIDC fixtures must satisfy the protocol rules.");
        }
    }

    [Benchmark]
    public OpenIdConnectProviderMetadata MaterializeMetadata() => new(_metadataDescriptor);

    [Benchmark]
    public ProtocolValidationResult ValidateMetadata() => _metadata.Validate(expectedIssuer: Issuer);

    [Benchmark]
    public OpenIdConnectAuthorizationRequest MaterializeAuthorizationRequest() => new(_requestDescriptor);

    [Benchmark]
    public ProtocolValidationResult ValidateAuthorizationRequest() => _request.Validate();
}
