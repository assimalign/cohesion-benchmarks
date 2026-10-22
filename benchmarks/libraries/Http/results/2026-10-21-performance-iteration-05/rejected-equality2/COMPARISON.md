| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=1&Reversed=False) | 11.9 ns | 10.5 ns | 0.88x | 0 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=1&Reversed=True) | 12.0 ns | 10.2 ns | 0.85x | 0 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=2&Reversed=False) | 20.8 ns | 15.7 ns | 0.75x | 0 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=2&Reversed=True) | 20.1 ns | 22.3 ns | 1.11x | 0 | 0 | REGRESSION |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=65&Reversed=False) | 6.10 us | 7.65 us | 1.25x | 0 | 0 | REGRESSION |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=65&Reversed=True) | 6.13 us | 7.81 us | 1.27x | 0 | 0 | REGRESSION |

3 regression(s): time ratio over 1.1 or allocations over 1 x.
