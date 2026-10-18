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
./scripts/Invoke-Benchmarks.ps1 -List                         # every suite
./scripts/Invoke-Benchmarks.ps1 -Area ObjectPool              # run one area (needs ../cohesion beside this repository)
./scripts/Invoke-Benchmarks.ps1 -Scope libraries -Profile dry # does everything build and run?
./scripts/Compare-Benchmarks.ps1 -Baseline <before> -Current <after>
```

CI ([benchmarks.yml](.github/workflows/benchmarks.yml)) checks every suite on pull requests, measures cohesion `main`
nightly, keeps time and allocation history on the `gh-pages` branch, and fails on a regression.
