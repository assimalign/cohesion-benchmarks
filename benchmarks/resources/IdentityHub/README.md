# IdentityHub benchmarks

5 standalone cases covering `Assimalign.Cohesion.IdentityHub.Hosting` and its root application
contracts: validated composition/build/disposal with 1 or 16 clients and two audiences, access-token
issuance, identity-token issuance, and JWKS serialization. Build includes client-secret hashing,
audience validation, endpoint-service construction and the explicit standalone Local resource scope.
Inputs and client identifiers are prepared in setup; files stay in a unique, cleaned temp directory.
The project explicitly references the pinned `IdentityModel.Token.JsonWebToken` and `Web.Hosting`
packages because the host's published nuspec omits these private runtime dependencies. They support
the measured host paths; this suite does not separately benchmark those libraries.

The pinned commit contains a real identity endpoint, unlike the other template areas. Token cases
bind typed delegates to the packaged internal `IdentitySigningKey` during setup because the public
host always starts a socket listener. They measure descriptor/claim construction, the signing lock
and ES256 serialization together. Key generation and signature/claim checks are setup work. Time is
fixed; no clock-dependent validation is used. JWKS reuses an output buffer and includes writer
creation, writing and flushing.

No counterpart package was introduced. A generic host omits identity registration validation; a
raw JWT writer omits the hub's descriptor and claims policy. Neither performs the same operation.

Not covered: host start/stop (it unconditionally binds an HTTP/TLS listener), endpoint dispatch,
client authentication, discovery, device authorization, persistent command-registry mutations,
bootstrap verification, signing-key persistence, ApplicationModel command authoring/planning,
Models property bags and Refs/Runtime packaging. The signing helper measurements do not claim
end-to-end endpoint performance. Trivial date/time helpers are deliberately omitted.

```powershell
dotnet build benchmarks/resources/IdentityHub/benchmarks/Assimalign.Cohesion.IdentityHub.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area IdentityHub -Profile dry
```
