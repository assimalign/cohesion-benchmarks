| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| MappingConfigurationBenchmarks.Configure_AutoMapper(Graph=False) | 100.28 us | 110.71 us | 1.10x | 38739 | 38699 | REGRESSION |
| MappingConfigurationBenchmarks.Configure_AutoMapper(Graph=True) | 148.01 us | 172.74 us | 1.17x | 81752 | 81840 | REGRESSION |
| MappingConfigurationBenchmarks.Configure(Graph=False) | 124.5 ns | 114.3 ns | 0.92x | 464 | 464 |  |
| MappingConfigurationBenchmarks.Configure(Graph=True) | 326.9 ns | 363.9 ns | 1.11x | 1224 | 1224 | REGRESSION |

3 regression(s): time ratio over 1.1 or allocations over 1 x.
