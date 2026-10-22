| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| RoutePathBenchmarks.Match(Shape=catchall) | 116.6 ns | 111.2 ns | 0.95x | 528 | 480 |  |
| RoutePathBenchmarks.Match(Shape=literal) | 59.2 ns | 58.9 ns | 0.99x | 192 | 192 |  |
| RoutePathBenchmarks.Match(Shape=parameter) | 111.4 ns | 118.7 ns | 1.07x | 384 | 384 |  |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
