# R1a execution record (2026-10-15)

> **Historical record.** This describes the comparison run that was paused on 2026-10-16, measured against
> Cohesion commit `bf20be5eb` and the pre-convention layout. Two things have moved since: this repository now
> measures the Cohesion packages cohesion's CI publishes (CONVENTIONS.md) rather than a source checkout, and T1
> landed on PR #1168, which changed the API and made most of the operations below native rather than emulated or
> unsupported. Current state: [../README.md](../README.md). Current capabilities:
> [COHESION-CAPABILITIES.md](COHESION-CAPABILITIES.md).

Historical note: the Cosmos DB emulator target was removed later on 2026-10-15; the execution record below is unchanged.

Base: 24d372a. Cohesion source: bf20be5eb452fa57363f412bdcff36b2dc2e7edd.
The Cohesion worktree remained unchanged. No dataset-owned source or central test-package entry was edited.

## Builds and tests

- `dotnet build database/src/Assimalign.Cohesion.Benchmarks.Database.Harness -c Release`: passed after each of items 1–3.
- `dotnet build database/Assimalign.Cohesion.Benchmarks.Database.slnx -c Release`: final pass, zero warnings/errors.
  The first CLI build exposed a duplicate local variable name, corrected before the CLI commit.
- `dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests/Assimalign.Cohesion.Benchmarks.Database.Tests.csproj -c Release`:
  final **28 passed, 0 failed, 0 skipped**. Later repetitions used --no-build after a successful solution build.
  The first pass was 22 passed / 1 failed: a collection-expression test passed a null row instead of a NULL-valued
  column. Explicit row construction fixed the test input; no assertion was removed.
- `database/scripts/Build-BenchImages.ps1`: both self-contained linux-arm64 Release images built successfully.
  The final clean-commit rebuild and image/environment identifiers are included in the handoff message.
- Every PowerShell script parsed without errors. `git diff --check` passed.

## Container readiness

Each row ran `database/scripts/Start-BenchEngine.ps1 -Target <id> -Memory <memory> -Suffix -r1a`,
then `database/scripts/Stop-BenchEngine.ps1 -Target <id> -Suffix -r1a -RemoveData`.

| Target | Memory | Key output |
|---|---:|---|
| postgresql | 1g | READY dbbench-postgresql-r1a; postgresql://localhost:55432 |
| sqlserver | 2g | READY dbbench-sqlserver-r1a; tcp://localhost:51433 |
| mongodb | 1g | READY dbbench-mongodb-r1a; mongodb://localhost:57017 |
| ravendb | 1g | READY dbbench-ravendb-r1a; http://localhost:58080 |
| cosmos-emulator | 2g | READY dbbench-cosmos-emulator-r1a; https://localhost:58081 |
| neo4j | 1g | READY dbbench-neo4j-r1a; bolt://localhost:57687 |
| cohesion-sql-tcp | 1g | READY dbbench-cohesion-sql-r1a; tcp://localhost:55740 |
| cohesion-graph-tcp | 1g | READY dbbench-cohesion-graph-r1a; tcp://localhost:55741 |

SQL Edge was rerun after adding --image-volume ignore; readiness passed again. Its final stop returned
container exit **137** after `podman stop --time 40`. That shutdown needs investigation before official runs.
Both Cohesion servers exited **0** and printed `STOPPED: server drained and engine disposed.`
MongoDB, RavenDB, Cosmos and Neo4j also returned exit 0 on the recorded stops.

SQL Edge initially allocated three image-declared anonymous auxiliary volumes. They were removed with that
container. The launcher now ignores implicit image volumes; the database path remains an explicit named mount.
No pre-existing containers/volumes were stopped or removed.

## Cohesion client paths

With the corresponding server running, on Windows:

```powershell
$cli = 'database/artifacts/bin/Assimalign.Cohesion.Benchmarks.Database.Cli/release/dbbench.dll'
dotnet $cli ping --target cohesion-sql-tcp --suffix -r1a --smoke
dotnet $cli ping --target cohesion-sql-tcp --endpoint tcp://127.0.0.1:55740 --smoke
dotnet $cli ping --target cohesion-graph-tcp --suffix -r1a --smoke
```

And from the driver (both targets, one at a time):

```powershell
podman run --rm --name dbbench-driver-smoke-r1a --network dbbench --cpus 1 --memory 512m --memory-swap 512m localhost/dbbench-driver:dev ping --target cohesion-sql-tcp --suffix -r1a --smoke
podman run --rm --name dbbench-driver-smoke-r1a --network dbbench --cpus 1 --memory 512m --memory-swap 512m localhost/dbbench-driver:dev ping --target cohesion-graph-tcp --suffix -r1a --smoke
```

SQL on both paths: `SELECT id FROM dbbench_smoke WHERE id = 1 => 1`, then authenticated handshake succeeded.
Graph on both paths: authenticated handshake, `MATCH (n:DbbenchReadiness) RETURN n.id => 0 rows`.

SQL cannot use SELECT without FROM (Database.Sql.Language/docs/DIALECT.md:42). Graph schema indexes remain
a C# API operation (Database.Graph/src/GraphSchema.cs:215); the TCP data-sharing contract preserves that path.
Every server switch is mapped to its public option and source line in engines/README.md.

## Environment and contract boundary

```powershell
dotnet $cli list
dotnet $cli generate --model sql --scale 0.01 --out database/artifacts/generation-contract
dotnet $cli env --out database/artifacts/env-windows-final.json
podman run --rm --name dbbench-env-r1a --network dbbench --cpus 3 --memory 3g --memory-swap 3g localhost/dbbench-driver:dev env
```

List: three empty workload registrations and eleven catalog targets. Generate: options parsed and logged,
then the expected DatasetGenerators.Get NotImplementedException. No dataset generation was attempted beyond
the committed contract. No full model load/verify/run is claimed.

The driver snapshot reports Ubuntu ARM64, Cohesion built commit bf20be5eb452fa57363f412bdcff36b2dc2e7edd,
26 optimized Cohesion assemblies / 0 Debug assemblies, memory.max=3221225472, cpu.max=300000 100000.
The final image also records memory.swap.max and whether the benchmarks build tree was dirty.
A six-second host telemetry probe recorded container CPU/memory and host CPU; one observed host sample was
100%, reinforcing that these are readiness smokes, not performance measurements.

At completion, no dbbench smoke containers or -r1a data volumes remain. The driver/server images and shared
dbbench network remain. Unrelated cohesion-registry and cohesion-docs-control-plane objects were untouched.

## Integration notes

Test package VersionOverride values avoid the other session's owned Directory.Packages.props entries;
reconcile those references during integration. The model sessions must implement the registration contracts,
use ParameterStreamContext for disjoint write IDs, and prove mutation baseline isolation. Database-specific
SQL Edge recovery and MongoDB journal acknowledgement are declared policies for those adapters; only container
readiness was exercised here. The orchestration remains deliberately a model-independent skeleton.
