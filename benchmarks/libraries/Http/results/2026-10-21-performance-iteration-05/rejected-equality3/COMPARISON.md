| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=1&Reversed=False) | 13.9 ns | 11.7 ns | 0.84x | 0 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=1&Reversed=True) | 13.3 ns | 11.1 ns | 0.84x | 0 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=2&Reversed=False) | 21.2 ns | 17.5 ns | 0.82x | 0 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=2&Reversed=True) | 20.6 ns | 20.3 ns | 0.99x | 0 | 0 |  |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=4097&Reversed=False) | 19.61 ms | 22.90 us | 0.00x | 32 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=4097&Reversed=True) | 19.34 ms | 27.30 ms | 1.41x | 82 | 626 | REGRESSION |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=65&Reversed=False) | 6.97 us | 308.3 ns | 0.04x | 0 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=65&Reversed=True) | 6.83 us | 8.11 us | 1.19x | 0 | 0 | REGRESSION |

2 regression(s): time ratio over 1.1 or allocations over 1 x.
