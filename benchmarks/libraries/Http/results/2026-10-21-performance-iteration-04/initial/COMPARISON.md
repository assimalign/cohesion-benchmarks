| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| HttpAcceptOrderingBenchmarks.ParseMedia(Entries=1) | 167.1 ns | 139.7 ns | 0.84x | 544 | 320 | faster |
| HttpAcceptOrderingBenchmarks.ParseMedia(Entries=16) | 2.45 us | 2.95 us | 1.20x | 4064 | 2864 | REGRESSION |
| HttpAcceptOrderingBenchmarks.ParseMedia(Entries=17) | 2.42 us | 2.61 us | 1.08x | 5288 | 5288 |  |
| HttpAcceptOrderingBenchmarks.ParseMedia(Entries=64) | 9.74 us | 10.19 us | 1.05x | 15056 | 15056 |  |
| HttpAcceptOrderingBenchmarks.ParseMedia(Entries=8) | 1.26 us | 1.25 us | 0.99x | 2232 | 1384 |  |
| HttpAcceptOrderingBenchmarks.ParseTokens(Entries=1) | 94.4 ns | 67.3 ns | 0.71x | 392 | 200 | faster |
| HttpAcceptOrderingBenchmarks.ParseTokens(Entries=16) | 1.36 us | 1.06 us | 0.78x | 2336 | 1472 | faster |
| HttpAcceptOrderingBenchmarks.ParseTokens(Entries=17) | 1.17 us | 1.15 us | 0.98x | 2968 | 2968 |  |
| HttpAcceptOrderingBenchmarks.ParseTokens(Entries=64) | 4.50 us | 4.52 us | 1.00x | 7808 | 7808 |  |
| HttpAcceptOrderingBenchmarks.ParseTokens(Entries=8) | 633.2 ns | 491.0 ns | 0.78x | 1408 | 744 | faster |
| HttpAcceptParserBenchmarks.ParseAccept | 1.01 us | 921.1 ns | 0.91x | 2384 | 1536 |  |
| HttpAcceptParserBenchmarks.ParseAccept_AspNetCore | 959.4 ns | 1.10 us | 1.14x | 1672 | 1672 | REGRESSION |
| HttpHeaderParserBenchmarks.ParseContentType | 197.4 ns | 228.7 ns | 1.16x | 432 | 432 | REGRESSION |
| HttpHeaderParserBenchmarks.ParseContentType_AspNetCore | 118.1 ns | 127.2 ns | 1.08x | 280 | 280 |  |
| HttpHeaderParserBenchmarks.ParseContentType_SystemNetHttp | 123.2 ns | 119.7 ns | 0.97x | 424 | 424 |  |
| HttpHeaderParserBenchmarks.ParseRange | 95.9 ns | 92.6 ns | 0.97x | 416 | 416 |  |
| HttpHeaderParserBenchmarks.ParseRange_AspNetCore | 161.3 ns | 159.3 ns | 0.99x | 304 | 304 |  |
| HttpHeaderParserBenchmarks.ParseRange_SystemNetHttp | 106.1 ns | 106.8 ns | 1.01x | 248 | 248 |  |

3 regression(s): time ratio over 1.1 or allocations over 1 x.
