# ObjectMapping benchmarks

Latest measurements: [iteration 9 execution results](results/2026-10-22-performance-iteration-09/README.md).

32 cases for Assimalign.Cohesion.ObjectMapping: four original cases plus 28 additions.

| Added workload | Cases | Shape |
| --- | ---: | --- |
| Collection | 4 | 1/100 flat sources to a materialized List of new targets |
| Nested graph | 4 | Customer and 1/100 newly mapped line items |
| Dispatch | 4 | Reused flat target with null overwrites; runtime Type dispatch to a new target |
| Configuration and first use | 8 | One flat profile/four graph profiles; configure only or configure and execute every map |
| Callback and custom-profile controls | 8 | Existing targets through typed/context callbacks, an external profile, and a derived profile overriding its interface action list |

All additions compare with the existing MIT-licensed AutoMapper 13.0.1 pin. Its version and the
repository's existing advisory suppression are unchanged. Both sides map the same members and produce
the same target types; setup checks all mapped values. Warm mapping prepares and exercises both
mappers outside measurement. Configuration benchmarks intentionally measure profile registration and
mapper creation. ConfigureAndMap also pays first-use compilation on AutoMapper; Cohesion's AOT-safe
delegate profiles need no runtime expression compilation. It measures configuration plus first calls,
not an isolated compiler or a process-cold startup.

The callback controls copy the same four values with explicitly supplied user code. They measure
custom extension-point dispatch and preserve its coverage during optimization; they are not
convention/member-based mapping comparisons. Report their AutoMapper ratios separately from
`MapperBenchmarks`, `CollectionMappingBenchmarks`, and `NestedMappingBenchmarks`.

Riok.Mapperly was considered and omitted because it generates mapping code at build time and does
not provide the equivalent runtime configuration lifecycle. No additional counterpart package was needed.

Not covered by timing: expression-based Cohesion profiles, generator/build-time cost, cyclic or polymorphic
graphs, flattening/conversions, null nested collections, collection merge policies, and query projection.

Validation:

```powershell
dotnet build benchmarks/libraries/ObjectMapping/benchmarks/Assimalign.Cohesion.ObjectMapping.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area ObjectMapping -Profile dry
```
