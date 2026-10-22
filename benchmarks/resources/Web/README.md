# Web benchmarks: Cohesion Web vs ASP.NET Core

End-to-end HTTP benchmarks of the Cohesion Web resource (`resources/Web` in
[cohesion](https://github.com/assimalign/cohesion)) against ASP.NET Core, the framework with the
same runtime, the same JIT and a counterpart for almost every Web package. Two server applications
implement the same scenarios, one per framework; a runner starts one server process per scenario,
checks that it answers correctly, and measures it under load with a raw-socket HTTP/1.1 client.

Measurements are published through the nightly workflow artifacts and dashboard; see the
[repository guide](../../../README.md). Local results are git-ignored.

## Running it

Prerequisites:

- .NET SDK 10.0.300 or later.
- A token that can read the Assimalign GitHub Packages feed. The Cohesion server restores its SDKs
  and `Assimalign.Cohesion.*` packages from the cohesion CI build pinned in `global.json`, through
  the repository's `nuget.config`; no cohesion checkout is needed:

  ```powershell
  gh auth refresh -h github.com -s read:packages      # once
  $env:NuGetPackageSourceCredentials_assimalign = "Username=<github-user>;Password=$(gh auth token)"
  ```

  `../../../scripts/Update-CohesionVersion.ps1` moves the pin (here and for the BenchmarkDotNet
  suites) to the newest cohesion `main` build, or `-Version <v>` to any build still on the feed,
  such as a cohesion pull request's (`../../../scripts/Resolve-CohesionVersion.ps1 -PullRequest <n>`).
  The report records the cohesion commit behind the pinned build.

Then, from this folder:

```powershell
./run.ps1                                            # every scenario, 1 iteration (~25 min)
./run.ps1 -Iterations 3 -Label "my machine"          # three measurement iterations (~70 min)
./run.ps1 -Scenarios plaintext,json,full-stack       # a subset; also: core, feature
./run.ps1 -ValidateOnly                              # start each server, record its response, no load
./run.ps1 -SkipPublish -- --server-cores 6 --gc workstation   # anything after -- goes to the runner
```

`run.ps1` publishes both servers (Release, self-contained for the host RID) into
`artifacts/publish/`, builds the runner, and writes `results.md` and `results.json` into
`results/<timestamp>/`. `dotnet run --project benchmarks/Assimalign.Cohesion.Web.Benchmarks.Runner/src -c Release -- --help`
lists every runner option.

Leave the machine idle while it runs; a build or a browser tab competes for the same cores.

## How a measurement works

```
runner (load generator, pinned to the client cores)
  │  for each scenario, iteration, framework (order alternates):
  ├─ start   <framework>-server --scenario <pipeline> --port <free port>
  │          env: DOTNET_PROCESSOR_COUNT, DOTNET_gcServer, DOTNET_GCHeapCount; affinity = server cores
  ├─ ready   poll until any HTTP response  → startup time (process start to first response)
  ├─ check   one exchange: status, expected body text, expected headers → pass or fail
  ├─ warmup  N keep-alive connections replay pre-encoded requests (5 s, discarded)
  ├─ measure same connections (10 s): latency histogram, status classes, bytes, server and client CPU
  └─ stop    working set, then kill the process
```

- **One pipeline per process.** A server composes exactly the scenario's pipeline, so a number
  means "the minimal application plus this feature", never a shared app where unrelated middleware
  runs on every request. `full-stack` is the deliberate exception.
- **The load generator** (`benchmarks/.../Runner/src/Load`, `Http`) writes each request once as bytes and
  replays it on keep-alive sockets, like wrk: a Host header plus whatever the scenario declares.
  It parses only the response framing (status, Content-Length, chunked, Connection) and records
  each response's latency in a log-linear histogram (exact below 256 ticks, then within 0.8%).
  Scenarios that need state (a sign-in cookie, a session, an antiforgery pair) run a setup request
  once per connection, so 64 connections are 64 users, not one hot session.
- **Validation before load.** A server that answers the wrong thing fast is reported as failed. A
  run also fails on socket errors or on any response outside the expected status class.
- **Cores.** The server is pinned to the first third of the logical processors and told that count
  through `DOTNET_PROCESSOR_COUNT` (the runtime sizes its thread pool, GC heaps and transport queues
  at startup, before the affinity mask applies); the runner takes the rest. On loopback the client
  spends roughly 1.5–2× the server's CPU per request, which is why the server gets a third, not a
  half. On the Snapdragon X Elite that is one 4-core cluster for the server and two for the client.
- **Order alternates** between frameworks per scenario and iteration, so neither always runs on a
  warmer or more throttled machine. Each server is launched once, unmeasured, before the run: the
  first launch after a publish pays for disk reads and antivirus scans.

### Reading the numbers

- **Throughput** is requests per second in the measured phase (messages per second for
  `websocket-echo`). **Ratio** is Cohesion ÷ ASP.NET Core; above 1 favors Cohesion.
- **† marks a client-bound run**: the server used less than 85% of its cores, so the load
  generator may have set the pace. Compare **requests per CPU-second** there instead: completed
  requests divided by the server's CPU time, which holds whoever set the pace.
- **Startup** is process start to first HTTP response; **working set** is read after the run.
- These are **loopback numbers on one machine**. Absolute values say little about a Linux server
  with a separate load generator; the ratios and the efficiency column are what transfer. Re-run on
  the target hardware before quoting a number.

## Parity

Both servers are published the same way (Release, self-contained, same .NET runtime, JIT with the
default tiered PGO, invariant globalization) and receive the same environment from the runner.
Where the frameworks' defaults differ in a way that is configuration rather than design, the
benchmark equalizes it:

| Setting | Cohesion default | ASP.NET Core default | Benchmark |
| --- | --- | --- | --- |
| GC | Workstation (Sdk.Web sets nothing) | Server (`Microsoft.NET.Sdk.Web`) | Server GC, one heap per server core, on both (`--gc` to change) |
| Logging | No providers | Console, Debug, EventSource | ASP.NET Core providers cleared; Cohesion's EventSource forwarding left on (its default) |
| Builder | `WebApplication.CreateBuilder` | `CreateBuilder` adds host filtering and more | ASP.NET Core uses `CreateSlimBuilder` |
| Endpoint thunks | Source-generated interceptors | Runtime-generated (RDF) under JIT | ASP.NET Core's Request Delegate Generator enabled |
| JSON | Source-generated context | Reflection or context | The same source-generated context, web defaults, on both |
| JWT role claim | `roles` | `role` (with `MapInboundClaims = false`) | `role` on both |
| Security headers | `UseSecurityHeaders()` | none | ASP.NET Core writes the same four headers from an `OnStarting` callback |
| Cookie policy | Upgrades `SameSite=None` to `Secure` | No upgrade | ASP.NET Core `Secure = Always` (same `Set-Cookie` for the scenario's cookie) |
| HTTPS redirect | 307 to 443 | 307, port unknown (no redirect) | 307 to 443 on both |
| Antiforgery | Middleware rejects | Middleware records; form binding rejects | ASP.NET Core endpoint checks `IAntiforgeryValidationFeature`; header `X-CSRF-TOKEN` on both |
| Compression | Brotli + gzip, Fastest | Brotli + gzip, Fastest | Both providers, `CompressionLevel.Fastest` |

Differences the benchmark keeps, because they are each framework's behavior:

- **Response framing.** Cohesion buffers small handler results and sends `Content-Length`;
  ASP.NET Core minimal APIs write string and JSON results chunked. The terminal-middleware
  scenarios (`plaintext`, `middleware-chain`, `large-response`) set `Content-Length` on both.
- **Headers.** Kestrel adds `Server: Kestrel`; ASP.NET Core's problem details add `traceId` and
  cache headers. The report's parity table lists head and body bytes for every scenario.
- **Brotli output size.** Same API and level on both, but ASP.NET Core writes the 8 KiB string in
  segments and flushes each through the encoder: 179 bytes against Cohesion's 90.
- **Health, OpenAPI, validation.** ASP.NET Core serves health through `UseHealthChecks` with a
  small JSON writer (Cohesion's `MapHealthChecks` needs no router and writes JSON); the OpenAPI
  documents describe the same endpoints but differ in content; the validators are different engines
  (ObjectValidation profile vs .NET 10 DataAnnotations validation) enforcing the same three rules.

## Coverage

Every Web package with a plaintext HTTP/1.1 request path has a scenario. The report's scenario
table describes each one.

| Web package | Scenarios |
| --- | --- |
| `Web`, `Web.Server`, `Web.Hosting` | `plaintext`, `plaintext-pipelined`, `middleware-chain`, `large-response`; startup and working set in every scenario |
| `Web.Routing` | `plaintext-routed`, `routing-table` |
| `Web.Api`, `Web.Serialization` | `json`, `query-binding`, `json-post` |
| `Web.SecurityHeaders` | `security-headers` |
| `Web.ForwardedHeaders` | `forwarded-headers` |
| `Web.HostFiltering` | `host-filtering` |
| `Web.HttpsPolicy` | `https-redirection` |
| `Web.ErrorHandling`, `Web.ProblemDetails` | `error-handling`, `validation-invalid` |
| `Web.Rewrite` | `rewrite` |
| `Web.CookiePolicy` | `cookie-policy` |
| `Web.Compression` | `compression-gzip`, `compression-brotli`, `request-decompression` |
| `Web.StaticFiles` | `static-file` |
| `Web.Authentication`, `.Bearer`, `.Cookie` | `auth-bearer`, `auth-cookie` |
| `Web.Authorization` | `authorization` |
| `Web.Antiforgery` | `antiforgery` |
| `Web.Cors` | `cors-preflight`, `cors-request` |
| `Web.RequestTimeouts` | `request-timeouts` |
| `Web.RateLimiting` | `rate-limiting` |
| `Web.Forms` | `forms` |
| `Web.Caching` | `output-cache` |
| `Web.Sessions` | `sessions` |
| `Web.Health` | `health` |
| `Web.OpenApi` | `openapi` |
| `Web.Validation` | `validation-valid`, `validation-invalid` |
| `Web.WebSockets` | `websocket-echo` |
| All of the above, composed | `full-stack` |

Not covered yet, and why:

| Area | Reason |
| --- | --- |
| HTTP/2, HTTP/3, TLS, `UseHsts`, `Web.Authentication.Certificate` | Need a TLS listener and an h2/h3 load generator; the raw-socket client speaks HTTP/1.1 only. |
| `Web.Query` | The QUERY method has no ASP.NET Core counterpart to compare against. |
| `Web.Diagnostics` (HTTP logging) | The cost is the sink's. A fair comparison needs both frameworks writing the same access-log format to the same kind of sink. |
| `Web.Testing` | An in-memory test host, not a request path; a BenchmarkDotNet comparison against `TestServer` fits it better. |
| `Web.ApplicationModel` | Orchestration, not the request path. |
| NativeAOT | `run.ps1 -Aot` publishes both servers with `PublishAot=true`; not part of the nightly end-to-end measurements. |

## BenchmarkDotNet suite

`benchmarks/Assimalign.Cohesion.Web.Benchmarks` measures per-operation costs the end-to-end runner can only show
in aggregate, following the repository conventions ([CONVENTIONS.md](../../../CONVENTIONS.md)). Like every suite, it
measures the cohesion CI packages at the repository's `CohesionPackageVersion`:

| Folder | Measures | Baseline |
| --- | --- | --- |
| `Web` | Dispatch through 1 or 10 middleware to a status-setting terminal (4 cases) | ASP.NET Core application pipeline |
| `Web.Api` | Typed endpoint binding of an integer route value, integer query value and context (2 cases) | ASP.NET Core RequestDelegateFactory |
| `Web.Hosting` | Production application-feature stamping and aggregate consumption, 4 or 16 features (2 cases) | Standalone |
| `Web.Routing` | Endpoint selection, integer constraint and value capture, hit/miss over 1 or 100 routes (8 cases) | ASP.NET Core routing middleware |
| `Web.Serialization` | HTTP JSON read/write with identical source-generated web contracts, 32 or 4096 text characters (8 cases) | ASP.NET Core HTTP JSON extensions |
| `Web.Serialization` | Registry reader/writer lookup over 1 or 8 registrations (4 cases); exact, absent, suffix and rejected Accept negotiation (8 cases); negotiated no-op writes over six Vary shapes (6 cases) | Standalone public serialization seams |
| `Web.Authentication.Bearer` | One request authenticated with an HS256 bearer token (service, handler, principal) | ASP.NET Core JwtBearer |
| `Web.Authentication.Cookie` | One request authenticated with a data-protected cookie | ASP.NET Core cookies |
| `Web.Authorization` | A `RequireRole` policy evaluation | ASP.NET Core authorization |
| `Web.Antiforgery` | Anonymous data-protected token-pair issue and header-token validation (4 cases) | ASP.NET Core antiforgery |
| `Web.RateLimiting` | Global concurrency middleware admit/reject, one permit and no queue (4 cases) | ASP.NET Core rate-limiting middleware |
| `Web.Caching` | Warm 1 KiB output-cache hit and Authorization-header bypass (4 cases) | ASP.NET Core output-cache middleware |
| `Web.Validation` | Registered profile validation, valid input or 400 problem+json with errors (2 cases) | Standalone |
| `Web.ErrorHandling` | Exception-boundary success or thrown fault rendered as 500 problem+json (2 cases) | Standalone |
| `External` | Existing HTTP/1.1 round trip and process-startup series; explicit opt-in only | — |

The suite includes **18 serialization traversal cases across 3 classes**, alongside the existing
JSON and other component workloads. It reuses
`Fixtures/BenchmarkHttpContext.cs`; `PipelineFixture` builds the public production Web pipeline without starting
the host. Registered application features are stamped by that pipeline. The feature-stamping benchmark uses a
fresh, capacity-sized exchange; most component benchmarks reuse prepared exchanges and reset their output state.
Authentication and antiforgery use fresh exchanges where per-request handler caches would otherwise skip work.

The new counterparts all come from the existing `Microsoft.AspNetCore.App` framework reference, so no new
counterpart NuGet pins are needed. Paired cases use the same inputs and work: typed integer binding without
serialization; constrained GET endpoint selection without dispatch; JSON with identical generated contracts;
anonymous antiforgery with purpose-bound data protection; BCL concurrency limiters with identical settings;
and output-cache replay of the same response. Setup checks results, cache handler counts, limiter decisions,
and successful/invalid token validation before measurement. Cache population is setup-only: the fixture supplies
the server's successful-response-completion notification after warming Cohesion's cache.

Counterparts deliberately not used: direct System.Text.Json calls omit HTTP media-type work; ASP.NET Core's
default model validation and exception handler differ in validation profiles, error payload and re-execution;
its feature collection alone omits Cohesion's application-feature stamping contract. Request `no-cache` was
rejected as a paired cache-bypass scenario because these server-owned caches honor it differently; both bypass
an Authorization-bearing request by default.

Remaining in-process gaps: body/form/header binding and binding failures; route groups, host matching, link
generation and method mismatch; authentication failures, cookie issue/renewal, certificate authentication and
composite authorization policies; authenticated/form antiforgery and antiforgery endpoint middleware; queued or
named endpoint rate limits; cache fill, expiration, Vary variants, tag eviction and concurrent coalescing; nested
validation and status-code pages. No new cases cover Query, HostFiltering, HttpsPolicy, Rewrite, SecurityHeaders,
RequestTimeouts, ForwardedHeaders, Sessions, Forms, Health, CookiePolicy, Cors, WebSockets, StaticFiles,
Compression, Diagnostics, OpenApi, Testing, ApplicationModel or transport/server lifecycle. Many of these still
have end-to-end scenarios above; those do not substitute for isolated in-process measurements.

```powershell
../../../scripts/Invoke-Benchmarks.ps1 -Area Web
$env:COHESION_BENCHMARK_TARGETS = 'cohesion,aspnetcore,node'   # opt into existing external targets
```

Leave `COHESION_BENCHMARK_TARGETS` unset for the default, socket-free suite and CI dry verification. Existing
external benchmark names are preserved; they enumerate no targets until explicitly selected.

## Layout

```
benchmarks/resources/Web/
├── global.json                        SDK pins: .NET and the cohesion CI build (end-to-end harness)
├── shared/                            scenario names, payloads, keys: linked into every project
│   ├── BenchmarkShared.cs
│   └── wwwroot/hello-4k.txt
├── benchmarks/
│   ├── Assimalign.Cohesion.Web.Benchmarks/src             BenchmarkDotNet suite (see below)
│   ├── Assimalign.Cohesion.Web.Benchmarks.CohesionServer/src    Sdk.Web application, one pipeline per --scenario
│   ├── Assimalign.Cohesion.Web.Benchmarks.AspNetCoreServer/src  Microsoft.NET.Sdk.Web application, the same pipelines
│   └── Assimalign.Cohesion.Web.Benchmarks.Runner/src            load generator, server launcher, report writer
├── targets/                           external servers the suite compares against (catalog.json, node, python)
└── run.ps1                            end-to-end: publish both servers, measure under load
```

Adding a scenario takes three edits: a pipeline name in `shared/BenchmarkShared.cs`
(`ServerScenarios`), the pipeline in both servers' `Compose` (and `Register` when it needs
services), and a `LoadScenario` in `benchmarks/.../Runner/src/Scenarios/ScenarioCatalog.cs` with the
request and what a correct response looks like. Run `./run.ps1 -ValidateOnly -Scenarios <id>` and
compare the two response heads in the report before measuring.
