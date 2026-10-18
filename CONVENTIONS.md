# Benchmark conventions

How benchmarks are laid out, written, run and tracked in this repository. The repository mirrors
[cohesion](https://github.com/assimalign/cohesion)'s high-level areas, not its project design.

## Layout

```
cohesion-benchmarks/
├── Directory.Build.props / .targets      repository defaults; package versions (one place)
├── global.json                           SDK pin (matches cohesion)
├── Assimalign.Cohesion.Benchmarks.slnx   every project
├── harness/
│   └── Assimalign.Cohesion.Benchmarks.Harness/src     entry point, run profiles, out-of-process toolkit
├── benchmarks/
│   ├── libraries/<Area>/                 one folder per cohesion libraries/<Area>
│   │   └── benchmarks/
│   │       └── Assimalign.Cohesion.<Area>.Benchmarks/src/
│   │           ├── Program.cs
│   │           └── <Package>/<Subject>Benchmarks.cs   one folder per cohesion package under test
│   └── resources/<Area>/                 one folder per cohesion resources/<Area>
│       ├── benchmarks/
│       │   ├── Assimalign.Cohesion.<Area>.Benchmarks/src/   the area's BenchmarkDotNet suite
│       │   └── Assimalign.Cohesion.<Area>.Benchmarks.<Role>/src/   end-to-end harness parts (servers, runners)
│       ├── targets/catalog.json          external applications the area compares against (optional)
│       ├── results/<date>-<what>/        checked-in results worth keeping (optional)
│       └── README.md
├── scripts/                              Invoke-Benchmarks, Compare-Benchmarks, ConvertTo-BenchmarkHistory
└── .github/workflows/benchmarks.yml      CI: validation on pull requests, history and regression alerts on main
```

- **One BenchmarkDotNet suite per area**, named `Assimalign.Cohesion.<Area>.Benchmarks`. Inside it, one folder per
  cohesion package under test, named after the package without the `Assimalign.Cohesion.` prefix
  (`Http.Connections/`, `Http.Cookies/`; the area's root package gets the area name: `ObjectPool/`).
- **Project names are unique across the repository** (BenchmarkDotNet locates the project by name).
- **`*.Benchmarks` is the marker.** A project whose name ends in `.Benchmarks` becomes an executable that references
  BenchmarkDotNet and the harness automatically (`Directory.Build.props`). Anything else (an end-to-end server, a
  load runner) is an ordinary project and is not run by `Invoke-Benchmarks` or CI's history job.
- An area with nothing to measure yet (`App` packages frameworks; `OpenTelemetry` is a placeholder) keeps a
  `README.md` saying so, so the gap is visible.

## Referencing Cohesion

Suites measure the **packages cohesion's CI publishes** to the Assimalign GitHub Packages feed, never a source
checkout. cohesion's `ci-packages.yml` publishes the full release set for every `main` push
(`10.0.0-preview.0.ci.<run>`) and every same-repository pull request commit (`10.0.0-preview.0.pr.<pr>.<run>`). A
suite names the packages it measures; they resolve at one version for the whole repository:

```xml
<CohesionReference Include="Assimalign.Cohesion.Http.Connections" />
<CohesionReference Include="Assimalign.Cohesion.Web.Authentication.Bearer" />
```

- **The pin.** `CohesionPackageVersion` in `Directory.Build.props` is the build every local run and every pull
  request check uses; the Web harness's `global.json` pins its Cohesion SDKs to the same build.
  `./scripts/Update-CohesionVersion.ps1` moves both to the newest `main` build (or `-Version <v>`). cohesion keeps
  only the newest 20 `main` builds, so a pin older than that stops restoring; move it at least that often.
- **Another build.** `-p:CohesionPackageVersion=<v>` (or `Invoke-Benchmarks.ps1 -CohesionVersion <v>`) measures any
  build still on the feed. `./scripts/Resolve-CohesionVersion.ps1` finds one: the newest `main` build by default,
  `-PullRequest <n>` for a cohesion pull request's newest build (an unmerged change: no checkout, no local pack; a
  pull request's builds are deleted when it closes), `-Version <v>` for the commit behind a version. It reads
  cohesion's public workflow history through the GitHub CLI.
- **Credentials.** GitHub Packages needs a token with `read:packages` even to read. `nuget.config` holds no
  credential; supply one through the environment, keyed by the source name:

  ```powershell
  gh auth refresh -h github.com -s read:packages      # once
  $env:NuGetPackageSourceCredentials_assimalign = "Username=<github-user>;Password=$(gh auth token)"
  ```

  CI uses the workflow token, or the `COHESION_PACKAGES_TOKEN` secret when one is set.
- Source-only test helpers (cohesion's test doubles, its test corpora) are not in the packages. A suite that needs
  one carries its own copy (`Web.Benchmarks/src/Fixtures/BenchmarkHttpContext.cs`; the OpenAPI corpus, with its
  `NOTICE.md`).

Suites run **in-process** (BenchmarkDotNet's `InProcessEmitToolchain`): the measured code is what you built, with
no rebuild of every suite in a generated child project, which keeps CI short. `--out-of-process` opts back into
BenchmarkDotNet's default toolchain (for example to compare GC modes in separate processes) and forwards
`CohesionPackageVersion` to the child build.

Comparison packages (the non-Cohesion counterpart) are referenced without a version; the version lives in
`Directory.Build.targets`.

End-to-end harnesses that run a real Cohesion application (the Web harness) use the Cohesion SDKs from the same feed,
pinned in their `global.json`; see their README.

## Writing a benchmark

```csharp
// Program.cs — the same in every suite
public static class Program
{
    public static int Main(string[] args) => BenchmarkProgram.Run(args, typeof(Program).Assembly);
}
```

- **Compare against the counterpart when there is one**, and make the counterpart the `Baseline`, so `Ratio` reads
  "Cohesion relative to the counterpart" (below 1 is better). Name the counterpart's method after it:
  `Parse` (Cohesion) and `Parse_SystemTextJson` (baseline). A class that compares several operations uses
  `[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]` with one `[BenchmarkCategory]` per operation.
- **No per-class jobs or exporters.** The profile decides the job; the harness adds the memory diagnoser, the GitHub
  markdown table and the full JSON report. Use job-mutator attributes (`[ExternalWorkload]`, `[InvocationCount]`)
  only when the workload needs them.
- **Stable names.** CI keys history on `Namespace.Class.Method(Params)`. Renaming a benchmark starts a new series;
  do it deliberately.
- **Keep setup out of the measurement**: build inputs in `[GlobalSetup]`, return results so the JIT cannot discard
  the work, and keep `[Params]` small (each combination is a CI series).
- **Measure what a request or caller pays.** Prefer the public API a consumer calls. Reach internals only when the
  question is about the internal itself, and say why in the class comment.

## Out-of-process targets

Not everything worth comparing is .NET code in the benchmark process: a Node or Go server, nginx in a container, a
database engine, a published Cohesion application, a CLI. An area lists those in `targets/catalog.json` (same
field names as the database suite's engine catalog):

```json
{
  "schemaVersion": 1,
  "targets": [
    { "id": "node", "kind": "process", "command": "node",
      "arguments": ["{catalogDir}/node-http/server.js", "{port}"],
      "ports": [{ "published": 0 }], "requires": ["node"],
      "readiness": { "kind": "http", "path": "/" }, "endpointTemplate": "http://{host}:{port}/" },
    { "id": "nginx", "kind": "container", "image": "docker.io/library/nginx:1.27-alpine",
      "ports": [{ "internal": 80, "published": 0 }],
      "readiness": { "kind": "http", "path": "/" }, "endpointTemplate": "http://{host}:{port}/" }
  ]
}
```

- `kind`: `process` (a long-running server started by the harness), `container` (docker or podman, whichever is
  running), or `command` (runs to completion; one run is the measured operation).
- `readiness`: `tcp` (default when there is a port), `http` (any status below 500), `log` (a regex on its output),
  `exec` (a command that exits 0), `none`.
- Placeholders: `{host}`, `{port}`, `{port:N}`, `{repoRoot}`, `{cohesionVersion}` (the measured cohesion package version),
  `{catalogDir}`, `{exe}`.
- `requires` (executables that must run, probed with `--version` or the given arguments) and `requiresFiles` (for
  example a published server) let a run skip a target this machine cannot start instead of failing.
  `COHESION_BENCHMARK_TARGETS=node,cohesion` restricts a run to those ids.

Benchmarks select targets with `[ParamsSource]` and start them in `[GlobalSetup]`:

```csharp
public IEnumerable<string> Targets => Catalog.AvailableIds(ExternalTargetKind.Process);

[ParamsSource(nameof(Targets))]
public string Target { get; set; } = "";

[GlobalSetup]  public void Start() => _target = Catalog.Start(Target);
[GlobalCleanup] public void Stop() => _target.Dispose();
[Benchmark]    public Task<int> Get() => /* one request to _target.Endpoint */;
```

Macro-sized work (starting a server, running a command) goes in a class marked `[ExternalWorkload]`, which switches
the profile's job to BenchmarkDotNet's monitoring strategy (no pilot, one invocation per iteration). BenchmarkDotNet
measures latency per operation; throughput under concurrent load belongs in an end-to-end harness (see the Web
area's runner).

## Running

```powershell
./scripts/Invoke-Benchmarks.ps1 -Area ObjectPool                         # one area, local profile
./scripts/Invoke-Benchmarks.ps1 -Scope libraries -Profile dry             # does everything build and run?
./scripts/Invoke-Benchmarks.ps1 -Area Http -Filter '*Cookie*' -CohesionVersion 10.0.0-preview.0.pr.1663.37
dotnet run -c Release --project benchmarks/libraries/ObjectPool/benchmarks/Assimalign.Cohesion.ObjectPool.Benchmarks/src -- --list flat
```

Profiles (`--profile`, `-Profile`, or `COHESION_BENCHMARK_PROFILE`): `local` (BenchmarkDotNet's default job, for
investigations), `ci` (1 launch, 5 warmups, 20 iterations: bounded and comparable run to run), `dry` (one
invocation: proves it runs). Results land in `.artifacts/results/<suite>/` (git-ignored).

To compare two cohesion builds (here `main` against a cohesion pull request) or two runs:

```powershell
$main = (./scripts/Resolve-CohesionVersion.ps1).Version
$change = (./scripts/Resolve-CohesionVersion.ps1 -PullRequest 1700).Version
./scripts/Invoke-Benchmarks.ps1 -Area Http -Profile ci -CohesionVersion $main -Output .artifacts\ab\before
./scripts/Invoke-Benchmarks.ps1 -Area Http -Profile ci -CohesionVersion $change -Output .artifacts\ab\after
./scripts/Compare-Benchmarks.ps1 -Baseline .artifacts\ab\before -Current .artifacts\ab\after
```

## CI, history and regressions

`.github/workflows/benchmarks.yml`:

- **Pull requests** in this repository run every suite under the `dry` profile against the pinned build: a
  benchmark that no longer builds or throws fails the check. No numbers are recorded.
- **Nightly, on demand, and on `repository_dispatch` from cohesion** (`cohesion-main`, optional
  `client_payload.version`) the `ci` profile runs every suite against the newest cohesion `main` build, or the build
  the dispatch names (`workflow_dispatch` input `cohesion-version`: a version, `pinned`, or `#<pr>`). Each result is
  converted to two series per benchmark, time (ns) and allocated bytes, and appended to the history on the
  `gh-pages` branch with [github-action-benchmark](https://github.com/benchmark-action/github-action-benchmark).
  Every data point records the cohesion package version and commit it measured. Only `main` builds are recorded; a
  measured pull request build appears in the run summary and artifacts.
- **Regressions**: compared with the previous run, a benchmark that got slower than the time threshold, or allocates
  more than the allocation threshold, comments on the commit and fails the run. Allocations are deterministic, so
  their threshold is tight; time on shared runners is noisy, so its threshold is loose. Both are workflow
  variables (`BENCHMARK_TIME_THRESHOLD`, `BENCHMARK_ALLOCATION_THRESHOLD`), as is the runner
  (`BENCHMARK_RUNNER`): point it at a dedicated self-hosted machine to tighten the time threshold.
- Charts: the repository's GitHub Pages site (Settings → Pages → deploy from branch `gh-pages`), under `history/`.
