# HTTP Accept ordering: performance iteration 4

Package-only BenchmarkDotNet measurements on Windows 11 ARM64, Snapdragon X X1E80100,
SDK 10.0.402 / runtime 10.0.13, BenchmarkDotNet 0.15.8, repository in-process harness.
Both repositories use `codex/library-performance`. No producer project reference or
committed version-pin change is used.

## Packages and matrix

- Baseline: `10.0.0-preview.1.perf.accept.before`, original `HttpAcceptParser` from cohesion
  base `c2b25e4ac`, with earlier iteration Core improvements still included.
- Initial candidate: `10.0.0-preview.1.perf.accept.after`, insertion sorting through sixteen
  valid entries for both media and token lists. Its sixteen-entry media regression was
  rejected; [initial comparison](initial/COMPARISON.md) and unedited JSON remain visible.
- Final candidate: `10.0.0-preview.1.perf.accept.final`, insertion sorting through eight
  valid media entries and sixteen valid tokens, with the original stable sort above each
  bound. No public API, parsing rule or dependency changes.

The final twenty cases cover media/token lists of 1, 8, 9, 16, 17 and 64 entries, existing
browser Accept parsing plus its ASP.NET Core counterpart, and six unchanged Content-Type
and Range parser controls. Synthetic inputs increase quality with equal-ranked pairs,
requiring moves while exercising stable ties. Initial eighteen-case runs omit nine entries.
The browser input and all existing benchmark identifiers remain unchanged.

Initial runs use five warmups and ten measured iterations, baseline then candidate. Final
runs reverse the order: candidate then baseline, three warmups and eight measurements,
one launch per case. No CPU-heavy builds, tests or native compilation overlap sampling.
The [final comparison](COMPARISON.md) uses medians and exact allocation bytes. Raw full JSON
is copied without editing into `before/`, `after/`, and `initial/{before,after}/`.

## Reproduction

Pack Core, then Http, in the cohesion worktree using `dotnet pack -c Release` and
`-p:CohesionPatchVersion=0-preview.1.perf.accept.before` or `0-preview.1.perf.accept.final`,
with output in separate ignored feeds. For baseline packing restore only `HttpAcceptParser.cs`
from the base commit, preserving earlier iteration changes, then restore the candidate.
Never overwrite a version already restored to the NuGet cache with different binaries.

Create an ignored temporary project ending `.Benchmarks.csproj` in the benchmark repository
so it imports the normal harness. Reference only Http via `CohesionReference`, add the
`Microsoft.AspNetCore.App` framework reference, and explicitly link these existing sources:

```xml
<ItemGroup>
  <CohesionReference Include="Assimalign.Cohesion.Http" />
  <FrameworkReference Include="Microsoft.AspNetCore.App" />
  <Compile Include="$(BenchmarksRoot)benchmarks/libraries/Http/benchmarks/Assimalign.Cohesion.Http.Benchmarks/src/Http/HttpAccept*Benchmarks.cs" />
  <Compile Include="$(BenchmarksRoot)benchmarks/libraries/Http/benchmarks/Assimalign.Cohesion.Http.Benchmarks/src/Http/HttpHeaderParserBenchmarks.cs" />
</ItemGroup>
```

Add an explicit `Compile` item for a temporary entry point calling
`BenchmarkProgram.Run(args, typeof(Program).Assembly)`. Use a temporary NuGet config mapping
`Assimalign.Cohesion.*` exclusively to the local before/after feeds and other packages to
nuget.org. Do not modify the committed feed configuration. Recorded local project:
`.artifacts/performance/accept/AcceptIteration.Benchmarks.csproj`.

```powershell
dotnet run -c Release --project .artifacts/performance/accept/AcceptIteration.Benchmarks.csproj `
  -p:CohesionPackageVersion=10.0.0-preview.1.perf.accept.final `
  -p:RestoreConfigFile=<absolute-temporary-config> -- `
  --profile ci --filter '*HttpAccept*' '*HttpHeaderParser*' `
  --warmupCount 3 --iterationCount 8 --artifacts .artifacts/performance/accept-final-after

# Repeat with the before version and a separate artifact path, after the candidate completes.
./scripts/Compare-Benchmarks.ps1 -Baseline <before-json-directory> `
  -Current <after-json-directory> -OutputPath <comparison.md> -FailOnRegression
```

## Interpretation

The rejected initial sixteen-entry media path saved allocations but increased median time
about 20%. Two unchanged controls also moved over 10% in that pair (ASP.NET Core Accept and
Cohesion Content-Type), so those timings alone do not establish code regressions. Final
measurements include both new size boundaries and repeat all controls. Allocation savings
are deterministic; sub-microsecond timing differences on one machine do not establish
end-to-end request throughput.

Browser Accept median time is 1,113.2 ns before versus 903.3 ns after in the final pair;
allocation falls from 2,384 B to 1,536 B (848 B / 35.6%). Its initial time reduction was
about 9%, compared with 18.9% in the repeat. Eight-media-entry sorting was nearly neutral
initially and about 13% lower in the repeat, so its timing gain is treated cautiously.
One-/eight-/sixteen-token medians in the final pair fall from 105.1 / 640.8 / 1,403.5 ns
to 64.3 / 484.0 / 1,047.9 ns; allocation reductions persist across both pairs.
The final sixteen-media-entry case returns to 2,433.7 ns before versus 2,399.1 ns after,
with the original 4,064 B allocation in both.

The final comparison matches all twenty cases with zero failed measurements and zero
regressions at the script's 10% timing / exact allocation thresholds. Large-list cases
remain within about 4% and allocate identical bytes. Unchanged Cohesion Content-Type and
Range controls also improve about 11% and 19% in that pair, demonstrating environmental
variation. Their gains are not attributed to the code change, and the exact browser timing
gain cannot be separated from that variation. The repeatable allocation reduction is the
strongest conclusion.
