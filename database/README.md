# Database benchmarks

The suite targets .NET 10 and uses the pinned Cohesion source tree. Read [DESIGN](docs/DESIGN.md),
[METHODOLOGY](docs/METHODOLOGY.md) and the [engine catalog notes](engines/README.md) before comparing results.

## Build and quick start

From the repository root in PowerShell 7:

```powershell
$env:COHESION_ROOT = 'C:\cw\bench\coh-a'
dotnet build database/Assimalign.Cohesion.Benchmarks.Database.slnx -c Release
dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests/Assimalign.Cohesion.Benchmarks.Database.Tests.csproj -c Release
$cli = 'database/artifacts/bin/Assimalign.Cohesion.Benchmarks.Database.Cli/release/dbbench.dll'
dotnet $cli help
dotnet $cli list
dotnet $cli env --out database/artifacts/environment.json
./database/scripts/Build-BenchImages.ps1
```

The build does not import the original repository scaffold. Build Cohesion's build/Tasks in Release first.
If restore stalls, set DOTNET_SYSTEM_NET_DISABLEIPV6=1 for that command. Image publishing runs from database/
to use its .NET 10 global.json and explicitly preserves Release through cross-repository references.

This infrastructure revision registers all three models with **zero workload targets and operations**.
The SQL and Graph TCP readiness adapters work now. Dataset generation stops at DatasetGenerators.Get until
the separate dataset work lands. The later model sessions fill their ModelRegistration.Create entry points;
the CLI registry then needs no change. No empty registration produces fabricated performance results.

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

## Official run (after model and dataset registrations land)

```powershell
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model sql -Targets cohesion-sql-embedded,cohesion-sql-tcp,postgresql,sqlserver -Scale 10 -Suites latency,throughput,mixed
```

The skeleton generates only if the manifest is missing, runs one engine at a time, loads, verifies, executes
both mixed workloads and the requested suites, stops the engine in finally, compares digest files, and reports.
Database and dataset files stay in named Podman volumes. Results/host telemetry are an artifact bind mount.
For Cohesion TCP it first loads the embedded target's volume if necessary; TCP LoadAsync must measure only the
fixed wire sample in separate sample storage, never reload or alter the served baseline.

Load/verify/run all take the directory containing manifest.json, not the dataset root. Use *.verify.json names
for comparison discovery. Commands return 0 on success, 1 on invalid configuration/runtime failure, 2 on
verification or measured operation errors, and 130 on cancellation. A report still records support and error
evidence for blocked targets; it omits their performance comparisons.

dbbench report --in database/artifacts/results --out database/artifacts/report writes one Markdown file per
model and findings.json. Raw output belongs under artifacts/ and is not committed.

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

Temporary VersionOverride entries in the test project avoid editing the dataset session's owned central
test-package entries. Reconcile these with Directory.Packages.props when integrating that session.
