# Web benchmarks: Cohesion Web vs ASP.NET Core

End-to-end HTTP benchmarks of the Cohesion Web resource (`resources/Web` in
[cohesion](https://github.com/assimalign/cohesion)) against ASP.NET Core, the framework with the
same runtime, the same JIT and a counterpart for almost every Web package. Two server applications
implement the same scenarios, one per framework; a runner starts one server process per scenario,
checks that it answers correctly, and measures it under load with a raw-socket HTTP/1.1 client.

Latest results: [results/2026-10-17-win-arm64/results.md](results/2026-10-17-win-arm64/results.md).

## Latest results (2026-10-17, cohesion `74976cdc5`)

Surface Laptop 7 (Snapdragon X Elite, 12 cores), Windows 11 on AC power, loopback, server on 4
cores and load generator on 8, 64 connections, 3 iterations of 10 s each (medians), JIT, Server GC
forced on both. All 222 runs passed validation with zero socket errors. Iteration-to-iteration
spread was up to about 14%, so treat ratio differences under 0.1 as noise.

| | Throughput, Cohesion ÷ ASP.NET Core (geometric mean) |
| --- | ---: |
| Core request path (10 scenarios) | 0.60× |
| Web packages (27 scenarios) | 0.78× |
| All 37 | 0.72× (efficiency per CPU-second: 0.73×) |

What stands out:

- **Cohesion is behind in 35 of 37 scenarios.** On the plain request path it reaches 0.75–0.82× of
  Kestrel (plaintext 102k vs 124k req/s, JSON 93k vs 125k), with p99 latency about 1.5× higher.
  The servers ran at the same CPU utilization (90–95%), so this is cost per request, not pacing.
- **HTTP/1.1 pipelining: 0.24×** (349k vs 1.46M req/s at depth 16). The gap is four times the
  non-pipelined one, so it sits in the HTTP/1.1 connection layer, not the pipeline.
- **Large responses: 0.38×.** A 64 KiB body written in one call: 30k vs 78k req/s, with the
  Cohesion server at only 77–80% CPU, p99 of 14–19 ms and a peak working set of 385–452 MB
  (ASP.NET Core: 112–116 MB). It looks like the body is buffered per request rather than written
  through, but that is not yet confirmed.
- **Authentication is about half as fast:** JWT bearer 0.50×, bearer plus role policy 0.51×,
  cookie 0.41×. The per-request validation or decryption work is roughly twice ASP.NET Core's.
- **Router scale:** 0.77× with one route but 0.61× with 100 routes. Matching cost grows with the
  route table in Cohesion; it doesn't in ASP.NET Core.
- **Where Cohesion wins or ties:** OpenAPI document serving is **21.8×** (Cohesion builds the
  document once and serves it with an ETag; ASP.NET Core regenerates it on every request).
  WebSocket echo is at parity (1.00×).
- **Startup is at parity** (about 600–700 ms to first response for both, JIT). The working set is
  higher for Cohesion on minimal pipelines (78 vs 47 MB for plaintext) and lower once ASP.NET Core
  loads its feature services (77 vs 94–100 MB for the auth, antiforgery and session scenarios).
- **Defaults worth knowing:** `Sdk.Web` does not enable Server GC (ASP.NET Core's Web SDK does);
  the benchmark forces it on for both. The first launch of a freshly published Cohesion app took
  about 7.4 s against 1.1 s for ASP.NET Core; later launches were about 600 ms for both. That points
  at first-load scanning of the 93 unsigned Assimalign assemblies, which is not yet verified.

## Running it

Prerequisites:

- .NET SDK 10.0.300 or later.
- The `cohesion` repository checked out beside `cohesion-benchmarks`, with its local feed packed.
  The Cohesion server restores `Assimalign.Cohesion.*` from `../../../../cohesion/_out/packages`
  (`nuget.config`) at `10.0.0-preview.1.local` (`global.json`):

  ```powershell
  pwsh ../../../../cohesion/installer/scripts/Install-Local.ps1 -Configuration Release
  ```

  Re-run it whenever the cohesion commit under test changes. The full pack is needed (not
  `-SkipLibraries`): `Web.OpenApi` ships NuGet-only, outside the `App.Web` framework.

Then, from this folder:

```powershell
./run.ps1                                            # every scenario, 1 iteration (~25 min)
./run.ps1 -Iterations 3 -Label "my machine"          # what the checked-in results use (~70 min)
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
| NativeAOT | `run.ps1 -Aot` publishes both servers with `PublishAot=true`; not part of the checked-in results yet. |

## BenchmarkDotNet suite

`benchmarks/Assimalign.Cohesion.Web.Benchmarks` measures per-operation costs the end-to-end runner can only show
in aggregate, following the repository conventions ([CONVENTIONS.md](../../../CONVENTIONS.md)). It builds Cohesion
from source (`CohesionRoot`), not from the packed feed:

| Folder | Measures | Baseline |
| --- | --- | --- |
| `Web.Authentication.Bearer` | One request authenticated with an HS256 bearer token (service, handler, principal) | ASP.NET Core JwtBearer |
| `Web.Authentication.Cookie` | One request authenticated with a data-protected cookie | ASP.NET Core cookies |
| `Web.Authorization` | A `RequireRole` policy evaluation | ASP.NET Core authorization |
| `External` | One HTTP/1.1 round trip, and launch-to-ready time, against servers in other processes: the published Cohesion and ASP.NET Core servers (when `run.ps1` has published them), Node.js, Python and nginx (container) | — |

```powershell
../../../scripts/Invoke-Benchmarks.ps1 -Area Web
$env:COHESION_BENCHMARK_TARGETS = 'cohesion,aspnetcore,node'   # restrict the external targets
```

[results/2026-10-18-auth-triage](results/2026-10-18-auth-triage/triage-report.md) is the investigation behind
the authentication rows of the 2026-10-17 results: the HTTP/1.1 request head was read one octet at a time.

## Layout

```
benchmarks/resources/Web/
├── global.json, nuget.config          SDK pins and the local Cohesion feed (end-to-end harness)
├── shared/                            scenario names, payloads, keys: linked into every project
│   ├── BenchmarkShared.cs
│   └── wwwroot/hello-4k.txt
├── benchmarks/
│   ├── Assimalign.Cohesion.Web.Benchmarks/src             BenchmarkDotNet suite (see below)
│   ├── Assimalign.Cohesion.Web.Benchmarks.CohesionServer/src    Sdk.Web application, one pipeline per --scenario
│   ├── Assimalign.Cohesion.Web.Benchmarks.AspNetCoreServer/src  Microsoft.NET.Sdk.Web application, the same pipelines
│   └── Assimalign.Cohesion.Web.Benchmarks.Runner/src            load generator, server launcher, report writer
├── targets/                           external servers the suite compares against (catalog.json, node, python)
├── run.ps1                            end-to-end: publish both servers, measure under load
└── results/<date>-<what>/             checked-in results and investigations
```

Adding a scenario takes three edits: a pipeline name in `shared/BenchmarkShared.cs`
(`ServerScenarios`), the pipeline in both servers' `Compose` (and `Register` when it needs
services), and a `LoadScenario` in `benchmarks/.../Runner/src/Scenarios/ScenarioCatalog.cs` with the
request and what a correct response looks like. Run `./run.ps1 -ValidateOnly -Scenarios <id>` and
compare the two response heads in the report before measuring.
