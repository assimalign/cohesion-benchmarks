| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| GlobBenchmarks.IsMatch(Pattern=p?th/(...)].txt [21]) | 6.66 us | 2.47 us | 0.37x | 0 | 0 | faster |
| GlobBenchmarks.IsMatch(Pattern=src/**/*.{cs,json}) | 6.49 us | 5.92 us | 0.91x | 0 | 0 |  |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
