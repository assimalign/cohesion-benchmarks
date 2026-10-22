# Layered connection-feature lookup results

Measured locally on 2026-10-21 on the existing `codex/library-performance` branches. Cohesion is
based on `c2b25e4ac` plus performance iterations 1 and 2; the companion repository is based on
`7a6aa1a` plus those iterations. Both compared packages contain the same preceding Core changes.
The baseline has the original Connections implementation. No committed package pin changes.

Named lookup keeps its local-slot check and walks exact built-in inner collections iteratively.
Custom collections and subclasses still receive their interface `Get(name)` call. This removes
repeated built-in dispatch/validation and bounds built-in traversal stack use without adding a
cache, fields, allocation or public API. Typed lookup and enumeration are unchanged.

## Retained repeat results

| Workload | Median before | Median after | Allocation in both |
| --- | ---: | ---: | ---: |
| Inherited named hit, one wrapper | 4.9 ns | 4.0 ns | 0 B |
| Inherited named hit, four wrappers | 12.9 ns | 11.9 ns | 0 B |
| Fully absent name, one wrapper | 4.2 ns | 3.7 ns | 0 B |
| Fully absent name, four wrappers | 12.2 ns | 10.1 ns | 0 B |
| Fully absent name, 16 wrappers | 39.3 ns | 28.7 ns | 0 B |

One-wrapper named hits improve about 19%, and the four-/16-wrapper absent-name cases about
17% / 27%. The smaller four-wrapper hit improvement should be treated cautiously. Sixteen
wrappers are a depth stress point, not a recommended topology. These are per-lookup JIT
microbenchmarks, not application throughput or native-performance measurements; the design still
recommends caching a resolved optional feature for repeated I/O.

[COMPARISON.md](COMPARISON.md) contains all 12 cases. The four unchanged typed-lookup controls
stay within about 4% on the repeat. Bare-collection and local-tombstone timings vary between runs,
so no systematic improvement is claimed for those controls. All cases allocate zero bytes.
Both comparisons report zero regressions at the script's 10% timing / exact allocation thresholds.

## Method and reports

Windows 11 ARM64, Snapdragon X 12-core X1E80100; .NET SDK 10.0.402 / runtime 10.0.13;
BenchmarkDotNet 0.15.8 with the repository in-process harness, one launch per package.
The initial baseline-then-candidate pair uses five warmups and 12 measured iterations. Its
one-wrapper typed-miss control varied by almost 10%, so the entire 12-case matrix was repeated
in reverse order, candidate then baseline, with three warmups and eight measured iterations each.
That control returns to about 23.2 ns in both repeat packages; named-hit and absent-name gains persist.

`before/` and `after/` retain unedited full JSON for the reverse-order repeat. `initial/` retains
the original unedited reports and comparison, including the control variation. Each pair uses
identical workloads, names, setup and parameters. Existing identifiers remain unchanged; the new
`ConnectionNamedMissBenchmarks` adds absent names through 0/1/4/16 wrappers. The existing named
miss hits an outer tombstone, so it does not measure a full traversal.

Local package versions are `10.0.0-preview.1.perf.connections.before` and
`10.0.0-preview.1.perf.connections.final`, packed through `CohesionPatchVersion`.
The ignored temporary runner links the two lookup classes and references packages through an
isolated local-feed configuration:

```powershell
dotnet run -c Release --project .artifacts/performance/connections/ConnectionsIteration.Benchmarks.csproj `
  -p:CohesionPackageVersion=<baseline-or-final-version> -p:RestoreConfigFile=<local-feed-config> -- `
  --profile ci --filter '*ConnectionFeatureLookupBenchmarks*' '*ConnectionNamedMissBenchmarks*' `
  --warmupCount 3 --iterationCount 8 --artifacts <output>
```

## Compatibility

The five new compatibility cases pass against baseline and candidate. They cover subclass interface
dispatch, local shadowing, tombstones, ordinal identity and live inner replacement at each depth.
Core Connections and five consumer suites pass: **229 tests, zero failures or skips**. Strict SDK
package validation preserves public API shape, parameter names and attributes, without suppressions.
The final packaged `win-arm64` NativeAOT consumer publishes and executes deep named/typed lookup,
ordinal misses, tombstones, replacement and custom subclass dispatch.

Tracked in [#1683](https://github.com/assimalign/cohesion/issues/1683). OpenTelemetry is excluded
from this performance effort; no OpenTelemetry implementation or benchmark changes are retained.
