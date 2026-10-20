using Assimalign.Cohesion.IdentityModel.Protocols;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.IdentityModel.Benchmarks.IdentityModel.Protocols;

/// <summary>
/// Measures the shared metadata snapshot used by protocol implementations, including endpoints, roles and
/// extension properties. The minimal consumer derivative exposes the protected extension point without adding
/// protocol-specific work. Other libraries lack this neutral snapshot contract, so there is no baseline.
/// </summary>
public class ProtocolMetadataBenchmarks
{
    private MetadataDescriptor _descriptor = null!;

    [Params(4, 16)]
    public int EndpointCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _descriptor = new MetadataDescriptor { EntityId = "https://identity.example.com" };
        _descriptor.Roles.Add(ProtocolRole.IdentityProvider);
        _descriptor.Roles.Add(ProtocolRole.RelyingParty);
        for (int index = 0; index < EndpointCount; index++)
        {
            _descriptor.Endpoints.Add(new ProtocolEndpoint(new ProtocolEndpointDescriptor
            {
                Kind = new ProtocolEndpointKind("endpoint-" + index),
                Location = "https://identity.example.com/endpoint/" + index,
                Binding = ProtocolBinding.HttpPost,
                Role = ProtocolRole.IdentityProvider,
            }));
            _descriptor.Properties["extension-" + index] = IdentityClaimValue.FromString("value-" + index);
        }
    }

    [Benchmark]
    public ProtocolMetadata Materialize() => new Metadata(_descriptor);

    private sealed class MetadataDescriptor : ProtocolMetadataDescriptor;

    private sealed class Metadata(MetadataDescriptor descriptor) : ProtocolMetadata(descriptor, AuthenticationProtocol.Saml2);
}
