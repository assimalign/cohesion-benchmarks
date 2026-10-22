| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| ContentNegotiationBenchmarks.Negotiate(Registrations=8&Scenario=Exact) | 498.5 ns | 457.3 ns | 0.92x | 1232 | 944 |  |
| JsonContentBenchmarks.Write_AspNetCore(TextLength=32) | 157.8 ns | 156.2 ns | 0.99x | 72 | 72 |  |
| JsonContentBenchmarks.Write_AspNetCore(TextLength=4096) | 546.5 ns | 540.7 ns | 0.99x | 72 | 72 |  |
| JsonContentBenchmarks.Write(TextLength=32) | 326.8 ns | 309.8 ns | 0.95x | 648 | 616 |  |
| JsonContentBenchmarks.Write(TextLength=4096) | 676.8 ns | 672.6 ns | 0.99x | 648 | 616 |  |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
