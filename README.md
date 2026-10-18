# cohesion-benchmarks

Benchmarks for [Cohesion](https://github.com/assimalign/cohesion), laid out by cohesion's areas.

| Where | What it measures |
| --- | --- |
| `benchmarks/libraries/<Area>` | A BenchmarkDotNet suite per library area, against the .NET counterpart where there is one |
| `benchmarks/resources/<Area>` | A BenchmarkDotNet suite per resource area |
| [benchmarks/resources/Web](benchmarks/resources/Web/README.md) | Also the end-to-end Web harness: Cohesion Web vs ASP.NET Core over HTTP, scenario by scenario, under load |
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
