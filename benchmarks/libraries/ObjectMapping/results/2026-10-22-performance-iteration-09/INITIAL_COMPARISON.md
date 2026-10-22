| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| CollectionMappingBenchmarks.MapCollection_AutoMapper(Count=1) | 67.7 ns | 58.4 ns | 0.86x | 144 | 144 | faster |
| CollectionMappingBenchmarks.MapCollection_AutoMapper(Count=100) | 1.04 us | 906.5 ns | 0.87x | 7792 | 7792 | faster |
| CollectionMappingBenchmarks.MapCollection(Count=1) | 97.2 ns | 57.7 ns | 0.59x | 240 | 152 | faster |
| CollectionMappingBenchmarks.MapCollection(Count=100) | 6.88 us | 3.86 us | 0.56x | 15288 | 6488 | faster |
| MapperBenchmarks.MapFlat | 99.4 ns | 52.0 ns | 0.52x | 144 | 56 | faster |
| MapperBenchmarks.MapFlat_AutoMapper | 53.6 ns | 45.7 ns | 0.85x | 56 | 56 | faster |
| MapperBenchmarks.MapNested | 1.04 us | 613.7 ns | 0.59x | 2288 | 1040 | faster |
| MapperBenchmarks.MapNested_AutoMapper | 225.8 ns | 189.1 ns | 0.84x | 952 | 952 | faster |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=ContextCallback) | | 43.8 ns | | | 0 | new |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=CustomProfile) | | 53.4 ns | | | 0 | new |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=OverriddenProfile) | | 56.0 ns | | | 0 | new |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=TypedCallback) | | 53.1 ns | | | 0 | new |
| MappingCallbackBenchmarks.MapCallback(Mode=ContextCallback) | | 18.0 ns | | | 48 | new |
| MappingCallbackBenchmarks.MapCallback(Mode=CustomProfile) | | 37.7 ns | | | 80 | new |
| MappingCallbackBenchmarks.MapCallback(Mode=OverriddenProfile) | | 40.6 ns | | | 80 | new |
| MappingCallbackBenchmarks.MapCallback(Mode=TypedCallback) | | 25.3 ns | | | 48 | new |
| MappingConfigurationBenchmarks.Configure_AutoMapper(Graph=False) | 84.91 us | 83.79 us | 0.99x | 38762 | 38682 |  |
| MappingConfigurationBenchmarks.Configure_AutoMapper(Graph=True) | 172.06 us | 174.46 us | 1.01x | 82068 | 81512 |  |
| MappingConfigurationBenchmarks.Configure(Graph=False) | 99.6 ns | 92.0 ns | 0.92x | 464 | 464 |  |
| MappingConfigurationBenchmarks.Configure(Graph=True) | 280.5 ns | 268.8 ns | 0.96x | 1224 | 1224 |  |
| MappingConfigurationBenchmarks.ConfigureAndMap_AutoMapper(Graph=False) | 262.31 us | 256.84 us | 0.98x | 68552 | 68481 |  |
| MappingConfigurationBenchmarks.ConfigureAndMap_AutoMapper(Graph=True) | 1.76 ms | 1.41 ms | 0.80x | 234814 | 234250 | faster |
| MappingConfigurationBenchmarks.ConfigureAndMap(Graph=False) | 154.9 ns | 129.6 ns | 0.84x | 608 | 520 | faster |
| MappingConfigurationBenchmarks.ConfigureAndMap(Graph=True) | 1.73 us | 1.17 us | 0.68x | 3912 | 2400 | faster |
| MappingDispatchBenchmarks.MapExisting | 62.0 ns | 24.5 ns | 0.40x | 88 | 0 | faster |
| MappingDispatchBenchmarks.MapExisting_AutoMapper | 53.7 ns | 44.2 ns | 0.82x | 0 | 0 | faster |
| MappingDispatchBenchmarks.MapRuntime | 67.2 ns | 36.9 ns | 0.55x | 144 | 56 | faster |
| MappingDispatchBenchmarks.MapRuntime_AutoMapper | 53.2 ns | 45.8 ns | 0.86x | 56 | 56 | faster |
| NestedMappingBenchmarks.MapGraph_AutoMapper(Items=1) | 90.2 ns | 81.9 ns | 0.91x | 280 | 280 | faster |
| NestedMappingBenchmarks.MapGraph_AutoMapper(Items=100) | 1.06 us | 1.01 us | 0.95x | 7136 | 7136 |  |
| NestedMappingBenchmarks.MapGraph(Items=1) | 399.6 ns | 277.7 ns | 0.70x | 752 | 464 | faster |
| NestedMappingBenchmarks.MapGraph(Items=100) | 7.60 us | 4.30 us | 0.57x | 17112 | 6800 | faster |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
