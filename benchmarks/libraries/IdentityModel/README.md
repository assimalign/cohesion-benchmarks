# IdentityModel benchmarks

42 cases across all seven packages (36 added to the original six). Existing benchmark names are preserved.

| Package | Cases | Work measured |
| --- | ---: | --- |
| IdentityModel | 6 | Subject materialization, provenance-preserving claim mapping, permission lookup at 8/32 claims |
| IdentityModel.Protocols | 2 | Shared metadata snapshots at 4/16 endpoints through the public derivation contract |
| IdentityModel.Protocols.OpenIdConnect | 4 | Discovery and authorization-request materialization and protocol-rule validation |
| IdentityModel.Protocols.Saml | 4 | Assertion materialization and SSO data rules for valid/expired assertions |
| IdentityModel.Token | 2 | Neutral issuer/audience/lifetime rules for valid/expired tokens |
| IdentityModel.Token.JsonWebToken | 20 | ES256 issue/read/full validation; HS256/RS256 reads; RSA PKCS#1/PSS and ECDSA signature verification; JWK parsing and RFC 7638 thumbprints |
| IdentityModel.Token.Saml | 4 | Token normalization at 8/32 multi-value attributes and bearer/issuer/audience/lifetime rules |

Microsoft.IdentityModel.JsonWebTokens remains the JWT counterpart, with its transitive Microsoft.IdentityModel.Tokens types for JWKs. The BCL's System.Security.Cryptography is the signature-only baseline: identical input, public key, hash, padding and signature representation. Full ES256 validation uses a fixed instant; the Microsoft lifetime callback applies the profile's required claims and maximum lifetime, and the Cohesion path explicitly checks audience membership. Both return token results without forcing Microsoft's lazy ClaimsIdentity.

The pinned APIs impose important limits. JWT writing and full signature-plus-claims validation support ES256 only. There is a public RSA signature verifier, but no HMAC verifier. HS256 and RS256 parsing does **not** verify signatures. OIDC and SAML expose descriptor-based contracts, not wire parsers; these benchmarks measure their materialization and data rules, not JSON/XML parsing, SAML signatures, decryption, or replay prevention. SAML validation outcomes are checked at fixed instants.

There is no public ClaimsPrincipal adapter: the canonical IdentitySubject construction is measured alone rather than compared to a different mutable output model. System.IdentityModel.Tokens.Jwt was considered but not added: the existing modern Microsoft handler covers JWT work, and another reader does not fill these unsupported Cohesion paths. Microsoft OIDC and SAML wire readers/cryptographic validators would do more work than the available descriptor paths and were not used as baselines. No new NuGet counterpart is needed. All cryptographic keys are generated in GlobalSetup; no key files are used.
