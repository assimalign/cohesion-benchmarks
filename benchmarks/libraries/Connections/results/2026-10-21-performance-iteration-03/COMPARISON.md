| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| ConnectionFeatureLookupBenchmarks.NamedHit(Layers=1) | 4.9 ns | 4.0 ns | 0.81x | 0 | 0 | faster |
| ConnectionFeatureLookupBenchmarks.NamedHit(Layers=4) | 12.9 ns | 11.9 ns | 0.93x | 0 | 0 |  |
| ConnectionFeatureLookupBenchmarks.NamedMiss(Layers=1) | 5.1 ns | 4.6 ns | 0.89x | 0 | 0 | faster |
| ConnectionFeatureLookupBenchmarks.NamedMiss(Layers=4) | 5.1 ns | 4.6 ns | 0.89x | 0 | 0 | faster |
| ConnectionFeatureLookupBenchmarks.TypedHit(Layers=1) | 13.5 ns | 13.5 ns | 1.00x | 0 | 0 |  |
| ConnectionFeatureLookupBenchmarks.TypedHit(Layers=4) | 46.5 ns | 48.2 ns | 1.04x | 0 | 0 |  |
| ConnectionFeatureLookupBenchmarks.TypedMiss(Layers=1) | 23.2 ns | 23.2 ns | 1.00x | 0 | 0 |  |
| ConnectionFeatureLookupBenchmarks.TypedMiss(Layers=4) | 48.1 ns | 49.7 ns | 1.03x | 0 | 0 |  |
| ConnectionNamedMissBenchmarks.NamedAbsent(Layers=0) | 1.3 ns | 1.3 ns | 0.97x | 0 | 0 |  |
| ConnectionNamedMissBenchmarks.NamedAbsent(Layers=1) | 4.2 ns | 3.7 ns | 0.89x | 0 | 0 | faster |
| ConnectionNamedMissBenchmarks.NamedAbsent(Layers=16) | 39.3 ns | 28.7 ns | 0.73x | 0 | 0 | faster |
| ConnectionNamedMissBenchmarks.NamedAbsent(Layers=4) | 12.2 ns | 10.1 ns | 0.83x | 0 | 0 | faster |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
