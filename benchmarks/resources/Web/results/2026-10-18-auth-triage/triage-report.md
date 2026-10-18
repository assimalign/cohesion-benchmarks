# Cohesion Web authentication performance triage

Baseline: `74976cdc5`, `feature/web-auth-perf`, Windows ARM64, .NET 10.0.13. This is a diagnosis and a set of independently measured, reverted experiments, not an implemented fix.

**The largest authentication-related cost is reading the credential header, before authentication runs.** Cohesion's HTTP/1.1 reader performs an asynchronous pipe-to-stream read and allocates a one-byte array for every byte. Reading the bearer header alone costs **24.05 us**; the cookie header costs **37.17 us**. An equally long inert header costs **24.01 us**. Fully validating and materializing the JWT costs only **3.05 us**, and decoding the cookie ticket costs **4.48 us**.

The auth libraries therefore do not account for the reported twofold request cost by themselves. The header-length experiment, production stack samples, and source all identify the same transport amplification. The larger Cohesion cookie amplifies it further. Remaining differences are explicitly left in the budget below; these experiments do not establish an exact additive decomposition of end-to-end CPU time.

## 1. Fairness check

The servers make equivalent authentication decisions for the supplied successful fixtures, but their cookie bytes, crypto defaults, and general acceptance policies are not identical.

| Check | Evidence and result |
| --- | --- |
| JWT wire input | Both consume the same 283-character compact HS256 token: 27-byte header JSON, 151-byte payload JSON, eight claims (`iss`, `aud`, `sub`, `name`, `role`, `iat`, `nbf`, `exp`). The key is the harness's SHA-256 of `cohesion-benchmarks/jwt`; issuer, audience, subject and role come from the linked, unmodified `shared/BenchmarkShared.cs`. The minting order and lifetime match `ScenarioCatalog.cs:296`. |
| JWT checks | Both verify the signature, issuer, audience, expiration and not-before on every request. Both use five minutes of skew. Neither caches a validation result across requests. Cohesion uses fixed-time MAC comparison. ASP.NET Core's `ValidateIssuerSigningKey` default does not disable signature verification. |
| Claims | `MapInboundClaims=false` is explicitly set on ASP.NET Core. Both produce eight claims, identical string/integer64 value types, name `alice`, and role `admin`. Cohesion's identity authentication type is `Bearer`; ASP.NET Core's is `AuthenticationTypes.Federation`. Both identities are authenticated; this metadata difference explains no significant cost. See `fixture-check.txt` and `fixture-p02.txt`. |
| Library benchmark fairness | The primary ASP.NET Core validation row forces `ClaimsIdentity` and constructs the principal. Its unforced, lazy result is reported separately. Comparing only the lazy result with Cohesion's eager principal would exaggerate the difference. |
| Bearer handler work | ASP.NET Core also saves the access token in authentication properties by default. Its handler benchmark resolves a fresh scoped authentication service/provider per operation; a root-resolved provider would incorrectly reuse a handler and its result. Both measured sides create fresh request contexts. |
| Role policy | Both policies require the single role `admin`, using claim type `role`. They do not add another JWT verification on this successful path. |
| Cookie contents | Both sign in one `ClaimsIdentity("Cookies")`, one `Claim(ClaimTypes.Name,"alice")`, with issued/expiry timestamps and a 14-day lifetime. Sliding renewal is enabled but these fresh tickets are before the midpoint, so neither renews per request. |
| Cookie sizes | Cohesion v1: **297 plaintext bytes, 342 protected bytes, 456 base64url characters**. ASP.NET Core: **137 / 228 / 304**. Complete credential header lines are **484 versus 333 bytes**. ASP.NET Core's serializer substitutes defaults for repeated claim metadata; Cohesion v1 writes the full strings. These are equivalent fixture contents, not equivalent representations. |
| Protection defaults | Cohesion uses AES-256-GCM, HKDF-SHA256 and a 256-bit ring master. ASP.NET Core selects its Windows CNG AES-256-CBC/HMACSHA256 construction. Both authenticate and decrypt every request. Measured unprotect is 3.915 versus 3.511 us. Configuring ASP.NET Core for AES-256-GCM instead costs 5.967 us here; changing algorithm classes does not explain the large end-to-end gap. |
| Persistence | The published Cohesion `DataProtection-Keys` directory is persistence, not per-request disk access. A real file-backed provider wrapped with a read counter performs **zero repository reads over 1,000 known-key unprotect operations**. `KeyRing.ResolveForUnprotect` uses the in-memory frozen snapshot; only a miss can trigger the throttled reload. The microbenchmarks use separate persisted rings under this output folder. ASP.NET Core's test ring is explicitly persisted there; at-rest key protection/application-discriminator settings differ from host defaults, affecting setup rather than known-key decrypt work. |
| Different general policies | Cohesion's document lifetime validation treats a missing `exp` as absent; ASP.NET Core requires expiration by default. Boundary and audience-normalization rules also differ. These do not change the successful benchmark token's checks or explain its timing. Aligning acceptance policies is a separate **owner decision**, not a performance optimization made here. Cohesion's key grace/reload rules also differ from ASP.NET Core's management policy. |

Harness references, relative to `cohesion-benchmarks/benchmarks/Web`: Cohesion `src/Assimalign.Cohesion.Web.Benchmarks.CohesionServer/src/Program.cs:105,116,122,315,330,443`; ASP.NET Core `src/Assimalign.Cohesion.Web.Benchmarks.AspNetCoreServer/src/Program.cs:133,140,143,352,357,484`; `shared/BenchmarkShared.cs:76`; runner `Scenarios/ScenarioCatalog.cs:296`. The benchmark project links the actual shared constants. Baseline PE inspection (`baseline-pe.csv`) confirms optimization is enabled in the published auth/identity/protection assemblies; they are not Debug builds.

Primary counterpart sources: [ASP.NET Core encryption defaults](https://github.com/dotnet/aspnetcore/blob/v10.0.13/src/DataProtection/DataProtection/src/AuthenticatedEncryption/ConfigurationModel/AuthenticatedEncryptorConfiguration.cs), [ticket serializer](https://github.com/dotnet/aspnetcore/blob/v10.0.13/src/Security/Authentication/Core/src/TicketSerializer.cs), [bearer handler](https://github.com/dotnet/aspnetcore/blob/v10.0.0/src/Security/Authentication/JwtBearer/src/JwtBearerHandler.cs), [IdentityModel keyed-hash pooling](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/8.0.1/src/Microsoft.IdentityModel.Tokens/SymmetricSignatureProvider.cs). Copies and URLs are in `sources/`.

## 2. Measurements and per-scheme breakdown

All times below are ns/op unless marked us. All B/op describe managed allocations. MemoryDiagnoser is enabled throughout. The main run uses the default isolated job; focused runs use the official Medium job (two launches, ten warmups, fifteen measured iterations per launch). Both sides run in the same BenchmarkDotNet run, with Server GC, four visible processors, affinity `0xF`, .NET 10.0.13 ARM64 and SDK 10.0.402. BDN uses its High performance power plan; the original end-to-end report used Balanced. Thus stage timings are useful cost scales, not exact subtractable components of that older report.

**Allocation qualification:** some counters were demonstrably unstable in this configuration, including negative whole-process deltas. This also affected a few isolated-job MemoryDiagnoser rows. Do not use the abandoned in-process run. Unmarked values below are stable BDN results; `~`/dagger values use controls or short collection-free checks, or explicitly show uncertainty. Raw reports retain the anomalous values; they were not edited. `alloc-baseline.csv` and `headers-cpu-baseline.csv` preserve the checks. The load-check allocation column is excluded from conclusions. The cached-handle and compact-format runs also contain anomalous complete-operation allocation rows; their gains below are timing claims, not allocation claims.

### Bearer and bearer plus role policy

| Stage | Cohesion ns | C B/op | ASP.NET Core ns | A B/op |
| --- | ---: | ---: | ---: | ---: |
| Split and header/payload base64url decode | 157.0 | 960 | included in parse | — |
| JSON to canonical descriptor | 1,329.2 | 3,064 | included in parse | — |
| Immutable model snapshot | 188.2 | 888 | included in parse | — |
| Complete document parse | 1,669.5 | 4,912 | 1,008.8 | 2,312 |
| Signature input/signature buffers | 113.9 | 1,328 | included in validation | — |
| HMAC over prepared bytes | 563.3 | 0 | 325.4 | 168 |
| Document, issuer, audience and lifetime rules | 134.2 | 352 | included in full validation | — |
| Claims to BCL principal | 468.1 | 2,016 | ~562.2 delta* | ~2,720 delta* |
| Full validation plus usable principal | 3,046.7 | 8,608 | 2,480.2 | 6,496 |
| ASP.NET Core full validation, principal not forced | — | — | 1,964.3 | 3,776 |
| Fresh fake context + credential header | 101.1 | 1,216 | 260.2 | 1,904 |
| Fresh Cohesion handler + context | 3,342.9 | 10,800 | included below | — |
| Fresh context + authentication service/handler | 3,440.0 | 11,448 | 3,932.7 | 12,216 |
| Role-policy evaluation over authenticated principal | 44.4 | ~72† | 208.4 | 704 |

*The ASP.NET Core principal delta is derived from its principal-forced and lazy rows in the same default run, not an independently isolated mapper. It includes the adapter's principal/Task construction. Cohesion's full-library helper calls the actual private signature/options/issuer/audience methods through delegates bound once at setup; no reflection executes in a timed operation. The focused run supersedes the original helper and boxed-tuple stage rows.

The real handler/service rows call production code and contain fresh-context overhead. They are not full HTTP middleware measurements. Cohesion dispatch over the handler adds about 97 ns in this fixture; no evidence supports assigning the missing tens of microseconds to authentication dispatch. Role evaluation itself is cheaper in Cohesion. The extra end-to-end role-policy cost therefore includes other routing/middleware work. Cookie service dispatch adds about 135 ns over its handler row, likewise much smaller than the end-to-end increment.

### Cookie

| Stage | Cohesion ns | C B/op | ASP.NET Core ns | A B/op |
| --- | ---: | ---: | ---: | ---: |
| Known-key lookup, with lifecycle checks | 39.5 | 0 | included in unprotect | — |
| HKDF-SHA256 | 1,157.1 | 288 | different KDF, included below | — |
| Fresh AES-GCM + decrypt + plaintext buffer | 2,695.1 | uncertain† | different construction, included below | — |
| Base64url decode | 82.0 | 368 | 59.8 | 256 |
| Unprotect | 3,914.9 | 656 | 3,510.8 | 560 |
| ASP.NET Core AES-GCM control | — | — | 5,967.2 | 560 |
| Deserialize ticket and construct principal | 375.4 | 1,712 | 415.4 | 1,400 |
| Cookie string to usable ticket (`SecureDataFormat` counterpart) | 4,479.8 | 2,736 | 4,089.8 | 2,072 |
| Fresh Cohesion cookie handler + context | 5,007.4 | 6,040 | included below | — |
| Fresh context + service/handler | 5,142.2 | 6,480 | 5,835.9 | 7,104 |

The standalone AES allocation row is unreliable (BDN reported 2,207 B; short checks approximately 402 B; paired-stage arithmetic suggests 368 B). Its timing is stable. This uncertainty does not affect the measured complete-unprotect or complete-ticket allocation rows. Role-policy BDN reported zero despite a repeatable 72 B in the short check. These exceptions are not treated as zero-allocation wins.

Same-algorithm cookie-size control: unprotecting 297 versus 137 plaintext bytes costs **3.920 versus 3.880 us**, only 40 ns difference. The size penalty is amplified mainly by the HTTP reader:

| Actual production line reader over a preloaded, reused pipe | Wire bytes | us/op | B/op |
| --- | ---: | ---: | ---: |
| Tiny inert header | 6 | 0.575 | 568 |
| Bearer credential | 307 | 24.050 | ~11,336† |
| Inert header, bearer-sized | 307 | 24.013 | 11,336 |
| Cohesion v1 cookie | 484 | 37.46 | 17,360 |
| ASP.NET Core cookie bytes through the **Cohesion** reader | 333 | 25.90 | uncertain† |

The last row is a size control, not an ASP.NET Core parser benchmark. Shrinking only Cohesion's cookie to the counterpart's wire length saves **11.56 us** in Cohesion's current reader. The default serialization mismatch therefore explains a material part of the cookie result, despite explaining almost none of the decrypt cost. No isolated Kestrel parser measurement was taken.

### End-to-end CPU budget

Derived from `results/2026-10-17-win-arm64/results.md`, efficiency table: **CPU us/request = 1,000,000 / requests per server CPU-second**. These are differences of reported medians, not paired-run subtraction. The routed plaintext row is the closest minimal-pipeline baseline.

| Scenario | C CPU us/req | A CPU us/req | C increment over routed plaintext | A increment |
| --- | ---: | ---: | ---: | ---: |
| Plaintext | 35.45 | 29.50 | — | — |
| Routed plaintext | 38.94 | 30.67 | — | — |
| Bearer | 73.18 | 36.21 | **34.24** | **5.55** |
| Bearer + role | 75.77 | 38.39 | **36.83** | **7.73** |
| Cookie | 89.17 | 37.13 | **50.24** | **6.47** |

Using unrouted plaintext instead gives increments C/A of 37.73/6.72 us (bearer), 40.32/8.90 us (role), and 53.72/7.64 us (cookie). Raw calculations: `e2e-cpu.csv`.

The isolated library is about **9%** of Cohesion's bearer/cookie auth increment, versus about **45%/63%** for ASP.NET Core. Adding the measured Cohesion credential-line work to the isolated library gives cost scales of **27.10 us bearer / 41.64 us cookie**, compared with increments of 34.24/50.24 us. The numerical remainders are roughly 7.15/8.60 us; they include unmeasured transport, actual middleware/context differences, GC and environment effects. They are **not** a measurement of authentication middleware alone. The role evaluation contributes only 0.044 us locally; end-to-end role increments over bearer are 2.59/2.18 us.

A supplementary four-worker check does not reproduce a native-crypto contention explosion: approximately **3.32 us CPU/JWT** and **4.57 us CPU/cookie format**. Four-worker header-reading medians are **25.70 us CPU/bearer line** and **37.03 us CPU/cookie line**. Together with the trace and inert-header control, this identifies the dominant missing work without claiming the entire residual has been assigned.

## 3. Ranked root causes

Paths below are relative to the worktree at `74976cdc5`; timings are measured stage costs and may overlap.

1. **Credential length drives one-byte HTTP reads: 24.05 us bearer, 37.17–37.46 us cookie.** `libraries/Http/Assimalign.Cohesion.Http.Connections/src/Internal/Http1/Http1MessageReader.cs:644` loops over `ReadByteAsync`; `:710` allocates `new byte[1]` and calls stream read for each byte. `libraries/Connections/Assimalign.Cohesion.Connections/src/DuplexPipeStream.cs:113` reads/advances the pipe on each call. This also entails at least 9,824/15,488 B of one-byte arrays for the respective lines, before line/string storage. Equal-length inert credentials reproduce the cost. Cookie v1's repeated metadata strings add 151 wire bytes over ASP.NET Core and about 11.56 us through this reader.
2. **Fresh cookie crypto setup: 3.915 us unprotect.** `libraries/Security/Assimalign.Cohesion.Security.DataProtection/src/Internal/AesGcmDataProtector.cs:108` derives a subkey per call; `:111` constructs/imports AES-GCM state per call. HKDF costs 1.157 us; fresh AES-GCM/decrypt costs 2.695 us. Key lookup is only 39.5 ns. The handle-reuse prototype isolates a 3.546 us saving while preserving each call's ring resolution and tag check.
3. **JWT eager parsing and model materialization: 1.670 us / 4,912 B.** `libraries/IdentityModel/Assimalign.Cohesion.IdentityModel.Token.JsonWebToken/src/Internal/JsonWebTokenParser.cs:19,35,50,66` creates segments/decoded buffers, JSON documents, duplicate-member sets, descriptors and canonical claims. `libraries/IdentityModel/Assimalign.Cohesion.IdentityModel.Token/src/IdentityToken.cs:54` and `libraries/IdentityModel/Assimalign.Cohesion.IdentityModel/src/Claims/IdentityClaimCollection.cs:27` copy the collections; `JoseHeader` snapshots its dictionary. ASP.NET Core parse costs 1.009 us / 2,312 B, with different materialization boundaries. Descriptor JSON work dominates the Cohesion parse stage; the snapshot alone is 188 ns.
4. **HMAC execution/setup: 563 ns versus 325 ns.** `resources/Web/Assimalign.Cohesion.Web.Authentication.Bearer/src/HmacJwtSignatureVerifier.cs:58` already uses span-based one-shot APIs and fixed-time comparison. The suspicion of a freshly constructed managed `HMACSHA256` in Cohesion is ruled out. ASP.NET Core's IdentityModel implementation pools keyed hash instances. A retained single-instance experimental bound is 353 ns, saving about 210 ns before any thread-safe pool/lifetime cost. Four-worker validation does not turn this into a tens-of-microseconds bottleneck.
5. **Duplicate BCL claim construction: 468 ns / 2,016 B for bearer mapping; 375 ns / 1,712 B for cookie deserialize.** `resources/Web/Assimalign.Cohesion.Web.Authentication.Bearer/src/Internal/JwtClaimsPrincipalMapper.cs:20,42,53` first creates claims without a subject in a list, then passes them to `ClaimsIdentity`, which copies them. `resources/Web/Assimalign.Cohesion.Web.Authentication.Cookie/src/Internal/CookieTicketSerializer.cs:101,115,122,125` uses the same pattern. Binding new claims directly to the destination identity removes the redundant copies without sharing principals between requests. Prototype results are below.

Production evidence is in `bearer.nettrace`, `trace-inclusive.txt`, `trace-exclusive.txt`, and `trace-allocations.csv`. The tool version exposes **sampled thread time** on Windows, not Linux-style kernel CPU sampling; waiting threads are included. ReadByte has 35.28% inclusive / 9.82% exclusive sampled time; bearer `AuthenticateCore` 8.75% inclusive; mapper 1.86%; token parse 4.71%. Inclusive values overlap through continuation stacks and must not be added or described as CPU percentages. Approximately 51% of allocation ticks were attributed to byte arrays. The trace captured 826 GC starts and 157,365 allocation ticks. A traced run is not a throughput comparison: current single-run throughput differed from the earlier report, so the old medians remain the end-to-end reference.

## 4. Fix proposals and independent prototypes

Ranked for the current baseline. **The first two gains overlap heavily. Do not add them:** fixing the reader should remove most of the CPU incentive for a new cookie format.

| Priority | Change | Measured gain or explicit estimate | Patch / decision |
| --- | --- | --- | --- |
| 1 | Scan buffered HTTP/1.1 input for CRLF, consume bounded slices, retain unread body/next-request bytes | **Estimate:** recover much of the 24–37 us credential-line work; no production buffered-reader implementation or end-to-end gain was measured | No transport patch, respecting the brief's limit on chasing non-auth work. A separate transport implementation is the highest-value next task. |
| 2, conditional | Compact default claim metadata in the cookie ticket | 297 -> **67** plaintext bytes; 456 -> **150** cookie characters; credential line 484 -> **178** bytes. Header read 13,750 ns versus 37,460 ns; saving **23.71 us**. Format decode 4.480 -> 4.217 us. | `patches/02-compact-cookie-v2.patch`. **Needs owner decision:** version/rolling-deployment strategy. |
| 3 | Reuse exclusively borrowed AES-GCM handles by resolved key object and purpose | Unprotect **3.915 -> 0.369 us** (90.6%); cookie format **4.480 -> 0.842 us**; fresh context/service **5.142 -> 1.397 us**, a **3.745 us** saving | `patches/01-cached-aes-handles.patch`. **Needs owner decision:** retained imported-key lifetime and deterministic disposal. |
| 4 | Construct claims with their destination identity as subject | Bearer mapper **468.1 -> 302.9 ns** (35.3%, saves 165.2 ns), **2,016 -> 1,216 B**; cookie deserialize **375.4 -> 317.7 ns** (15.4%, saves 57.7 ns), **1,712 -> 1,464 B**. Full bearer service **3,440.0 -> 3,249.5 ns**, saves 190.5 ns. | `patches/03-direct-principal-construction.patch`; no public API or wire change. |
| 5 | Reduce eager JWT descriptor/model/buffer churn | **Estimate only:** the 1.670 us parse and 4,912 B stage is the upper bound; no safe complete streaming-parser rewrite was prototyped | Preserve duplicate-member rejection, depth limits, numeric fidelity, algorithm/header checks, and canonical-model immutability. Update JWT and family design docs if ownership/laziness changes. |
| 6 | Thread-safe keyed HMAC reuse | Experimental single-instance bound: about **0.210 us**. A real bounded concurrent pool would save less and needs ownership/disposal design | No production patch. Never share an unguarded HMAC instance concurrently. |

**01: cached handles.** The ring lookup, revocation/grace check, producing-key selection, AAD and tag verification still run every call. The cache is keyed by the resolved immutable key object, not just a GUID. Imported handles are exclusively borrowed, bounded per key/purpose, and discarded on failed decrypt/overflow. Temporary derivation bytes are still zeroed, but imported key material lives longer. The prototype relies on eventual SafeHandle cleanup for retained entries; deterministic provider disposal/retirement would require an ownership design, potentially an additive public disposal API. No validation-result or principal cache was introduced. Required design update: DataProtection `docs/DESIGN.md`, crypto construction and ownership. Tests: **53 data-protection + 60 cookie passed**, including warmed-cache expiry coverage and added revocation-after-reload and concurrent valid/invalid-tag tests. Further tests: deterministic disposal/drain, pool bounds under bursts, failed imports, retirement cleanup and longer concurrency stress.

**02: compact v2.** Default strings use explicit boolean discriminators, avoiding reserved-string collisions. Properties, timestamps, claim values/types/issuers and principal semantics remain unchanged. The reader accepts v1 and v2; existing v1-only nodes reject newly written v2 tickets. Shipping this patch as-is into a mixed fleet would cause authentication failures. Prefer reader-first/writer-later deployment, or an explicit negotiated/configured write format. A public format option would be a public API addition; the experimental patch itself changes only private wire handling. Required design update: Cookie `docs/DESIGN.md`, binary format and rolling compatibility. **62 cookie tests passed**, including a golden v1 ticket produced by the untouched baseline serializer and literal-NUL metadata round trips. Further tests: full multi-identity/default/custom-field matrix and mixed-version fleet behavior. After fixing header reading, re-evaluate whether the remaining size benefit justifies format migration.

The focused before/after comparison has stable counterpart controls: ASP.NET Core unprotect is 3.511 -> 3.508 us in 01; format is 4.090 -> 4.091 us in 02; deserialize is 415.4 -> 406.5 ns in 03. ASP.NET Core full JWT validation rises 2.480 -> 2.546 us in 03. Cohesion full JWT validation is 3.047 -> 2.955 us there, but this 92 ns difference is small relative to the error bands and cross-run drift; prefer the isolated mapper's 165 ns saving and 800 B reduction as the clear result. These are microbenchmark gains, not measured increases in whole-server requests/second.

**03: direct principal construction.** All **57 bearer, 60 cookie and 106 authorization tests passed**. It preserves fresh per-request identities, name/role types, issuers and values; no shared mutable principal is cached. It adds no public API, serializer version, reflection or dependency. Design semantics do not change; a performance note in Bearer/Cookie design docs is sufficient. Further coverage should retain multi-identity ordering, custom name/role types and original-issuer assertions.

All three experiments are independent, saved as complete patches and reverted before completion. All affected production builds retain the repo's trim/AOT analyzers; no new Microsoft.Extensions package, dynamic code or reflection-based JSON was added to Cohesion. The benchmark harness uses setup-time reflection solely to bind internal measurement delegates. **A NativeAOT publish/run was not performed**, and these are JIT performance figures, not AOT performance claims. The prototype patches are experiments requiring the design/ownership decisions above, not ready-to-merge changes.

The buffered-reader proposal should preserve Latin-1 decoding, request/header limits, the first-byte/header deadlines, unread body bytes, pipelining and CONNECT stream ownership. Its intended implementation can stay internal; update the HTTP Connections design/overview and any affected stream-ownership contract. Add fragmented/multi-segment CRLF, pipelined requests, exact-limit/over-limit, cancellation/timeout, malformed line and body-boundary tests. Keyed HMAC reuse similarly can stay internal, but must document ownership in Bearer design docs and test concurrent use, replacement/disposal, invalid signatures and all supported HS algorithms. No new dependency or dynamic code is needed for either proposal.

Changing public `Task` contracts to `ValueTask`, caching successful JWT/ticket validation, or caching principals is not justified by these measurements. The latter two would change expiration/revocation/mutability semantics and require an explicit owner decision; none was implemented.

### Other suspected costs checked

- **Disk/key deserialization on each valid cookie:** ruled out by the 1,000-call repository counter and 39.5 ns lookup row.
- **Managed HMAC object construction:** ruled out by the existing static span APIs at `HmacJwtSignatureVerifier.cs:58`; native keyed-hash setup remains measurable.
- **Disabled logging and exception control flow:** the valid bearer/cookie paths do not invoke their rejection EventSources or throw exceptions. Rejection metadata formatting is behind `IsEnabled` in the respective `src/Internal/EventSource/*EventSource.cs:26` wrappers. Invalid-input performance is outside this successful-request study.
- **Async/Task and feature plumbing:** handler success uses `Task.FromResult` (Bearer `JwtBearerHandler.cs:109`, Cookie `CookieAuthenticationHandler.cs:85`); service awaits, creates its item-key string, caches the handler within the request and installs the result feature (`AuthenticationService.cs:54,147,165`). `Extensions/AuthenticationWebApplicationExtensions.cs:36` awaits the service and assigns `context.User`. This allocates, but fresh service/handler rows do not reproduce the missing tens of microseconds.
- **LINQ/closures/boxing:** default-run staged holders accidentally boxed value tuples; the focused 960/1,328 B rows remove that benchmark-only artifact. Production collection snapshots and claim cloning are accounted for above. No successful-request evidence elevates a closure or boxing hotspot over those measured costs.

## 5. Incidental findings and boundaries

- The trace also exposes HTTP/1 request-body drain and MemoryStream growth. They were not chased beyond this note.
- Feature lookup/write and scheme-cache-key/Task allocations remain observable auth plumbing, but the measured service delta is small and the ASP.NET Core service fixture allocates more overall. No feature-collection or DI redesign is proposed from this evidence.
- Allocation-counter instability needs a separate minimal runtime/tool investigation. Do not infer zero allocations from the flagged rows or use negative load-check deltas.
- The global missing-expiration acceptance difference is a security-policy compatibility question. Tightening it needs deliberate failure-semantics tests and owner agreement; it was not silently changed.

The Authentication project has `docs/DESIGN.md` but no `docs/OVERVIEW.md` at this commit. Relevant design/overview documents and the requested general/Web/resource rules were reviewed; no standalone performance/AOT rule file exists beyond the sections in the general rules.

## 6. Reproduction

The investigation ran a stage-level harness that reached into Cohesion internals by reflection, against the
2026-10-18 working copies; it was not kept, and its commands and paths are omitted here. The same questions are
measured by the current suites:

- `benchmarks/libraries/Http` — `Http1RequestBenchmarks`: one HTTP/1.1 request cycle with no, bearer-sized and
  cookie-sized headers (the header-read cost behind section 3, item 1).
- `benchmarks/resources/Web` — `JwtBearerAuthenticationBenchmarks`, `CookieAuthenticationBenchmarks`,
  `RolePolicyBenchmarks`: per-request authentication against ASP.NET Core.
- `benchmarks/libraries/IdentityModel` and `benchmarks/libraries/Security` — JWT parsing/validation and data
  protection in isolation.

```powershell
./scripts/Invoke-Benchmarks.ps1 -Area Http,Web -Profile ci -CohesionRoot <checkout> -Output <dir>
./scripts/Compare-Benchmarks.ps1 -Baseline <before> -Current <after>
```

The end-to-end numbers come from the Web harness (`../../run.ps1`); `ab/` holds the before/after tables.
