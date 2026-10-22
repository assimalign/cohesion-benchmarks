| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| HttpAcceptOrderingBenchmarks.ParseMedia(Entries=1) | 176.1 ns | 133.3 ns | 0.76x | 544 | 320 | faster |
| HttpAcceptOrderingBenchmarks.ParseMedia(Entries=16) | 2.43 us | 2.40 us | 0.99x | 4064 | 4064 |  |
| HttpAcceptOrderingBenchmarks.ParseMedia(Entries=17) | 2.65 us | 2.55 us | 0.96x | 5288 | 5288 |  |
| HttpAcceptOrderingBenchmarks.ParseMedia(Entries=64) | 9.59 us | 9.76 us | 1.02x | 15056 | 15056 |  |
| HttpAcceptOrderingBenchmarks.ParseMedia(Entries=8) | 1.39 us | 1.20 us | 0.87x | 2232 | 1384 | faster |
| HttpAcceptOrderingBenchmarks.ParseMedia(Entries=9) | 1.40 us | 1.41 us | 1.01x | 2936 | 2936 |  |
| HttpAcceptOrderingBenchmarks.ParseTokens(Entries=1) | 105.1 ns | 64.3 ns | 0.61x | 392 | 200 | faster |
| HttpAcceptOrderingBenchmarks.ParseTokens(Entries=16) | 1.40 us | 1.05 us | 0.75x | 2336 | 1472 | faster |
| HttpAcceptOrderingBenchmarks.ParseTokens(Entries=17) | 1.19 us | 1.18 us | 0.99x | 2968 | 2968 |  |
| HttpAcceptOrderingBenchmarks.ParseTokens(Entries=64) | 4.63 us | 4.77 us | 1.03x | 7808 | 7808 |  |
| HttpAcceptOrderingBenchmarks.ParseTokens(Entries=8) | 640.8 ns | 484.0 ns | 0.76x | 1408 | 744 | faster |
| HttpAcceptOrderingBenchmarks.ParseTokens(Entries=9) | 706.2 ns | 589.3 ns | 0.83x | 1784 | 1080 | faster |
| HttpAcceptParserBenchmarks.ParseAccept | 1.11 us | 903.3 ns | 0.81x | 2384 | 1536 | faster |
| HttpAcceptParserBenchmarks.ParseAccept_AspNetCore | 998.9 ns | 996.9 ns | 1.00x | 1672 | 1672 |  |
| HttpHeaderParserBenchmarks.ParseContentType | 236.9 ns | 210.7 ns | 0.89x | 432 | 432 | faster |
| HttpHeaderParserBenchmarks.ParseContentType_AspNetCore | 130.9 ns | 134.2 ns | 1.03x | 280 | 280 |  |
| HttpHeaderParserBenchmarks.ParseContentType_SystemNetHttp | 123.8 ns | 121.1 ns | 0.98x | 424 | 424 |  |
| HttpHeaderParserBenchmarks.ParseRange | 111.9 ns | 90.6 ns | 0.81x | 416 | 416 | faster |
| HttpHeaderParserBenchmarks.ParseRange_AspNetCore | 172.4 ns | 169.4 ns | 0.98x | 304 | 304 |  |
| HttpHeaderParserBenchmarks.ParseRange_SystemNetHttp | 110.9 ns | 105.7 ns | 0.95x | 248 | 248 |  |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
