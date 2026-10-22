| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| ConnectionFeatureLookupBenchmarks.NamedHit(Layers=1) | 5.0 ns | 3.9 ns | 0.77x | 0 | 0 | faster |
| ConnectionFeatureLookupBenchmarks.NamedHit(Layers=4) | 12.9 ns | 12.1 ns | 0.93x | 0 | 0 |  |
| ConnectionFeatureLookupBenchmarks.NamedMiss(Layers=1) | 4.4 ns | 4.6 ns | 1.06x | 0 | 0 |  |
| ConnectionFeatureLookupBenchmarks.NamedMiss(Layers=4) | 4.3 ns | 4.6 ns | 1.06x | 0 | 0 |  |
| ConnectionFeatureLookupBenchmarks.TypedHit(Layers=1) | 13.8 ns | 13.6 ns | 0.99x | 0 | 0 |  |
| ConnectionFeatureLookupBenchmarks.TypedHit(Layers=4) | 48.3 ns | 47.5 ns | 0.98x | 0 | 0 |  |
| ConnectionFeatureLookupBenchmarks.TypedMiss(Layers=1) | 23.2 ns | 25.5 ns | 1.10x | 0 | 0 |  |
| ConnectionFeatureLookupBenchmarks.TypedMiss(Layers=4) | 49.6 ns | 48.9 ns | 0.99x | 0 | 0 |  |
| ConnectionNamedMissBenchmarks.NamedAbsent(Layers=0) | 1.7 ns | 1.3 ns | 0.78x | 0 | 0 | faster |
| ConnectionNamedMissBenchmarks.NamedAbsent(Layers=1) | 4.2 ns | 3.8 ns | 0.90x | 0 | 0 | faster |
| ConnectionNamedMissBenchmarks.NamedAbsent(Layers=16) | 41.0 ns | 30.6 ns | 0.75x | 0 | 0 | faster |
| ConnectionNamedMissBenchmarks.NamedAbsent(Layers=4) | 14.2 ns | 10.4 ns | 0.73x | 0 | 0 | faster |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
