# Small-parser performance sweep: iteration 8

Measured 2026-10-22 on Windows 11 ARM64, Snapdragon X X1E80100, SDK 10.0.402 / runtime
10.0.13, BenchmarkDotNet 0.15.8 in-process. Both repositories use codex/library-performance.
Versions: `10.0.0-preview.1.perf.sweep.before` and `.after`; committed CI pins unchanged.

Retained changes:

- Http.Forms scans content-type, disposition and boundary parameters as spans, preserving
  grammar and duplicate precedence. In the longer run, multipart is 13–15% faster and saves
  about 9–12% allocation. URL decoding saves 176 bytes; its small-case control also gets
  faster, limiting attribution of exact timing gains. Percent-decoding semantics stay unchanged.
- Compact JWT parts locate separators without a split array, saving 48 bytes per valid parse.
  Parsing time is approximately unchanged across the two pairs; initial increases do not
  repeat in the same cases. These cases do not verify signatures on either side.
- Single-token OpenAPI pointer appends avoid a builder. Pointer-heavy transforms are 12–26%
  faster in the longer run and save 26–48% allocation. Same-version copy controls have unchanged
  allocation and timing within about 1%; source independence/version policy stays intact.

[#1699](https://github.com/assimalign/cohesion/issues/1699) also fixes TryParse throwing for a
blank encoded JWT header or payload. Original tests reproduce three failures with ten passing
compatibility cases; all thirteen pass after correction. Constructors, valid text, empty
signatures, signature checks and trust policy keep their behavior. Public signatures are unchanged.

## Timing evidence and flags

[Longer comparison](COMPARISON.md): sixteen successful cases, baseline then candidate,
five warmups/twelve measurements. Builds, tests and native compilation do not overlap timing.

One flag remains: the unchanged ASP.NET Core 32-field multipart control's reported allocation
is 374,465 → 374,466 bytes; its time is 56.64 → 56.39 us. No counterpart code/input changed.
This asynchronous one-byte report difference is kept, not edited or hidden. No repeatable
Cohesion execution slowdown is observed in the measured modified paths. Small timing deltas
remain sensitive to environmental variation.

[Initial comparison](initial/COMPARISON.md): candidate then baseline, three warmups/eight
measurements. It has five flags, including a 32% timing shift in an unchanged ASP.NET Core
control and increases in Cohesion URL decoding / RS256 parsing. Those increases do not repeat.
OpenAPI same-version controls' initial 11–12% shifts reduce to about 1% in the longer run.
HS256's small delta changes direction, so no JWT throughput gain is claimed.

Twelve full JSON reports are copied unmodified and hash-verified against their local outputs.
ASP.NET Core / Microsoft IdentityModel counterparts and same-version copies are controls.
These are local microbenchmarks, not an end-to-end HTTP throughput claim.

## Validation and tracking

Full Release suites pass 186 tests: Http.Forms 62 / JWT 102 / OpenApi.Versioning 22. Eight
new form/pointer compatibility cases pass the original code; JWT's three original failures
are documented above. Strict API validation includes parameter names and attributes.
A package-only ARM64 NativeAOT consumer publishes and executes successfully with trim/AOT
warnings treated as errors. Existing nullable warnings in untouched form code remain.

Performance work: [#1696](https://github.com/assimalign/cohesion/issues/1696),
[#1697](https://github.com/assimalign/cohesion/issues/1697),
[#1698](https://github.com/assimalign/cohesion/issues/1698).
Producer detail: `docs/performance/LIBRARY_PERFORMANCE_ITERATION_08.md`; consolidated change list:
`docs/performance/LIBRARY_PERFORMANCE_SUMMARY.md`.
