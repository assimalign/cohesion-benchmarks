# Database benchmarks

> **Paused before the full-scale runs (2026-10-16).** No SF10 benchmark has been run, and no number in this tree
> is a result. Read **[STATUS](docs/STATUS.md)** first: it records what is built and verified, why the runs are on
> hold (the Cohesion pin is 52 commits stale and the compiled-schema format moved to v6), and what has to be true
> before they start.

The suite targets .NET 10 and uses the pinned Cohesion source tree. Read [DESIGN](docs/DESIGN.md),
[METHODOLOGY](docs/METHODOLOGY.md) and the [engine catalog notes](engines/README.md) before comparing results.

## Build and quick start

From the repository root in PowerShell 7:

```powershell
$env:COHESION_ROOT = 'C:\cw\bench\coh-sql'
$env:DBBENCH_DATA_SCRATCH = Join-Path $PWD 'database/artifacts/test-data'
dotnet build database/Assimalign.Cohesion.Benchmarks.Database.slnx -c Release -p:ShouldUnsetParentConfigurationAndPlatform=false
dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests/Assimalign.Cohesion.Benchmarks.Database.Tests.csproj -c Release --no-build
$cli = 'database/artifacts/bin/Assimalign.Cohesion.Benchmarks.Database.Cli/release/dbbench.dll'
dotnet $cli help
dotnet $cli list
dotnet $cli env --out database/artifacts/environment.json
./database/scripts/Build-BenchImages.ps1
```

The build does not import the original repository scaffold. Build Cohesion's build/Tasks in Release first.
If restore stalls, set DOTNET_SYSTEM_NET_DISABLEIPV6=1 for that command. Image publishing runs from database/
to use its .NET 10 global.json and explicitly preserves Release through cross-repository references.

SQL, Documents and Graph have deterministic datasets, concrete workloads and engine adapters. See
[SQL](docs/SQL.md), [Documents](docs/DOCUMENTS.md) and [Graph](docs/GRAPH.md) for contracts and engine findings.

## Development smoke

```powershell
./database/scripts/Start-BenchEngine.ps1 -Target cohesion-sql-tcp -Memory 1g -Suffix -r1a
dotnet $cli ping --target cohesion-sql-tcp --suffix -r1a --smoke
podman run --rm --name dbbench-driver-smoke-r1a --network dbbench --cpus 1 --memory 512m --memory-swap 512m localhost/dbbench-driver:dev ping --target cohesion-sql-tcp --suffix -r1a --smoke
./database/scripts/Stop-BenchEngine.ps1 -Target cohesion-sql-tcp -Suffix -r1a -RemoveData
```

Use cohesion-graph-tcp for the Graph handshake and trivial MATCH/RETURN. SQL readiness alone is a successful
authenticated ConnectAsync handshake: this dialect requires FROM. The optional --smoke creates a dedicated
dbbench_smoke table and checks its one row. All other engines use their catalog readiness probes.
The scripts publish only to Windows loopback. Suffixes isolate names and volumes, but published ports remain
fixed: do not run two copies of the same target simultaneously.

Smaller memory limits explicitly select a development cache profile. Those measurements are not official.
Stop/remove every smoke container and its data volume; keep the images and shared dbbench network.

## Official SF10 run

Read the [full runbook](docs/RUNBOOK.md) for resume, diagnostics and cleanup. The existing `dbbench-datasets`
volume holds `/data/<model>/sf10` and is mounted read-only.

The [R5 rehearsal record](docs/REHEARSAL-R5.md) contains load timings, SF10 planning arithmetic and current
blockers. SQL and Documents passed SF 0.1 verification. Pinned Cohesion Graph has four verification self-errors
and an SF10 identity-directory memory requirement above the 6 GiB budget; it needs an engine change.

Build images, then run models sequentially:

```powershell
./database/scripts/Build-BenchImages.ps1 -Tag readiness
$full = @{Scale=10; DatasetVolume='dbbench-datasets'; Tag='readiness'; Suffix='-full'; RunId='sf10-first'}
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model sql @full
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model documents @full
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model graph @full
```

Defaults: one repetition; point latency 10 s warm-up, then 30 s or 5,000 attempts; analytics 1 warm-up plus
3 measured executions; throughput clients 1,8,32 for 30 s after 10 s warm-up; both mixed workloads at 8,32 clients.
Engines receive 3 CPUs/6 GiB with no extra swap, network drivers 3 CPUs/3 GiB, embedded drivers 3 CPUs/6 GiB.

Per target: load, verify, read latency, read throughput, write latency, write throughput, mixed, stop.
Verification uses a disposable copy. A pristine loaded checkpoint supplies each target invocation, including
Cohesion TCP from its embedded target; TCP measures a separate ingest sample and never reloads the baseline.
Writes persist after measurement. Documents retain their isolated `write_*` fixture collections. The launcher
finishes with cross-target verification and the model report, even after failed phases.

Results are under `database/artifacts/results/<run-id>/<model>/<target>/`; model logs, phase ledger and
`_model/report/` make progress and failures visible. A report can render failed evidence: inspect verification
and error tables before interpreting performance. Load has no wall deadline and emits progress; individual
operations retain 30 s/600 s deadlines. A first measured timeout ends that group without retrying its remainder.

```powershell
# Resume from this run's retained checkpoint, replacing selected phase output and preserving its history.
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model sql @full -SkipLoad -Targets postgresql `
    -Phases read-throughput,compare,report
```

`-Targets`, `-Phases`, `-Operations` and timing parameters support focused reruns. Use the same run ID/suffix
and dataset for `-SkipLoad`. The runbook contains exact SF 0.1 generation and rehearsal commands. Raw results,
generated datasets, dumps and build products are ignored and must not be committed.

## Extension contracts

Harness/IBenchmarkModel and IBenchmarkTarget are the extension seam. Targets declare every operation's
support, honor cancellation through response consumption and cleanup, report effective settings/version, and
implement prepare, ingest, finalization, storage measurement and client creation. Parameter generation and
statement construction happen in IParameterStream.Next before the runner timer. ExecuteAsync consumes the
complete logical result, including allowed client completion, and sends scalar rows to IVerificationSink only
during verification. Ordered operations must produce their declared total order with key tie-breakers.

ServedDataTarget names the embedded data owner; LoadMetric.IsSample distinguishes wire samples from full
load ratios. No target may silently substitute a query or omit a weighted unsupported operation from a mix.
See [METHODOLOGY](docs/METHODOLOGY.md) for canonical values, cancellation, mutation isolation and report gates.
