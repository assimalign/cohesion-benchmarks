# ObjectMapping execution performance — iteration 9

Measured on Windows 11 ARM64, Snapdragon X X1E80100, .NET SDK 10.0.402/runtime
10.0.13, BenchmarkDotNet 0.15.8 with the repository's in-process CI harness.
AutoMapper remains at the existing 13.0.1 pin. Neither committed package pin changes.

## Retained result

The retained package is `10.0.0-preview.1.perf.mapping.inline`. All eight warm
member-mapping workloads improve against the original performance-branch package.
Existing-target and runtime-Type dispatch beat AutoMapper; flat creation is near
parity across runs. Collections and nested graphs still trail AutoMapper.

Medians from the final normal-affinity run, compared with the matching original
cases in `confirmation/before`:

| Workload | Original Cohesion | Retained Cohesion | Final AutoMapper | Cohesion bytes before → after |
| --- | ---: | ---: | ---: | ---: |
| Existing flat target | 58.0 ns | 20.6 ns | 47.7 ns | 88 → 0 |
| Runtime-Type new target | 77.1 ns | 33.7 ns | 49.8 ns | 144 → 56 |
| Generic new flat target | 91.8 ns | 47.7 ns | 54.3 ns | 144 → 56 |
| Collection, 1 item | 90.9 ns | 82.8 ns | 73.9 ns | 240 → 152 |
| Collection, 100 items | 6.73 us | 5.02 us | 1.10 us | 15,288 → 6,488 |
| Nested graph, 1 item | 410.0 ns | 226.2 ns | 81.6 ns | 752 → 376 |
| Nested graph, 10 items | 961.8 ns | 555.2 ns | 193.8 ns | 2,288 → 952 |
| Nested graph, 100 items | 7.59 us | 4.08 us | 970.2 ns | 17,112 → 6,712 |

AutoMapper's flat control varies: an earlier confirmation measured Cohesion 50.1 ns
against AutoMapper 46.0 ns. The latest flat win does not establish an unconditional
advantage. Callback controls use explicitly supplied four-field copy code and must
be assessed separately from automatic member/convention mapping.

## Evidence and flags

`RUNS.json` identifies every package, case count, warmup/measurement count and affinity.
`SHA256.csv` verifies unedited JSON and markdown copies against original reports;
`PACKAGES.csv` identifies the isolated package binaries. The original package includes
earlier performance iterations and is not the pristine repository head.
The archive contains 46 full JSON reports, 46 markdown tables, eight comparison reports,
92 verified report hashes and 12 package hashes across sixteen runs.

- `INITIAL_COMPARISON.md`: no flags; eight added callback cases.
- `CONFIRMATION_COMPARISON.md`: six flags, comprising two Cohesion configuration
  timings and four AutoMapper configuration allocation flags, one also a timing flag.
- `CONFIGURATION_RECHECK.md`: three flags, one Cohesion graph configuration timing
  and two unchanged AutoMapper controls. The owner accepts configuration-only startup cost.
- `rejected-sequence/after`: discarded extra top-level array/List loop. Its extra
  32-byte saving did not establish an acceptable execution-time benefit.
- `SINGLE_CORE_COMPARISON.md`: one Cohesion runtime-dispatch flag. Small internal/private
  helpers subsequently receive inlining hints. Absolute single-core times differ sharply
  from normal-affinity measurements and must not be combined with them.
- `RUNTIME_RECHECK.md`: matched isolated runtime repeat after inlining; no flags.
  The original sixteen-case single-core order was not repeated after inlining, so the
  isolated repeat does not establish that every case-history/affinity combination is unaffected.
- `FINAL_NORMAL_COMPARISON.md`: two flags, both unchanged AutoMapper timing controls.
  All eight Cohesion member-mapping cases improve and allocate less.
- `FINAL_CALLBACK_COMPARISON.md`: two flags, a Cohesion custom-profile callback timing
  and the unchanged AutoMapper context-callback control.
- `CALLBACK_RECHECK.md`: matched eight-case callback repeat, with no flags. Custom-profile
  dispatch is 39.6 → 31.3 ns with unchanged 80-byte allocation; context callbacks are
  30.1 → 17.1 ns (88 → 48 B), typed callbacks 27.3 → 7.9 ns (88 → 0 B), overridden
  profiles 36.8 → 36.0 ns (80 B). The earlier custom-profile slowdown does not repeat.

Focused comparisons label unselected cases `removed`; those methods remain in the
32-case benchmark suite. No flag or raw report is removed to improve the result.

No producer builds, tests or native compilation overlap timing samples. The retained
producer passes 124 library tests, three generator tests, strict package API validation
and a package-only Windows ARM64 NativeAOT smoke. Twelve compatibility cases also pass
against the original package. Public APIs and custom context identity are preserved.

## Reproduction

Build isolated Core and ObjectMapping packages using unique `CohesionPatchVersion`
values, then override `CohesionPackageVersion` for a package-only benchmark runner.
Use `--profile ci --warmupCount 5 --iterationCount 12`, selecting the four member-mapping
classes or `MappingCallbackBenchmarks` separately. Use the same cases and affinity for
both packages; keep the normal-affinity and CPU-8 sensitivity results separate.

The tracked producer report is `docs/performance/LIBRARY_PERFORMANCE_ITERATION_09.md`
in the Cohesion companion. Work item: [#1700](https://github.com/assimalign/cohesion/issues/1700).
