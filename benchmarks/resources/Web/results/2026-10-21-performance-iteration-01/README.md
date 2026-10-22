# Route-path allocation results

Local Windows ARM64 investigation on 2026-10-21, starting from cohesion `c2b25e4ac` and
cohesion-benchmarks `7a6aa1a`. Catch-all matching now joins an array span instead of first
allocating a slice array. Splitting itself is unchanged: two span-splitting candidates were
discarded because they reduced allocations at the cost of slower matching.

| Shape | Median before | Median after | Allocated before | Allocated after |
| --- | ---: | ---: | ---: | ---: |
| Catch-all | 116.6 ns | 111.2 ns | 528 B | 480 B |
| Literal (unchanged control) | 59.2 ns | 58.9 ns | 192 B | 192 B |
| Parameter (unchanged control) | 111.4 ns | 118.7 ns | 384 B | 384 B |

The retained gain is **48 B fewer per catch-all match** (9.1%). Timing variation in unchanged
controls is visible, so this is an allocation result; the small catch-all timing difference
is not treated as a throughput promise. No case crosses the comparison script's 10% timing
regression threshold, and allocations do not increase.

[COMPARISON.md](COMPARISON.md) and the unedited full BenchmarkDotNet JSON in `before/` and
`after/` are the evidence. The benchmark is new: one public `Route.TryMatchPath` call without
pipeline/hosting overhead, with fixed literal, constrained parameter and catch-all fixtures.
Setup checks the match and capture count.

Machine: Windows 11 ARM64, Snapdragon X X1E80100, .NET SDK 10.0.402 / runtime 10.0.13,
BenchmarkDotNet 0.15.8 in-process, one launch, five warmups and 15 measured iterations.
Local package versions: `10.0.0-preview.1.perf.before` and `10.0.0-preview.1.perf.final2`.
Neither package is published, and the committed package pin is unchanged.

The temporary runner uses NuGet packages only; its ignored config maps Cohesion to the local feed.
Both sides use the identical five-case selection to keep preceding warmup work consistent:

```powershell
dotnet run -c Release --project .artifacts/performance/src/PerformanceIteration.Benchmarks.csproj `
  -p:CohesionPackageVersion=<before-or-after-version> -p:RestoreConfigFile=<local-feed-config> -- `
  --profile ci --filter '*GlobBenchmarks.IsMatch' '*RoutePathBenchmarks*' `
  --warmupCount 5 --iterationCount 15 --artifacts <output>
```

Only the routing reports are preserved here. The full routing suite passes (398 tests), including
root paths, repeated/trailing slashes, catch-all captures, precedence, constraints and method behavior.
Strict package API validation preserves signatures, parameter names and attributes. A focused
NativeAOT consumer could not be linked because the machine lacks ARM64 C++ linker tools; native
execution remains unverified.
