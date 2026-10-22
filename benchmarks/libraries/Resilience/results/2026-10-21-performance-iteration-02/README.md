# Timeout completion results

Local Windows ARM64 investigation on 2026-10-21, continuing `codex/library-performance` in both
repositories. Cohesion starts from `c2b25e4ac` plus iteration 1's changes; the companion repository
starts from `7a6aa1a` plus iteration 1. The timeout implementation is unchanged in the baseline.
Both versions contain the same preceding Core improvements. No committed package pin changes.

The timeout strategy now consumes synchronously completed outcomes directly. Incomplete callbacks
use `Task.WaitAsync` so the losing deadline timer is disposed when a callback finishes. Previously
every successful execution left its `Task.Delay` timer pending for the full budget.

| Workload | Median before | Median after | Allocated before | Allocated after |
| --- | ---: | ---: | ---: | ---: |
| Synchronous value callback | 1.17 us | 201.1 ns | 560 B | 240 B |
| Synchronous void callback | 969.9 ns | 244.5 ns | 552 B | 216 B |
| Callback yielding once | 3.87 us | 2.39 us | 1,557 B | 1,469 B |
| Unchanged Polly synchronous control | 179.8 ns | 166.4 ns | 0 B | 0 B |

The synchronous value path allocates 57.1% less, and the void path 60.9% less. The yielding
callback includes scheduler cost and can finish at different points in the strategy's setup;
its timing and allocation counts are workload averages, not a deterministic per-path cost.
These microbenchmarks do not establish application throughput or cross-machine rankings.

## Reports and repeat methodology

Machine: Windows 11 ARM64, Snapdragon X 12-core X1E80100, SDK 10.0.402 / runtime 10.0.13.
BenchmarkDotNet 0.15.8, repository in-process harness, one launch, five warmups, 15 measured
iterations. Local package versions: `10.0.0-preview.1.perf.timeout.before` and
`10.0.0-preview.1.perf.timeout.after`. Inputs, callback behavior, timeout budget and benchmark
identifiers match between packages. The existing one-second budget remains unchanged.

The first expanded run measured baseline then candidate. Its unchanged Polly control also
improved by about 21%, so the synchronous pair was repeated in reverse order (candidate then
baseline). The repeat again reports 560 B versus 240 B for Cohesion, with about 7.5% control
timing variation. The baseline timer accumulation creates GC pressure; use the repeated
synchronous result and deterministic allocation savings instead of attributing all expanded
timing changes to instruction cost.

`before/` and `after/` contain unedited full JSON reports: the synchronous value/control reports
come from the reverse-order repeat, while void/yield reports come from the expanded run.
`expanded-run/` retains the original synchronous/control reports to expose the timing drift.
[COMPARISON.md](COMPARISON.md) compares the retained reports and finds no regression at the
script's 10% timing / exact allocation thresholds.

The ignored temporary runner links the two timeout benchmark classes and uses NuGet packages,
with local source mapping isolated from the committed feed configuration:

```powershell
dotnet run -c Release --project .artifacts/performance/timeout/TimeoutIteration.Benchmarks.csproj `
  -p:CohesionPackageVersion=<before-or-after-version> -p:RestoreConfigFile=<local-feed-config> -- `
  --profile ci --filter '*TimeoutPipelineBenchmarks*' '*TimeoutCompletionBenchmarks*' `
  --warmupCount 5 --iterationCount 15 --artifacts <output>
```

The reverse-order repeat uses only `'*TimeoutPipelineBenchmarks*'`. The focused package consumer
also passed synchronous value, yielding value and synchronous void smoke checks with AOT analyzers
enabled. Native publication for `win-arm64` still stops at missing C++ platform linker tools.
Strict SDK package validation preserves public API shape, parameter names and attributes.
All seven Resilience suites pass (27 tests, including 12 Timeout tests); six of the initial seven
completion tests reproduced the issue against the baseline before the implementation changed.
The implementation and regression tests are tracked by
[#1681](https://github.com/assimalign/cohesion/issues/1681).
