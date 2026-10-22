# Glob matching results

Local Windows ARM64 investigation on 2026-10-21, starting from cohesion `c2b25e4ac` and
cohesion-benchmarks `7a6aa1a`. The parse cache now retains length bounds in an internal immutable
pattern object. Each match performs a constant-time precheck instead of walking every token.
Cached parsing still allocates one Glob of the same size; one metadata object is retained per
cached pattern, within the existing cache-clear policy.

The fixed-length pattern's 64-input batch fell from **6.66 us to 2.47 us** (about 63% less time).
The variable-length pattern fell from 6.49 us to 5.92 us on the repeat run. Both remain allocation-free.
The first shorter run showed inconsistent variable-pattern timing, so the repeat is the evidence
used here; the smaller variable-pattern improvement should be treated with more caution.

[COMPARISON.md](COMPARISON.md) and `before/` / `after/` retain the full reports for the repeat.
Machine: Windows 11 ARM64, Snapdragon X X1E80100, .NET SDK 10.0.402 / runtime 10.0.13,
BenchmarkDotNet 0.15.8 in-process, one launch, five warmups and 15 measured iterations.
Local package versions: `10.0.0-preview.1.perf.before` and `10.0.0-preview.1.perf.after2`.
The benchmark code, input patterns and names did not change.

The ignored temporary runner references locally packed NuGet packages and links the suite classes:

```powershell
dotnet run -c Release --project .artifacts/performance/src/PerformanceIteration.Benchmarks.csproj `
  -p:CohesionPackageVersion=<before-or-after-version> -p:RestoreConfigFile=<local-feed-config> -- `
  --profile ci --filter '*GlobBenchmarks.IsMatch' '*RoutePathBenchmarks*' `
  --warmupCount 5 --iterationCount 15 --artifacts <output>
```

This folder preserves the glob reports only. The full Core suite passed (475 tests, one existing
skip), including concurrent parsing, brace groups, immutable token views and cached-parse allocations.
NativeAOT publication could not link because the machine lacks ARM64 C++ linker tools.
