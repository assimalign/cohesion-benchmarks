# Database benchmark suite — design

This suite compares the core engine functionality and performance of the three Cohesion database models with
established engines, on realistic datasets of about 10 GB per model, and turns every Cohesion deficit it finds
into evidence for the "Database Hardening" work items.

Out of scope: the compiled-schema SDK task, migrations and provisioning, replication and high availability,
security, and SDK packaging. Schemas are created with plain DDL statements through each engine's normal
statement path.

## 1. Targets

| Model | Target id | Engine | Image / build | Access path | Ranked |
|---|---|---|---|---|---|
| sql | `cohesion-sql-tcp` | Cohesion SQL | `CohesionServer` app, built from source (§9) | Cohesion SQL client over TCP | yes |
| sql | `cohesion-sql-embedded` | Cohesion SQL | engine in the driver process | in-process session API | yes, separate column |
| sql | `postgresql` | PostgreSQL 17 | `docker.io/library/postgres:17` | Npgsql | yes |
| sql | `sqlserver` | SQL Server engine via Azure SQL Edge | `mcr.microsoft.com/azure-sql-edge:latest` | Microsoft.Data.SqlClient | yes, labeled |
| documents | `cohesion-documents-embedded` | Cohesion Documents | engine in the driver process | in-process session API | yes, labeled in-process |
| documents | `mongodb` | MongoDB 8.0 | `docker.io/library/mongo:8.0` | MongoDB.Driver | yes |
| documents | `ravendb` | RavenDB 7.1 (unlicensed) | `docker.io/ravendb/ravendb:7.1-latest` | RavenDB.Client | yes |
| graph | `cohesion-graph-tcp` | Cohesion Graph | `CohesionServer` app, built from source (§9) | Cohesion Graph client over TCP | yes |
| graph | `cohesion-graph-embedded` | Cohesion Graph | engine in the driver process | in-process session API | yes, separate column |
| graph | `neo4j` | Neo4j 5 Community | `docker.io/library/neo4j:5-community` | Neo4j.Driver (Bolt) | yes |

Owner decisions of 2026-10-15 behind this table:

- SQL Server ships no ARM64 build; this machine is Windows on ARM64. Azure SQL Edge (retired by Microsoft in
  September 2025, still published for linux/arm64) is the only native ARM64 build of the SQL Server engine. Its
  results are always labeled "SQL Server engine via Azure SQL Edge".
- Cosmos DB is out of scope. The only local option, the vnext-preview Linux emulator, is not the engine that runs
  in the Azure service, so its results say nothing about Cosmos DB, functionally or for performance. The emulator
  target was first kept as an unranked parity check, then removed entirely (owner decision, later on 2026-10-15).
- Cohesion Documents has no network server or client yet (issue #190; `Database.Documents.Client` has no
  source). It runs in-process. Cohesion SQL and Graph run both in-process and over TCP, so the cost of the
  protocol is measured rather than guessed, and the Documents in-process numbers can be read in that light.

## 2. Topology and resource budget

```
host (the measuring machine)
└── container runtime (docker or podman, whichever is running)
    ├── engine container   one at a time   3 CPU, 6 GB, swap pinned to memory, named data volume
    └── the benchmark process, with the same budget when it hosts an embedded engine
```

- **One engine at a time.** Only the engine under test runs during a measured phase.
- **Same budget for every engine:** `--cpus 3 --memory 6g --memory-swap 6g`, declared per target in
  `targets/catalog.json` and applied by the shared harness. RavenDB's unlicensed limit (3 cores, 6 GB) set this
  budget; the others match it so no engine gets more hardware. Swap is pinned to memory so a target over its
  budget fails rather than swapping and posting a slower number.
- **Embedded targets** get the engine budget too, because engine and workload share one process. An in-process
  number and a containerized one are never compared without saying which is which.
- **Database files live in named volumes**, never in the container layer and never on a `/mnt/c` bind mount.
- **Ports are allocated, not fixed.** Each target publishes to a free port (`"published": 0`) and benchmarks read
  the endpoint from the harness, so two runs never collide. The paused run's fixed development ports are gone.
- **A loaded volume belongs to one Cohesion build** (§9): the storage format refuses older files, so a pin move
  means regenerate and reload.

## 3. Layout

The suite follows this repository's conventions (`CONVENTIONS.md`), so it lives under the Database resource area
and shares the repository-wide harness, scripts and CI rather than carrying its own:

```
benchmarks/resources/Database/
├── README.md                                    what runs today, what does not, and why
├── docs/                                        this design, the capability baseline, the datasets, the analysis
├── targets/catalog.json                         the five external engines, with the shared budget
├── targets/<engine>/                            per-engine settings and their reasons
├── benchmarks/
│   ├── Assimalign.Cohesion.Database.Benchmarks/             the BenchmarkDotNet suite (runs in CI)
│   ├── Assimalign.Cohesion.Database.Benchmarks.Datasets/    deterministic generators for the three datasets
│   └── Assimalign.Cohesion.Database.Benchmarks.<Role>/      the comparison's own parts, when they are built
├── tests/                                       the generators' determinism tests (not run by CI)
└── results/<date>-<what>/                       results worth keeping
```

Only a project whose name ends in `.Benchmarks` is a BenchmarkDotNet suite that `Invoke-Benchmarks.ps1` and CI
run. Everything else — a server host, a load runner, the generators — is an ordinary project, which is how the
comparison's parts fit beside the micro-benchmarks without being run as micro-benchmarks. The Web area is the
worked precedent for an area that has both.

## 4. How a comparison runs

Two mechanisms already exist and the comparison uses them instead of a bespoke command line:

- **Profiles** decide the job: `local` for investigation, `ci` for a bounded, comparable run (1 launch, 5 warmups,
  20 iterations), `dry` to prove a benchmark still runs. `scripts/Invoke-Benchmarks.ps1 -Area Database` runs the
  suite; `-CohesionVersion` measures another Cohesion build; `Compare-Benchmarks.ps1` diffs two runs.
- **The external-target toolkit** in the shared harness starts and stops the engines from `targets/catalog.json`,
  waits for readiness, allocates ports, and skips a target this machine cannot start rather than failing the run.
  A benchmark selects targets with `[ParamsSource]` over `Catalog.AvailableIds(...)` and starts one in
  `[GlobalSetup]`. Macro-sized work (starting an engine, loading a dataset) goes in a class marked
  `[ExternalWorkload]`, which switches to BenchmarkDotNet's monitoring strategy.

What the toolkit does not do is drive a long multi-phase comparison — generate, load, verify, then several
measured passes per engine. That belongs in a role project beside the suite, as the Web area's runner does for
HTTP load, and it is the main piece still to be written (README.md, "What is not here yet").

## 5. Datasets

### 5.1 Principles

- **Counter-based determinism.** Every attribute of entity *i* is a pure function of `(seed, entity kind, i)`,
  computed with a keyed 64-bit mixer (SplitMix64/xxHash-style). Any partition can be generated independently and
  in parallel, and the workload driver can recompute any entity's attributes (an existing customer's email, a
  person's first name) without reading files.
- **Scale factor.** `--scale 1` is about 1 GB of raw output; the benchmark scale is **10** (about 10 GB per
  model). A size test generates a small scale and checks the extrapolated size stays within ±15% of
  `scale × 1 GB`. Development smoke runs use 0.01.
- **Realistic shape.** Zipfian popularity (customers, products, persons), seasonal and weekday order timestamps
  over five years, long-tailed text lengths, optional fields and NULLs at realistic rates, heterogeneous document
  shapes, power-law graph degrees.
- **Files.** `<out>/<model>/sf<scale>/<table>/part-NNNNN.<ext>` plus `manifest.json` (generator version, seed,
  scale, per-table row counts, byte counts and SHA-256 of every part). SQL and graph data are RFC 4180 CSV with a
  header row (an unquoted empty field is NULL; an empty string is `""`); documents are JSON Lines.

### 5.2 SQL — "commerce" (retail OLTP and reporting)

Approximate rows per scale factor 1 (×10 for the benchmark scale):

| Table | Rows / SF | Key columns and notes |
|---|---|---|
| `warehouses` | 20 (fixed) | `warehouse_id` PK, name, region, country_code |
| `categories` | 400 (fixed) | `category_id` PK, `parent_id` (3-level tree), name |
| `suppliers` | 2,000 | `supplier_id` PK, name, country_code, rating |
| `customers` | 250,000 | `customer_id` PK, `email` UNIQUE, names, phone, birth_date, country_code, city, created_at, loyalty_tier, marketing_opt_in, lifetime_value NUMERIC(12,2) |
| `customer_addresses` | 375,000 | `address_id` PK, customer_id, kind, lines, city, region, postal_code, country_code, is_default |
| `products` | 50,000 | `product_id` PK, `sku` UNIQUE, name, description (50–1,000 chars), category_id, supplier_id, brand, unit_price, cost, weight_grams, created_at, is_active |
| `inventory` | 200,000 | PK (warehouse_id, product_id), quantity_on_hand, reorder_point, updated_at |
| `orders` | 1,000,000 | `order_id` PK, customer_id (Zipf), order_date, status (placed/paid/shipped/delivered/cancelled/returned), channel, shipping_address_id, currency, subtotal, tax, shipping_fee, total, promo_code (mostly NULL) |
| `order_items` | 3,500,000 | PK (order_id, line_no), product_id (Zipf), quantity, unit_price, discount, line_total |
| `payments` | 1,050,000 | `payment_id` PK, order_id, method, amount, status, paid_at, provider_ref |
| `shipments` | 900,000 | `shipment_id` PK, order_id, warehouse_id, carrier, tracking_no, shipped_at, delivered_at (NULL while in transit) |
| `reviews` | 400,000 | `review_id` PK, product_id, customer_id, rating 1–5 (skewed high), title, body (100–2,000 chars), created_at, helpful_votes |
| `order_status_history` | 3,000,000 | PK (order_id, seq), status, occurred_at, note (mostly NULL) |

Types are portable: BIGINT/INTEGER/SMALLINT, NUMERIC(p,s), VARCHAR(n)/TEXT, DATE, TIMESTAMP (UTC, no zone),
BOOLEAN (`BIT` on SQL Server). Secondary indexes, identical on every engine: `customers(email)` unique,
`customer_addresses(customer_id)`, `products(sku)` unique, `products(category_id)`, `products(supplier_id)`,
`inventory(product_id)`, `orders(customer_id, order_date)`, `orders(order_date)`, `orders(status, order_date)`,
`order_items(product_id)`, `payments(order_id)`, `shipments(order_id)`, `reviews(product_id, created_at)`,
`reviews(customer_id)`. No foreign-key constraints (documented in METHODOLOGY: FK enforcement is an engine
feature compared separately, not a load-time tax on one engine).

### 5.3 Documents — "marketplace"

| Collection | Docs / SF | Shape (average size) |
|---|---|---|
| `products` | 100,000 | category-dependent attributes (electronics `specs` object, apparel `sizes`/`colors` arrays, books `authors` array + isbn, grocery `nutrition` object), `variants` array (sku, price, stock, attributes), tags array, `rating` summary object, seller reference, images array, long description (~3 KB) |
| `orders` | 250,000 | embedded customer snapshot, shipping address object, `items` array (productId, name, category, variant, quantity, price), `payment` object, `statusHistory` array, totals, placedAt, status (~1.8 KB) |
| `customers` | 150,000 | addresses array, preferences object, devices array, loyalty object, consent flags (~1 KB) |
| `reviews` | 150,000 | productId, customerId, rating, title, body, helpfulVotes, media array, `replies` array (~0.8 KB) |

Document keys are strings `<collection>/<n>` (RavenDB-style); every engine stores the same key as its document id.
Secondary indexes, identical in meaning on every engine: `orders(customer.id, placedAt)`, `orders(status, placedAt)`,
`products(category, variants[].price)` (multi-value), `products(name)`, `reviews(productId, createdAt)`.

### 5.4 Graph — "social network" (LDBC SNB-inspired)

Nodes: `Person`, `Forum`, `Post`, `Comment`, `Tag`, `TagClass`, `Place` (City/Country/Continent),
`Organisation` (University/Company). Relationships: `KNOWS` (Person–Person, power-law degree, creationDate),
`HAS_CREATOR`, `REPLY_OF`, `LIKES` (creationDate), `HAS_TAG`, `HAS_INTEREST`, `HAS_MEMBER` (joinDate),
`CONTAINER_OF`, `HAS_MODERATOR`, `IS_LOCATED_IN`, `STUDY_AT`/`WORK_AT` (year), `IS_PART_OF`, `HAS_TYPE`.
Text content of posts and comments (20–2,000 chars, long-tailed) is the bulk of the bytes. Per SF 1: about
30,000 persons, 1.5 M KNOWS (undirected friendship stored as one directed edge), 1.2 M posts, 3 M comments,
6 M likes; the generator calibrates counts so SF 1 ≈ 1 GB. Indexes: unique id per label, `Person(firstName)`,
`Post(creationDate)`, `Tag(name)`.

## 6. Workloads

Every operation has an id, a category, exact semantics, a parameter distribution and, for ordered results, a total
order (ties broken by key) so result digests are comparable. Each target implements each operation in its native
language and declares, per operation, one **support level**:

- **native** — one request in the engine's own query language or API produces the exact result. A semantically
  identical rewrite counts as native (for example a prefix match written as the range `name >= 'abc' AND name < 'abd'`).
- **emulated** — the engine produces a correct superset or the parts of the result, and the client completes it
  with a documented, bounded step. Allowed completions only: client-side ORDER BY/LIMIT/OFFSET over the rows the
  engine selected; client-side aggregation over the rows the engine selected; a loop of point reads for a multi-get;
  read-modify-write for a partial update; several statements in one transaction where one statement would be
  native elsewhere (for example the two halves of a UNION). Never allowed: client-side joins across tables or
  labels, client-side filtering of rows the engine could not filter (the selective predicate must run in the
  engine), or schema changes made to compensate for a missing feature. Emulated measurements are reported and
  compared, always marked as emulated, and every emulation is a Cohesion finding in itself.
- **unsupported** — the engine cannot express the operation within those rules. The result carries the reason and
  the evidence (a Cohesion `file:line`, the documentation line, or the engine's error).

`docs/COHESION-CAPABILITIES.md` records what the pinned Cohesion commit can and cannot express, with citations; the
targets use it to choose a support level, and the hardening analysis starts from it.

### 6.1 SQL operations

| Id | Category | Semantics |
|---|---|---|
| `sql.customer-by-id` | point read | customer row by PK |
| `sql.customer-by-email` | point read | customer row by unique secondary key |
| `sql.order-lines` | join read | one order joined to its lines (2 tables), ordered by line_no |
| `sql.order-detail` | join read | order + its lines + product names (3 tables), ordered by line_no |
| `sql.customer-recent-orders` | range read | a customer's 20 most recent orders (order_date desc, order_id desc) |
| `sql.product-review-summary` | aggregate read | COUNT and AVG(rating) of one product's reviews |
| `sql.product-latest-reviews` | top-N read | 5 most recent reviews of a product |
| `sql.inventory-by-product` | range read | stock for a product across warehouses |
| `sql.tx.place-order` | write txn | insert order + 1–6 lines + payment + history, decrement inventory rows, commit |
| `sql.tx.ship-order` | write txn | update order status, insert shipment + history, commit |
| `sql.update-customer` | write | single-row update by PK |
| `sql.a.revenue-by-channel` | analytics | one quarter of non-cancelled orders grouped by channel (no date functions) |
| `sql.a.revenue-by-month` | analytics | one year of orders grouped by year, month, channel (EXTRACT or equivalent) |
| `sql.a.top-products-in-category` | analytics | 3-way join, one category, one quarter, top 20 by revenue |
| `sql.a.repeat-customers` | analytics | customers with ≥ 5 orders in a year (GROUP BY/HAVING), top 100 |
| `sql.a.reorder-report` | analytics | inventory below reorder point in a warehouse joined to its product (2 tables) |
| `sql.a.brand-ratings` | analytics | review count and average by brand, HAVING count ≥ 100 |
| `sql.a.unshipped-orders` | analytics | paid orders older than a cutoff with NOT EXISTS shipment |
| `sql.a.orders-full-aggregate` | scan | COUNT/SUM/AVG over all orders |
| `sql.a.product-name-like` | scan | `name LIKE '%term%'`, first 50 by id |
| `sql.a.country-category-revenue` | analytics | 5-way join, delivered orders in a year, by country and top-level category |
| `sql.a.category-rank-window` | analytics | top 3 products per category by revenue using RANK() OVER (PARTITION BY …) |
| `sql.a.above-country-average` | analytics | customers whose lifetime value exceeds their country's average (correlated subquery or CTE) |
| `sql.a.monthly-growth-cte` | analytics | CTE of monthly revenue + LAG() month-over-month growth |

New rows written by write operations take ids from a per-client range above the generated maximum
(`base + clientIndex × 10^9 + sequence`), so no engine needs sequences and runs never collide.

### 6.2 Document operations

| Id | Category | Semantics |
|---|---|---|
| `doc.order-by-id` | point read | one order by id |
| `doc.products-multiget` | point read | 10 products by id |
| `doc.customer-recent-orders` | index query | orders where customer.id = X, placedAt desc, first 20 |
| `doc.products-by-category-price` | multi-value query | products in category X with any variant price ≤ Y, first 50 by id |
| `doc.orders-status-range` | index query | count of orders with status X placed in a date range, plus the first page of 25 |
| `doc.products-name-prefix` | index query | products whose name starts with a prefix, first 50 by name then id |
| `doc.revenue-by-category-30d` | aggregation | orders of the last 30 days, items unwound, revenue grouped by item category |
| `doc.insert-order` | write | insert a new order document |
| `doc.patch-order-status` | write | set status and append a statusHistory entry (partial update where supported) |
| `doc.adjust-variant-stock` | write | change one variant's stock inside the variants array |
| `doc.replace-customer` | write | replace a whole customer document |

### 6.3 Graph operations

| Id | Category | Semantics |
|---|---|---|
| `graph.person-profile` | point read | person by id with city |
| `graph.person-recent-messages` | 1-hop | a person's 10 most recent posts/comments |
| `graph.person-friends` | 1-hop | friends with friendship date, newest first |
| `graph.friends-recent-posts` | 2-hop | 20 most recent posts by a person's friends |
| `graph.fof-recent-messages` | 3-hop | 20 most recent messages by friends and friends-of-friends |
| `graph.friends-within-3-by-name` | variable-length | persons with a first name within 1..3 KNOWS hops, by distance then id, first 20 |
| `graph.shortest-path-knows` | path | length of the shortest KNOWS path between two persons |
| `graph.friend-recommendation` | pattern + aggregate | friends-of-friends who are not friends, ranked by mutual friends, first 10 |
| `graph.tag-cooccurrence` | pattern + aggregate | tags co-occurring with tag T on posts by a person's friends |
| `graph.country-person-count` | aggregate | persons per country through IS_LOCATED_IN/IS_PART_OF |
| `graph.add-post` | write | create a post with HAS_CREATOR and HAS_TAG relationships |
| `graph.add-like` | write | create a LIKES relationship |
| `graph.add-friendship` | write | create a KNOWS relationship |

### 6.4 Suites

- `load` — bulk ingest from the dataset files using each engine's best supported path (PostgreSQL `COPY`,
  `SqlBulkCopy`, MongoDB `InsertMany` unordered, RavenDB `BulkInsert`, Neo4j batched `UNWIND`, Cohesion's fastest
  public path). Measured: wall time per table, rows/s, MB/s, index build time, on-disk size afterwards. The
  path used is recorded with the result.
- `latency` — one client, each operation in turn: warm-up then measurement (point operations: 10 s warm-up,
  30 s or 5,000 iterations; analytics: 1 warm-up, 5 measured executions). Reports p50/p90/p95/p99/p99.9/max.
- `throughput` — closed-loop clients at 1, 8 and 32 (3 CPUs), fixed duration after warm-up, per operation group.
- `mixed` — weighted OLTP mixes (read-heavy 90/10 and balanced 50/50) at 8 and 32 clients.

Each measured pass runs `--repeat 3` times; the report shows the median and the min–max spread. Timeouts:
30 s for point operations, 600 s for analytics; a timeout counts as an error, never as a sample.

## 7. Verification and reporting

A timing is only meaningful once both engines are known to have done the same work, so verification comes first
and gates everything after it.

- **Verification.** Each operation returns its rows; the harness normalizes them (ordered by the operation's
  total order, or sorted when the operation is unordered; decimals to 2 places, floating averages to 6
  significant digits, timestamps as UTC ISO-8601, strings exact) and takes a SHA-256 over the normalized rows plus
  the row count. A fixed parameter set runs on every target and every pair must agree. A mismatch blocks the
  comparison instead of being reported as a difference in speed.
- **Support levels travel with the result.** Every operation records native, emulated or unsupported per target
  (§6), so a table can never silently compare a native query against a client-side workaround.
- **Reporting.** BenchmarkDotNet's own reports plus the repository's history pipeline cover the per-operation
  numbers: `ci` runs append two series per benchmark, time and allocated bytes, to the `gh-pages` history, and a
  regression past the threshold fails the run. A comparison pass additionally writes, per model, the load table,
  the per-operation table with the ratio to the best ranked engine, the support matrix and the error matrix, and
  a `findings.json` of every Cohesion deficit (slower than the best ranked engine by ≥ 2× on an operation, ≥ 1.5×
  on load, unsupported, errors, verification mismatches).
- **Every result records what it measured**: the Cohesion package version and commit, the dataset manifest
  fingerprint and scale, each engine's image digest and effective settings, the observed container budget, and
  the host. A run on a machine whose CPU was busy is identifiable afterwards rather than quietly averaged in.

## 8. Engine settings (shared budget: 3 CPU, 6 GB)

All engines run with durable commits. Cache sizes follow each vendor's guidance for a 6 GB host; every value
and its reason lives in `targets/README.md`.

| Engine | Settings |
|---|---|
| PostgreSQL 17 | `shared_buffers=1536MB`, `effective_cache_size=4GB`, `work_mem=64MB`, `maintenance_work_mem=1GB`, `max_wal_size=8GB`, `checkpoint_timeout=15min`, `random_page_cost=1.1`, `max_parallel_workers_per_gather=2`, `synchronous_commit=on`, `fsync=on` |
| SQL Server (SQL Edge) | `MSSQL_MEMORY_LIMIT_MB=4608`, `MSSQL_PID=Developer`, database recovery model `SIMPLE` (no log backups exist, as PostgreSQL archives no WAL), full durability |
| MongoDB 8.0 | `--wiredTigerCacheSizeGB 2.5`, writes with `w:1, j:true` |
| RavenDB 7.1 | defaults (memory-mapped storage within the container limit), unsecured local setup, EULA accepted per owner decision, waits for non-stale indexes before measuring |
| Neo4j 5 | heap 1.5 GB (initial = max), page cache 3 GB, default transaction log flushing |
| Cohesion (all models) | buffer pool 3 GiB, `StorageCommitDurability` synchronous, other options default |

## 9. Measuring Cohesion

The suite measures **the Cohesion packages cohesion's CI publishes**, never a source checkout
(`CONVENTIONS.md`, "Referencing Cohesion"). A project names what it measures with `CohesionReference` items and
they all resolve at `CohesionPackageVersion`, so one run measures exactly one Cohesion build. That replaces the
pinned worktree the paused run used, and with it the problem that made it stale: moving to a newer build is now
`scripts/Update-CohesionVersion.ps1`, and a daily pull request proposes it.

Consequences the comparison has to respect:

- **The pin follows `main`, so a pin move can be a breaking change.** PR #1168 merged, and the pin crossing it at
  `ci.44` turned the Database API from interface-first to concrete-first and stopped the suite compiling. That is
  the mechanism working: a `dry` check fails, the call sites are migrated against the API that actually shipped,
  and the move is one commit. Expect it again while the line is pre-release; COHESION-CAPABILITIES.md keeps the
  shape of the last one.
- **Engine data cannot outlive a format bump.** The storage record format and the compiled-schema document
  format both advance without an upgrade reader, and an older file is refused by design. So a loaded engine volume
  belongs to one Cohesion build: a comparison regenerates and reloads after the pin moves, and never carries a
  loaded volume across it.
- **Schemas are created with ordinary DDL** through each engine's statement path — no compiled schema, no SDK
  build task, no migrations. That keeps the comparison about the engine rather than the tooling around it.
- **Both transports are measurable.** SQL, Graph and — since T1 — Documents each have a TCP server, so each model
  can be measured embedded and over the wire, and the protocol's cost is measured rather than assumed. The
  earlier design ran Documents embedded only because it had no server; that is no longer true.

## 10. Analysis and hardening

A comparison pass writes `findings.json` (§7). Each Cohesion finding is then investigated against the Cohesion
commit behind the measured package, with a profiler trace or a targeted micro-measurement and source citations,
and filed under the Database Hardening epic ([#1511](https://github.com/assimalign/cohesion/issues/1511)) with
its evidence: the operation, the numbers, the counterpart's numbers, the dataset scale, and the commit.

A finding is worth filing only once its root cause is in the source rather than in the measurement. The paused
run's analysis is the worked example — [HARDENING.md](HARDENING.md) and
[hardening-inventory.json](hardening-inventory.json) — including the part that matters most: of 53 findings, 25
were already tracked and were deliberately not refiled, and two symptoms that did not reproduce under probing
were not filed at all.
