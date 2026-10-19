# Cohesion capability baseline

What the Cohesion database engines can and cannot express, so the comparison's operations can be classified
**native**, **emulated** or **unsupported** (DESIGN.md §6) without guessing, and so the hardening analysis starts
from evidence.

**Two versions matter, and they differ enormously.**

| | Commit | Where it is | What the suite sees |
| --- | --- | --- | --- |
| **`main`** | `ff7a93e80` | The CI packages this repository pins (`CohesionPackageVersion`) | Interface-first API, pre-T1 language surface. **What the BenchmarkDotNet suite compiles against today.** |
| **T1 tip** | `52edc41c8` | Branch `claude/database-inventory-sql-expansion-aa27d1`, PR #1168, awaiting owner sign-off | Concrete-first API, the full T1 language surface. **What the comparison is designed for.** |

The table below is the **T1 tip**. Re-verify it whenever #1168 moves or merges; the paths are relative to
`resources/Database/` in cohesion unless another repository is named.

> The earlier edition of this file described commit `bf20be5eb`. T1 landed on 2026-10-18 and moved SQL from 33 of
> 49 executable clauses to **51 of 57**, OQL to **31 of 32** and GQL to **26 of 33**, so most of that edition's
> "unsupported" rows are now native. What did **not** change is the planner and executor: every performance
> limitation it recorded is still present.

## The API migration #1168 will force

`main` is interface-first; the T1 tip is concrete-first. `bf20be5eb` is **not** an ancestor of `main`, so this is
not a fast-forward. Everything in this table breaks at the moment #1168 merges, including the three benchmarks
already in the suite.

| Concern | `main` (pinned today) | T1 tip |
| --- | --- | --- |
| Engine creation | `SqlDatabaseEngine.Create(options)` with `options.EngineName` | `SqlDatabaseEngine.Create(name, options)`; no `EngineName` on options |
| Engine base | `: IDatabaseEngine` | `: DatabaseEngine` |
| Database | `IDatabase` | sealed `SqlDatabase` etc., leaves of `DatabaseInstance` |
| Session | `IDatabaseSession` | `SqlDatabaseSession : DatabaseLanguageSession<SqlQueryRequest>`; key-value is `DatabaseExecutionSession<KeyValueRequest>` and has **no text overload** |
| Transaction | `IDatabaseTransaction` | sealed leaves of `DatabaseTransaction` |
| Statement result | `QueryResult` / `QueryResultSet` / `QueryRow` | `DatabaseResult` / `DatabaseResultSet` / `DatabaseRow` |
| Key-value get/put | on `IKeyValueDatabase`, takes `IDatabaseSession` | on `KeyValueDatabase`, takes `KeyValueDatabaseSession` |

The same rename hit the `Execution` child root wholesale: `QueryRequest`/`QueryResult`/`QueryResultSet`/
`QueryColumn`/`QueryRow`/`QueryResultStatus` became `Database*`. `DatabaseType.Character` (19) was appended, so an
exhaustive `switch` over `DatabaseType` stops compiling. `GqlNodePattern.Properties` and
`GqlRelationshipPattern.Properties` became `IReadOnlyDictionary<string, GqlExpression>`
(`Graph.Language/docs/DESIGN.md:16`).

## SQL

**Now supported** (was not at `bf20be5eb`): N-way `INNER JOIN` in written order, `LEFT` and `CROSS` joins, `JOIN …
USING`, non-recursive `WITH`, correlated subqueries (including in `UPDATE`/`DELETE` `WHERE`), aliased derived
tables, `COUNT(DISTINCT x)`, `EXTRACT` with the full temporal family (`NOW`, `CURRENT_TIMESTAMP`/`_DATE`/`_TIME`,
`LOCALTIME(STAMP)`, `DATE_TRUNC`), the string family (`SUBSTRING`, `TRIM`/`LTRIM`/`RTRIM`, `REPLACE`,
`CONCAT`/`CONCAT_WS`, `POSITION`, `STRPOS`, `LEFT`, `RIGHT`, `LPAD`, `RPAD`, `REVERSE`, `CHAR_LENGTH`), the numeric
family (`ROUND`, `FLOOR`, `CEILING`, `POWER`, `SQRT`, `MOD`, `ABS`, `SIGN`, `EXP`, `LN`, `LOG10`),
`UNION`/`INTERSECT`/`EXCEPT` (ALL or DISTINCT), `ORDER BY … NULLS FIRST/LAST`, `LIKE … ESCAPE`,
`IS [NOT] DISTINCT FROM`, `INSERT … ON CONFLICT` (DO NOTHING / DO UPDATE with `EXCLUDED`), `RETURNING`,
read-only `CREATE [OR REPLACE] VIEW`, identity columns, typed literals (`DATE '…'`, `TIMESTAMP`, day-time
`INTERVAL`, `X'…'`, `N'…'`), multi-word type names (`DOUBLE PRECISION`, `TIMESTAMP WITH TIME ZONE`,
`CHARACTER VARYING`), and **`EXPLAIN`**, which returns the chosen plan without executing it
(`Sql/docs/DESIGN.md:484`; estimates and actual row counts are always null in T1).

`CHAR(n)` is now a **distinct blank-padded type** (`DatabaseType.Character`, `Sql/docs/DESIGN.md:133`): it pads to
`n` Unicode scalars, non-space overflow is `COHSQLE012`, and index keys trim before encoding. A schema that used
`CHAR` as a synonym for `VARCHAR` changes meaning.

**Still not supported:** `RIGHT`/`FULL OUTER`/`NATURAL`/`LATERAL` joins, `RECURSIVE` CTEs, window functions
(all nine names reject at parse time), `ILIKE`, `SIMILAR TO`, `::` casts, quantified comparisons (`= ANY`),
subqueries in `UPDATE SET`, `UPDATE … FROM`, `MERGE`, `TRUNCATE`, `GRANT`, general sequences,
`SELECT` without `FROM` at top level (`COHSQLE013`, `Sql/src/Internal/SqlPlanner.cs:157`), savepoints and
`SET TRANSACTION`, `Serializable` isolation (rejected at begin, `Sql/src/SqlDatabaseSession.cs:165`), and **any
bulk-load or `COPY` path** — there is no such grammar and no bulk member on the engine, database, session or
client. Isolation levels available: `ReadCommitted` and `Snapshot` (default). Non-goals are listed at
`Sql/docs/DESIGN.md:5017`.

### Planner and executor — unchanged by T1

Every one of these was true at `bf20be5eb` and is still true, which is why the filed performance work stands:

- **`UPDATE`/`DELETE` collect targets with a full scan**, even on an indexed equality
  (`Sql/src/Internal/SqlPlanExecutor.cs:431` and `:559`); access-path selection is SELECT-only
  (`Sql/docs/DESIGN.md:2682`).
- **No cost model and no statistics**; selection is rule-based (`Sql/docs/DESIGN.md:2666`).
- **No hash or merge join** — nested loop with a scan or correlated index-seek child only.
- **No index-only or covering scan**: "a seek always fetches the row" (`Sql/docs/DESIGN.md:5025`).
- **`LIMIT` still materializes everything first**: the whole row list is projected, sorted and deduplicated, then
  windowed with `GetRange` (`Sql/src/Internal/SqlPlanExecutor.cs:188`). `FETCH FIRST` and `TOP (n)` are new
  spellings onto the same fields, not a top-N operator.
- **Rows are decoded before the visibility test**: `DecodeRow` runs, then `snapshot.IsVisible(writer)`
  (`Sql/src/Internal/SqlPlanExecutor.cs:1433`).
- **No plan cache and no prepared statements**; the wire protocol has no Prepare/Bind/Execute message.
- **No query parallelism**; only engine-owned background workers are threaded.
- **Views cost O(body rows) per reference**, with no merging or predicate pushdown
  (`Sql.Language/docs/DIALECT.md:305`).

## Documents

**The big change: a network server and client now exist.** `DocumentDatabaseServer.Create(engine, options)` with
`AddServer(...)` registration (`Documents/src/Server/DocumentDatabaseServer.cs`) and a `DocumentClient` /
`DocumentConnection` / `DocumentResultSet` client (`Documents.Client/src/`). Issue #190 is closed. At
`bf20be5eb` the client project held nothing but a `.csproj`, which is why the earlier design ran Documents
in-process only. **There is no transaction control over that wire** (`Documents/docs/DESIGN.md:1306`).

**Now supported:** `LIMIT`/`OFFSET`, array existential predicates (`ANY`/`SOME … SATISFIES`,
`EVERY … SATISFIES`, `UNNEST` / `INNER UNNEST` / `LEFT [OUTER] UNNEST`, chainable), `IN`, `BETWEEN`,
`LIKE … ESCAPE`, `DISTINCT`, OQL `INSERT`/`UPDATE`/`DELETE`/`UPSERT` with `SET` and `UNSET`, **compound indexes**
over 1–32 ordered paths with longest-equality-prefix plus trailing-range seeks, **unique indexes**
(`COHDOC020` on violation), a large scalar function set, and specified MISSING semantics throughout
(MISSING ≠ NULL; projections omit MISSING).

**Still not supported:** `EXISTS` outside the DDL modifier, subqueries (the one excluded clause of 32), joins
(`UNNEST` is explicitly not a join), **array/multikey indexes** (index paths take a fixed subscript such as
`tags[0]`; non-scalar components are `COHDOC021`), any **C# patch API** (`DocumentCollection` is `GetAsync`,
`PutAsync` whole-content, `DeleteAsync` — `Documents/src/DocumentCollection.cs:75`; partial update exists only
through OQL), any **batch or bulk get/put**, and a key-lookup access path — "T1 has no key-lookup access path"
(`Documents/docs/DESIGN.md:880`), so `META().id` comparisons stay residual filters over a scan.

Documented limits: query inputs, results and whole JSON values are materialized in managed memory; `UNNEST`
multiplies input rows with **no row cap**; database-wide writer locking is conservative; no statistics, join,
subquery or external sort (`Documents/docs/DESIGN.md:1250`).

## Graph

**Now supported:** `ORDER BY` with ASC/DESC and NULLS FIRST/LAST, `LIMIT`/`SKIP`/`OFFSET`, aggregates
(`COUNT(*)`, `COUNT([DISTINCT] e)`, `SUM`, `AVG`, `MIN`, `MAX`, `COLLECT_LIST`) with ISO `GROUP BY` and the empty
grouping set, **query parameters** (`$name`/`@name`, named scalars plus list and map, validated while planning,
pinned by client/server wire tests), **variable-length / quantified paths** (`{m,n}`, `{m}`, `{m,}`, `{,n}`,
`{,}`, `+`, `*`, Cypher `*m..n`) with trail semantics and group variables, `OPTIONAL MATCH` including a leading
one, `WITH`/`UNWIND`/`LET`/`FOR`/`FILTER`/`NEXT`, and `SET`/`REMOVE` for properties and labels.

**Still not supported:** **shortest path** — the path-search prefixes (`ALL SHORTEST`, `ANY SHORTEST`,
`SHORTEST [k]`) are `COHDBL001` (`Graph.Language/src/GqlQueryParser.cs:377`). The length of a path you already
matched is available (`path_length` / `length(p)`, `Graph.Language/docs/DESIGN.md:638`), so a shortest-path
*distance* must be computed client-side over a quantified-path match — inside the bounds below. Also absent:
`NOT EXISTS` pattern predicates, `MERGE`, set operations, `HAVING`, statistical aggregates, unique constraints
("current node-property indexes are nonunique", `Graph/docs/DESIGN.md:962`), **explicit transactions over the
wire** (deliberately deferred: `BEGIN`/`COMMIT`/`ROLLBACK` do not parse and reserved byte 9 is a
`ProtocolViolation`, `Graph/docs/DESIGN.md:221`), and any **bulk import** API. Label indexes exist but only
through the C# API (`GraphSchema.CreateIndexAsync`, `Graph/src/GraphSchema.cs:215`), one property key each; there
is no `CREATE INDEX` GQL clause.

**The traversal bounds are unchanged and still the binding constraint:** 100,000 materialized path matches per
pattern and 1,000,000 candidate expansions per statement, each overflowing with `COHDBG004`
(`Graph/docs/DESIGN.md:568`). `LIMIT` cannot suppress them — "LIMIT 1 over more than 100,000 matches still fails
COHDBG004" (`Graph.Language/docs/DESIGN.md:843`) — because the executor materializes every stage eagerly
(`Graph/docs/DESIGN.md:374`). Quantified paths add a hard cap of 64 relationships. **Candidate budget is still
charged before relationship type and direction are filtered**: `budget.Consume()` at
`Graph/src/Internal/GraphPlanExecutor.Paths.cs:160` precedes the trail, label and endpoint tests at `:166`.

Also unchanged: `GraphStore` builds an in-memory dictionary over **every** node and relationship when a database
opens (`Graph.Storage/src/GraphStore.cs:27`, now a `VersionChain` per entry rather than a single reference), which
is what puts a 10 GB graph out of reach of a 6 GB budget. Defining N new labels in one statement costs time
quadratic in N — 1,000 labels take 7.5 s, 2,000 take 42 s (`Graph/docs/DESIGN.md:550`).

## Storage and schema formats

| | T1 tip | `bf20be5eb` | `main` |
| --- | --- | --- | --- |
| Record space format | **25** (`Sql/src/Internal/SqlRowCodec.cs:87`) | 10 | 3 |
| Compiled schema document | **`cohesion/database-schema/v7`** (`Sql.Schema/src/SqlCompiledSchema.cs:55`) | v4 | v1 |

An older file is **refused, with no upgrade path** — a deliberate pre-release decision
(`Sql/docs/DESIGN.md:2793`). `OpenDatabaseAsync` throws naming the database and both versions and tells you to
export and recreate; a cleanly closed older image is left byte-identical. Compiled schemas below the current
format are rejected without compatibility readers (`Sql.Schema/docs/DESIGN.md:26`). So any engine data this suite
loads must be regenerated whenever the format moves, and a dataset is never carried across a format bump.
