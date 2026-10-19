# R5 readiness rehearsal

> **Historical record.** This describes the comparison run that was paused on 2026-10-16, measured against
> Cohesion commit `bf20be5eb` and the pre-convention layout. Two things have moved since: this repository now
> measures the Cohesion packages cohesion's CI publishes (CONVENTIONS.md) rather than a source checkout, and T1
> landed on PR #1168, which changed the API and made most of the operations below native rather than emulated or
> unsupported. Current state: [../README.md](../README.md). Current capabilities:
> [COHESION-CAPABILITIES.md](COHESION-CAPABILITIES.md).

This record covers the R5 changes from benchmark base `388199b`, using read-only Cohesion
`bf20be5eb452fa57363f412bdcff36b2dc2e7edd`. Commands ran on Windows ARM64 through PowerShell and Podman.
Raw evidence stays in ignored `database/artifacts/`; the reproducible launch instructions are in
[RUNBOOK.md](RUNBOOK.md). Results are diagnostic short passes at the official resource budget, not SF10 results.

**SQL and Documents completed with zero verification mismatches. Neo4j completed every Graph phase, but
Graph's all-green acceptance is blocked by four Cohesion verification self-errors at the pinned engine.**
All completed shared cases match. The separate 2 GiB OOM reproduction attributes the pressure to Cohesion's
path materialization. SF10 Graph also cannot fit the engine's in-memory identity directory in 6 GiB. These
are engine findings; queries, support declarations, deadlines and the read-only engine source were not changed
to conceal them. Do not treat this record as approval to publish an all-green full-scale comparison.

## Reproduction and evidence

The driver/server images use tag `r5`. Every session-created container and named volume uses `dbbench-` and
the suffix `-r5`. The shared SF10 dataset volume was never written. SF 0.1 was generated in the owned
`dbbench-datasets-r5` volume, mounted read-only for loading/verification/measurement.

```powershell
$env:COHESION_ROOT = 'C:\cw\bench\coh-sql'
$env:DOTNET_SYSTEM_NET_DISABLEIPV6 = '1'
$env:DBBENCH_DATA_SCRATCH = Join-Path $PWD 'database/artifacts/test-data-r5'
./database/scripts/Build-BenchImages.ps1 -Tag r5
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

The launcher uses 3 CPUs/6 GiB for each engine or embedded driver and 3 CPUs/3 GiB for network drivers, with
memory-swap equal to memory. All phases, including failed attempts, have timestamped logs and status receipts
under `artifacts/results/rehearsal-r5/<model>/`. `phases.jsonl` preserves attempt durations; `_history/` retains
replaced output outside report input. A failed verification or operation is never silently ranked.

An image rebuild overlapped the first embedded SQL read-latency attempt. That attempt is retained in history;
its replacement must be used for timing conclusions. Later builds ran during verification, outside timed
performance phases. Verification wall time includes that incidental load and is a planning observation.

## Portable manifest check

Windows and Linux independently generated Documents SF 0.0001 with one thread. Their complete manifest bytes
have the same SHA-256, `86e329d0d311c21bb3a9b42a40ca650b0645bee1f26c5ac40507a4e8a01c35a2`.
No newline normalization or copied replacement manifest was used. Evidence: `r5-canonical-windows.log`,
`r5-canonical-linux.log`, `r5-canonical-comparison.log`.

```powershell
dotnet database/artifacts/bin/Assimalign.Cohesion.Benchmarks.Database.Cli/release/dbbench.dll `
    generate --model documents --scale 0.0001 --threads 1 --out database/artifacts/canonical-r5
podman run --rm --name dbbench-generate-r5 --network none --cpus 3 --memory 3g --memory-swap 3g `
    -v dbbench-datasets-r5:/data localhost/dbbench-driver:r5 generate --model documents --scale 0.0001 --threads 1 --out /data
# Compare Get-FileHash of the Windows manifest with sha256sum of /data/documents/sf0.0001/manifest.json.
```

## Findings established during the rehearsal

Paths below refer to the pinned Cohesion source. The source was not patched.

- SQL joins scan the outer relation before applying the complete WHERE predicate:
  `resources/Database/Assimalign.Cohesion.Database.Sql/src/Internal/SqlPlanExecutor.Joins.cs:31-34,64-89`.
  `order-lines` therefore visits orders beyond the requested order. The TCP verification's 100 cases took
  82.281 seconds at SF 0.1. Increasing the data by 100 times can cross the point deadline even though the logical
  result remains small. This is an engine planning cost, not client result caching or parameter generation.
- SQL `NOT IN (SELECT ...)` compares an operand with the complete materialized candidate list:
  `resources/Database/Assimalign.Cohesion.Database.Sql/src/Internal/SqlExpressionEvaluator.cs:759-789`.
  The TCP `unshipped-orders` verification's 100 cases took 125.358 seconds. Both outer and inner cardinalities
  grow with scale, so a linear extrapolation is not a reliable bound for this operation.
- PostgreSQL's `above-country-average` plan is an outer customer sequential scan with an aggregate subplan
  containing another customer sequential scan. The observed plan cost was `19449943.17`, with 25,000 outer
  customers. Each execution performs about 625 million inner tuple visits at SF 0.1. The parameterless query
  is still executed for every fixed verification case; query results are not cached by the driver.
  Evidence: `r5-postgresql-correlated-plan.log` and PostgreSQL `pg_stat_user_tables.seq_scan/seq_tup_read`.
- Npgsql successfully connected using the catalog credentials but printed a missing `libgssapi_krb5.so.2`
  warning during its GSS probe. The driver image now installs `libgssapi-krb5-2`; no query, authentication
  policy, engine setting or timed region was changed to suppress the warning.

## Implementation choices

Pass order removes inverse SQL/Graph cleanup statements and their retained per-client mutation lists. A
disposable verification copy is necessary because verification itself writes. A pristine offline checkpoint
also lets TCP serve data loaded once by the embedded target after the embedded write passes finish. These
copies occur at target invocation boundaries, never between performance suites. Copy/start time is reported
outside operation timing. Documents retain their separate write fixture collections.

Shipment-history `seq` is BIGINT on every SQL target so persistent shipment writes can use the same disjoint
write-ID namespaces as other inserts. Dataset values and logical indexes are unchanged. The official mixed
workloads explicitly select the common supported operation subset across all targets, with 90/10 or 50/50
read/write ratios and descriptor proportions within each class. Unsupported single-operation entries remain
visible with their evidence.

The dated R1A execution record retains its original emulator references plus the removal note. Active source,
catalog and documentation references were removed. This preserves the requested history rather than editing
an old execution record to make the grep output shorter.

## Build and test ledger

All commands use Release. `-p:ShouldUnsetParentConfigurationAndPlatform=false` preserves that configuration
through the read-only cross-repository references. `--no-restore`/`--no-build` reuse the immediately preceding
successful build where indicated. Log names below are relative to `database/artifacts/`.

```powershell
dotnet build database/Assimalign.Cohesion.Benchmarks.Database.slnx -c Release -p:ShouldUnsetParentConfigurationAndPlatform=false
dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release --no-build
dotnet list database/src/Assimalign.Cohesion.Benchmarks.Database.Cli package --vulnerable --include-transitive
```

| Check / log | Result |
|---|---|
| Item 0 build, `r5-item0-build.log` | Passed, 0 errors; 12 pre-pin vulnerability warnings |
| Item 0 tests, filter `DocumentContractTests\|ReportTests`, `r5-item0-tests.log` | 9 passed |
| Item 1 builds, `r5-item1-build.log`, `r5-item1-build-final.log` | Both passed, 0 errors; 6 pre-pin warnings each |
| Item 1 tests, filter `GraphAdapterTests\|Sql.ModelTests\|DocumentContractTests`, `r5-item1-tests.log` | 22 passed |
| Item 1 final tests, same filter plus `Sql.SchemaTests`, `r5-item1-tests-final.log` | 29 passed |
| Item 2 build, `r5-item2-build.log` | Passed, 0 errors; 6 pre-pin warnings |
| Item 2 runner tests, `r5-item2-tests.log`, `r5-item2-tests-final.log` | 8 passed, then 11 passed after adding coverage |
| Item 3 build, `r5-item3-build.log` | Passed, 0 warnings, 0 errors |
| Item 3 full tests, `r5-item3-tests.log` | 111 passed, 1 existing opt-in dataset test skipped |
| Package audit, `r5-vulnerable.log` | CLI has no vulnerable packages including transitives |
| Item 4 build, `r5-item4-build.log` | Passed, 0 warnings, 0 errors |
| Report deduplication test, `r5-item4-report-tests.log` | 7 passed, 1 failed: the new fixture changed a manifest hash instead of a row digest |
| Corrected row-digest fixture, `r5-item4-report-tests-final.log` | 8 passed; 100 actual digest mismatches remain asserted |
| Item 4 full tests, `r5-item4-tests.log` | 112 passed, 1 existing opt-in dataset test skipped |
| Fatal-phase report test, `r5-item4-fatal-report-tests.log` | 9 report tests passed, including a fatal phase with no result JSON |
| Final Release build, `r5-final-build.log` | Passed, 0 warnings, 0 errors; 32.57 s |
| Final full tests, `r5-final-tests.log` | 113 passed, 1 existing opt-in skip, 0 failed; 114 total |
| Final CLI transitive audit, `r5-final-vulnerable.log` | No vulnerable packages |

Focused tests use `--filter 'FullyQualifiedName~X|FullyQualifiedName~Y'` with the names in the table. The existing
skip is `Generate_ScaleOne_ReportsActualSizesAndCounts`, gated by `DBBENCH_LONG_TESTS=1`; it was not weakened.
The failed new fixture was corrected to exercise the intended digest mismatch, not relaxed to accept incorrect
behavior. All PowerShell scripts were parsed and `git diff --check` was run before commits.

Image builds used `Build-BenchImages.ps1 -Tag r5`; logs are `r5-images-initial.log`, `r5-images-progress.log`,
`r5-images-native-dependency.log`, `r5-images-load-restart.log` and `r5-images-report-diagnostics.log`.
Item 4 images record benchmark commit `462f537` with the working-tree dirty flag, since this rehearsal validates
the changes before their item 4 commit. The unchanged Cohesion server image remained
`ba1d787db65be1feb895b2d7a334f877a67a7cc547e211e4bf21aefc0c5d98e0`.
The native-dependency fix was checked with:

```powershell
podman run --rm --name dbbench-native-check-r5 --network dbbench --cpus 3 --memory 3g --memory-swap 3g `
    localhost/dbbench-driver:r5 ping --target postgresql --suffix -r5
# PING postgresql: ready. (exit 0, no missing-library warning)
```

For memory attribution only, original sources were archived from `388199b` into ignored
`artifacts/r5-baseline/`, published in Release for `linux-arm64`, and built as `localhost/dbbench-driver:r5-baseline`.
This diagnostic image is separate from the current CLI and its vulnerability audit. Its workload, adapters and harness are unchanged. Only fatal-exception stack logging is added via
`DBBENCH_DIAGNOSTIC_STACKS=1`; this executes after failure, outside timing. The Cohesion source remains pinned. Build logs: `r5-baseline-build.log`,
`r5-baseline-image.log`, `r5-baseline-stack-build.log`, `r5-baseline-stack-image.log`. That baseline image was
built as a contingency, but no baseline workload was needed: the current combined read pass reproduced the OOM.

## Rehearsal outcomes and repaired defects

| Model | Completed verification cases by target | Pairwise matching cases | Comparison errors |
|---|---|---:|---:|
| SQL | Embedded 1900; TCP 1900; PostgreSQL 2400; SQL Server 2400 | 11900 | 0 |
| Documents | Embedded 1000; MongoDB 1100; RavenDB 1100 | 3100 | 0 |
| Graph | Embedded 802; TCP 800; Neo4j 1300 | 2402 | 4 self-errors; 0 observed digest differences |

Unsupported cases are excluded with their recorded evidence: SQL Cohesion has five unsupported operations,
Documents Cohesion one, Graph embedded two and Graph TCP four. The four Graph comparison errors are:

- Embedded `fof-recent-messages`, first case: `COHDBG004: The query exceeded 100000 path matches.`
- Embedded `friends-within-3-by-name`, after two cases: `COHDBG004: The query exceeded 1000000 candidate expansions.`
- Embedded and TCP `country-person-count`, first case: the same 1,000,000-candidate error.

The limits are in `resources/Database/Assimalign.Cohesion.Database.Graph/src/Internal/GraphPlanExecutor.cs:17,144,245-248`.
Candidate accounting precedes the relationship-type check at lines 155-156; WHERE evaluation follows materialized
bindings at lines 45-56. No incomplete result was hashed as a success. Embedded and TCP each have failed verify,
read-latency, read-throughput and both mixed phases; their write phases completed. Reports retain these failures.

The official script exposed and fixed the following harness/orchestration defects:

- A PowerShell thread job running Neo4j's import bridge has an empty `$PSScriptRoot`. Pass the resolved common
  script path explicitly, dot-source it inside the bridge's exception handler, and publish import failures to
  the waiting driver. The retry loaded, verified all 1300 cases and completed every phase. No importer setting
  or query was altered. Evidence: `r5-rehearsal-neo4j-retry.log`, `graph/neo4j/load/import.log` and its receipt.
- Neo4j restart and official engine readiness had fixed retry limits. They now report readiness progress while
  their container is alive; authentication/configuration failures still fail immediately. Load/import has no
  wall deadline. Operation deadlines remain 30/600 seconds.
- Successful PowerShell invocations inherited an earlier native `podman ... exists` exit code. An explicit
  success exit fixes this. Summary-only SQL and Documents invocations both returned 0 and compared with zero
  mismatches (`r5-sql-summary-exit-check.log`, `r5-documents-summary-exit-check.log`).
- A failed phase could leave no result JSON. Reports now include phase receipts and render failed-only runs;
  identical embedded verification copies are deduplicated without accepting conflicting evidence. Missing
  dataset setup was deliberately tested with `dbbench-missing-r5`: exit 2, a readable failed-phase report, and
  no accidental creation of that missing volume (`r5-missing-dataset-check.log`).
- Verification now prints per-operation start/end and case progress, so long parameterless SQL checks remain
  observable. The fixed 100-case contract was preserved.

RavenDB 7.1.12/build 71040 returned one HTTP 503 during the first eight-client warm-up of
`doc.products-by-category-price`: `RavenException: System.OutOfMemoryException`, with server frames
`Dictionary..ctor -> Raven.Server.Utils.LruDictionary..ctor -> QueryResultRetrieverBase.TryGetValue`.
That group's measured window had 3025 successes, zero measured errors and zero timeouts, but the warm-up
error correctly failed its phase. The server's largest sampled container usage was about 1.093 GB of 6.442 GB;
sampling does not exclude a short-lived spike. Its category query is limited to 50 rows. The returned stack
matches the [server retriever's cache allocation](https://github.com/ravendb/ravendb/blob/v7.1/src/Raven.Server/Documents/Queries/Results/QueryResultRetrieverBase.cs#L821)
and [LruDictionary constructor](https://github.com/ravendb/ravendb/blob/v7.1/src/Raven.Server/Utils/LruDictionary.cs#L13)
on the v7.1 source branch; that branch is supporting context, not a claim of a binary-exact source checkout.

Both RavenDB read phases were replayed from its pristine checkpoint with identical parameters, without a code,
query or engine-setting change. They completed without warm-up/measured errors, and Documents comparison stayed
at zero. The original failure is preserved under `_history/` and in `phases.jsonl`; the successful retry does
not prove the intermittent server defect fixed. Evidence: `r5-rehearsal-documents.log`,
`r5-rehearsal-raven-retry.log`, `r5-raven-query-retriever.cs` and `r5-raven-lru.cs`.

Actual retries and fault checks:

```powershell
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model sql @short -SkipLoad -Targets cohesion-sql-embedded `
    -Phases read-latency,compare,report
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model graph @short -Targets neo4j
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model documents @short -SkipLoad -Targets ravendb `
    -Phases read-latency,read-throughput,compare,report
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model sql @short -Phases compare,report
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model documents @short -Phases compare,report
./database/scripts/Invoke-DatabaseBenchmark.ps1 -Model sql -Scale 0.1 -DatasetVolume dbbench-missing-r5 `
    -Tag r5 -Suffix -r5 -RunId r5-missing-dataset -Phases report
```

The first SQL invocation and its quiet replacement finished all requested phases successfully but returned the
old inherited native exit 1 before that launcher fix. The first Documents invocation returned 2 for RavenDB's
warm-up failure. The original Graph invocation returned 2; Neo4j's retry also returned 2 solely because model
comparison still sees the four Cohesion errors. RavenDB's final replay returned 0. The intentional missing-data
probe returned 2. Exit status describes phases attempted by that invocation; reports also inspect retained phases.

## Graph memory attribution

Diagnostics use the same pinned engine and current adapter/harness, with SF0.01, a 1024 MiB buffer pool and
three CPUs. They are outside official results. The SF0.01 load at 2 GiB completed: full load 10.399 s,
samples 5.214 s, stored bytes 236,237,843. Combined throughput uses clients 1 and 8, one-second warm-up and
ten-second measurement, starting from a fresh offline copy. Counters are collected in a separate PID-sharing
helper, using `dotnet-counters` and `dotnet-gcdump` 10.0.750501; target memory limits are unchanged.

**The current combined throughput process reproduces the OOM before any write operation.** It passes the
first four read operations and the single-client friends-of-friends group, then aborts at eight clients.
The process exits 1 after 116.825 s; `OOMKilled=false`, no controller timeout. The failure stack begins:

```text
System.OutOfMemoryException
  GraphRecordCodec.Decode
  GraphStore.ReadAt
  GraphStore.GetIncidentAsync
  GraphPlanExecutor.<MatchAsync>...Expand
  GraphPlanExecutor.ExecuteAsync
  GraphDatabase.RunAsync
  CohesionBenchmarkClient.ExecuteAsync (line 46: awaiting the engine path result)
```

The counter CSV records peak GC committed bytes **1,594,789,888**, process working set **1,698,324,480**,
Gen2 heap size **1,440,268,504**, pinned-object heap **63,115,192**, and allocation rate **1,422,677,576 bytes/s**.
These maxima need not occur in the same sample. Committed GC memory approaches the documented default
[75% container heap hard limit](https://learn.microsoft.com/en-us/dotnet/core/runtime-config/garbage-collector#heap-hard-limit-percent)
(0.75 × 2,147,483,648 = 1,610,612,736 bytes). This is a managed allocation failure, not a kernel OOM kill.

A separate six-GiB, eight-client friends-of-friends run with the same 1024 MiB buffer pool produced a GC dump
at second five. Its report records **477,897,822 heap bytes / 6,468,721 objects**, including:

| Snapshot object | Count / size evidence |
|---|---:|
| Engine GraphPath | 202,959 |
| Engine GraphNode | 183,433 |
| Engine GraphNode[] and GraphRelationship[] | 202,968 each |
| Property/binding Dictionary<string,object> | 594,628 |
| ReadOnlyDictionary<string,object> | 412,484 |
| String | 2,399,088 in the small-object type group |
| Harness latency histogram arrays | 8 × 185,792 = 1,486,336 bytes |
| Object[] (includes adapter scalar rows) | 20,946 in the small-array type group |
| Engine identity-directory entries array | 15,573,576 bytes |

The source explains ownership: `GraphRecordCodec.cs:63-86,133-153` decodes new property dictionaries/strings;
`GraphStore.cs:180-193,311-319` materializes incident records; and `GraphPlanExecutor.cs:130-145,166-176`
copies binding dictionaries and node/relationship arrays while retaining selected paths before predicates.
The adapter's `CohesionBenchmarkClient.cs:46-47` waits for that engine result before projecting scalar rows.
Its per-operation rows and the harness histograms do not retain previous groups' graph results. The failure
stack, counter pressure and object profile attribute the reproduced memory exhaustion to engine path
materialization; they do not justify an adapter/histogram change or a smaller, semantically different query.

The first dump completed and its report parsed successfully. The tool warns that .NET 10 uses potentially
lossy event buffering; this snapshot is not an exact census of the later OOM peak. A second collector attempt
at second twenty exited 137 and left a partial dump, which was excluded. The six-GiB workload process itself
was not OOM-killed and completed with the expected COHDBG004 errors (exit 2). Profiling pauses make its timings
ineligible for performance comparisons. Evidence is in `r5-memory-current-2g/` and `r5-memory-fof-6g-dump/`:
`runtime.csv`, `console.log`, `container-inspect.json`, `diagnostic-status.json`, `heap-5.gcdump`, and
`heap-5-report.txt`. The second partial dump and collector log remain as diagnostic failure evidence.

Reproduction setup (after regenerating the owned dataset volume; all R5 volumes were removed at completion):

```powershell
. ./database/scripts/BenchEngine.Common.ps1
$sdk = 'mcr.microsoft.com/dotnet/sdk:10.0.300-noble-aot'
podman volume create --label "dbbench.owner=$script:Owner" dbbench-diagnostics-tools-r5
podman volume create --label "dbbench.owner=$script:Owner" dbbench-graph-diagnostic-pristine-r5
podman run --rm --name dbbench-diagnostic-install-r5 --label "dbbench.owner=$script:Owner" `
    --network dbbench --cpus 3 --memory 3g --memory-swap 3g -v dbbench-diagnostics-tools-r5:/tools `
    $sdk sh -c 'dotnet tool install dotnet-counters --tool-path /tools --version 10.0.750501 && dotnet tool install dotnet-gcdump --tool-path /tools --version 10.0.750501'
podman run --rm --name dbbench-generate-r5 --label "dbbench.owner=$script:Owner" --network none `
    --cpus 3 --memory 3g --memory-swap 3g -v dbbench-datasets-r5:/data localhost/dbbench-driver:r5 `
    generate --model graph --scale 0.01 --threads 3 --out /data
$loadOut = [IO.Path]::GetFullPath('database/artifacts/r5-memory-load')
New-Item -ItemType Directory -Force $loadOut | Out-Null
podman run --rm --name dbbench-graph-memory-load-r5 --label "dbbench.owner=$script:Owner" --network dbbench `
    --cpus 3 --memory 2g --memory-swap 2g --env DBBENCH_DIAGNOSTIC_STACKS=1 `
    -v dbbench-graph-diagnostic-pristine-r5:/engine-data -v dbbench-datasets-r5:/data:ro -v "${loadOut}:/results" `
    localhost/dbbench-driver:r5 load --model graph --target cohesion-graph-embedded --data /data/graph/sf0.01 `
    --buffer-pool-mb 1024 --out /results
./database/scripts/Invoke-GraphMemoryDiagnostic.ps1 -OutputDirectory database/artifacts/r5-memory-current-2g
./database/scripts/Invoke-GraphMemoryDiagnostic.ps1 -Memory 6g -Clients 8 -Operations graph.fof-recent-messages `
    -DurationSeconds 35 -DumpAtSeconds 5,20 -MaximumSeconds 180 -OutputDirectory database/artifacts/r5-memory-fof-6g-dump
$dumpOut = [IO.Path]::GetFullPath('database/artifacts/r5-memory-fof-6g-dump')
podman run --rm --name dbbench-graph-heapreport-r5 --label "dbbench.owner=$script:Owner" --network none `
    --cpus 3 --memory 6g --memory-swap 6g -v dbbench-diagnostics-tools-r5:/tools:ro -v "${dumpOut}:/evidence:ro" `
    $sdk /tools/dotnet-gcdump report /evidence/heap-5.gcdump
```

Use new empty output directories for another attempt; the diagnostic script refuses to overwrite evidence.
It removes its temporary working/IPC volumes and helpers, retaining the named source and tools volumes for
explicit cleanup. A controller success means evidence was captured; consult `diagnostic-status.json` for the
workload's exit. The original R4 baseline image was not needed to reproduce this failure.

## Graph SF10 capacity finding

The existing shared SF10 manifest was read through a read-only mount, without loading or modifying it. It has
42,311,285 nodes and 219,931,600 relationships, total **262,242,885** records in 125 CSV parts.
Fingerprint: `619620e6df54626ac279dedbc13293af2fe270a0653f43fc171b8cfbdc581e46`.
Evidence: `r5-sf10-graph-manifest.json` (an ignored evidence copy).

`resources/Database/Assimalign.Cohesion.Database.Graph.Storage/src/GraphStore.cs:26,35-44,361` retains a dictionary entry for every graph record, with key
`(byte Kind, ulong Id)` and value `(PageId, int SlotIndex, TransactionSequence Writer)`. PageId and Writer
each wrap eight bytes. Even a hypothetical perfectly packed representation needs **29 bytes per record**:
`262242885 × (1+8+8+4+8) = 7,605,043,665 bytes`, already greater than `6 × 1024^3 = 6,442,450,944`.
Real dictionary entries, alignment, buckets, managed-runtime overhead, page cache and the loader's ID map add
more. The SF0.01 heap dump independently shows the identity-directory entries array at 15,573,576 bytes.
This lower bound alone proves the pinned implementation cannot load/open the full SF10 Graph baseline under
the required budget. Raising only the traversal caps would not solve it. The owner needs an engine design
change (a paged/compact directory and bounded traversal materialization), followed by a new pinned rehearsal.

## SF 0.1 timings

Seconds below come from retained accepted phase receipts and load metrics. Full load includes finalization;
samples are separate. Phase load additionally includes setup, server/import coordination and checkpointing.
Target wall time is the sum of its current phase receipts, excluding model comparison/report and superseded
attempts. It is a reproducible planning observation, not a cross-engine performance score.

| Model / target | Full load | Samples | Load phase | Verify phase | Read passes | Write passes | Mixes | Target wall |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| sql / cohesion-sql-embedded | 21.16 | 0.78 | 26.65 | 577.04 | 371.88 | 97.35 | 70.52 | 1145.89 |
| sql / cohesion-sql-tcp | 0.00 | 3.40 | 12.45 | 448.87 | 324.93 | 68.95 | 59.55 | 925.33 |
| sql / postgresql | 3.49 | 0.00 | 15.72 | 3582.19 | 734.56 | 66.20 | 31.40 | 4435.97 |
| sql / sqlserver | 9.49 | 0.00 | 25.59 | 69.85 | 334.96 | 67.56 | 32.67 | 541.68 |
| documents / cohesion-documents-embedded | 5.35 | 1.58 | 12.36 | 43.93 | 134.82 | 91.55 | 38.41 | 323.37 |
| documents / mongodb | 2.93 | 0.00 | 18.69 | 22.13 | 135.50 | 88.28 | 32.78 | 306.68 |
| documents / ravendb | 9.27 | 0.00 | 24.91 | 38.18 | 145.65 | 93.01 | 33.20 | 346.23 |
| graph / cohesion-graph-embedded | 100.16 | 32.29 | 141.96 | 25.30 | 215.54 | 74.04 | 54.06 | 513.26 |
| graph / cohesion-graph-tcp | 0.00 | 21.70 | 32.50 | 41.07 | 149.06 | 67.48 | 49.44 | 359.11 |
| graph / neo4j | 14.53 | 12.13 | 99.18 | 124.77 | 183.84 | 66.48 | 32.17 | 527.88 |

TCP full load is zero by design: it serves the embedded-loaded checkpoint. Graph Cohesion phase durations
include failed operations and are not valid performance rankings. Neo4j import processed 424,385 nodes,
2,199,889 relationships and 3,354,487 properties; importer time was 9.029 s, host import metric 11.628 s,
reported peak import memory 1.036 GiB.

### Slowest successful Cohesion latency observations

These are measured p50 seconds, one measured analytic execution in the short pass. They are scale-planning
observations, not stable percentile estimates. A successful sample does not erase errors in other cases.

| Target | Operation | p50 seconds |
|---|---|---:|
| cohesion-sql-embedded | sql.a.unshipped-orders | 1.026539 |
| cohesion-sql-embedded | sql.order-lines | 0.847359 |
| cohesion-sql-embedded | sql.a.brand-ratings | 0.452038 |
| cohesion-sql-tcp | sql.a.unshipped-orders | 0.970049 |
| cohesion-sql-tcp | sql.order-lines | 0.841727 |
| cohesion-sql-tcp | sql.a.brand-ratings | 0.414188 |
| cohesion-documents-embedded | doc.orders-status-range | 0.365311 |
| cohesion-documents-embedded | doc.revenue-by-category-30d | 0.020084 |
| cohesion-documents-embedded | doc.products-multiget | 0.013711 |
| cohesion-graph-embedded | graph.friends-within-3-by-name | 0.536391 |
| cohesion-graph-embedded | graph.tag-cooccurrence | 0.225519 |
| cohesion-graph-embedded | graph.friends-recent-posts | 0.021215 |
| cohesion-graph-tcp | graph.tag-cooccurrence | 0.191245 |
| cohesion-graph-tcp | graph.friends-recent-posts | 0.029471 |
| cohesion-graph-tcp | graph.person-recent-messages | 0.003655 |

Embedded Graph additionally failed `fof-recent-messages` in 0.875312 s and `country-person-count` in
1.352135 s for the measured analytic execution. These failures have no successful latency sample.

## SF10 wall-time planning

These are **conditional attempt-time scenarios**, not a claim that SF10 will verify or fit memory. Scale grows
by 10 / 0.1 = 100. Only SF 0.1 was measured. There is no empirical SF10 upper bound; paging, compaction,
contention and cancellation latency can exceed these scenarios. Keep engine timeouts and phase logs enabled.

For each target, the calculation is `T = L + V + P + H` (seconds):

- `L = 100 × full-load seconds + sample seconds`. Wire/calibration samples are capped, so held constant.
- Start with each operation's observed single-client p50 `t`. Assume 2× for indexed/bounded work, 100×
  for scans/analytics, and 10,000× for Cohesion SQL NOT IN and PostgreSQL correlated AVG (both nested scans).
  Cohesion SQL order-lines, update-customer and ship-order use 100×. Documents status/prefix/category/revenue
  reads use 100×; ID/multiget/customer-recent and writes use 2×. Graph country count/shortest path use 100×,
  other bounded queries use 2×. These growth assumptions are explicit extrapolations, not measurements.
- `V = sum(100 × t10)`, or one deadline if the first case is predicted to time out. Verification still
  executes every case until an error; it never caches the parameterless queries.
- Point latency: `(ceil(10/t10) + min(5000, ceil(30/t10))) × t10`; analytics: `4 × t10`. A predicted
  first timeout costs one warm-up plus one measured deadline. Throughput uses 1,8,32 clients, with a
  conservative CPU-contention scenario `tc = t10 × max(1, clients/3)` and `(ceil(10/tc)+ceil(30/tc))×tc`,
  or two deadlines. `P` sums these and the four mixed groups. Each mix uses 40 seconds plus up to two
  largest in-flight operation times in the common subset, or two deadlines if predicted to time out.
- Observed residual `H0 = target wall − load metrics − verification execution − suite wall`. The table
  brackets `H` from `H0` to `100×H0` because it mixes fixed startup with data-size-dependent copy/open
  work. This deliberately broad bracket is not a statistical confidence interval or guaranteed bound.

| Model / target | L | V | P | H scenario | Total hours |
|---|---:|---:|---:|---:|---:|
| sql / cohesion-sql-embedded | 2117.28 | 13191.40 | 10260.57 | 76.23–7622.74 | 7.12–9.22 |
| sql / cohesion-sql-tcp | 3.40 | 10564.86 | 10072.38 | 48.60–4859.72 | 5.75–7.08 |
| sql / postgresql | 348.61 | 2616.55 | 8336.67 | 60.49–6049.36 | 3.16–4.82 |
| sql / sqlserver | 948.78 | 5565.24 | 4506.56 | 60.53–6052.63 | 3.08–4.74 |
| documents / cohesion-documents-embedded | 536.87 | 266.73 | 1884.04 | 22.76–2275.87 | 0.75–1.38 |
| documents / mongodb | 293.40 | 61.25 | 1758.14 | 56.71–5670.75 | 0.60–2.16 |
| documents / ravendb | 926.74 | 583.64 | 2049.38 | 62.25–6225.03 | 1.01–2.72 |
| graph / neo4j | 1465.36 | 8500.16 | 7178.17 | 164.41–16441.41 | 4.81–9.33 |
| graph / cohesion-graph-embedded | 10048.14 conditional | blocked | blocked | unknown | cannot finish within 6 GiB |
| graph / cohesion-graph-tcp | 21.70 conditional | blocked | blocked | unknown | cannot finish within 6 GiB |

Worked examples:

- Embedded SQL load: `21.1649951 × 100 + 0.7802604 = 2117.2797704 s`. Total lower scenario:
  `2117.28 + 13191.40 + 10260.57 + 76.23 = 25645.48 s = 7.12 h`. Upper residual scenario: 9.22 h.
- PostgreSQL above-country-average: `31.4115 × 10000 = 314115 s` predicted per execution, exceeding
  the 600 s deadline. One verification timeout (600), analytics warm-up + first measured timeout (1200),
  and three throughput groups (3 × 1200) already consume about 5400 s = 90 min and fail verification.
- Cohesion order-lines: `0.847359 × 100 = 84.7359 s`, beyond its 30 s point deadline. Thus the SQL
  attempt-time totals must not be interpreted as forecasts of a successful verified SF10 pass.
- Embedded Graph load alone would be `100.1585178 × 100 + 32.2919397 = 10048.1437197 s = 2.79 h`
  **if it fit**. It cannot fit the pinned engine's identity directory under the required budget (see the
  capacity finding). TCP has no second full load and only a 21.703 s sample, but cannot serve a baseline
  that the embedded target cannot load/open. No finite successful full-pass estimate is defensible.

Planning totals from the numeric scenarios: SQL about 19.11–28.64 hours, Documents about 2.36–6.26 hours,
Neo4j Graph about 4.81–9.33 hours. Graph Cohesion requires an engine capacity change before an SF10 schedule
can be accepted. These totals exclude retries, image builds, generation and final report rendering.

## Completion, deviations and owner actions

All ten targets attempted every official-script phase at SF 0.1 and all three model reports were produced.
SQL and Documents have clean retained phase sets and zero comparison errors. Graph has four genuine engine
self-errors, so the brief's all-green acceptance condition is **not met**. Failed Graph operations were not
relabelled unsupported, and successful partial cases were not presented as complete verification.

The R1A historical emulator references were retained, as explicitly requested for that dated execution record;
therefore the literal grep has its original two historical matches as well as the new history note and DESIGN
decision. There are no active target/package/catalog/script references. The memory reproduction additionally
used SF0.01 at 2 GiB and a separate profiled 6 GiB run; those are diagnostics, not official SF0.1 results.
The incidental SQL image-build overlap was rerun quietly, and RavenDB's failed read phase was retried without
a claimed engine fix. Original attempts remain in the ledger/history.

The owner must resolve Cohesion Graph's directory capacity and traversal/materialization design, select a new
engine pin, and rehearse again before scheduling a valid SF10 Graph comparison. Raising memory or silently
dropping failing operations would change the requested experiment. SQL's nested scans and point scans are
likely to hit the existing SF10 deadlines; budget the conditional failed-attempt times above. RavenDB's
intermittent server allocation failure remains a follow-up for a server-focused reproduction.

Every owned `dbbench-*-r5` container and volume, including datasets, checkpoints, diagnostic tools and source
stores, was removed after evidence capture. `r5-cleanup.log` records ownership-checked removals and an empty
remaining-r5 inventory. The pre-existing `dbbench-datasets` volume, images, shared network and unrelated stopped
containers/anonymous volumes remain. The Cohesion source checkout is clean at the original pin. Raw ignored
artifacts remain locally for review; no generated data, results, dumps or build output is committed.

Final validation also parsed every PowerShell script and passed `git diff --check`. No test was removed or
weakened. The only skipped test is the pre-existing long SF1 dataset test. No pushes or external repository
writes were made. Rebuild images from the committed tree for a subsequent official pass.
