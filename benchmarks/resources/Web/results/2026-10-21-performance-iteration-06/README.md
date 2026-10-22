# Web serialization performance: iteration 6

Measured on 2026-10-21, Windows 11 ARM64, Snapdragon X X1E80100, .NET SDK 10.0.402 /
runtime 10.0.13, BenchmarkDotNet 0.15.8. Both repositories use `codex/library-performance`.
Package-only versions: `10.0.0-preview.1.perf.serialization.before` and
`10.0.0-preview.1.perf.serialization.after`. The committed CI pin is unchanged.

The candidate replaces interface enumeration with indexed traversal in Web.Serialization
registry lookup and negotiation, and scans Vary tokens as spans. Custom media-type
advertisements stay live. Public APIs and matching/header behavior are unchanged.
Tracked by [cohesion #1688](https://github.com/assimalign/cohesion/issues/1688).

Registry lookups are 43–47% faster in the full matrix, with 32–256 bytes removed per
operation. Negotiation saves 64–344 bytes, negotiated no-op writes save 96–376 bytes,
and HTTP JSON read/write each save 32 bytes. The no-op writes isolate registry/header
cost; they do not measure serialization throughput. These are local in-process results,
not an end-to-end HTTP throughput claim.

## Evidence and timing checks

- [Full comparison](COMPARISON.md): 26 cases, candidate then baseline, three warmups and
  eight measurements. Four unchanged ASP.NET Core JSON cases are environmental controls.
- [Longer negotiation/JSON repeat](repeat/COMPARISON.md): five cases, baseline then
  candidate, five warmups and twelve measurements.
- [Longer Vary repeat](vary-repeat/COMPARISON.md): two cases, same longer settings/order.

The initial large JSON write is 9.8% slower, and two other Cohesion cases have smaller
positive time deltas. All three are rechecked even though they fall below the script's
10% threshold. The longer JSON write is 676.8 ns before / 672.6 ns after; eight-writer
exact negotiation is 498.5 ns / 457.3 ns; Vary append is 243.3 ns / 198.2 ns. None of
the initial slowdowns repeats. All comparison reports have zero regression flags;
all measured cases succeed. Builds/tests/native compilation do not overlap measurements.

Fourteen original full JSON reports are saved unchanged, with SHA-256 verification against
the local outputs: eight for the full matrix, four for the first repeat, two for Vary.
The original apparent slowdowns remain in the full comparison. Timing variation limits
precision on small deltas; memory savings do not justify a repeatable slowdown.

Producer validation: 61 Release tests pass; eleven new compatibility cases also pass
the original implementation. Strict API package validation (including parameter names
and attributes) succeeds. A package-only ARM64 NativeAOT consumer publishes and runs
successfully with trim/AOT warnings treated as errors.

The detailed investigation is recorded in cohesion at
`docs/performance/LIBRARY_PERFORMANCE_ITERATION_06.md`.
