# HTTP media-type parsing, formatting and equality: iteration 5

Package-only BenchmarkDotNet runs on Windows 11 ARM64, Snapdragon X X1E80100,
SDK 10.0.402 / runtime 10.0.13, BenchmarkDotNet 0.15.8, repository in-process harness.
Both repositories remain on `codex/library-performance` with their committed pins unchanged.

## Scope and packages

- Baseline `10.0.0-preview.1.perf.media.before`: original `HttpMediaType` from cohesion
  base `c2b25e4ac`, with all earlier iteration changes still present elsewhere.
- Candidate `10.0.0-preview.1.perf.media.final4`: direct string concatenation without
  parameters, delayed temporary list creation until a second valid parameter, and the
  owner-approved repeated-name equality correction.
- Intermediate `media.after` was used for API/native validation before the equality fix;
  it contributes no retained timing measurements.

Public signatures, parsing/storage rules, first-occurrence lookup and hashing remain
unchanged. The owner approved equality that ignores ordering between distinct names but
preserves each repeated name's value sequence. No parsed-input or rendered-string cache
is added. Equality uses bounded stack space; reordered lists above 4,096 parameters
use a heap bitmap to avoid trading execution time for lower memory use.

## Workloads

Sixteen new media-type cases parse/render plain, charset, multiple-parameter and escaped
inputs in Cohesion and ASP.NET Core. Both formatters start from parsed values and return
the identical canonical text. A counterpart may retain/reuse the original text, so this
does not require both to allocate a fresh formatted string. Six existing Content-Type/Range
parser cases are controls. Eight unique-name equality cases compare original/reversed order
at 1, 2, 65 and 4,097 parameters; every input compares equal in both implementations. The old
incorrect duplicate-name comparisons are regression tests, not equivalent-work benchmarks.

The initial 28-case matrix and two intermediate equality revisions exposed execution
regressions, so those implementations were rejected. Their raw reports/comparisons remain
under `initial/`, `rejected-equality2/` and `rejected-equality3/`. The owner explicitly
disallowed saving memory at the expense of execution time. Final measurements recheck
ordinary and large reversed lists, as well as parsing/rendering controls. All runs use
one launch, three warmups and eight measured iterations. Builds, tests and NativeAOT
compilation do not overlap timing samples. Full JSON is copied unedited; comparisons
use medians and exact bytes. Background allocations affect reported bytes/op on slow,
low-invocation-count large-list benchmarks; the bitmap itself is 544 B for 4,097 entries
and is created only when the sequences differ.

The final matrix combines 22 parsing/rendering/control cases and eight equality cases.
Both pairs run candidate then baseline, reversing the initial order. A longer isolated
two-parameter reversed comparison uses five warmups and twelve measurements; its full-matrix
5% increase reverses to 20.1 ns before versus 17.4 ns after. See
[small repeat](small-repeat/COMPARISON.md) and its unedited reports.

| Workload | Median before | Median after | Bytes before | Bytes after |
| --- | ---: | ---: | ---: | ---: |
| Plain rendering | 18.2 ns | 9.7 ns | 160 | 56 |
| Charset parsing | 121.2 ns | 108.3 ns | 296 | 176 |
| Multiple-parameter parsing | 203.4 ns | 208.7 ns | 424 | 424 |
| Escaped-parameter parsing | 239.4 ns | 226.2 ns | 424 | 424 |
| 65 ordered parameters, equality | 6.27 us | 324.3 ns | 0 | 0 |
| 65 reversed parameters, equality | 6.54 us | 4.83 us | 0 | 0 |
| 4,097 ordered parameters, equality | 19.10 ms | 28.20 us | background noise | 0 |
| 4,097 reversed parameters, equality | 18.86 ms | 16.94 ms | background noise | one bitmap |

Plain rendering is about 47% faster and allocates 65% less; charset parsing is about 11%
faster and allocates 40.5% less. The small multiple-/escaped-/unparameterized parsing timing
differences change sign across the initial and final pairs. No memory-saving change is
retained on the strength of lower allocation while accepting a repeatable CPU slowdown.

All thirty final cases match, with zero failed measurements. The automatic comparison flags
two unchanged ASP.NET Core formatters (11%/13% timing movement) and the large bitmap's
allocation. Those counterpart timing directions reverse from the initial run. The 4,097-entry
reordered equality case runs faster but adds a 544-byte bitmap to preserve correctness and
bound stack use. The measured totals are 618 B/op after and 42 B/op baseline background noise.
Ordinary comparisons allocate zero. These stress-sized equality cases do not represent
typical Content-Type headers.

## Reproduction

Pack Core and then Http in cohesion with `dotnet pack -c Release`, using
`-p:CohesionPatchVersion=0-preview.1.perf.media.before` or `0-preview.1.perf.media.final4`.
Output to distinct ignored local feeds. For the baseline restore only `HttpMediaType.cs`
from the base commit, preserving all prior iteration changes; restore the candidate after
packing. Do not overwrite an already-restored version with different binaries.

Use an ignored temporary `.Benchmarks.csproj` in this repository, importing the normal
harness. Reference Http via `CohesionReference`, add `Microsoft.AspNetCore.App` as a framework
reference and explicitly compile these existing benchmark sources:

```xml
<ItemGroup>
  <CohesionReference Include="Assimalign.Cohesion.Http" />
  <FrameworkReference Include="Microsoft.AspNetCore.App" />
  <Compile Include="$(BenchmarksRoot)benchmarks/libraries/Http/benchmarks/Assimalign.Cohesion.Http.Benchmarks/src/Http/HttpMediaTypeBenchmarks.cs" />
  <Compile Include="$(BenchmarksRoot)benchmarks/libraries/Http/benchmarks/Assimalign.Cohesion.Http.Benchmarks/src/Http/HttpMediaTypeEqualityBenchmarks.cs" />
  <Compile Include="$(BenchmarksRoot)benchmarks/libraries/Http/benchmarks/Assimalign.Cohesion.Http.Benchmarks/src/Http/HttpHeaderParserBenchmarks.cs" />
</ItemGroup>
```

Explicitly compile a temporary entry point calling
`BenchmarkProgram.Run(args, typeof(Program).Assembly)`. The recorded runner is
`.artifacts/performance/media/MediaIteration.Benchmarks.csproj`. A temporary NuGet config
maps `Assimalign.Cohesion.*` exclusively to the local feeds and other packages to nuget.org;
the committed feed configuration remains untouched.

```powershell
dotnet run -c Release --project .artifacts/performance/media/MediaIteration.Benchmarks.csproj `
  -p:CohesionPackageVersion=10.0.0-preview.1.perf.media.final4 `
  -p:RestoreConfigFile=<absolute-temporary-config> -- `
  --profile ci --filter '*HttpMediaType*' '*HttpHeaderParser*' `
  --warmupCount 3 --iterationCount 8 --artifacts .artifacts/performance/media-final-after

# The baseline can use the same full filter, or combine the two recorded baseline runs.
./scripts/Compare-Benchmarks.ps1 -Baseline <before-json-directory> `
  -Current <after-json-directory> -OutputPath <comparison.md> -FailOnRegression
```

Interpret timing differences against the unchanged controls and their spread. Allocation
savings are deterministic; local parsing/rendering improvements are not HTTP throughput
claims. The equality law tests and 4,096/4,097-entry boundary tests establish correctness
separately from the large-list performance cases.
