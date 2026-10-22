| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| MemoryCacheAccessBenchmarks.SetSized | 131.8 ns | 130.1 ns | 0.99x | 400 | 304 |  |
| MemoryCacheAccessBenchmarks.SetSized_MicrosoftExtensions | 71.5 ns | 71.2 ns | 1.00x | 104 | 104 |  |
| MemoryCacheAccessBenchmarks.SetWithoutExpiration | 109.0 ns | 105.8 ns | 0.97x | 400 | 304 |  |
| MemoryCacheAccessBenchmarks.SetWithoutExpiration_MicrosoftExtensions | 70.7 ns | 72.7 ns | 1.03x | 104 | 104 |  |
| MemoryCacheBenchmarks.Set | 129.1 ns | 126.5 ns | 0.98x | 400 | 304 |  |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
