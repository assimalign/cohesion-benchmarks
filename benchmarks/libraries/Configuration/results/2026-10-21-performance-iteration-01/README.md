# Configuration key and path results

Local Windows ARM64 investigation on 2026-10-21, starting from cohesion `c2b25e4ac` and
cohesion-benchmarks `7a6aa1a`. Validation now formats its failure message only on invalid input.
String-backed keys retain their immutable input; span-backed keys still own a copy.
Nullable-key comparison correctness is repaired under cohesion #1679. Public signatures are unchanged.

| Operation | Median before | Median after | Allocated before | Allocated after |
| --- | ---: | ---: | ---: | ---: |
| Key from string | 59.7 ns | 2.4 ns | 312 B | 0 B |
| Key from span | 60.9 ns | 9.4 ns | 312 B | 56 B |
| Three-segment path parse | 188.6-196.9 ns | 63.1-68.2 ns | 1,000 B | 232 B |

[COMPARISON.md](COMPARISON.md) and the unedited `before/` / `after/` full BenchmarkDotNet reports
are the evidence. Path parsing allocates 76.8% less. These are microbenchmarks, not a provider-load
or end-to-end application throughput measurement.

Machine and exact command match the [cache result record](../../../Cache/results/2026-10-21-performance-iteration-01/README.md).
One launch, three warmups, eight measured iterations, in-process BenchmarkDotNet 0.15.8 on .NET 10.0.13 ARM64.
Local packages: `10.0.0-preview.1.perf.before` and `10.0.0-preview.1.perf.after`; no committed pin changes.

The new `KeyBenchmarks` cases separate successful validation and ownership from provider work.
The full affected configuration suite passed (232 tests). Baseline ConfigurationBinder IL2070/IL3050
warnings remain tracked by cohesion #1220; no suppression was added. NativeAOT publication reached
the missing ARM64 linker prerequisite, so native execution is not claimed.
