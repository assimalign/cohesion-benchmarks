# Marketplace Documents benchmark

R3 compares `cohesion-documents-embedded`, `mongodb`, and `ravendb`. Cohesion source is pinned to `bf20be5eb`; benchmark foundation is
`87afd6d`. Cohesion Documents runs only in process. Its source worktree is read-only.

## Semantics and support

The dataset's keys, names, categories, and statuses used in predicates are ASCII. Equality,
prefix matching, and tie-breaking are case-sensitive and ordinal on that domain. Parameters
include uppercase category/status/prefix variants, which must return no lower-case matches.
No adapter changes the harness normalizer or filters out engine-selected rows in the client.

Every result is a scalar projection. O = `(id, customerId, placedAt UTC, status, total decimal)`;
P = `(id, name, category, first variant price decimal)`. Multi-get uses ten distinct keys
(or all keys when fewer than ten exist), ordered ordinally. Missing point reads produce no row.
Date intervals are `[start, end)`. Revenue is `sum(item.quantity * item.price)`, including
all statuses, over a fixed 30-day interval; the first interval ends at 2026-01-01T00:00:00Z,
and later cases step back in 30-day increments. Monetary values remain decimal.

| Operation | Output and total order | Cohesion | MongoDB | RavenDB |
|---|---|---|---|---|
| doc.order-by-id | O; at most one | native | native | native |
| doc.products-multiget | P; id ASC | emulated | native | native |
| doc.customer-recent-orders | O; placedAt DESC, id ASC; 20 | emulated | native | native |
| doc.products-by-category-price | P; id ASC; 50; category AND any variant price <= bound | unsupported | native | native |
| doc.orders-status-range | `("count", count)` then O; placedAt ASC, id ASC; 25 | emulated | native | native |
| doc.products-name-prefix | P; name ASC, id ASC; 50 | emulated | native | native |
| doc.revenue-by-category-30d | `(category, revenue)`; category ASC | emulated | native | emulated |
| doc.insert-order | O read back during verification | native | native | native |
| doc.patch-order-status | id, status, history length, last status, last occurredAt | emulated | native | native |
| doc.adjust-variant-stock | id, variant index 0, sku, stock | emulated | native | native |
| doc.replace-customer | every scalar leaf, sorted property paths; array lengths and indexed leaves | native | native | native |

Read-back checks for writes run only when a verification sink is supplied; the measured operation
ends after its durable write acknowledgment and response consumption. Engine metadata is never
hashed. Whole replacement verification includes nested fields, so an adapter cannot accidentally
merge or retain fields from the previous customer.

Evidence for Cohesion, relative to `COHESION_ROOT/resources/Database/`:

- `Assimalign.Cohesion.Database.Documents/src/DocumentCollection.cs:75,109,154`: Get/Put/Delete,
  no batch or patch API. Multi-get loops point reads. Both partial updates read the document,
  change the requested field/array, and Put with the expected stored version. No retry hides
  a conflict. Insert and replacement each use one Put.
- `Assimalign.Cohesion.Database.Documents.Language/docs/DESIGN.md:48-62`: no LIMIT/OFFSET,
  UNNEST or ANY/EVERY predicates. Paging is an allowed client take after engine filtering and
  ordering. Category/variant-price is unsupported; no client filter or flattened schema substitutes.
- `Assimalign.Cohesion.Database.Documents/docs/DESIGN.md:354-357`: indexed equality/range access.
  Revenue selects its complete 30-day range in the engine, then the client unwinds items and sums.
- `Assimalign.Cohesion.Database.Documents/docs/DESIGN.md:3-11,320-328`: ordinal string semantics
  and deterministic ordering. Prefix uses the native range rewrite and then the emulated page.
- `Assimalign.Cohesion.Database.Documents.Catalog/docs/DESIGN.md:100-118`: scalar single-path,
  nonunique indexes; no compound, multikey, unique or expression indexes.

RavenDB revenue performs the same allowed aggregation completion over an explicitly indexed
date selection. This completion is included in measured time and marked emulated.

## Queries and mutation requests

`O` and `P` below abbreviate the explicit scalar projections defined above. The full executable
texts and prebuilt parameters are in `Documents/DocumentQueries.cs`. OQL O expands to
`d.id AS id, d.customer.id AS customerId, d.placedAt AS placedAt, d.status AS status, d.total AS total`.
OQL P expands to `d.id AS id, d.name AS name, d.category AS category, d.variants[0].price AS price`.
Each driver consumes all response batches/pages before finishing. No result cache exists in the driver.

### Cohesion OQL / API

```text
order-by-id: orders.GetAsync(session, key)
products-multiget: foreach sorted key: products.GetAsync(session, key)
customer-recent-orders:
 SELECT O FROM orders d WHERE d.customer.id = @customer ORDER BY d.placedAt DESC, d.id
 client takes 20
products-by-category-price: unsupported (ANY ... SATISFIES returns COHDBL001)
orders-status-range:
 SELECT O FROM orders d
 WHERE d.status = @status AND d.placedAt >= @start AND d.placedAt < @end
 ORDER BY d.placedAt, d.id
 client counts and takes 25
products-name-prefix:
 SELECT P FROM products d WHERE d.name >= @prefix AND d.name < @upper ORDER BY d.name, d.id
 @upper = @prefix + U+FFFF; client takes 50
revenue-by-category-30d:
 SELECT d.id AS id, d.items AS items FROM orders d WHERE d.placedAt >= @start AND d.placedAt < @end
 client unwinds items, sums quantity * price by category, and orders groups
insert-order: PutAsync(session, new key, prebuilt JSON)
replace-customer: PutAsync(session, fixture key, prebuilt complete JSON)
patch-order-status / adjust-variant-stock:
 GetAsync -> mutate JSON -> PutAsync(session, key, bytes, expectedVersion)
```

### MongoDB

All queries use simple collation. `_id` and `id` contain the same marketplace key. Numeric JSON
money is converted directly to BSON Decimal128 without a POCO or floating-point round trip.

```text
order-by-id: find({_id:key}) -> O
products-multiget:
 [{$match:{_id:{$in:keys}}}, {$sort:{id:1}}, {$project:P}]
customer-recent-orders:
 [{$match:{"customer.id":customer}}, {$sort:{placedAt:-1,id:1}}, {$limit:20}, {$project:O}]
products-by-category-price:
 [{$match:{category:category,"variants.price":{$lte:price}}}, {$sort:{id:1}}, {$limit:50}, {$project:P}]
orders-status-range:
 [{$match:{status:status,placedAt:{$gte:start,$lt:end}}},
  {$facet:{count:[{$count:"value"}],page:[{$sort:{placedAt:1,id:1}},{$limit:25},{$project:O}]}}]
products-name-prefix:
 [{$match:{name:/^escapedPrefix/}}, {$sort:{name:1,id:1}}, {$limit:50}, {$project:P}]
revenue-by-category-30d:
 [{$match:{placedAt:{$gte:start,$lt:end}}}, {$unwind:"$items"},
  {$group:{_id:"$items.category",revenue:{$sum:{$multiply:["$items.quantity","$items.price"]}}}},
  {$sort:{_id:1}}, {$project:{_id:0,category:"$_id",revenue:1}}]
insert-order: InsertOne(prebuilt BSON), w:1,j:true
replace-customer: ReplaceOne({_id:key}, prebuilt BSON), w:1,j:true
patch-order-status: UpdateOne({_id:key}, {$set:{status:value},$push:{statusHistory:{status:value,occurredAt:time}}})
adjust-variant-stock: UpdateOne({_id:key}, {$inc:{"variants.0.stock":delta}})
```

P uses `$arrayElemAt:["$variants.price",0]` for its scalar price. The facet returns count and
page from one query. Empty counts become the logical integer zero.

### RavenDB RQL / HTTP

RavenDB.Client handles BulkInsert, database maintenance, index creation, and non-stale checks.
Timed reads and writes use raw JSON RavenCommand requests through its RequestExecutor, without
session tracking or response caching. Topology updates are disabled for the single configured
local endpoint, including the Windows port relay. All query requests set `WaitForNonStaleResults=true` and a five-minute wait.
No auto-index query is issued. O and P are JavaScript object projections from the matching
source document, with `id: id(d)` and the corresponding explicitly named scalar fields.

```text
order-by-id: GET /databases/benchmark_r3/docs?id=key
products-multiget: GET /databases/benchmark_r3/docs?id=k1&id=k2&... (keys in total order)
customer-recent-orders:
 from index 'Bench/orders/CustomerDate' as d where exact(Customer = $customer)
 order by PlacedAt desc, Key select O limit 20
products-by-category-price:
 from index 'Bench/products/CategoryPrice' as d where exact(Category = $category) and Prices <= $price
 order by Key select P limit 50
orders-status-range:
 from index 'Bench/orders/StatusDate' as d
 where exact(Status = $status) and PlacedAt >= $start and PlacedAt < $end
 order by PlacedAt, Key select O limit 25
 (count is the response's TotalResults)
products-name-prefix:
 from index 'Bench/products/Name' as d where exact(startsWith(Name, $prefix))
 order by Name, Key select P limit 50
revenue-by-category-30d:
 from index 'Bench/orders/Date' as d where PlacedAt >= $start and PlacedAt < $end
 select {id:id(d),items:d.items} limit 2147483647
 (client sums selected items and orders category groups)
insert-order / replace-customer: PUT /docs?id=key with full JSON and @metadata.@collection
patch-order-status: PATCH /docs?id=key
 this.status = args.status; this.statusHistory.push({status:args.status,occurredAt:args.at});
adjust-variant-stock: PATCH /docs?id=key
 this.variants[0].stock += args.delta;
```

Exact string index fields plus `exact(startsWith(...))` prevent lowercase analysis. See the
vendor's [starts-with semantics](https://ravendb.net/docs/article-page/7.0/Csharp/client-api/session/querying/text-search/starts-with-query)
and [raw bulk serializer convention](https://ravendb.net/docs/article-page/7.1/Csharp/client-api/configuration/serialization).

## Indexes, durability, and load paths

The required marketplace secondary indexes are present with these explicit differences. All
engines also get a standalone placedAt index for the revenue selection. The write collections
receive the same supported definitions. Id/key tie-breaker fields in RavenDB indexes allow
the required server-side total order. No inferred/automatic Raven indexes are allowed.

| Engine | Definitions / physical differences | Load path |
|---|---|---|
| Cohesion | `CREATE INDEX ix_customer_id ON orders(customer.id)`, `ix_placedAt ON orders(placedAt)`, `ix_status ON orders(status)`; products(category), products(name); reviews(productId), reviews(createdAt). Single-path alternatives to compounds; no variant-price array index. | JSONL bytes, PutAsync in explicit transactions of up to `--batch` docs. Representative disposable-database probes compare 1 and up to 4 concurrent sessions with the same batch size. Use faster result; report both probes as samples. OQL indexes after ingest. |
| MongoDB | orders(customer.id ASC, placedAt DESC), orders(status ASC, placedAt ASC), orders(placedAt ASC); products(category ASC, variants.price ASC) multikey; products(name ASC); reviews(productId ASC, createdAt ASC). | Direct JSONL-to-BSON conversion; unordered InsertMany batches; up to four concurrent parts. Indexes after ingest. |
| RavenDB | Explicit CustomerDate, StatusDate, Date, CategoryPrice, Name, ProductDate map indexes; string keys/category/name/status indexed Exact; Prices is variants.Select(v=>v.price). | One BulkInsert per active part worker, up to four; raw JSON into explicit @collection. Index submission and non-stale wait timed separately. Index errors/Auto indexes fail load. |

Full policies live in `engines/{mongodb,ravendb}/INDEXES.md` and the target source.

Cohesion uses `BufferPoolCapacity=3221225472`, `StorageCommitDurability.Synchronous`, and other
defaults. Mongo uses `w:1,j:true` and simple collation. Raven keeps its durable storage defaults.
Smoke Mongo/Raven containers have 3 CPUs, 2 GiB and equal memory-swap.
The embedded Linux driver has 3 CPUs, 6 GiB and equal memory-swap. Windows runs are development
smokes without cgroup limits; do not treat their timings as comparable official results.
The generic report's budget header remains the harness's nominal 6 GiB budget; the actual
development limits are recorded in each server's container inspection and in this document.

Storage is measured immediately after finalization: Cohesion sums its database directory
(including its log files), Mongo reports `dbStats.storageSize + indexSize`, and Raven reports
`SizeOnDisk.SizeInBytes`. These engine-reported quantities have different allocation, journal,
compression, and checkpoint accounting. They are diagnostic sizes, not a storage-efficiency
ranking.

## Mutation isolation and repeatability

`write_orders`, `write_products`, and `write_customers` live beside the generated collections in
the same database. Their shapes and indexes are equivalent. Each opened logical client copies
the first generated order, product, and customer into separate warm-up and measured fixture
keys before warm-up. Warm-up throughput cannot change a measured fixture's starting history or stock.
Partial updates and replacements touch only these fixtures. New order keys come from
`ParameterStreamContext.WriteId` and are inserted only in `write_orders`. No selective query is
reimplemented by changing the data schema. Baseline reads keep their original data throughout.

Start each complete invocation with a fresh load. Warm-up, measurement, clients, suites, and
repetitions have disjoint insert ranges. Re-running the same complete scope without reloading
is invalid (duplicate inserts on engines that have insert-only APIs). The isolation test
executes all four writes from three client namespaces and asserts unchanged read digests.

## Reproduction and evidence

```powershell
$env:COHESION_ROOT = 'C:/cw/bench/coh-doc'
$env:DOTNET_SYSTEM_NET_DISABLEIPV6 = '1'
dotnet build database/Assimalign.Cohesion.Benchmarks.Database.slnx -c Release -p:ShouldUnsetParentConfigurationAndPlatform=false
$env:DBBENCH_DATA_SCRATCH = 'C:/cw/bench/b-doc/database/artifacts/test-data'
dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release -p:ShouldUnsetParentConfigurationAndPlatform=false
./database/scripts/Build-BenchImages.ps1 -Tag r3
$smoke = './database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests/Documents/Invoke-DocumentsSmoke.ps1'
# Sequential invocations; at most one R3 engine container runs at a time.
& $smoke -Target cohesion-documents-embedded
& $smoke -Target mongodb
& $smoke -Target ravendb
& $smoke -Target cohesion-documents-embedded -Driver
```

The script generates SF0.01, loads, verifies 100 cases per operation, runs latency with a
10-second/20-iteration cap and one-second warm-up, runs throughput for 10 seconds at clients
1 and 8 with one-second warm-up, compares digests, renders a report, and removes its containers
and volumes in finally. Latency and throughput each use one repetition. Linux driver and Windows
reports live in separate artifact directories. Raw datasets/results/logs are not committed.
The script records other sessions' containers at startup without stopping them. Any overlap and
host CPU noise make these development observations unsuitable for official rankings.
The Linux driver independently regenerates the JSONL data, checks its complete manifest
against the Windows manifest after removing only CR from the latter, and then uses the exact
Windows manifest bytes. This checks identical metadata and part hashes before loading. It
avoids the foundation serializer's platform-newline difference without changing any recorded
result digest, normalizer, or comparator.

## SF0.01 smoke record — 2026-10-15

Seed `20261015`; 1,500 customers, 1,000 products, 2,500 orders, 1,500 reviews: 6,500 documents,
9,644,294 JSONL bytes. Canonical manifest SHA256:
`a8ea4cbb75755e023aace0a096132265c1dd2b81fd6681fc942956b50dccd7cc`.

The executable build was `60040cc7a2fcb75d0aafad275beb1f6839e83d7e`, with only this document and
the smoke script uncommitted at image-build time. Cohesion binaries identify
`bf20be5eb452fa57363f412bdcff36b2dc2e7edd`. Windows was ARM64, Windows 10.0.28000; the Linux
driver was ARM64, Ubuntu 24.04.5. Server versions were MongoDB 8.0.34 and RavenDB 7.1.12. Driver image ID:
`b847f85d6612c8e2b3bcd48708a27db7881edd2e6833a26042739a3baaa102ff`.

The retained four paths verified: 1,000 cases on each Cohesion platform, and 1,100 on each other engine,
4,200 total across the retained paths. The combined comparison returned **0 mismatches**. Each engine returned zero rows
for all 25 uppercase prefix probes. The unsupported Cohesion array-price operation was reported
as unsupported, never as an empty successful result.

The Linux result captures `memory.max=6442450944`, `cpu.max=300000 100000`, and `memory.swap.max=0`.
The Windows target results capture the servers' actual 2 GiB limits, while
embedded Windows remains uncapped. Other sessions' Graph containers overlapped some passes.
These are functional development smokes with one repetition and 20 latency samples per operation,
not publishable performance rankings. The original Linux run, retained separately, also had
zero execution errors but used the platform-specific manifest; the canonical-input repeat
below was required for cross-platform comparison, not selected by speed.

### Load observations

| Target | Ingest seconds | Finalization seconds | Total excluding probes | Storage bytes |
|---|---:|---:|---:|---:|
| Cohesion Windows | 1.439 | 0.856 | 2.295 | 26953966 |
| Cohesion Linux | 0.559 | 0.479 | 1.038 | 26928736 |
| MongoDB | 0.930 | 0.199 | 1.129 | 479232 |
| RavenDB | 0.980 | 4.353 | 5.333 | 138944512 |

### Latency p50, microseconds

| Operation | Cohesion Windows | Cohesion Linux | MongoDB | RavenDB |
|---|---:|---:|---:|---:|
| doc.order-by-id | 474 | 1001 | 542 | 514 |
| doc.products-multiget | 4027 | 20223 | 1590 | 1572 |
| doc.customer-recent-orders | 1479 | 10463 | 417 | 691 |
| doc.products-by-category-price | unsupported | unsupported | 916 | 1640 |
| doc.orders-status-range | 76543 | 146175 | 4347 | 3871 |
| doc.products-name-prefix | 509 | 12463 | 330 | 440 |
| doc.revenue-by-category-30d | 1386 | 39071 | 393 | 1559 |
| doc.insert-order | 470 | 15367 | 1680 | 3173 |
| doc.patch-order-status | 1318 | 19279 | 863 | 3653 |
| doc.adjust-variant-stock | 1088 | 16367 | 856 | 3451 |
| doc.replace-customer | 494 | 9015 | 6219 | 1586 |

### Throughput operations/second, clients 1 / 8

| Operation | Cohesion Windows | Cohesion Linux | MongoDB | RavenDB |
|---|---:|---:|---:|---:|
| doc.order-by-id | 2583.2 / 2568.8 | 128.2 / 43.7 | 2350.8 / 19617.0 | 2685.7 / 11256.1 |
| doc.products-multiget | 289.8 / 269.8 | 10.8 / 37.6 | 2277.6 / 9321.3 | 667.9 / 3184.4 |
| doc.customer-recent-orders | 287.0 / 1358.2 | 171.2 / 233.5 | 643.5 / 6215.1 | 1049.6 / 3734.6 |
| doc.products-by-category-price | unsupported | unsupported | 624.3 / 4687.2 | 497.8 / 390.9 |
| doc.orders-status-range | 11.9 / 43.1 | 27.4 / 48.1 | 1654.7 / 11140.3 | 141.7 / 426.1 |
| doc.products-name-prefix | 1553.3 / 2502.0 | 656.9 / 66.7 | 2536.4 / 19920.8 | 519.7 / 3803.7 |
| doc.revenue-by-category-30d | 496.3 / 2299.9 | 51.0 / 277.2 | 1704.1 / 9371.3 | 394.2 / 917.4 |
| doc.insert-order | 1974.7 / 1797.2 | 123.8 / 440.9 | 620.7 / 1183.7 | 41.8 / 312.4 |
| doc.patch-order-status | 131.2 / 296.3 | 23.5 / 20.6 | 355.1 / 1696.9 | 27.4 / 72.6 |
| doc.adjust-variant-stock | 254.7 / 478.3 | 39.6 / 219.6 | 704.3 / 3608.9 | 13.6 / 65.7 |
| doc.replace-customer | 348.0 / 651.7 | 19.8 / 49.0 | 403.3 / 2100.0 | 52.5 / 325.8 |

### Execution checks

| Target | Verification cases | Latency successes | Throughput successes | Errors / timeouts / warm-up errors |
|---|---:|---:|---:|---|
| Cohesion Windows | 1000 | 200 | 202038 | 0 / 0 / 0 |
| Cohesion Linux | 1000 | 200 | 27114 | 0 / 0 / 0 |
| MongoDB | 1100 | 220 | 1027703 | 0 / 0 / 0 |
| RavenDB | 1100 | 220 | 305983 | 0 / 0 / 0 |

### Recorded high host CPU samples (at least 80%)

| Target | Load | Latency | Throughput |
|---|---:|---:|---:|
| Cohesion Windows | 2/4 | 6/12 | 133/222 |
| Cohesion Linux | 0/2 | 0/19 | 0/254 |
| MongoDB | unavailable | 7/13 | 89/243 |
| RavenDB | 0/6 | 0/12 | 149/228 |

The Cohesion loader probes compared one versus four sessions over the same 6,500 documents:
Windows 1.120 / 1.026 seconds; Linux 0.731 / 0.695 seconds. Both selected four sessions.
The engine still serializes writers; these figures do not establish a general parallel-load
speedup. Console LOAD totals include the probes (Windows 4.441 seconds, Linux 2.464 seconds),
whereas the load table and report ratios exclude their `IsSample=true` metrics.

Raven finalization separates 3.887 seconds submitting static indexes from 0.465 seconds waiting
for all indexes to become non-stale.

The Windows report contains 28 Cohesion findings: seven emulations, one unsupported operation,
one load deficit, and 19 performance-threshold findings. The separate Linux report contains
eight support findings. Both reports have zero verification or execution-error findings.
The Windows status-range p50 was 76.543 ms versus Raven's 3.871 ms; its eight-client throughput
was 43.1 ops/s versus Mongo's 11,140.3 ops/s. These are noisy observations across development
paths, not controlled performance claims. The query-planning evidence follows below.

The constrained Linux run also warrants investigation: order-by-id throughput fell from
128.2 to 43.7 ops/s at eight clients, and prefix throughput from 656.9 to 66.7 ops/s, with no
errors. No profile was collected, so the cause is not assigned to an engine component. Host CPU
below 80% does not exclude cgroup throttling or storage contention. Preserve these results and
repeat on an otherwise idle machine before attributing the regressions.


Artifacts are under `database/artifacts/documents-r3-smoke/`: exact expanded command lines in
`windows/*.commands.log` and `container/*.commands.log`; result JSON and verification files
beside them; Markdown reports and findings in each `report/`; historical comparison in
`parity/comparison.json`. Artifacts and generated data are intentionally untracked.

### Cohesion status-range planning

The exact reproducer is `doc.orders-status-range` in the query listing above, with both the
status and placedAt scalar indexes present. In the pinned engine,
`Documents/src/Internal/DocumentPlanner.cs:132-171` chooses one access path and prefers an
equality predicate over a range predicate; it does not estimate selectivity or intersect
the two indexes. `Documents/src/Internal/DocumentPlanExecutor.cs:41-56` then reads and parses
every candidate, checks the complete predicate, and materializes the matches; lines 105-118
sort the output. Paths here abbreviate `Assimalign.Cohesion.Database.Documents` beneath
`COHESION_ROOT/resources/Database`.

Consequently this query uses the status equality path even when its date range would be more
selective. That is a source-supported explanation to investigate for the observed slow query,
not a profiled attribution of all elapsed time. The adapter retains both required indexes and
the correct query. No Cohesion wrong-result case was observed.

## Integration and earlier diagnostic attempts

Shared changes are limited to the tests project's reference to the Documents model and the
storage-unavailable contract/CLI mapping. All adapter, parameter, query, index and loader work
lives in the owned Documents paths. Cohesion source remains unmodified.

The closest supported secondary indexes, additional shared placedAt index, and mutation fixture
collections are explicit above. No result normalizer was changed.

Earlier diagnostic attempts are retained in ignored `database/artifacts/documents-*.log`:

- Adapter builds initially exposed the OQL ExecuteAsync overload, Raven metadata namespace,
  and absence of QueryRow.GetDecimal. Those compile errors were fixed; subsequent Release
  builds passed with zero warnings and errors.
- A generate command preceded completion of the first CLI build and failed to find dbbench;
  it passed after awaiting the build. Later build/run phases were kept sequential.
- The first Raven prepare used GET /stats to discover a missing database and received HTTP
  503. It now uses GetDatabaseRecordOperation; load and all verification cases passed.
- The first Windows Cohesion smoke completed verification, latency and throughput, then the
  script tried a cross-target compare with only one verification file. The CLI correctly
  rejected that comparison. The script now compares when at least two files exist; a report
  was generated from the intact Cohesion results, and subsequent targets compare normally.
- The first Linux run completed successfully but cross-platform comparison rejected four
  pairs because of manifest hashes. All JSONL part hashes matched. Windows manifest SHA256 was
  `a8ea4cbb75755e023aace0a096132265c1dd2b81fd6681fc942956b50dccd7cc`; Linux's LF-only manifest was
  `3b49219b7fff9b7f8e7d7667f9707288bce4f39c5f85ba4e14bed2a7352542eb`.
  Replacing CRLF with LF made the entire texts identical. The source is the default newline
  in `Datasets/IO/DatasetJsonContext.cs:6`, used by `DatasetManifestFile.cs:23`.
  The owned smoke script now distributes one byte-identical manifest after checking metadata,
  and the complete Linux pass was rerun. The original run is retained separately under
  `database/artifacts/documents-r3-linux-manifest-original`; no result file was rewritten.
  The helper's first attempt also exposed CRLF in the PowerShell here-string passed to
  `/bin/sh` (`set: Illegal option -`); converting that command text to LF fixed it, and its
  finally block removed both temporary volumes before retrying.

No tests were weakened or removed. The only skipped suite test is the pre-existing opt-in
SF1 generation measurement, `Generate_ScaleOne_ReportsActualSizesAndCounts`.

## Verification command ledger

The commands below use the environment variables and paths from the reproduction section.
`CLI` abbreviates `dotnet database/artifacts/bin/Assimalign.Cohesion.Benchmarks.Database.Cli/release/dbbench.dll`.
The smoke script logs every expanded load, verify, run, compare, report, and driver command.

| Command / executions | Result |
|---|---|
| `dotnet build database/src/Assimalign.Cohesion.Benchmarks.Database.Documents -c Release` (incremental adapter builds, including no-restore variants and the Release propagation property) | Initial ExecuteAsync, metadata namespace, and GetDecimal compile failures fixed; subsequent builds pass. Logs: `documents-item1*`, `item2*`, `item3*`. |
| `dotnet build database/Assimalign.Cohesion.Benchmarks.Database.slnx -c Release -p:ShouldUnsetParentConfigurationAndPlatform=false` (repeated after fixes) | Final: 0 warnings, 0 errors, 88.27 seconds. One earlier owned-CLI DLL lock failure was resolved by sequencing build and execution. Logs: `documents-r3-final-build.log` and prior build logs. |
| `dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release -p:ShouldUnsetParentConfigurationAndPlatform=false` | Final: 76 passed, 0 failed, 1 existing skip, 97 seconds of test duration. Log: `documents-r3-final-tests.log`. |
| Same test command with `--no-build` | Earlier full runs: 75/0/1 before the isolation test, then 76/0/1. Logs: `documents-item4-tests-full.log`, `documents-r3-storage-tests.log`. |
| Same test command with `--filter FullyQualifiedName~Documents` (with and without `--no-build`) | First 7/0/0, then 8/0/0 after isolation coverage; final focused run 8/0/0. Logs: `documents-item4-tests*.log`, `documents-r3-verification-tests.log`. |
| `./database/scripts/Build-BenchImages.ps1 -Tag r3` | Both image builds passed; final driver and server images embed the pinned Cohesion commit and optimized Release assemblies. Logs: `documents-r3-images-initial.log`, `documents-r3-images-final.log`. |
| `CLI generate --model documents --scale 0.01 --threads 2 --out <artifact-root>/data` | 6,500 rows, 9,644,294 bytes; deterministic part hashes on both platforms. The first invocation ran before CLI build completion and was retried successfully. |
| `Invoke-DocumentsSmoke.ps1 -Target mongodb`, `ravendb` with `-Root database/artifacts/documents-r3-verification -VerifyOnly` | Real-engine load and 100-case verification passed for all supported operations; comparison 0 mismatches after the fixes described above. Cohesion's earlier manual load/verify supplied its file. |
| `Invoke-DocumentsSmoke.ps1 -Target cohesion-documents-embedded`, `mongodb`, `ravendb` | All final load, verify, latency, and throughput phases passed. Latency: 20 iterations, 1-second warm-up; throughput: clients 1 and 8, 10 seconds each, 1-second warm-up. The initial one-file compare guard was corrected without discarding measured results. |
| `Invoke-DocumentsSmoke.ps1 -Target cohesion-documents-embedded -Driver` | Original functional pass passed; metadata comparison exposed platform newlines. Canonical-input repeat passed all phases after the helper's CRLF fix. 3 CPUs, 6 GiB, zero swap. |
| `CLI verify --compare database/artifacts/documents-r3-smoke/parity --out database/artifacts/documents-r3-smoke/parity/comparison.json` | Final: `VERIFY COMPARE: 0 mismatches.` Five files, 5,300 cases. First comparison rejected four dataset-hash pairs before canonical input was distributed. |
| `CLI report --in database/artifacts/documents-r3-smoke/windows --out database/artifacts/documents-r3-smoke/windows/report` | Rendered `documents.md` and `findings.json`: 28 Cohesion findings, zero error/verification findings. |
| Same report command with `container` in place of `windows` | Rendered both files: eight Cohesion support findings, zero error/verification findings. |
| Start/Stop engine scripts with `-Suffix -r3`, Mongo/Raven `-Memory 2g`; stop with `-RemoveData` | Every owned server stopped with exit 0 and was removed with its volume. Driver containers used `--rm`; their two private volumes were removed in finally. Pulled/built images remain. |

The host's default SDK was an 11.0 preview targeting net10.0. The explicit MSBuild Release
propagation property prevents referenced Cohesion projects from falling back to Debug. Every
captured Cohesion assembly had `optimized=true`; no engine assembly was silently measured in Debug.

Owner follow-ups: fix the shared manifest serializer's cross-platform newline contract; prioritize
Cohesion LIMIT, array predicates, compound/multivalue indexes, batch/patch APIs and selectivity-aware
planning; investigate the Linux concurrency observations on an idle machine. Official larger-scale
comparisons need matched resource budgets, isolated runs, more samples/repetitions, and consistent
storage accounting.
