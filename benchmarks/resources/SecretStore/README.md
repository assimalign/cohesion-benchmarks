# SecretStore benchmarks

12 cases (6 methods across two value sizes), running entirely in process against the pinned
Cohesion packages. No server, socket, container or external service is started.

| Package / path | Cases | Work measured |
| --- | ---: | --- |
| `Assimalign.Cohesion.SecretStore.Hosting` | 8 | Seeded reads, missing-path lookup, and the store's purpose-scoped protect/unprotect path |
| `Assimalign.Cohesion.SecretStore.Client` | 4 | Secret and PEM response reads: request construction, bearer header and result handling |

Values are deterministic 32-byte and 4-KiB byte sequences. The actual engine is file-backed:
repository reads include file I/O and authenticated decryption of the protected file the seed
writes at initialization. Each benchmark creates and deletes its own temp directory. Missing-path
lookup is an invariant-cost control across the two store fixtures. The PEM client fixture measures
response handling only, not certificate validation.

The repository has no public in-process facade. Setup binds typed delegates to the packaged
internal repository; measured calls do not use reflection. The protection fixture uses the same
`secret-store / secrets / v1` purpose chain, a warmed key ring and a private temp key repository.
`Security.DataProtection` is explicitly referenced at the repository pin because Hosting marks that
implementation dependency private. The public client uses a caller-owned `HttpMessageInvoker`
transport whose handler creates fresh responses over bytes prepared in setup. Setup checks round
trips, the bearer header and route selection.

All cases are standalone. No counterpart package was added. BCL `AesGcm` was considered and rejected:
it does not perform the protector's key-ring lookup, purpose-bound HKDF derivation, random nonce
generation and versioned authenticated envelope handling. A raw HTTP call omits the client's path
validation and result handling; a dictionary omits persistence and encryption. Neither is an honest
substitute for the measured operation.

Not covered: certificate issuance/enrollment and CA hierarchy, trust-issuer persistence and token
authentication, HTTP endpoint dispatch, contention, cold startup/recovery and key rotation. The root,
the resource integration (`SecretStore.Hosting.ApplicationModel`, which ships no actions of its
own), Refs and Runtime packages do not receive separate startup or SDK benchmarks. The pinned
repository/client does not expose secret writes, enumeration or version-history operations, so
there are no write, secret-list or versioning benchmarks. No network or server throughput is claimed.

```powershell
dotnet build benchmarks/resources/SecretStore/benchmarks/Assimalign.Cohesion.SecretStore.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area SecretStore -Profile dry
```
