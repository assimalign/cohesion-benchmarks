| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| CollectionMappingBenchmarks.MapCollection_AutoMapper(Count=1) | 64.9 ns | 62.4 ns | 0.96x | 144 | 144 |  |
| CollectionMappingBenchmarks.MapCollection_AutoMapper(Count=100) | 1.01 us | 907.3 ns | 0.90x | 7792 | 7792 | faster |
| CollectionMappingBenchmarks.MapCollection(Count=1) | 90.9 ns | 60.1 ns | 0.66x | 240 | 152 | faster |
| CollectionMappingBenchmarks.MapCollection(Count=100) | 6.73 us | 4.33 us | 0.64x | 15288 | 6488 | faster |
| MapperBenchmarks.MapFlat | 91.8 ns | 50.1 ns | 0.55x | 144 | 56 | faster |
| MapperBenchmarks.MapFlat_AutoMapper | 48.2 ns | 46.0 ns | 0.95x | 56 | 56 |  |
| MapperBenchmarks.MapNested | 961.8 ns | 566.0 ns | 0.59x | 2288 | 952 | faster |
| MapperBenchmarks.MapNested_AutoMapper | 202.9 ns | 174.6 ns | 0.86x | 952 | 952 | faster |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=ContextCallback) | 53.0 ns | 45.9 ns | 0.87x | 0 | 0 | faster |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=CustomProfile) | 55.3 ns | 45.4 ns | 0.82x | 0 | 0 | faster |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=OverriddenProfile) | 53.2 ns | 50.4 ns | 0.95x | 0 | 0 |  |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=TypedCallback) | 52.9 ns | 57.2 ns | 1.08x | 0 | 0 |  |
| MappingCallbackBenchmarks.MapCallback(Mode=ContextCallback) | 26.0 ns | 17.6 ns | 0.68x | 88 | 48 | faster |
| MappingCallbackBenchmarks.MapCallback(Mode=CustomProfile) | 41.3 ns | 31.4 ns | 0.76x | 80 | 80 | faster |
| MappingCallbackBenchmarks.MapCallback(Mode=OverriddenProfile) | 39.4 ns | 32.3 ns | 0.82x | 80 | 80 | faster |
| MappingCallbackBenchmarks.MapCallback(Mode=TypedCallback) | 28.7 ns | 7.9 ns | 0.27x | 88 | 0 | faster |
| MappingConfigurationBenchmarks.Configure_AutoMapper(Graph=False) | 83.99 us | 102.33 us | 1.22x | 38762 | 38803 | REGRESSION |
| MappingConfigurationBenchmarks.Configure_AutoMapper(Graph=True) | 174.33 us | 188.53 us | 1.08x | 81800 | 81832 | REGRESSION |
| MappingConfigurationBenchmarks.Configure(Graph=False) | 95.8 ns | 113.5 ns | 1.18x | 464 | 464 | REGRESSION |
| MappingConfigurationBenchmarks.Configure(Graph=True) | 282.8 ns | 340.9 ns | 1.21x | 1224 | 1224 | REGRESSION |
| MappingConfigurationBenchmarks.ConfigureAndMap_AutoMapper(Graph=False) | 267.77 us | 259.11 us | 0.97x | 68558 | 68595 | REGRESSION |
| MappingConfigurationBenchmarks.ConfigureAndMap_AutoMapper(Graph=True) | 1.79 ms | 1.59 ms | 0.89x | 234275 | 234769 | REGRESSION |
| MappingConfigurationBenchmarks.ConfigureAndMap(Graph=False) | 166.4 ns | 123.5 ns | 0.74x | 608 | 520 | faster |
| MappingConfigurationBenchmarks.ConfigureAndMap(Graph=True) | 1.89 us | 1.16 us | 0.61x | 3912 | 2312 | faster |
| MappingDispatchBenchmarks.MapExisting | 58.0 ns | 25.1 ns | 0.43x | 88 | 0 | faster |
| MappingDispatchBenchmarks.MapExisting_AutoMapper | 52.8 ns | 51.6 ns | 0.98x | 0 | 0 |  |
| MappingDispatchBenchmarks.MapRuntime | 77.1 ns | 37.3 ns | 0.48x | 144 | 56 | faster |
| MappingDispatchBenchmarks.MapRuntime_AutoMapper | 58.5 ns | 60.0 ns | 1.03x | 56 | 56 |  |
| NestedMappingBenchmarks.MapGraph_AutoMapper(Items=1) | 95.0 ns | 99.1 ns | 1.04x | 280 | 280 |  |
| NestedMappingBenchmarks.MapGraph_AutoMapper(Items=100) | 1.13 us | 1.07 us | 0.94x | 7136 | 7136 |  |
| NestedMappingBenchmarks.MapGraph(Items=1) | 410.0 ns | 295.7 ns | 0.72x | 752 | 376 | faster |
| NestedMappingBenchmarks.MapGraph(Items=100) | 7.59 us | 5.40 us | 0.71x | 17112 | 6712 | faster |

6 regression(s): time ratio over 1.1 or allocations over 1 x.
