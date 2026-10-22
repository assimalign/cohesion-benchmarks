| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| KeyBenchmarks.EqualNullable | 5.1 ns | 4.5 ns | 0.89x | 0 | 0 | faster |
| KeyBenchmarks.FromSpan | 60.9 ns | 9.4 ns | 0.15x | 312 | 56 | faster |
| KeyBenchmarks.FromString | 59.7 ns | 2.4 ns | 0.04x | 312 | 0 | faster |
| PathBenchmarks.Parse(value=/Serv(...)\0:Url [23]) | 196.9 ns | 63.1 ns | 0.32x | 1000 | 232 | faster |
| PathBenchmarks.Parse(value=Servi(...)quest [24]) | 188.6 ns | 68.2 ns | 0.36x | 1000 | 232 | faster |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
