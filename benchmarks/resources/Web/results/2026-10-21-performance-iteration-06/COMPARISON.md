| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| ContentNegotiationBenchmarks.Negotiate(Registrations=1&Scenario=Exact) | 161.8 ns | 157.8 ns | 0.98x | 568 | 504 |  |
| ContentNegotiationBenchmarks.Negotiate(Registrations=1&Scenario=None) | 39.0 ns | 30.3 ns | 0.78x | 216 | 152 | faster |
| ContentNegotiationBenchmarks.Negotiate(Registrations=1&Scenario=Rejected) | 113.3 ns | 111.1 ns | 0.98x | 536 | 472 |  |
| ContentNegotiationBenchmarks.Negotiate(Registrations=1&Scenario=Suffix) | 193.0 ns | 175.3 ns | 0.91x | 656 | 536 | faster |
| ContentNegotiationBenchmarks.Negotiate(Registrations=8&Scenario=Exact) | 552.6 ns | 553.1 ns | 1.00x | 1232 | 944 |  |
| ContentNegotiationBenchmarks.Negotiate(Registrations=8&Scenario=None) | 351.1 ns | 309.3 ns | 0.88x | 656 | 368 | faster |
| ContentNegotiationBenchmarks.Negotiate(Registrations=8&Scenario=Rejected) | 514.0 ns | 466.5 ns | 0.91x | 1200 | 912 | faster |
| ContentNegotiationBenchmarks.Negotiate(Registrations=8&Scenario=Suffix) | 700.4 ns | 619.9 ns | 0.89x | 1320 | 976 | faster |
| ContentRegistryBenchmarks.Reader(Registrations=1) | 12.7 ns | 7.1 ns | 0.56x | 32 | 0 | faster |
| ContentRegistryBenchmarks.Reader(Registrations=8) | 109.0 ns | 58.2 ns | 0.53x | 256 | 0 | faster |
| ContentRegistryBenchmarks.Writer(Registrations=1) | 12.5 ns | 7.1 ns | 0.57x | 32 | 0 | faster |
| ContentRegistryBenchmarks.Writer(Registrations=8) | 98.3 ns | 55.8 ns | 0.57x | 256 | 0 | faster |
| JsonContentBenchmarks.Read_AspNetCore(TextLength=32) | 668.3 ns | 684.0 ns | 1.02x | 544 | 544 |  |
| JsonContentBenchmarks.Read_AspNetCore(TextLength=4096) | 1.97 us | 1.92 us | 0.97x | 12796 | 12796 |  |
| JsonContentBenchmarks.Read(TextLength=32) | 432.3 ns | 410.7 ns | 0.95x | 312 | 280 |  |
| JsonContentBenchmarks.Read(TextLength=4096) | 1.13 us | 1.08 us | 0.96x | 8440 | 8408 |  |
| JsonContentBenchmarks.Write_AspNetCore(TextLength=32) | 157.5 ns | 159.0 ns | 1.01x | 72 | 72 |  |
| JsonContentBenchmarks.Write_AspNetCore(TextLength=4096) | 565.7 ns | 574.9 ns | 1.02x | 72 | 72 |  |
| JsonContentBenchmarks.Write(TextLength=32) | 331.2 ns | 323.5 ns | 0.98x | 648 | 616 |  |
| JsonContentBenchmarks.Write(TextLength=4096) | 678.9 ns | 745.3 ns | 1.10x | 648 | 616 |  |
| NegotiatedWriteBenchmarks.Write(Vary=) | 191.0 ns | 183.2 ns | 0.96x | 600 | 504 |  |
| NegotiatedWriteBenchmarks.Write(Vary=Accept) | 211.4 ns | 204.1 ns | 0.97x | 632 | 504 |  |
| NegotiatedWriteBenchmarks.Write(Vary=Origi(...)CCEPT [31]) | 253.4 ns | 236.1 ns | 0.93x | 880 | 504 |  |
| NegotiatedWriteBenchmarks.Write(Vary=Origi(...)oding [23]) | 249.0 ns | 255.2 ns | 1.02x | 880 | 592 |  |
| NegotiatedWriteBenchmarks.Write(Vary=Origin, *) | 229.9 ns | 209.8 ns | 0.91x | 736 | 504 |  |
| NegotiatedWriteBenchmarks.Write(Vary=Origin) | 218.9 ns | 213.4 ns | 0.97x | 688 | 560 |  |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
