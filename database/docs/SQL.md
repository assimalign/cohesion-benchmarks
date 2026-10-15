# SQL commerce benchmark

## R2 smoke execution, 2026-10-15

All four targets loaded, verified and ran at SF 0.01. PostgreSQL and SQL Server
passed 2,400 verification cases each; Cohesion embedded and TCP passed 1,900 each.
The other five Cohesion operations are explicitly unsupported. Comparison of all
shared cases returned **0 mismatches**. A second 1,900-case TCP verification after
the write passes also matched, checking restoration of the served baseline.

These are development smoke measurements, **not official performance results**:
one repetition, no warm-up, five latency samples per supported operation, and
10-second throughput windows. Other sessions were busy on the host; the report
records substantial host CPU noise. Every engine in this SQL task ran alone,
with 3 CPUs, 2 GiB RAM and memory-swap equal to memory. Windows hosted the three
network drivers; the embedded driver and engine shared one constrained container.
Only the embedded engine/driver shared its memory budget. Do not interpret the
smoke ratios as release performance rankings.

- Benchmark base: `87afd6d`; measurement revision: `0831c11` plus the working
  changes subsequently committed for verification. Raw provenance retains the
  dirty flag. Cohesion source and built assemblies: `bf20be5eb452fa57363f412bdcff36b2dc2e7edd`, Release.
- Dataset: seed `20261015`, 13 tables, 117,261 rows. Manifest SHA-256:
  `c3e86b08638d6ac8a41c6ae88892ebcc66cdbb1eaffd614fb81fd25a150e391a`.
- PostgreSQL: `17.11 (Debian 17.11-1.pgdg13+2)`; SQL Edge SQL engine: `15.00.2000`.
- PostgreSQL effective settings: `shared_buffers=512MB`, `work_mem=16MB`,
  `maintenance_work_mem=256MB`, `fsync=on`, `synchronous_commit=on`, both locales `C`.
- SQL Server: `Latin1_General_100_BIN2_UTF8`, `SIMPLE`, delayed durability `DISABLED`.
- Both Cohesion modes: synchronous durability, 1,073,741,824-byte buffer pool,
  explicit binary string columns.

### Load measurements

Times below sum the recorded table phases. Database/schema preparation and
selection-only concurrency trials are outside the full ingest-plus-index total.
Primary keys exist during ingest; all 14 secondary indexes are created afterward.
Every full load ingested the same 117,261 rows.

| Target | Ingest seconds | Index/statistics seconds | Full phase total seconds | Stored bytes at load observation |
|---|---:|---:|---:|---:|
| PostgreSQL | 0.510410 | 0.364881 | 0.875290 | 28,997,299 |
| SQL Server / Azure SQL Edge | 2.312022 | 0.531840 | 2.843862 | 83,886,080 |
| Cohesion embedded | 2.355471 | 0.589445 | 2.944916 | 53,986,681 |
| Cohesion TCP | served embedded data | served embedded indexes | no second full load | 40,665,164 |

Cohesion's load chooser inserted 10,000 orders in a scratch database: one session
took **0.756523 s (13,218 rows/s)**; four took **0.329602 s (30,340 rows/s)**.
It therefore used four sessions for the full load. The 1.086125-second selection
cost is reported as `concurrency-trial`, with `isSample=true`. The original CLI
printed 4.031 seconds including those trials; the full-load value is 2.944916.
The CLI now prints full and sample durations separately.

TCP measured its own public `SqlClient` INSERT path in `benchmark_tcp_ingest`,
without modifying `benchmark`. It caps each table at the first 100,000 rows;
SF 0.01 contains fewer, so the sample consumes all 10,000 orders and 34,832 items.

| TCP sample | Rows | Seconds | Rows/s |
|---|---:|---:|---:|
| orders | 10,000 | 0.183998 | 54,348 |
| order_items | 34,832 | 0.346370 | 100,563 |

These are `tcp-ingest`, `isSample=true`; they never enter full-load comparisons.
An earlier successful TCP sample (0.63155 s / 1.10180 s) lacked a storage metric.
It is retained in the original-record archive; the complete repeat above is the
report's sample. TCP storage observes the same baseline after embedded shutdown
and workload cleanup, so its file length need not equal the earlier embedded
load observation. No separate TCP copy or logical dataset was generated.

### Commands and outcomes

The following commands use PowerShell from the repository root. `$cli` is the
built Windows driver; `-Suffix -r2` names only this task's containers and volumes.
SQL Server and Cohesion TCP used explicit IPv4 endpoints to avoid this host's
black-holed IPv6 route. Development credentials come from the catalog.

```powershell
$env:COHESION_ROOT = 'C:\cw\bench\coh-sql'
$env:DOTNET_SYSTEM_NET_DISABLEIPV6 = '1'
$cli = 'database/artifacts/bin/Assimalign.Cohesion.Benchmarks.Database.Cli/release/dbbench.dll'
$data = 'database/artifacts/sql-r2-data/sql/sf0.01'
$results = 'database/artifacts/sql-r2-results'
dotnet $cli generate --model sql --scale 0.01 --seed 20261015 --threads 2 --out database/artifacts/sql-r2-data
./database/scripts/Build-BenchImages.ps1 -Tag r2
```

Generation and image builds passed. For `postgresql`, then `sqlserver`, the
following sequence completed before the next engine started. `$endpoint` was
empty for PostgreSQL and `@('--endpoint','tcp://127.0.0.1:51433')` for SQL Server.

```powershell
./database/scripts/Start-BenchEngine.ps1 -Target $target -Memory 2g -Suffix -r2
dotnet $cli load --model sql --target $target --suffix -r2 @endpoint --data $data --out $results
dotnet $cli verify --model sql --target $target --suffix -r2 @endpoint --data $data --out "$results/$target.verify.json"
dotnet $cli run --model sql --target $target --suffix -r2 @endpoint --data $data --suite latency --duration 10s --iterations 5 --warmup 0s --repeat 1 --verification "$results/$target.verify.json" --out $results
dotnet $cli run --model sql --target $target --suffix -r2 @endpoint --data $data --suite throughput --clients 1,8 --duration 10s --warmup 0s --repeat 1 --ops sql.customer-by-id,sql.tx.place-order,sql.tx.ship-order,sql.a.revenue-by-month --verification "$results/$target.verify.json" --out $results
./database/scripts/Stop-BenchEngine.ps1 -Target $target -Suffix -r2 -RemoveData
```

Latency exercised every supported operation. Throughput exercised a point read,
both write transactions and monthly aggregation, each with 1 and 8 clients for
10 seconds. All measured operations had zero errors and zero timeouts. PostgreSQL
and the fixed SQL Server container exited 0. SQL Server's signal-handling diagnosis
and the sole exit-137 reproduction happened **before** its benchmark load.

For embedded execution, the dataset was copied with a temporary `podman cp`
helper into `dbbench-sql-datasets-r2`; engine data lived in
`dbbench-cohesion-sql-data-r2`. Both volumes carried this worktree's `dbbench.owner`
label. The helper was removed. Each phase used this container command, replacing
`<phase arguments>` with load, verify, latency or throughput arguments above:

```powershell
podman run --rm --name dbbench-cohesion-sql-r2 --network dbbench --cpus 3 --memory 2g --memory-swap 2g --volume dbbench-cohesion-sql-data-r2:/engine-data --volume dbbench-sql-datasets-r2:/datasets --volume C:/cw/bench/b-sql/database/artifacts/sql-r2-results:/results localhost/dbbench-driver:r2 <phase arguments> --model sql --target cohesion-sql-embedded --data /datasets/sql/sf0.01 --storage /engine-data --buffer-pool-mb 1024
```

Output/verification paths were `/results` inside that container. Every embedded
phase exited 0 and removed its container. Only then did `CohesionServer` start,
mounting **the same engine-data volume** at `/data`; no data copy and no embedded
process ran alongside the server. The empty persisted TCP scratch database was
also created by the embedded builder and opened by the server's handshake.

```powershell
./database/scripts/Start-BenchEngine.ps1 -Target cohesion-sql-tcp -Memory 2g -Suffix -r2 -Tag r2
# Same load / verify / run commands as above, with:
# --target cohesion-sql-tcp --endpoint tcp://127.0.0.1:55740 --buffer-pool-mb 1024
dotnet $cli verify --model sql --target cohesion-sql-tcp --suffix -r2 --endpoint tcp://127.0.0.1:55740 --buffer-pool-mb 1024 --data $data --out "$results/cohesion-sql-tcp-postrun.verify.json"
dotnet $cli verify --compare $results --out database/artifacts/sql-r2-compare.json
./database/scripts/Stop-BenchEngine.ps1 -Target cohesion-sql-tcp -Suffix -r2 -RemoveData
. ./database/scripts/BenchEngine.Common.ps1
Assert-BenchOwner dbbench-sql-datasets-r2 -Volume
podman volume rm dbbench-sql-datasets-r2
dotnet $cli report --in $results --out database/artifacts/sql-r2-report
```

Key output: `VERIFY COMPARE: 0 mismatches.`; `STOPPED dbbench-cohesion-sql-r2
exit=0`; `STOPPED: server drained and engine disposed.`; report rendered `sql.md`
and `findings.json`. Its 63 findings comprise 10 unsupported, 4 emulated, 48
performance-ratio observations and 1 load-ratio observation. The latter 49 are
noisy smoke observations, not confirmed performance regressions. All task-owned
containers and volumes were removed; pulled/built images were retained.

Raw JSON, verification digests and logs remain under ignored `database/artifacts/`.
The report is `sql-r2-report/sql.md`; raw runs are in `sql-r2-results/`.
The foundation initially stamped 6 GiB regardless of observed limits. Nine early
records' `budget` fields were corrected from their captured Podman/cgroup evidence
to 3 CPUs / 2 GiB; all original bytes are retained in `sql-r2-original-metadata/`.
Windows driver memory is 0 (unbounded/unobserved), embedded driver memory 2 GiB.
No timing, sample, verification digest, environment evidence or result row changed.
The CLI now records actual observed budgets and regression tests cover both paths.

### Slow-path observations

These five-sample medians are in milliseconds and are diagnostic only:

| Operation | PostgreSQL | SQL Server | Cohesion embedded | Cohesion TCP |
|---|---:|---:|---:|---:|
| order-lines | 1.208 | 0.439 | 280.319 | 93.311 |
| place-order | 18.431 | 20.047 | 6.663 | 14.127 |
| ship-order | 3.605 | 3.539 | 13.031 | 29.263 |
| update-customer | 1.373 | 2.377 | 5.019 | 3.877 |
| orders-full-aggregate | 2.441 | 2.559 | 61.183 | 69.375 |

The embedded throughput process took 368.246 seconds wall time despite eight
approximately ten-second measured windows. Post-pass inverse UPDATE/DELETE scans
restore committed writes outside measurement and explain much of the remaining
time. The final TCP digest comparison checks the resulting baseline, rather than
assuming cleanup succeeded. No scan or durable commit was bypassed in a timed
write operation.
