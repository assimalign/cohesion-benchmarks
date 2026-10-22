# Foundation performance pass: iteration 7

Measured 2026-10-22 on Windows 11 ARM64, Snapdragon X X1E80100, SDK 10.0.402 / runtime
10.0.13, BenchmarkDotNet 0.15.8 in-process. This shared evidence covers Content.Bmff,
ObjectMapping, Dns, IdentityModel.Token and an ObjectValidation investigation.

## Retained changes

- BMFF prefixes use exact buffers: prefixed saves are 14–18% faster and save 560 bytes
  per container in these fixtures. Zero-prefix allocation counts are unchanged.
- Counted mapping sequences reserve capacity: 6–7% faster, saving 24 bytes at one item
  and 1,336 bytes at 100 items. Mapping remains eager and single-pass.
- DNS name writing reuses a 63-byte label buffer and avoids suffix builders: 16–19% faster,
  51–54% lower allocation. Wire case, ASCII fallback and compression are preserved.
- Neutral successful token validation removes its 32-byte diagnostic-list allocation:
  5.6 → 2.6 ns. An initial 3% failure-path increase does not repeat: 77.5 → 76.9 ns.

[Retained comparison](COMPARISON.md): eighteen successful cases, zero regression flags.
Versions are `10.0.0-preview.1.perf.batch.before` and `.after`, including earlier retained
iterations in both. The complete first matrix ran candidate then baseline, three warmups
and eight measurements; builds/tests/native compilation finished before timing.

## Rejected validation work

Both validation candidates reduce failure-path allocations, but valid collection execution
remains slower across the comparisons. The implementation and DESIGN are restored to baseline;
[#1692](https://github.com/assimalign/cohesion/issues/1692) is Backlog and excluded from the
branch closing manifest.

- [Initial full comparison](rejected-validation1-initial/COMPARISON.md): 26 successful cases.
  The original validation reports are here; the other eighteen raw cases remain in the
  canonical before/after folders.
- [First longer repeat](rejected-validation1-repeat/COMPARISON.md): ten cases, baseline then
  candidate, five warmups/twelve measurements. Invalid nested validation remains 6% slower;
  valid collection is 14% slower. The unchanged controls also move. Three flags are preserved.
  This repeat also confirms the accepted token failure path's timing increase does not repeat.
- [Second candidate](rejected-validation2/COMPARISON.md): eight cases using isolated
  `10.0.0-preview.1.perf.batch.validation2` versus the original validation package, same longer
  sampling/order. The simpler join speeds invalid nested errors, but valid collection is
  12.33 → 14.13 us. Two flags are preserved, including a FluentValidation control.

Allocation savings do not override the execution-time gate. Timing causation is uncertain
where controls move, but neither validation candidate is retained. No raw report is edited;
all sixteen full JSON reports are verified against source SHA-256 hashes.

Producer candidate checks: 1,021 Release tests pass; nineteen selected compatibility cases
pass the original implementation. Strict API validation checks attributes and parameter names.
Package-only ARM64 NativeAOT consumers publish and execute. The second validation candidate
also passes its 291 tests/API/native checks before rejection; functional success alone is
insufficient. Existing nullable warnings in unchanged validation code remain.

Tracked work: [#1691](https://github.com/assimalign/cohesion/issues/1691),
[#1693](https://github.com/assimalign/cohesion/issues/1693),
[#1694](https://github.com/assimalign/cohesion/issues/1694),
[#1695](https://github.com/assimalign/cohesion/issues/1695).
Detailed producer report: `docs/performance/LIBRARY_PERFORMANCE_ITERATION_07.md`.
These are local microbenchmarks, not an end-to-end throughput measurement.
