# Cohesion Database Hardening inventory

R6 proposes **28 new tasks in eight features**, and supplies current evidence for **25 tasks already covered by existing issues**. The machine-readable filing contract is [hardening-inventory.json](hardening-inventory.json). File only tasks with `fileAsNew: true`; link the other tasks to their existing issues. The Fxx/Txx identifiers are local cross-references, not proposed work-breakdown codes or issue-title prefixes.

The first priorities are read-only WAL elision (#1280), append/flush concurrency (#1276), selective SQL join inputs, and typed graph adjacency. The probes establish that tiny reads pay durable journal flushes and that concurrent callers wait behind journal/transaction gates. They do not establish that these mechanisms explain the entire original concurrency regression. The SQL and Documents selective-query gaps follow directly from their access paths. Functional work should reuse the T1 language tickets wherever their scope matches.

## Scope and evidence rules

This is analysis of Cohesion `bf20be5eb452fa57363f412bdcff36b2dc2e7edd`, with the benchmarks worktree based on `388199bfe748bc77a124b637c567c03612dcadcc`. Cohesion and the reference-engine repositories were read-only. Only this report and the inventory JSON were written in the benchmarks worktree. Probe source, generated build output, logs and local data stayed under `%TEMP%\r6-probe`; Linux data used the session-owned `dbbench-data-r6` volume. The probe container and volume were removed. No GitHub calls or writes were used; R5 resources were untouched.

The report distinguishes three kinds of evidence:

- **Development smoke**: the existing R2/R3/R4 measurements at SF 0.01 on a shared, noisy host. These locate problems; they are not SF10 rankings or capacity guarantees. Historical Windows embedded results and network competitors do not isolate transport or platform costs. Only verified operations are eligible for comparisons. The R4 embedded run had two statement-bound verification errors; its timings are diagnostic observations, not a ranked comparison.
- **R6 probe measurement**: a small deterministic fixture with timing, allocation, runtime contention and journal instrumentation. These measurements isolate mechanisms; they are not replacements for the official suite. Durability-disabled and held-snapshot controls deliberately change one cost and are never eligible benchmark results.
- **Code inspection**: a reachable implementation cost or missing feature, with no claim that its contribution to a smoke timing was separately measured. Proposed acceptance thresholds are future targets, not achieved results. Functional exclusions are supported by the pinned parser/profile/design and the capability baseline.

All reference precedents are from these local commits; file-and-line citations identify the implementation being adapted, not a claim that an entire reference design can be copied unchanged.

| Source | Commit | Root used for citations |
|---|---|---|
| Cohesion | `bf20be5eb452fa57363f412bdcff36b2dc2e7edd` | `C:/cw/bench/cohesion/` |
| PostgreSQL | `85f55534e80b633bdb7bfc91eaa5793021d6bf66` | `C:/Source/repos/postgres/postgres/` |
| RavenDB | `83399cb8bc866c118615b714a641191ff0764dec` | `C:/Source/repos/ravendb/ravendb/` |
| Neo4j | `54a7dcf7c2501b31866199143364c5332da8936f` | `C:/Source/repos/neo4j/neo4j/` |

In task citations, `Cohesion Database/Sql/...` expands to `C:/cw/bench/cohesion/resources/Database/Assimalign.Cohesion.Database.Sql/...`; other database modules use the same prefix. The JSON keeps full absolute paths. Benchmark/report and raw-result paths are preserved in `generatedFrom.evidence`. Their historical run metadata is retained; `benchmarksCommit` names the analysis base, not a claim that every historical run used that commit. The local issue dump includes only the first 1,200 body characters; duplicate decisions use its explicit titles/scope and current source. Any broader scope revealed when the orchestrator opens an issue should be reconciled before filing.

## Coverage of the eight starting symptoms

| Symptom | Confirmed evidence and explanation | Inventory route |
|---|---|---|
| 1. SQL two-table order lookup | `sql.order-lines` p50 280.319 ms embedded / 93.311 ms TCP versus PostgreSQL 1.208 ms: the embedded observation is 232.1x. The executor scans the outer table and forms joined rows before WHERE. Inner index use alone cannot fix the outer scan. | F02.T01; costed joins F02.T03 |
| 2. SQL UPDATE/DELETE target scans | Both paths call `Scan` despite an indexed predicate. `sql.tx.ship-order` p50 is 13.031 ms embedded versus PostgreSQL 3.605 ms; `sql.update-customer` is 5.019 versus 1.373 ms. Those statement/transaction timings include other work and do not isolate scan time. | F02.T02, existing #1274 |
| 3. Documents point reads and negative scaling | Linux R3 throughput 128.1828 ops/s at one client and 43.6530 at eight; all operations in those rows succeeded. `1/128.1828` is 7.8029 ms per completion, **not p50**. The separate latency-only p50 is 1.001 ms; the one-client throughput phase p50 is 5.303 ms. R6 measured commit/flush dominance but did not reproduce that exact slope. | F01.T01/T02; later lock/allocation work F04.T02/T03 and F07.T01 |
| 4. Documents status/date query | Windows p50 76.543 ms versus RavenDB 3.871 ms (19.8x); Linux Cohesion observation 146.175 ms. Equality is preferred to range without selectivity, so residual date filtering parses every chosen candidate. Content validation and executor parsing duplicate JSON work. | F04.T01/T02; compound indexes #1135 |
| 5. Graph two/three-hop bounds | R4 reproduced COHDBG004 for 100,000 completed matches and 1,000,000 candidate expansions at SF 0.01. Incident edges are gathered and charged before type/direction filtering; genuine qualifying fan-out can still exceed the match cap. | F05.T01/T02; native quantified paths #1144 |
| 6. Graph TCP negative scaling | Recent-messages throughput 263.0218 → 38.3197 ops/s from one to eight clients (6.86x lower). R6 reproduced a smaller 416.97 → 377.00 decline; at eight clients, journal flushes consumed 5.609 s of a 6.053 s interval and aggregate monitor wait was 37.282 s. Framing/path allocation adds a separate measured cost. | F01.T01/T02 and F06.T02; native top-N #1142 |
| 7. Embedded graph process exhausted 2 GiB | Open. A combined benchmark-process failure does not establish an engine leak. R5 owns attribution. R6 did not rerun or duplicate that investigation; source-inspected eager storage/execution tasks are independently justified. | Deferred; not-filed list |
| 8. Functional gaps | N-way/outer joins, CTEs, windows, correlated subqueries, temporal functions, DISTINCT aggregates, ingest, OQL paging/arrays/indexes/patch/batch get, and GQL paging/aggregates/parameters/paths/predicates/transactions/indexes are all mapped below. Additional baseline T1 exclusions are listed as already covered. | F03–F08 and not-filed list |

The `target`/`competitor` strings in JSON include platform and client-count qualifiers when required by the fixed schema. Empty benchmark arrays mean no comparable numeric observation was supplied; unsupported operations and coded errors are not represented as zero latency.

## Feature and task summary

Priority is impact (`P001` highest); wave is dependency order (`W01` earliest). Effort estimates are relative: S is a local change, M a subsystem change, L crosses APIs/operators, and XL needs an execution, persistence or concurrency design. They are not delivery dates. Within each feature, the urgent bottlenecks precede later expansion work.


| Feature | Task | Category | Evidence headline | Priority | Wave / effort | Filing |
|---|---|---|---|---|---|---|
| F01 Shared read transactions and commit concurrency | [F01.T01](#f01t01) Elide WAL and durable flushes for read-only transactions | performance | Measured: a point read pays a durable commit; this shared path also limits graph TCP reads. | P001 | W01 / M | Reuse #1280 |
| F01 Shared read transactions and commit concurrency | [F01.T02](#f01t02) Release journal append synchronization while forcing durable bytes | scalability | Measured contention and code inspection: journal fsync blocks other appenders and propagates through transaction gates. | P001 | W01 / L | Reuse #1276 |
| F01 Shared read transactions and commit concurrency | [F01.T03](#f01t03) Permit independent model writers without a database-wide writer lock | scalability | Code inspection: Documents and Graph serialize whole writer transactions; the kernel also serializes physical statement application. | P003 | W04 / XL | New |
| F02 SQL access paths and bounded execution | [F02.T01](#f02t01) Push selective predicates into both SQL join inputs | performance | Smoke: sql.order-lines is 280.319 ms embedded versus PostgreSQL 1.208 ms (232.1x); TCP is 93.311 ms. | P001 | W01 / M | New |
| F02 SQL access paths and bounded execution | [F02.T02](#f02t02) Seek indexed UPDATE and DELETE targets | performance | Smoke and code inspection: every write statement scans; cleanup scans also extend total run time. | P001 | W01 / M | Reuse #1274 |
| F02 SQL access paths and bounded execution | [F02.T03](#f02t03) Cost SQL access paths and add a hash join for broad equijoins | performance | Code inspection: longest-prefix rules and nested loops cannot choose selective inputs or a linear broad join. | P002 | W03 / XL | New |
| F02 SQL access paths and bounded execution | [F02.T04](#f02t04) Stream SQL results and apply LIMIT through bounded top-N execution | memory | Code inspection: LIMIT follows complete matching, projection, sorting and a final output list. | P002 | W02 / L | New |
| F02 SQL access paths and bounded execution | [F02.T05](#f02t05) Check MVCC visibility before decoding SQL row values | performance | Code inspection: scans allocate and decode invisible row versions before testing their stamps. | P003 | W03 / M | New |
| F03 SQL analytical and transactional language coverage | [F03.T01](#f03t01) Execute N-way, LEFT, CROSS and USING joins | functional-gap | Existing T1 scope; three-table benchmark operations cannot execute. | P002 | W02 / L | Reuse #1109 |
| F03 SQL analytical and transactional language coverage | [F03.T02](#f03t02) Execute RIGHT and FULL OUTER joins | functional-gap | Code inspection: #1109 covers LEFT/CROSS/N-way but does not name RIGHT/FULL. | P004 | W04 / L | New |
| F03 SQL analytical and transactional language coverage | [F03.T03](#f03t03) Execute non-recursive common table expressions | functional-gap | Reuse #1116; category ranking and reusable analytical subplans need WITH. | P002 | W02 / L | Reuse #1116 |
| F03 SQL analytical and transactional language coverage | [F03.T04](#f03t04) Execute correlated subqueries and derived tables | functional-gap | Reuse #1113; above-country-average cannot be expressed. | P002 | W03 / XL | Reuse #1113 |
| F03 SQL analytical and transactional language coverage | [F03.T05](#f03t05) Execute SQL window ranking and aggregate functions | functional-gap | Uncovered execution feature; category-rank-window reports COHDBL001 for OVER. | P003 | W04 / XL | New |
| F03 SQL analytical and transactional language coverage | [F03.T06](#f03t06) Execute EXTRACT and the planned temporal scalar functions | functional-gap | Reuse #1120 rather than file separate date-function tickets. | P002 | W02 / L | Reuse #1120 |
| F03 SQL analytical and transactional language coverage | [F03.T07](#f03t07) Execute DISTINCT aggregate arguments | functional-gap | Reuse #1115 for COUNT(DISTINCT) and related aggregates. | P002 | W02 / L | Reuse #1115 |
| F03 SQL analytical and transactional language coverage | [F03.T08](#f03t08) Execute explicit NULLS FIRST and NULLS LAST | functional-gap | Existing #1112; do not duplicate while implementing top-N. | P003 | W02 / S | Reuse #1112 |
| F03 SQL analytical and transactional language coverage | [F03.T09](#f03t09) Execute SELECT without FROM | functional-gap | Existing #1373 includes transaction timestamps and scalar-only SELECT. | P004 | W02 / S | Reuse #1373 |
| F03 SQL analytical and transactional language coverage | [F03.T10](#f03t10) Execute SQL savepoints and isolation-level syntax | functional-gap | Reuse #1012; transaction-level recovery remains coarser than the planned SQL surface. | P004 | W04 / XL | Reuse #1012 |
| F04 Documents query execution, indexes and APIs | [F04.T01](#f04t01) Choose document access paths by selectivity and intersect indexed predicates | performance | Smoke: status/date p50 is 76.543 ms versus RavenDB 3.871 ms (19.8x). | P002 | W02 / L | New |
| F04 Documents query execution, indexes and APIs | [F04.T02](#f04t02) Read and project document JSON once per candidate | memory | Code inspection and probe allocation: point reads allocate about 4.5 KB per 1 KB document; OQL adds another parse and clone. | P003 | W03 / L | New |
| F04 Documents query execution, indexes and APIs | [F04.T03](#f04t03) Keep document catalog reads outside a single decode lock | scalability | Code inspection: point metadata lookup and full collection enumeration run under one catalog monitor. | P003 | W03 / L | New |
| F04 Documents query execution, indexes and APIs | [F04.T04](#f04t04) Execute OQL LIMIT and OFFSET with bounded execution | functional-gap | Reuse #1129; current recent-order queries return all rows for client trimming. | P002 | W02 / M | Reuse #1129 |
| F04 Documents query execution, indexes and APIs | [F04.T05](#f04t05) Execute array existential predicates and UNNEST | functional-gap | Reuse #1136; products-by-category-price is unsupported because filtering arrays in the client is forbidden. | P002 | W02 / L | Reuse #1136 |
| F04 Documents query execution, indexes and APIs | [F04.T06](#f04t06) Execute compound and unique document indexes | functional-gap | Reuse #1135; scalar single-path indexes cannot serve status plus date as one ordered range. | P002 | W02 / L | Reuse #1135 |
| F04 Documents query execution, indexes and APIs | [F04.T07](#f04t07) Index document array elements with multikey semantics | functional-gap | Code inspection: the compound-index T1 ticket does not supply multikey element postings. | P003 | W03 / L | New |
| F04 Documents query execution, indexes and APIs | [F04.T08](#f04t08) Execute atomic document patches through the existing OQL work | functional-gap | Reuse #1133; patch-order-status and adjust-variant-stock currently read and replace content. | P002 | W02 / L | Reuse #1133 |
| F04 Documents query execution, indexes and APIs | [F04.T09](#f04t09) Add ordered batch document reads in one snapshot | performance | Smoke: products-multiget p50 4.027 ms Windows / 20.223 ms Linux; RavenDB 1.572 ms. | P003 | W03 / M | New |
| F04 Documents query execution, indexes and APIs | [F04.T10](#f04t10) Expose Documents through its planned wire server and client | functional-gap | Existing #190 owns both server and client, not only a client wrapper. | P002 | W02 / L | Reuse #190 |
| F04 Documents query execution, indexes and APIs | [F04.T11](#f04t11) Accelerate document content checksums without changing persisted CRC semantics | performance | Code inspection only: every content read runs the byte-at-a-time IEEE CRC loop. | P004 | W03 / M | Reuse #1271 |
| F05 Graph query execution and indexing | [F05.T01](#f05t01) Filter graph adjacency by type and direction before candidate expansion | performance | R4: two/three-hop patterns hit 100,000 matches or 1,000,000 candidates at SF0.01. | P001 | W01 / L | New |
| F05 Graph query execution and indexing | [F05.T02](#f05t02) Stream graph bindings under explicit memory and work budgets | memory | Measured R4 bound errors; code inspection shows retained binding/path arrays before projection. | P001 | W02 / XL | New |
| F05 Graph query execution and indexing | [F05.T03](#f05t03) Add an MVCC label index for graph anchor scans | performance | R4 country-count observations: embedded p50 359.679 ms (run verification incomplete; diagnostic only), TCP 605.695 ms, Neo4j 12.079 ms. | P002 | W02 / L | New |
| F05 Graph query execution and indexing | [F05.T04](#f05t04) Persist graph identity lookup instead of rebuilding the complete record map | scalability | R4 observed 0.592 s open, 153.96 MiB process working set, 79.24 MiB managed; these are whole-process values. | P003 | W03 / XL | New |
| F05 Graph query execution and indexing | [F05.T05](#f05t05) Execute GQL ORDER BY, OFFSET and LIMIT | functional-gap | Reuse #1142; recent-messages currently transfers all paths and sorts/trims client-side. | P002 | W02 / L | Reuse #1142 |
| F05 Graph query execution and indexing | [F05.T06](#f05t06) Execute GQL grouping and aggregate expressions | functional-gap | Reuse #1147; country count and tag cooccurrence currently aggregate client-side. | P002 | W02 / L | Reuse #1147 |
| F05 Graph query execution and indexing | [F05.T07](#f05t07) Bind GQL parameters from APIs and the wire | functional-gap | Reuse #1138 and the shared #1102 parameter contract. | P002 | W02 / M | Reuse #1138 |
| F05 Graph query execution and indexing | [F05.T08](#f05t08) Execute variable-length graph paths with trail semantics | functional-gap | Reuse #1144; multi-depth benchmark emulation currently sends several fixed patterns. | P002 | W03 / XL | Reuse #1144 |
| F05 Graph query execution and indexing | [F05.T09](#f05t09) Expose shortest graph paths with their actual distance | functional-gap | Uncovered: the traversal API provides reachability but cannot supply correct per-target shortest distance for the workload. | P003 | W03 / L | New |
| F05 Graph query execution and indexing | [F05.T10](#f05t10) Execute NOT EXISTS graph pattern predicates | functional-gap | Uncovered anti-pattern feature; friend recommendation is currently unsupported. | P003 | W03 / L | New |
| F06 Query preparation and wire execution | [F06.T01](#f06t01) Cache parsed and bound query shapes with schema-aware invalidation | performance | Code inspection: SQL, OQL and GQL parse/plan on each text call; parameters do not provide server preparation. | P003 | W03 / L | New |
| F06 Query preparation and wire execution | [F06.T02](#f06t02) Batch result frames and remove repeated graph path serialization allocations | performance | Measured: Linux 20-path TCP reads allocate 615–623 KB/op versus about 227–229 KB embedded. | P002 | W02 / L | New |
| F06 Query preparation and wire execution | [F06.T03](#f06t03) Add explicit graph transactions over the wire | functional-gap | Uncovered transport capability; multiple fixed-pattern emulations cannot share a TCP snapshot. | P002 | W02 / L | New |
| F07 Storage, indexes and MVCC maintenance | [F07.T01](#f07t01) Remove page I/O from the global buffer-pool lock | scalability | Code inspection: both hits and miss reads serialize under one pool monitor. | P002 | W03 / XL | New |
| F07 Storage, indexes and MVCC maintenance | [F07.T02](#f07t02) Make B+Tree range cursors incremental and cancellable | memory | Code inspection: opening a cursor materializes every key and entry reference under a tree read latch. | P002 | W02 / L | New |
| F07 Storage, indexes and MVCC maintenance | [F07.T03](#f07t03) Prune dead secondary-index versions below the snapshot horizon | performance | Reuse #1195; current SQL index-maintenance worker remains a stub. | P002 | W02 / L | Reuse #1195 |
| F07 Storage, indexes and MVCC maintenance | [F07.T04](#f07t04) Reuse the unique-key descent for B+Tree insertion | performance | Reuse #1239; unique insertion still descends for HasLiveEntry and again for InsertCore. | P004 | W03 / M | Reuse #1239 |
| F07 Storage, indexes and MVCC maintenance | [F07.T05](#f07t05) Spread checkpoint I/O and admit readers during checkpoint progress | performance | Reuse #1269 and related #1283; checkpoint stalls are documented, not remeasured by R6. | P002 | W03 / XL | Reuse #1269 |
| F07 Storage, indexes and MVCC maintenance | [F07.T06](#f07t06) Bound MVCC prune discovery as well as mutation batches | memory | Code inspection: record purge batches writes but first copies every eligible tombstone candidate. | P004 | W04 / L | New |
| F07 Storage, indexes and MVCC maintenance | [F07.T07](#f07t07) Partition lock-manager synchronization while preserving fair queues | scalability | Code inspection only: release is already owner-indexed, but acquisition/release queues share one monitor. | P005 | W05 / L | New |
| F08 Durable bulk ingest paths | [F08.T01](#f08t01) Resolve graph schema definitions by identity without catalog-wide scans | performance | Code inspection: the quadratic catalog work from #1229 is still present and adds cost to graph ingest. | P003 | W02 / M | Reuse #1229 |
| F08 Durable bulk ingest paths | [F08.T02](#f08t02) Add a streaming SQL bulk-load protocol and API | load | Code inspection: the SQL client exposes query/execute operations but no bulk stream. | P003 | W03 / L | New |
| F08 Durable bulk ingest paths | [F08.T03](#f08t03) Add bounded bulk document writes and deferred index finalization | load | Code inspection: Documents ingest loops PutAsync; there is no batch content API. | P003 | W03 / L | New |
| F08 Durable bulk ingest paths | [F08.T04](#f08t04) Add a bulk graph importer with ordered adjacency and index construction | load | Code inspection: per-element APIs or multi-path INSERT are the only ingest paths. | P003 | W04 / XL | New |

### Dependency and sequencing notes

W01 removes read-only durability cost, decouples journal force, pushes selective SQL join predicates, adopts indexed DML and filters graph adjacency. W02 adds bounded results/ranges, core native language features and graph label access. W03 builds on these foundations for selectivity/plan caching, JSON and storage concurrency, persistent graph lookup and ingest. W04 carries the larger writer-concurrency, window, MVCC backlog and graph-import designs. W05 contains lock-table partitioning, gated on new profiling; no task is forced into W06 merely to fill every wave.

Internal dependencies: F01.T03 follows F01.T01/T02 and #1393; F04.T03 and F07.T01 must be profiled after F01.T01; F02.T04 and F05.T02 establish ownership/budget contracts used by pagination and aggregate tasks; F07.T02 coordinates with F07.T03 before cursor continuation references can be reclaimed; F06.T01 requires GQL parameters (#1138) for reusable graph shapes; F08.T04 builds on typed adjacency, label indexes and persistent identity mapping. A feature's wave denotes its first work, not completion of all its tasks. Relations in JSON are from the task's perspective: `blocked-by #n` means the referenced issue is a prerequisite; `blocks #n` would mean this task must land first. A related closed ancestor provides context, not proof of duplicate scope.

## Task details

### F01 — Shared read transactions and commit concurrency

Remove unnecessary read WAL first, then isolate flush synchronization and evaluate safe write concurrency. Existing kernel issues are reuse targets, not new tickets. Feature priority P001; starting wave W01.

<a id="f01t01"></a>

#### F01.T01 — Elide WAL and durable flushes for read-only transactions

**performance; P001; W01; effort M; reuse existing issue.** Measured: a point read pays a durable commit; this shared path also limits graph TCP reads.

**Root cause.** The read-only state is not used to suppress lifecycle WAL. Every successful autocommit read drains 76 bytes of begin/commit frames and normally forces the journal. GetAsync itself does not execute OQL; statement parsing cannot explain this read latency.

**Evidence classification:** Measurement and code inspection.

| Operation | Target | Scale | Metric | Value | Comparison | Comparison value |
|---|---|---:|---|---:|---|---:|
| `doc.order-by-id` | cohesion-documents-embedded (Linux, 8 clients) | 0.01 | opsPerSec | 43.653007071084005 | same target, 1 client | 128.1828414448442 |
| `graph.person-recent-messages` | cohesion-graph-tcp (8 clients) | 0.01 | opsPerSec | 38.32 | same target, 1 client | 263.022 |

**Measured evidence.** R6 Linux ARM64, 3 CPUs/2 GiB, 100 documents: explicit begin/read/commit averaged 0.0193/0.0268/8.1587 ms over 200 transactions, with 200 durable flushes. Autocommit at one client: 234.53 ops/s; 4.956 of 5.006 s inside the forwarding journal Flush wrapper, 1,174 flushes and 89,224 WAL bytes for 1,174 reads. A held snapshot fetched 175,837 reads/s with zero flushes (diagnostic amortization, different transaction boundaries). Windows 8-client stack: six workers at GatedJournalLog.AppendCommitAsync monitor entry, one at TransactionManager.BeginAsync monitor entry and one at Storage.AwaitCommitDurable; 207.96 aggregate contention-seconds over 30.03 s. Original Linux 128.2 -> 43.7 slope did not reproduce (R6 234.5/244.0/831.5 ops/s at 1/3/8); shared-host fsync latency varied. This proves the read-WAL/flush mechanism, not every millisecond of R3.

**Source.**

- `Cohesion Database/Documents/src/DocumentCollection.cs:75` — GetAsync uses RunAsync
- `Cohesion Database/Documents/src/DocumentDatabase.cs:314` — autocommit begins a Snapshot context
- `Cohesion Database/Documents/src/Internal/DocumentOperation.cs:74` — read operations commit their context
- `Cohesion Database/Graph/src/GraphDatabase.Lifetime.cs:32` — graph autocommit starts a Snapshot context
- `Cohesion Database/Graph/src/Internal/GraphOperation.cs:72` — graph read operations also commit
- `Cohesion Database/Transactions/src/TransactionCoordinator.cs:1082` — begin is always journaled
- `Cohesion Database/Transactions/src/TransactionCoordinator.cs:1122` — commit always appends and ensures durability

**Proposal.** Implement #1280 with a non-writing transaction/snapshot identity and lazy durable writer enrollment on first mutation. Preserve snapshot pinning, rollback classification and writer crash recovery in every model.

**Reference precedent.**

- `PostgreSQL/src/backend/access/transam/xact.c:1384` — transactions without an assigned XID avoid commit WAL; the no-XLOG exit is at 1429

**Acceptance.**

- Autocommit and explicit read-only transactions in SQL, Documents and Graph produce zero journal bytes and zero durable flushes.
- Crash/recovery suites preserve every acknowledged write and snapshot visibility when a read transaction becomes a writer.
- On a quiet 3 CPU host, Documents point-read throughput at 3 clients is at least 2.7x the 1-client result; report 8 clients separately and require no regression.

**Existing work.** #1280 (duplicate)

<a id="f01t02"></a>

#### F01.T02 — Release journal append synchronization while forcing durable bytes

**scalability; P001; W01; effort L; reuse existing issue.** Measured contention and code inspection: journal fsync blocks other appenders and propagates through transaction gates.

**Root cause.** One synchronous flush holds StorageJournal._syncRoot; appenders wait behind it while holding or waiting for GatedJournalLog._gate. More callers increase waiters rather than independent disk work. The old recovery-reader half of #1276 has already changed (ReadSequential uses buffered reads); only the live flush coupling is asserted here.

**Evidence classification:** Measurement and code inspection.

| Operation | Target | Scale | Metric | Value | Comparison | Comparison value |
|---|---|---:|---|---:|---|---:|
| `doc.order-by-id` | cohesion-documents-embedded (Linux, 8 clients) | 0.01 | opsPerSec | 43.653007071084005 | same target, 1 client | 128.1828414448442 |
| `graph.person-recent-messages` | cohesion-graph-tcp (8 clients) | 0.01 | opsPerSec | 38.32 | same target, 1 client | 263.022 |

**Measured evidence.** R6 Linux ARM64, 3 CPUs/2 GiB, 100 documents: explicit begin/read/commit averaged 0.0193/0.0268/8.1587 ms over 200 transactions, with 200 durable flushes. Autocommit at one client: 234.53 ops/s; 4.956 of 5.006 s inside the forwarding journal Flush wrapper, 1,174 flushes and 89,224 WAL bytes for 1,174 reads. A held snapshot fetched 175,837 reads/s with zero flushes (diagnostic amortization, different transaction boundaries). Windows 8-client stack: six workers at GatedJournalLog.AppendCommitAsync monitor entry, one at TransactionManager.BeginAsync monitor entry and one at Storage.AwaitCommitDurable; 207.96 aggregate contention-seconds over 30.03 s. Original Linux 128.2 -> 43.7 slope did not reproduce (R6 234.5/244.0/831.5 ops/s at 1/3/8); shared-host fsync latency varied. This proves the read-WAL/flush mechanism, not every millisecond of R3. R6 Linux fixed 20-path recent-messages query, one engine plus loopback TCP client in one 3 CPU/2 GiB process: TCP paths 416.97/604.66/377.00 ops/s at 1/3/8 clients. At 8 clients, 2,282 reads generated 2,174 durable flushes, 5.609 s in flush calls over 6.053 s wall time, 37.282 aggregate contention-seconds, 20 peak pool threads and 3 peak queued work items. Durability=None diagnostic: 4,432.75 ops/s at 8 clients and zero durable flushes; this is not an eligible benchmark result or a proposed durability setting. The smaller negative slope reproduces, but the original 263.022 -> 38.320 magnitude is not attributed fully.

**Source.**

- `Cohesion Database/Storage/src/Journal/StorageJournal.cs:770` — the journal monitor covers DrainLocked(durable:true)
- `Cohesion Database/Storage/src/Journal/StorageJournal.cs:1397` — FlushCore synchronously invokes the file handle
- `Cohesion Database/Transactions/src/TransactionCoordinator.cs:1127` — the lifecycle gate encloses AppendCommit, which can wait for the journal monitor

**Proposal.** Reuse #1276: separate append and force coordination, publish written/durable LSN watermarks, and coalesce waiters for a common durable prefix. Keep fsync-on-ack, cancellation and offline transitions intact; do not switch benchmark durability to None.

**Reference precedent.**

- `Neo4j/community/kernel/src/main/java/org/neo4j/wal/files/TransactionLogFile.java:1194` — force prepares the writer under synchronization and forces outside the lock at 1204

**Acceptance.**

- An instrumented blocked force does not block appending subsequent WAL into available buffers.
- Concurrent durable commits acknowledge only LSNs proven durable; forced-failure and restart tests pass.
- The 8-client read/write probe reports append-gate wait, force wait and flushes/commit independently; synchronous 3-client distinct-key writes improve at least 2x over this baseline after F01.T03.

**Existing work.** #1276 (duplicate)

<a id="f01t03"></a>

#### F01.T03 — Permit independent model writers without a database-wide writer lock

**scalability; P003; W04; effort XL; file as new.** Code inspection: Documents and Graph serialize whole writer transactions; the kernel also serializes physical statement application.

**Root cause.** Model-wide exclusive ownership lasts until transaction completion, including durable commit. Removing just that lock still leaves the per-database apply gate; removing either without a new page/index conflict protocol would reintroduce deadlocks or rollback corruption.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Documents/src/DocumentDatabase.cs:377` — LockWriterAsync acquires an exclusive database resource
- `Cohesion Database/Graph/src/GraphDatabase.Lifetime.cs:122` — graph writers acquire the exclusive database resource too
- `Cohesion Database/Transactions/src/TransactionCoordinator.cs:37` — the documented bracket model serializes application
- `Cohesion Database/Transactions/src/TransactionCoordinator.cs:511` — every physical apply waits on one semaphore

**Proposal.** After F01.T01/T02, design per-document/per-node/key logical locks and short page latches with retryable physical application. Retain a catalog gate for schema changes and define lock order, endpoint deletion and index rollback before implementation. Start with disjoint-key Documents writes; expand to Graph only after crash validation.

**Reference precedent.**

- `PostgreSQL/src/backend/storage/lmgr/lock.c:2338` — LockReleaseAll manages backend-owned lock entries
- `PostgreSQL/src/backend/access/nbtree/nbtinsert.c:190` — unique insertion coordinates concurrent writers at the index leaf/key conflict

**Acceptance.**

- Disjoint-key writes at 3 clients achieve at least 2x 1-client throughput with synchronous durability on the controlled fixture.
- Same-key writes preserve conflict detection and exactly one visible committed version; cancellation, deadlock, checkpoint and restart tests pass.

**Existing work.** #1393 (blocked-by); #1195 (related)

### F02 — SQL access paths and bounded execution

Fix selective joins and DML first, then reduce scan, sorting and materialization costs. Feature priority P001; starting wave W01.

<a id="f02t01"></a>

#### F02.T01 — Push selective predicates into both SQL join inputs

**performance; P001; W01; effort M; file as new.** Smoke: sql.order-lines is 280.319 ms embedded versus PostgreSQL 1.208 ms (232.1x); TCP is 93.311 ms.

**Root cause.** The planner can choose an index on the inner ON key, but the executor cannot seek or filter the outer input by the order-id predicate. It scans every outer row, probes/materializes matches and only then tests WHERE; if no usable inner index exists it evaluates the Cartesian candidate set.

**Evidence classification:** Development smoke and code inspection.

| Operation | Target | Scale | Metric | Value | Comparison | Comparison value |
|---|---|---:|---|---:|---|---:|
| `sql.order-lines` | cohesion-sql-embedded | 0.01 | p50Ms | 280.319 | postgresql | 1.208 |
| `sql.order-lines` | cohesion-sql-tcp | 0.01 | p50Ms | 93.311 | postgresql | 1.208 |

**Source.**

- `Cohesion Database/Sql/src/Internal/SqlPlanExecutor.Joins.cs:30` — WHERE runs after join candidate construction
- `Cohesion Database/Sql/src/Internal/SqlPlanExecutor.Joins.cs:64` — the outer table always scans
- `Cohesion Database/Sql/src/Internal/SqlPlanExecutor.Joins.cs:86` — each pair allocates a joined row before filtering

**Proposal.** Bind per-relation restrictions, choose an outer access path using the existing equality/range seek rules, propagate safe equijoin constants, and apply residual filters before pair allocation. Keep ON and WHERE semantics separate for future outer joins.

**Reference precedent.**

- `PostgreSQL/src/backend/optimizer/plan/initsplan.c:3261` — singleton restrictions attach to baserestrictinfo; append at 3315

**Acceptance.**

- The order-id predicate produces an outer key seek and inner order-lines seek; records examined grows with returned lines, not total orders.
- At SF10 after verification, sql.order-lines p50 is within 2x PostgreSQL under the shared settings.
- NULLs, duplicate keys, mixed numeric types and detached-index fallback preserve result digests.

**Existing work.** #1019 (related); #1109 (related)

<a id="f02t02"></a>

#### F02.T02 — Seek indexed UPDATE and DELETE targets

**performance; P001; W01; effort M; reuse existing issue.** Smoke and code inspection: every write statement scans; cleanup scans also extend total run time.

**Root cause.** DML target discovery does not reuse SELECT access paths, so a primary-key update costs O(table versions) before row locking, cloning and index maintenance. Place-order repeats this for each inventory line; rollback cleanup repeats the same cost outside timers.

**Evidence classification:** Development smoke and code inspection.

| Operation | Target | Scale | Metric | Value | Comparison | Comparison value |
|---|---|---:|---|---:|---|---:|
| `sql.tx.ship-order` | cohesion-sql-embedded | 0.01 | p50Ms | 13.031 | postgresql | 3.605 |
| `sql.update-customer` | cohesion-sql-embedded | 0.01 | p50Ms | 5.019 | postgresql | 1.373 |

**Source.**

- `Cohesion Database/Sql/src/Internal/SqlPlanExecutor.cs:360` — UPDATE target discovery calls Scan
- `Cohesion Database/Sql/src/Internal/SqlPlanExecutor.cs:472` — DELETE target discovery calls Scan
- `Cohesion Database/Sql/docs/DESIGN.md:1656` — DML scan limitation

**Proposal.** Implement #1274 by planning an index seek/range for DML, collecting stable target identities before mutation, and rechecking conflict/visibility after locks. Preserve Halloween protection and FK/unique maintenance.

**Reference precedent.**

- `PostgreSQL/src/backend/executor/nodeModifyTable.c:4497` — ModifyTable consumes its child plan via ExecProcNode, allowing indexed target access

**Acceptance.**

- 50,000-row key UPDATE/DELETE costs at most 2x the same operation at 1,000 rows, with rows examined bounded by matching targets.
- Range updates that change their own indexed key update each original target once; rollback and FK tests pass.

**Existing work.** #1274 (duplicate)

<a id="f02t03"></a>

#### F02.T03 — Cost SQL access paths and add a hash join for broad equijoins

**performance; P002; W03; effort XL; file as new.** Code inspection: longest-prefix rules and nested loops cannot choose selective inputs or a linear broad join.

**Root cause.** Index presence and syntactic prefix length are used as cost proxies; neither cardinality nor distribution determines the outer side or join algorithm.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql/src/Internal/SqlPlanner.cs:374` — single-table access selection uses rule-based equality/range prefixes
- `Cohesion Database/Sql/src/Internal/SqlPlanExecutor.Joins.cs:44` — unindexed join evaluates O(L*R) candidate pairs
- `Cohesion Database/Sql/docs/DESIGN.md:1583` — planner has no statistics/cost model

**Proposal.** Add persisted row counts, distinct-count/selectivity samples and ANALYZE/refresh policy; estimate base and join cardinality; choose scan, index nested loop or bounded hash join. Add partition spill before enabling hash joins beyond the memory grant. F02.T01 precedes costing.

**Reference precedent.**

- `PostgreSQL/src/backend/optimizer/path/costsize.c:546` — cost_index estimates rows and I/O from selectivity
- `PostgreSQL/src/backend/executor/nodeHashjoin.c:180` — hybrid hash join implementation and memory partitioning

**Acceptance.**

- EXPLAIN (#1104) shows estimated/actual candidate counts and chosen join algorithm.
- A 10x increase in both inputs of a broad equijoin avoids a 100x predicate count; spill stays within a configured grant.
- Benchmarks retain identical indexes and run statistics maintenance within documented setup.

**Existing work.** #1104 (blocked-by); #1109 (related)

<a id="f02t04"></a>

#### F02.T04 — Stream SQL results and apply LIMIT through bounded top-N execution

**memory; P002; W02; effort L; file as new.** Code inspection: LIMIT follows complete matching, projection, sorting and a final output list.

**Root cause.** Memory and time are proportional to all qualifying rows even when ten are returned. Per-row object arrays, boxing and duplicated source/output collections add allocation and GC load.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql/src/Internal/SqlPlanExecutor.cs:134` — all matching source arrays accumulate
- `Cohesion Database/Sql/src/Internal/SqlPlanExecutor.cs:157` — projection and full sorting precede LIMIT
- `Cohesion Database/Sql/src/Internal/SqlPlanExecutor.cs:185` — the window materializes another list

**Proposal.** Introduce pull/batch operators with explicit snapshot ownership, early termination for unordered LIMIT, order-preserving index scans, bounded top-N heaps and spillable sort/aggregation. Carry only needed columns and release buffers on cancellation.

**Reference precedent.**

- `PostgreSQL/src/backend/utils/sort/tuplesort.c:157` — bounded heap state
- `PostgreSQL/src/backend/utils/sort/tuplesort.c:737` — sort bound configuration
- `PostgreSQL/src/backend/executor/nodeLimit.c:39` — separate LIMIT executor node

**Acceptance.**

- An indexed ORDER BY LIMIT 10 visits at most offset+10 qualifying rows where semantics permit.
- Unindexed top-N retains O(N) rows plus bounded operator buffers, and a full sort spills within its grant.
- SF10 limited-query peak managed bytes falls at least 80% versus the eager baseline, with identical digests.

**Existing work.** #1100 (related); #1112 (related)

<a id="f02t05"></a>

#### F02.T05 — Check MVCC visibility before decoding SQL row values

**performance; P003; W03; effort M; file as new.** Code inspection: scans allocate and decode invisible row versions before testing their stamps.

**Root cause.** Rows hidden by a held snapshot or committed deletion still pay DecodeRow and object-array costs. This worsens aging workloads independently of target-selection scans.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql/src/Internal/SqlPlanExecutor.cs:1413` — visibility filtering consumes already-decoded versions
- `Cohesion Database/Sql/src/Internal/SqlPlanExecutor.cs:1452` — ScanVersions decodes every encountered record
- `Cohesion Database/Transactions/src/TransactionSnapshot.cs:78` — visibility can be decided from writer/deleter stamps

**Proposal.** Add a validated stamp/header read, skip invisible versions before value decoding, and decode only referenced columns for visible rows. Keep corruption checks and snapshot pinning intact; index-build scans still need every required historical version.

**Reference precedent.**

- `PostgreSQL/src/backend/access/heap/heapam.c:1019` — heap scan applies HeapTupleSatisfiesVisibility before returning the tuple

**Acceptance.**

- With 90% invisible versions, decoded-value count equals visible rows, and allocated bytes fall at least 70%.
- Old-snapshot reads and corrupt-record diagnostics remain correct while purge runs.

**Existing work.** #1361 (related); #1362 (related)

### F03 — SQL analytical and transactional language coverage

Reuse the T1 tickets for covered syntax; only window execution and remaining outer joins need new scope. Feature priority P002; starting wave W02.

<a id="f03t01"></a>

#### F03.T01 — Execute N-way, LEFT, CROSS and USING joins

**functional-gap; P002; W02; effort L; reuse existing issue.** Existing T1 scope; three-table benchmark operations cannot execute.

**Root cause.** Only two stored tables with INNER JOIN ON execute.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql.Language/docs/DIALECT.md:55` — Only two stored tables with INNER JOIN ON execute.

**Proposal.** Implement #1109 with a join tree and correct null extension; use the access and filter work in F02.T01.

**Reference precedent.**

- `PostgreSQL/src/backend/executor/nodeNestloop.c:154` — nested loop handles unmatched outer rows

**Acceptance.**

- Three-table benchmark queries become native and verify against PostgreSQL.
- LEFT/USING/CROSS cases preserve duplicates, nulls and ON-versus-WHERE semantics.

**Existing work.** #1109 (duplicate)

<a id="f03t02"></a>

#### F03.T02 — Execute RIGHT and FULL OUTER joins

**functional-gap; P004; W04; effort L; file as new.** Code inspection: #1109 covers LEFT/CROSS/N-way but does not name RIGHT/FULL.

**Root cause.** The executable join subset excludes RIGHT and FULL.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql.Language/docs/DIALECT.md:55` — The executable join subset excludes RIGHT and FULL.

**Proposal.** After #1109, normalize RIGHT to LEFT where possible and add FULL unmatched-side tracking in the physical join operators; preserve three-valued ON/WHERE semantics.

**Reference precedent.**

- `PostgreSQL/src/backend/executor/nodeHashjoin.c:180` — hash join implementation supports outer join processing

**Acceptance.**

- RIGHT and FULL match PostgreSQL for empty sides, duplicate keys, null keys and unmatched rows.
- EXPLAIN identifies normalization or the full-join physical operator.

**Existing work.** #1109 (blocked-by)

<a id="f03t03"></a>

#### F03.T03 — Execute non-recursive common table expressions

**functional-gap; P002; W02; effort L; reuse existing issue.** Reuse #1116; category ranking and reusable analytical subplans need WITH.

**Root cause.** WITH/CTEs are not executable.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql.Language/docs/DIALECT.md:62` — WITH/CTEs are not executable.

**Proposal.** Implement #1116 with scoped names and explicit inline/materialized choices; compose with the window task.

**Reference precedent.**

- `PostgreSQL/src/backend/parser/parse_cte.c:110` — WITH-clause analysis and namespace handling

**Acceptance.**

- Non-recursive CTEs execute in SELECT and DML with PostgreSQL-equivalent scope and digests.

**Existing work.** #1116 (duplicate)

<a id="f03t04"></a>

#### F03.T04 — Execute correlated subqueries and derived tables

**functional-gap; P002; W03; effort XL; reuse existing issue.** Reuse #1113; above-country-average cannot be expressed.

**Root cause.** Correlated subqueries and derived tables are excluded.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql.Language/docs/DIALECT.md:971` — Correlated subqueries and derived tables are excluded.

**Proposal.** Implement #1113 with parameterized subplans and decorrelation where valid; avoid substituting client-side joins.

**Reference precedent.**

- `PostgreSQL/src/backend/executor/nodeSubplan.c:59` — subplan evaluation within expressions

**Acceptance.**

- sql.a.above-country-average verifies natively at SF0.01 and SF10.
- Correlated EXISTS, IN and scalar-cardinality errors preserve SQL NULL semantics.

**Existing work.** #1113 (duplicate)

<a id="f03t05"></a>

#### F03.T05 — Execute SQL window ranking and aggregate functions

**functional-gap; P003; W04; effort XL; file as new.** Uncovered execution feature; category-rank-window reports COHDBL001 for OVER.

**Root cause.** Window functions do not execute.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql.Language/docs/DIALECT.md:63` — Window functions do not execute.

**Proposal.** Add a window AST, partition/order binding and operators for ROW_NUMBER, RANK, DENSE_RANK and aggregate windows with ROWS frames, then RANGE frames with explicit type semantics. Coordinate WITH support through #1116 and bounded storage through F02.T04.

**Reference precedent.**

- `PostgreSQL/src/backend/executor/nodeWindowAgg.c:6` — WindowAgg requires ordered partitions and uses a tuplestore

**Acceptance.**

- sql.a.category-rank-window becomes native and matches PostgreSQL, including ties and empty partitions.
- Large partitions spill under a memory grant; frame boundaries and NULL ordering have conformance tests.

**Existing work.** #1116 (blocked-by); #1112 (blocked-by); #1096 (related)

<a id="f03t06"></a>

#### F03.T06 — Execute EXTRACT and the planned temporal scalar functions

**functional-gap; P002; W02; effort L; reuse existing issue.** Reuse #1120 rather than file separate date-function tickets.

**Root cause.** EXTRACT and several recognized date functions are not executable built-ins.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql.Language/docs/DIALECT.md:1572` — EXTRACT and several recognized date functions are not executable built-ins.

**Proposal.** Implement the T1 function table with validated signatures, transaction-start time and typed temporal arithmetic; reuse the function ABI work.

**Reference precedent.**

- `PostgreSQL/src/backend/utils/adt/timestamp.c:5793` — timestamp and interval function implementation

**Acceptance.**

- Revenue-by-month runs without client computation and matches PostgreSQL over month/year boundaries.
- EXTRACT, CURRENT_DATE/TIMESTAMP and interval arithmetic have timezone and null tests.

**Existing work.** #1120 (duplicate); #1358 (related); #1373 (related)

<a id="f03t07"></a>

#### F03.T07 — Execute DISTINCT aggregate arguments

**functional-gap; P002; W02; effort L; reuse existing issue.** Reuse #1115 for COUNT(DISTINCT) and related aggregates.

**Root cause.** Aggregate DISTINCT/ALL arguments are excluded.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql.Language/docs/DIALECT.md:928` — Aggregate DISTINCT/ALL arguments are excluded.

**Proposal.** Implement #1115 with per-group distinct state, shared equality/collation semantics and bounded spill.

**Reference precedent.**

- `PostgreSQL/src/backend/executor/nodeAgg.c:33` — DISTINCT/ORDER BY aggregate input processing

**Acceptance.**

- COUNT(DISTINCT) ignores NULL and collapses duplicates under the bound collation.
- SF10 aggregate state stays within its grant and spills deterministically.

**Existing work.** #1115 (duplicate)

<a id="f03t08"></a>

#### F03.T08 — Execute explicit NULLS FIRST and NULLS LAST

**functional-gap; P003; W02; effort S; reuse existing issue.** Existing #1112; do not duplicate while implementing top-N.

**Root cause.** Explicit null ordering is rejected.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql.Language/docs/DIALECT.md:52` — Explicit null ordering is rejected.

**Proposal.** Implement #1112 and propagate null ordering through comparison, index-order compatibility and top-N.

**Reference precedent.**

- `PostgreSQL/src/backend/utils/sort/tuplesortvariants.c:231` — sort keys carry explicit nulls-first flags

**Acceptance.**

- ASC/DESC with both NULL placements match PostgreSQL in full sort and top-N.

**Existing work.** #1112 (duplicate)

<a id="f03t09"></a>

#### F03.T09 — Execute SELECT without FROM

**functional-gap; P004; W02; effort S; reuse existing issue.** Existing #1373 includes transaction timestamps and scalar-only SELECT.

**Root cause.** The planner requires a FROM relation.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql.Language/docs/DIALECT.md:42` — The planner requires a FROM relation.

**Proposal.** Reuse #1373 with a one-row input and no implicit storage scan.

**Reference precedent.**

- `PostgreSQL/src/backend/executor/nodeResult.c:101` — Result handles constant target lists without an outer plan

**Acceptance.**

- SELECT 1 and temporal/function-only projections return one correct row without storage access.

**Existing work.** #1373 (duplicate)

<a id="f03t10"></a>

#### F03.T10 — Execute SQL savepoints and isolation-level syntax

**functional-gap; P004; W04; effort XL; reuse existing issue.** Reuse #1012; transaction-level recovery remains coarser than the planned SQL surface.

**Root cause.** The exposed transaction path offers begin/commit/rollback, without a statement undo boundary that implements savepoints.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql/docs/DESIGN.md:2980` — savepoint and isolation syntax exclusions

**Proposal.** Implement #1012 with coherent logical/index/WAL savepoint markers and failure-state rules; preserve the current all-or-nothing behavior until complete.

**Reference precedent.**

- `PostgreSQL/src/backend/access/transam/xact.c:4426` — transaction/subtransaction state machine

**Acceptance.**

- SAVEPOINT, ROLLBACK TO and RELEASE preserve prior changes, locks and restart correctness.

**Existing work.** #1012 (duplicate)

### F04 — Documents query execution, indexes and APIs

Choose selective access, avoid redundant JSON work, and reuse OQL T1 coverage while adding multikey and batch APIs. Feature priority P002; starting wave W02.

<a id="f04t01"></a>

#### F04.T01 — Choose document access paths by selectivity and intersect indexed predicates

**performance; P002; W02; effort L; file as new.** Smoke: status/date p50 is 76.543 ms versus RavenDB 3.871 ms (19.8x).

**Root cause.** The status equality index wins irrespective of how narrow the date range is; same-kind paths tie by index name. The planner neither costs candidates nor intersects the two indexes. The source explains amplification, but no percentage of the original 76.543 ms is claimed from profiling.

**Evidence classification:** Development smoke and code inspection.

| Operation | Target | Scale | Metric | Value | Comparison | Comparison value |
|---|---|---:|---|---:|---|---:|
| `doc.orders-status-range` | cohesion-documents-embedded (Windows) | 0.01 | p50Ms | 76.543 | ravendb | 3.871 |

**Source.**

- `Cohesion Database/Documents/src/Internal/DocumentPlanner.cs:132` — one index is chosen; equality wins over range at 165
- `Cohesion Database/Documents/src/Internal/DocumentPlanExecutor.cs:41` — every selected candidate is fetched and parsed

**Proposal.** Collect per-path cardinality/selectivity estimates; select the cheapest equality/range input or intersect sorted identity streams before reading JSON. Add compound equality-prefix/range support after #1135.

**Reference precedent.**

- `RavenDB/src/Corax/Querying/Matches/BinaryMatch.Boolean.cs:129` — AND intersects result sets; counts inform confidence and mode at 165

**Acceptance.**

- A fixture with common status and rare dates fetches no more than 2x the selective date candidate count.
- All status/date queries verify; SF10 p50 is within 2x RavenDB once compared on controlled supported paths.
- EXPLAIN reports chosen index/intersection and candidate counts after #1104.

**Existing work.** #1135 (related); #1104 (related)

<a id="f04t02"></a>

#### F04.T02 — Read and project document JSON once per candidate

**memory; P003; W03; effort L; file as new.** Code inspection and probe allocation: point reads allocate about 4.5 KB per 1 KB document; OQL adds another parse and clone.

**Root cause.** Persisted content is reconstructed, CRC checked and parsed for validation on every read. OQL then reparses it and clones matches into retained JSON trees, followed by projection and sorting. This is distinct from the dominant durable flush.

**Evidence classification:** Measurement and code inspection.

| Operation | Target | Scale | Metric | Value | Comparison | Comparison value |
|---|---|---:|---|---:|---|---:|
| `doc.orders-status-range` | cohesion-documents-embedded (Windows) | 0.01 | p50Ms | 76.543 | ravendb | 3.871 |

**Measured evidence.** R6 Linux docs-auto c=1 allocated 4,510.8 bytes/op; shared-snapshot reads allocated 4,223.2 bytes/op. Allocation totals are measured; type-level dominance follows source inspection, not a sampled allocation trace.

**Source.**

- `Cohesion Database/Documents.Storage/src/DocumentStorage.Content.cs:18` — ValidateContent constructs a JsonDocument
- `Cohesion Database/Documents.Storage/src/DocumentStorage.Content.cs:59` — ReadContent allocates all bytes and validates them again
- `Cohesion Database/Documents/src/Internal/DocumentPlanExecutor.cs:55` — OQL parses that content again and clones matching roots
- `Cohesion Database/Documents/src/Internal/DocumentPlanExecutor.cs:105` — output rows sort after full materialization

**Proposal.** Introduce an internal validated read lease with a single JSON parse shared by predicate/projection, direct projection of needed values, and bounded result buffers. Keep corruption detection and public byte-memory lifetime guarantees; do not silently return borrowed page memory. Consider a versioned binary JSON representation only after measuring the simpler single-parse path.

**Reference precedent.**

- `RavenDB/src/Raven.Server/Documents/DocumentsStorage.cs:1765` — document reads expose BlittableJsonReaderObject over stored table bytes

**Acceptance.**

- OQL instrumentation records at most one JSON parse per fetched candidate and no whole-document clone for scalar projections.
- Point-read allocated bytes for the 1 KB fixture fall at least 40%; OQL allocation falls at least 50%, with unchanged checksums/digests.
- Disposed/cancelled results release every lease and preserve snapshot lifetime.

**Existing work.** #1271 (related); #1129 (related)

<a id="f04t03"></a>

#### F04.T03 — Keep document catalog reads outside a single decode lock

**scalability; P003; W03; effort L; file as new.** Code inspection: point metadata lookup and full collection enumeration run under one catalog monitor.

**Root cause.** Catalog identity maps and version resolution share an exclusive monitor. After read WAL is removed, concurrent metadata decoding and collection enumeration can serialize otherwise independent readers. Probe throughput in held snapshots rises from 1 to 3 clients then falls at 8, but this alone does not isolate catalog versus buffer-pool locks.

**Evidence classification:** Measurement and code inspection.

**Measured evidence.** R6 Linux held-snapshot point reads: 175,837/440,401/270,890 ops/s at 1/3/8 clients. This diagnostic removes per-read commits; it is evidence of a remaining concurrency ceiling, not proof of this particular lock.

**Source.**

- `Cohesion Database/Documents.Catalog/src/DocumentCatalog.cs:93` — FindCollection holds _sync while resolving visible versions
- `Cohesion Database/Documents.Catalog/src/DocumentCatalog.cs:195` — FindDocument holds the same monitor
- `Cohesion Database/Documents.Catalog/src/DocumentCatalog.cs:215` — enumeration filters and sorts all document keys under that monitor

**Proposal.** After F01.T01, instrument catalog and buffer lock waits separately; snapshot immutable map entries under short locks and resolve versions under page latches, or partition identity maps by collection/key. Add an ordered per-collection identity index for prefix iteration.

**Reference precedent.**

- `PostgreSQL/src/backend/storage/buffer/bufmgr.c:2484` — partitioned buffer mapping locks protect short map operations
- `RavenDB/src/Raven.Server/Documents/DocumentsStorage.cs:932` — prefix queries use bounded start/take traversal

**Acceptance.**

- Catalog lock time excludes storage reads, JSON decoding and complete-key sorting.
- At fixed durable semantics after F01.T01, point reads scale at least 2.7x from 1 to 3 clients and do not regress at 8; report lock wait attribution.
- Concurrent collection drop/replace and old-snapshot lookups retain existing semantics.

**Existing work.** #1280 (blocked-by)

<a id="f04t04"></a>

#### F04.T04 — Execute OQL LIMIT and OFFSET with bounded execution

**functional-gap; P002; W02; effort M; reuse existing issue.** Reuse #1129; current recent-order queries return all rows for client trimming.

**Root cause.** No LIMIT/OFFSET syntax or plan operator exists.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Documents.Language/docs/DESIGN.md:52` — unsupported OQL clauses and expressions
- `Cohesion Database/Documents/src/Internal/DocumentPlanExecutor.cs:105` — sorting is eager

**Proposal.** Implement #1129, with index-order early stop and bounded top-N rather than only slicing a full output list.

**Reference precedent.**

- `RavenDB/src/Raven.Server/Documents/Queries/Dynamic/CollectionQueryRunner.cs:158` — query Start/Take are passed into execution

**Acceptance.**

- Native pagination matches existing normalized results including ties and parameter validation.
- LIMIT 10 has O(10) retained result state when no full sort is necessary.

**Existing work.** #1129 (duplicate)

<a id="f04t05"></a>

#### F04.T05 — Execute array existential predicates and UNNEST

**functional-gap; P002; W02; effort L; reuse existing issue.** Reuse #1136; products-by-category-price is unsupported because filtering arrays in the client is forbidden.

**Root cause.** Fixed array subscripts exist; ANY/SOME/EVERY and UNNEST do not.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Documents.Language/docs/DESIGN.md:55` — UNNEST is unsupported
- `Cohesion Database/Documents.Language/docs/DESIGN.md:56` — ANY/SOME/EVERY quantified predicates are unsupported

**Proposal.** Implement #1136 with SQL++ MISSING/NULL semantics and a correlated element binding; coordinate index plans with multikey support.

**Reference precedent.**

- `RavenDB/src/Raven.Server/Documents/Indexes/Persistence/Corax/CoraxDocumentConverterBase.cs:363` — array values are traversed as indexed lists

**Acceptance.**

- doc.products-by-category-price verifies natively for missing, empty, null and multi-element arrays.
- Predicates referring to the same element cannot match values from different elements.

**Existing work.** #1136 (duplicate); #1126 (blocked-by)

<a id="f04t06"></a>

#### F04.T06 — Execute compound and unique document indexes

**functional-gap; P002; W02; effort L; reuse existing issue.** Reuse #1135; scalar single-path indexes cannot serve status plus date as one ordered range.

**Root cause.** The catalog, key encoding and planner use one scalar path per index.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Documents.Catalog/src/ValueObjects/DocumentIndexMetadata.cs:10` — metadata carries one path
- `Cohesion Database/Documents/src/Internal/DocumentPlanner.cs:138` — indexes expose one path

**Proposal.** Implement #1135 with versioned catalog records, tuple key encoding, equality prefixes, one trailing range and unique-null policy. Keep old file-set compatibility explicit.

**Reference precedent.**

- `RavenDB/src/Raven.Server/Documents/Indexes/Persistence/Corax/CoraxDocumentConverterBase.cs:119` — compound fields are processed during conversion

**Acceptance.**

- The required customer/date and status/date indexes are usable with native ordering and range seeks.
- Crash/reopen and unique-conflict tests cover old and new catalog formats.

**Existing work.** #1135 (duplicate)

<a id="f04t07"></a>

#### F04.T07 — Index document array elements with multikey semantics

**functional-gap; P003; W03; effort L; file as new.** Code inspection: the compound-index T1 ticket does not supply multikey element postings.

**Root cause.** There is no per-document set of array-element keys or element correlation metadata; indexing a scalar subscript cannot implement arbitrary ANY predicates.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Documents.Catalog/src/Internal/DocumentIndexKeys.cs:16` — path extraction produces a single scalar value
- `Cohesion Database/Documents.Catalog/docs/DESIGN.md:100` — index key limitations

**Proposal.** After #1136, emit deduplicated scalar postings per array element, maintain all old/new postings atomically on replacement, and specify whether compound arrays carry element identity. Require same-element semantics for category/variant price and stock predicates.

**Reference precedent.**

- `RavenDB/src/Raven.Server/Documents/Indexes/Persistence/Corax/CoraxDocumentConverterBase.cs:363` — enumerables increment list context and recursively emit elements

**Acceptance.**

- Array price queries use an index and return each document once; cross-element false positives are rejected.
- Replacement, deletion, rollback and old snapshots preserve all multikey postings.
- Selective SF10 array queries fetch at most 2x qualifying documents.

**Existing work.** #1136 (blocked-by); #1135 (related)

<a id="f04t08"></a>

#### F04.T08 — Execute atomic document patches through the existing OQL work

**functional-gap; P002; W02; effort L; reuse existing issue.** Reuse #1133; patch-order-status and adjust-variant-stock currently read and replace content.

**Root cause.** No partial-update primitive exists; client emulation must fetch and rebuild a document, then perform an expected-version replacement.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Documents/src/DocumentCollection.cs:109` — PutAsync replaces whole content
- `Cohesion Database/Documents.Language/docs/DESIGN.md:52` — unsupported OQL clauses and expressions

**Proposal.** Extend #1133 implementation evidence to the benchmark patches; execute nested SET/UNSET with one statement snapshot, conflict checks and atomic index updates. A physical in-place delta format is a separate optimization, not required for initial patch semantics.

**Reference precedent.**

- `RavenDB/src/Raven.Client/Documents/Operations/PatchOperation.cs:59` — patch command combines identity, change vector and patch request

**Acceptance.**

- Both patch workloads run natively without a client-side pre-read and match existing results.
- Concurrent patches either compose under defined semantics or return a retryable version conflict; no lost update.

**Existing work.** #1133 (duplicate)

<a id="f04t09"></a>

#### F04.T09 — Add ordered batch document reads in one snapshot

**performance; P003; W03; effort M; file as new.** Smoke: products-multiget p50 4.027 ms Windows / 20.223 ms Linux; RavenDB 1.572 ms.

**Root cause.** Repeated GetAsync calls impose repeated admission, snapshot and commit overhead and have no single-snapshot multi-key result contract.

**Evidence classification:** Development smoke and code inspection.

| Operation | Target | Scale | Metric | Value | Comparison | Comparison value |
|---|---|---:|---|---:|---|---:|
| `doc.products-multiget` | cohesion-documents-embedded (Windows) | 0.01 | p50Ms | 4.027 | ravendb | 1.572 |

**Source.**

- `Cohesion Database/Documents/src/DocumentCollection.cs:75` — the public read API accepts one identity
- `Cohesion Database/Documents/src/DocumentDatabaseSession.cs:178` — collection access is a session operation

**Proposal.** Add GetManyAsync with one snapshot, ordered keys, explicit missing/duplicate behavior, bounded chunking and cancellation; add its protocol operation through #190. Reuse shared read elision before measuring gains.

**Reference precedent.**

- `RavenDB/src/Raven.Client/Documents/Session/Operations/LoadOperation.cs:34` — one GetDocumentsCommand carries an array of ids
- `RavenDB/src/Raven.Client/Documents/Session/Operations/LoadOperation.cs:135` — ByIds accepts multiple keys

**Acceptance.**

- A 10-key batch performs one statement admission and no read-only WAL; duplicate and absent ids have documented deterministic results.
- SF10 multiget p50 is within 2x RavenDB under equivalent transport and key sequence.

**Existing work.** #190 (related); #1280 (blocked-by)

<a id="f04t10"></a>

#### F04.T10 — Expose Documents through its planned wire server and client

**functional-gap; P002; W02; effort L; reuse existing issue.** Existing #190 owns both server and client, not only a client wrapper.

**Root cause.** The benchmark can use Documents only in-process; public network-engine parity cannot be measured.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Documents/docs/DESIGN.md:220` — Documents has no wire server/client

**Proposal.** Implement #190 with bounded framing, session transactions, OQL and document operations; include batch reads and transport cancellation tests from this inventory.

**Reference precedent.**

- `RavenDB/src/Raven.Client/Documents/Session/Operations/LoadOperation.cs:34` — the document client creates a network command

**Acceptance.**

- All currently supported Documents benchmark operations load, verify and run through TCP with the same normalized digest as embedded.
- Disconnects release sessions/snapshots and transactions cannot leak into pooled rentals.

**Existing work.** #190 (duplicate)

<a id="f04t11"></a>

#### F04.T11 — Accelerate document content checksums without changing persisted CRC semantics

**performance; P004; W03; effort M; reuse existing issue.** Code inspection only: every content read runs the byte-at-a-time IEEE CRC loop.

**Root cause.** CRC cost grows with content size and is repeated for every query candidate; it is not the measured multi-millisecond point-read bottleneck.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Documents.Storage/src/Internal/DocumentChunkCodec.cs:50` — one table step per byte
- `Cohesion Database/Documents.Storage/src/Internal/DocumentReadStream.cs:57` — reads append checksum state

**Proposal.** Reuse #1271: implement a faster IEEE-compatible slice/vector algorithm or explicitly version a different checksum. Never substitute CRC-32C for IEEE in existing stores.

**Reference precedent.**

- `PostgreSQL/src/port/pg_crc32c_armv8.c:30` — architecture-specific accelerated checksum implementation; polynomial differs, so it is an optimization pattern only

**Acceptance.**

- Existing IEEE checksum vectors and persisted documents remain readable.
- For 64 KiB documents, checksum-only throughput improves at least 2x on ARM64 and scalar fallback remains correct.

**Existing work.** #1271 (duplicate)

### F05 — Graph query execution and indexing

Reduce candidate expansion, bound execution memory, and replace graph-client emulation with native language features. Feature priority P001; starting wave W01.

<a id="f05t01"></a>

#### F05.T01 — Filter graph adjacency by type and direction before candidate expansion

**performance; P001; W01; effort L; file as new.** R4: two/three-hop patterns hit 100,000 matches or 1,000,000 candidates at SF0.01.

**Root cause.** The storage API decodes every incident relationship into a list, then the executor consumes candidate budget before checking type and direction. High-degree nodes waste work on irrelevant edges; real matching-path fan-out can still exceed the separate match bound.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Graph/src/Internal/GraphPlanExecutor.cs:153` — all incident edges are fetched
- `Cohesion Database/Graph/src/Internal/GraphPlanExecutor.cs:155` — budget is charged before type/direction rejection
- `Cohesion Database/Graph.Storage/src/GraphStore.cs:180` — adjacency keys contain node and relationship id, not type/direction

**Proposal.** Add type/direction-aware adjacency cursors keyed by endpoint/type/direction/edge identity, push edge and safe node predicates during expansion, and distinguish scanned physical candidates from viable expansions in diagnostics. Preserve a physical-work safety budget; do not fix this merely by moving its counter after filtering.

**Reference precedent.**

- `Neo4j/community/kernel/src/main/java/org/neo4j/kernel/impl/newapi/DefaultRelationshipTraversalCursor.java:67` — RelationshipSelection is passed into the storage cursor before traversal

**Acceptance.**

- Adding 1 million unrelated-type edges to an anchor does not increase decoded matching-type edges beyond a small indexed overhead.
- Both exact R4 COHDBG004 repros verify after this and F05.T02; record actual physical and viable expansion counts.
- Cancellation and adversarial high-degree workloads remain bounded and diagnostic.

**Existing work.** #1245 (related); #1144 (related)

<a id="f05t02"></a>

#### F05.T02 — Stream graph bindings under explicit memory and work budgets

**memory; P001; W02; effort XL; file as new.** Measured R4 bound errors; code inspection shows retained binding/path arrays before projection.

**Root cause.** Fixed pattern depth guarantees termination but not affordable fan-out. Full binding lists and copied arrays/dictionaries retain intermediates; downstream LIMIT/aggregation cannot stop upstream expansion. This is not an attribution of the separate 2 GiB OOM.

**Evidence classification:** Measurement and code inspection.

**Measured evidence.** R4 post-run audit: person=165 FoF query throws COHDBG004: The query exceeded 100000 path matches.; person=266/firstName=Penelope 3-hop query throws COHDBG004: The query exceeded 1000000 candidate expansions. R6 did not rerun the full graph or investigate the R5-owned OOM.

**Source.**

- `Cohesion Database/Graph/src/Internal/GraphPlanExecutor.cs:17` — MaximumMatches is a hard 100,000 cap
- `Cohesion Database/Graph/src/Internal/GraphPlanExecutor.cs:144` — completed bindings accumulate before result projection
- `Cohesion Database/Graph/src/Internal/GraphPlanExecutor.cs:166` — each expansion copies a dictionary
- `Cohesion Database/Graph/src/Internal/GraphPlanExecutor.cs:245` — separate 1,000,000 candidate cap

**Proposal.** Introduce iterator/batch binding operators with slot-based bindings and explicit ownership; push predicates, DISTINCT/aggregates and top-N where valid. Expose per-query memory/work limits and cancellation while retaining safe defaults. Owner must choose budget semantics and any opt-in overrides before relaxing hard bounds.

**Reference precedent.**

- `Neo4j/community/cypher/interpreted-runtime/src/main/scala/org/neo4j/cypher/internal/runtime/interpreted/pipes/VarLengthExpandPipe.scala:74` — expansion receives an operator memory tracker and yields an iterator
- `Neo4j/community/cypher/interpreted-runtime/src/main/scala/org/neo4j/cypher/internal/runtime/interpreted/pipes/TopPipe.scala:58` — top-N uses a scoped memory tracker and bounded table

**Acceptance.**

- R4 FoF and three-hop-name repros return complete verified results at SF0.01 without raising process memory limits.
- A deliberately explosive query terminates with a coded budget diagnostic and releases all leases.
- At SF10, profile peak retained bytes and expansions; no unbounded binding lists remain on limited queries.

**Existing work.** #1142 (related); #1144 (related)

<a id="f05t03"></a>

#### F05.T03 — Add an MVCC label index for graph anchor scans

**performance; P002; W02; effort L; file as new.** R4 country-count observations: embedded p50 359.679 ms (run verification incomplete; diagnostic only), TCP 605.695 ms, Neo4j 12.079 ms.

**Root cause.** A label anchor is not an index scan: 300 Persons require considering the 43,595-node R4 store. All node records are decoded before label filtering.

**Evidence classification:** Development smoke and code inspection.

| Operation | Target | Scale | Metric | Value | Comparison | Comparison value |
|---|---|---:|---|---:|---|---:|
| `graph.country-person-count` | cohesion-graph-embedded (diagnostic; run verification incomplete) | 0.01 | p50Ms | 359.679 | neo4j | 12.079 |
| `graph.country-person-count` | cohesion-graph-tcp | 0.01 | p50Ms | 605.695 | neo4j | 12.079 |

**Source.**

- `Cohesion Database/Graph.Storage/src/GraphStore.cs:129` — GetNodes(label) reads every node and sorts matches
- `Cohesion Database/Graph.Storage/src/GraphStore.cs:291` — Ids builds an array from the full record dictionary

**Proposal.** Maintain label-token to node-identity postings transactionally, support label-expression unions/intersections, and iterate visible identities without materializing all nodes. #1245 is related predicate anchoring, not storage label-index implementation.

**Reference precedent.**

- `Neo4j/community/kernel/src/main/java/org/neo4j/kernel/impl/newapi/DefaultNodeLabelIndexCursor.java:40` — a dedicated label/token cursor includes transaction-state additions/removals

**Acceptance.**

- Person label scan examines O(Persons) entries rather than O(all nodes), and old snapshots see correct label versions.
- The country-count benchmark remains correct and its anchor node decodes fall at least 90% on the R4 distribution.

**Existing work.** #1245 (related); #1139 (related)

<a id="f05t04"></a>

#### F05.T04 — Persist graph identity lookup instead of rebuilding the complete record map

**scalability; P003; W03; effort XL; file as new.** R4 observed 0.592 s open, 153.96 MiB process working set, 79.24 MiB managed; these are whole-process values.

**Root cause.** Startup is O(V+E+I), with at least one managed dictionary entry per record and serialized lookup/decode. Process memory in the smoke includes runtime and driver, so it is not the map size.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Graph.Storage/src/GraphStore.cs:26` — all record identities live in a dictionary
- `Cohesion Database/Graph.Storage/src/GraphStore.cs:36` — open scans and decodes the complete graph owner chain
- `Cohesion Database/Graph.Storage/src/GraphStore.cs:302` — Find reads a version while holding the map monitor

**Proposal.** Persist kind/id to version-head mapping in an indexed directory, load it lazily through the buffer pool, and resolve version chains outside a global dictionary monitor. Define transactional head updates and crash rebuild tooling; coordinate with label and adjacency indexes.

**Reference precedent.**

- `Neo4j/community/record-storage-engine/src/main/java/org/neo4j/kernel/impl/store/NodeStore.java:51` — nodes reside in a record store rather than a required all-node managed dictionary

**Acceptance.**

- Opening a 10x larger graph without queries does not allocate 10x identity-map entries.
- Bound managed directory memory by configuration; random lookup remains logarithmic or direct and survives restart/rollback.

**Existing work.** #1234 (related)

<a id="f05t05"></a>

#### F05.T05 — Execute GQL ORDER BY, OFFSET and LIMIT

**functional-gap; P002; W02; effort L; reuse existing issue.** Reuse #1142; recent-messages currently transfers all paths and sorts/trims client-side.

**Root cause.** There is no native order/limit operator, so final-cardinality reduction happens after network transfer.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Graph.Language/docs/DESIGN.md:173` — GQL execution exclusions
- `Cohesion Database/Graph/src/Internal/GraphPlanExecutor.cs:60` — path results are fully materialized

**Proposal.** Implement #1142 with bounded top-N and deterministic null/tie ordering, using F05.T02 streaming.

**Reference precedent.**

- `Neo4j/community/cypher/interpreted-runtime/src/main/scala/org/neo4j/cypher/internal/runtime/interpreted/pipes/TopPipe.scala:47` — LIMIT determines a bounded TopTable

**Acceptance.**

- Recent-messages returns only the requested ten rows over TCP and matches Neo4j ordering/digests.
- Top-N memory is proportional to N, not all paths.

**Existing work.** #1142 (duplicate)

<a id="f05t06"></a>

#### F05.T06 — Execute GQL grouping and aggregate expressions

**functional-gap; P002; W02; effort L; reuse existing issue.** Reuse #1147; country count and tag cooccurrence currently aggregate client-side.

**Root cause.** The native executor has no grouping/aggregate operator, requiring transfer of all source rows.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Graph.Language/docs/DESIGN.md:173` — GQL execution exclusions
- `Cohesion Database/Graph.Language/src/GqlQueryParser.cs:221` — function calls including aggregate expressions are rejected
- `Cohesion Database/Graph/src/Internal/GraphPlanExecutor.cs:105` — projection emits ordinary bound values

**Proposal.** Implement #1147 with DISTINCT aggregate support where specified, hash/ordered grouping and bounded spill; preserve entity identity semantics.

**Reference precedent.**

- `Neo4j/community/cypher/interpreted-runtime/src/main/scala/org/neo4j/cypher/internal/runtime/interpreted/pipes/EagerAggregationPipe.scala:31` — aggregation builds group state through a dedicated pipe

**Acceptance.**

- Country count and tag cooccurrence verify natively; wire rows equal groups, not source paths.
- Grouping has a configured memory budget and old snapshots remain valid through consumption.

**Existing work.** #1147 (duplicate)

<a id="f05t07"></a>

#### F05.T07 — Bind GQL parameters from APIs and the wire

**functional-gap; P002; W02; effort M; reuse existing issue.** Reuse #1138 and the shared #1102 parameter contract.

**Root cause.** Benchmark statements inline escaped literals because the GQL execution surface rejects parameters; this prevents reusable shape plans.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Graph.Language/docs/DESIGN.md:173` — GQL execution exclusions
- `Cohesion Database/Graph.Language/src/GqlQueryParser.cs:191` — parameter tokens are rejected
- `Cohesion Database/Graph/src/Internal/GraphPlanExecutor.cs:39` — planning happens per request

**Proposal.** Implement #1138 with typed value slots bound at execution and one normalized parameter-name contract.

**Reference precedent.**

- `Neo4j/community/cypher/runtime-util/src/main/scala/org/neo4j/cypher/internal/runtime/createParameterArray.scala:29` — runtime parameter values are bound into reusable slots

**Acceptance.**

- The same parsed statement executes different ids safely without literal reconstruction.
- Wire and embedded missing/duplicate/type errors share coded diagnostics.

**Existing work.** #1138 (duplicate); #1102 (blocked-by)

<a id="f05t08"></a>

#### F05.T08 — Execute variable-length graph paths with trail semantics

**functional-gap; P002; W03; effort XL; reuse existing issue.** Reuse #1144; multi-depth benchmark emulation currently sends several fixed patterns.

**Root cause.** The AST/executor has one fixed step per syntactic edge, not a quantified expansion.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Graph.Language/src/GqlQueryParser.cs:186` — quantified/variable-length patterns rejected
- `Cohesion Database/Graph/src/Internal/GraphPlanExecutor.cs:123` — steps are fixed from syntax

**Proposal.** Implement #1144 with bounded traversal and trail identity rules, using typed adjacency and streaming budgets rather than concatenating fixed-depth result lists.

**Reference precedent.**

- `Neo4j/community/cypher/interpreted-runtime/src/main/scala/org/neo4j/cypher/internal/runtime/interpreted/pipes/VarLengthExpandPipe.scala:75` — variable-length iterator receives direction, types, traversal mode and maximum depth

**Acceptance.**

- The 1..2-hop FoF and 1..3-hop name workloads execute as native single statements.
- Cycles, zero-length bounds and duplicate paths match the selected GQL/Cypher semantics.

**Existing work.** #1144 (duplicate)

<a id="f05t09"></a>

#### F05.T09 — Expose shortest graph paths with their actual distance

**functional-gap; P003; W03; effort L; file as new.** Uncovered: the traversal API provides reachability but cannot supply correct per-target shortest distance for the workload.

**Root cause.** Neither fixed GQL patterns nor a visited traversal sequence is a shortest-path result contract. Inferring distance from visit order is incorrect.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Graph/docs/DESIGN.md:392` — C# traversal surface
- `Cohesion Database/Graph.Language/docs/DESIGN.md:173` — GQL execution exclusions
- `benchmarks/database/docs/GRAPH.md:323` — shortest distance remains unsupported; BFS counterexample is covered by benchmark tests

**Proposal.** Add bounded BFS/bidirectional BFS with explicit distance/path result, relationship-type and direction filters, no-path/null behavior, and a GQL shortest selector. Preserve identity-based visited state and define single/all-shortest tie behavior.

**Reference precedent.**

- `Neo4j/community/cypher/slotted-runtime/src/main/scala/org/neo4j/cypher/internal/runtime/slotted/pipes/ShortestPathSlottedPipe.scala:100` — ShortestPathBFSFactory supplies shortestPathIterator at 120

**Acceptance.**

- graph.shortest-path-knows becomes native and matches Neo4j, including disconnected nodes, cycles and unequal branch lengths.
- Memory is O(visited frontier/vertices) under a configurable traversal budget; cancellation is prompt.

**Existing work.** #1144 (blocked-by); #1098 (related)

<a id="f05t10"></a>

#### F05.T10 — Execute NOT EXISTS graph pattern predicates

**functional-gap; P003; W03; effort L; file as new.** Uncovered anti-pattern feature; friend recommendation is currently unsupported.

**Root cause.** Scalar NOT support does not supply a correlated pattern-existence operator; client filtering would change workload semantics.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Graph.Language/docs/DESIGN.md:173` — GQL execution exclusions
- `benchmarks/database/docs/GRAPH.md:66` — recommendation requires engine-selected candidates and predicates

**Proposal.** Add correlated EXISTS/NOT EXISTS pattern subplans with semi/anti apply and early exit; reuse endpoint/type adjacency seeks. Coordinate scalar WHERE support (#1140) and WITH/grouping (#1145/#1147) without treating those as complete coverage.

**Reference precedent.**

- `Neo4j/community/cypher/slotted-runtime/src/main/scala/org/neo4j/cypher/internal/runtime/slotted/pipes/SelectOrSemiApplySlottedPipe.scala:49` — semi-apply evaluates existence of correlated inner results

**Acceptance.**

- Recommendation excludes direct friends inside the engine and verifies against Neo4j.
- NOT EXISTS stops at the first matching edge and handles empty/multiple/correlated bindings.

**Existing work.** #1140 (blocked-by); #1145 (related); #1147 (related)

### F06 — Query preparation and wire execution

Cache validated plan shapes, reduce result framing cost and add graph transaction state. Feature priority P002; starting wave W02.

<a id="f06t01"></a>

#### F06.T01 — Cache parsed and bound query shapes with schema-aware invalidation

**performance; P003; W03; effort L; file as new.** Code inspection: SQL, OQL and GQL parse/plan on each text call; parameters do not provide server preparation.

**Root cause.** Repeated benchmark operations pay tokenization, AST allocation, catalog binding and expression analysis. Caching mutable ASTs or parameter-dependent plans naively would return wrong results or retain unbounded literal variants.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql/src/SqlDatabaseSession.cs:199` — text constructs SqlQueryRequest.FromSql on every call
- `Cohesion Database/Sql/src/Internal/SqlQueryExecutor.cs:52` — a new planner binds each request
- `Cohesion Database/Documents/src/DocumentDatabaseSession.cs:408` — text dispatch builds an OQL request
- `Cohesion Database/Graph/src/Internal/GraphPlanExecutor.cs:39` — a new planner binds each execution

**Proposal.** Add bounded per-database parsed/plan caches keyed by text/parameter types, schema/function/collation version and relevant session settings. Bind values separately, invalidate on DDL, account memory and expose hit/miss metrics. Graph requires #1138; snapshot-sensitive catalog metadata cannot outlive its version.

**Reference precedent.**

- `PostgreSQL/src/backend/utils/cache/plancache.c:8` — plan cache has explicit invalidation and revalidation of dependent objects

**Acceptance.**

- After first use, 1,000 executions of one parameterized shape parse once and bind values correctly.
- DDL/function changes invalidate affected plans; different security and collation contexts cannot share unsafe state.
- At least 50% lower parse/plan allocated bytes on repeated point queries; total latency reported after read-WAL removal.

**Existing work.** #1138 (blocked-by); #1167 (related); #1104 (related)

<a id="f06t02"></a>

#### F06.T02 — Batch result frames and remove repeated graph path serialization allocations

**performance; P002; W02; effort L; file as new.** Measured: Linux 20-path TCP reads allocate 615–623 KB/op versus about 227–229 KB embedded.

**Root cause.** Each path repeats shared nodes and properties; server and client copy framed payloads and build entity objects. Per-frame flushes create many transport scheduling points. One exchange per connection and MaxPoolSize=64 in the benchmark mean an 8-client result is not evidence of pool exhaustion. No Nagle claim: TCP NoDelay defaults true.

**Evidence classification:** Measurement and code inspection.

| Operation | Target | Scale | Metric | Value | Comparison | Comparison value |
|---|---|---:|---|---:|---|---:|
| `graph.person-recent-messages` | cohesion-graph-tcp (8 clients) | 0.01 | opsPerSec | 38.32 | same target, 1 client | 263.022 |

**Measured evidence.** R6 Linux fixed 20-path recent-messages query, one engine plus loopback TCP client in one 3 CPU/2 GiB process: TCP paths 416.97/604.66/377.00 ops/s at 1/3/8 clients. At 8 clients, 2,282 reads generated 2,174 durable flushes, 5.609 s in flush calls over 6.053 s wall time, 37.282 aggregate contention-seconds, 20 peak pool threads and 3 peak queued work items. Durability=None diagnostic: 4,432.75 ops/s at 8 clients and zero durable flushes; this is not an eligible benchmark result or a proposed durability setting. The smaller negative slope reproduces, but the original 263.022 -> 38.320 magnitude is not attributed fully. At 8 clients synchronous paths allocated 621,613 bytes/op; scalars 414,896. Totals include both client and server in this diagnostic process; the benchmark separates them.

**Source.**

- `Cohesion Database/Graph/src/Internal/GraphDatabaseServerSession.cs:496` — encodes and sends every complete path
- `Cohesion Database/Graph/src/Internal/GraphDatabaseServerSession.cs:581` — each frame write flushes the transport
- `Cohesion Database/Protocol/src/Internal/ProtocolStreamFrameWriter.cs:37` — header and payload are separate writes
- `Cohesion Database/Protocol/src/Internal/ProtocolStreamFrameReader.cs:61` — each payload allocates a fresh array
- `Cohesion Database/Graph/src/Protocol/GraphProtocolPathMessage.cs:38` — path encoding returns another array
- `benchmarks/database/src/Assimalign.Cohesion.Benchmarks.Database.Graph/CohesionTarget.cs:36` — benchmark pool limit is 64
- `Cohesion/libraries/Connections/Assimalign.Cohesion.Connections.Tcp/src/TcpConnectionFactoryOptions.cs:23` — client NoDelay defaults true
- `Cohesion/libraries/Connections/Assimalign.Cohesion.Connections.Tcp/src/TcpConnectionListenerOptions.cs:40` — listener NoDelay defaults true

**Proposal.** Use pooled owned frame buffers and bounded byte/count batches; flush at result/latency boundaries and honor backpressure. Prefer scalar projection plus server top-N for recent messages. Add per-phase server execution, encoding, send and client decode counters before changing pipelining or entity deduplication.

**Reference precedent.**

- `Neo4j/community/bolt/src/main/java/org/neo4j/packstream/codec/transport/ChunkFrameEncoder.java:63` — chunks are written into an output buffer
- `Neo4j/community/bolt/src/main/java/org/neo4j/bolt/protocol/common/fsm/response/NetworkResponseHandler.java:153` — success completion flushes the response

**Acceptance.**

- At 20 paths, allocated bytes per TCP operation fall at least 40% and transport flush count is bounded by batch size, not row count.
- Slow consumer, cancellation, partial frame and pool-rental tests preserve result ownership and terminal diagnostics.
- After F01 fixes, 8-client recent-messages throughput does not fall below 1 client on a quiet host; collect server and client spans.

**Existing work.** #1016 (related); #1087 (related); #1230 (related); #1313 (related)

<a id="f06t03"></a>

#### F06.T03 — Add explicit graph transactions over the wire

**functional-gap; P002; W02; effort L; file as new.** Uncovered transport capability; multiple fixed-pattern emulations cannot share a TCP snapshot.

**Root cause.** In-process GraphDatabaseSession exposes transaction objects, but GraphConnection/protocol lacks begin/commit/rollback state. Multiple requests cannot guarantee one read snapshot.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Graph.Client/src/GraphConnection.cs:16` — explicit graph transactions are unsupported
- `Cohesion Database/Graph/src/Internal/GraphDatabaseServerSession.cs:492` — execution is an independent session operation

**Proposal.** Add version-negotiated transaction messages and server session state, binding all requests to the same context until commit/rollback. Define disconnect, aborted statement, timeout and pooled-connection reset behavior; reuse #1105 capability negotiation.

**Reference precedent.**

- `Neo4j/community/bolt/src/main/java/org/neo4j/bolt/protocol/common/fsm/transition/ready/CreateTransactionStateTransition.java:63` — Bolt BEGIN creates a connection-owned explicit transaction

**Acceptance.**

- Two read statements separated by another client write observe one snapshot within a wire transaction.
- Errors, disconnect and pool return roll back or refuse unfinished transactions without silent autocommit.
- Graph multi-statement benchmark emulations can run over TCP until replaced by native queries.

**Existing work.** #1105 (blocked-by); #1013 (related); #1188 (related)

### F07 — Storage, indexes and MVCC maintenance

Make cached reads and ranges incremental; reuse the existing vacuum, unique-insert and checkpoint defects. Feature priority P002; starting wave W02.

<a id="f07t01"></a>

#### F07.T01 — Remove page I/O from the global buffer-pool lock

**scalability; P002; W03; effort XL; file as new.** Code inspection: both hits and miss reads serialize under one pool monitor.

**Root cause.** Even independent page requests share the map/LRU lock; a slow disk miss or dirty eviction blocks unrelated hot hits. The 32 MiB default is configurable, and increasing it alone does not remove contention.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Storage/src/Internal/StorageBufferPool.cs:209` — one monitor covers pin lookup/LRU updates
- `Cohesion Database/Storage/src/Internal/StorageBufferPool.cs:236` — a miss reads and verifies the page inside that monitor
- `Cohesion Database/Storage/src/Internal/StorageBufferPool.cs:424` — FlushAll writes pages under that monitor

**Proposal.** Partition page lookup, introduce per-frame I/O ownership and awaitable miss completion, use a clock or segmented replacement policy, and perform read/write I/O outside mapping locks. Preserve pin lifetime, page restore seqlocks, WAL-before-data ordering and bounded memory.

**Reference precedent.**

- `PostgreSQL/src/backend/storage/buffer/bufmgr.c:2484` — mapping uses partition locks
- `PostgreSQL/src/backend/storage/buffer/bufmgr.c:22` — StartReadBuffers and WaitReadBuffers separate initiation from completion

**Acceptance.**

- A blocked cold-page read does not block a cached read on an unrelated page.
- 8-client cached-read throughput exceeds 3-client throughput on a quiet host after F01.T01; publish per-lock wait attribution.
- Pinned-page, restore, checksum and concurrent DDL tests retain correctness.

**Existing work.** #1157 (related); #1365 (related); #1269 (related)

<a id="f07t02"></a>

#### F07.T02 — Make B+Tree range cursors incremental and cancellable

**memory; P002; W02; effort L; file as new.** Code inspection: opening a cursor materializes every key and entry reference under a tree read latch.

**Root cause.** A cursor is a snapshot list, not an incremental leaf walk. LIMIT cannot stop key collection; wide ranges allocate O(matches), and writers wait for the full read-latched collection.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Indexing/src/BTreeIndex.cs:336` — OpenCursor collects the entire range before returning
- `Cohesion Database/Indexing/src/BTreeIndex.cs:621` — every visible key is copied into a result list

**Proposal.** Implement a resumable leaf cursor with bounded batches, key/reference/writer continuation, snapshot pinning and safe split/delete retries. Propagate cancellation into cursor creation and reverse scans; coordinate pruning so continuation references cannot be reclaimed early.

**Reference precedent.**

- `PostgreSQL/src/backend/access/nbtree/nbtree.c:230` — btgettuple calls _bt_first then _bt_next incrementally

**Acceptance.**

- Opening a million-key range and consuming ten results allocates O(batch size) and visits O(tree height+batch pages).
- Concurrent splits, duplicate runs, rollback, forward/reverse bounds and old-snapshot scans return every expected entry once.

**Existing work.** #1159 (related); #1194 (related); #1195 (related)

<a id="f07t03"></a>

#### F07.T03 — Prune dead secondary-index versions below the snapshot horizon

**performance; P002; W02; effort L; reuse existing issue.** Reuse #1195; current SQL index-maintenance worker remains a stub.

**Root cause.** Record-version pruning does not reclaim committed dead B+Tree entries. UNIQUE and range scans keep revisiting dead entries as churn accumulates.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql/src/Internal/SqlIndexMaintenanceWorker.cs:29` — maintenance body is a stub
- `Cohesion Database/Indexing/src/BTreeIndex.cs:217` — UNIQUE scans live/dead versions through HasLiveEntry
- `Cohesion Database/Indexing/src/BTreeIndex.cs:497` — existing purge handles aborted writers, not committed dead versions

**Proposal.** Implement #1195 with committed-deleter visibility below the oldest pinned horizon, batched index vacuum and safe page reclamation; share the service across model indexes.

**Reference precedent.**

- `PostgreSQL/src/backend/access/nbtree/nbtree.c:1122` — B-tree access method includes vacuum support
- `PostgreSQL/src/backend/access/nbtree/nbtpage.c:3013` — deleted page reuse waits for safe reclamation

**Acceptance.**

- The 20,000-update unique-key fixture has flat per-update cost within 2x between first and last quartiles.
- A held old snapshot reads its versions through concurrent vacuum; index counts shrink after it ends.

**Existing work.** #1195 (duplicate); #1361 (related)

<a id="f07t04"></a>

#### F07.T04 — Reuse the unique-key descent for B+Tree insertion

**performance; P004; W03; effort M; reuse existing issue.** Reuse #1239; unique insertion still descends for HasLiveEntry and again for InsertCore.

**Root cause.** The same search path is traversed twice, increasing latch hold and page pin work for bulk/random unique inserts.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Indexing/src/BTreeIndex.cs:217` — uniqueness probe precedes InsertCore at 222

**Proposal.** Implement #1239 by retaining leaf/search bounds from the uniqueness check and advancing safely to the tuple-order insertion point.

**Reference precedent.**

- `PostgreSQL/src/backend/access/nbtree/nbtinsert.c:170` — one search feeds uniqueness checking and insertion-position reuse

**Acceptance.**

- Unique insertion performs one root descent in the uncontended no-split case.
- Random and monotonic unique insertion throughput does not regress; duplicate/race/crash tests pass.

**Existing work.** #1239 (duplicate)

<a id="f07t05"></a>

#### F07.T05 — Spread checkpoint I/O and admit readers during checkpoint progress

**performance; P002; W03; effort XL; reuse existing issue.** Reuse #1269 and related #1283; checkpoint stalls are documented, not remeasured by R6.

**Root cause.** Synchronous checkpoint I/O and quiescence requirements couple foreground admission to all dirty pages. The baseline documents 241–267 ms stalls; R6 makes no new checkpoint timing claim.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Storage/src/Storage.cs:1543` — checkpoint owns the transaction lock and rejects active physical brackets
- `Cohesion Database/Storage/src/Internal/StorageBufferPool.cs:424` — dirty-page flush holds the global pool lock
- `Cohesion Database/Transactions/src/TransactionCoordinator.cs:63` — apply gate serializes physical mutation/checkpoint work

**Proposal.** Implement #1269 first with bounded paced writeback outside pool locks and read-only begin independence; coordinate #1283 for checkpoint progress under sustained writers. A fuzzy checkpoint/redo-point design requires explicit crash-order review, not merely weakening the active-bracket check.

**Reference precedent.**

- `PostgreSQL/src/backend/postmaster/checkpointer.c:795` — CheckpointWriteDelay spreads writes toward completion target

**Acceptance.**

- At the default pool, reader p99 during checkpoint is below 10 ms on controlled storage.
- A sustained-write test makes bounded checkpoint progress without unbounded WAL growth; crash recovery validates every checkpoint boundary.

**Existing work.** #1269 (duplicate); #1283 (related)

<a id="f07t06"></a>

#### F07.T06 — Bound MVCC prune discovery as well as mutation batches

**memory; P004; W04; effort L; file as new.** Code inspection: record purge batches writes but first copies every eligible tombstone candidate.

**Root cause.** Long-lived snapshots retain a large in-memory prunable list; after the horizon advances, discovery and settled-set allocation scale with the entire backlog even though mutations are batched.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Transactions/src/RecordSpaceVersionStore.cs:200` — FindAll copies eligible entries from the complete prunable list
- `Cohesion Database/Transactions/src/RecordSpaceVersionStore.cs:214` — settled tracking is sized to all candidates

**Proposal.** Partition/order prune candidates by deleter sequence and process a bounded continuation per pass, maintaining a stable safe horizon and retry markers. Expose backlog bytes/age and ensure a failed batch cannot starve later eligible work.

**Reference precedent.**

- `PostgreSQL/src/backend/access/heap/vacuumlazy.c:106` — lazy vacuum performs staged bounded heap/index cleanup

**Acceptance.**

- Releasing a snapshot after 1 million obsolete versions does not allocate one array/hash set for the whole backlog.
- Each pass respects configured time/byte limits and eventually drains eligible versions despite isolated unreadable records.
- The pinned snapshot horizon is never crossed.

**Existing work.** #1361 (related); #1284 (related); #1195 (related)

<a id="f07t07"></a>

#### F07.T07 — Partition lock-manager synchronization while preserving fair queues

**scalability; P005; W05; effort L; file as new.** Code inspection only: release is already owner-indexed, but acquisition/release queues share one monitor.

**Root cause.** The central lock table can serialize unrelated resource owners. R6 did not measure this lock as the current point-read bottleneck, so this is a later scalability task, not an explanation of the smoke regressions.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Transactions/src/LockManager.cs:331` — lock acquisition uses one _sync monitor
- `Cohesion Database/Transactions/src/LockManager.cs:510` — owner-indexed release still runs under _sync
- `Cohesion Database/Transactions/src/LockManager.cs:90` — the old whole-table release issue is already addressed

**Proposal.** After writer/read bottlenecks are removed, measure lock wait attribution and partition resource tables while retaining ordered queues and a consistent cross-partition deadlock graph. Keep the existing owner-resource reverse map.

**Reference precedent.**

- `PostgreSQL/src/backend/storage/lmgr/lock.c:769` — shared lock-manager structures use partitioned synchronization

**Acceptance.**

- Unrelated-resource lock operations scale from 1 to 3 clients; merge only if measured throughput improves at least 25%.
- FIFO exclusive-wait fairness, soft/hard deadlocks and owner cleanup remain correct.

**Existing work.** #1163 (related); #1250 (related); #1420 (related)

### F08 — Durable bulk ingest paths

Replace per-record API/text overhead with bounded, restart-safe bulk paths while keeping equivalent indexes and durability. Feature priority P003; starting wave W03.

<a id="f08t01"></a>

#### F08.T01 — Resolve graph schema definitions by identity without catalog-wide scans

**performance; P003; W02; effort M; reuse existing issue.** Code inspection: the quadratic catalog work from #1229 is still present and adds cost to graph ingest.

**Root cause.** Definition identity and ownership lookup use complete visible-definition lists. Repeating this for each new label/type and discovered property causes catalog-size-dependent work inside writes. This is the existing #1229 defect despite its stale source filenames.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Graph.Catalog/src/GraphCatalog.cs:239` — SaveDefinition enumerates both definition kinds to check identity
- `Cohesion Database/Graph.Catalog/src/GraphCatalog.cs:300` — parent validation enumerates definitions twice for old/latest snapshots
- `Cohesion Database/Graph.Catalog/src/GraphCatalog.cs:319` — owner lookup scans definitions
- `Cohesion Database/Graph/src/GraphDatabase.cs:492` — undeclared-property validation enumerates label/type definitions

**Proposal.** Reuse #1229: maintain identity-to-version-head lookup alongside name lookup, resolve visibility at the requested snapshot, and read owner metadata directly. Cache only version-safe metadata within a statement; preserve schema-owned definitions and conflict checks.

**Reference precedent.**

- `Neo4j/community/token-api/src/main/java/org/neo4j/token/TokenRegistry.java:101` — getToken performs direct id-to-token lookup; name lookup uses a separate map at 93

**Acceptance.**

- Defining N labels/types performs O(N log N) or better catalog lookup work; doubling 1,000 to 2,000 definitions takes at most 2.5x time on a controlled host.
- Per-property validation performs no full catalog enumeration; rollback, old snapshots and schema ownership tests pass.

**Existing work.** #1229 (duplicate)

<a id="f08t02"></a>

#### F08.T02 — Add a streaming SQL bulk-load protocol and API

**load; P003; W03; effort L; file as new.** Code inspection: the SQL client exposes query/execute operations but no bulk stream.

**Root cause.** Batching INSERT text still pays tokenization/value-object construction, row/index work and frame limits. There is no typed bulk row stream or dedicated batched apply.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Sql.Client/src/SqlConnection.cs:82` — query/execute/scalar API surface
- `Cohesion Database/Sql.Language/docs/DIALECT.md:43` — multi-row INSERT is the available ingest primitive

**Proposal.** Add typed column metadata and bounded row batches with explicit atomic-batch semantics, checks/foreign keys, backpressure and final counts; exploit bulk index building after load where all engines use the same schedule.

**Reference precedent.**

- `PostgreSQL/src/backend/commands/copyfrom.c:65` — COPY uses bounded tuple/byte multi-insert buffers

**Acceptance.**

- Load and verify the SF10 SQL dataset with the identical indexes, durable acknowledged batches and unchanged row digests.
- Ingest plus index finalization is at least 2x faster than current INSERT batching, with bounded driver/server memory.
- A partial batch failure has explicit rollback/progress semantics and passes crash tests.

**Existing work.** #43 (related)

<a id="f08t03"></a>

#### F08.T03 — Add bounded bulk document writes and deferred index finalization

**load; P003; W03; effort L; file as new.** Code inspection: Documents ingest loops PutAsync; there is no batch content API.

**Root cause.** An explicit transaction amortizes durability but does not remove per-document operation/copy/catalog overhead or offer streaming backpressure.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Documents/src/DocumentCollection.cs:109` — one-document PutAsync captures and writes content
- `Cohesion Database/Documents/src/DocumentCollection.cs:113` — input bytes are copied before each operation

**Proposal.** Add PutMany/bulk writer with bounded bytes, documented batch atomicity, cancellation, expected-version policy and post-load index construction. Reuse #190 for eventual transport and keep logical index parity.

**Reference precedent.**

- `RavenDB/src/Raven.Client/Documents/BulkInsert/BulkInsertOperation.cs:346` — bulk StoreAsync writes through a streaming operation

**Acceptance.**

- SF10 ingest plus index finalization improves at least 2x over per-document API batching with identical content/index digests.
- Peak in-flight bytes remain within a configured bound and acknowledged batches survive restart.

**Existing work.** #190 (related); #1135 (related)

<a id="f08t04"></a>

#### F08.T04 — Add a bulk graph importer with ordered adjacency and index construction

**load; P003; W04; effort XL; file as new.** Code inspection: per-element APIs or multi-path INSERT are the only ingest paths.

**Root cause.** Per-entity MVCC, catalog/property validation and adjacency B+Tree mutation repeat at every insert; endpoint resolution and textual INSERT add work on the wire.

**Evidence classification:** Code inspection / documented functional exclusion.

**Source.**

- `Cohesion Database/Graph/src/GraphDatabase.cs:234` — one-node creation API
- `Cohesion Database/Graph/src/GraphDatabase.cs:314` — one-relationship creation API
- `Cohesion Database/Graph.Storage/src/GraphStore.cs:145` — each relationship maintains endpoint adjacency

**Proposal.** Provide an offline empty-store importer plus bounded online batches with separate contracts. Map external ids deterministically, validate endpoints, sort/build node/label/type/adjacency/property indexes, and publish the completed store atomically with a durable import manifest.

**Reference precedent.**

- `Neo4j/community/record-storage-engine/src/main/java/org/neo4j/internal/batchimport/ParallelBatchImporter.java:168` — staged importer initializes, imports nodes and then relationships

**Acceptance.**

- Full SF10 graph load has identical node/edge/index counts and query digests; all index-build time is included.
- Total ingest/finalization improves at least 3x over current per-element load; import memory is bounded.
- Interrupted offline import cannot be mistaken for a complete database, and online acknowledged batches survive restart.

**Existing work.** #1229 (related); #1234 (related)

## Probe measurements

The complete scratch source and raw measurement rows are preserved in the appendices below. The fixture was built against the pinned projects without editing them. All disk output was redirected with `Redirect.props`, including Cohesion code generation. Runtime was .NET 10.0.13; the installed build SDK was 11.0.100-rc.2.26504.105 targeting `net10.0`. Windows reports 12 logical CPUs; the Linux container had 3 CPUs, 2 GiB RAM, and swap capped to the same 2 GiB. These are development probes, not the suite's official 3 CPU / 6 GiB SF10 contract.

### Method and attribution limits

Documents used 100 deterministic documents with a 1,024-character payload, one warmed key, and the default buffer pool. Each worker had its own session/collection handle. Fifty untimed point reads preceded 200 explicit begin/read/commit measurements. Autocommit intervals targeted five seconds at 1, 3 and 8 clients; a three-second held-snapshot interval followed each. Setup, parameter creation, session opening, logging and validation setup were outside the per-operation timer; result existence/count validation remained inside. No random input or driver result caching was used.

Graph used one Person, 20 Posts with 1,024-character content, 20 HAS_CREATOR edges and a Person-id index. Every operation returned and checked 20 results. Embedded paths, TCP paths and TCP scalar rows ran consecutively for each client count, for nominal 4/6/4 seconds. The server and loopback client shared the probe process, so TCP allocation/CPU totals include both. Connections were opened before timing, one per worker, with pool maximum 64. The Windows graph pilot overlapped Linux publishing during its first interval; it is preserved for audit but excluded from quantitative conclusions. Linux measurement intervals ran sequentially without an R6 build or another R6 probe in parallel. Other sessions' host activity was uncontrolled.

The graph path statement was:

```gql
MATCH route=(p:Person {id:1})<-[:HAS_CREATOR]-(m:Post|Comment) RETURN route
```

The scalar control returned `m.id,m.creationDate,m.content` from the same 20 Posts. It changes returned representation and projected columns, so the allocation difference is not a pure network-cost subtraction.

`Stopwatch` measured explicit phases and logical-operation duration. A transparent `IFileSystemFileHandle` forwarding wrapper timed journal synchronous `Write` and `Flush`, installed by reflection into the **probe's live object only**. Every call and durability argument reached the original handle. The wrapper's asynchronous methods forward without timing; the cited `StorageJournal.FlushCore` path calls the timed synchronous methods. `StorageEventSource.JournalFlushes` supplied the durable-flush count. Runtime `EventListener` collected contention-stop durations; `Monitor.LockContentionCount`, GC allocation/collection counters, process CPU and sampled thread-pool thread/pending counts supplied cross-checks. An installed `dotnet-stack` captured only the owned Windows probe process.

Allocation is process-wide, including runtime/instrumentation; it is not type-attributed. Monitor wait is summed across threads and can exceed wall time. Runtime event delivery can cross interval boundaries. The sampled thread-pool maximum is a lower bound, not an exact peak. Workers and the 50 ms stop sampler share the thread pool: some eight-client intervals ran beyond the nominal duration, especially held snapshots. Tables use actual elapsed seconds; this scheduler interaction and changing fsync latency prevent interpreting the slopes as controlled capacity curves. No p50/p99 is inferred from a probe's mean.

Fresh stores with `Durability=None` are a diagnostic control only. They retain journal appends but remove durable flush waits. A held explicit snapshot similarly amortizes transaction lifecycle cost while changing transaction boundaries. Neither setting is a recommendation for benchmark runs or production durability.

### Where the point-read time goes

| Platform / durability | Begin mean ms | Get mean ms | Commit mean ms | Durable flushes / 200 transactions |
|---|---:|---:|---:|---:|
| Windows / synchronous | 0.00343 | 0.02988 | 0.33516 | 200 |
| Linux / synchronous | 0.01933 | 0.02678 | 8.15871 | 200 |
| Linux / None diagnostic | 0.00209 | 0.01629 | 0.00246 | 0 |

The Linux explicit commit phase accounts for 99.44% of the three timed phases. During the following one-client autocommit interval, 1,174 reads wrote 89,224 WAL bytes: **76 bytes per read**. Those reads caused 1,174 durable flushes, taking 4.956 seconds inside `Flush` over 5.006 seconds wall time (99.0%); journal writes took only 12.589 ms in total. That is approximately 4.222 ms of synchronous flush per completion versus a 4.263 ms mean whole operation. The difference from the earlier 8.159 ms explicit commit mean demonstrates storage/host variability, not two different engine commit policies.

The end-to-end route is `DocumentCollection.GetAsync` → `DocumentDatabase.RunAsync/BeginOperationAsync` → catalog/version/content read → `DocumentOperation.CompleteAsync` → transaction commit → journal append/force. See F01.T01/T02 for exact source lines. Direct GetAsync has no OQL parse/plan phase. A held snapshot reaches 175,837 reads/s at one client with zero flushes, while still allocating about 4,223 bytes per roughly 1 KiB document. This separates the expensive lifecycle from content-read allocation; it does not prove which allocation type dominates. JSON validation, byte copies and cloned OQL trees are source-inspected in F04.T02.

The 30-second Windows eight-client stack run completed 88,918 reads in 30.031 seconds (2,960.9 ops/s), with 83,106 durable flushes, 27.159 seconds in flush calls and **207.958 aggregate seconds of monitor contention**. A stack snapshot showed six workers at `Monitor.Enter_Slowpath → TransactionCoordinator.GatedJournalLog.AppendCommitAsync`, one at `Monitor.Enter → TransactionManager.BeginAsync`, and one at `Storage.AwaitCommitDurable`. The snapshot locates a convoy; it is not a lock-by-lock time histogram. The code establishes why: journal force holds `_syncRoot`, and appenders can wait for it while holding or waiting for the lifecycle `_gate` (F01.T02). Fewer flushes than reads at higher concurrency reflect shared durable-prefix coverage, not zero-WAL reads.

R3's exact 128.18 → 43.65 slope did **not** reproduce: Linux R6 autocommit produced 234.53/243.97/831.50 ops/s at 1/3/8 clients. The point-read fsync mechanism and journal contention are established; attribution of the original negative slope remains incomplete. After removing read WAL, catalog, buffer and scheduler contention need separately attributed quiet-host measurements before selecting a concurrency redesign.

### Where graph TCP time and allocation go

The same read lifecycle occurs in Graph: `GraphDatabase.Lifetime.BeginOperationAsync` creates a snapshot, and `GraphOperation.CompleteAsync` commits it (F01.T01). The recent-messages probe returned 20 paths, with synchronous TCP throughput 416.97/604.66/377.00 ops/s at 1/3/8 clients. At eight clients, 2,282 reads in 6.053 seconds caused 2,174 durable flushes taking 5.609 seconds and 37.282 aggregate seconds of monitor wait. Dividing by completed operations gives 2.458 ms in flush calls and 16.338 ms aggregate contention per read; these are overlapping/process-wide diagnostics and must not be added into an exact critical-path partition. Mean operation latency was 21.203 ms. Only 3.338 process CPU-seconds were consumed, inconsistent with CPU saturation as the sole cause in this fixture.

The None control removed durable flushes and reached 4,432.75 ops/s at eight clients, with 4.781 aggregate contention-seconds over 6.026 seconds and mean latency 1.804 ms. This is strong evidence of flush/gate dominance, but not a fair comparative throughput result. The original R4 263.02 → 38.32 decline is much larger than R6's one-to-eight decline; no claim is made that transport framing alone explains it.

Paths allocated about 615–623 KB/op across the TCP runs, versus about 227–229 KB/op embedded. At eight clients, returning scalar rows reduced synchronous allocation from 621,613 to 414,896 bytes/op and increased throughput from 377.00 to 642.44 ops/s. Graph server path encoding, repeated entity properties, fresh payload arrays and per-frame flushes explain plausible sources of this amplification (F06.T02). Their individual time/allocation shares were not traced. The pool ceiling is 64 and NoDelay is true at both TCP endpoints; neither pool exhaustion nor Nagle is established by this evidence.

### Complete Linux interval tables

`Flush ms` is time in the forwarding handle's synchronous Flush calls; with None it can include inexpensive non-durable flush calls while durable-flush count stays zero. `Wait ms` is summed runtime monitor wait. All other counters are interval deltas; thread/pending values are sampled maxima. Raw rows, including journal bytes, CPU and all GC generations, appear in the appendix.


#### docs-linux-sync-1.jsonl

| Operation | Clients | Actual seconds | Ops/s | Mean ms | Allocated B/op | Durable flushes | Flush ms | Wait ms | GC 0/1/2 | Pool threads / pending |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---|---|
| docs-auto | 1 | 5.006 | 234.53 | 4.2631 | 4,511 | 1174 | 4,956.14 | 0.00 | 1/1/0 | 3 / 0 |
| docs-shared-snapshot | 1 | 3.027 | 175,837.28 | 0.0055 | 4,223 | 0 | 0.00 | 0.00 | 538/0/0 | 3 / 0 |
| docs-auto | 3 | 5.132 | 243.97 | 12.1523 | 5,940 | 1154 | 5,010.19 | 10,033.89 | 2/0/0 | 4 / 0 |
| docs-shared-snapshot | 3 | 3.019 | 440,401.21 | 0.0066 | 4,226 | 0 | 0.00 | 307.65 | 1383/1/1 | 5 / 0 |
| docs-auto | 8 | 5.041 | 831.50 | 7.5431 | 6,114 | 3873 | 4,701.15 | 26,351.94 | 6/0/0 | 9 / 4 |
| docs-shared-snapshot | 8 | 8.638 | 270,890.01 | 0.0292 | 4,247 | 0 | 0.00 | 33,628.34 | 2458/1/0 | 9 / 0 |

#### docs-linux-none-1.jsonl

| Operation | Clients | Actual seconds | Ops/s | Mean ms | Allocated B/op | Durable flushes | Flush ms | Wait ms | GC 0/1/2 | Pool threads / pending |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---|---|
| docs-auto | 1 | 5.044 | 121,369.84 | 0.0081 | 4,480 | 0 | 112.48 | 1.00 | 655/1/0 | 4 / 0 |
| docs-shared-snapshot | 1 | 3.015 | 210,786.55 | 0.0046 | 4,223 | 0 | 0.00 | 0.00 | 642/0/0 | 4 / 0 |
| docs-auto | 3 | 5.442 | 224,550.15 | 0.0132 | 4,772 | 0 | 219.46 | 1,374.33 | 1445/1/1 | 4 / 0 |
| docs-shared-snapshot | 3 | 3.014 | 389,193.81 | 0.0075 | 4,226 | 0 | 0.00 | 322.23 | 1221/0/0 | 5 / 0 |
| docs-auto | 8 | 5.489 | 154,662.21 | 0.0321 | 4,985 | 0 | 275.74 | 7,946.65 | 1045/1/0 | 6 / 3 |
| docs-shared-snapshot | 8 | 6.002 | 279,547.76 | 0.0212 | 4,249 | 0 | 0.00 | 11,907.05 | 1760/0/0 | 7 / 2 |

#### graph-linux-sync-1.jsonl

| Operation | Clients | Actual seconds | Ops/s | Mean ms | Allocated B/op | Durable flushes | Flush ms | Wait ms | GC 0/1/2 | Pool threads / pending |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---|---|
| graph-embedded-paths | 1 | 4.008 | 635.96 | 1.5720 | 226,734 | 2549 | 3,531.15 | 0.82 | 138/1/0 | 3 / 0 |
| graph-tcp-paths | 1 | 6.051 | 416.97 | 2.3979 | 615,194 | 2523 | 4,235.27 | 38.85 | 381/0/0 | 8 / 4 |
| graph-tcp-scalars | 1 | 4.005 | 402.23 | 2.4857 | 412,716 | 1611 | 3,139.39 | 0.94 | 160/1/1 | 8 / 1 |
| graph-embedded-paths | 3 | 4.043 | 522.59 | 5.7339 | 228,620 | 2106 | 3,834.79 | 7,681.78 | 118/0/0 | 8 / 0 |
| graph-tcp-paths | 3 | 6.045 | 604.66 | 4.9606 | 623,390 | 3614 | 5,267.33 | 9,946.28 | 560/1/0 | 10 / 2 |
| graph-tcp-scalars | 3 | 4.021 | 693.69 | 4.3231 | 414,225 | 2779 | 3,484.85 | 6,497.20 | 285/0/0 | 11 / 1 |
| graph-embedded-paths | 8 | 4.812 | 585.81 | 7.2498 | 228,776 | 2767 | 4,508.86 | 15,047.24 | 144/33/0 | 11 / 6 |
| graph-tcp-paths | 8 | 6.053 | 377.00 | 21.2031 | 621,613 | 2174 | 5,609.22 | 37,282.20 | 293/153/1 | 20 / 3 |
| graph-tcp-scalars | 8 | 4.056 | 642.44 | 12.4375 | 414,896 | 2504 | 3,616.10 | 23,441.39 | 228/83/0 | 20 / 4 |

#### graph-linux-none-1.jsonl

| Operation | Clients | Actual seconds | Ops/s | Mean ms | Allocated B/op | Durable flushes | Flush ms | Wait ms | GC 0/1/2 | Pool threads / pending |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---|---|
| graph-embedded-paths | 1 | 4.020 | 10,715.89 | 0.0930 | 226,819 | 0 | 9.31 | 0.54 | 2337/1/0 | 3 / 0 |
| graph-tcp-paths | 1 | 6.039 | 1,640.79 | 0.6091 | 615,933 | 0 | 1.81 | 125.41 | 1494/1/1 | 10 / 2 |
| graph-tcp-scalars | 1 | 4.005 | 1,895.44 | 0.5273 | 413,354 | 0 | 1.32 | 16.24 | 755/0/0 | 10 / 2 |
| graph-embedded-paths | 3 | 4.905 | 11,201.28 | 0.2675 | 227,566 | 0 | 15.87 | 2,969.55 | 3128/0/0 | 10 / 0 |
| graph-tcp-paths | 3 | 6.007 | 4,513.59 | 0.6644 | 619,484 | 0 | 5.77 | 725.65 | 4216/1/0 | 16 / 4 |
| graph-tcp-scalars | 3 | 4.039 | 5,498.02 | 0.5454 | 412,961 | 0 | 4.88 | 330.43 | 2337/0/0 | 16 / 5 |
| graph-embedded-paths | 8 | 4.025 | 10,800.88 | 0.6304 | 227,872 | 0 | 10.61 | 17,731.85 | 2169/752/19 | 16 / 7 |
| graph-tcp-paths | 8 | 6.026 | 4,432.75 | 1.8042 | 622,246 | 0 | 11.05 | 4,781.20 | 3981/1052/3 | 16 / 10 |
| graph-tcp-scalars | 8 | 4.021 | 6,142.41 | 1.3020 | 412,644 | 0 | 4.64 | 1,190.19 | 2229/742/1 | 16 / 10 |

### Exact R4 statement-bound reproductions

These statements are preserved from the R4 post-run audit, not newly executed by R6. They isolate two distinct bound failures; repeated report entries are not separate defects. See F05.T01/T02 for the execution path and proposed design.

```gql
MATCH route=(p:Person {id:165})-[:KNOWS]-(:Person)-[:KNOWS]-(a:Person)<-[:HAS_CREATOR]-(m:Post|Comment) WHERE a.id <> p.id RETURN route
```

`COHDBG004: The query exceeded 100000 path matches.`

```gql
MATCH (p:Person {id:266})-[:KNOWS]-(:Person)-[:KNOWS]-(:Person)-[:KNOWS]-(f:Person {firstName:'Penelope'}) WHERE f.id <> p.id RETURN f.id,f.firstName,f.lastName
```

`COHDBG004: The query exceeded 1000000 candidate expansions.`

## Not filed

Duplicate tasks remain in the inventory so the orchestrator can attach this evidence without creating another issue. “Not reproducible” below means the R6 probe did not reproduce the exact observation, or, for the R5-owned OOM, no R6 reproduction was attempted and existing isolated R4 runs did not settle the cause. It does not mean the original report was invalid.

| Finding | Reason |
|---|---|
| F01.T01: Elide WAL and durable flushes for read-only transactions | duplicate of #1280 |
| F01.T02: Release journal append synchronization while forcing durable bytes | duplicate of #1276 |
| F02.T02: Seek indexed UPDATE and DELETE targets | duplicate of #1274 |
| F03.T01: Execute N-way, LEFT, CROSS and USING joins | duplicate of #1109 |
| F03.T03: Execute non-recursive common table expressions | duplicate of #1116 |
| F03.T04: Execute correlated subqueries and derived tables | duplicate of #1113 |
| F03.T06: Execute EXTRACT and the planned temporal scalar functions | duplicate of #1120 |
| F03.T07: Execute DISTINCT aggregate arguments | duplicate of #1115 |
| F03.T08: Execute explicit NULLS FIRST and NULLS LAST | duplicate of #1112 |
| F03.T09: Execute SELECT without FROM | duplicate of #1373 |
| F03.T10: Execute SQL savepoints and isolation-level syntax | duplicate of #1012 |
| F04.T04: Execute OQL LIMIT and OFFSET with bounded execution | duplicate of #1129 |
| F04.T05: Execute array existential predicates and UNNEST | duplicate of #1136 |
| F04.T06: Execute compound and unique document indexes | duplicate of #1135 |
| F04.T08: Execute atomic document patches through the existing OQL work | duplicate of #1133 |
| F04.T10: Expose Documents through its planned wire server and client | duplicate of #190 |
| F04.T11: Accelerate document content checksums without changing persisted CRC semantics | duplicate of #1271 |
| F05.T05: Execute GQL ORDER BY, OFFSET and LIMIT | duplicate of #1142 |
| F05.T06: Execute GQL grouping and aggregate expressions | duplicate of #1147 |
| F05.T07: Bind GQL parameters from APIs and the wire | duplicate of #1138 |
| F05.T08: Execute variable-length graph paths with trail semantics | duplicate of #1144 |
| F07.T03: Prune dead secondary-index versions below the snapshot horizon | duplicate of #1195 |
| F07.T04: Reuse the unique-key descent for B+Tree insertion | duplicate of #1239 |
| F07.T05: Spread checkpoint I/O and admit readers during checkpoint progress | duplicate of #1269 |
| F08.T01: Resolve graph schema definitions by identity without catalog-wide scans | duplicate of #1229 |
| The embedded graph combined-throughput process exhausted 2 GiB; R5 owns engine-versus-harness attribution. No R6 memory-leak ticket is justified by process RSS alone. | not reproducible |
| The exact R3 Documents 128.2 -> 43.7 ops/s slope was not reproduced. Read WAL/flush serialization is measured and routed to #1280/#1276; no separate negative-scaling defect is filed. | not reproducible |
| The full R4 graph TCP 263.022 -> 38.320 magnitude was not reproduced. R6 reproduced a smaller 416.97 -> 377.00 slope with measured flush waits; do not file an asserted TCP pool or Nagle defect. | not reproducible |
| Historical whole-lock-table ReleaseAll scan is already replaced by _owned reverse lookup at LockManager.cs:90,510. The dump still lists #1250 open; reconcile status instead of refiling. | duplicate of #1250 |
| Historical duplicate-key B+Tree ordering/leaf-seek defect already has composite search ordering at BTreeIndex.cs:581,598. Do not refile from stale design prose. | duplicate of #1194 |
| Per-transaction full 8 KiB WAL images and unbuffered journal appends are stale descriptions: StorageJournal.cs:33,58 documents checkpoint images/deltas and append buffering now implemented. | duplicate of #1236, #1252, #1253 |
| Three reads and two arrays per recovery frame are stale: StorageJournal.cs:1414 now reads chunks sequentially; keep only the unresolved flush-lock half of #1276. | duplicate of #1276 |
| Debug O(resident pages) invariant checking is not a Release benchmark cause; current hit path checks one frame at StorageBufferPool.cs:219. | duplicate of #1240 |
| Buffer-pool size and WAL-size checkpoint trigger already exist at engine options and Storage.cs:344; no new configuration-exposure ticket. | duplicate of #1254 |
| Other baseline SQL gaps already planned: set operations, RETURNING, UPSERT, views, identity, type aliases and typed literals; carry their native support tests forward under the existing T1 work. | duplicate of #1114, #1117, #1118, #1124, #1123, #1108, #1110 |
| Other baseline OQL gaps already planned: MISSING semantics, IN/BETWEEN/LIKE, DISTINCT/value constructors, collection DDL, INSERT/DELETE, UPSERT and scalar functions. | duplicate of #1126, #1127, #1128, #1130, #1131, #1132, #1134, #1137 |
| Other baseline GQL gaps already planned: label/edge syntax, scalar WHERE, computed RETURN/DISTINCT, OPTIONAL MATCH, composition, values/functions and SET/REMOVE. | duplicate of #1139, #1140, #1141, #1143, #1145, #1146, #1148, #1149 |
| The R4 report repeats verification evidence across runs; these are two engine bounds, not dozens of distinct defects. Report deduplication and host telemetry belong to benchmark readiness. | benchmark-side |

## Design decisions before implementation

- **Graph work and memory limits (F05.T01/T02).** Keep current safe defaults until execution is incremental. Decide whether public budgets constrain examined candidates, retained bytes, elapsed work or a combination, and which callers may override them. Raising/removing the current caps alone does not solve eager allocation or high fan-out. Define whether results are atomic or can stream before a later coded budget error.
- **Concurrent writes (F01.T03, #1393).** Decide whether B+Tree pages retain physical rollback, and define logical conflict ownership, page-latch order, retry rules and rollback recovery before removing the model-wide writer lock or apply gate. The existing read-WAL and flush changes do not require accepting that larger redesign.
- **Operator memory and snapshot lifetime (F02.T04, F05.T02, F07.T02).** Choose memory grants/spill policy and cursor ownership through cancellation, disposal, pool return and checkpoint/purge. Streaming must not release an MVCC horizon while a consumer can still dereference its records.
- **Persistence and index formats (F04.T06/T07, F05.T01/T03/T04).** Decide compatibility/migration policy for compound/multikey keys, typed adjacency, label postings and a persistent identity directory. Specify array element correlation and uniqueness semantics. Keep the persisted IEEE checksum unless a versioned change is intentionally approved.
- **Bulk ingest contracts (F08).** Choose online atomic-batch limits versus an offline empty-store import contract. Include index finalization in reported load time and preserve the same logical indexes across engines. Decide resumability and error reporting before exposing the public API.
- **Remaining SQL scope (F03.T02/T05).** Decide whether RIGHT/FULL OUTER joins and window functions should be new hardening tasks or explicit extensions of the language feature; #1109 does not cover RIGHT/FULL and no matching window task appears in the supplied dump. The inventory keeps these as new work rather than silently expanding existing tickets.

## Verification and command ledger

The suite was built and tested with its normal nearest `Directory.Build.props`, plus an output-only redirect. This deviation from the literal build command was necessary to honor the stronger restriction that Cohesion remain read-only and the benchmarks worktree receive only the two documents. `Redirect.props` also redirects Cohesion's code-generation path; plain `--artifacts-path` would not cover that path. No product source, dependency, test, configuration or existing output was edited.

PowerShell command variables used below:

```powershell
$probe = Join-Path $env:TEMP 'r6-probe'
$env:DOTNET_SYSTEM_NET_DISABLEIPV6 = '1'
```

| Command | Result |
|---|---|
| `git log -1 --oneline` | Initial base confirmed: `388199b`; branch `bench/analysis`. |
| `dotnet build "$probe/R6Probe.csproj" -c Release -p:DirectoryBuildPropsPath="$probe/Redirect.props" -p:DebugType=portable` | Final build succeeded, 0 errors / 0 warnings. Successful rebuild logs: `build.log`, `build3.log`, `build4.log`. Two initial scratch-only setup attempts failed: incorrect props import caused NU1201, then default source globbing included generated scratch files and caused duplicate attributes (270 errors / 103 warnings in `build2.log`). Nearest-props import and explicit Compile items fixed these; no Cohesion edit. |
| `dotnet "$probe/build/bin/R6Probe/release/R6Probe.dll" docs-sync-1` | Completed Windows document phases and all 1/3/8-client intervals; `docs-sync-1.jsonl`. |
| `dotnet "$probe/build/bin/R6Probe/release/R6Probe.dll" docs-stack stack` | Completed owned 30-second eight-client run and held-snapshot control; `docs-stack.jsonl`. |
| `dotnet-stack report --process-id <owned-probe-pid>` | Captured `docs-stack-1.txt`; no other process attached. An initial invocation before the PID was available failed with a missing-argument error and made no attachment. |
| `dotnet "$probe/build/bin/R6Probe/release/R6Probe.dll" graph-sync-1` | Completed Windows graph pilot and all count assertions; first interval overlapped publish, so excluded from causal quantitative comparisons. |
| `dotnet publish "$probe/R6Probe.csproj" -c Release -r linux-arm64 --self-contained true -p:DirectoryBuildPropsPath="$probe/Redirect.props" -p:DebugType=portable -o "$probe/linux"` | Succeeded; `publish.log`. |
| `/probe/R6Probe docs-linux-sync-1` in the session container | Completed Linux document synchronous probe; all reads present. |
| `podman exec dbbench-probe-r6 /probe/R6Probe graph-linux-sync-1` | Completed 9 Linux graph intervals; each operation returned 20 results. |
| `podman exec dbbench-probe-r6 /probe/R6Probe graph-linux-none-1 none` | Completed 9 diagnostic intervals; zero durable flushes. |
| `podman exec dbbench-probe-r6 /probe/R6Probe docs-linux-none-1 none` | Completed 6 diagnostic intervals and explicit phases; zero durable flushes. |
| `dotnet build database/Assimalign.Cohesion.Benchmarks.Database.slnx -c Release -p:DirectoryBuildPropsPath="$probe/Redirect.props"` | **PASS**, 0 errors / 12 warnings, 12.88 s; `solution-build.log`. Existing NU1902/NU1903 warnings concern `System.Security.Cryptography.Pkcs` 9.0.18; dependency changes are out of scope. |
| `dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release --no-build -p:DirectoryBuildPropsPath="$probe/Redirect.props"` | **PASS: 103 passed, 0 failed, 1 skipped, 104 total**, 53 s; `tests.log`. The pre-existing opt-in scale-one generation test requires `DBBENCH_LONG_TESTS`; no tests changed or bypassed. |
| `node "$probe/audit-final.mjs"` | PASS: exact JSON shape/enums, 53 task IDs, issue references, filing decisions, 49 evidence paths, 179 distinct citation locations, 14 raw benchmark observations and report/task correspondence. |
| `git diff --check` and staged equivalent | Whitespace validation before commit; only the two requested documents are staged. |

No new adapter was written, so no new full benchmark smoke was necessary. Existing R2/R3/R4 raw results were checked. The R6 probes used the real Cohesion engine and graph wire stack; no public-engine comparison was run with diagnostic settings. No global tools were installed.

The Linux launch used an already available runtime-deps image and a named data volume. The first direct entrypoint launch failed with permission denied because Windows copying did not preserve the executable bit; the shell entrypoint ran `chmod +x` before executing. The synchronous Documents run used that shell directly; later probes used the waiting shell shown here. Only the R6 container was recreated between these stages.

```powershell
podman volume create dbbench-data-r6
podman create --name dbbench-probe-r6 --cpus 3 --memory 2g --memory-swap 2g --volume dbbench-data-r6:/tmp/r6-probe/data --entrypoint /bin/sh mcr.microsoft.com/dotnet/runtime-deps:10.0 -c 'chmod +x /probe/R6Probe && sleep 3600'
podman cp "$probe/linux" dbbench-probe-r6:/probe
podman start dbbench-probe-r6
podman exec dbbench-probe-r6 /probe/R6Probe graph-linux-sync-1
podman exec dbbench-probe-r6 /probe/R6Probe graph-linux-none-1 none
podman exec dbbench-probe-r6 /probe/R6Probe docs-linux-none-1 none
podman stop dbbench-probe-r6
podman rm dbbench-probe-r6
podman volume rm dbbench-data-r6
```

Stop/remove commands completed successfully; subsequent exact-name container/volume checks were empty. Probe engine/server lifetimes were disposed and all owned processes completed. R5 and other sessions' resources were not stopped or inspected beyond the required source/report reads.

## Follow-up evidence owned elsewhere

R5 retains attribution of the combined graph-process 2 GiB exhaustion. Its conclusion may add a separate engine or harness item; it must not be inferred from this report's eager-allocation inspection. The orchestrator should add quiet-host SF10 latency, throughput, memory and load measurements to the proposed tasks, retaining platform, client count, verification status and equal durability/index settings. Recheck read scaling after #1280 before attributing residual waits to catalog, buffer or lock-manager changes. Reconcile issue status for fixes already visible in this pinned tree instead of filing duplicates.


## Appendix: reproducible probe source

Copy each block to the named file under `%TEMP%\r6-probe`. The output paths in `Redirect.props` name the captured machine; adjust only that scratch prefix when reproducing on another account. Use a fresh fixture argument for each run because setup creates a collection/database and is not an idempotent loader. The code is diagnostic measurement code, not a proposed Cohesion patch. The `minthreads` switch exists for experimentation but was not used in the measurements reported here.

### R6Probe.csproj

SHA-256 of captured file: `6bf56897364988076229e8578ec6703369b7adf78275fbced74961cc6e2e19d5`.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType><TargetFramework>net10.0</TargetFramework><LangVersion>preview</LangVersion>
    <Nullable>enable</Nullable><ImplicitUsings>disable</ImplicitUsings><EnablePreviewFeatures>true</EnablePreviewFeatures>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="Program.cs;TimedHandle.cs;GraphProbe.cs" />
    <ProjectReference Include="C:/cw/bench/cohesion/resources/Database/Assimalign.Cohesion.Database.Documents/src/Assimalign.Cohesion.Database.Documents.csproj" />
    <ProjectReference Include="C:/cw/bench/cohesion/resources/Database/Assimalign.Cohesion.Database.Graph/src/Assimalign.Cohesion.Database.Graph.csproj" />
    <ProjectReference Include="C:/cw/bench/cohesion/resources/Database/Assimalign.Cohesion.Database.Graph.Client/src/Assimalign.Cohesion.Database.Graph.Client.csproj" />
    <ProjectReference Include="C:/cw/bench/cohesion/libraries/Connections/Assimalign.Cohesion.Connections.Tcp/src/Assimalign.Cohesion.Connections.Tcp.csproj" />
  </ItemGroup>
</Project>
```

### Redirect.props

SHA-256 of captured file: `da01b1ed724b8c6a95033230a01ae6d4caa8258ceca47deca117dd1880f88b93`.

```xml
<Project>
  <Import Project="$([MSBuild]::GetPathOfFileAbove('Directory.Build.props','$(MSBuildProjectDirectory)'))" Condition="$([System.String]::Copy('$(MSBuildProjectDirectory)').StartsWith('C:\cw\bench'))" />
  <PropertyGroup>
    <BaseIntermediateOutputPath>C:/Users/chase/AppData/Local/Temp/r6-probe/build/obj/$(MSBuildProjectName)/</BaseIntermediateOutputPath>
    <MSBuildProjectExtensionsPath>$(BaseIntermediateOutputPath)</MSBuildProjectExtensionsPath>
    <BaseOutputPath>C:/Users/chase/AppData/Local/Temp/r6-probe/build/bin/$(MSBuildProjectName)/</BaseOutputPath>
    <ArtifactsPath>C:/Users/chase/AppData/Local/Temp/r6-probe/build/artifacts</ArtifactsPath>
    <_CohesionIntermediateOutputPath>$(BaseIntermediateOutputPath)$(Configuration)/</_CohesionIntermediateOutputPath>
    <_CohesionCodeGenPath>$(_CohesionIntermediateOutputPath)CodeGeneration/</_CohesionCodeGenPath>
    <DefaultItemExcludes>$(DefaultItemExcludes);obj/**;bin/**;artifacts/**</DefaultItemExcludes>
  </PropertyGroup>
</Project>
```

### Program.cs

SHA-256 of captured file: `940a38eb6cdf6c2d862c37c5a9f65c83c1e7da6daa3d7a76577fbc371332bdbe`.

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Database.Documents;
using Assimalign.Cohesion.Database.Storage;

namespace R6Probe;

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        using var events = new RuntimeEvents();
        Console.WriteLine(JsonSerializer.Serialize(new { pid=Environment.ProcessId, runtime=Environment.Version.ToString(), cpu=Environment.ProcessorCount, args }));
        bool none=Array.IndexOf(args,"none")>=0;
        if(args.Length>0 && args[0].StartsWith("graph",StringComparison.Ordinal)){await GraphProbe.Run(args,events,none);return;}
        string root=Path.Combine(Path.GetTempPath(),"r6-probe","data",args.Length>0?args[0]:"docs-sync");
        var builder=DocumentDatabaseEngine.CreateBuilder("r6");
        builder.Options.RootPath=root;
        builder.Options.Durability=none?StorageCommitDurability.None:StorageCommitDurability.Synchronous;
        builder.AddDatabase("probe");
        await using var engine=await builder.BuildAsync();
        var db=await engine.OpenDatabaseAsync("probe");
        TimedHandle.Attach(db);
        await using(var setup=await db.CreateSessionAsync())
        {
            var c=await setup.CreateCollectionAsync("orders");
            await using var tx=await setup.BeginTransactionAsync();
            byte[] json=Encoding.UTF8.GetBytes("{\"id\":1,\"status\":\"placed\",\"content\":\""+new string('x',1024)+"\"}");
            for(int i=0;i<100;i++)await c.PutAsync(setup,"orders/"+i,json);
            await tx.CommitAsync();
        }
        await using(var session=await db.CreateSessionAsync())
        {
            var c=await session.GetCollectionAsync("orders");
            for(int i=0;i<50;i++)await c.GetAsync(session,"orders/1");
            double begin=0,read=0,commit=0;
            long flush=FlushCount();
            for(int i=0;i<200;i++)
            {
                long t=Stopwatch.GetTimestamp();
                await using var tx=await session.BeginTransactionAsync();
                begin+=Stopwatch.GetElapsedTime(t).TotalMilliseconds;t=Stopwatch.GetTimestamp();
                var d=await c.GetAsync(session,"orders/1");
                if(d is null)throw new Exception("Missing");
                read+=Stopwatch.GetElapsedTime(t).TotalMilliseconds;t=Stopwatch.GetTimestamp();
                await tx.CommitAsync();
                commit+=Stopwatch.GetElapsedTime(t).TotalMilliseconds;
            }
            Console.WriteLine(JsonSerializer.Serialize(new{phase="docs-explicit-one-read",beginMs=begin/200,readMs=read/200,commitMs=commit/200,flushes=FlushCount()-flush}));
        }
        foreach(int clients in Array.IndexOf(args,"stack")>=0?new[]{8}:new[]{1,3,8})
        {
            var sessions=new DocumentDatabaseSession[clients];var collections=new DocumentCollection[clients];
            for(int i=0;i<clients;i++){sessions[i]=await db.CreateSessionAsync();collections[i]=await sessions[i].GetCollectionAsync("orders");}
            await Measure("docs-auto",clients,async i=>{var d=await collections[i].GetAsync(sessions[i],"orders/1");if(d is null)throw new Exception("Missing");},events,Array.IndexOf(args,"stack")>=0?30:5);
            var txs=new DocumentDatabaseTransaction[clients];
            for(int i=0;i<clients;i++)txs[i]=await sessions[i].BeginTransactionAsync();
            await Measure("docs-shared-snapshot",clients,async i=>{var d=await collections[i].GetAsync(sessions[i],"orders/1");if(d is null)throw new Exception("Missing");},events,3);
            for(int i=0;i<clients;i++){await txs[i].CommitAsync();await txs[i].DisposeAsync();await sessions[i].DisposeAsync();}
        }
    }

    internal static async Task Measure(string name,int clients,Func<int,ValueTask> op,RuntimeEvents events,int seconds)
    {
        using var start=new ManualResetEventSlim(false);
        long total=0;long totalTicks=0;int maxThreads=0;long maxPending=0;
        using var stop=new CancellationTokenSource();
        Task[] tasks=new Task[clients];
        for(int i=0;i<clients;i++){int client=i;tasks[i]=Task.Run(async()=>{start.Wait();long n=0,ticks=0;while(!stop.IsCancellationRequested){long t=Stopwatch.GetTimestamp();await op(client);ticks+=Stopwatch.GetTimestamp()-t;n++;}Interlocked.Add(ref total,n);Interlocked.Add(ref totalTicks,ticks);});}
        long allocated=GC.GetTotalAllocatedBytes(true),contentions=Monitor.LockContentionCount,flush=FlushCount();
        long flushTicks=TimedHandle.FlushTicks,writeTicks=TimedHandle.WriteTicks,writeBytes=TimedHandle.WriteBytes;
        long runtimeContentions=events.Contentions,waitNs=events.WaitNs;int g0=GC.CollectionCount(0),g1=GC.CollectionCount(1),g2=GC.CollectionCount(2);
        TimeSpan cpu=Process.GetCurrentProcess().TotalProcessorTime;
        var sw=Stopwatch.StartNew();start.Set();
        while(sw.Elapsed.TotalSeconds<seconds){await Task.Delay(50);maxThreads=Math.Max(maxThreads,ThreadPool.ThreadCount);maxPending=Math.Max(maxPending,ThreadPool.PendingWorkItemCount);}
        stop.Cancel();await Task.WhenAll(tasks);sw.Stop();
        Console.WriteLine(JsonSerializer.Serialize(new{name,clients,ops=total,seconds=sw.Elapsed.TotalSeconds,opsPerSec=total/sw.Elapsed.TotalSeconds,meanMs=totalTicks*1000.0/Stopwatch.Frequency/total,allocatedPerOp=(GC.GetTotalAllocatedBytes(true)-allocated)/(double)total,contentions=Monitor.LockContentionCount-contentions,runtimeContentions=events.Contentions-runtimeContentions,waitMs=(events.WaitNs-waitNs)/1e6,flushes=FlushCount()-flush,flushMs=(TimedHandle.FlushTicks-flushTicks)*1000.0/Stopwatch.Frequency,writeMs=(TimedHandle.WriteTicks-writeTicks)*1000.0/Stopwatch.Frequency,journalBytes=TimedHandle.WriteBytes-writeBytes,gc0=GC.CollectionCount(0)-g0,gc1=GC.CollectionCount(1)-g1,gc2=GC.CollectionCount(2)-g2,maxThreads,maxPending,cpuSeconds=(Process.GetCurrentProcess().TotalProcessorTime-cpu).TotalSeconds}));
    }
    internal static long FlushCount()
    {
        var t=typeof(StorageJournal).Assembly.GetType("Assimalign.Cohesion.Database.Storage.Internal.StorageEventSource");
        if(t is null)return -1;
        var log=t.GetField("Log",BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic)?.GetValue(null);
        return log is null?-1:(long)t.GetProperty("JournalFlushes",BindingFlags.Instance|BindingFlags.NonPublic)!.GetValue(log)!;
    }
}

internal sealed class RuntimeEvents:EventListener
{
    internal long Contentions;
    internal long WaitNs;
    protected override void OnEventSourceCreated(EventSource source)
    {
        if(source.Name=="Microsoft-Windows-DotNETRuntime")EnableEvents(source,EventLevel.Informational,(EventKeywords)(0x4000|1));
    }
    protected override void OnEventWritten(EventWrittenEventArgs data)
    {
        if(data.EventName?.StartsWith("ContentionStop",StringComparison.Ordinal)==true)
        {
            Interlocked.Increment(ref Contentions);
            if(data.PayloadNames is not null && data.Payload is not null)
                for(int i=0;i<data.PayloadNames.Count;i++)if(data.PayloadNames[i]=="DurationNs")Interlocked.Add(ref WaitNs,(long)Convert.ToDouble(data.Payload[i]));
        }
    }
}
```

### TimedHandle.cs

SHA-256 of captured file: `7b913e8f342ab98d081f64464cc722e7d420f7f774b29ce7fb751642c968c29a`.

```csharp
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Database.Storage;
using Assimalign.Cohesion.FileSystem;
namespace R6Probe;
internal sealed class TimedHandle(IFileSystemFileHandle inner):IFileSystemFileHandle
{
    internal static long FlushTicks,WriteTicks,WriteBytes;
    internal static void Attach(object database)
    {
        var coordinator=database.GetType().GetProperty("Coordinator",BindingFlags.Instance|BindingFlags.NonPublic)!.GetValue(database)!;
        var journal=(StorageJournal)coordinator.GetType().GetField("_journal",BindingFlags.Instance|BindingFlags.NonPublic)!.GetValue(coordinator)!;
        var field=typeof(StorageJournal).GetField("_handle",BindingFlags.Instance|BindingFlags.NonPublic)!;
        field.SetValue(journal,new TimedHandle((IFileSystemFileHandle)field.GetValue(journal)!));
    }
    public long Length=>inner.Length;
    public bool SupportsDurableFlush=>inner.SupportsDurableFlush;
    public int Read(Span<byte> buffer,long offset)=>inner.Read(buffer,offset);
    public ValueTask<int> ReadAsync(Memory<byte> buffer,long offset,CancellationToken token=default)=>inner.ReadAsync(buffer,offset,token);
    public void Write(ReadOnlySpan<byte> buffer,long offset){long t=Stopwatch.GetTimestamp();inner.Write(buffer,offset);Interlocked.Add(ref WriteTicks,Stopwatch.GetTimestamp()-t);Interlocked.Add(ref WriteBytes,buffer.Length);}
    public ValueTask WriteAsync(ReadOnlyMemory<byte> buffer,long offset,CancellationToken token=default)=>inner.WriteAsync(buffer,offset,token);
    public void SetLength(long length)=>inner.SetLength(length);
    public void Flush(bool durable){long t=Stopwatch.GetTimestamp();inner.Flush(durable);Interlocked.Add(ref FlushTicks,Stopwatch.GetTimestamp()-t);}
    public ValueTask FlushAsync(bool durable,CancellationToken token=default)=>inner.FlushAsync(durable,token);
    public void Dispose()=>inner.Dispose();
    public ValueTask DisposeAsync()=>inner.DisposeAsync();
}
```

### GraphProbe.cs

SHA-256 of captured file: `576781901a20995cb0f2ef0ebf1bbd165e54e21066b72ae6fb4e8634e4f2cebc`.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Connections.Tcp;
using Assimalign.Cohesion.Database.Client;
using Assimalign.Cohesion.Database.Graph;
using Assimalign.Cohesion.Database.Graph.Client;
using Assimalign.Cohesion.Database.Security;
using Assimalign.Cohesion.Database.Storage;
namespace R6Probe;
internal static class GraphProbe
{
    internal static async Task Run(string[] args,RuntimeEvents events,bool none)
    {
        if(Array.IndexOf(args,"minthreads")>=0)ThreadPool.SetMinThreads(32,32);
        var builder=GraphDatabaseEngine.CreateBuilder("r6-graph");
        builder.Options.RootPath=Path.Combine(Path.GetTempPath(),"r6-probe","data",args[0]);
        builder.Options.Durability=none?StorageCommitDurability.None:StorageCommitDurability.Synchronous;
        builder.AddDatabase("probe");
        await using var engine=await builder.BuildAsync();
        var db=await engine.OpenDatabaseAsync("probe");TimedHandle.Attach(db);
        await using(var s=await db.CreateSessionAsync())
        {
            await using(var tx=await s.BeginTransactionAsync())
            {
                var person=await db.CreateNodeAsync(s,["Person"],new Dictionary<string,object?>{{"id",1L},{"firstName","R6"}});
                for(int i=0;i<20;i++)
                {
                    var post=await db.CreateNodeAsync(s,["Post"],new Dictionary<string,object?>{{"id",(long)i},{"creationDate",100000L+i},{"content",new string('x',1024)}});
                    await db.CreateRelationshipAsync(s,post.Id,person.Id,"HAS_CREATOR",new Dictionary<string,object?>());
                }
                await tx.CommitAsync();
            }
            await GraphSchema.Open(db,s).CreateIndexAsync("Person","person_id","id");
        }
        const string query="MATCH route=(p:Person {id:1})<-[:HAS_CREATOR]-(m:Post|Comment) RETURN route";
        var listener=new TcpConnectionListener(new(){EndPoint=new IPEndPoint(IPAddress.Loopback,58746)});
        await using var server=GraphDatabaseServer.Create(engine,new(){Listener=listener,Authenticator=DatabaseAuthenticator.AllowAll});
        await server.StartAsync();
        try
        {
            var settings=DatabaseConnectionSettings.For(new Uri("tcp://127.0.0.1:58746"),"probe","r6");settings.MaxPoolSize=64;
            await using var client=GraphClient.Create(new(){Settings=settings,ConnectionFactory=new TcpConnectionFactory()});
            foreach(int clients in new[]{1,3,8})
            {
                var sessions=new GraphDatabaseSession[clients];var connections=new GraphConnection[clients];
                for(int i=0;i<clients;i++){sessions[i]=await db.CreateSessionAsync();connections[i]=await client.ConnectAsync();}
                await Program.Measure("graph-embedded-paths",clients,async i=>{var r=(GraphPathsQueryResult)await sessions[i].ExecuteAsync(GraphPathsQueryRequest.FromGql(query));if(r.Paths.Count!=20)throw new Exception("Count");},events,4);
                await Program.Measure("graph-tcp-paths",clients,async i=>{int count=0;await foreach(var path in connections[i].QueryPathsAsync(query))count++;if(count!=20)throw new Exception("Count");},events,6);
                await Program.Measure("graph-tcp-scalars",clients,async i=>{var rows=await connections[i].QueryAsync("MATCH (p:Person {id:1})<-[:HAS_CREATOR]-(m:Post) RETURN m.id,m.creationDate,m.content");if(rows.Count!=20)throw new Exception("Count");},events,4);
                for(int i=0;i<clients;i++){await sessions[i].DisposeAsync();await connections[i].DisposeAsync();}
            }
        }
        finally{await server.StopAsync(CancellationToken.None);}
    }
}
```

## Appendix: raw probe rows and provenance

The rows preserve all measurements independently of temporary log retention. Windows graph rows are the excluded pilot. SHA-256 hashes refer to the original byte streams, including their original line endings.

<details>
<summary>docs-sync-1.jsonl — SHA-256 a9243df557db5fa85eee230e24688806637cf86d0a67b0889724a8a2943838bb</summary>

```jsonl
{"pid":41332,"runtime":"10.0.13","cpu":12,"args":["docs-sync-1"]}
{"phase":"docs-explicit-one-read","beginMs":0.003427499999999986,"readMs":0.029882499999999968,"commitMs":0.33516200000000007,"flushes":200}
{"name":"docs-auto","clients":1,"ops":15457,"seconds":5.0366545,"opsPerSec":3068.9021849721876,"meanMs":0.3255591770718768,"allocatedPerOp":4660.967069935951,"contentions":1,"runtimeContentions":1,"waitMs":0.8683,"flushes":15457,"gc0":17,"gc1":1,"gc2":0,"maxThreads":5,"maxPending":1,"cpuSeconds":2.375}
{"name":"docs-shared-snapshot","clients":1,"ops":641979,"seconds":3.0368654,"opsPerSec":211395.27619498712,"meanMs":0.004689225348492708,"allocatedPerOp":4223.232523182222,"contentions":0,"runtimeContentions":0,"waitMs":0,"flushes":0,"gc0":648,"gc1":0,"gc2":0,"maxThreads":5,"maxPending":0,"cpuSeconds":2.984375}
{"name":"docs-auto","clients":3,"ops":15504,"seconds":5.0541475,"opsPerSec":3067.579646221247,"meanMs":0.9778442788957689,"allocatedPerOp":5927.257997936016,"contentions":22232,"runtimeContentions":22229,"waitMs":9735.2932,"flushes":15441,"gc0":23,"gc1":1,"gc2":1,"maxThreads":6,"maxPending":1,"cpuSeconds":2.25}
{"name":"docs-shared-snapshot","clients":3,"ops":1806492,"seconds":3.0508515,"opsPerSec":592127.1487648613,"meanMs":0.005033096631482453,"allocatedPerOp":4224.6590054093795,"contentions":1904,"runtimeContentions":1906,"waitMs":82.0997,"flushes":0,"gc0":1879,"gc1":0,"gc2":0,"maxThreads":6,"maxPending":0,"cpuSeconds":8.46875}
{"name":"docs-auto","clients":8,"ops":15447,"seconds":5.0333327,"opsPerSec":3068.940783509105,"meanMs":2.605914455881401,"allocatedPerOp":6209.155952612157,"contentions":22499,"runtimeContentions":22489,"waitMs":34764.087,"flushes":14778,"gc0":24,"gc1":1,"gc2":0,"maxThreads":12,"maxPending":1,"cpuSeconds":2.4375}
{"name":"docs-shared-snapshot","clients":8,"ops":1378400,"seconds":3.0041337,"opsPerSec":458834.43869359075,"meanMs":0.017396222576900756,"allocatedPerOp":4275.001230412072,"contentions":101948,"runtimeContentions":101930,"waitMs":10025.6392,"flushes":0,"gc0":1458,"gc1":0,"gc2":0,"maxThreads":12,"maxPending":0,"cpuSeconds":13.484375}
```

</details>

<details>
<summary>docs-stack.jsonl — SHA-256 7f1f5427f203705dc15f03b1071f2c979c95b5147f7fa5c879c80126b682e93d</summary>

```jsonl
{"pid":34628,"runtime":"10.0.13","cpu":12,"args":["docs-stack","stack"]}
{"phase":"docs-explicit-one-read","beginMs":0.0040835,"readMs":0.029813000000000013,"commitMs":0.3148640000000001,"flushes":200}
{"name":"docs-auto","clients":8,"ops":88918,"seconds":30.0307492,"opsPerSec":2960.898491337006,"meanMs":2.7015652038957243,"allocatedPerOp":6270.3132324163835,"contentions":137962,"runtimeContentions":137946,"waitMs":207958.2014,"flushes":83106,"flushMs":27158.8477,"writeMs":1205.3742,"journalBytes":6757768,"gc0":139,"gc1":3,"gc2":1,"maxThreads":15,"maxPending":1,"cpuSeconds":14.59375}
{"name":"docs-shared-snapshot","clients":8,"ops":1199409,"seconds":3.0301112,"opsPerSec":395830.0276240687,"meanMs":0.020168319480677568,"allocatedPerOp":4248.567806311275,"contentions":43074,"runtimeContentions":43080,"waitMs":12158.1053,"flushes":0,"flushMs":0,"writeMs":0,"journalBytes":0,"gc0":1260,"gc1":1,"gc2":0,"maxThreads":14,"maxPending":0,"cpuSeconds":9.515625}
```

</details>

<details>
<summary>docs-linux-sync-1.jsonl — SHA-256 e91cfcc2c9c1c9d32957d227605eba741955d1a1c1e2beda00c8402b9c1cd550</summary>

```jsonl
{"pid":1,"runtime":"10.0.13","cpu":3,"args":["docs-linux-sync-1"]}
{"phase":"docs-explicit-one-read","beginMs":0.019328499999999995,"readMs":0.026778500000000004,"commitMs":8.158708500000001,"flushes":200}
{"name":"docs-auto","clients":1,"ops":1174,"seconds":5.0057685,"opsPerSec":234.52942340421853,"meanMs":4.263073287052811,"allocatedPerOp":4510.814310051107,"contentions":0,"runtimeContentions":0,"waitMs":0,"flushes":1174,"flushMs":4956.137138,"writeMs":12.5886,"journalBytes":89224,"gc0":1,"gc1":1,"gc2":0,"maxThreads":3,"maxPending":0,"cpuSeconds":0.234464}
{"name":"docs-shared-snapshot","clients":1,"ops":532239,"seconds":3.0268837,"opsPerSec":175837.28109540517,"meanMs":0.0055021526945601505,"allocatedPerOp":4223.243602967839,"contentions":0,"runtimeContentions":0,"waitMs":0,"flushes":0,"flushMs":0,"writeMs":0,"journalBytes":0,"gc0":538,"gc1":0,"gc2":0,"maxThreads":3,"maxPending":0,"cpuSeconds":3.154628}
{"name":"docs-auto","clients":3,"ops":1252,"seconds":5.131822,"opsPerSec":243.96793185734035,"meanMs":12.152294514376997,"allocatedPerOp":5939.776357827476,"contentions":2108,"runtimeContentions":2108,"waitMs":10033.887079,"flushes":1154,"flushMs":5010.191041,"writeMs":16.951501,"journalBytes":95152,"gc0":2,"gc1":0,"gc2":0,"maxThreads":4,"maxPending":0,"cpuSeconds":0.392096}
{"name":"docs-shared-snapshot","clients":3,"ops":1329398,"seconds":3.0186066,"opsPerSec":440401.2102802664,"meanMs":0.0066238888865486475,"allocatedPerOp":4225.811352206036,"contentions":4162,"runtimeContentions":4155,"waitMs":307.652833,"flushes":0,"flushMs":0,"writeMs":0,"journalBytes":0,"gc0":1383,"gc1":1,"gc2":1,"maxThreads":5,"maxPending":0,"cpuSeconds":8.274103}
{"name":"docs-auto","clients":8,"ops":4192,"seconds":5.0414806,"opsPerSec":831.5017616055093,"meanMs":7.543094467318703,"allocatedPerOp":6113.832061068702,"contentions":6854,"runtimeContentions":6840,"waitMs":26351.935409,"flushes":3873,"flushMs":4701.146681,"writeMs":32.159509,"journalBytes":318592,"gc0":6,"gc1":0,"gc2":0,"maxThreads":9,"maxPending":4,"cpuSeconds":0.854295}
{"name":"docs-shared-snapshot","clients":8,"ops":2339920,"seconds":8.6378971,"opsPerSec":270890.00631878333,"meanMs":0.029202440092823686,"allocatedPerOp":4247.318280966871,"contentions":79599,"runtimeContentions":79606,"waitMs":33628.341086,"flushes":0,"flushMs":0,"writeMs":0,"journalBytes":0,"gc0":2458,"gc1":1,"gc2":0,"maxThreads":9,"maxPending":0,"cpuSeconds":25.960041}
```

</details>

<details>
<summary>docs-linux-none-1.jsonl — SHA-256 988d5af1dff881db030320cdcb18c2c6d053f95e9eaa1698f2d55407544db8a8</summary>

```jsonl
{"pid":73,"runtime":"10.0.13","cpu":3,"args":["docs-linux-none-1","none"]}
{"phase":"docs-explicit-one-read","beginMs":0.002087,"readMs":0.016291500000000025,"commitMs":0.0024585000000000097,"flushes":0}
{"name":"docs-auto","clients":1,"ops":612205,"seconds":5.0441281,"opsPerSec":121369.83594845659,"meanMs":0.008050665082774562,"allocatedPerOp":4479.742556823287,"contentions":3,"runtimeContentions":3,"waitMs":1.0043,"flushes":0,"flushMs":112.476795,"writeMs":445.406586,"journalBytes":46527580,"gc0":655,"gc1":1,"gc2":0,"maxThreads":4,"maxPending":0,"cpuSeconds":5.445468}
{"name":"docs-shared-snapshot","clients":1,"ops":635500,"seconds":3.0148983,"opsPerSec":210786.54626592214,"meanMs":0.004566128918961448,"allocatedPerOp":4223.239402045633,"contentions":0,"runtimeContentions":0,"waitMs":0,"flushes":0,"flushMs":0,"writeMs":0,"journalBytes":0,"gc0":642,"gc1":0,"gc2":0,"maxThreads":4,"maxPending":0,"cpuSeconds":3.078735}
{"name":"docs-auto","clients":3,"ops":1221947,"seconds":5.4417554,"opsPerSec":224550.15159262763,"meanMs":0.01317163724531424,"allocatedPerOp":4771.655955618369,"contentions":21171,"runtimeContentions":21159,"waitMs":1374.327658,"flushes":0,"flushMs":219.462437,"writeMs":1204.222132,"journalBytes":92867972,"gc0":1445,"gc1":1,"gc2":1,"maxThreads":4,"maxPending":0,"cpuSeconds":14.547478}
{"name":"docs-shared-snapshot","clients":3,"ops":1172970,"seconds":3.0138455,"opsPerSec":389193.80572096346,"meanMs":0.007516283094196783,"allocatedPerOp":4226.247054911891,"contentions":4267,"runtimeContentions":4268,"waitMs":322.227266,"flushes":0,"flushMs":0,"writeMs":0,"journalBytes":0,"gc0":1221,"gc1":0,"gc2":0,"maxThreads":5,"maxPending":0,"cpuSeconds":8.27433}
{"name":"docs-auto","clients":8,"ops":848929,"seconds":5.4889234,"opsPerSec":154662.20570686046,"meanMs":0.0320528632865646,"allocatedPerOp":4985.245333826504,"contentions":39960,"runtimeContentions":39932,"waitMs":7946.652434,"flushes":0,"flushMs":275.74223,"writeMs":1255.926853,"journalBytes":64518604,"gc0":1045,"gc1":1,"gc2":0,"maxThreads":6,"maxPending":3,"cpuSeconds":16.502116}
{"name":"docs-shared-snapshot","clients":8,"ops":1677860,"seconds":6.0020513,"opsPerSec":279547.76061310904,"meanMs":0.021159058751028095,"allocatedPerOp":4248.73351531117,"contentions":60535,"runtimeContentions":60468,"waitMs":11907.051845,"flushes":0,"flushMs":0,"writeMs":0,"journalBytes":0,"gc0":1760,"gc1":0,"gc2":0,"maxThreads":7,"maxPending":2,"cpuSeconds":18.015842}
```

</details>

<details>
<summary>graph-sync-1.jsonl — SHA-256 f7e596e75efc9f4319ceeecb6976b68b55d9cf9acb345b58e8f388c7b4506fe0</summary>

```jsonl
{"pid":29132,"runtime":"10.0.13","cpu":12,"args":["graph-sync-1"]}
{"name":"graph-embedded-paths","clients":1,"ops":9779,"seconds":4.0169013,"opsPerSec":2434.463600088954,"meanMs":0.41066209223847017,"allocatedPerOp":227093.9179875243,"contentions":2,"runtimeContentions":2,"waitMs":0.5097,"flushes":9779,"flushMs":2809.6712,"writeMs":116.2882,"journalBytes":743204,"gc0":531,"gc1":1,"gc2":0,"maxThreads":5,"maxPending":1,"cpuSeconds":2.65625}
{"name":"graph-tcp-paths","clients":1,"ops":9043,"seconds":6.0578953,"opsPerSec":1492.7626761723664,"meanMs":0.6698478823399314,"allocatedPerOp":615096.6458033838,"contentions":129,"runtimeContentions":129,"waitMs":14.2838,"flushes":9043,"flushMs":2599.5909,"writeMs":138.7759,"journalBytes":687268,"gc0":1371,"gc1":1,"gc2":1,"maxThreads":19,"maxPending":2,"cpuSeconds":11.390625}
{"name":"graph-tcp-scalars","clients":1,"ops":4333,"seconds":4.0175775,"opsPerSec":1078.5106198947003,"meanMs":0.927122732517886,"allocatedPerOp":413769.1613201015,"contentions":39,"runtimeContentions":39,"waitMs":2.7298,"flushes":4333,"flushMs":2232.0531,"writeMs":73.6109,"journalBytes":329308,"gc0":434,"gc1":0,"gc2":0,"maxThreads":19,"maxPending":2,"cpuSeconds":3.65625}
{"name":"graph-embedded-paths","clients":3,"ops":9013,"seconds":4.0481058,"opsPerSec":2226.4734286342023,"meanMs":1.347266148896039,"allocatedPerOp":228861.56085654054,"contentions":18775,"runtimeContentions":18772,"waitMs":6892.5797,"flushes":8992,"flushMs":3163.0979,"writeMs":168.0392,"journalBytes":684988,"gc0":504,"gc1":0,"gc2":0,"maxThreads":19,"maxPending":1,"cpuSeconds":2.796875}
{"name":"graph-tcp-paths","clients":3,"ops":13938,"seconds":6.0310496,"opsPerSec":2311.040519381568,"meanMs":1.2979561845314966,"allocatedPerOp":616989.1605682308,"contentions":28925,"runtimeContentions":28927,"waitMs":6344.2813,"flushes":13937,"flushMs":4534.0564,"writeMs":305.433,"journalBytes":1059288,"gc0":2162,"gc1":1,"gc2":0,"maxThreads":19,"maxPending":3,"cpuSeconds":14.96875}
{"name":"graph-tcp-scalars","clients":3,"ops":9105,"seconds":4.0257502,"opsPerSec":2261.6902558931747,"meanMs":1.3261268643602415,"allocatedPerOp":416025.11938495334,"contentions":20271,"runtimeContentions":20271,"waitMs":4435.3404,"flushes":9104,"flushMs":2909.9257,"writeMs":162.2566,"journalBytes":691980,"gc0":953,"gc1":0,"gc2":0,"maxThreads":19,"maxPending":3,"cpuSeconds":9.46875}
{"name":"graph-embedded-paths","clients":8,"ops":11542,"seconds":4.0249083,"opsPerSec":2867.642972139266,"meanMs":2.7889817449315544,"allocatedPerOp":229256.3125974701,"contentions":28445,"runtimeContentions":28432,"waitMs":25856.838,"flushes":11412,"flushMs":3548.1234,"writeMs":219.6597,"journalBytes":877192,"gc0":452,"gc1":411,"gc2":1,"maxThreads":19,"maxPending":1,"cpuSeconds":3.390625}
{"name":"graph-tcp-paths","clients":8,"ops":12475,"seconds":6.0138736,"opsPerSec":2074.370169669013,"meanMs":3.8555677675350704,"allocatedPerOp":617858.3086172345,"contentions":41921,"runtimeContentions":41918,"waitMs":34196.875,"flushes":12440,"flushMs":5131.0019,"writeMs":387.861,"journalBytes":948100,"gc0":1596,"gc1":1066,"gc2":7,"maxThreads":19,"maxPending":2,"cpuSeconds":15.625}
{"name":"graph-tcp-scalars","clients":8,"ops":9300,"seconds":4.0232171,"opsPerSec":2311.5829369486423,"meanMs":3.459713322580645,"allocatedPerOp":416494.78365591395,"contentions":30803,"runtimeContentions":30805,"waitMs":22787.8634,"flushes":9261,"flushMs":3488.3049,"writeMs":211.2165,"journalBytes":706800,"gc0":826,"gc1":407,"gc2":1,"maxThreads":19,"maxPending":3,"cpuSeconds":10.140625}
```

</details>

<details>
<summary>graph-linux-sync-1.jsonl — SHA-256 553a8831e89a228f0b8e1976479c7d59b9b043e87bb67d13052fb67ccbba2cb7</summary>

```jsonl
{"pid":4,"runtime":"10.0.13","cpu":3,"args":["graph-linux-sync-1"]}
{"name":"graph-embedded-paths","clients":1,"ops":2549,"seconds":4.0081378,"opsPerSec":635.9561789517317,"meanMs":1.5720270137308747,"allocatedPerOp":226734.42134170263,"contentions":2,"runtimeContentions":2,"waitMs":0.8167,"flushes":2549,"flushMs":3531.153267,"writeMs":18.809195,"journalBytes":193724,"gc0":138,"gc1":1,"gc2":0,"maxThreads":3,"maxPending":0,"cpuSeconds":1.308972}
{"name":"graph-tcp-paths","clients":1,"ops":2523,"seconds":6.0507773,"opsPerSec":416.9712212016132,"meanMs":2.3978730554894967,"allocatedPerOp":615194.0705509314,"contentions":584,"runtimeContentions":584,"waitMs":38.8521,"flushes":2523,"flushMs":4235.274833,"writeMs":33.595601,"journalBytes":191748,"gc0":381,"gc1":0,"gc2":0,"maxThreads":8,"maxPending":4,"cpuSeconds":4.355805}
{"name":"graph-tcp-scalars","clients":1,"ops":1611,"seconds":4.0051263,"opsPerSec":402.2345063125725,"meanMs":2.485746569832402,"allocatedPerOp":412715.808814401,"contentions":13,"runtimeContentions":13,"waitMs":0.9356,"flushes":1611,"flushMs":3139.393847,"writeMs":18.267501,"journalBytes":122436,"gc0":160,"gc1":1,"gc2":1,"maxThreads":8,"maxPending":1,"cpuSeconds":1.811569}
{"name":"graph-embedded-paths","clients":3,"ops":2113,"seconds":4.0433232,"opsPerSec":522.5899329541601,"meanMs":5.73386451206815,"allocatedPerOp":228620.39564600095,"contentions":4212,"runtimeContentions":4212,"waitMs":7681.777482,"flushes":2106,"flushMs":3834.788222,"writeMs":26.660609,"journalBytes":160588,"gc0":118,"gc1":0,"gc2":0,"maxThreads":8,"maxPending":0,"cpuSeconds":0.843393}
{"name":"graph-tcp-paths","clients":3,"ops":3655,"seconds":6.0446936,"opsPerSec":604.6625754529559,"meanMs":4.960572659097128,"allocatedPerOp":623389.5682626539,"contentions":7635,"runtimeContentions":7624,"waitMs":9946.282058,"flushes":3614,"flushMs":5267.32763,"writeMs":68.769808,"journalBytes":277780,"gc0":560,"gc1":1,"gc2":0,"maxThreads":10,"maxPending":2,"cpuSeconds":3.322125}
{"name":"graph-tcp-scalars","clients":3,"ops":2789,"seconds":4.0205064,"opsPerSec":693.6937098271998,"meanMs":4.323129433130155,"allocatedPerOp":414224.7486554321,"contentions":5764,"runtimeContentions":5772,"waitMs":6497.196763,"flushes":2779,"flushMs":3484.846063,"writeMs":33.992105,"journalBytes":211964,"gc0":285,"gc1":0,"gc2":0,"maxThreads":11,"maxPending":1,"cpuSeconds":2.465486}
{"name":"graph-embedded-paths","clients":8,"ops":2819,"seconds":4.8121752,"opsPerSec":585.805770330224,"meanMs":7.249823705214616,"allocatedPerOp":228775.75594182333,"contentions":5965,"runtimeContentions":5968,"waitMs":15047.242418,"flushes":2767,"flushMs":4508.860342,"writeMs":37.686904,"journalBytes":214244,"gc0":144,"gc1":33,"gc2":0,"maxThreads":11,"maxPending":6,"cpuSeconds":1.208197}
{"name":"graph-tcp-paths","clients":8,"ops":2282,"seconds":6.0530865,"opsPerSec":376.99775147769657,"meanMs":21.203068563978967,"allocatedPerOp":621612.9851007888,"contentions":6828,"runtimeContentions":6824,"waitMs":37282.200532,"flushes":2174,"flushMs":5609.223535,"writeMs":63.826616,"journalBytes":173432,"gc0":293,"gc1":153,"gc2":1,"maxThreads":20,"maxPending":3,"cpuSeconds":3.338492}
{"name":"graph-tcp-scalars","clients":8,"ops":2606,"seconds":4.0564222,"opsPerSec":642.4380578530509,"meanMs":12.43753626362241,"allocatedPerOp":414896.2425172678,"contentions":7092,"runtimeContentions":7090,"waitMs":23441.386578,"flushes":2504,"flushMs":3616.095686,"writeMs":37.320705,"journalBytes":198056,"gc0":228,"gc1":83,"gc2":0,"maxThreads":20,"maxPending":4,"cpuSeconds":2.720622}
```

</details>

<details>
<summary>graph-linux-none-1.jsonl — SHA-256 fbae0e97673161415231084aaaca51d597b91615eaf87cc40fc378ed22213989</summary>

```jsonl
{"pid":40,"runtime":"10.0.13","cpu":3,"args":["graph-linux-none-1","none"]}
{"name":"graph-embedded-paths","clients":1,"ops":43079,"seconds":4.020103,"opsPerSec":10715.894592750485,"meanMs":0.09298006146846492,"allocatedPerOp":226818.87638988835,"contentions":14,"runtimeContentions":14,"waitMs":0.5389,"flushes":0,"flushMs":9.311101,"writeMs":84.910002,"journalBytes":3274004,"gc0":2337,"gc1":1,"gc2":0,"maxThreads":3,"maxPending":0,"cpuSeconds":5.09725}
{"name":"graph-tcp-paths","clients":1,"ops":9908,"seconds":6.0385618,"opsPerSec":1640.788043272158,"meanMs":0.609125182276948,"allocatedPerOp":615932.5756964069,"contentions":2457,"runtimeContentions":2457,"waitMs":125.413003,"flushes":0,"flushMs":1.806401,"writeMs":63.064902,"journalBytes":753008,"gc0":1494,"gc1":1,"gc2":1,"maxThreads":10,"maxPending":2,"cpuSeconds":13.988845}
{"name":"graph-tcp-scalars","clients":1,"ops":7592,"seconds":4.0053948,"opsPerSec":1895.4436151961847,"meanMs":0.5273198991043203,"allocatedPerOp":413353.5511064278,"contentions":242,"runtimeContentions":242,"waitMs":16.242102,"flushes":0,"flushMs":1.3249,"writeMs":46.867103,"journalBytes":576992,"gc0":755,"gc1":0,"gc2":0,"maxThreads":10,"maxPending":2,"cpuSeconds":8.130925}
{"name":"graph-embedded-paths","clients":3,"ops":54944,"seconds":4.9051539,"opsPerSec":11201.279535796011,"meanMs":0.2674677015324694,"allocatedPerOp":227566.20253348866,"contentions":26030,"runtimeContentions":26019,"waitMs":2969.551014,"flushes":0,"flushMs":15.8738,"writeMs":136.107306,"journalBytes":4175744,"gc0":3128,"gc1":0,"gc2":0,"maxThreads":10,"maxPending":0,"cpuSeconds":9.93251}
{"name":"graph-tcp-paths","clients":3,"ops":27111,"seconds":6.0065274,"opsPerSec":4513.589665802573,"meanMs":0.6643837124045591,"allocatedPerOp":619484.2350337502,"contentions":6081,"runtimeContentions":6091,"waitMs":725.651411,"flushes":0,"flushMs":5.7668,"writeMs":90.202401,"journalBytes":2060436,"gc0":4216,"gc1":1,"gc2":0,"maxThreads":16,"maxPending":4,"cpuSeconds":14.983305}
{"name":"graph-tcp-scalars","clients":3,"ops":22205,"seconds":4.0387278,"opsPerSec":5498.018460169561,"meanMs":0.5453993700067553,"allocatedPerOp":412960.62976807024,"contentions":3106,"runtimeContentions":3095,"waitMs":330.429107,"flushes":0,"flushMs":4.877,"writeMs":59.760303,"journalBytes":1687580,"gc0":2337,"gc1":0,"gc2":0,"maxThreads":16,"maxPending":5,"cpuSeconds":10.650235}
{"name":"graph-embedded-paths","clients":8,"ops":43471,"seconds":4.024764,"opsPerSec":10800.881741140598,"meanMs":0.630428695521152,"allocatedPerOp":227871.97810034276,"contentions":31041,"runtimeContentions":30966,"waitMs":17731.848905,"flushes":0,"flushMs":10.606801,"writeMs":110.724604,"journalBytes":3303796,"gc0":2169,"gc1":752,"gc2":19,"maxThreads":16,"maxPending":7,"cpuSeconds":8.326853}
{"name":"graph-tcp-paths","clients":8,"ops":26711,"seconds":6.0258347,"opsPerSec":4432.746885672122,"meanMs":1.804181124181049,"allocatedPerOp":622246.0508404777,"contentions":11764,"runtimeContentions":11839,"waitMs":4781.203356,"flushes":0,"flushMs":11.046901,"writeMs":113.136404,"journalBytes":2030036,"gc0":3981,"gc1":1052,"gc2":3,"maxThreads":16,"maxPending":10,"cpuSeconds":15.893952}
{"name":"graph-tcp-scalars","clients":8,"ops":24696,"seconds":4.0205736,"opsPerSec":6142.407143100179,"meanMs":1.302038016520894,"allocatedPerOp":412644.43699384516,"contentions":3281,"runtimeContentions":3281,"waitMs":1190.192048,"flushes":0,"flushMs":4.6383,"writeMs":65.043302,"journalBytes":1876896,"gc0":2229,"gc1":742,"gc2":1,"maxThreads":16,"maxPending":10,"cpuSeconds":10.384917}
```

</details>

### Stack snapshot provenance

`docs-stack-1.txt` SHA-256: `0bfe5da513820da29b87e323fea12e55091b397de1942a595348a0354f7cec72`. The complete stack is included to preserve the gate attribution; addresses and native frames alone are not used to identify a lock.

<details>
<summary>Owned probe stack snapshot</summary>

```text
Thread (0x7C18):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.Monitor.Wait(class System.Object,int32)
  System.Private.CoreLib.il!System.Threading.ManualResetEventSlim.Wait(int32,value class System.Threading.CancellationToken)
  System.Private.CoreLib.il!System.Threading.Tasks.Task.SpinThenBlockingWait(int32,value class System.Threading.CancellationToken)
  System.Private.CoreLib.il!System.Threading.Tasks.Task.InternalWaitCore(int32,value class System.Threading.CancellationToken)
  System.Private.CoreLib.il!System.Threading.Tasks.Task.InternalWait(int32,value class System.Threading.CancellationToken)
  System.Private.CoreLib.il!System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(class System.Threading.Tasks.Task,value class System.Threading.Tasks.ConfigureAwaitOptions)
  R6Probe!R6Probe.Program.<Main>(class System.String[])

Thread (0x8A54):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.Thread.Sleep(int32)
  System.Private.CoreLib.il!System.Diagnostics.Tracing.EventPipeEventDispatcher.DispatchEventsToEventListeners(unsigned int64,value class System.DateTime,int64,int64,class System.Threading.Tasks.Task,value class System.Threading.CancellationToken)
  System.Private.CoreLib.il!System.Threading.ExecutionContext.RunInternal(class System.Threading.ExecutionContext,class System.Threading.ContextCallback,class System.Object)
  System.Private.CoreLib.il!System.Threading.Tasks.Task.ExecuteWithThreadLocal(class System.Threading.Tasks.Task&,class System.Threading.Thread)
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x8FA8):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.ManualResetEventSlim.Wait(int32,value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.Internal.DocumentWriteAheadFlushWorker.WaitForTrigger(value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngineWorker.Run(value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngine.Pump(class Assimalign.Cohesion.Database.DatabaseEngineWorker)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngine+<>c__DisplayClass62_0.<AttachWorker>b__0()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0xBC68):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.WaitHandle.WaitOneNoCheck(int32,bool,class System.Object,value class WaitHandleWaitSourceMap)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngineWorker.WaitForTrigger(value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngineWorker.Run(value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngine.Pump(class Assimalign.Cohesion.Database.DatabaseEngineWorker)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngine+<>c__DisplayClass62_0.<AttachWorker>b__0()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0xAE64):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.ManualResetEventSlim.Wait(int32,value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.DatabaseCheckpointWorker`1[System.__Canon].WaitForTrigger(value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngineWorker.Run(value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngine.Pump(class Assimalign.Cohesion.Database.DatabaseEngineWorker)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngine+<>c__DisplayClass62_0.<AttachWorker>b__0()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0xB8AC):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.Monitor.Wait(class System.Object,int32)
  System.Private.CoreLib.il!System.Threading.ManualResetEventSlim.Wait(int32,value class System.Threading.CancellationToken)
  System.Private.CoreLib.il!System.Threading.ManualResetEventSlim.Wait(value class System.TimeSpan,value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.Internal.DocumentVersionPurgeWorker.WaitForTrigger(value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngineWorker.Run(value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngine.Pump(class Assimalign.Cohesion.Database.DatabaseEngineWorker)
  Assimalign.Cohesion.Database!Assimalign.Cohesion.Database.DatabaseEngine+<>c__DisplayClass62_0.<AttachWorker>b__0()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0xA44C):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+IOCompletionPoller.Poll()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x524C):
  [Native Frames]
  ?!?
  Assimalign.Cohesion.Database.Storage!Assimalign.Cohesion.Database.Storage.Storage.AwaitCommitDurable(int64,value class Assimalign.Cohesion.Database.Storage.StorageCommitDurability)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+GatedJournalLog.AppendCommitAsync(value class Assimalign.Cohesion.Database.Transactions.TransactionSequence,value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionManager+<CommitAsync>d__35.MoveNext()
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+<CommitAsync>d__55.MoveNext()
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.Internal.DocumentOperation+<CompleteAsync>d__14.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.DocumentDatabase+<RunAsync>d__28`1[System.Nullable`1[Assimalign.Cohesion.Database.Documents.Document]].MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass0_0+<<Main>b__0>d.MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1+<<Measure>b__0>d.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start(!!0&)
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1.<Measure>b__0()
  System.Private.CoreLib.il!System.Threading.Tasks.Task`1[System.__Canon].InnerInvoke()
  System.Private.CoreLib.il!System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(class System.Threading.Thread,class System.Threading.ExecutionContext,class System.Threading.ContextCallback,class System.Object)
  System.Private.CoreLib.il!System.Threading.Tasks.Task.ExecuteWithThreadLocal(class System.Threading.Tasks.Task&,class System.Threading.Thread)
  System.Private.CoreLib.il!System.Threading.ThreadPoolWorkQueue.Dispatch()
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x9E20):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.WaitHandle.WaitOneNoCheck(int32,bool,class System.Object,value class WaitHandleWaitSourceMap)
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+GateThread.GateThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x4FC4):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.Monitor.Enter_Slowpath(class System.Object)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+GatedJournalLog.AppendCommitAsync(value class Assimalign.Cohesion.Database.Transactions.TransactionSequence,value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionManager+<CommitAsync>d__35.MoveNext()
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+<CommitAsync>d__55.MoveNext()
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.Internal.DocumentOperation+<CompleteAsync>d__14.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.DocumentDatabase+<RunAsync>d__28`1[System.Nullable`1[Assimalign.Cohesion.Database.Documents.Document]].MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass0_0+<<Main>b__0>d.MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1+<<Measure>b__0>d.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start(!!0&)
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1.<Measure>b__0()
  System.Private.CoreLib.il!System.Threading.Tasks.Task`1[System.__Canon].InnerInvoke()
  System.Private.CoreLib.il!System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(class System.Threading.Thread,class System.Threading.ExecutionContext,class System.Threading.ContextCallback,class System.Object)
  System.Private.CoreLib.il!System.Threading.Tasks.Task.ExecuteWithThreadLocal(class System.Threading.Tasks.Task&,class System.Threading.Thread)
  System.Private.CoreLib.il!System.Threading.ThreadPoolWorkQueue.Dispatch()
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x8BC4):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.Monitor.Enter_Slowpath(class System.Object)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+GatedJournalLog.AppendCommitAsync(value class Assimalign.Cohesion.Database.Transactions.TransactionSequence,value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionManager+<CommitAsync>d__35.MoveNext()
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+<CommitAsync>d__55.MoveNext()
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.Internal.DocumentOperation+<CompleteAsync>d__14.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.DocumentDatabase+<RunAsync>d__28`1[System.Nullable`1[Assimalign.Cohesion.Database.Documents.Document]].MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass0_0+<<Main>b__0>d.MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1+<<Measure>b__0>d.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start(!!0&)
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1.<Measure>b__0()
  System.Private.CoreLib.il!System.Threading.Tasks.Task`1[System.__Canon].InnerInvoke()
  System.Private.CoreLib.il!System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(class System.Threading.Thread,class System.Threading.ExecutionContext,class System.Threading.ContextCallback,class System.Object)
  System.Private.CoreLib.il!System.Threading.Tasks.Task.ExecuteWithThreadLocal(class System.Threading.Tasks.Task&,class System.Threading.Thread)
  System.Private.CoreLib.il!System.Threading.ThreadPoolWorkQueue.Dispatch()
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x8AF4):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.Monitor.Enter_Slowpath(class System.Object)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+GatedJournalLog.AppendCommitAsync(value class Assimalign.Cohesion.Database.Transactions.TransactionSequence,value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionManager+<CommitAsync>d__35.MoveNext()
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+<CommitAsync>d__55.MoveNext()
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.Internal.DocumentOperation+<CompleteAsync>d__14.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.DocumentDatabase+<RunAsync>d__28`1[System.Nullable`1[Assimalign.Cohesion.Database.Documents.Document]].MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass0_0+<<Main>b__0>d.MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1+<<Measure>b__0>d.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start(!!0&)
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1.<Measure>b__0()
  System.Private.CoreLib.il!System.Threading.Tasks.Task`1[System.__Canon].InnerInvoke()
  System.Private.CoreLib.il!System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(class System.Threading.Thread,class System.Threading.ExecutionContext,class System.Threading.ContextCallback,class System.Object)
  System.Private.CoreLib.il!System.Threading.Tasks.Task.ExecuteWithThreadLocal(class System.Threading.Tasks.Task&,class System.Threading.Thread)
  System.Private.CoreLib.il!System.Threading.ThreadPoolWorkQueue.Dispatch()
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0xBC48):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.Monitor.Enter_Slowpath(class System.Object)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+GatedJournalLog.AppendCommitAsync(value class Assimalign.Cohesion.Database.Transactions.TransactionSequence,value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionManager+<CommitAsync>d__35.MoveNext()
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+<CommitAsync>d__55.MoveNext()
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.Internal.DocumentOperation+<CompleteAsync>d__14.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.DocumentDatabase+<RunAsync>d__28`1[System.Nullable`1[Assimalign.Cohesion.Database.Documents.Document]].MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass0_0+<<Main>b__0>d.MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1+<<Measure>b__0>d.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start(!!0&)
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1.<Measure>b__0()
  System.Private.CoreLib.il!System.Threading.Tasks.Task`1[System.__Canon].InnerInvoke()
  System.Private.CoreLib.il!System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(class System.Threading.Thread,class System.Threading.ExecutionContext,class System.Threading.ContextCallback,class System.Object)
  System.Private.CoreLib.il!System.Threading.Tasks.Task.ExecuteWithThreadLocal(class System.Threading.Tasks.Task&,class System.Threading.Thread)
  System.Private.CoreLib.il!System.Threading.ThreadPoolWorkQueue.Dispatch()
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x8A00):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.Monitor.Enter_Slowpath(class System.Object)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+GatedJournalLog.AppendCommitAsync(value class Assimalign.Cohesion.Database.Transactions.TransactionSequence,value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionManager+<CommitAsync>d__35.MoveNext()
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+<CommitAsync>d__55.MoveNext()
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.Internal.DocumentOperation+<CompleteAsync>d__14.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.DocumentDatabase+<RunAsync>d__28`1[System.Nullable`1[Assimalign.Cohesion.Database.Documents.Document]].MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass0_0+<<Main>b__0>d.MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1+<<Measure>b__0>d.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start(!!0&)
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1.<Measure>b__0()
  System.Private.CoreLib.il!System.Threading.Tasks.Task`1[System.__Canon].InnerInvoke()
  System.Private.CoreLib.il!System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(class System.Threading.Thread,class System.Threading.ExecutionContext,class System.Threading.ContextCallback,class System.Object)
  System.Private.CoreLib.il!System.Threading.Tasks.Task.ExecuteWithThreadLocal(class System.Threading.Tasks.Task&,class System.Threading.Thread)
  System.Private.CoreLib.il!System.Threading.ThreadPoolWorkQueue.Dispatch()
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0xBFFC):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.WaitHandle.WaitOneNoCheck(int32,bool,class System.Object,value class WaitHandleWaitSourceMap)
  System.Private.CoreLib.il!System.Threading.TimerQueue.TimerThread()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x2E90):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.Monitor.Enter_Slowpath(class System.Object)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionManager+<BeginAsync>d__34.MoveNext()
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+<BeginAsync>d__54.MoveNext()
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.DocumentDatabase+<BeginOperationAsync>d__27.MoveNext()
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.DocumentDatabase+<RunAsync>d__28`1[System.Nullable`1[Assimalign.Cohesion.Database.Documents.Document]].MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass0_0+<<Main>b__0>d.MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1+<<Measure>b__0>d.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start(!!0&)
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1.<Measure>b__0()
  System.Private.CoreLib.il!System.Threading.Tasks.Task`1[System.__Canon].InnerInvoke()
  System.Private.CoreLib.il!System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(class System.Threading.Thread,class System.Threading.ExecutionContext,class System.Threading.ContextCallback,class System.Object)
  System.Private.CoreLib.il!System.Threading.Tasks.Task.ExecuteWithThreadLocal(class System.Threading.Tasks.Task&,class System.Threading.Thread)
  System.Private.CoreLib.il!System.Threading.ThreadPoolWorkQueue.Dispatch()
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x854C):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.Monitor.Enter_Slowpath(class System.Object)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+GatedJournalLog.AppendCommitAsync(value class Assimalign.Cohesion.Database.Transactions.TransactionSequence,value class System.Threading.CancellationToken)
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionManager+<CommitAsync>d__35.MoveNext()
  Assimalign.Cohesion.Database.Transactions!Assimalign.Cohesion.Database.Transactions.TransactionCoordinator+<CommitAsync>d__55.MoveNext()
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.Internal.DocumentOperation+<CompleteAsync>d__14.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  Assimalign.Cohesion.Database.Documents!Assimalign.Cohesion.Database.Documents.DocumentDatabase+<RunAsync>d__28`1[System.Nullable`1[Assimalign.Cohesion.Database.Documents.Document]].MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass0_0+<<Main>b__0>d.MoveNext()
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1+<<Measure>b__0>d.MoveNext()
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start(!!0&)
  System.Private.CoreLib.il!System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start(!!0&)
  R6Probe!R6Probe.Program+<>c__DisplayClass1_1.<Measure>b__0()
  System.Private.CoreLib.il!System.Threading.Tasks.Task`1[System.__Canon].InnerInvoke()
  System.Private.CoreLib.il!System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop(class System.Threading.Thread,class System.Threading.ExecutionContext,class System.Threading.ContextCallback,class System.Object)
  System.Private.CoreLib.il!System.Threading.Tasks.Task.ExecuteWithThreadLocal(class System.Threading.Tasks.Task&,class System.Threading.Thread)
  System.Private.CoreLib.il!System.Threading.ThreadPoolWorkQueue.Dispatch()
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x5758):
  CPU_TIME
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.Wait(int32,bool)
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x451C):
  CPU_TIME
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.WaitForSignal(int32)
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.Wait(int32,bool)
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x7CAC):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.WaitForSignal(int32)
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.Wait(int32,bool)
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x4C90):
  CPU_TIME
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.WaitForSignal(int32)
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.Wait(int32,bool)
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x5958):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.WaitForSignal(int32)
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.Wait(int32,bool)
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0x9F60):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.WaitForSignal(int32)
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.Wait(int32,bool)
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()

Thread (0xB500):
  [Native Frames]
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.WaitForSignal(int32)
  System.Private.CoreLib.il!System.Threading.LowLevelLifoSemaphore.Wait(int32,bool)
  System.Private.CoreLib.il!System.Threading.PortableThreadPool+WorkerThread.WorkerThreadStart()
  System.Private.CoreLib.il!System.Threading.Thread.StartCallback()
```

</details>

### Input manifest

All input paths are also in the JSON. The report itself is not hashed here to avoid a recursive digest.

| Evidence path | SHA-256 |
|---|---|
| `C:/cw/bench/b-sql/database/artifacts/sql-r2-results/20261015T2141230753801Z-postgresql-latency-1.json` | `49e61a7595d46b5ce98ee827dbfea5aec43c5773c539d4d9635abe0841354e73` |
| `C:/cw/bench/b-sql/database/artifacts/sql-r2-results/20261015T2141281853729Z-postgresql-throughput-1.json` | `3aae67205137c2799b180d7f7033594199fe448178b90fb16620c74d99e04e0b` |
| `C:/cw/bench/b-sql/database/artifacts/sql-r2-results/20261015T2149021705785Z-sqlserver-latency-1.json` | `e7ee793bea2d1a0dbd7110bf2fbfd9b66aa65efb623861af0d3216b31d8277da` |
| `C:/cw/bench/b-sql/database/artifacts/sql-r2-results/20261015T2149047883682Z-sqlserver-throughput-1.json` | `a82a61a2fca4a75abe31e528d0d57254352c9dbbcb32c9c3aa18ec5047096923` |
| `C:/cw/bench/b-sql/database/artifacts/sql-r2-results/20261015T2153255829733Z-cohesion-sql-embedded-latency-1.json` | `63bbd3de8bb29837fe01dbd5e8c6dbad7b73d7d431f884e52735539258b65194` |
| `C:/cw/bench/b-sql/database/artifacts/sql-r2-results/20261015T2153329507202Z-cohesion-sql-embedded-throughput-1.json` | `26a908a4166b693a2e8ff384981f0ca8a46800ee421daed163f7af02e552983b` |
| `C:/cw/bench/b-sql/database/artifacts/sql-r2-results/20261015T2205431048551Z-cohesion-sql-tcp-latency-1.json` | `57804794c55cc01cb54b41b9c6232fe5304237ab6501bc74e0154f624143ff76` |
| `C:/cw/bench/b-sql/database/artifacts/sql-r2-results/20261015T2205481646179Z-cohesion-sql-tcp-throughput-1.json` | `86efef8c11c4683cadf7dad4f4e71923e027058f4b43a23f4af1005e745e16dd` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/windows/20261015T2243062945987Z-cohesion-documents-embedded-latency-1.json` | `f732406cbb462fca5d2a540d349fc4b5f926de33e4ad5d08229b54c10dd0e6de` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/windows/20261015T2243202253144Z-cohesion-documents-embedded-throughput-1.json` | `726cf5366580cecc7f7a56002a47bc31a46e278b9776833dafc0bd80374c317d` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/windows/20261015T2248152079026Z-mongodb-latency-1.json` | `d62282f560dea01f53d48130b625ea1f1cb8b0e316da54214d676ffec1501651` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/windows/20261015T2248304012094Z-mongodb-throughput-1.json` | `f24d03951b8200aa159a159590c958f935cea998c1f7c938ae45081e2044826c` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/windows/20261015T2253003112034Z-ravendb-latency-1.json` | `71d4c86c9ee049bae40fdfa33f1aac377daea71172092708b39efe3a235c2b9e` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/windows/20261015T2253140361534Z-ravendb-throughput-1.json` | `01bb91bbae318d63999f235aae75cc012f4f97ea709257e84c673fc4fcda9bb9` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/windows/20261015T2259571172796Z-cosmos-emulator-latency-1.json` | `3c76b450c691a026b08529918fadc91d2096ae279e90d2ed91e6d70e1d8b58f2` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/windows/20261015T2300240615638Z-cosmos-emulator-throughput-1.json` | `af6d4f6167036f570bca27bb5dff5d75066696282fefa526a09db820d5934543` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/container/20261015T2314322498665Z-cohesion-documents-embedded-latency-1.json` | `3ccb168577cb7db17e1cd5f2121a6898ebb2431f5371b192fb22d687a329fc83` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/container/20261015T2314530954491Z-cohesion-documents-embedded-throughput-1.json` | `67f1702abb40b964dd7331ed370de7afad46ea5cdafe3a1e8deec09f7aceccc4` |
| `C:/cw/bench/b-graph/database/artifacts/r4-results/20261015T2238533088177Z-cohesion-graph-embedded-latency-1.json` | `eab2e0f87f42200d984fc2ba762562335ff336fc5fbe89f3a45af3531439f1cd` |
| `C:/cw/bench/b-graph/database/artifacts/r4-results/20261015T2246597549654Z-cohesion-graph-embedded-throughput-1.json` | `722e854bc90da76d69bcf24e0012212b3396e78060a1d7adc9cd9d9dcce5f113` |
| `C:/cw/bench/b-graph/database/artifacts/r4-results/20261015T2247127375422Z-cohesion-graph-embedded-throughput-1.json` | `2ac76f6870bd6384670ac2793c60baa33b25d516edbf666050b13257a8361ae3` |
| `C:/cw/bench/b-graph/database/artifacts/r4-results/20261015T2250013940970Z-cohesion-graph-embedded-throughput-1.json` | `7f3594af769c1d18024488d6ae3e3e9abe2231b3491fc549fced73257b3351f7` |
| `C:/cw/bench/b-graph/database/artifacts/r4-results/20261015T2250144890841Z-cohesion-graph-embedded-throughput-1.json` | `401589d9218603702000a769271956db38e8e626cd0e10b01096dc57e8b4f9f3` |
| `C:/cw/bench/b-graph/database/artifacts/r4-results/20261015T2258312233435Z-cohesion-graph-tcp-latency-1.json` | `91cbc33bf43c0d7935efd0cd9edab8805fe852e879a3bbbc0151b382ea7e839c` |
| `C:/cw/bench/b-graph/database/artifacts/r4-results/20261015T2301542371756Z-cohesion-graph-tcp-throughput-1.json` | `c3e979620dc93d7f24e8687bd3bf8072ceef32cd13dda1277f0117c91c9c01c4` |
| `C:/cw/bench/b-graph/database/artifacts/r4-results/20261015T2323039915828Z-neo4j-latency-1.json` | `33f570bfc1fec966e437ee139b2daa84a649aca539bee82cdb668108bc5798da` |
| `C:/cw/bench/b-graph/database/artifacts/r4-results/20261015T2326092569514Z-neo4j-throughput-1.json` | `d02bedb87958d28a01e71a2200ec6d42a75806b62f064331301f366a242b0f08` |
| `database/docs/COHESION-CAPABILITIES.md` | `e7b117d1af4c8e68d06c9ec863bf6976374006f340b0c98a8b4594805b1b2169` |
| `database/docs/SQL.md` | `308a8cfcc985b696efd20a8695593bde590cef01a6faed26c5320c21d5e8a748` |
| `database/docs/DOCUMENTS.md` | `22fedcc7efc58f94fbc693c353377403ddea722a013219271c9484697cb9b0d3` |
| `database/docs/GRAPH.md` | `a4319b40b2b291d1be56463315a2249d7301f1036bbf063eccfb9cb8ad1b0f8d` |
| `C:/cw/bench/b-sql/database/artifacts/sql-r2-report/sql.md` | `6b4bda48f024a2363e8841a18d6fbacdbd8dc5b3f1e789e82787fc33ae7c3de9` |
| `C:/cw/bench/b-sql/database/artifacts/sql-r2-report/findings.json` | `72a0c93c74551f571821c63e4b7c2cb7b40f7b1869a4efe70fee679f156e9102` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/windows/report/documents.md` | `103fb1ee927946c807d21705ad261050954d4a67bc15943508628e0da3a318a0` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/windows/report/findings.json` | `d7f987a48143ae7bad3406c50a0af39a0ccc1fcfc178412291dc0d3303d7a8a2` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/container/report/documents.md` | `31846ec2c11584a2b2f353b131b5403b101891a887a4fe50eda94eb5784f6bd1` |
| `C:/cw/bench/b-doc/database/artifacts/documents-r3-smoke/container/report/findings.json` | `e65046a150fb1d7a7c3c3970e607fbb1da6c24a227c17d0e7384d8a72b22fbd7` |
| `C:/cw/bench/b-graph/database/artifacts/r4-report/graph.md` | `5384cd41dbe962c88cd5d4e1e1b67e0bddcbfc7919efdb2e293dec15fb35c34b` |
| `C:/cw/bench/b-graph/database/artifacts/r4-report/findings.json` | `d6ad1c7f3dd65eef421413378e3e84de701cb940a4301c95c9e1905e2d10ac49` |
| `C:/Users/chase/AppData/Local/Temp/claude/C--Source-repos-assimalign-cohesion/2994f116-47ca-4997-88ac-2017811fa3e5/scratchpad/main-dataplatform-issues.jsonl` | `9ab716792b3e54d7b150340331e72f546481b7cbd2e0f349f3f66bded2dc3a20` |
| `C:/Users/chase/AppData/Local/Temp/r6-probe/docs-sync-1.jsonl` | `a9243df557db5fa85eee230e24688806637cf86d0a67b0889724a8a2943838bb` |
| `C:/Users/chase/AppData/Local/Temp/r6-probe/docs-stack.jsonl` | `7f1f5427f203705dc15f03b1071f2c979c95b5147f7fa5c879c80126b682e93d` |
| `C:/Users/chase/AppData/Local/Temp/r6-probe/docs-stack-1.txt` | `0bfe5da513820da29b87e323fea12e55091b397de1942a595348a0354f7cec72` |
| `C:/Users/chase/AppData/Local/Temp/r6-probe/docs-linux-sync-1.jsonl` | `e91cfcc2c9c1c9d32957d227605eba741955d1a1c1e2beda00c8402b9c1cd550` |
| `C:/Users/chase/AppData/Local/Temp/r6-probe/docs-linux-none-1.jsonl` | `988d5af1dff881db030320cdcb18c2c6d053f95e9eaa1698f2d55407544db8a8` |
| `C:/Users/chase/AppData/Local/Temp/r6-probe/graph-sync-1.jsonl` | `f7e596e75efc9f4319ceeecb6976b68b55d9cf9acb345b58e8f388c7b4506fe0` |
| `C:/Users/chase/AppData/Local/Temp/r6-probe/graph-linux-sync-1.jsonl` | `553a8831e89a228f0b8e1976479c7d59b9b043e87bb67d13052fb67ccbba2cb7` |
| `C:/Users/chase/AppData/Local/Temp/r6-probe/graph-linux-none-1.jsonl` | `fbae0e97673161415231084aaaca51d597b91615eaf87cc40fc378ed22213989` |
