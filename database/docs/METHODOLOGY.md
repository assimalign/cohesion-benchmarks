# Methodology

## Fairness and storage

Run one engine at a time on the dbbench network. Every engine receives 3 CPUs and 6 GiB with memory-swap equal
to memory. A network driver receives 3 CPUs and 3 GiB; an embedded driver receives the engine's 6 GiB because
the engine and workload share a process. Named volumes keep database and dataset I/O on the Podman VM's ext4
disk. No database files belong on Windows bind mounts. Published ports are development paths only.

Every write acknowledges durable commit. PostgreSQL fsync/synchronous_commit remain on; MongoDB adapters use
w:1,j:true; SQL Edge disables delayed durability; Neo4j retains normal log flushing; Cohesion selects
StorageCommitDurability.Synchronous. RavenDB defaults remain in effect and index catch-up must finish before
measurement. Identical logical secondary indexes are created on every engine. Foreign keys are excluded
from load because their enforcement is a separate feature, not a one-engine ingestion tax.

Settings and reasons live in engines/catalog.json and engines/README.md. The 1–2 GiB readiness profile scales
caches only to prove startup and protocol behavior; it cannot produce official comparisons.

## Workload and timing

The same seed, operation and client index select the same parameter stream on every engine. Stable FNV-1a
operation keys and SplitMix64 are available as DeterministicSequence. Generate parameters, format escaped GQL
literals, and build command objects before entering ExecuteAsync. Never cache results in the driver.

Latency uses one client per operation. Point defaults: 10 seconds warm-up, then the first of 30 seconds or
5,000 attempts. Analytics defaults: one warm-up and three attempts. Throughput uses closed-loop clients at
1, 8, 32 for 30 seconds after 10 seconds warm-up. Mixed uses configured descriptor weights at 8 and 32 clients,
with separate read-heavy (90/10) and balanced (50/50) mixes. The model owns the exact read/write weight totals.
An unsupported weighted operation blocks an unfiltered mix. The official script logs and explicitly selects the common supported subset across every registered target, retaining a 90/10 or 50/50 read/write ratio and descriptor proportions within each class. Explicit --ops permits a
documented comparable subset.

All clients finish warm-up before a shared measured start. Connection establishment and disposal are excluded.
Per-operation latency measures exactly ExecuteAsync; wall throughput also includes driver parameter preparation
and accounting. A request in flight at the duration boundary is allowed to finish and its completion extends the
wall denominator, so no work disappears. The runner merges client histograms and records both successes and
failed attempts. The first full pass uses one repetition; --repeat remains configurable.

Deadlines are 30 seconds for point operations and 600 seconds for analytics. Adapters MUST propagate the token
through all I/O and return only once cancellation has quiesced their operation. A canceled, failed or late
completion is an error, never a histogram sample. Outer cancellation aborts the pass and drains client cleanup.
Warm-up failures are counted separately and invalidate that operation's performance comparison. A timeout on the first measured execution stops further iterations of that group; in-flight peers drain. A mixed group stops too, since dropping a member would change its distribution. Loads have no wall-clock deadline and print progress every 30 seconds.

The HDR layout tracks integer microseconds from 1 to 3,600,000,000, with 2,048 sub-buckets, giving three
significant decimal digits. Values below one microsecond round up; values above one hour are errors.
dbbench-hdr-v1 is our explicit encoding (not a standard HDR wire codec): base64 of gzip containing little-endian
magic 0x44424831, int64 exact maximum, int32 bucket count, then int64 counts. Percentiles use nearest ranks and
upper bucket equivalents capped at the exact maximum. No coordinated-omission correction is applied to this
closed-loop experiment.

ParameterStreamContext.WriteId allocates disjoint billion-key client ranges above the dataset maximum for
warm-up, verification, repetitions, concurrency and suites. Mutation adapters must use this context (or prove
equivalent deterministic disjoint ranges). SQL shipment-history sequence columns are BIGINT on every target,
so the same namespace also covers history rows; generated values and indexes are unchanged.

The official order is verification, all read latency passes, all read throughput passes, all write latency
passes, all write throughput passes, then read-heavy and balanced mixed passes. OperationDescriptor.Access
classifies reads and writes. SQL and Graph perform no inverse statements on client disposal. Writes remain
durable, and engines may finish with different data because their completed operation counts differ.
No subsequent read-only performance measurement may use that changed store. A partial read rerun needs a
pristine store. Verification writes use a disposable copy; Cohesion TCP must receive the embedded-loaded
pristine store, never an embedded-mutated store. Copy/open costs stay outside operation timing and are logged.

Documents' write_orders, write_products and write_customers are equivalent isolation: write fixtures have
the same shapes/indexes but are never queried by baseline reads. Keep those collections, even with the same
read-before-write schedule. Reusing a completed write namespace requires a fresh working store; duplicate-key
errors must never be treated as insert measurements.

## Verification

Every operation has 100 fixed parameter cases. Ordered output preserves the descriptor's total order;
unordered output sorts canonical row bytes lexicographically and preserves duplicates. The canonical row starts
with an int32 column count. Each scalar has a one-byte kind, int32 UTF-8 length and its UTF-8 text. Each row
is prefixed by its int32 byte length; the SHA-256 input ends in the int64 row count. Framing is little-endian.
Each case records its hash and count, so row boundaries, empty rows, NULL, empty strings and repeated rows differ.

Decimal values round to two places using midpoint-to-even. Floating values use invariant G6 (six significant
digits), with negative zero normalized to zero. Integer widths share one representation. Timestamps use UTC
yyyy-MM-ddTHH:mm:ss.fffffffZ; unspecified DateTime is interpreted as UTC for SQL TIMESTAMP. Strings preserve exact
Unicode code points, case and whitespace. Adapters project scalar logical values consistently; complex
documents/paths need a defined common scalar projection. DateOnly, TimeOnly and bytes have separate canonical
kinds. No culture-specific formatting is allowed.

Verification compares every supported target pair per operation and parameter, including row counts.
Unsupported operations carry reason and evidence instead of an empty digest. Missing operations, incomplete
cases and execution failures fail comparison. A different manifest hash or seed is incompatible evidence.
dbbench verify --compare returns nonzero for mismatches. Reports block the involved targets' performance rows.

## Support and evidence

Native means the engine performs the logical operation in its query language or API. Emulated means bounded
client completion allowed by DESIGN §6: sort/limit, aggregate selected rows, multi-get point loop,
read-modify-write, or multiple statements in one transaction. Filtering missing predicates and joining missing
tables in the client are forbidden. Emulated time includes that completion and is visibly marked.
Unsupported needs a reason and file:line, documentation or actual error. All emulations and unsupported
Cohesion operations become findings, independently of their speed.

## Environment and noise

Results include manifest hash/scale/seed, budgets, settings/version, image digest, inspection metadata, OS/CPU/
memory, repository commits and actual Cohesion assembly informational commits. The optimization guard examines
DebuggableAttribute on loaded Cohesion assemblies and refuses official load/run with disabled JIT optimization.
It does not infer optimization from a directory named Release.
OfficialRunGuard also verifies the observed driver cgroup CPU/memory/swap limits and network engine inspect
limits. Missing resource evidence or a development budget refuses the official label.

Native runs sample Windows total CPU or Linux /proc/stat and named-container podman stats at one Hz. In official
driver runs, Measure-BenchHost.ps1 supplies Windows CPU plus Podman CPU/memory through the artifact telemetry
file because runtime-deps images do not include Podman. Snapshot collection supplies host-side podman inspect.
Missing/stale telemetry is explicitly unknown, never zero. Linux-only snapshots describe the Podman VM.
CPU >=80% of the whole host is annotated as noise; retain raw samples and rerun noisy official experiments
under quieter conditions. Do not subtract estimated interference from the measured numbers.

## Reading reports

Compare only the same dataset hash, seed, budget and official/development mode. Suite/mix, concurrency and
explicit timing settings have separate tables. The report uses the median of each repetition's statistic and
shows min–max spreads. Latency ratios use median p50 / best ranked p50; throughput ratios use best ranked ops/s /
target ops/s. Load ratios use full ingest + index/statistics/compaction metrics, excluding IsSample wire metrics.
Adapters must include every timed finalization phase in those metrics.

The best ranked engine sets 1×. Unranked targets appear in functional verification/support/error tables only.
Cohesion findings include ratios >=2 for operations, >=1.5 for load, all unsupported/emulated operations,
errors (including warm-up) and verification failures. findings.json includes triggering numbers and evidence.
Any failed repetition prevents the operation's successful repetitions from being selectively ranked.
Missing verification blocks performance even for apparently fast results.
