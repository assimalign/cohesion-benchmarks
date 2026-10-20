# LogSpace benchmarks

28 standalone cases cover `LogSpace.Hosting`'s implemented in-process log pipeline:

- OTLP JSON parsing and normalization with simple/structured AnyValues, attribute merging, trace
  IDs, timestamp validation and emitting-resource checks (1/128 records).
- Bounded queue admission, both pre-normalized and complete parse/normalize/admit batches.
- Durable segment flushing, including per-record fsync and sidecar index writes.
- NDJSON queries over two segments (64/1024 records), resource/time filters, misses, and cursor resume.

These are the actual internal types from the pinned NuGet package. Setup binds delegates to them
because the product exposes ingestion/query publicly only through its HTTPS listeners. No source
is copied and no reflection occurs in measured operations. No server/container is started. Disk
fixtures live in unique temporary directories and are removed; query samples include file I/O and
may benefit from OS caching. Flush filenames use the product's current UTC date, without
date-dependent assertions.

There is no third-party counterpart: JSON serializers, logging sinks, and generic databases do
different work and would omit OTLP semantics, durability, filtering, or cursor guarantees.
Not covered: HTTPS/mTLS/token authorization, concurrent producers, queue saturation, 16 MiB segment
rollover, 4 MiB response limits, crash recovery, host lifecycle, or application-model planning.
Retention/archival and area telemetry are not implemented in the pinned source.

Run: `pwsh ./scripts/Invoke-Benchmarks.ps1 -Area LogSpace -Profile dry`
