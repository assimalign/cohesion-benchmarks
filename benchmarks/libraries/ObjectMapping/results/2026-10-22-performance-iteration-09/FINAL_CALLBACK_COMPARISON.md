| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| CollectionMappingBenchmarks.MapCollection_AutoMapper(Count=1) | 64.9 ns | | | 144 | | removed |
| CollectionMappingBenchmarks.MapCollection_AutoMapper(Count=100) | 1.01 us | | | 7792 | | removed |
| CollectionMappingBenchmarks.MapCollection(Count=1) | 90.9 ns | | | 240 | | removed |
| CollectionMappingBenchmarks.MapCollection(Count=100) | 6.73 us | | | 15288 | | removed |
| MapperBenchmarks.MapFlat | 91.8 ns | | | 144 | | removed |
| MapperBenchmarks.MapFlat_AutoMapper | 48.2 ns | | | 56 | | removed |
| MapperBenchmarks.MapNested | 961.8 ns | | | 2288 | | removed |
| MapperBenchmarks.MapNested_AutoMapper | 202.9 ns | | | 952 | | removed |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=ContextCallback) | 53.0 ns | 104.7 ns | 1.98x | 0 | 0 | REGRESSION |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=CustomProfile) | 55.3 ns | 59.4 ns | 1.07x | 0 | 0 |  |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=OverriddenProfile) | 53.2 ns | 53.1 ns | 1.00x | 0 | 0 |  |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=TypedCallback) | 52.9 ns | 51.5 ns | 0.97x | 0 | 0 |  |
| MappingCallbackBenchmarks.MapCallback(Mode=ContextCallback) | 26.0 ns | 22.9 ns | 0.88x | 88 | 48 | faster |
| MappingCallbackBenchmarks.MapCallback(Mode=CustomProfile) | 41.3 ns | 51.3 ns | 1.24x | 80 | 80 | REGRESSION |
| MappingCallbackBenchmarks.MapCallback(Mode=OverriddenProfile) | 39.4 ns | 36.0 ns | 0.91x | 80 | 80 |  |
| MappingCallbackBenchmarks.MapCallback(Mode=TypedCallback) | 28.7 ns | 8.1 ns | 0.28x | 88 | 0 | faster |
| MappingConfigurationBenchmarks.Configure_AutoMapper(Graph=False) | 83.99 us | | | 38762 | | removed |
| MappingConfigurationBenchmarks.Configure_AutoMapper(Graph=True) | 174.33 us | | | 81800 | | removed |
| MappingConfigurationBenchmarks.Configure(Graph=False) | 95.8 ns | | | 464 | | removed |
| MappingConfigurationBenchmarks.Configure(Graph=True) | 282.8 ns | | | 1224 | | removed |
| MappingConfigurationBenchmarks.ConfigureAndMap_AutoMapper(Graph=False) | 267.77 us | | | 68558 | | removed |
| MappingConfigurationBenchmarks.ConfigureAndMap_AutoMapper(Graph=True) | 1.79 ms | | | 234275 | | removed |
| MappingConfigurationBenchmarks.ConfigureAndMap(Graph=False) | 166.4 ns | | | 608 | | removed |
| MappingConfigurationBenchmarks.ConfigureAndMap(Graph=True) | 1.89 us | | | 3912 | | removed |
| MappingDispatchBenchmarks.MapExisting | 58.0 ns | | | 88 | | removed |
| MappingDispatchBenchmarks.MapExisting_AutoMapper | 52.8 ns | | | 0 | | removed |
| MappingDispatchBenchmarks.MapRuntime | 77.1 ns | | | 144 | | removed |
| MappingDispatchBenchmarks.MapRuntime_AutoMapper | 58.5 ns | | | 56 | | removed |
| NestedMappingBenchmarks.MapGraph_AutoMapper(Items=1) | 95.0 ns | | | 280 | | removed |
| NestedMappingBenchmarks.MapGraph_AutoMapper(Items=100) | 1.13 us | | | 7136 | | removed |
| NestedMappingBenchmarks.MapGraph(Items=1) | 410.0 ns | | | 752 | | removed |
| NestedMappingBenchmarks.MapGraph(Items=100) | 7.59 us | | | 17112 | | removed |

2 regression(s): time ratio over 1.1 or allocations over 1 x.
