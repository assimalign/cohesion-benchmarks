# Deterministic database datasets

Generator format version **1**, default seed **20261015**. The size unit throughout this document is
**decimal GB: 1 GB = 1,000,000,000 bytes**. SF1 targets 1 GB of uncompressed part files, including CSV
headers and record separators, within ±15%. The manifest itself is excluded from size calibration.
SF10 is the benchmark scale. SF0.01 is the integrity/smoke scale.

The Datasets library has no Cohesion dependency. Its pinned interfaces remain unchanged. The public
entry point is `DatasetGenerators.Get(DatasetModel.Sql | Documents | Graph).GenerateAsync(...)`.
Generators report progress after completed parts; a successful generation ends by saving its manifest.

## Files and loader contract

| Model enum / folder | Dataset name | Format |
|---|---|---|
| `Sql` / `sql` | commerce | CSV |
| `Documents` / `documents` | marketplace | JSON Lines |
| `Graph` / `graph` | social | CSV |

```text
<out>/sql/sf1/manifest.json
<out>/sql/sf1/customers/part-00000.csv
<out>/documents/sf1/products/part-00000.jsonl
<out>/graph/sf1/person_knows_person/part-00000.csv
```

Scale directory names use the invariant round-trip representation of the supplied `double`, so close
scale values cannot accidentally share a directory. Existing dataset directories are refused unless
`Overwrite = true`. Overwrite replaces only the canonical model/scale child beneath the supplied root.
A failed or cancelled generation can leave partial files but does not publish a completed manifest;
retry with overwrite. A pre-cancelled call leaves existing data intact.

CSV is UTF-8 without BOM, RFC 4180 quoting, one header per part, and LF record endings. Unquoted empty
fields mean NULL; `""` means an empty string. Commas, quotes, CR and LF in text cause quoting, with quotes
doubled. Money is fixed-scale decimal (`12.50`), dates are `yyyy-MM-dd`, UTC timestamps are
`yyyy-MM-dd HH:mm:ss`, and booleans are `true` / `false`. Embedded newlines mean a physical line count
is not a row count. `CsvReader` decodes quoted records, resolves names through the header, and provides
`GetField(...)` spans plus `IsNull(...)`; spans remain valid until the next `Read()`.

JSON Lines uses `Utf8JsonWriter`, no indentation, a fixed property order for each shape, two fractional
digits for money, and UTC timestamps `yyyy-MM-ddTHH:mm:ssZ`. Optional properties are omitted. They are
never emitted as JSON null. `JsonLinesReader.Current` is a reusable UTF-8 memory slice, valid until the
next `Read()`; it does not deserialize the document.

`DatasetFiles.EnumerateParts(directory, manifest, table)` follows manifest part order and rejects paths
that escape the dataset directory. `DatasetManifestFile.Save/Load` uses the source-generated
`DatasetJsonContext`; no runtime reflection serializer is needed. Manifests contain generator version,
model, name, seed, scale, ordered tables, row and byte counts, maximum ids, and each relative part path
and lowercase SHA-256. Hashes are accumulated while writing with `IncrementalHash`.

The dataset `Fingerprint` is SHA-256 of the concatenated **32-byte binary part hashes**, in manifest
table order and then part order. It is distinct from the hash of the JSON manifest file. No clock,
elapsed time, absolute path or worker count enters the manifest. Identical seed and scale therefore
produce identical bytes and manifests for any thread count.

## Randomness, text and partitioning

`CounterRandom.Value(seed, stream, index, attribute)` combines independent keys with the SplitMix64
finalizer. `EntityRandom` advances only an entity-local counter. Stream names have a stable hash;
changing scheduling never advances another entity's stream. No generator uses system randomness or
the wall clock.

- Uniform integer/long draws use rejection to avoid modulo bias. Decimal draws are uniform on a
  fixed-scale integer lattice; the scaled bounds must fit Int64.
- Weighted categorical draws use an alias table: O(n) setup and O(1) samples.
- Zipf uses constant-memory [Hörmann–Derflinger rejection-inversion](https://statmath.wu.ac.at/papers/96-04-04.wh-der.abs.html),
  with stable `expm1`/`log1p` integral calculations around exponent 1. It builds no CDF. Uniform and Zipf
  rejection and Feistel cycle walking have **expected**, rather than worst-case, O(1) sample cost.
  The Zipf API supports n through 2^50 and exponents from 0 through 8.
- Popularity ranks pass through an eight-round Feistel permutation with cycle walking onto `[1,n]`.
  Both permutation and inverse are available; popular entities are not simply the smallest ids.
- Normal draws use Box–Muller. Text length is a bounded log-normal draw. Text consists of synthetic
  token sentences and paragraphs from 3,610 original word forms, plus embedded first/last names,
  72 factual city/country pairs, invented brand-like names, product nouns/adjectives and tag topics.
  No prose was copied from external works.
- The calendar is fixed at 2021-01-01 00:00:00Z through 2025-12-31 23:59:59Z, with second precision.
  November/December day weights are 2.6 times other months; weekdays weigh 1.2 and weekends 0.8.
  Hour weights peak at 17:00–21:00. Dependent events reserve seconds before the horizon and are strictly
  increasing. Social people and forums exist at the baseline, before forum membership and messages;
  friendships, messages, orders and reviews use the weighted calendar.

Fixed entity ranges determine parts. `Threads` only limits concurrent range jobs. Nominal large-file
parts are roughly 64–256 MB at SF10; small reference tables and final partial ranges are smaller.

| Generator family | Fixed range per job / part |
|---|---|
| Commerce customers | 500,000 customers |
| Commerce addresses | 1,000,000 addresses |
| Commerce products | 200,000 products |
| Commerce inventory | 500,000 products, four inventory rows each |
| Commerce orders and all dependent tables | 1,000,000 orders |
| Commerce reviews | 150,000 reviews |
| Marketplace customers / products / orders / reviews | 100,000 / 40,000 / 60,000 / 100,000 documents |
| Social people and their relationships | 200,000 latent person ranks |
| Social posts and relationships | 6,000,000 posts per job; node files split every 500,000 ids |
| Social comments and relationships | 17,500,000 comments per job; node files split every 700,000 ids |
| Social likes | 40,000 latent person ranks |

Social node splits align exactly inside the larger relationship ranges. Each range is generated once,
with bounded writer buffers and per-person friendship/like sets, without reading generated data back.
Reference tables, person properties and less frequent relationships can remain below the nominal part
size even at SF10. Files and rows have stable order; social person files are in permuted latent-rank
order, not numeric id order.

## Commerce schema and consistency

[CommerceSchema](../src/Assimalign.Cohesion.Benchmarks.Database.Datasets/Commerce/CommerceSchema.cs)
publishes columns, logical types, nullability, precision/scale/length, primary keys and secondary indexes
in load order. `N` below means NUMERIC(12,2), `Vn` means VARCHAR(n), `TS` means UTC TIMESTAMP,
and `?` marks nullable columns. All unmarked columns are non-null. Foreign references are validated by
tests but deliberately do not become physical foreign-key constraints.

| Table | Columns in file order | Primary key |
|---|---|---|
| warehouses | warehouse_id BIGINT, name V100, region V100, country_code V2 | warehouse_id |
| categories | category_id BIGINT, parent_id BIGINT?, name V100 | category_id |
| suppliers | supplier_id BIGINT, name V160, country_code V2, rating N | supplier_id |
| customers | customer_id BIGINT, email V254, first_name V80, last_name V80, phone V30?, birth_date DATE, country_code V2, city V100, created_at TS, loyalty_tier V20, marketing_opt_in BOOLEAN, lifetime_value N | customer_id |
| customer_addresses | address_id BIGINT, customer_id BIGINT, kind V20, line1 V200, line2 V200?, city V100, region V100, postal_code V20, country_code V2, is_default BOOLEAN | address_id |
| products | product_id BIGINT, sku V40, name V200, description TEXT, category_id BIGINT, supplier_id BIGINT, brand V100, unit_price N, cost N, weight_grams INTEGER, created_at TS, is_active BOOLEAN | product_id |
| inventory | warehouse_id BIGINT, product_id BIGINT, quantity_on_hand INTEGER, reorder_point INTEGER, updated_at TS | warehouse_id, product_id |
| orders | order_id BIGINT, customer_id BIGINT, order_date TS, status V20, channel V20, shipping_address_id BIGINT, currency V3, subtotal N, tax N, shipping_fee N, total N, promo_code V40? | order_id |
| order_items | order_id BIGINT, line_no SMALLINT, product_id BIGINT, quantity INTEGER, unit_price N, discount N, line_total N | order_id, line_no |
| payments | payment_id BIGINT, order_id BIGINT, method V30, amount N, status V20, paid_at TS, provider_ref V80 | payment_id |
| shipments | shipment_id BIGINT, order_id BIGINT, warehouse_id BIGINT, carrier V40, tracking_no V80, shipped_at TS, delivered_at TS? | shipment_id |
| reviews | review_id BIGINT, product_id BIGINT, customer_id BIGINT, rating SMALLINT, title V200, body TEXT, created_at TS, helpful_votes INTEGER | review_id |
| order_status_history | order_id BIGINT, seq SMALLINT, status V20, occurred_at TS, note V500? | order_id, seq |

Secondary indexes exactly match DESIGN: `customers(email)` unique; `customer_addresses(customer_id)`;
`products(sku)` unique, `products(category_id)`, `products(supplier_id)`; `inventory(product_id)`;
`orders(customer_id, order_date)`, `orders(order_date)`, `orders(status, order_date)`;
`order_items(product_id)`; `payments(order_id)`; `shipments(order_id)`;
`reviews(product_id, created_at)` and `reviews(customer_id)`.

Categories have ten roots (1–10), sixty middle nodes (11–70), and 330 leaves (71–400). Every product
uses a leaf category. Suppliers and addresses always exist. Every customer has a primary address;
the first half have a second address. Shipping uses the selected customer's own address. Inventory
has four distinct warehouses per product. Emails and SKUs include the entity id and are unique.

Customer and product popularity use Zipf exponent 1.08 and independent id permutations. Orders have
1–6 lines uniformly, quantity 1–4, prices copied from their products, and a 15% chance of a 10% line
discount. `discount` is a **money amount**, not a rate. `line_total = quantity × unit_price - discount`;
`subtotal = sum(line_total)`. Money uses two decimals, midpoint away from zero. Tax uses fixed synthetic
benchmark rates: US 7%, CA 13%, GB/FR 20%, DE 19%, AU/JP 10%, all other countries 15%. These are dataset
rules, not a current tax model. Shipping is 6.50 below subtotal 100, otherwise zero. Total equals
subtotal plus rounded tax plus shipping. Promo codes are present on about 12% of orders.

About 3% of orders are cancelled and 2% of sufficiently old orders returned. Orders younger than one
day are placed/paid; ages 1–4 days are paid/shipped; ages 4–10 days are predominantly shipped; older
orders are predominantly delivered, with a small paid backlog. Complete paths are
placed → paid → shipped → delivered → returned, truncated to the terminal state; cancellation follows
placed directly. Payment exists for every paid-or-later order, with an 8% failed-first-attempt rate.
Failed attempts precede the successful payment. Shipment rows exist for shipped/delivered/returned
orders; in-transit `delivered_at` is NULL. History, payment and shipment event times agree.

Review rating weights for 1–5 stars are 2:4:12:32:50. Product descriptions are 50–1,000 characters
(log-normal median 230), review bodies 100–2,000 (median 650), both sigma 0.8. Text lengths are bounded
after sampling. Customer phone is absent about 8%, address line2 about 80%, and history notes about 96%.
Customer lifetime values and product creation dates are independently seeded descriptive attributes;
they are not recomputed purchase aggregates or signup-before-purchase constraints.

## Marketplace shapes

[MarketplaceSchema](../src/Assimalign.Cohesion.Benchmarks.Database.Datasets/Marketplace/MarketplaceSchema.cs)
lists collections in load order: customers, products, orders, reviews. Every document has an `id` equal
to its key: `customers/<n>`, `products/<n>`, `orders/<n>` or `reviews/<n>`.

| Collection | Required shape and variation |
|---|---|
| customers | Names/email, birthDate, createdAt; 1–2 addresses; preferences with categories, interests, locale/currency and flags; 1–3 devices; loyalty tier/points/lifetimeValue; consent flags. Phone is omitted about 10%. |
| products | Name, category, brand, long description, createdAt, active; 1–4 variants with sku/price/stock/attributes; 3–7 distinct tags; rating average/count; seller referencing an existing customer; 2–5 images. Categories are uniformly electronics/apparel/books/grocery. |
| orders | Customer identity snapshot, an exact existing shipping-address snapshot, 2–5 items, payment, ordered statusHistory, currency, subtotal/tax/shippingFee/total, placedAt and terminal status. Items copy an existing product's name/category and existing variant SKU/price. PromoCode occurs about 12%; note about 7%. |
| reviews | Existing productId/customerId, skewed 1–5 rating, body, createdAt, helpfulVotes, media and replies arrays. Title is omitted about 5%; media occurs about 15%; about 22% have 1–3 replies by existing customers at increasing times. |

Electronics carry a `specs` object (processor, memoryGb, storageGb, powerWatts, wireless), plus an optional
warrantyMonths. Apparel has sizes/colors arrays and material. Books have an authors array, a valid
ISBN-13 check digit and page count. Grocery has nutrition (servingGrams, calories, protein/fat/carbohydrate
grams) and an organic flag. Other category-specific shapes are absent. About 30% of products also have
careInstructions. Descriptions use log-normal median 2,200, sigma 0.8, bounds 300–12,000 characters.
Review bodies use median 300, bounds 100–2,000.

Order customer/product selection uses independently permuted Zipf(1.08) ranks. Variant price is the
base price multiplied by `1 + 0.08 × zeroBasedVariant`, rounded to two decimals. Embedded prices and
identities use the same descriptor functions as product/customer generation. Order totals and status
age rules use the commerce rules. Product rating summaries are seeded attributes, not aggregates of
the generated reviews; the workload does not assume such aggregate equality.

Logical secondary indexes: `orders(customer.id, placedAt)`, `orders(status, placedAt)`,
`products(category, variants[].price)` (multi-value), `products(name)`, and
`reviews(productId, createdAt)`.

## Social graph

[SocialSchema](../src/Assimalign.Cohesion.Benchmarks.Database.Datasets/Social/SocialSchema.cs) describes
each label, property and relationship endpoint. IDs are unique within a label; Post 1 and Comment 1
are distinct nodes. Every relationship CSV begins with `src,dst`; property columns follow.

| Node file / label | Properties after BIGINT id |
|---|---|
| person / Person | firstName V80, lastName V80, gender V20, birthday DATE, creationDate TS, locationIP V40, browserUsed V40, email V254, speaks V100 |
| forum / Forum | title V200, creationDate TS |
| post / Post | imageFile V500?, creationDate TS, locationIP V40, browserUsed V40, language V10, content TEXT, length INTEGER |
| comment / Comment | creationDate TS, locationIP V40, browserUsed V40, content TEXT, length INTEGER |
| tag / Tag | name V200, url V500 |
| tagclass / TagClass | name V200, url V500 |
| place / Place | name V200, url V500, type V20 (City/Country/Continent) |
| organisation / Organisation | type V20 (University/Company), name V200, url V500 |

Each label has a unique id index. Additional indexes are `Person(firstName)`, `Post(creationDate)` and
`Tag(name)`. Relationship types/files and actual counts are listed below. `KNOWS` and `LIKES` carry
`creationDate TS`, `HAS_MEMBER` carries `joinDate TS`, and `STUDY_AT`/`WORK_AT` carry `year INTEGER`.
Other relationships have only endpoints. File names explicitly encode endpoint labels, including
separate comment→post and comment→comment `REPLY_OF` files.

Friendships use a seeded latent-rank permutation. Each lower rank proposes at most
`5 + Zipf(n=400, s=1.5)` higher ranks, capped by the remaining population. Up to 65% of targets are drawn
without replacement from the same city; the remainder use a global permutation without replacement.
Only the lower rank proposes the pair, and output is canonicalized to `src < dst`. There are no
self-loops or duplicate friendships. City also determines university, increasing study locality.
At SF1 the measured mean undirected degree is **40.51**.

Forum membership is a deterministic partition of latent person ranks; the moderator is a member.
Posts cycle across creator ranks and belong to that creator's forum. Comments map to a post thread by
id modulo post count; later bands can reply to an earlier comment in the same thread (70% when one
exists). Timestamp bands ensure strictly later replies without traversing generated files. Comment
authors are members of the thread's forum. Posts and comments have 20–2,000 character content,
log-normal medians 70 and 48 respectively, sigma 1.05. About 10% of posts carry an imageFile.

Likes average 200 per person: 55% of candidate draws target a known creator's post, the rest use
permuted Zipf message popularity. Duplicate candidates are retried; after eight attempts a uniform
fallback guarantees practical progress. Every person/message pair is unique within its relationship
type. Likes occur after the message and after the pair's seeded affinity date, so every like on a
friend's message also occurs after that friendship. At extreme tiny scales likes are capped by the
number of distinct person/message pairs.

## Counts, calibration and measured runs

Primary scaled counts are rounded to nearest integer, midpoint away from zero, with a minimum of one.
Warehouse/category and graph vocabulary/geography counts are fixed. Derived row counts follow actual
entity structure and lifecycle; they are not filled with dummy records to meet approximate counts.

Measured on 2026-10-15, Windows ARM64 Surface Laptop, Snapdragon X2 Elite (12 logical processors),
32 GB RAM, .NET runtime 10.0.13, SDK 11.0.100-rc.2.26504.105, Release, seed 20261015, four workers,
local scratch files. Models were generated sequentially. Stopwatch measurements include generation,
file writes, incremental SHA-256 and manifest creation; they exclude subsequent scratch deletion.
These are single-run generation observations on a shared host, not engine performance measurements.

| Model | Actual SF1 bytes | SF1 GB | Generation seconds | Total rows / documents | Linear SF10 estimate (GB) |
|---|---:|---:|---:|---:|---:|
| Commerce | 964,251,944 | 0.964252 | 9.920 | 11,707,533 | 9.642519 |
| Marketplace | 955,805,332 | 0.955805 | 6.540 | 650,000 | 9.558053 |
| Social | 1,030,264,265 | 1.030264 | 20.971 | 26,226,031 | 10.302643 |

Social totals contain 4,232,285 nodes and 21,993,746 relationships.
SF10 estimates are ten times measured SF1 bytes; SF10 was not generated. Longer decimal ids and fixed
reference tables make exact scaling slightly nonlinear. All three measured SF1 sizes pass ±15%.

| Model | SF0.05 bytes | SF1 extrapolation from SF0.05 | Error versus 1 GB |
|---|---:|---:|---:|
| Commerce | 46,777,812 | 935,556,240 | −6.44% |
| Marketplace | 47,649,122 | 952,982,440 | −4.70% |
| Social | 48,765,213 | 975,304,260 | −2.47% |

The calibration failures before tuning were 789,909,600 / 843,121,400 / 840,104,400 extrapolated bytes
(commerce / marketplace / social). Only bounded text medians changed to meet the target; checks were
not relaxed.

### Commerce counts

| Table | Scaling rule | Actual SF1 rows | Actual SF1 bytes |
|---|---|---:|---:|
| warehouses | 20 fixed | 20 | 557 |
| categories | 400 fixed | 400 | 7,391 |
| suppliers | 2,000 × SF | 2,000 | 71,791 |
| customers | 250,000 × SF | 250,000 | 32,604,024 |
| customer_addresses | 375,000 × SF | 375,000 | 28,858,826 |
| products | 50,000 × SF | 50,000 | 19,914,955 |
| inventory | 4 per product | 200,000 | 7,227,098 |
| orders | 1,000,000 × SF | 1,000,000 | 89,607,358 |
| order_items | 1–6 per order (mean 3.5) | 3,499,885 | 125,784,697 |
| payments | Paid-or-later plus failed attempts | 1,046,788 | 72,027,056 |
| shipments | Shipped, delivered or returned orders | 957,074 | 79,512,387 |
| reviews | 400,000 × SF | 400,000 | 357,104,422 |
| order_status_history | Complete lifecycle paths | 3,926,366 | 151,531,382 |

### Marketplace counts

| Collection | Documents per SF | Actual SF1 bytes | Mean bytes/document |
|---|---:|---:|---:|
| customers | 150,000 | 141,494,602 | 943.3 |
| products | 100,000 | 382,246,189 | 3822.5 |
| orders | 250,000 | 325,125,674 | 1300.5 |
| reviews | 150,000 | 106,938,867 | 712.9 |

### Social counts

| Node file | Scaling rule | Actual SF1 nodes | Actual SF1 bytes |
|---|---|---:|---:|
| place | 72 cities + 43 countries + 6 continents, fixed | 121 | 6,418 |
| organisation | 72 universities + 72 companies, fixed | 144 | 11,055 |
| tagclass | 20 fixed | 20 | 1,171 |
| tag | 1,000 fixed | 1,000 | 51,835 |
| person | 30,000 × SF | 30,000 | 3,179,115 |
| forum | 1,000 × SF | 1,000 | 46,973 |
| post | 1,200,000 × SF | 1,200,000 | 216,054,964 |
| comment | 3,000,000 × SF | 3,000,000 | 406,824,236 |

| Relationship file | Logical type | Actual SF1 edges | Actual SF1 bytes |
|---|---|---:|---:|
| place_ispartof_place | IS_PART_OF | 115 | 768 |
| organisation_islocatedin_place | IS_LOCATED_IN | 144 | 890 |
| tagclass_ispartof_tagclass | IS_PART_OF | 19 | 95 |
| tag_hastype_tagclass | HAS_TYPE | 1,000 | 6,451 |
| person_islocatedin_place | IS_LOCATED_IN | 30,000 | 255,149 |
| person_studyat_organisation | STUDY_AT | 24,053 | 324,814 |
| person_workat_organisation | WORK_AT | 25,492 | 363,412 |
| person_hasinterest_tag | HAS_INTEREST | 90,305 | 860,060 |
| person_knows_person | KNOWS | 607,604 | 18,993,879 |
| forum_hasmoderator_person | HAS_MODERATOR | 1,000 | 9,536 |
| forum_hasmember_person | HAS_MEMBER | 30,000 | 885,701 |
| forum_hastag_tag | HAS_TAG | 2,000 | 15,569 |
| forum_containerof_post | CONTAINER_OF | 1,200,000 | 13,160,504 |
| post_hascreator_person | HAS_CREATOR | 1,200,000 | 15,244,664 |
| post_islocatedin_place | IS_LOCATED_IN | 1,200,000 | 11,938,784 |
| post_hastag_tag | HAS_TAG | 1,680,874 | 18,434,669 |
| comment_hascreator_person | HAS_CREATOR | 3,000,000 | 39,778,532 |
| comment_islocatedin_place | IS_LOCATED_IN | 3,000,000 | 31,513,538 |
| comment_replyof_post | REPLY_OF | 1,738,936 | 25,055,468 |
| comment_replyof_comment | REPLY_OF | 1,261,064 | 19,398,554 |
| comment_hastag_tag | HAS_TAG | 901,140 | 10,383,251 |
| person_likes_post | LIKES | 4,118,546 | 134,797,797 |
| person_likes_comment | LIKES | 1,881,454 | 62,666,413 |

Scaled social relationships are derived from their owners: one city per person/message/organisation;
one creator per message; one reply target per comment; one forum container per post; one moderator per
forum; one membership per person; two tags per forum; 1–2 tags per post; about 30% of comments tagged;
1–5 interests per person; about 80% study links and 85% work links; 6,000,000 total likes per SF.
Geography and tag taxonomy relationships are fixed. Friendship counts vary with the seed's degree draws.

### Dataset fingerprints

| Dataset | SHA-256 fingerprint |
|---|---|
| Sql | `29a71838dbefbc86acb39790b823197ef46b4d58a5a4768347e4d4030d8ca528` |
| Documents | `e33f0ad3602abac7886ce92bff7678edcde727f1d1fbef957bbb41a55561f45e` |
| Graph | `30bd4d952354ba907a9ed345e7398ff7025ef45be99e2c775ca5114bc81f8f3a` |

## Workload descriptors

`CommerceDescriptor`, `MarketplaceDescriptor` and `SocialDescriptor` accept `(seed, scale)` or a
`DatasetManifest`. A manifest with a different model or unsupported generator version is rejected.
The generators instantiate these very classes, so recomputation shares their exact code path.

| Descriptor | Useful pure parameter methods and counts |
|---|---|
| CommerceDescriptor | Customers, Addresses, Products, Suppliers, Orders, Reviews; Customer/CustomerEmail, Product/ProductCategory/ProductBrand, CategoryParent/CategoryName, AddressCustomer, Order/OrderDate, Line |
| MarketplaceDescriptor | Customers, Products, Orders, Reviews; Key, Customer, Product, Variant, VariantPriceRange, Order/OrderDate |
| SocialDescriptor | Persons, Forums, Posts, Comments, Likes, Tags, TagClasses; FirstName, LastName, PersonCity, City, TagName, PostDate, FriendshipDate |

All expose `MaximumIds()` and `IdRange(name)`. Descriptors made from a manifest also expose its
`GeneratedTables`, including actual derived row counts. Commerce composite-key child tables report
the maximum owning order/product id. Payment ids reserve two slots per order and may have gaps;
shipments also have gaps. Their maxima are the actual highest generated ids. Dense customer/product/
person ranges can be used directly for existence checks. Graph relationship files have no independent
id, so their maximum is zero; endpoint labels determine id allocation.

```csharp
DatasetManifest manifest = DatasetManifestFile.Load(Path.Combine(dataDirectory, "manifest.json"));
var commerce = new CommerceDescriptor(manifest);
string email = commerce.CustomerEmail(customerId);
DateTime orderDate = commerce.OrderDate(orderId);
long newOrderId = checked(commerce.IdRange("orders").Maximum + clientIndex * 1_000_000_000L + sequence);
```

Compute workload parameters before entering an operation's timer. There is no id→file scan, cached
query result or dependency on a previously generated partition.

## Verification and reproduction

Only Datasets and Tests need building; Tests transitively builds the existing Harness reference.
No solution, CLI, engine model or Cohesion source build is needed.

```powershell
dotnet build database/src/Assimalign.Cohesion.Benchmarks.Database.Datasets -c Release
dotnet build database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release
dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release

$env:DBBENCH_LONG_TESTS = '1'
dotnet test database/tests/Assimalign.Cohesion.Benchmarks.Database.Tests -c Release --no-build `
  --filter FullyQualifiedName~Generate_ScaleOne --logger 'console;verbosity=detailed'
Remove-Item Env:DBBENCH_LONG_TESTS
```

The non-long suite has **40 passing tests and one opt-in skip**. It checks every SF0.01 foreign
reference, embedded product/address snapshot, graph endpoint, reply chronology, friendship uniqueness,
total and primary key; descriptor recomputation; actual maximum ids; null/empty/quoted/Unicode/buffer
boundary round trips; distribution probabilities; exact permutation inversion; and overwrite/cancellation.
Determinism tests compare raw files and SHA-256 at SF0.01 and complete manifests across multiple parts
at SF0.41 commerce/marketplace and SF1.34 social, with one versus four workers. Changed seeds change
fingerprints. Three SF0.05 calibration tests enforce the ±15% limit; the opt-in test measures all
three SF1 models and also enforces that size limit and mean social degree 35–45.

The Datasets Release build also passes `-p:IsAotCompatible=true` with zero warnings (including trim/AOT
analysis). This is an analyzer check, not a native executable publication. Tests use xUnit 2.9.3,
xunit.runner.visualstudio 4.0.1, Microsoft.NET.Test.Sdk 18.11.0 and Shouldly 4.3.0, the latest stable
versions returned by NuGet and successfully restored on the measurement date.

Test-generated files live below `C:\cw\bench\data-scratch\r1b-<process>-<counter>` and are deleted by
the test owner after each run, including assertion failures. `DBBENCH_DATA_SCRATCH` can override the
root. No generated dataset is committed or retained.

## Specification reconciliations

- DESIGN §5.2 calls its dependent-table counts approximate. Complete placed/paid/shipped/delivered
  paths require about 3.93 million history rows at SF1 rather than the listed 3 million. Payments and
  shipments similarly follow the generated lifecycle, rather than fixed quotas.
- DESIGN §5.4's 1.5 million undirected friendships among 30,000 people imply mean degree 100.
  Brief R1b explicitly requires mean about 40. This implementation follows that requirement, producing
  607,604 friendships at the default SF1 seed. The owner should reconcile DESIGN's approximate counts.
- All required primary scale counts and all requested logical indexes are preserved. Calibration
  adjusts bounded text medians, not primary entity counts. Small tables and final part tails naturally
  remain below the nominal large-part target.
