using System;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.IdentityModel.Benchmarks.IdentityModel;

/// <summary>
/// Materializes a canonical subject, maps cross-protocol claim names, and evaluates an authorization claim.
/// ClaimsPrincipal has a different mutable object model and lacks this provenance-preserving mapper, so these
/// paths stand alone instead of comparing different output shapes. No ClaimsPrincipal adapter exists at this pin.
/// </summary>
public class IdentitySubjectBenchmarks
{
    private IdentitySubjectDescriptor _descriptor = null!;
    private IIdentityClaimCollection _claims = null!;

    [Params(8, 32)]
    public int ClaimCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _descriptor = new IdentitySubjectDescriptor
        {
            Identifier = new SubjectIdentifier("user-42"),
            DisplayName = "Benchmark User",
        };
        _descriptor.Claims.Add(new IdentityClaim("urn:oid:0.9.2342.19200300.100.1.3", "user@example.com"));
        for (int index = 1; index < ClaimCount - 1; index++)
        {
            _descriptor.Claims.Add(new IdentityClaim("permission", "orders." + index));
        }
        _descriptor.Claims.Add(new IdentityClaim("permission", "orders.write"));
        _claims = new IdentityClaimCollection(_descriptor.Claims);
        if (!HasClaim() || Canonicalize().GetString("email") != "user@example.com")
        {
            throw new InvalidOperationException("The authorization and mapping fixtures are invalid.");
        }
    }

    [Benchmark]
    public IdentitySubject Materialize() => new(_descriptor);

    [Benchmark]
    public IIdentityClaimCollection Canonicalize() => _claims.Canonicalize();

    [Benchmark]
    public bool HasClaim() => _claims.HasClaim("permission", "orders.write");
}
