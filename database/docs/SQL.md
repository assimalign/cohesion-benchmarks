# SQL commerce benchmark

## Contract, implementation and support

The adapter is registered by [ModelRegistration](../src/Assimalign.Cohesion.Benchmarks.Database.Sql/ModelRegistration.cs)
and implements all 24 operations from DESIGN section 6.1. Source of truth:
[SqlSchema](../src/Assimalign.Cohesion.Benchmarks.Database.Sql/SqlSchema.cs),
[SqlQueries](../src/Assimalign.Cohesion.Benchmarks.Database.Sql/SqlQueries.cs),
[SqlParameterStream](../src/Assimalign.Cohesion.Benchmarks.Database.Sql/SqlParameterStream.cs)
and [SqlModel](../src/Assimalign.Cohesion.Benchmarks.Database.Sql/SqlModel.cs).
`SqlTarget` handles lifecycle/load; `SqlSession` adapts the three public APIs;
`SqlBenchmarkClient` executes and drains results. No Cohesion sample-harness or
internal API dependency is used, and the Cohesion checkout was not changed.

Each native entry below passed 100 deterministic verification cases. PostgreSQL
and SQL Server support all 24 natively. Both Cohesion transports have 17 native,
2 emulated and 5 unsupported operations. `V` means real-engine verification;
the other evidence letters are explained immediately below the table.

| Operation | Cohesion embedded | Cohesion TCP | PostgreSQL | SQL Server | Evidence |
|---|---|---|---|---|---|
| `sql.customer-by-id` | native | native | native | native | V |
| `sql.customer-by-email` | native | native | native | native | V |
| `sql.order-lines` | native | native | native | native | V |
| `sql.order-detail` | unsupported | unsupported | native | native | J |
| `sql.customer-recent-orders` | native | native | native | native | V |
| `sql.product-review-summary` | native | native | native | native | V |
| `sql.product-latest-reviews` | native | native | native | native | V |
| `sql.inventory-by-product` | native | native | native | native | V |
| `sql.tx.place-order` | native | native | native | native | V |
| `sql.tx.ship-order` | native | native | native | native | V |
| `sql.update-customer` | native | native | native | native | V |
| `sql.a.revenue-by-channel` | native | native | native | native | V |
| `sql.a.revenue-by-month` | emulated | emulated | native | native | M |
| `sql.a.top-products-in-category` | unsupported | unsupported | native | native | J |
| `sql.a.repeat-customers` | native | native | native | native | V |
| `sql.a.reorder-report` | native | native | native | native | V |
| `sql.a.brand-ratings` | native | native | native | native | V |
| `sql.a.unshipped-orders` | native | native | native | native | N |
| `sql.a.orders-full-aggregate` | native | native | native | native | V |
| `sql.a.product-name-like` | native | native | native | native | V |
| `sql.a.country-category-revenue` | unsupported | unsupported | native | native | C/J |
| `sql.a.category-rank-window` | unsupported | unsupported | native | native | C/W |
| `sql.a.above-country-average` | unsupported | unsupported | native | native | S |
| `sql.a.monthly-growth-cte` | emulated | emulated | native | native | M/C/W |

### Cohesion capability evidence and minimal probes

All Cohesion references in this document are relative to
`$COHESION_ROOT/resources/Database/` at `bf20be5eb`. `SQL DESIGN` abbreviates
`Assimalign.Cohesion.Database.Sql/docs/DESIGN.md`; `DIALECT` abbreviates
`Assimalign.Cohesion.Database.Sql.Language/docs/DIALECT.md`.
The [capability test](../tests/Assimalign.Cohesion.Benchmarks.Database.Tests/Sql/CohesionCapabilityTests.cs)
executes these probes and the actual five unsupported query texts. Exact observed
messages, rather than inferred feature support, determine the classifications:

| Code | Probe | Actual error / evidence |
|---|---|---|
| J | `SELECT a.id FROM a INNER JOIN b ON a.id=b.id INNER JOIN c ON a.id=c.id` | `SQL parse error COHDBL001: The SQL JOIN surface supports exactly two tables; additional JOIN clauses are not supported.` DIALECT:55,616-618. Actual order-detail and top-products queries return this message. |
| C | `WITH x AS (SELECT id FROM a) SELECT id FROM x` | `SQL parse error COHDBL001: The WITH clause is not supported by the SQL surface of this database model.` DIALECT:61-63. The actual country-category and category-rank queries fail here first. |
| W | `SELECT RANK() OVER (PARTITION BY id ORDER BY happened) FROM a` | `SQL parse error COHDBL001: The OVER clause is not supported by the SQL surface of this database model.` DIALECT:61-63. |
| S | `SELECT id FROM a WHERE id > (SELECT AVG(b.id) FROM b WHERE b.id=a.id)` | `SQL parse error COHDBL001: Correlated SQL subqueries are not supported: reference 'a.id' is outside the subquery's local FROM/JOIN scope.` The actual operation reports reference `c.country_code`. DIALECT:971-990. |
| M | `SELECT EXTRACT(YEAR FROM happened) FROM a` | `SQL parse error SQL0003: Expected ')' after the EXTRACT arguments but found 'FROM'.` |
| M | `SELECT EXTRACT('year', happened) FROM a` | `Function 'EXTRACT' is not supported by the executor yet.` DIALECT:853-860,1572-1581. |
| N | `SELECT id FROM a WHERE id NOT IN (SELECT id FROM b) ORDER BY id` | Succeeds; the full unshipped-orders operation also passes all 100 cases. |

The probe fixture creates `a`, `b`, `c` with `(id BIGINT PRIMARY KEY, happened
TIMESTAMP)` and inserts `(1,'2025-01-01T00:00:00')`. Country/category revenue joins
five relations including a category-root CTE; that CTE traverses stored parent
rows. Removing its CTE would still exceed the two-table limit. Resolving hierarchy
from the generator in the driver, or doing the country-average comparison there,
would violate DESIGN section 6's prohibition on client joins/filtering. Neither
is done. Window ranking remains unsupported, including tied rank semantics.

`NOT IN` is an exact native rewrite of the required anti-existence query because
`shipments.order_id` is declared `NOT NULL`, as is the outer order key. No client
filtering or joined-table materialization is used for that operation.

For both monthly operations, the engine executes the selective half-open year
predicate and returns `(order_date, channel, total)`. The client folds those
selected rows into at most 48 decimal month/channel buckets. Monthly growth sums
the channels and emits current revenue, previous existing month's revenue and
their **absolute currency difference**, with NULL previous/growth on the first
month. Missing months are skipped, matching `LAG` over the native monthly result.
This is bounded-memory selected-row aggregation explicitly permitted by DESIGN
section 6; it is always labeled emulated. Filtering, row transfer and aggregation
are all inside the logical operation's timer, including when no digest sink is
present. There is no result cache or client join.

### Schema and exact semantic choices

DDL is rendered from `CommerceSchema`, including all columns, nullability,
precision/scale, composite keys and 14 secondary indexes. There are no foreign
keys. SQL Server uses `BIT`, `DATETIME2(0)`, `VARCHAR(n)` / `VARCHAR(MAX)`, database
and column collation `Latin1_General_100_BIN2_UTF8`, recovery `SIMPLE` and delayed
durability disabled. PostgreSQL creates from `template0` with UTF8, `LC_COLLATE
'C' LC_CTYPE 'C'`; string columns additionally say `COLLATE "C"`.

Cohesion's default is **binary, culture-independent Unicode code-point order**:
DIALECT:530-549 and `Assimalign.Cohesion.Database.Types/src/Collation.cs:28-29,99-109`.
Its binary comparison uses unchanged UTF-8 order; it is not .NET's UTF-16 ordinal
comparison for arbitrary supplementary characters. This is the precise meaning
of binary/ordinal in this suite. Columns explicitly say `COLLATE binary`; the
commerce corpus verifies equal equality and ordering across all three dialects.
DIALECT:552-554 confirms WHERE, LIKE, ordering, grouping, distinct and uniqueness
use the same effective collation. No digest string normalization was added.

Dates are bound parameters: `order_date >= @start AND order_date < @end`. Cohesion
has no typed date literals. Npgsql receives unspecified-kind timestamps for
timestamp-without-time-zone; the common logical values remain UTC. ADO `DATE`
fields are mapped from reader metadata to `DateOnly`, preserving the logical
schema instead of changing the harness normalizer. AVG of an integer rating is
cast to `NUMERIC(18,6)` before averaging so SQL Server cannot truncate it; the
result is also decimal(18,6). Empty aggregates retain SQL NULL semantics. Native
date extraction is EXTRACT on PostgreSQL and DATEPART on SQL Server. Concatenation
uses `||` versus `+`; SQL Server limits use ordered `OFFSET 0 ROWS FETCH NEXT`.
Every limited result includes a key tie-breaker, and all other multirow results
have the total order shown in the statements below.

Parameters are generated before timing using deterministic `EntityRandom` streams
keyed by seed, operation and client. Customer/product reads reuse the dataset's
Zipf distribution and id permutations. Order lookups are uniform over generated
orders. Analytics draw years 2021-2025, quarters 1-4, leaf categories 71-400 and
warehouses 1-20 uniformly as applicable. LIKE terms come from a deterministic
product-name token. All targets receive the same sequence for a given context.

New order/payment/shipment ids use the manifest's generated maximum and
`ParameterStreamContext.WriteId`: `base + client * 10^9 + sequence`, with the
harness's suite/repetition/concurrency/warm-up namespace incorporated in base.
Existing mutable entities are partitioned into disjoint client stripes on every
engine. Ship history uses unused sequence numbers above the generated maximum;
place-order writes 1-6 lines, payment and history and decrements actual inventory
rows. The two txn operations explicitly BEGIN and COMMIT. Update-customer is one
autocommitted UPDATE. Verification reads committed write state in fixed statement
and row order. R5 removes post-pass inverse mutations: all read measurements precede writes and mixed passes.
History seq is widened to BIGINT identically on all engines for disjoint cross-pass write namespaces. Cohesion UPDATE/DELETE scans and durable
commits are used normally, with no retry or index-based workaround.

### Load paths and limits

PostgreSQL uses `COPY <table> FROM STDIN (FORMAT csv, HEADER true)` and streams each
part's original bytes into Npgsql's text-import `BaseStream` (no CSV reserialization).
SQL Server uses `SqlBulkCopy` with a typed `DbDataReader` implementing `IDataReader`
over the shared CSV reader, `EnableStreaming=true`, `TableLock` and batches of at
least 10,000. Each path runs up to four parts concurrently within a table. Tables
are sequenced to preserve per-table wall-time accounting; the tiny smoke has only
one part per table. All larger tables with multiple parts use the same concurrency
limit, without requiring a different adapter.

Cohesion embedded uses public `SqlDatabaseSession.ExecuteAsync` multi-row INSERTs,
at most 1,000 rows and 1 MiB UTF-8 per statement, four statements per explicit
transaction. A conservative serialized-row check rejects rows above 8,000 bytes,
below the 8 KiB record limit; statements are well below the 16 MiB wire frame limit.
One producer feeds bounded queues for the measured 1-versus-4-session chooser and
the selected full-load path. SQL DESIGN:2186-2214 documents the one-writer-statement
apply gate; concurrent parsing/input work can still help, as the sample showed.
The public TCP `SqlClient` uses the same batching in one session for the fixed
scratch sample. The server opens that persisted database on handshake through
`Assimalign.Cohesion.Database.Sql/src/Internal/SqlDatabaseServerSession.cs:483-490`.

Indexes are always deferred until all data is loaded. PostgreSQL then ANALYZEs
every table; SQL Server runs UPDATE STATISTICS. Cohesion has no statistics command
or cost-based planner at this pin (SQL DESIGN:1583-1591); only CREATE INDEX is timed
in its finalization phase. That limitation is recorded in every load metric.

### Engine findings

No wrong-result finding was observed in the supported or emulated operations.
The capability errors above are reproducible findings; both date-function failures
were surprising because the parsed call syntax and executable surface differ.
Binary collation's precise Unicode ordering also matters when extending the corpus.

The slow `order-lines` operation has a source-level explanation:
`Assimalign.Cohesion.Database.Sql/src/Internal/SqlPlanExecutor.Joins.cs:30-35`
applies WHERE to joined candidates, while line 64 scans the outer table even for
this order-id lookup; lines 67-80 seek or buffer the inner side. The measured
280.319 ms embedded / 93.311 ms TCP medians are compatible with that path; the
timing difference between transports on this busy host is not evidence that TCP
is intrinsically faster. Minimal workload statement:

```sql
SELECT o.order_id, i.line_no
FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id
WHERE o.order_id = @id ORDER BY i.line_no;
```

SQL DESIGN:1656-1657 explicitly states UPDATE/DELETE target selection still scans.
The measured ship/update latencies and long post-pass cleanup are therefore
expected; this adapter exposes them. SQL DESIGN:2186-2214 also limits concurrent
writer apply work. No engine finding has been filed remotely.

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

## Build and test ledger

All commands ran from this worktree with `COHESION_ROOT=C:\cw\bench\coh-sql` and
Release configuration. Logs below are under ignored `database/artifacts/`.
The final full suite passed **80 tests, failed 0, skipped 1**. The skip is the
foundation's opt-in `Generate_ScaleOne_ReportsActualSizesAndCounts`, which requires
`DBBENCH_LONG_TESTS=1`. No test was weakened or removed.

```powershell
dotnet build database/Assimalign.Cohesion.Benchmarks.Database.slnx -c Release
dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release --no-build
dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release --no-build --filter FullyQualifiedName~Sql --logger 'console;verbosity=detailed'
```

| Command / invocation | Log | Result |
|---|---|---|
| Solution build after schema | `sql-build-1.log` | pass, 0 warnings / 0 errors |
| Solution build after loaders | `sql-build-2.log` | pass, 12 warnings / 0 errors |
| Solution build with SqlClient 7.1.0 | `sql-build-2b.log` | pass, 12 warnings / 0 errors |
| Solution build after workloads | `sql-build-3.log` | pass, 12 warnings / 0 errors |
| Solution build after real-engine fixes | `sql-build-4-final.log` | pass, 12 warnings / 0 errors |
| Final solution build after precise capability evidence | `sql-build-final.log` | pass, 12 warnings / 0 errors |
| Full tests after schema, `--no-build` | `sql-test-1.log` | 72 pass / 0 fail / 1 skip |
| SQL-filtered tests after loaders, `--no-build` | `sql-test-2.log` | 6 pass / 0 fail / 0 skip |
| SQL-filtered tests after workloads, `--no-restore -p:BuildProjectReferences=false --logger 'console;verbosity=detailed'` | `sql-test-3.log` | 11 pass / 0 fail / 0 skip |
| Full tests after real-engine fixes, `--no-build --logger 'console;verbosity=detailed'` | `sql-test-final.log` | 80 pass / 0 fail / 1 skip |
| Final SQL-filtered tests | `sql-test-final-sql.log` | 13 pass / 0 fail / 0 skip |

Three additional targeted driver rebuilds used:

```powershell
dotnet build database/src/Assimalign.Cohesion.Benchmarks.Database.Cli -c Release --no-restore
# Final console-only change also used -p:BuildProjectReferences=false.
```

`sql-build-4a.log`, `sql-build-4b.log`, `sql-build-4c.log`: all passed, 4 warnings /
0 errors each. The repeated warnings are NuGet NU1902/NU1903 for SqlClient's
transitive `System.Security.Cryptography.Pkcs` (initially 9.0.13, finally 9.0.18),
including GHSA-2fmv-rv2c-59x7 and GHSA-846f-f764-76mx on 9.0.18. They do not fail
the build. A central patched-version decision remains for the owner; this brief
authorized new Npgsql/SqlClient entries, not a broader transitive package policy.

`Build-BenchImages.ps1 -Tag r2` passed twice during smoke development
(`sql-images.log`, `sql-images-2.log`). The second build included the globalization
fix. A third, final `Build-BenchImages.ps1 -Tag r2` passed after all adapter fixes
(`sql-images-final.log`), building both driver and server against the same pinned
Cohesion commit. `podman run --rm --name dbbench-cohesion-sql-r2 --cpus 3 --memory 2g
--memory-swap 2g localhost/dbbench-driver:r2 list --model sql` then exited 0 and
listed four targets / 24 operations (`sql-image-final-list.log`); its container
was automatically removed. A local, uncommitted audit helper exported the query text below and checked
budget metadata. Its first three builds failed with CS5001 (the `artifacts`
source exclusion) and CA2252 (preview assembly annotation); after adding the
explicit Compile item and annotation, builds 4 and 5 passed. Those scratch
failures did not involve the benchmark solution. The audit then confirmed that
the nine archived/current result pairs differed **only** in their budget field.

### Resolved smoke setup failures and shared changes

The first PostgreSQL load ingested successfully but failed introspection with
`42704: unrecognized configuration parameter "lc_collate"`. Reading `datcollate`
and `datctype` from `pg_database` fixed this; the recorded complete load is the
successful repeat. SqlClient first failed with `NotSupportedException:
Globalization Invariant Mode is not supported.` Disabling invariant globalization
in the CLI and driver image fixed the blocker. A localhost SQL connection then
timed out on this machine; the successful runs used IPv4 explicitly. Diagnostic
pings before database creation also correctly reported that `benchmark` did not
yet exist. These setup attempts are in `sql-postgresql-load.log`,
`sql-sqlclient-ping*.log` and `sql-sqlserver-load.log`.

The smallest shared-file changes needed for this adapter were the test project's
SQL reference, moving CLI engine description after load preparation (the database
does not exist beforehand), CLI/driver globalization configuration, observed
resource-budget metadata and separate full/sample LOAD console totals. The SQL
Edge catalog command change addresses shutdown and is documented separately in
the engine's [shutdown note](../engines/sqlserver/shutdown.md). Dataset generation, result normalization, operation
timers, report scoring and other model adapters were not changed.

## Statements and total orders

Generated from `SqlQueries`, `SqlSchema`, and the deterministic write parameter stream.
Bindings are prepared before timing; dates use `@start <= order_date < @end`.
All limited queries include a key tie-breaker. A primary-key/unique lookup and a
single aggregate row are trivially ordered. The two emulations emit year, month,
then binary channel order, or year/month order for growth.

### `sql.customer-by-id`

**Cohesion**

```sql
SELECT * FROM customers WHERE customer_id = @id;
```

**PostgreSql**

```sql
SELECT * FROM customers WHERE customer_id = @id;
```

**SqlServer**

```sql
SELECT * FROM customers WHERE customer_id = @id;
```

### `sql.customer-by-email`

**Cohesion**

```sql
SELECT * FROM customers WHERE email = @email;
```

**PostgreSql**

```sql
SELECT * FROM customers WHERE email = @email;
```

**SqlServer**

```sql
SELECT * FROM customers WHERE email = @email;
```

### `sql.order-lines`

**Cohesion**

```sql
SELECT o.order_id, o.customer_id, o.order_date, o.status, i.line_no, i.product_id, i.quantity, i.unit_price, i.discount, i.line_total FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id WHERE o.order_id = @id ORDER BY i.line_no;
```

**PostgreSql**

```sql
SELECT o.order_id, o.customer_id, o.order_date, o.status, i.line_no, i.product_id, i.quantity, i.unit_price, i.discount, i.line_total FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id WHERE o.order_id = @id ORDER BY i.line_no;
```

**SqlServer**

```sql
SELECT o.order_id, o.customer_id, o.order_date, o.status, i.line_no, i.product_id, i.quantity, i.unit_price, i.discount, i.line_total FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id WHERE o.order_id = @id ORDER BY i.line_no;
```

### `sql.order-detail`

**Cohesion**

```sql
SELECT o.order_id, o.customer_id, o.order_date, o.status, i.line_no, i.product_id, p.name, i.quantity, i.unit_price, i.discount, i.line_total FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id INNER JOIN products p ON i.product_id = p.product_id WHERE o.order_id = @id ORDER BY i.line_no;
```

**PostgreSql**

```sql
SELECT o.order_id, o.customer_id, o.order_date, o.status, i.line_no, i.product_id, p.name, i.quantity, i.unit_price, i.discount, i.line_total FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id INNER JOIN products p ON i.product_id = p.product_id WHERE o.order_id = @id ORDER BY i.line_no;
```

**SqlServer**

```sql
SELECT o.order_id, o.customer_id, o.order_date, o.status, i.line_no, i.product_id, p.name, i.quantity, i.unit_price, i.discount, i.line_total FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id INNER JOIN products p ON i.product_id = p.product_id WHERE o.order_id = @id ORDER BY i.line_no;
```

### `sql.customer-recent-orders`

**Cohesion**

```sql
SELECT * FROM orders WHERE customer_id = @id ORDER BY order_date DESC, order_id DESC LIMIT 20;
```

**PostgreSql**

```sql
SELECT * FROM orders WHERE customer_id = @id ORDER BY order_date DESC, order_id DESC LIMIT 20;
```

**SqlServer**

```sql
SELECT * FROM orders WHERE customer_id = @id ORDER BY order_date DESC, order_id DESC OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY;
```

### `sql.product-review-summary`

**Cohesion**

```sql
SELECT COUNT(*), CAST(AVG(CAST(rating AS NUMERIC(18,6))) AS NUMERIC(18,6)) FROM reviews WHERE product_id = @id;
```

**PostgreSql**

```sql
SELECT COUNT(*), CAST(AVG(CAST(rating AS NUMERIC(18,6))) AS NUMERIC(18,6)) FROM reviews WHERE product_id = @id;
```

**SqlServer**

```sql
SELECT COUNT(*), CAST(AVG(CAST(rating AS NUMERIC(18,6))) AS NUMERIC(18,6)) FROM reviews WHERE product_id = @id;
```

### `sql.product-latest-reviews`

**Cohesion**

```sql
SELECT * FROM reviews WHERE product_id = @id ORDER BY created_at DESC, review_id DESC LIMIT 5;
```

**PostgreSql**

```sql
SELECT * FROM reviews WHERE product_id = @id ORDER BY created_at DESC, review_id DESC LIMIT 5;
```

**SqlServer**

```sql
SELECT * FROM reviews WHERE product_id = @id ORDER BY created_at DESC, review_id DESC OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY;
```

### `sql.inventory-by-product`

**Cohesion**

```sql
SELECT warehouse_id, product_id, quantity_on_hand, reorder_point, updated_at FROM inventory WHERE product_id = @id ORDER BY warehouse_id;
```

**PostgreSql**

```sql
SELECT warehouse_id, product_id, quantity_on_hand, reorder_point, updated_at FROM inventory WHERE product_id = @id ORDER BY warehouse_id;
```

**SqlServer**

```sql
SELECT warehouse_id, product_id, quantity_on_hand, reorder_point, updated_at FROM inventory WHERE product_id = @id ORDER BY warehouse_id;
```

### `sql.tx.place-order`

**Cohesion**

```sql
BEGIN;
INSERT INTO orders (order_id,customer_id,order_date,status,channel,shipping_address_id,currency,subtotal,tax,shipping_fee,total,promo_code) VALUES (@id,@customer,@time,'paid','web',@customer,'USD',@subtotal,@tax,0,@total,NULL);
INSERT INTO order_items (order_id,line_no,product_id,quantity,unit_price,discount,line_total) VALUES (@id,@line,@product,@quantity,@price,0,@total);
INSERT INTO payments (payment_id,order_id,method,amount,status,paid_at,provider_ref) VALUES (@payment,@id,'card',@total,'succeeded',@time,@reference);
INSERT INTO order_status_history (order_id,seq,status,occurred_at,note) VALUES (@id,1,'paid',@time,NULL);
UPDATE inventory SET quantity_on_hand = quantity_on_hand - @quantity WHERE warehouse_id = @warehouse AND product_id = @product;
COMMIT;
```

**PostgreSql**

```sql
BEGIN;
INSERT INTO orders (order_id,customer_id,order_date,status,channel,shipping_address_id,currency,subtotal,tax,shipping_fee,total,promo_code) VALUES (@id,@customer,@time,'paid','web',@customer,'USD',@subtotal,@tax,0,@total,NULL);
INSERT INTO order_items (order_id,line_no,product_id,quantity,unit_price,discount,line_total) VALUES (@id,@line,@product,@quantity,@price,0,@total);
INSERT INTO payments (payment_id,order_id,method,amount,status,paid_at,provider_ref) VALUES (@payment,@id,'card',@total,'succeeded',@time,@reference);
INSERT INTO order_status_history (order_id,seq,status,occurred_at,note) VALUES (@id,1,'paid',@time,NULL);
UPDATE inventory SET quantity_on_hand = quantity_on_hand - @quantity WHERE warehouse_id = @warehouse AND product_id = @product;
COMMIT;
```

**SqlServer**

```sql
BEGIN TRANSACTION;
INSERT INTO orders (order_id,customer_id,order_date,status,channel,shipping_address_id,currency,subtotal,tax,shipping_fee,total,promo_code) VALUES (@id,@customer,@time,'paid','web',@customer,'USD',@subtotal,@tax,0,@total,NULL);
INSERT INTO order_items (order_id,line_no,product_id,quantity,unit_price,discount,line_total) VALUES (@id,@line,@product,@quantity,@price,0,@total);
INSERT INTO payments (payment_id,order_id,method,amount,status,paid_at,provider_ref) VALUES (@payment,@id,'card',@total,'succeeded',@time,@reference);
INSERT INTO order_status_history (order_id,seq,status,occurred_at,note) VALUES (@id,1,'paid',@time,NULL);
UPDATE inventory SET quantity_on_hand = quantity_on_hand - @quantity WHERE warehouse_id = @warehouse AND product_id = @product;
COMMIT;
```

The verification path reads committed state back; post-pass inverse mutations restore the baseline outside measurement. For place-order the line INSERT and inventory UPDATE repeat for each of 1–6 lines.

### `sql.tx.ship-order`

**Cohesion**

```sql
BEGIN;
UPDATE orders SET status = 'shipped' WHERE order_id = @id;
INSERT INTO shipments (shipment_id,order_id,warehouse_id,carrier,tracking_no,shipped_at,delivered_at) VALUES (@shipment,@id,@warehouse,'Benchmark Carrier',@tracking,@time,NULL);
INSERT INTO order_status_history (order_id,seq,status,occurred_at,note) VALUES (@id,@seq,'shipped',@time,NULL);
COMMIT;
```

**PostgreSql**

```sql
BEGIN;
UPDATE orders SET status = 'shipped' WHERE order_id = @id;
INSERT INTO shipments (shipment_id,order_id,warehouse_id,carrier,tracking_no,shipped_at,delivered_at) VALUES (@shipment,@id,@warehouse,'Benchmark Carrier',@tracking,@time,NULL);
INSERT INTO order_status_history (order_id,seq,status,occurred_at,note) VALUES (@id,@seq,'shipped',@time,NULL);
COMMIT;
```

**SqlServer**

```sql
BEGIN TRANSACTION;
UPDATE orders SET status = 'shipped' WHERE order_id = @id;
INSERT INTO shipments (shipment_id,order_id,warehouse_id,carrier,tracking_no,shipped_at,delivered_at) VALUES (@shipment,@id,@warehouse,'Benchmark Carrier',@tracking,@time,NULL);
INSERT INTO order_status_history (order_id,seq,status,occurred_at,note) VALUES (@id,@seq,'shipped',@time,NULL);
COMMIT;
```

The verification path reads committed state back; post-pass inverse mutations restore the baseline outside measurement. For place-order the line INSERT and inventory UPDATE repeat for each of 1–6 lines.

### `sql.update-customer`

**Cohesion**

```sql
UPDATE customers SET city = @city WHERE customer_id = @id;
```

**PostgreSql**

```sql
UPDATE customers SET city = @city WHERE customer_id = @id;
```

**SqlServer**

```sql
UPDATE customers SET city = @city WHERE customer_id = @id;
```

The verification path reads committed state back; post-pass inverse mutations restore the baseline outside measurement. For place-order the line INSERT and inventory UPDATE repeat for each of 1–6 lines.

### `sql.a.revenue-by-channel`

**Cohesion**

```sql
SELECT channel, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end AND status <> 'cancelled' GROUP BY channel ORDER BY channel;
```

**PostgreSql**

```sql
SELECT channel, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end AND status <> 'cancelled' GROUP BY channel ORDER BY channel;
```

**SqlServer**

```sql
SELECT channel, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end AND status <> 'cancelled' GROUP BY channel ORDER BY channel;
```

### `sql.a.revenue-by-month`

**Cohesion**

```sql
SELECT order_date, channel, total FROM orders WHERE order_date >= @start AND order_date < @end;
```

**PostgreSql**

```sql
SELECT CAST(EXTRACT(YEAR FROM order_date) AS INTEGER) AS year_no, CAST(EXTRACT(MONTH FROM order_date) AS INTEGER) AS month_no, channel, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end GROUP BY EXTRACT(YEAR FROM order_date), EXTRACT(MONTH FROM order_date), channel ORDER BY year_no, month_no, channel;
```

**SqlServer**

```sql
SELECT CAST(DATEPART(year, order_date) AS INTEGER) AS year_no, CAST(DATEPART(month, order_date) AS INTEGER) AS month_no, channel, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end GROUP BY DATEPART(year, order_date), DATEPART(month, order_date), channel ORDER BY year_no, month_no, channel;
```

### `sql.a.top-products-in-category`

**Cohesion**

```sql
SELECT p.product_id, p.name, SUM(i.line_total) AS revenue FROM products p INNER JOIN order_items i ON p.product_id = i.product_id INNER JOIN orders o ON i.order_id = o.order_id WHERE p.category_id = @category AND o.order_date >= @start AND o.order_date < @end AND o.status <> 'cancelled' GROUP BY p.product_id, p.name ORDER BY revenue DESC, p.product_id LIMIT 20;
```

**PostgreSql**

```sql
SELECT p.product_id, p.name, SUM(i.line_total) AS revenue FROM products p INNER JOIN order_items i ON p.product_id = i.product_id INNER JOIN orders o ON i.order_id = o.order_id WHERE p.category_id = @category AND o.order_date >= @start AND o.order_date < @end AND o.status <> 'cancelled' GROUP BY p.product_id, p.name ORDER BY revenue DESC, p.product_id LIMIT 20;
```

**SqlServer**

```sql
SELECT p.product_id, p.name, SUM(i.line_total) AS revenue FROM products p INNER JOIN order_items i ON p.product_id = i.product_id INNER JOIN orders o ON i.order_id = o.order_id WHERE p.category_id = @category AND o.order_date >= @start AND o.order_date < @end AND o.status <> 'cancelled' GROUP BY p.product_id, p.name ORDER BY revenue DESC, p.product_id OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY;
```

### `sql.a.repeat-customers`

**Cohesion**

```sql
SELECT customer_id, COUNT(*) AS order_count, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end GROUP BY customer_id HAVING COUNT(*) >= 5 ORDER BY order_count DESC, customer_id LIMIT 100;
```

**PostgreSql**

```sql
SELECT customer_id, COUNT(*) AS order_count, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end GROUP BY customer_id HAVING COUNT(*) >= 5 ORDER BY order_count DESC, customer_id LIMIT 100;
```

**SqlServer**

```sql
SELECT customer_id, COUNT(*) AS order_count, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end GROUP BY customer_id HAVING COUNT(*) >= 5 ORDER BY order_count DESC, customer_id OFFSET 0 ROWS FETCH NEXT 100 ROWS ONLY;
```

### `sql.a.reorder-report`

**Cohesion**

```sql
SELECT i.warehouse_id, i.product_id, p.name, i.quantity_on_hand, i.reorder_point FROM inventory i INNER JOIN products p ON i.product_id = p.product_id WHERE i.warehouse_id = @warehouse AND i.quantity_on_hand < i.reorder_point ORDER BY i.product_id;
```

**PostgreSql**

```sql
SELECT i.warehouse_id, i.product_id, p.name, i.quantity_on_hand, i.reorder_point FROM inventory i INNER JOIN products p ON i.product_id = p.product_id WHERE i.warehouse_id = @warehouse AND i.quantity_on_hand < i.reorder_point ORDER BY i.product_id;
```

**SqlServer**

```sql
SELECT i.warehouse_id, i.product_id, p.name, i.quantity_on_hand, i.reorder_point FROM inventory i INNER JOIN products p ON i.product_id = p.product_id WHERE i.warehouse_id = @warehouse AND i.quantity_on_hand < i.reorder_point ORDER BY i.product_id;
```

### `sql.a.brand-ratings`

**Cohesion**

```sql
SELECT p.brand, COUNT(*) AS review_count, CAST(AVG(CAST(r.rating AS NUMERIC(18,6))) AS NUMERIC(18,6)) AS average_rating FROM products p INNER JOIN reviews r ON p.product_id = r.product_id GROUP BY p.brand HAVING COUNT(*) >= 100 ORDER BY p.brand;
```

**PostgreSql**

```sql
SELECT p.brand, COUNT(*) AS review_count, CAST(AVG(CAST(r.rating AS NUMERIC(18,6))) AS NUMERIC(18,6)) AS average_rating FROM products p INNER JOIN reviews r ON p.product_id = r.product_id GROUP BY p.brand HAVING COUNT(*) >= 100 ORDER BY p.brand;
```

**SqlServer**

```sql
SELECT p.brand, COUNT(*) AS review_count, CAST(AVG(CAST(r.rating AS NUMERIC(18,6))) AS NUMERIC(18,6)) AS average_rating FROM products p INNER JOIN reviews r ON p.product_id = r.product_id GROUP BY p.brand HAVING COUNT(*) >= 100 ORDER BY p.brand;
```

### `sql.a.unshipped-orders`

**Cohesion**

```sql
SELECT order_id, customer_id, order_date, total FROM orders WHERE status = 'paid' AND order_date < @start AND order_id NOT IN (SELECT order_id FROM shipments) ORDER BY order_date, order_id;
```

**PostgreSql**

```sql
SELECT order_id, customer_id, order_date, total FROM orders WHERE status = 'paid' AND order_date < @start AND order_id NOT IN (SELECT order_id FROM shipments) ORDER BY order_date, order_id;
```

**SqlServer**

```sql
SELECT order_id, customer_id, order_date, total FROM orders WHERE status = 'paid' AND order_date < @start AND order_id NOT IN (SELECT order_id FROM shipments) ORDER BY order_date, order_id;
```

### `sql.a.orders-full-aggregate`

**Cohesion**

```sql
SELECT COUNT(*), SUM(total), CAST(AVG(total) AS NUMERIC(18,6)) FROM orders;
```

**PostgreSql**

```sql
SELECT COUNT(*), SUM(total), CAST(AVG(total) AS NUMERIC(18,6)) FROM orders;
```

**SqlServer**

```sql
SELECT COUNT(*), SUM(total), CAST(AVG(total) AS NUMERIC(18,6)) FROM orders;
```

### `sql.a.product-name-like`

**Cohesion**

```sql
SELECT product_id, name FROM products WHERE name LIKE @term ORDER BY product_id LIMIT 50;
```

**PostgreSql**

```sql
SELECT product_id, name FROM products WHERE name LIKE @term ORDER BY product_id LIMIT 50;
```

**SqlServer**

```sql
SELECT product_id, name FROM products WHERE name LIKE @term ORDER BY product_id OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY;
```

### `sql.a.country-category-revenue`

**Cohesion**

```sql
WITH category_roots AS (SELECT leaf.category_id, root.category_id AS root_id, root.name AS root_name FROM categories leaf INNER JOIN categories parent ON leaf.parent_id = parent.category_id INNER JOIN categories root ON parent.parent_id = root.category_id) SELECT c.country_code, k.root_id, k.root_name, SUM(i.line_total) AS revenue FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id INNER JOIN products p ON p.product_id = i.product_id INNER JOIN customers c ON c.customer_id = o.customer_id INNER JOIN category_roots k ON k.category_id = p.category_id WHERE o.status = 'delivered' AND o.order_date >= @start AND o.order_date < @end GROUP BY c.country_code, k.root_id, k.root_name ORDER BY c.country_code, k.root_id;
```

**PostgreSql**

```sql
WITH category_roots AS (SELECT leaf.category_id, root.category_id AS root_id, root.name AS root_name FROM categories leaf INNER JOIN categories parent ON leaf.parent_id = parent.category_id INNER JOIN categories root ON parent.parent_id = root.category_id) SELECT c.country_code, k.root_id, k.root_name, SUM(i.line_total) AS revenue FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id INNER JOIN products p ON p.product_id = i.product_id INNER JOIN customers c ON c.customer_id = o.customer_id INNER JOIN category_roots k ON k.category_id = p.category_id WHERE o.status = 'delivered' AND o.order_date >= @start AND o.order_date < @end GROUP BY c.country_code, k.root_id, k.root_name ORDER BY c.country_code, k.root_id;
```

**SqlServer**

```sql
WITH category_roots AS (SELECT leaf.category_id, root.category_id AS root_id, root.name AS root_name FROM categories leaf INNER JOIN categories parent ON leaf.parent_id = parent.category_id INNER JOIN categories root ON parent.parent_id = root.category_id) SELECT c.country_code, k.root_id, k.root_name, SUM(i.line_total) AS revenue FROM orders o INNER JOIN order_items i ON o.order_id = i.order_id INNER JOIN products p ON p.product_id = i.product_id INNER JOIN customers c ON c.customer_id = o.customer_id INNER JOIN category_roots k ON k.category_id = p.category_id WHERE o.status = 'delivered' AND o.order_date >= @start AND o.order_date < @end GROUP BY c.country_code, k.root_id, k.root_name ORDER BY c.country_code, k.root_id;
```

### `sql.a.category-rank-window`

**Cohesion**

```sql
WITH revenues AS (SELECT p.category_id, p.product_id, SUM(i.line_total) AS revenue FROM products p INNER JOIN order_items i ON p.product_id = i.product_id GROUP BY p.category_id, p.product_id), ranked AS (SELECT category_id, product_id, revenue, RANK() OVER (PARTITION BY category_id ORDER BY revenue DESC) AS revenue_rank FROM revenues) SELECT category_id, product_id, revenue, revenue_rank FROM ranked WHERE revenue_rank <= 3 ORDER BY category_id, revenue_rank, product_id;
```

**PostgreSql**

```sql
WITH revenues AS (SELECT p.category_id, p.product_id, SUM(i.line_total) AS revenue FROM products p INNER JOIN order_items i ON p.product_id = i.product_id GROUP BY p.category_id, p.product_id), ranked AS (SELECT category_id, product_id, revenue, RANK() OVER (PARTITION BY category_id ORDER BY revenue DESC) AS revenue_rank FROM revenues) SELECT category_id, product_id, revenue, revenue_rank FROM ranked WHERE revenue_rank <= 3 ORDER BY category_id, revenue_rank, product_id;
```

**SqlServer**

```sql
WITH revenues AS (SELECT p.category_id, p.product_id, SUM(i.line_total) AS revenue FROM products p INNER JOIN order_items i ON p.product_id = i.product_id GROUP BY p.category_id, p.product_id), ranked AS (SELECT category_id, product_id, revenue, RANK() OVER (PARTITION BY category_id ORDER BY revenue DESC) AS revenue_rank FROM revenues) SELECT category_id, product_id, revenue, revenue_rank FROM ranked WHERE revenue_rank <= 3 ORDER BY category_id, revenue_rank, product_id;
```

### `sql.a.above-country-average`

**Cohesion**

```sql
SELECT c.customer_id, c.country_code, c.first_name || ' ' || c.last_name AS full_name, c.lifetime_value FROM customers c WHERE c.lifetime_value > (SELECT AVG(x.lifetime_value) FROM customers x WHERE x.country_code = c.country_code) ORDER BY c.country_code, c.customer_id;
```

**PostgreSql**

```sql
SELECT c.customer_id, c.country_code, c.first_name || ' ' || c.last_name AS full_name, c.lifetime_value FROM customers c WHERE c.lifetime_value > (SELECT AVG(x.lifetime_value) FROM customers x WHERE x.country_code = c.country_code) ORDER BY c.country_code, c.customer_id;
```

**SqlServer**

```sql
SELECT c.customer_id, c.country_code, c.first_name + ' ' + c.last_name AS full_name, c.lifetime_value FROM customers c WHERE c.lifetime_value > (SELECT AVG(x.lifetime_value) FROM customers x WHERE x.country_code = c.country_code) ORDER BY c.country_code, c.customer_id;
```

### `sql.a.monthly-growth-cte`

**Cohesion**

```sql
SELECT order_date, channel, total FROM orders WHERE order_date >= @start AND order_date < @end;
```

**PostgreSql**

```sql
WITH monthly AS (SELECT CAST(EXTRACT(YEAR FROM order_date) AS INTEGER) AS year_no, CAST(EXTRACT(MONTH FROM order_date) AS INTEGER) AS month_no, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end GROUP BY EXTRACT(YEAR FROM order_date), EXTRACT(MONTH FROM order_date)), growth AS (SELECT year_no, month_no, revenue, LAG(revenue) OVER (ORDER BY year_no, month_no) AS previous_revenue FROM monthly) SELECT year_no, month_no, revenue, previous_revenue, revenue - previous_revenue AS growth FROM growth ORDER BY year_no, month_no;
```

**SqlServer**

```sql
WITH monthly AS (SELECT CAST(DATEPART(year, order_date) AS INTEGER) AS year_no, CAST(DATEPART(month, order_date) AS INTEGER) AS month_no, SUM(total) AS revenue FROM orders WHERE order_date >= @start AND order_date < @end GROUP BY DATEPART(year, order_date), DATEPART(month, order_date)), growth AS (SELECT year_no, month_no, revenue, LAG(revenue) OVER (ORDER BY year_no, month_no) AS previous_revenue FROM monthly) SELECT year_no, month_no, revenue, previous_revenue, revenue - previous_revenue AS growth FROM growth ORDER BY year_no, month_no;
```

## Generated schema DDL

Secondary indexes are identical and appear separately after the three table schemas. No foreign keys.

### Cohesion schema

```sql
CREATE TABLE warehouses (warehouse_id BIGINT NOT NULL, name VARCHAR(100) COLLATE binary NOT NULL, region VARCHAR(100) COLLATE binary NOT NULL, country_code VARCHAR(2) COLLATE binary NOT NULL, PRIMARY KEY (warehouse_id));
CREATE TABLE categories (category_id BIGINT NOT NULL, parent_id BIGINT NULL, name VARCHAR(100) COLLATE binary NOT NULL, PRIMARY KEY (category_id));
CREATE TABLE suppliers (supplier_id BIGINT NOT NULL, name VARCHAR(160) COLLATE binary NOT NULL, country_code VARCHAR(2) COLLATE binary NOT NULL, rating NUMERIC(12,2) NOT NULL, PRIMARY KEY (supplier_id));
CREATE TABLE customers (customer_id BIGINT NOT NULL, email VARCHAR(254) COLLATE binary NOT NULL, first_name VARCHAR(80) COLLATE binary NOT NULL, last_name VARCHAR(80) COLLATE binary NOT NULL, phone VARCHAR(30) COLLATE binary NULL, birth_date DATE NOT NULL, country_code VARCHAR(2) COLLATE binary NOT NULL, city VARCHAR(100) COLLATE binary NOT NULL, created_at TIMESTAMP NOT NULL, loyalty_tier VARCHAR(20) COLLATE binary NOT NULL, marketing_opt_in BOOLEAN NOT NULL, lifetime_value NUMERIC(12,2) NOT NULL, PRIMARY KEY (customer_id));
CREATE TABLE customer_addresses (address_id BIGINT NOT NULL, customer_id BIGINT NOT NULL, kind VARCHAR(20) COLLATE binary NOT NULL, line1 VARCHAR(200) COLLATE binary NOT NULL, line2 VARCHAR(200) COLLATE binary NULL, city VARCHAR(100) COLLATE binary NOT NULL, region VARCHAR(100) COLLATE binary NOT NULL, postal_code VARCHAR(20) COLLATE binary NOT NULL, country_code VARCHAR(2) COLLATE binary NOT NULL, is_default BOOLEAN NOT NULL, PRIMARY KEY (address_id));
CREATE TABLE products (product_id BIGINT NOT NULL, sku VARCHAR(40) COLLATE binary NOT NULL, name VARCHAR(200) COLLATE binary NOT NULL, description TEXT COLLATE binary NOT NULL, category_id BIGINT NOT NULL, supplier_id BIGINT NOT NULL, brand VARCHAR(100) COLLATE binary NOT NULL, unit_price NUMERIC(12,2) NOT NULL, cost NUMERIC(12,2) NOT NULL, weight_grams INTEGER NOT NULL, created_at TIMESTAMP NOT NULL, is_active BOOLEAN NOT NULL, PRIMARY KEY (product_id));
CREATE TABLE inventory (warehouse_id BIGINT NOT NULL, product_id BIGINT NOT NULL, quantity_on_hand INTEGER NOT NULL, reorder_point INTEGER NOT NULL, updated_at TIMESTAMP NOT NULL, PRIMARY KEY (warehouse_id, product_id));
CREATE TABLE orders (order_id BIGINT NOT NULL, customer_id BIGINT NOT NULL, order_date TIMESTAMP NOT NULL, status VARCHAR(20) COLLATE binary NOT NULL, channel VARCHAR(20) COLLATE binary NOT NULL, shipping_address_id BIGINT NOT NULL, currency VARCHAR(3) COLLATE binary NOT NULL, subtotal NUMERIC(12,2) NOT NULL, tax NUMERIC(12,2) NOT NULL, shipping_fee NUMERIC(12,2) NOT NULL, total NUMERIC(12,2) NOT NULL, promo_code VARCHAR(40) COLLATE binary NULL, PRIMARY KEY (order_id));
CREATE TABLE order_items (order_id BIGINT NOT NULL, line_no SMALLINT NOT NULL, product_id BIGINT NOT NULL, quantity INTEGER NOT NULL, unit_price NUMERIC(12,2) NOT NULL, discount NUMERIC(12,2) NOT NULL, line_total NUMERIC(12,2) NOT NULL, PRIMARY KEY (order_id, line_no));
CREATE TABLE payments (payment_id BIGINT NOT NULL, order_id BIGINT NOT NULL, method VARCHAR(30) COLLATE binary NOT NULL, amount NUMERIC(12,2) NOT NULL, status VARCHAR(20) COLLATE binary NOT NULL, paid_at TIMESTAMP NOT NULL, provider_ref VARCHAR(80) COLLATE binary NOT NULL, PRIMARY KEY (payment_id));
CREATE TABLE shipments (shipment_id BIGINT NOT NULL, order_id BIGINT NOT NULL, warehouse_id BIGINT NOT NULL, carrier VARCHAR(40) COLLATE binary NOT NULL, tracking_no VARCHAR(80) COLLATE binary NOT NULL, shipped_at TIMESTAMP NOT NULL, delivered_at TIMESTAMP NULL, PRIMARY KEY (shipment_id));
CREATE TABLE reviews (review_id BIGINT NOT NULL, product_id BIGINT NOT NULL, customer_id BIGINT NOT NULL, rating SMALLINT NOT NULL, title VARCHAR(200) COLLATE binary NOT NULL, body TEXT COLLATE binary NOT NULL, created_at TIMESTAMP NOT NULL, helpful_votes INTEGER NOT NULL, PRIMARY KEY (review_id));
CREATE TABLE order_status_history (order_id BIGINT NOT NULL, seq SMALLINT NOT NULL, status VARCHAR(20) COLLATE binary NOT NULL, occurred_at TIMESTAMP NOT NULL, note VARCHAR(500) COLLATE binary NULL, PRIMARY KEY (order_id, seq));
```

### PostgreSql schema

```sql
CREATE DATABASE benchmark TEMPLATE template0 ENCODING 'UTF8' LC_COLLATE 'C' LC_CTYPE 'C';
CREATE TABLE warehouses (warehouse_id BIGINT NOT NULL, name VARCHAR(100) COLLATE "C" NOT NULL, region VARCHAR(100) COLLATE "C" NOT NULL, country_code VARCHAR(2) COLLATE "C" NOT NULL, PRIMARY KEY (warehouse_id));
CREATE TABLE categories (category_id BIGINT NOT NULL, parent_id BIGINT NULL, name VARCHAR(100) COLLATE "C" NOT NULL, PRIMARY KEY (category_id));
CREATE TABLE suppliers (supplier_id BIGINT NOT NULL, name VARCHAR(160) COLLATE "C" NOT NULL, country_code VARCHAR(2) COLLATE "C" NOT NULL, rating NUMERIC(12,2) NOT NULL, PRIMARY KEY (supplier_id));
CREATE TABLE customers (customer_id BIGINT NOT NULL, email VARCHAR(254) COLLATE "C" NOT NULL, first_name VARCHAR(80) COLLATE "C" NOT NULL, last_name VARCHAR(80) COLLATE "C" NOT NULL, phone VARCHAR(30) COLLATE "C" NULL, birth_date DATE NOT NULL, country_code VARCHAR(2) COLLATE "C" NOT NULL, city VARCHAR(100) COLLATE "C" NOT NULL, created_at TIMESTAMP NOT NULL, loyalty_tier VARCHAR(20) COLLATE "C" NOT NULL, marketing_opt_in BOOLEAN NOT NULL, lifetime_value NUMERIC(12,2) NOT NULL, PRIMARY KEY (customer_id));
CREATE TABLE customer_addresses (address_id BIGINT NOT NULL, customer_id BIGINT NOT NULL, kind VARCHAR(20) COLLATE "C" NOT NULL, line1 VARCHAR(200) COLLATE "C" NOT NULL, line2 VARCHAR(200) COLLATE "C" NULL, city VARCHAR(100) COLLATE "C" NOT NULL, region VARCHAR(100) COLLATE "C" NOT NULL, postal_code VARCHAR(20) COLLATE "C" NOT NULL, country_code VARCHAR(2) COLLATE "C" NOT NULL, is_default BOOLEAN NOT NULL, PRIMARY KEY (address_id));
CREATE TABLE products (product_id BIGINT NOT NULL, sku VARCHAR(40) COLLATE "C" NOT NULL, name VARCHAR(200) COLLATE "C" NOT NULL, description TEXT COLLATE "C" NOT NULL, category_id BIGINT NOT NULL, supplier_id BIGINT NOT NULL, brand VARCHAR(100) COLLATE "C" NOT NULL, unit_price NUMERIC(12,2) NOT NULL, cost NUMERIC(12,2) NOT NULL, weight_grams INTEGER NOT NULL, created_at TIMESTAMP NOT NULL, is_active BOOLEAN NOT NULL, PRIMARY KEY (product_id));
CREATE TABLE inventory (warehouse_id BIGINT NOT NULL, product_id BIGINT NOT NULL, quantity_on_hand INTEGER NOT NULL, reorder_point INTEGER NOT NULL, updated_at TIMESTAMP NOT NULL, PRIMARY KEY (warehouse_id, product_id));
CREATE TABLE orders (order_id BIGINT NOT NULL, customer_id BIGINT NOT NULL, order_date TIMESTAMP NOT NULL, status VARCHAR(20) COLLATE "C" NOT NULL, channel VARCHAR(20) COLLATE "C" NOT NULL, shipping_address_id BIGINT NOT NULL, currency VARCHAR(3) COLLATE "C" NOT NULL, subtotal NUMERIC(12,2) NOT NULL, tax NUMERIC(12,2) NOT NULL, shipping_fee NUMERIC(12,2) NOT NULL, total NUMERIC(12,2) NOT NULL, promo_code VARCHAR(40) COLLATE "C" NULL, PRIMARY KEY (order_id));
CREATE TABLE order_items (order_id BIGINT NOT NULL, line_no SMALLINT NOT NULL, product_id BIGINT NOT NULL, quantity INTEGER NOT NULL, unit_price NUMERIC(12,2) NOT NULL, discount NUMERIC(12,2) NOT NULL, line_total NUMERIC(12,2) NOT NULL, PRIMARY KEY (order_id, line_no));
CREATE TABLE payments (payment_id BIGINT NOT NULL, order_id BIGINT NOT NULL, method VARCHAR(30) COLLATE "C" NOT NULL, amount NUMERIC(12,2) NOT NULL, status VARCHAR(20) COLLATE "C" NOT NULL, paid_at TIMESTAMP NOT NULL, provider_ref VARCHAR(80) COLLATE "C" NOT NULL, PRIMARY KEY (payment_id));
CREATE TABLE shipments (shipment_id BIGINT NOT NULL, order_id BIGINT NOT NULL, warehouse_id BIGINT NOT NULL, carrier VARCHAR(40) COLLATE "C" NOT NULL, tracking_no VARCHAR(80) COLLATE "C" NOT NULL, shipped_at TIMESTAMP NOT NULL, delivered_at TIMESTAMP NULL, PRIMARY KEY (shipment_id));
CREATE TABLE reviews (review_id BIGINT NOT NULL, product_id BIGINT NOT NULL, customer_id BIGINT NOT NULL, rating SMALLINT NOT NULL, title VARCHAR(200) COLLATE "C" NOT NULL, body TEXT COLLATE "C" NOT NULL, created_at TIMESTAMP NOT NULL, helpful_votes INTEGER NOT NULL, PRIMARY KEY (review_id));
CREATE TABLE order_status_history (order_id BIGINT NOT NULL, seq SMALLINT NOT NULL, status VARCHAR(20) COLLATE "C" NOT NULL, occurred_at TIMESTAMP NOT NULL, note VARCHAR(500) COLLATE "C" NULL, PRIMARY KEY (order_id, seq));
```

### SqlServer schema

```sql
CREATE DATABASE benchmark COLLATE Latin1_General_100_BIN2_UTF8;
ALTER DATABASE benchmark SET RECOVERY SIMPLE;
ALTER DATABASE benchmark SET DELAYED_DURABILITY = DISABLED;
CREATE TABLE warehouses (warehouse_id BIGINT NOT NULL, name VARCHAR(100) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, region VARCHAR(100) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, country_code VARCHAR(2) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, PRIMARY KEY (warehouse_id));
CREATE TABLE categories (category_id BIGINT NOT NULL, parent_id BIGINT NULL, name VARCHAR(100) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, PRIMARY KEY (category_id));
CREATE TABLE suppliers (supplier_id BIGINT NOT NULL, name VARCHAR(160) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, country_code VARCHAR(2) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, rating NUMERIC(12,2) NOT NULL, PRIMARY KEY (supplier_id));
CREATE TABLE customers (customer_id BIGINT NOT NULL, email VARCHAR(254) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, first_name VARCHAR(80) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, last_name VARCHAR(80) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, phone VARCHAR(30) COLLATE Latin1_General_100_BIN2_UTF8 NULL, birth_date DATE NOT NULL, country_code VARCHAR(2) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, city VARCHAR(100) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, created_at DATETIME2(0) NOT NULL, loyalty_tier VARCHAR(20) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, marketing_opt_in BIT NOT NULL, lifetime_value NUMERIC(12,2) NOT NULL, PRIMARY KEY (customer_id));
CREATE TABLE customer_addresses (address_id BIGINT NOT NULL, customer_id BIGINT NOT NULL, kind VARCHAR(20) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, line1 VARCHAR(200) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, line2 VARCHAR(200) COLLATE Latin1_General_100_BIN2_UTF8 NULL, city VARCHAR(100) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, region VARCHAR(100) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, postal_code VARCHAR(20) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, country_code VARCHAR(2) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, is_default BIT NOT NULL, PRIMARY KEY (address_id));
CREATE TABLE products (product_id BIGINT NOT NULL, sku VARCHAR(40) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, name VARCHAR(200) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, description VARCHAR(MAX) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, category_id BIGINT NOT NULL, supplier_id BIGINT NOT NULL, brand VARCHAR(100) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, unit_price NUMERIC(12,2) NOT NULL, cost NUMERIC(12,2) NOT NULL, weight_grams INTEGER NOT NULL, created_at DATETIME2(0) NOT NULL, is_active BIT NOT NULL, PRIMARY KEY (product_id));
CREATE TABLE inventory (warehouse_id BIGINT NOT NULL, product_id BIGINT NOT NULL, quantity_on_hand INTEGER NOT NULL, reorder_point INTEGER NOT NULL, updated_at DATETIME2(0) NOT NULL, PRIMARY KEY (warehouse_id, product_id));
CREATE TABLE orders (order_id BIGINT NOT NULL, customer_id BIGINT NOT NULL, order_date DATETIME2(0) NOT NULL, status VARCHAR(20) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, channel VARCHAR(20) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, shipping_address_id BIGINT NOT NULL, currency VARCHAR(3) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, subtotal NUMERIC(12,2) NOT NULL, tax NUMERIC(12,2) NOT NULL, shipping_fee NUMERIC(12,2) NOT NULL, total NUMERIC(12,2) NOT NULL, promo_code VARCHAR(40) COLLATE Latin1_General_100_BIN2_UTF8 NULL, PRIMARY KEY (order_id));
CREATE TABLE order_items (order_id BIGINT NOT NULL, line_no SMALLINT NOT NULL, product_id BIGINT NOT NULL, quantity INTEGER NOT NULL, unit_price NUMERIC(12,2) NOT NULL, discount NUMERIC(12,2) NOT NULL, line_total NUMERIC(12,2) NOT NULL, PRIMARY KEY (order_id, line_no));
CREATE TABLE payments (payment_id BIGINT NOT NULL, order_id BIGINT NOT NULL, method VARCHAR(30) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, amount NUMERIC(12,2) NOT NULL, status VARCHAR(20) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, paid_at DATETIME2(0) NOT NULL, provider_ref VARCHAR(80) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, PRIMARY KEY (payment_id));
CREATE TABLE shipments (shipment_id BIGINT NOT NULL, order_id BIGINT NOT NULL, warehouse_id BIGINT NOT NULL, carrier VARCHAR(40) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, tracking_no VARCHAR(80) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, shipped_at DATETIME2(0) NOT NULL, delivered_at DATETIME2(0) NULL, PRIMARY KEY (shipment_id));
CREATE TABLE reviews (review_id BIGINT NOT NULL, product_id BIGINT NOT NULL, customer_id BIGINT NOT NULL, rating SMALLINT NOT NULL, title VARCHAR(200) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, body VARCHAR(MAX) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, created_at DATETIME2(0) NOT NULL, helpful_votes INTEGER NOT NULL, PRIMARY KEY (review_id));
CREATE TABLE order_status_history (order_id BIGINT NOT NULL, seq SMALLINT NOT NULL, status VARCHAR(20) COLLATE Latin1_General_100_BIN2_UTF8 NOT NULL, occurred_at DATETIME2(0) NOT NULL, note VARCHAR(500) COLLATE Latin1_General_100_BIN2_UTF8 NULL, PRIMARY KEY (order_id, seq));
```

### Secondary indexes, all dialects

```sql
CREATE UNIQUE INDEX ix_customers_email ON customers (email);
CREATE INDEX ix_customer_addresses_customer_id ON customer_addresses (customer_id);
CREATE UNIQUE INDEX ix_products_sku ON products (sku);
CREATE INDEX ix_products_category_id ON products (category_id);
CREATE INDEX ix_products_supplier_id ON products (supplier_id);
CREATE INDEX ix_inventory_product_id ON inventory (product_id);
CREATE INDEX ix_orders_customer_id_order_date ON orders (customer_id, order_date);
CREATE INDEX ix_orders_order_date ON orders (order_date);
CREATE INDEX ix_orders_status_order_date ON orders (status, order_date);
CREATE INDEX ix_order_items_product_id ON order_items (product_id);
CREATE INDEX ix_payments_order_id ON payments (order_id);
CREATE INDEX ix_shipments_order_id ON shipments (order_id);
CREATE INDEX ix_reviews_product_id_created_at ON reviews (product_id, created_at);
CREATE INDEX ix_reviews_customer_id ON reviews (customer_id);
```

After the indexes, PostgreSQL runs `ANALYZE <table>` and SQL Server runs `UPDATE STATISTICS <table>` on every table. Cohesion has no statistics command/cost model (SQL DESIGN:1583–1591).
