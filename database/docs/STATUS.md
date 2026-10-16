# Status — paused before the full-scale runs (2026-10-16)

The database comparison suite is **built, verified and paused**. No full-scale (SF10) benchmark has been run, and
no performance comparison here should be quoted as a result. This document says exactly what works, why the runs
are on hold, and what has to be true before they start.

## Why this is paused

The owner stopped before the SF10 passes: there is substantial engine work still in flight, so measuring now would
describe a version of Cohesion that no longer exists by the time the numbers are read.

The suite is pinned to Cohesion `bf20be5eb` (branch `claude/database-inventory-sql-expansion-aa27d1`). That branch
advanced **52 commits** within a day of the pin, and the moving parts sit underneath this suite:

| Change on the branch | Effect here |
|---|---|
| Compiled-schema format **v5** (`d13a5ab3e`) and **v6** (`38c9e6366`, CHAR recorded separately from VARCHAR) | The schema layer the suite renders against changed twice. Generated schemas and any persisted engine data from the pin are stale. |
| **`CHAR(n)` became a distinct blank-padded type** (#1437, merge `dc77b37ff`) | A type-system change in the dialect the SQL suite targets. The commerce schema uses `VARCHAR`, but padding, comparison and coercion semantics moved across storage and the wire. |
| **SQL type modifiers validated at declaration** (#1438, merge `408cd5a41`): decimal scale normalization, FLOAT precision, temporal and interval qualifiers | The suite declares `NUMERIC(12,2)`, `VARCHAR(n)` and `TIMESTAMP`; those declarations now pass through new validation. |
| **Cancellation carried through planning and binding** (#1419, merge `77c893241`) | Touches the statement hot path the suite measures. |
| **DROP TABLE page reclamation in bounded batches** (#1443) | Storage behaviour change under the write workloads. |

Re-pinning is not a one-line change: the datasets, rendered schemas and any loaded engine volumes have to be
regenerated against the new pin, and the SQL schema renderer must be reviewed against the v6 format and the CHAR
type before its numbers mean anything.

## What is built and verified

- **Suite**: `database/` — harness, deterministic dataset generators, three model suites, the `dbbench` CLI, the
  engine catalog with start/stop/build/orchestration scripts, and `CohesionServer` (hosts one Cohesion SQL or Graph
  engine behind its TCP server). Builds clean in Release; **113 tests pass**, 1 opt-in long test skipped.
- **Datasets**: deterministic and calibrated. SF1 measures 0.96–1.03 GB per model; SF10 generated and measured at
  **9.2 GiB (SQL), 9.0 GiB (documents), 10 GiB (graph)** in the `dbbench-datasets` volume. Byte-identical for any
  thread count and across Windows and Linux.
- **Targets**: Cohesion SQL (in-process and TCP), PostgreSQL 17, SQL Server via Azure SQL Edge, Cohesion Documents
  (in-process), MongoDB 8.0, RavenDB 7.1, Cohesion Graph (in-process and TCP), Neo4j 5. Cosmos DB was removed
  (owner decision: the Linux emulator is not the engine the Azure service runs).
- **Rehearsed at SF 0.1 through the official orchestration**, real budgets, all ten targets:
  **SQL 11,900 pairwise verification cases, 0 mismatches; Documents 3,100, 0 mismatches**; Graph verified
  2,402 completed cases with 4 Cohesion engine errors (below).
- **Fairness**: one engine at a time, identical `--cpus 3 --memory 6g --memory-swap 6g`, durable commits, equivalent
  indexes, identical operation and parameter streams, and every comparable operation's result digest verified equal
  before any timing is reported. Reads are measured on pristine data before any write pass runs.

## What blocks a full-scale run

1. **The pin is stale** (above). Re-pin, regenerate, re-verify.
2. **Cohesion Graph cannot open the 10 GB graph in the 6 GB budget.** `GraphStore` builds an in-memory
   `Dictionary<(byte Kind, ulong Id), Reference>` over **every** node and relationship at open
   (`resources/Database/Assimalign.Cohesion.Database.Graph.Storage/src/GraphStore.cs:26` and its constructor).
   SF10 is 262,242,885 records; at 48 bytes per dictionary entry plus buckets that is **~13.6 GB for the identity
   map alone**, before the buffer pool. Filed as #1535. Until it lands, the Graph comparison can only run at a
   smaller scale (SF1 is ~26.2 M records, ~1.4 GB of map) — a fair comparison, but not a 10 GB one.
3. **Graph traversal limits stop verification at SF 0.01 already.** Friends-of-friends exceeds 100,000 paths;
   three-hop name search and country counts exceed 1,000,000 candidate expansions (`COHDBG004`). Filed as #1532
   (filter adjacency before charging the budget) and #1533 (stream bindings under a budget).
4. **Graph memory exhaustion is the engine's, not the harness's.** A combined 8-client graph throughput pass fails
   at 2 GiB: 1.595 GB GC committed, 1.42 GB/s allocation, a heap dump holding 202,959 `GraphPath` objects and
   594,628 property/binding dictionaries, against 1.49 MB of harness histograms. Attribution recorded in
   `REHEARSAL-R5.md`.
5. **Machine time.** Extrapolated SF10 attempt times (including predicted timeouts) are **~19–26 h for the four
   SQL targets** and **~2.4–6.3 h for the three document targets**, on a laptop shared with other work. The
   orchestration is resumable per phase and target, so this can be spread out, but it is not a background task.

## Findings already filed

The analysis of record is [HARDENING.md](HARDENING.md) and [hardening-inventory.json](hardening-inventory.json):
53 findings — **28 filed as new work, 25 matched to existing issues and deliberately not refiled**, 38 not filed
with reasons (duplicates, descriptions already fixed in the pinned source, and two symptoms that did not
reproduce under probing).

Epic **#1511** `[L03.02.08] Cohesion - Data Platform (Hardening)` under the Data Platform root #1067, with
features **#1512–#1519** and tasks **#1520–#1547**.

The three highest-value items were already filed from code review before this suite existed; the benchmark
supplied their first quantified impact:

- **#1280** — read-only transactions append journal records and force a durable flush. A document point read spends
  ~99% of its wall time inside journal flushes. This is the measured cause of both the Documents and the Graph TCP
  read numbers, which had looked like two separate concurrency problems.
- **#1276** — journal append holds its lock across fsync: 5.6 s of flushing in a 6.0 s window at 8 clients, with
  37 s of aggregate lock wait.
- **#1274** — `UPDATE`/`DELETE` scan to find their targets instead of seeking.

Every quoted number in those items is labelled **development scale (SF 0.01) on a shared host**. They locate
problems; they are not rankings. The items' root causes are source-based and were re-checked against the branch
tip: the cited hot paths (`SqlPlanExecutor.Joins.cs`, `GraphPlanExecutor.cs`, `GraphStore.cs`,
`DocumentPlanner.cs`, `TransactionCoordinator.cs`, `StorageJournal.cs`) are unchanged by the 52 commits, except
one CHAR-semantics commit in the join executor that leaves the outer-scan behaviour intact.

## How to resume

1. **Re-pin Cohesion.** Point `COHESION_ROOT` at a fresh worktree of the integration branch and build its
   `build/Tasks` in Release and Debug. Record the new commit; every result file carries it.
2. **Review the SQL schema renderer** against compiled-schema format v6 and the distinct `CHAR` type, then
   re-run `dotnet test database/tests/...` and the SF 0.1 rehearsal from [RUNBOOK.md](RUNBOOK.md). Verification
   must come back with 0 mismatches before any timing is kept.
3. **Regenerate the datasets** if the generator version or any schema changed, and discard engine volumes loaded
   from the old pin (the storage format gate will refuse older files by design).
4. **Decide the Graph scale** — SF1 at the shared budget, or wait for #1535 — and record the decision.
5. **Run the official passes** with [RUNBOOK.md](RUNBOOK.md), one model at a time, resuming per phase.
6. **Attach the SF10 numbers** to the filed items, replacing the SF 0.01 figures, and re-run the hardening
   analysis against the new pin for anything the newer engine work changed.

## Reading the documents here

| Document | What it holds |
|---|---|
| [DESIGN.md](DESIGN.md) | Targets, topology and budgets, datasets, the operation catalog, support levels, fairness rules, owner decisions |
| [METHODOLOGY.md](METHODOLOGY.md) | Fairness rules, pass order, verification, how to read ratios and findings |
| [RUNBOOK.md](RUNBOOK.md) | How to run, resume and read a full pass |
| [DATASETS.md](DATASETS.md) | Schemas, distributions, counts, formats, measured sizes |
| [COHESION-CAPABILITIES.md](COHESION-CAPABILITIES.md) | What Cohesion could and could not express at the pin, with citations |
| [SQL.md](SQL.md), [DOCUMENTS.md](DOCUMENTS.md), [GRAPH.md](GRAPH.md) | Per-model statements, support levels with evidence, load paths |
| [HARDENING.md](HARDENING.md), [hardening-inventory.json](hardening-inventory.json) | The hardening analysis and the filing contract |
| [REHEARSAL-R5.md](REHEARSAL-R5.md) | The SF 0.1 rehearsal record, timings and SF10 estimates |
| [SMOKE-R1A.md](SMOKE-R1A.md) | The infrastructure smoke record (includes historical Cosmos references) |
