# Graph suite: Cohesion Graph and Neo4j

R4 implements the social workload against Cohesion **bf20be5eb452fa57363f412bdcff36b2dc2e7edd**
and Neo4j 5 Community. The benchmark base is **87afd6d**. The model registers all thirteen operations;
unsupported operations remain visible in verification, runs, mixed-workload blocking, and reports.
The source checkout is read-only. No engine patch, result cache, client join, or normalization change is used.

**The all-green acceptance condition is blocked at the pinned engine revision.** All three adapters load and run,
and every completed verification case has the same digest. Two embedded operations fail the engine's statement
bounds at SF0.01, and the first combined embedded throughput process exhausted memory. These failures are
retained in the report. No wrong-result case was observed. The evidence and remaining owner decisions are below.

## Mapping and indexes

[`SocialSchema`](../src/Assimalign.Cohesion.Benchmarks.Database.Datasets/Social/SocialSchema.cs) is the only
source of labels, relationship endpoint labels, property names, and index definitions.
[`GraphMapping`](../src/Assimalign.Cohesion.Benchmarks.Database.Graph/GraphMapping.cs) decodes every property
and generates Neo4j typed import headers and constraints/indexes, and Cohesion C# index calls.

The eight labels are Place, Organisation, TagClass, Tag, Person, Forum, Post, and Comment. The twenty-three
relationship files map to their declared types; KNOWS is one directed stored edge, matched in either direction.
Dataset ids are scoped to labels, including overlapping Post and Comment ids. Neo4j import ID spaces preserve
these scopes. The Cohesion API loader holds a `(label, dataset id) -> GraphNodeId` dictionary during ingestion.

**Every DATE/TIMESTAMP becomes signed int64 UTC epoch milliseconds on both engines**, including Person.birthday,
all creationDate properties, and HAS_MEMBER.joinDate. A birthday is midnight UTC; dates before 1970 remain negative.
Integer/year properties also use int64. NULL means absent property; an empty string remains present and empty.
No engine-specific temporal normalization occurs in verification.

Both engines receive these eleven label/property pairs: id on all eight labels, Person.firstName,
Post.creationDate, and Tag.name. Neo4j's eight uniqueness constraints create their own indexes; the other three
are range indexes. Cohesion uses `GraphSchema.CreateIndexAsync` for all eleven, **non-unique and equality-only**.
It cannot enforce the same id uniqueness or accelerate date ranges. No index is added to compensate for a query feature.

## Exact operation semantics and support

N = native, E = bounded client completion, U = unsupported. The Cypher/GQL catalog below is generated from
[`GraphQueries`](../src/Assimalign.Cohesion.Benchmarks.Database.Graph/GraphQueries.cs), the code used by the driver.
Parameters, escaped literals, and statements are constructed by the deterministic parameter stream before timing.
Person, other-person, post, tag, and first-name-source ids are uniform over their generated ranges. Distinct
shortest-path endpoints are selected. The manifest seed, operation id, and client index key each stream.

| Operation | Neo4j | Embedded | TCP | Projection and total order / completion |
|---|---|---|---|---|
| person-profile | N | N | N | All Person properties, city id/name; one row by unique dataset id |
| person-recent-messages | N | E | E | kind, message id, creationDate, author id, content; date DESC, id DESC, kind ASC; first 10 |
| person-friends | N | E | E | friend id/names and friendship date; date DESC, friend id ASC |
| friends-recent-posts | N | E | E | Distinct posts with the message projection/order; first 20 |
| fof-recent-messages | N | E | U | Distinct messages by persons at distance 1 or 2, excluding the start person; message order, first 20 |
| friends-within-3-by-name | N | E | U | Matching person id/names and minimum distance 1..3; distance ASC, id ASC; first 20 |
| shortest-path-knows | N | U | U | One shortest length, NULL when disconnected |
| friend-recommendation | N | U | U | Candidate id/names and distinct mutual-friend count; exclude self/direct friends; count DESC, id ASC; first 10 |
| tag-cooccurrence | N | E | E | Other tag id/name and distinct co-tagged post count; count DESC, id ASC |
| country-person-count | N | E | E | Country id/name and distinct person count through city; country id ASC |
| add-post | N | N | N | New post id/date, creator id, tag id, returned by the committed insertion |
| add-like | N | N | N | Person id, post id, creationDate returned by the committed edge insertion |
| add-friendship | N | N | N | Endpoint ids and creationDate returned by the committed edge insertion |

Message queries use a **single `Post|Comment` label expression** and the path API. Its node labels distinguish
overlapping message id spaces without a driver join or an invented schema property. The client projects the
selected path to the common scalar row and sorts it. Friends-of-friends uses two fixed patterns in one embedded
snapshot transaction, followed by DISTINCT aggregation and top-N. The three-distance name search uses three
fixed patterns in one transaction with the firstName and self-exclusion predicates in every engine statement.
No failed statement is retried with a narrower pattern.

Tag and country queries let the engine select every endpoint/path and selective predicate. The client counts
selected ids, orders the groups, and emits exactly the same scalar projection as Cypher. Intermediate statement
bounds are ordinary errors; an incomplete result is never emitted as a successful digest.

### Evidence that changes the brief's expected baseline

Paths below are relative to `COHESION_ROOT/resources/Database/` at the pinned commit.

* `Assimalign.Cohesion.Database.Graph.Language/docs/DESIGN.md:98-115` describes AND-only comparisons,
  fixed paths, and projections. Live-engine probes return `GQL parse error COHDBL001` for ORDER BY, LIMIT,
  FUNCTION count, PARAMETER, QUANTIFIED PATTERN OR STAR PROJECTION, ANY SHORTEST, and NOT.
* **Recommendations cannot be emulated legally.** The selective exclusion of direct friends needs a negative
  relationship predicate. `WHERE NOT EXISTS { MATCH (p)-[:KNOWS]-(c) }` returns
  `The NOT clause is not supported by the GQL surface of this database model.` Doing this exclusion in the
  client would be an anti-join, forbidden by DESIGN §6.
* **The public breadth-first API cannot produce a native shortest length.**
  `Assimalign.Cohesion.Database.Graph/src/GraphDatabase.cs:368-415` internally queues `(Id, Depth)` but returns
  only `GraphNode` values, discarding depth and predecessor. `src/ValueObjects/GraphTraversal.cs:11-15` supplies
  a maximum depth, not a destination or distance projection. The committed test constructs a chain 1–2–3 and a
  star 2–1–3: a single traversal returns the same `[2,3]` node sequence, although distance(1,3) differs.
  Repeated depth probes or a client graph search would not meet the native/allowed-completion rules.
* **TCP cannot combine the fixed-length statements in a transaction.**
  `Assimalign.Cohesion.Database.Graph.Client/src/GraphConnection.cs:13-16` explicitly excludes wire transactions.
  Consequently fof-recent-messages and friends-within-3-by-name are unsupported over TCP. Single-pattern
  Post|Comment reads avoid this limitation while preserving exact message semantics.
* Statement limits are 100,000 intermediate matches and 1,000,000 candidates
  (`Assimalign.Cohesion.Database.Graph/src/Internal/GraphPlanExecutor.cs:17,245-248`). Bounds remain errors.
  Candidate accounting happens before the relationship-type check (`GraphPlanExecutor.cs:157-160`), so
  incident relationships of other types consume the budget too. WHERE predicates run after the matching
  bindings have been materialized (`GraphPlanExecutor.cs:51-57`).

## Ingest and mutation isolation

Neo4j full load converts the RFC 4180 dataset into separate typed header and data files, with epoch conversion
already applied. The host adapter stops its owned Neo4j container and runs `neo4j-admin database import full`
inside a temporary Neo4j container with the **same CPU/memory/swap budget**, mounting the named database and
dataset volumes. The database starts after import. Conversion, volume staging, import, and index finalization
are separate load metrics. Startup/shutdown time is excluded. The host orchestration is necessary because the
published driver has no Podman executable; network driver runs use the imported database.

The import uses integer ID spaces, multiline RFC CSV, default empty-string preservation, and a 1 GiB
off-heap import budget plus a 512 MiB Java heap at the 2 GiB smoke limit. The same proportions (50% off-heap,
25% heap, 25% native overhead) follow the observed container limit for larger runs. See
[Neo4j full import](https://neo4j.com/docs/operations-manual/current/import/full-import/) and
[typed header format](https://neo4j.com/docs/operations-manual/current/import/header-format/).
The database name precedes the variable-arity file options; putting it last lets Neo4j parse it as a CSV filename.

All sample paths select the first 100,000 posts and every KNOWS edge touching the first 100,000 persons, capped
by actual dataset counts. They include those posts' HAS_CREATOR and HAS_TAG edges. Person/Tag endpoint setup
is excluded from sample timings. Neo4j uses `UNWIND $rows CREATE` and `UNWIND $rows MATCH ... CREATE`, with
the unchanged dataset ids and properties on an initially empty, indexed database. It deletes the sample outside
timing, then offline import replaces those stores with the full baseline. This ordering accommodates Community's
single user database without changing any sampled property. Sample indexes/setup are excluded from sample timings;
the final full-load index build is measured after offline import.

The embedded loader measures API-per-element and multi-path INSERT candidates on separate sample databases,
both in explicit transactions. It retains the faster measured path for the full load and records both candidate
metrics as `IsSample`. API loads create indexes after ingest; if GQL wins, id indexes precede relationship MATCH
statements and their build time is included, with the remaining indexes built afterwards. No privileged bulk API
or internal store access is used. The TCP sample uses a separate embedded-prepared `benchmark-tcp-ingest`
database, with Person/Tag endpoints and equivalent id/name/date indexes. It uses inlined escaped INSERT
statements and records `tcp-ingest`. It never reloads or mutates the served baseline.
The smoke restarts the TCP server after that sample, retaining the volume: otherwise the server's cached
sample database and its identity map would consume part of the baseline workload's memory budget.
Baseline storage accounting includes only the `benchmark/` file set, excluding this sample database.

Normal writes use `ParameterStreamContext.WriteId`, with post ids above the generated maximum. Their date is
the deterministic synthetic epoch-millisecond value `1900000000000 + write id`, also uniquely identifying
new edges. R5 removes client cleanup statements: inserted posts and edges persist after disposal. All read latency
and throughput passes precede writes and mixed passes. Verification writes use a disposable store; TCP
serves the pristine embedded-loaded baseline. No later read-only measurement uses the changed working store. The harness refuses mixed workloads containing unsupported operations.

## Validation and development smoke

These are development runs at SF0.01,
3 CPUs and 2 GiB per engine, with swap limited to the same 2 GiB. They are not official performance comparisons.
Only one R4 engine is active at a time. Other authorized sessions share this machine; the smoke script records
their active container names at each phase's start. This shared-host activity prevents treating these timings
as quiet-host performance results.
The dataset has 300 persons, 12,000 posts, 30,000 comments, and 5,104 KNOWS edges; seed 20261015;
fingerprint `dedee2fc5f4df8378982ace82508e19cddfc0fc7d966cfa241b4af1b67b3aaa7`.

The typed import reported **43,595 nodes, 220,155 relationships, and 338,238 properties**. Neo4j is 5.26.32.
Its import wall time was 24.356266 s (the importer's own internal timer reported 10.733 s), with peak reported
import memory 1.033 GiB. CSV conversion took 0.737195 s, staging 6.682039 s, and indexes 3.271214 s.
Online sample ingestion took 3.860364 s for 12,000 posts and 33,956 relationships, before full import.
Full-load stored bytes were 565,321,895; Neo4j's default transaction-log preallocation remains enabled.
An earlier run using remapped sample ids was superseded and excluded from report inputs.

Cohesion's calibration sample measured API 2.267639 s and GQL 2.126626 s.
GQL won and loaded the complete dataset in 14.153926 s, with 0.734861 s for index preparation
and finalization. The full load's console total, 19.283 s, also includes both calibration samples; it is not
just the selected full-ingest path. Stored baseline bytes were 236,236,967. The final index metric checks all
eleven definitions after eight id indexes were built earlier, so its "rows" must not be added to the earlier
eight to infer the number of distinct indexes.

Neo4j passed **100 cases for all thirteen operations**, then latency and throughput at clients 1 and 8,
with zero measured/warmup errors. Embedded Cohesion passed all 100 cases for nine operations. Its
fof-recent-messages verification stopped on case 0 at 100,000 path matches; friends-within-3-by-name passed
four cases and stopped on case 4 at 1,000,000 candidate expansions. **All 904 completed embedded cases
matched Neo4j's digests.** Neither bound is a result mismatch or an unsupported classification.

The embedded latency pass recorded 12 measured fof errors and 7 measured name-search errors, plus 2 and 1
warmup errors respectively, with no timeouts. Other measured operations had no errors. The first combined
throughput attempt aborted with `OutOfMemoryException` at the 2 GiB limit and produced no suite result.
Throughput is therefore also attempted separately for each operation/client count, retaining process-level
failures as `*.aborted.json` rather than inventing histograms. The smoke report wrapper appends these fatal
attempts to Markdown and `findings.json`; it does not change verification or result normalization.

All 22 separate embedded throughput attempts completed. At clients 1/8, fof recorded 15/15 measured errors
and 2/7 warmup errors; the name search recorded 6/6 measured errors and 1/4 warmup errors. All were COHDBG004,
with zero timeouts. Every other attempted operation had zero errors. The earlier OOM did not recur in a fresh
process per operation/count; its precise allocation cause remains unisolated. These separate runs preserve
the logical parameter streams but change process/cache history, so they are diagnostic smoke results.

A post-run audit reopened the named volume with public APIs, checked all 31 file-derived counts and all eleven
indexes, and confirmed that write cleanup restored the full logical baseline. It reproduced these minimal
single statements at SF0.01:

```gql
MATCH route=(p:Person {id:165})-[:KNOWS]-(:Person)-[:KNOWS]-(a:Person)<-[:HAS_CREATOR]-(m:Post|Comment) WHERE a.id <> p.id RETURN route
```

`COHDBG004: The query exceeded 100000 path matches.`

```gql
MATCH (p:Person {id:266})-[:KNOWS]-(:Person)-[:KNOWS]-(:Person)-[:KNOWS]-(f:Person {firstName:'Penelope'}) WHERE f.id <> p.id RETURN f.id,f.firstName,f.lastName
```

`COHDBG004: The query exceeded 1000000 candidate expansions.` The first statement uses
`GraphPathsQueryRequest.FromGql`; the second uses `GraphDatabaseSession.ExecuteAsync`.

The graph tests cover epoch values before/after 1970, literal round trips including Unicode/quotes/backslashes,
overlapping Post/Comment ids and tie ordering, multi-hop minimum distances, distinct aggregates, actual rejection
diagnostics, the traversal counterexample, parameter/write partition determinism, and committed-write cleanup.
The test project has one added Graph project reference. The CLI also records the observed CPU/memory budget;
the foundation previously stamped the default 6 GiB into 2 GiB smoke results. Unknown native limits are zero,
and the official-run guard is unchanged. These are the two required shared-file exceptions.

The store's open cost is explicitly measured: process working set, managed bytes, and elapsed time after open.
These include the host driver/runtime, and are not an allocation attribution. The underlying reason is
`Assimalign.Cohesion.Database.Graph.Storage/docs/DESIGN.md:190-192`: the engine rebuilds an in-memory identity
directory in O(V+E+I) and label scans read all node records and sort their matches. There is no label index.
After opening the loaded store for embedded latency, working set was **161,439,744 bytes (153.96 MiB)**,
managed memory 83,091,416 bytes (79.24 MiB), and open time 0.591959 s, with a 1 GiB configured buffer pool.
The store's label scan implementation is `GraphStore.cs:129-134`; this matters even when only 300 Persons
are wanted from a store containing 43,595 nodes.

TCP passed all **900 cases across its nine supported operations**, matching both other targets. Its sample
ingest took 9.012866 s: posts 3.649023 s, KNOWS 1.037989 s, HAS_CREATOR 2.411873 s, and HAS_TAG 1.913981 s.
After the sample and restart, server RSS was 173,540 KiB (**169.47 MiB**), before verification. Its latency
and throughput passes completed with zero measured/warmup errors or timeouts. The same baseline file set was 78,823,614 bytes
when the TCP load measured it, after embedded runs, cleanup, and reopen; these are file-length snapshots at
different lifecycle points, not an immutable storage-size comparison.

The 10-second duration applies to each measured phase. Warmup, open/reopen, and durable cleanup of thousands
of inserted records extend command wall time. Network driver containers cannot invoke host `podman stats`;
their resource samples retain `podman stats unavailable`, while host-side load samples and explicit engine
inspection capture the limits. No telemetry is filled in or inferred as a measured value.

The final comparison reports `VERIFY COMPARE: 2 mismatches.` Both entries are self-errors on the embedded target:
the two COHDBG004 failures above. There are **zero cross-target digest or row-count differences**: 1,300 Neo4j,
904 embedded, and 900 TCP completed cases. The Markdown report renders successfully. Its 88 finding entries
include repeated verification evidence attached to individual runs and one appended fatal-process entry;
they do not represent 88 distinct engine defects. Unsupported operations remain explicit and embedded runs
with incomplete verification are excluded from rankings.

The eight-client TCP results are unexpectedly slower for several operations: recent messages fall from
263.022 to 38.320 ops/s and add-post from 422.594 to 68.446 ops/s. The cause was not isolated on this shared
host. Likewise, the combined embedded `OutOfMemoryException` is a process-level finding, not an attribution
to a particular engine allocation: the process contains both the engine and the client completion buffers.

All R4 containers and named data/dataset volumes were removed after the final report. Pulled/built images
and ignored local evidence remain. The benchmark images record commit `1abba512853378ee6f87202143d47441708c9d3d`
with `benchmarksDirty=true`, because the smoke fixes were exercised before the fifth commit. They record
Cohesion `bf20be5eb452fa57363f412bdcff36b2dc2e7edd`, Release optimization, and actual 3 CPU / 2 GiB engine
limits. Driver runs also have a 2 GiB limit; the host-orchestrated Neo4j load records its native driver limit
as unknown (zero). Neo4j.Driver is 5.28.3. The Cohesion server image id is
`b78d57fd474efe6a62b1e3df10345fc2a6985a521599d50e3efcc2eb9e077578`.

### Commands and execution record

Run from the benchmark worktree. Entering `database/` for .NET commands selects the suite's .NET 10 SDK;
the repository root also contains an unrelated scaffold. All builds below are Release.

```powershell
$env:COHESION_ROOT = 'C:\cw\bench\coh-graph'
$env:DOTNET_SYSTEM_NET_DISABLEIPV6 = '1'
Push-Location database
dotnet build Assimalign.Cohesion.Benchmarks.Database.slnx -c Release
dotnet test tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release
Pop-Location
$cli = 'database/artifacts/bin/Assimalign.Cohesion.Benchmarks.Database.Cli/release/dbbench.dll'
dotnet $cli generate --model graph --scale 0.01 --out database/artifacts/r4-data
& database/scripts/Build-BenchImages.ps1 -Tag r4
& database/engines/neo4j/Invoke-GraphSmoke.ps1 -Phase neo4j -Suffix -r4 -Tag r4
& database/engines/neo4j/Invoke-GraphSmoke.ps1 -Phase embedded -Suffix -r4 -Tag r4
& database/engines/neo4j/Invoke-GraphSmoke.ps1 -Phase tcp -Suffix -r4 -Tag r4
& database/engines/neo4j/Invoke-GraphSmoke.ps1 -Phase report -Suffix -r4 -Tag r4
& database/engines/neo4j/Invoke-GraphSmoke.ps1 -Phase cleanup -Suffix -r4 -Tag r4
```

The smoke script uses `Start-BenchEngine.ps1 -Target <target> -Memory 2g -Suffix -r4 -Tag r4` and
`Stop-BenchEngine.ps1 -Target <target> -Suffix -r4 -RemoveData`. Inside each driver container it executes
the following CLI commands (with `/results/<target>.environment.json` for network load/run provenance):

```text
dbbench load --model graph --target <target> --suffix -r4 --data /data/graph/sf0.01 --buffer-pool-mb 1024 --batch 1000 --out /results
dbbench verify --model graph --target <target> --suffix -r4 --data /data/graph/sf0.01 --buffer-pool-mb 1024 --out /results/<target>.verify.json
dbbench run --model graph --target <target> --suffix -r4 --data /data/graph/sf0.01 --buffer-pool-mb 1024 --suite latency --clients 1 --duration 10s --warmup 1s --iterations 2147483647 --repeat 1 --out /results --verification /results/<target>.verify.json
dbbench run --model graph --target <target> --suffix -r4 --data /data/graph/sf0.01 --buffer-pool-mb 1024 --suite throughput --clients 1,8 --duration 10s --warmup 1s --repeat 1 --out /results --verification /results/<target>.verify.json
```

Neo4j's load runs on the Windows host instead, with the host dataset/result paths and `--batch 1000`.
After the combined embedded throughput abort, `-Phase embedded-throughput` ran the throughput command
separately with `--ops <operation> --clients 1` and `--clients 8` for each of its eleven N/E operations.
The committed `embedded` phase now uses those same isolated attempts, retaining any fatal exits.

This ledger includes failed and superseded attempts. Repeated identical builds are grouped; log names are
relative to `database/artifacts/`, which is deliberately ignored and not committed.

| Command / invocation | Result and evidence logs |
|---|---|
| `dotnet build database/src/Assimalign.Cohesion.Benchmarks.Database.Graph -c Release` (mapping, loaders; loader repeat with `--no-restore`) | Three passes, zero warnings/errors: `r4-mapping-build.log`, `r4-loaders-build.log`, `r4-loaders-build2.log` |
| `dotnet build database/Assimalign.Cohesion.Benchmarks.Database.slnx -c Release`, then `--no-restore` | First failed CS0428 (`Keys.Count`); fixed to `Keys.Length`; rebuild passed: `r4-operations-build.log`, `r4-operations-build2.log` |
| `dotnet build database/src/Assimalign.Cohesion.Benchmarks.Database.Cli -c Release --no-restore -p:BuildProjectReferences=false`, also with `-p:EnablePreviewFeatures=true` | Two failed attempts with CA2252 after cross-runtime publish; normal full build resolved them without suppression: `r4-import-fix-build.log`, `r4-import-fix-build2.log` |
| `dotnet build Assimalign.Cohesion.Benchmarks.Database.slnx -c Release` from `database/` (including `--no-restore` incremental repeats) | All passed, zero warnings/errors: `r4-build-after-publish.log`, `r4-pre-smoke-build.log`, `r4-budget-build.log`, `r4-verification-build.log`, `r4-smoke-ready-build.log`, `r4-storage-build.log`, `r4-final-smoke-build.log`, `r4-final-build.log`, `r4-sample-parity-build.log` |
| `dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release --filter FullyQualifiedName~GraphAdapterTests` (and `--no-restore` repeat) | First 13 pass / 1 fail from reserved GQL alias `path`; renamed to `route`; then 14 pass: `r4-graph-tests.log`, `r4-graph-tests2.log` |
| Same graph filter with `--logger 'console;verbosity=detailed'`; final `--no-build` repeat | Both passed all 15 after adding the BFS counterexample: `r4-graph-evidence.log`, `r4-graph-tests-final.log` |
| `dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release --no-build` | Initially 82 pass / 1 skip, then 83 pass / 1 skip after the added test: `r4-tests-all.log`, `r4-tests-complete.log`, `r4-tests-final-smoke.log`, `r4-final-tests.log` |
| `dotnet test tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release` from `database/` | Final code including original-id Neo4j sample: **83 pass / 0 fail / 1 skip, total 84**, `r4-final-parity-tests.log`. The unchanged skip is the opt-in scale-one dataset test |
| `dotnet $cli generate --model graph --scale 0.01 --out database/artifacts/r4-data` | Passed: 31 tables, 263,750 rows, 9,498,682 CSV bytes, `r4-generate.log`. Linux named-volume generation produced identical part hashes |
| `Build-BenchImages.ps1 -Tag r4` | Nine successful publish/build invocations: `r4-images.log`, `r4-images2.log`, `r4-images3.log`, `r4-images4.log`, `r4-images-budget.log`, `r4-images-verification-commit.log`, `r4-images-storage.log`, `r4-images-final-smoke.log`, `r4-images-sample-parity.log` |
| `Start-BenchEngine.ps1 -Target neo4j -Memory 2g -Suffix -r4`; `dotnet $cli ping --target neo4j --suffix -r4` | Ready; initial server stopped while waiting for the other sessions, `r4-neo4j-start.log`, `r4-neo4j-ping.log` |
| `dotnet $cli load --model graph --target neo4j --suffix -r4 --data database/artifacts/r4-data/graph/sf0.01 --out database/artifacts/r4-results --batch 1000` | First failed `File 'neo4j' doesn't exist`; fixed import positional-argument order, `r4-neo4j-load.log` |
| `Invoke-GraphSmoke.ps1 -Phase neo4j -Suffix -r4 -Tag r4` (initial attempt, then `-ReuseNeo4jLoad`) | Initial manifest-byte check caught CRLF/LF JSON formatting only; all part hashes matched. Script now compares all parsed fields and copies the exact host manifest. Successful earlier run was later superseded because its online sample remapped ids |
| Same Neo4j phase after preserving original sample ids, with a fresh full import | **Exit 0**: load 38.907 s / 565,321,895 bytes; verify 1,300 cases; latency 13 entries and throughput 26 entries, zero errors/timeouts. `r4-smoke-neo4j-final-phase.log`, `r4-smoke-neo4j-{load,verify,latency,throughput}.log` |
| `Invoke-GraphSmoke.ps1 -Phase embedded -Suffix -r4 -Tag r4` | Load passed; verify exit 2 with two engine bounds; latency exit 2 (19 measured errors, 3 warmup errors); first combined throughput aborted exit 1 with OOM. `r4-smoke-embedded-phase.log`, `r4-smoke-cohesion-graph-embedded-*.log` |
| `Invoke-GraphSmoke.ps1 -Phase embedded-throughput -Suffix -r4 -Tag r4` | Exit 2: all 22 processes completed; four FoF/name-search entries have 42 measured / 14 warmup COHDBG004 errors total; remaining eighteen entries have zero errors; zero timeouts |
| `dotnet publish artifacts/r4-probe/GraphSmokeProbe.csproj -c Release -r linux-arm64 --self-contained true -o artifacts/r4-probe-publish` | Scratch audit initially failed CS5001 because artifacts sources are excluded by default; explicit Compile Include fixed it. Publish passed: `r4-probe-build.log`, `r4-probe-build2.log` |
| Audit app `/probe/graph-smoke-probe` in the 3 CPU / 2 GiB owned driver container over the existing graph volume | Exit 0: all 31 counts, 11 indexes, and both exact statement-bound repros; `r4-store-audit.log` |
| `Invoke-GraphSmoke.ps1 -Phase tcp -Suffix -r4 -Tag r4` | Exit 0: sample load 9.012866 s; verify 900 cases; latency and throughput, clients 1/8, zero errors/timeouts. `r4-smoke-tcp-phase.log`, `r4-smoke-cohesion-graph-tcp-*.log` |
| `dotnet $cli verify --compare database/artifacts/r4-results --out database/artifacts/r4-results/comparison.json` | Exit 2 for the two embedded self-errors; zero differing completed digests. Run before and after final Neo4j replacement; `r4-report.log` records the final result |
| `dotnet $cli report --in database/artifacts/r4-results --out database/artifacts/r4-report` via `-Phase report` | Rendered successfully, exit 0; wrapper retains comparison exit 2 and appends the fatal attempt to Markdown/findings JSON |
| `Invoke-GraphSmoke.ps1 -Phase cleanup -Suffix -r4 -Tag r4`; `podman ps -a`; `podman volume ls` | Exit 0; no R4 containers or volumes remain. No other session's resources were stopped or removed |

The local [rendered report](../artifacts/r4-report/graph.md), [findings JSON](../artifacts/r4-report/findings.json),
[comparison](../artifacts/r4-results/comparison.json), [final digest audit](../artifacts/r4-final-verification-summary.log),
and [full test log](../artifacts/r4-final-parity-tests.log) remain available in this worktree.
Superseded Neo4j files are outside the report input in `artifacts/r4-superseded-neo4j-negative-sample/`.

### Scope changes and remaining decisions

The source/API evidence requires shortest-path and recommendation to be unsupported on both Cohesion targets,
and the two multi-statement traversals to be unsupported on TCP. A single Post|Comment label expression
replaces the brief's expected separate post/comment reads. GQL full-load id indexes precede relationship
inserts so their endpoint MATCHs use the same indexed lookup as other sample paths; the time remains counted.
The shared test-project reference and CLI observed-budget correction are the only changes outside Graph
ownership apart from the explicitly allowed Neo4j.Driver package entry.

The owner must decide how to address statement-bound feasibility and the missing shortest-distance,
negative-pattern, and wire-transaction APIs before demanding a complete graph comparison at this engine
revision. Increasing bounds blindly is not a demonstrated solution: the memory abort still needs isolation.
This benchmark does not relax the workload or reclassify failed emulation as unsupported. Follow-up work
should isolate the combined-process OOM and TCP concurrency slowdown, add host-side network-run telemetry,
deduplicate report verification evidence, then run quiet-host official repetitions with fresh stores and the
standard 3 CPU / 6 GiB budget. None of those investigations is implied by these smoke timings.

## Smoke timing observations

Raw single-repetition diagnostics on a shared host; embedded Cohesion is excluded from report rankings because verification is incomplete. Error cells are not successful-operation quantiles. See the generated report for percentiles, histograms, errors, and metadata.

| Operation | Neo4j p50 / p95 ms | Embedded p50 / p95 ms | TCP p50 / p95 ms |
|---|---:|---:|---:|
| person-profile | 1.429 / 6.687 | 1.710 / 50.303 | 1.697 / 5.527 |
| person-recent-messages | 1.330 / 6.595 | 3.329 / 47.519 | 3.087 / 5.707 |
| person-friends | 1.225 / 6.383 | 1.647 / 5.163 | 1.669 / 2.801 |
| friends-recent-posts | 8.239 / 20.895 | 17.935 / 57.727 | 22.655 / 80.895 |
| fof-recent-messages | 114.495 / 187.903 | error (12 measured) | unsupported |
| friends-within-3-by-name | 1.425 / 5.899 | error (7 measured) | unsupported |
| shortest-path-knows | 1.198 / 6.031 | unsupported | unsupported |
| friend-recommendation | 3.831 / 14.695 | unsupported | unsupported |
| tag-cooccurrence | 1.184 / 6.575 | 26.959 / 81.151 | 26.719 / 189.183 |
| country-person-count | 12.079 / 45.343 | 359.679 / 725.503 | 605.695 / 903.173 |
| add-post | 8.519 / 28.335 | 18.559 / 96.703 | 1.356 / 14.335 |
| add-like | 2.139 / 3.931 | 23.247 / 100.223 | 1.497 / 2.261 |
| add-friendship | 1.710 / 3.651 | 25.967 / 93.119 | 1.712 / 4.999 |

Measured throughput, operations/second at clients 1 / 8. Embedded rows come from separate processes per operation/count after the combined attempt aborted. Measured intervals are at least 10 seconds; final in-flight operations can extend them.

| Operation | Neo4j ops/s (1 / 8) | Embedded ops/s (1 / 8) | TCP ops/s (1 / 8) |
|---|---:|---:|---:|
| person-profile | 676.562 / 1479.356 | 495.365 / 669.587 | 580.035 / 805.931 |
| person-recent-messages | 381.451 / 779.689 | 478.746 / 583.415 | 263.022 / 38.320 |
| person-friends | 413.233 / 1431.182 | 429.714 / 674.668 | 105.272 / 180.627 |
| friends-recent-posts | 175.690 / 323.918 | 33.522 / 48.273 | 27.728 / 57.819 |
| fof-recent-messages | 4.956 / 10.064 | error (15) / error (15) | unsupported |
| friends-within-3-by-name | 299.744 / 1006.955 | error (6) / error (6) | unsupported |
| shortest-path-knows | 472.198 / 1169.050 | unsupported | unsupported |
| friend-recommendation | 159.968 / 566.924 | unsupported | unsupported |
| tag-cooccurrence | 542.998 / 1532.962 | 13.093 / 20.577 | 24.483 / 37.845 |
| country-person-count | 26.555 / 150.964 | 3.682 / 4.057 | 2.480 / 4.102 |
| add-post | 24.566 / 481.383 | 565.129 / 496.728 | 422.594 / 68.446 |
| add-like | 243.549 / 1332.026 | 473.259 / 488.428 | 59.794 / 18.284 |
| add-friendship | 331.693 / 788.140 | 546.386 / 659.032 | 97.496 / 20.698 |

## Query catalog

Cypher uses named parameters. GQL examples inline person=1, other=2, post=1, tag=1, firstName='Alice',
write id=999 to show escaping and exact syntax. Actual new ids always come from the harness mutation namespace.
Multiple GQL lines below execute in one embedded transaction; they are unsupported on TCP. The two unsupported
language examples are diagnostic repros and are never substituted for the requested operation.

### graph.person-profile

Cypher:

```cypher
MATCH (p:Person {id:$person})-[:IS_LOCATED_IN]->(c:Place {type:'City'}) RETURN p.id,p.firstName,p.lastName,p.gender,p.birthday,p.creationDate,p.locationIP,p.browserUsed,p.email,p.speaks,c.id,c.name
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH (p:Person {id:1})-[:IS_LOCATED_IN]->(c:Place {type:'City'}) RETURN p.id,p.firstName,p.lastName,p.gender,p.birthday,p.creationDate,p.locationIP,p.browserUsed,p.email,p.speaks,c.id,c.name
```

### graph.person-recent-messages

Cypher:

```cypher
MATCH (a:Person {id:$person})<-[:HAS_CREATOR]-(m:Post|Comment) RETURN CASE WHEN m:Post THEN 'Post' ELSE 'Comment' END AS kind, m.id AS id, m.creationDate AS created, a.id AS author, m.content AS content ORDER BY created DESC, id DESC, kind ASC LIMIT 10
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH route=(p:Person {id:1})<-[:HAS_CREATOR]-(m:Post|Comment) RETURN route
```

### graph.person-friends

Cypher:

```cypher
MATCH (p:Person {id:$person})-[r:KNOWS]-(f:Person) RETURN f.id AS id,f.firstName,f.lastName,r.creationDate AS created ORDER BY created DESC,id ASC
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH (p:Person {id:1})-[r:KNOWS]-(f:Person) RETURN f.id,f.firstName,f.lastName,r.creationDate
```

### graph.friends-recent-posts

Cypher:

```cypher
MATCH (p:Person {id:$person})-[:KNOWS]-(a:Person)<-[:HAS_CREATOR]-(m:Post) RETURN DISTINCT CASE WHEN m:Post THEN 'Post' ELSE 'Comment' END AS kind, m.id AS id, m.creationDate AS created, a.id AS author, m.content AS content ORDER BY created DESC, id DESC, kind ASC LIMIT 20
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH route=(p:Person {id:1})-[:KNOWS]-(a:Person)<-[:HAS_CREATOR]-(m:Post) RETURN route
```

### graph.fof-recent-messages

Cypher:

```cypher
MATCH (p:Person {id:$person})-[:KNOWS*1..2]-(a:Person)<-[:HAS_CREATOR]-(m:Post|Comment) WHERE a.id <> p.id RETURN DISTINCT CASE WHEN m:Post THEN 'Post' ELSE 'Comment' END AS kind, m.id AS id, m.creationDate AS created, a.id AS author, m.content AS content ORDER BY created DESC, id DESC, kind ASC LIMIT 20
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH route=(p:Person {id:1})-[:KNOWS]-(a:Person)<-[:HAS_CREATOR]-(m:Post|Comment) WHERE a.id <> p.id RETURN route
MATCH route=(p:Person {id:1})-[:KNOWS]-(:Person)-[:KNOWS]-(a:Person)<-[:HAS_CREATOR]-(m:Post|Comment) WHERE a.id <> p.id RETURN route
```

### graph.friends-within-3-by-name

Cypher:

```cypher
MATCH path=(p:Person {id:$person})-[:KNOWS*1..3]-(f:Person {firstName:$name}) WHERE f.id <> p.id WITH f,min(length(path)) AS distance RETURN f.id AS id,f.firstName,f.lastName,distance ORDER BY distance,id LIMIT 20
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH (p:Person {id:1})-[:KNOWS]-(f:Person {firstName:'Alice'}) WHERE f.id <> p.id RETURN f.id,f.firstName,f.lastName
MATCH (p:Person {id:1})-[:KNOWS]-(:Person)-[:KNOWS]-(f:Person {firstName:'Alice'}) WHERE f.id <> p.id RETURN f.id,f.firstName,f.lastName
MATCH (p:Person {id:1})-[:KNOWS]-(:Person)-[:KNOWS]-(:Person)-[:KNOWS]-(f:Person {firstName:'Alice'}) WHERE f.id <> p.id RETURN f.id,f.firstName,f.lastName
```

### graph.shortest-path-knows

Cypher:

```cypher
MATCH (a:Person {id:$person}),(b:Person {id:$other}) OPTIONAL MATCH path=shortestPath((a)-[:KNOWS*]-(b)) RETURN length(path)
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH ANY SHORTEST (p:Person {id:1})-[:KNOWS]-(b:Person {id:2}) RETURN p
```

### graph.friend-recommendation

Cypher:

```cypher
MATCH (p:Person {id:$person})-[:KNOWS]-(f:Person)-[:KNOWS]-(c:Person) WHERE c.id <> p.id AND NOT EXISTS { MATCH (p)-[:KNOWS]-(c) } WITH c,count(DISTINCT f) AS mutual RETURN c.id AS id,c.firstName,c.lastName,mutual ORDER BY mutual DESC,id LIMIT 10
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH (p:Person {id:1})-[:KNOWS]-(f:Person)-[:KNOWS]-(c:Person) WHERE c.id <> p.id AND NOT EXISTS { MATCH (p)-[:KNOWS]-(c) } RETURN c.id,f.id
```

### graph.tag-cooccurrence

Cypher:

```cypher
MATCH (p:Person {id:$person})-[:KNOWS]-(f:Person)<-[:HAS_CREATOR]-(m:Post)-[:HAS_TAG]->(t:Tag {id:$tag}), (m)-[:HAS_TAG]->(u:Tag) WHERE u.id <> t.id WITH u,count(DISTINCT m) AS frequency RETURN u.id AS id,u.name,frequency ORDER BY frequency DESC,id
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH (p:Person {id:1})-[:KNOWS]-(f:Person)<-[:HAS_CREATOR]-(m:Post)-[:HAS_TAG]->(t:Tag {id:1}), (m)-[:HAS_TAG]->(u:Tag) WHERE u.id <> t.id RETURN u.id,u.name,m.id
```

### graph.country-person-count

Cypher:

```cypher
MATCH (p:Person)-[:IS_LOCATED_IN]->(:Place {type:'City'})-[:IS_PART_OF]->(c:Place {type:'Country'}) RETURN c.id AS id,c.name,count(DISTINCT p) AS persons ORDER BY id
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH (p:Person)-[:IS_LOCATED_IN]->(:Place {type:'City'})-[:IS_PART_OF]->(c:Place {type:'Country'}) RETURN c.id,c.name,p.id
```

### graph.add-post

Cypher:

```cypher
MATCH (a:Person {id:$person}),(t:Tag {id:$tag}) CREATE (m:Post {id:$id,creationDate:$date,content:$content,locationIP:'127.0.0.1',browserUsed:'dbbench',language:'en',length:$length})-[:HAS_CREATOR]->(a),(m)-[:HAS_TAG]->(t) RETURN m.id,m.creationDate,a.id,t.id
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH (a:Person {id:1}),(t:Tag {id:1}) INSERT (m:Post {"id":999,"creationDate":1900000000999,"content":'Benchmark ''post'' \ 999',"locationIP":'127.0.0.1',"browserUsed":'dbbench',"language":'en',"length":22})-[:HAS_CREATOR]->(a),(m)-[:HAS_TAG]->(t) RETURN m.id,m.creationDate,a.id,t.id
```

### graph.add-like

Cypher:

```cypher
MATCH (a:Person {id:$person}),(b:Post {id:$post}) CREATE (a)-[r:LIKES {creationDate:$date}]->(b) RETURN a.id,b.id,r.creationDate
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH (a:Person {id:1}),(b:Post {id:1}) INSERT (a)-[r:LIKES {creationDate:1900000000999}]->(b) RETURN a.id,b.id,r.creationDate
```

### graph.add-friendship

Cypher:

```cypher
MATCH (a:Person {id:$person}),(b:Person {id:$other}) CREATE (a)-[r:KNOWS {creationDate:$date}]->(b) RETURN a.id,b.id,r.creationDate
```

GQL (unsupported entries are diagnostic repros):

```gql
MATCH (a:Person {id:1}),(b:Person {id:2}) INSERT (a)-[r:KNOWS {creationDate:1900000000999}]->(b) RETURN a.id,b.id,r.creationDate
```
