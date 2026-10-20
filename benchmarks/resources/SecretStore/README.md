# SecretStore benchmarks

36 cases (18 methods across two value sizes), running entirely in process against the pinned
Cohesion packages. No server, socket, container or external service is started.

| Package / path | Cases | Work measured |
| --- | ---: | --- |
| `Assimalign.Cohesion.SecretStore.Hosting` | 18 | Seeded and command-declared reads, missing-path lookup, declaration replay, store/delete cycles, add-secret command replay and execute/delete cycles, and the store's purpose-scoped protect/unprotect path |
| `Assimalign.Cohesion.SecretStore.Client` | 14 | Secret and PEM response reads, command request encoding, applied/rejected/deleted observations, and empty successful observations |
| `Assimalign.Cohesion.SecretStore.ApplicationModel.Orchestration` | 4 | Public add-secret input resolution from parameter and resource sources, including declaration validation and delivered JSON/base64 encoding |

Values are deterministic 32-byte and 4-KiB byte sequences. The actual engine is file-backed:
repository reads include file I/O and authenticated decryption, while writes include protected
framing, atomic replacement and the implementation's write-through behavior. Each benchmark
creates and deletes its own temp directory. Cycles reuse one key and restore its absent state;
declaration replay measures the shipped same-owner, same-source, same-value behavior rather than
inventing an overwrite API. Missing-path lookup is an invariant-cost control across the two store
fixtures. The PEM client fixture measures response handling only, not certificate validation.

The repository has no public in-process facade. Setup binds typed delegates to the packaged
internal repository; measured calls do not use reflection. The add-secret handler is reached through
`IResourceCommandHandler`; its unused certificate-authority collaborator is omitted. The protection
fixture uses the same `secret-store / secrets / v1` purpose chain, a warmed key ring and a private
temp key repository. `Security.DataProtection` is explicitly referenced at the repository pin because
Hosting marks that implementation dependency private. Public clients use caller-owned
`HttpMessageInvoker` transports whose handlers consume the encoded command body and create fresh
responses over bytes prepared in setup. The input resolver receives a fixed, already-resolved source
to isolate parsing/validation and delivery encoding from source retrieval. Setup checks round trips,
wire payloads, authorization, route construction, observation statuses and command deletion.

All cases are standalone. No counterpart package was added. BCL `AesGcm` was considered and rejected:
it does not perform the protector's key-ring lookup, purpose-bound HKDF derivation, random nonce
generation and versioned authenticated envelope handling. A raw `System.Text.Json` encode omits the
client's HTTP envelope and observation semantics; a dictionary omits persistence, encryption and
ownership checks. Neither is an honest substitute for the measured operation.

Not covered: certificate issuance/enrollment and CA hierarchy, trust-issuer persistence and token
authentication, HTTP endpoint dispatch, contention, cold startup/recovery and key rotation. The root,
ApplicationModel, Refs and Runtime packages do not receive separate startup/manifest/SDK benchmarks.
The pinned repository/client does not expose secret enumeration or version-history operations, so
there are no secret-list or versioning benchmarks. No network or server throughput is claimed.

```powershell
dotnet build benchmarks/resources/SecretStore/benchmarks/Assimalign.Cohesion.SecretStore.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area SecretStore -Profile dry
```
