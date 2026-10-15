# Cohesion capability baseline

What the Cohesion database engines can and cannot express at the pinned commit **`bf20be5eb`** of
`claude/database-inventory-sql-expansion-aa27d1`. The benchmark targets use this to choose each operation's support
level (DESIGN §6), and the hardening analysis starts from it. Re-survey whenever the pinned commit moves.

Paths are relative to the Cohesion repository root. Aliases:
`DIA` = `resources/Database/Assimalign.Cohesion.Database.Sql.Language/docs/DIALECT.md`,
`SQD` = `resources/Database/Assimalign.Cohesion.Database.Sql/docs/DESIGN.md`,
`SQW` = `resources/Database/Assimalign.Cohesion.Database.Sql/docs/WIRE-PROTOCOL.md`,
`PEX` = `resources/Database/Assimalign.Cohesion.Database.Sql/src/Internal/SqlPlanExecutor*.cs`,
`PLN` = `resources/Database/Assimalign.Cohesion.Database.Sql/src/Internal/SqlPlanner.cs`,
`STD` = `resources/Database/Assimalign.Cohesion.Database.Storage/docs/DESIGN.md`,
`DOD` = `resources/Database/Assimalign.Cohesion.Database.Documents/docs/DESIGN.md`,
`DLD` = `resources/Database/Assimalign.Cohesion.Database.Documents.Language/docs/DESIGN.md`,
`GRD` = `resources/Database/Assimalign.Cohesion.Database.Graph/docs/DESIGN.md`,
`GLD` = `resources/Database/Assimalign.Cohesion.Database.Graph.Language/docs/DESIGN.md`.

## SQL

| Area | Supported | Not supported |
|---|---|---|
| Types | TINYINT–BIGINT, DECIMAL/NUMERIC (p ≤ 28), REAL/DOUBLE (computed in Decimal), CHAR/VARCHAR/TEXT (one string type), BOOLEAN, DATE, TIME, TIMESTAMP, TIMESTAMPTZ, INTERVAL, UUID, BINARY, JSON storage (DIA:1265-1280, 1446-1449) | multi-word type names such as `DOUBLE PRECISION` (`Sql.Language/src/SqlTypeNames.cs:31-48`); typed literals `DATE '…'` (DIA:1666-1667); JSON functions (DIA:1394-1398); identity/sequence columns (DIA:40-69) |
| Limits | — | a row must fit one 8 KiB page record, no overflow storage (`Database.Storage/src/Storage.cs:1703-1706`); index key ≤ 1,024 bytes (`Database.Indexing/src/Internal/BTreeNode.cs:85`); one statement per request, wire frame ≤ 16 MiB (DIA:83-84, SQW:68-69) |
| DDL | CREATE TABLE with PK (incl. composite), NOT NULL, UNIQUE, CHECK, DEFAULT, FOREIGN KEY (CASCADE/RESTRICT); CREATE [UNIQUE] INDEX on several columns (DIA:46-50, 65-67) | DESC/expression/INCLUDE/partial indexes (DIA:49); online index build — CREATE INDEX takes an exclusive table lock (SQD:2281-2291); TRUNCATE, MERGE, views (DIA:64, 69); DDL inside a transaction (SQD:3007-3010) |
| DML | multi-row INSERT VALUES, INSERT … SELECT, UPDATE/DELETE with WHERE, parameters `@name`/`$1` (DIA:43-45, 159, 961-969) | UPDATE … FROM, subqueries in UPDATE/DELETE, ON CONFLICT/UPSERT, RETURNING (DIA:57, 59, 125-126); **any bulk-load API** — the client has only Query/Execute/ExecuteScalar (`Database.Sql.Client/src/SqlConnection.cs:82-162`) |
| Joins | **exactly two tables** with INNER JOIN … ON (DIA:55) | LEFT/RIGHT/FULL/CROSS/NATURAL/USING joins and joins of three or more tables (DIA:616-618) |
| Query | GROUP BY/HAVING, ORDER BY, LIMIT/OFFSET, DISTINCT, CASE, COALESCE, LIKE, uncorrelated scalar/IN/EXISTS subqueries, CAST (DIA:42-57, 1208, 1384-1398) | COUNT(DISTINCT) (DIA:928-931); SELECT without FROM (DIA:42); correlated subqueries, derived tables, ANY/ALL, LATERAL (DIA:971-990); CTEs, window functions, UNION/INTERSECT/EXCEPT (DIA:61-63); NULLS FIRST/LAST, FETCH/TOP (DIA:52, 58); ILIKE (`Sql.Language/src/SqlLanguageProfile.cs:34`) |
| Functions | UPPER, LOWER, LENGTH, ABS, COALESCE, `||`; COUNT, SUM, AVG, MIN, MAX (DIA:1200, 1495-1511) | recognized but not executable: NULLIF, **EXTRACT**, NOW, CURRENT_DATE/TIME/TIMESTAMP, SUBSTRING, TRIM, REPLACE, CONCAT, ROUND, FLOOR, CEILING, POWER, SQRT, MOD (DIA:206-208, 853-860, 1572-1581); user functions can be registered through `SqlDatabaseEngineBuilder.Functions` (DIA:1493-1497) |
| Planner | rule-based; index seek for single-table SELECT on the longest equality prefix plus one range column (SQD:1640-1654, PLN:374-430); index nested-loop join when ON matches an index prefix (DIA:596-614) | statistics/cost model (SQD:1583-1591); **UPDATE and DELETE always scan** (SQD:1656-1657, PEX:360-366, 472); IN-lists, OR, LIKE prefixes fall back to scans; hash and merge joins (PEX.Joins:44-91); index-only scans (SQD:3851-3852); top-N — LIMIT applies after full materialization and sort (PEX:131-186); parallel query; EXPLAIN (event source only); prepared statements/plan cache (SQD:1993-1994) |
| Transactions | BEGIN/COMMIT/ROLLBACK; Snapshot (default), ReadCommitted via the C# API; row locks + intent locks, deadlock detection (SQD:2143-2214) | Serializable, SET TRANSACTION, SAVEPOINT (SQD:2980-2985); concurrent writers — **one writer statement applies at a time per database** (SQD:2186-2214) |

## Documents

- **No network server or client.** "Documents has no wire server or client yet" (DOD:220, 622, 652);
  `Database.Documents.Client` has no source. In-process only. Tracked as #190 ("Document client APIs").
- **OQL supported:** SELECT, FROM (one collection), WHERE, GROUP BY, HAVING, ORDER BY, nested paths, fixed array
  subscripts, AND/OR/NOT, IS [NOT] NULL, arithmetic, aggregates, parameters (DLD:38-47, 107-147; DOD:292-317).
- **OQL not supported:** **LIMIT/OFFSET**, UNNEST, ANY/EVERY … SATISFIES (array predicates), subqueries,
  UPSERT/MERGE, DISTINCT, IN, LIKE, BETWEEN, EXISTS, non-aggregate functions, joins, OQL INSERT/UPDATE/DELETE
  (DLD:48-62; DOD:278-280).
- **Indexes:** single path, non-unique, boolean/number/string keys ≤ 1,024 bytes; no compound, unique, multikey or
  expression indexes (DLD:46; `Database.Documents.Catalog/docs/DESIGN.md:100-118`); equality and range use (DOD:354-357).
- **API:** GetAsync, PutAsync (whole-document replace, optional expected version), DeleteAsync; **no patch, no batch
  API** (`Database.Documents/src/DocumentCollection.cs:75, 109, 154`; DOD:363-370).
- **Execution:** materializes all input and output in memory, no external sort, no statistics (DOD:618-623); one
  writer at a time per database (DOD:80-86, 99-115).

## Graph

- **GQL that executes:** MATCH (fixed patterns up to 64 relationships, any direction, inline property maps, named
  paths), WHERE with AND only, RETURN, INSERT/CREATE, DELETE, DETACH DELETE, SHOW, label expressions; MATCH followed
  by INSERT connects matched nodes (GLD:75-105, 127-130; `Database.Graph/tests/GqlProfileExecutionTests.cs:55`).
- **Not supported:** OPTIONAL MATCH, **variable-length/quantified paths, shortest path**, **parameters**, functions,
  **aggregates, ORDER BY, LIMIT/SKIP**, OR, set operations, MERGE, WITH/UNWIND (keywords only), SET (GLD:173,
  198-206; GRD:108, 411-414). Variable-depth traversal exists only as the C# `TraverseAsync` (GRD:392-398).
- **Hard per-statement bounds:** 100,000 intermediate matches and 1,000,000 examined candidates, then COHDBG004
  (GRD:386-388; `Database.Graph/src/Internal/GraphPlanExecutor.cs:17, 245`).
- **Indexes:** only through C# `GraphSchema.CreateIndexAsync(label, name, propertyKey)`; one property, non-unique,
  equality only (`Database.Graph/src/GraphSchema.cs:215-221`); **no label index** — a label scan reads every node
  record and sorts (`Database.Graph.Storage/docs/DESIGN.md:190-192`).
- **Load:** no bulk import; multi-path INSERT statements or per-element `CreateNodeAsync`/`CreateRelationshipAsync`
  (`Database.Graph/src/GraphDatabase.cs:234, 314`); introducing N new labels in one statement is O(N²) (GRD:377-380).
- **Open:** the store rebuilds an in-memory id map in O(V+E+I) on open (`Database.Graph.Storage/docs/DESIGN.md:190-192`).
- **Transactions:** in-process Snapshot/ReadCommitted with one writer lock per database; **none over the wire**
  (GRD:143-206; `Database.Graph.Client/docs/OVERVIEW.md:31-32`).

## Servers, clients, storage

- SQL server: `SqlDatabaseServer.Create(engine, options)`; `Listen(Uri)` from `Database.Sql.Tcp` needs an IP
  literal, `localhost`, `0.0.0.0` or `::` and a non-zero port; the database must exist before a client connects;
  authentication defaults to allow-all; no TLS (SQD:2887-2889, 2949-2954).
- Graph server: no TCP helper; give `GraphDatabaseServerOptions.Listener` a `TcpConnectionListener`.
- Clients: settings keys `Database`, `Principal`, `Endpoint` (default port 5740), `MaxPoolSize` (default 8); one
  request at a time per connection, no pipelining, full result materialized client-side
  (`Database.Client/src/DatabaseConnectionSettings.cs:27-149`; SQW:47).
- Engine options (SQL; Graph and Documents match except the expression limit): `Durability` (Synchronous on durable
  storage by default; Grouped; None), `GroupCommitWindow` 5 ms, `CheckpointInterval` 5 min,
  `CheckpointJournalSize` 256 MiB, `BufferPoolCapacity` **32 MiB per database**, `PageWriteBackInterval` 1 s,
  `PageWriteBackBatchSize` 16, `MaintenanceInterval` 60 s, `RootPath` (null = in-memory)
  (`Database.Sql/src/SqlDatabaseEngineOptions.cs:32-196`).
- Fixed: 8 KiB pages, 1 MiB catalog pool, buffered OS I/O (no write-through/unbuffered option), synchronous
  `FlushToDisk` on the async path, no compression (STD:677-683, 853; `FileSystem.Physical/src/Internal/PhysicalFileSystemFile.cs:74-75`).
- Known costs documented by the design: one global buffer-pool lock with synchronous miss reads (STD:329-357);
  checkpoint stalls of 241–267 ms every 256 MiB of log (STD:1684-1693); UNIQUE checks slow as dead versions
  accumulate (SQD:2387-2389); scans decode every row version before filtering (PEX:1405-1440); per-row arrays,
  boxing and an interpretive evaluator (SQD:3870-3874).
