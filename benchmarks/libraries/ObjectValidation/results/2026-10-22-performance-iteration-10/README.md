# ObjectValidation performance — iteration 10

All eleven retained warm Cohesion workloads improve against the original package. Cohesion beats
FluentValidation on the failing registration and numeric/date comparison fixtures, and wins the
32-item invalid tag case in this final run. Invalid nested objects/collections are near parity in
the final run; successful validation still trails FluentValidation. The results do not establish
universal counterpart parity.

The retained package is `10.0.0-preview.1.perf.validation10.strings`. Both packages use the same
26 cases, five warmups, twelve measured iterations and normal affinity. Counterpart: the existing
FluentValidation 11.11.0 pin. Windows 11 ARM64, Snapdragon X X1E80100, SDK 10.0.402/runtime 10.0.13,
BenchmarkDotNet 0.15.8 in-process harness. No tests/builds/native compilation overlap measurements.

## Final medians

True/False denotes the valid/invalid fixture. Profile build is separate from warm validation.

| Workload | Original Cohesion | Retained Cohesion | FluentValidation in final run | Cohesion bytes before → after |
| --- | ---: | ---: | ---: | ---: |
| Tags, 32 items (False) | 19.79 us | 7.15 us | 8.38 us | 157240 → 21538 |
| Tags, 32 items (True) | 17.83 us | 7.06 us | 5.05 us | 157424 → 10977 |
| Tags, 4 items (False) | 2.81 us | 1.15 us | 960.4 ns | 21464 → 4552 |
| Tags, 4 items (True) | 2.56 us | 987.0 ns | 593.6 ns | 21472 → 3232 |
| Nested collection, 8 items (False) | 17.22 us | 14.35 us | 14.78 us | 137304 → 82777 |
| Nested collection, 8 items (True) | 16.83 us | 5.89 us | 3.82 us | 126040 → 26899 |
| Nested address (False) | 2.56 us | 2.05 us | 2.20 us | 17200 → 11761 |
| Nested address (True) | 1.85 us | 1.14 us | 564.7 ns | 16056 → 5041 |
| Profile build, collection=False | 2.26 us | 2.20 us | 1.91 us | 6112 → 6112 |
| Profile build, collection=True | 4.12 us | 4.50 us | 4.25 us | 10952 → 10952 |
| Successful registration | 3.05 us | 1.41 us | 950.3 ns | 23992 → 3528 |
| Failing numeric/date comparisons | 2.64 us | 2.26 us | 3.76 us | 21128 → 8553 |
| Failing registration | 2.72 us | 1.83 us | 3.69 us | 21672 → 6945 |

Cohesion retains per-rule elapsed times and invocation records; FluentValidation does not return
equivalent instrumentation. Setup verifies equivalent error counts and cascade behavior. The
comparison preserves these existing contracts rather than omitting validation or diagnostics.

## Changes and validation

- Create concurrent context queues only when first written or publicly read. Public collection
  identity, retained live views, concurrent publication and callback context lifetimes remain intact.
- Internal immediate error enumeration avoids allocating empty built-in context queues.
- Six empty/length string rules use direct checks rather than falling through to character
  enumeration and boxing. UTF-16 lengths, whitespace, null handling and non-string fallbacks stay intact.
- 306 library tests pass. All fifteen new compatibility cases also pass the original package.
  Strict public API validation and package-only ARM64 NativeAOT publication/execution pass.

## Complete evidence

Eight runs preserve 26 unedited full JSON reports and 26 markdown tables. `SHA256.csv` contains
52 verified report hashes; `PACKAGES.csv` identifies eight Core/Validation package binaries across
four isolated versions. `RUNS.json` records packages, counts, sampling, order and affinity. Git
attributes preserve the raw reports byte-for-byte despite local line-ending conversion.

- `INITIAL_COMPARISON.md`: original then candidate1, three warmups/eight measurements, no flags.
  Several unchanged FluentValidation controls also become faster, limiting causal timing attribution.
- `CONFIRMATION_COMPARISON.md`: intermediate queue refinement then original, five/twelve;
  ten flags, comprising nine unchanged FluentValidation controls and one Cohesion invalid nested
  timing increase, 2.13 -> 2.38 us.
- `NESTED_RECHECK.md`: matched eight-case intermediate nested repeat, five/twenty;
  no flags. Invalid nested improves 1.85 -> 1.60 us, invalid collection 17.80 -> 13.69 us;
  the earlier Cohesion slowdown does not repeat. All four Cohesion cases improve.
- `FINAL_STRINGS_COMPARISON.md`: retained string refinement then original, five/twelve;
  five flags, all unchanged FluentValidation timing controls. Every Cohesion warm case improves.
  Collection profile build is 4.12 -> 4.50 us with unchanged 10,952-byte allocation; no setup
  speed improvement is claimed. The source profile-construction path is unchanged.

All flags remain visible. Earlier iteration 7 span-based source-composition candidates remain
rejected in the Content iteration 7 evidence; their algorithms are unchanged here. These
microbenchmarks do not establish application throughput, all-rule parity or NativeAOT speed ratios.

Producer report: `docs/performance/LIBRARY_PERFORMANCE_ITERATION_10.md` in Cohesion.
Work item: [#1703](https://github.com/assimalign/cohesion/issues/1703).
