| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| CollectionMappingBenchmarks.MapCollection_AutoMapper(Count=1) | 425.4 ns | 320.4 ns | 0.75x | 144 | 144 | faster |
| CollectionMappingBenchmarks.MapCollection_AutoMapper(Count=100) | 1.62 us | 1.23 us | 0.76x | 7792 | 7792 | faster |
| CollectionMappingBenchmarks.MapCollection(Count=1) | 772.4 ns | 458.0 ns | 0.59x | 240 | 152 | faster |
| CollectionMappingBenchmarks.MapCollection(Count=100) | 91.53 us | 21.02 us | 0.23x | 15288 | 6488 | faster |
| MapperBenchmarks.MapFlat | 141.1 ns | 85.1 ns | 0.60x | 144 | 56 | faster |
| MapperBenchmarks.MapFlat_AutoMapper | 394.3 ns | 290.7 ns | 0.74x | 56 | 56 | faster |
| MapperBenchmarks.MapNested | 4.78 us | 2.91 us | 0.61x | 2288 | 952 | faster |
| MapperBenchmarks.MapNested_AutoMapper | 255.5 ns | 215.5 ns | 0.84x | 952 | 952 | faster |
| MappingDispatchBenchmarks.MapExisting | 66.5 ns | 32.2 ns | 0.48x | 88 | 0 | faster |
| MappingDispatchBenchmarks.MapExisting_AutoMapper | 56.5 ns | 57.5 ns | 1.02x | 0 | 0 |  |
| MappingDispatchBenchmarks.MapRuntime | 74.3 ns | 126.4 ns | 1.70x | 144 | 56 | REGRESSION |
| MappingDispatchBenchmarks.MapRuntime_AutoMapper | 58.6 ns | 63.4 ns | 1.08x | 56 | 56 |  |
| NestedMappingBenchmarks.MapGraph_AutoMapper(Items=1) | 98.2 ns | 97.1 ns | 0.99x | 280 | 280 |  |
| NestedMappingBenchmarks.MapGraph_AutoMapper(Items=100) | 992.4 ns | 993.6 ns | 1.00x | 7136 | 7136 |  |
| NestedMappingBenchmarks.MapGraph(Items=1) | 1.44 us | 740.4 ns | 0.51x | 752 | 376 | faster |
| NestedMappingBenchmarks.MapGraph(Items=100) | 6.97 us | 4.75 us | 0.68x | 17112 | 6712 | faster |

1 regression(s): time ratio over 1.1 or allocations over 1 x.
