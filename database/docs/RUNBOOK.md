# Full-scale runbook

Run from the repository root in PowerShell 7. Use one engine at a time. Database files belong in named
Podman volumes; result JSON and logs belong in `database/artifacts/`. The catalog contains local development
credentials only. In Git Bash, prefix **every** Podman command with `MSYS_NO_PATHCONV=1`.

Read [the R5 rehearsal and capacity findings](REHEARSAL-R5.md) before scheduling SF10. SQL and Documents
completed SF 0.1 verification; pinned Cohesion Graph did not. Its traversal limits fail four checks, and its
SF10 identity directory alone exceeds 6 GiB. The SQL/Neo4j time forecasts are conditional on their operation
deadlines and are not promises of successful SF10 verification. A new engine pin needs a new rehearsal.

## Build the pinned sources

```powershell
$env:COHESION_ROOT = 'C:\cw\bench\coh-sql'
$env:DOTNET_SYSTEM_NET_DISABLEIPV6 = '1'
$env:DBBENCH_DATA_SCRATCH = Join-Path $PWD 'database/artifacts/test-data'
git log -1 --oneline
git -C $env:COHESION_ROOT log -1 --oneline
dotnet build database/Assimalign.Cohesion.Benchmarks.Database.slnx -c Release -p:ShouldUnsetParentConfigurationAndPlatform=false
dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release --no-build
dotnet list database/src/Assimalign.Cohesion.Benchmarks.Database.Cli package --vulnerable --include-transitive
./database/scripts/Build-BenchImages.ps1 -Tag readiness
```

Keep Cohesion at the intended source commit; the R5 baseline is `bf20be5eb`. Building may produce its normal
build outputs, but do not edit its source. The image build records source commits and preserves Release
through project references. Official load/run also checks the loaded assemblies' optimization attributes.

## Run the first SF10 pass

The existing `dbbench-datasets` volume must contain `/data/sql/sf10`, `/data/documents/sf10` and
`/data/graph/sf10`. The launcher mounts it read-only, validates the manifest and never generates or changes
it. Do not remove that shared volume. Use a unique suffix and run ID for your own engine volumes/results.

```powershell
$run = 'sf10-first'
$common = @{ Scale=10; DatasetVolume='dbbench-datasets'; Tag='readiness'; Suffix='-full'; RunId=$run }
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model sql @common
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model documents @common
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model graph @common
```

Each command attempts the whole model, records phase failures and returns 2 if any attempted phase failed. Inspect the
model log even when a later report succeeds. A successful report command means it rendered the evidence;
it does not mean that verification or every operation passed.

Default parameters implement the first full pass:

| Parameter | Default | Meaning |
|---|---:|---|
| `Repeat` | 1 | Measured repetitions |
| `WarmupSeconds` | 10 | Point/throughput/mixed warm-up per group |
| `DurationSeconds` | 30 | Point/throughput/mixed measured window |
| `PointIterations` | 5000 | Additional single-client point attempt limit |
| `AnalyticsWarmup`, `AnalyticsIterations` | 1, 3 | Single-client analytic executions |
| `Clients` | 1,8,32 | Throughput concurrency |
| `MixedClients` | 8,32 | Both read-heavy and balanced concurrency |
| `PointTimeoutSeconds`, `AnalyticsTimeoutSeconds` | 30,600 | Per-operation deadlines |

A group drains in-flight work after its window; actual elapsed time may exceed the window. The first measured
execution timing out ends that group without further iterations. Load has no wall-clock deadline and emits a
30-second heartbeat plus adapter progress. Official startup waits without a wall deadline while the engine
container remains running and prints readiness progress; an exited container fails immediately. Verification runs 100 fixed cases per supported operation and
stops that operation on its first error; short performance windows do not shorten verification.

All engine containers have 3 CPUs, 6 GiB memory and 6 GiB memory-swap (no extra swap). The network driver has
3 CPUs/3 GiB; an embedded driver has 3 CPUs/6 GiB and the data volume. Neo4j imports typed CSV derived from the
read-only dataset through a temporary named volume, with the same engine budget. No other engine runs then.

For each target the launcher loads, verifies, runs read latency, read throughput, write latency, write
throughput, then both mixed workloads, and stops the engine. Classes come from operation descriptors.
Mixed passes explicitly log the common supported subset across all model targets, preserving 90/10 or 50/50
read/write ratios. Single-operation passes retain unsupported declarations and their evidence.

Verification includes writes, so it uses a disposable offline copy. A pristine checkpoint of the loaded store
is retained. At the start of each target invocation, the working volume is copied from that checkpoint;
Cohesion TCP serves the embedded-loaded store and measures only its separate `tcp-ingest` sample. It never
reloads the full dataset. No inverse statements or volume copies run between measurement suites. Copies and
server startups are logged outside operation timing. These copies need disk capacity in addition to the dataset
and engine store (up to a checkpoint, working store and temporary verification store per target).

## Resume selected work

Keep the same run ID, suffix, image and dataset. Checkpoint receipts under `<model>/_state/` bind the source
volume to the manifest SHA-256. `SkipLoad` requires those receipts and volumes. It starts a fresh working copy,
so write IDs and read baselines are safe to reuse. Old selected phase output moves to `<run>/_history/`, outside
report input; untouched phases remain. Preserve the whole run directory and volumes until the pass is accepted.
The exit code covers this invocation's selected phases. The report also reads retained phases, so a successful
summary-only invocation can still render an earlier failed performance phase.

```powershell
# Retry verification and every measurement for one target, using its loaded checkpoint.
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model sql @common -SkipLoad -Targets cohesion-sql-tcp

# Retry only a failed read phase and refresh comparison/report.
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model sql @common -SkipLoad -Targets postgresql `
    -Phases read-throughput,compare,report

# Reload a failed target, then execute all its phases again.
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model documents @common -Targets mongodb

# Re-render evidence without starting any engine.
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model graph @common -Phases compare,report
```

`-Operations` accepts operation IDs for focused diagnostics. Apply the same selection when comparing targets;
do not present a partial retry as a full pass. A failed verification file allows diagnostic timing, but the
report blocks its target from performance rankings. Missing verification evidence prevents official timing.
If a process is killed, inspect its console/error log, stop only your owned container, and resume. Do not prune
shared resources or remove another worktree's ownership labels to force access.

## Read the results

Under `database/artifacts/results/<run-id>/<model>/`:

- `orchestration.log`: timestamped START/END lines, phase duration, commands and errors.
- `phases.jsonl`: append-only phase attempt ledger, including failed attempts.
- `<target>/<phase>/console.log` and `phase.status.json`: phase output and exit evidence.
- `<target>/<phase>/*-load.json` or result JSON: load/finalization metrics, latency histograms, attempts,
  warm-up failures, timeouts, throughput, observed limits, versions and provenance.
- `<target>/<target>.verify.json`: canonical cases and supported-operation errors.
- `_model/compare/mismatches.json`: pairwise digest mismatches and self-errors/incomplete verification.
- `_model/report/<model>.md` and `findings.json`: report and machine-readable findings.

Check the phase ledger, verification coverage and mismatch count before interpreting ratios. Unsupported is
different from an empty result. Inspect warm-up failures as well as measured errors. Budget columns are derived
from observed container limits; zero means unknown, never an assumed 6 GiB. Comparing unlike manifests, seeds,
budgets, source versions, settings or workload selections is invalid. Keep noisy runs as evidence and rerun
affected phases; do not subtract guessed interference.

## Short SF 0.1 rehearsal

Generate into your own dataset volume, then keep it read-only during all three model runs. For example, after
building the `r5` images (the names below are reserved for the R5 session):

```powershell
. ./database/scripts/BenchEngine.Common.ps1
podman volume create --label "dbbench.owner=$script:Owner" dbbench-datasets-r5
foreach ($model in @('sql','documents','graph')) {
    podman run --rm --name dbbench-generate-r5 --label "dbbench.owner=$script:Owner" --network none `
        --cpus 3 --memory 3g --memory-swap 3g -v dbbench-datasets-r5:/data localhost/dbbench-driver:r5 `
        generate --model $model --scale 0.1 --threads 3 --out /data
}
$short = @{Scale=0.1; DatasetVolume='dbbench-datasets-r5'; Tag='r5'; Suffix='-r5'; RunId='rehearsal-r5';
    WarmupSeconds=2; DurationSeconds=5; AnalyticsWarmup=1; AnalyticsIterations=1; Clients=@(1,8); MixedClients=@(1,8)}
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model sql @short
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model documents @short
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model graph @short
```

The rehearsal still uses the real engine and driver budgets. Diagnostics that attach profilers or reproduce
2 GiB failures must be kept separate from comparable performance results.

`Invoke-GraphMemoryDiagnostic.ps1` captures System.Runtime counters and optional GC dumps from a fresh copy
without changing the target budget. The [memory attribution section](REHEARSAL-R5.md#graph-memory-attribution)
has complete setup, load, collection and heap-report commands. Its controller exit means collection completed;
`diagnostic-status.json` contains the actual workload exit, OOM-kill flag and timeout flag. Use new empty result
directories for repeats. Partial/failed dumps must not be used as a heap census.

## Cleanup

The launcher stops engines and removes driver/import/verification helpers. Working volumes and pristine
checkpoints remain intentionally for resume. Once results are accepted, list resources with your exact suffix,
check each ownership label against `BenchEngine.Common.ps1`, then remove only those containers and volumes.
Use `Stop-BenchEngine.ps1 -Target <id> -Suffix <suffix> -RemoveData` for the working data volume; remove its
`-pristine<suffix>` checkpoint separately after ownership verification. Keep images and the shared `dbbench`
network. Never use global prune, `rm -a`, or remove `dbbench-datasets`.
