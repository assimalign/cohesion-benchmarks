| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| TimeoutCompletionBenchmarks.ExecuteAsyncValue | 3.87 us | 2.39 us | 0.62x | 1557 | 1469 | faster |
| TimeoutCompletionBenchmarks.ExecuteVoid | 969.9 ns | 244.5 ns | 0.25x | 552 | 216 | faster |
| TimeoutPipelineBenchmarks.Execute | 1.17 us | 201.1 ns | 0.17x | 560 | 240 | faster |
| TimeoutPipelineBenchmarks.Execute_Polly | 179.8 ns | 166.4 ns | 0.93x | 0 | 0 |  |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
