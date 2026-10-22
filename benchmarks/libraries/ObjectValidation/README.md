# ObjectValidation benchmarks

Latest measurements: [iteration 10 execution results](results/2026-10-22-performance-iteration-10/README.md).

26 package-only cases compare Cohesion with the existing FluentValidation 11.11.0 pin.
Both sides validate the same fixtures and setup checks equal error counts. Validators are
built and exercised outside warm validation measurements.

| Workload | Cases | Behavior |
| --- | ---: | --- |
| Registration and comparisons | 6 | Valid/three-error registration and four-error numeric/date comparisons |
| Scalar collections | 8 | Four or 32 tags, valid or every fourth tag invalid, both rules evaluated |
| Nested profiles and collections | 8 | One address or eight child addresses, valid or two errors per invalid address |
| Profile construction | 4 | Parent plus nested profile, optionally with a collection profile; build cost only |

Registration/comparison rule chains stop on their first failure. The dedicated collection
and nested suites continue through their chains on both sides. Member and element traversal
continue so neither side wins by omitting failures.

Cohesion also records elapsed validation time and a timed invocation record for each rule;
the FluentValidation result does not contain equivalent per-rule instrumentation. These
measurements preserve Cohesion's current diagnostics contract rather than disabling work to
claim parity. Library-specific error messages and source formats are preserved, not made identical.

The suite does not establish parity for arbitrary custom validators, asynchronous validators,
all rule types, database-backed checks, every cascade policy or application throughput.
NativeAOT compatibility is checked separately from these JIT timing samples.

Run with the repository harness:

```powershell
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area ObjectValidation -Profile ci
```

For before/after work, consume uniquely versioned local packages through the
`CohesionPackageVersion` override. Keep counterpart versions, cases, warmups, measurement
counts and affinity matched, and preserve raw reports and comparison flags.
