# Database benchmarks

Two things live here, at very different stages.

| | What it is | State |
| --- | --- | --- |
| `benchmarks/Assimalign.Cohesion.Database.Benchmarks` | The area's BenchmarkDotNet suite: a SQL point query against SQLite, SQL parsing, and a key-value point read | **Runs.** Part of every CI check and the nightly history |
| Everything else here | Groundwork for comparing the Cohesion engines with PostgreSQL, SQL Server, MongoDB, RavenDB and Neo4j on about 10 GB per model | **Not running.** Datasets, engine catalog and design only |

Nothing in this folder has produced a comparative performance result. Treat any number quoted in
[docs/](docs/) as what it says it is: a development-scale measurement from a paused run against a Cohesion build
that no longer exists.

## The BenchmarkDotNet suite

```powershell
$env:NuGetPackageSourceCredentials_assimalign = "Username=<github-user>;Password=$(gh auth token)"  # read:packages
../../../scripts/Invoke-Benchmarks.ps1 -Area Database
```

Three benchmarks, each measuring the embedded engine through the API an application calls:

- `Database.Sql/SqlPointQueryBenchmarks` — a parameterized primary-key lookup, with SQLite through
  `Microsoft.Data.Sqlite` as the baseline, so `Ratio` reads "Cohesion relative to the counterpart".
- `Database.Sql.Language/SqlQueryParserBenchmarks` — parsing a point select, an insert and a join.
- `Database.KeyValuePair/KeyValuePointBenchmarks` — a point read through one session.

These three were migrated on 2026-10-19 when the pin moved to `ci.44`, the first build carrying PR #1168: the
Database API went from interface-first to concrete-first and they stopped compiling, exactly as planned. The
shape of that break is kept in
[docs/COHESION-CAPABILITIES.md](docs/COHESION-CAPABILITIES.md#the-api-migration-now-done), because anything else
written against the older API needs the same edits.

## The comparison: what is here

- **[docs/DESIGN.md](docs/DESIGN.md)** — the comparison's design: targets, the shared resource budget, the three
  datasets, the operation catalog, and how an operation is classified native, emulated or unsupported.
- **[docs/COHESION-CAPABILITIES.md](docs/COHESION-CAPABILITIES.md)** — what the Cohesion engines can and cannot
  express, with citations, as of the pinned build. Read this before writing an operation; it is what keeps
  a comparison honest rather than accidentally measuring a client-side workaround.
- **[targets/catalog.json](targets/catalog.json)** — the five external engines, each a container the shared
  harness can start, with an identical 3 CPU / 6 GB budget and swap pinned to memory. Settings and their reasons
  are in [targets/README.md](targets/README.md).
- **`benchmarks/Assimalign.Cohesion.Database.Benchmarks.Datasets`** — deterministic generators for the three
  datasets (commerce, marketplace, social). Byte-identical output for a seed and scale, on any thread count and
  on Windows or Linux; scale 1 measures 0.96–1.03 GB per model, scale 10 is the comparison's size.
  [docs/DATASETS.md](docs/DATASETS.md) has the schemas, distributions and measured sizes.
- **`tests/`** — the generators' determinism tests. CI does not run them (the workflow builds and runs suites and
  has no test step), so run them by hand after touching a generator:

  ```powershell
  dotnet test benchmarks/resources/Database/tests/Assimalign.Cohesion.Database.Benchmarks.Datasets.Tests/src
  ```

## What is not here yet

The comparison needs three things this folder does not have, in this order:

1. **A Cohesion side.** The engines run embedded, and SQL, Documents and Graph each have a TCP server, so the area
   needs the role projects for them — a server host and the per-model adapters — written against whatever API is
   on `main` at the time. The earlier attempt is on the `feature/database-benchmarks` branch
   (commit `ab502c7`): a runner, a `dbbench` CLI, three model adapters, loaders and verification digests, roughly
   20,000 lines. It was built against a Cohesion **source checkout**, which this repository's conventions replace
   with CI packages, and against a pre-T1 language surface, so it is a reference to read rather than code to
   restore.
2. **The operations, reclassified.** T1 (complete 2026-10-18 on #1168) moved SQL from 33 of 49 executable clauses
   to 51 of 57, OQL to 31 of 32 and GQL to 26 of 33, and gave Documents a network server and client. Most of the
   operations the paused run had to emulate or skip are now native, so the per-model support tables in
   [docs/SQL.md](docs/SQL.md), [docs/DOCUMENTS.md](docs/DOCUMENTS.md) and [docs/GRAPH.md](docs/GRAPH.md) describe
   a surface two versions out of date and have to be redone against the pin that is current then.
3. **A graph scale that fits.** `GraphStore` builds an in-memory dictionary over every node and relationship when
   a database opens, so the 10 GB graph (262 million records) needs roughly 13.6 GB for that map alone and cannot
   open inside the 6 GB budget. Filed as
   [#1535](https://github.com/assimalign/cohesion/issues/1535). Until it lands the graph comparison runs at a
   smaller scale, which is a fair comparison but not a 10 GB one.

## What the paused run found

The run never reached full scale, but its development-scale pass and a root-cause analysis produced the
**Database Hardening** epic, [#1511](https://github.com/assimalign/cohesion/issues/1511), with features
#1512–#1519 and 28 tasks #1520–#1547. The analysis of record is [docs/HARDENING.md](docs/HARDENING.md) and
[docs/hardening-inventory.json](docs/hardening-inventory.json): 53 findings, 28 filed as new work, 25 matched to
existing issues and deliberately not refiled.

T1 resolved many of the 25 matched items (they were T1's own tickets) but **none of the 28 new tasks**, because
T1 changed the language surface and left the planner and executor alone. Re-verified after T1: `UPDATE` and
`DELETE` still scan to find their targets, there is still no cost model, no hash join, no index-only scan and no
plan cache, `LIMIT` still materializes everything before windowing, rows are still decoded before the MVCC
visibility test, the graph candidate budget is still charged before relationship type and direction are filtered,
and the buffer pool still serializes page I/O under one lock. The three highest-value items were filed from code
review before this suite existed and the benchmark supplied their first quantified impact:
[#1280](https://github.com/assimalign/cohesion/issues/1280) (read-only transactions force a durable journal
flush — about 99% of a document point read's wall time),
[#1276](https://github.com/assimalign/cohesion/issues/1276) (journal append holds its lock across fsync) and
[#1274](https://github.com/assimalign/cohesion/issues/1274) (`UPDATE`/`DELETE` scan instead of seek).

## Historical records

These describe the paused run against commit `bf20be5eb` and are kept as evidence, not as current guidance:
[docs/METHODOLOGY.md](docs/METHODOLOGY.md), [docs/RUNBOOK.md](docs/RUNBOOK.md),
[docs/REHEARSAL-R5.md](docs/REHEARSAL-R5.md), [docs/SMOKE-R1A.md](docs/SMOKE-R1A.md), and the per-model
[SQL](docs/SQL.md), [DOCUMENTS](docs/DOCUMENTS.md) and [GRAPH](docs/GRAPH.md) documents. Each carries a header
saying so.
