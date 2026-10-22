| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| CollectionMappingBenchmarks.MapCollection_AutoMapper(Count=1) | 64.9 ns | 73.9 ns | 1.14x | 144 | 144 | REGRESSION |
| CollectionMappingBenchmarks.MapCollection_AutoMapper(Count=100) | 1.01 us | 1.10 us | 1.09x | 7792 | 7792 |  |
| CollectionMappingBenchmarks.MapCollection(Count=1) | 90.9 ns | 82.8 ns | 0.91x | 240 | 152 |  |
| CollectionMappingBenchmarks.MapCollection(Count=100) | 6.73 us | 5.02 us | 0.75x | 15288 | 6488 | faster |
| MapperBenchmarks.MapFlat | 91.8 ns | 47.7 ns | 0.52x | 144 | 56 | faster |
| MapperBenchmarks.MapFlat_AutoMapper | 48.2 ns | 54.3 ns | 1.13x | 56 | 56 | REGRESSION |
| MapperBenchmarks.MapNested | 961.8 ns | 555.2 ns | 0.58x | 2288 | 952 | faster |
| MapperBenchmarks.MapNested_AutoMapper | 202.9 ns | 193.8 ns | 0.96x | 952 | 952 |  |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=ContextCallback) | 53.0 ns | | | 0 | | removed |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=CustomProfile) | 55.3 ns | | | 0 | | removed |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=OverriddenProfile) | 53.2 ns | | | 0 | | removed |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=TypedCallback) | 52.9 ns | | | 0 | | removed |
| MappingCallbackBenchmarks.MapCallback(Mode=ContextCallback) | 26.0 ns | | | 88 | | removed |
| MappingCallbackBenchmarks.MapCallback(Mode=CustomProfile) | 41.3 ns | | | 80 | | removed |
| MappingCallbackBenchmarks.MapCallback(Mode=OverriddenProfile) | 39.4 ns | | | 80 | | removed |
| MappingCallbackBenchmarks.MapCallback(Mode=TypedCallback) | 28.7 ns | | | 88 | | removed |
| MappingConfigurationBenchmarks.Configure_AutoMapper(Graph=False) | 83.99 us | | | 38762 | | removed |
| MappingConfigurationBenchmarks.Configure_AutoMapper(Graph=True) | 174.33 us | | | 81800 | | removed |
| MappingConfigurationBenchmarks.Configure(Graph=False) | 95.8 ns | | | 464 | | removed |
| MappingConfigurationBenchmarks.Configure(Graph=True) | 282.8 ns | | | 1224 | | removed |
| MappingConfigurationBenchmarks.ConfigureAndMap_AutoMapper(Graph=False) | 267.77 us | | | 68558 | | removed |
| MappingConfigurationBenchmarks.ConfigureAndMap_AutoMapper(Graph=True) | 1.79 ms | | | 234275 | | removed |
| MappingConfigurationBenchmarks.ConfigureAndMap(Graph=False) | 166.4 ns | | | 608 | | removed |
| MappingConfigurationBenchmarks.ConfigureAndMap(Graph=True) | 1.89 us | | | 3912 | | removed |
| MappingDispatchBenchmarks.MapExisting | 58.0 ns | 20.6 ns | 0.36x | 88 | 0 | faster |
| MappingDispatchBenchmarks.MapExisting_AutoMapper | 52.8 ns | 47.7 ns | 0.90x | 0 | 0 | faster |
| MappingDispatchBenchmarks.MapRuntime | 77.1 ns | 33.7 ns | 0.44x | 144 | 56 | faster |
| MappingDispatchBenchmarks.MapRuntime_AutoMapper | 58.5 ns | 49.8 ns | 0.85x | 56 | 56 | faster |
| NestedMappingBenchmarks.MapGraph_AutoMapper(Items=1) | 95.0 ns | 81.6 ns | 0.86x | 280 | 280 | faster |
| NestedMappingBenchmarks.MapGraph_AutoMapper(Items=100) | 1.13 us | 970.2 ns | 0.86x | 7136 | 7136 | faster |
| NestedMappingBenchmarks.MapGraph(Items=1) | 410.0 ns | 226.2 ns | 0.55x | 752 | 376 | faster |
| NestedMappingBenchmarks.MapGraph(Items=100) | 7.59 us | 4.08 us | 0.54x | 17112 | 6712 | faster |

2 regression(s): time ratio over 1.1 or allocations over 1 x.
