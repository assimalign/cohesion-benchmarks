# cohesion-benchmarks

Benchmarks for [Cohesion](https://github.com/assimalign/cohesion), laid out by cohesion's areas.

| Where | What it measures |
| --- | --- |
| `benchmarks/libraries/<Area>` | A BenchmarkDotNet suite per library area, against the .NET counterpart where there is one |
| `benchmarks/resources/<Area>` | A BenchmarkDotNet suite per resource area |
| [benchmarks/resources/Web](benchmarks/resources/Web/README.md) | Also the end-to-end Web harness: Cohesion Web vs ASP.NET Core over HTTP, scenario by scenario, under load |
| [benchmarks/resources/Database](benchmarks/resources/Database/README.md) | Also the groundwork for the multi-engine comparison (PostgreSQL, SQL Server, MongoDB, RavenDB, Neo4j): deterministic datasets, the engine catalog, and the capability baseline. Not yet running |
| `harness/` | The shared conventions: entry point, run profiles, and the toolkit for benchmarking applications in other processes (servers, containers, CLIs, non-.NET) |

[CONVENTIONS.md](CONVENTIONS.md) is how a suite is laid out, written, run, compared and tracked.

```powershell
$env:NuGetPackageSourceCredentials_assimalign = "Username=<github-user>;Password=$(gh auth token)"  # read:packages

./scripts/Invoke-Benchmarks.ps1 -List                         # every suite
./scripts/Invoke-Benchmarks.ps1 -Area ObjectPool              # run one area against the pinned cohesion build
./scripts/Invoke-Benchmarks.ps1 -Scope libraries -Profile dry # does everything build and run?
./scripts/Invoke-Benchmarks.ps1 -Area Http -CohesionVersion (./scripts/Resolve-CohesionVersion.ps1 -PullRequest 1700).Version
./scripts/Compare-Benchmarks.ps1 -Baseline <before> -Current <after>
./scripts/Update-CohesionVersion.ps1                          # move the pin to the newest cohesion main build
```

The suites measure the Cohesion packages cohesion's CI publishes to the Assimalign GitHub Packages feed: a pinned
`main` build by default, or any `main` or cohesion pull request build still on the feed
([CONVENTIONS.md](CONVENTIONS.md#referencing-cohesion)). No cohesion checkout is needed.

CI ([benchmarks.yml](.github/workflows/benchmarks.yml)) checks every suite on pull requests, measures the newest
cohesion `main` build nightly, keeps time and allocation history on the `gh-pages` branch, and fails on a regression.

On `main` and on the nightly it measures each suite on **Linux, Windows and macOS**, plus a **NativeAOT** leg of
the cases that opt into it; pull requests measure Linux only. Each leg records its own history series and is
compared only against itself — see [Legs](CONVENTIONS.md#legs-operating-system-framework-and-runtime).

**[The dashboard](https://assimalign.github.io/cohesion-benchmarks/)** shows that history: every area's movement
since the previous run on one screen, a sidebar to each area, and a benchmark's own chart on demand. Its source is
[site/index.html](site/index.html) — one static page, no dependencies — published by
[site.yml](.github/workflows/site.yml). The raw per-benchmark charts stay at
[`/history/`](https://assimalign.github.io/cohesion-benchmarks/history/).
