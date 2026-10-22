| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| HttpHeaderParserBenchmarks.ParseContentType | 218.3 ns | 234.2 ns | 1.07x | 432 | 432 |  |
| HttpHeaderParserBenchmarks.ParseContentType_AspNetCore | 127.2 ns | 122.2 ns | 0.96x | 280 | 280 |  |
| HttpHeaderParserBenchmarks.ParseContentType_SystemNetHttp | 114.3 ns | 114.1 ns | 1.00x | 424 | 424 |  |
| HttpHeaderParserBenchmarks.ParseRange | 87.9 ns | 92.0 ns | 1.05x | 416 | 416 |  |
| HttpHeaderParserBenchmarks.ParseRange_AspNetCore | 182.7 ns | 162.1 ns | 0.89x | 304 | 304 | faster |
| HttpHeaderParserBenchmarks.ParseRange_SystemNetHttp | 104.6 ns | 99.7 ns | 0.95x | 248 | 248 |  |
| HttpMediaTypeBenchmarks.Format_AspNetCore(Scenario=Charset) | 51.7 ns | 45.6 ns | 0.88x | 280 | 280 | faster |
| HttpMediaTypeBenchmarks.Format_AspNetCore(Scenario=Escaped) | 79.4 ns | 74.5 ns | 0.94x | 472 | 472 |  |
| HttpMediaTypeBenchmarks.Format_AspNetCore(Scenario=Multiple) | 78.3 ns | 74.0 ns | 0.94x | 488 | 488 |  |
| HttpMediaTypeBenchmarks.Format_AspNetCore(Scenario=Plain) | 17.2 ns | 16.8 ns | 0.98x | 160 | 160 |  |
| HttpMediaTypeBenchmarks.Format(Scenario=Charset) | 68.9 ns | 55.0 ns | 0.80x | 376 | 376 | faster |
| HttpMediaTypeBenchmarks.Format(Scenario=Escaped) | 133.6 ns | 119.2 ns | 0.89x | 584 | 584 | faster |
| HttpMediaTypeBenchmarks.Format(Scenario=Multiple) | 86.7 ns | 70.6 ns | 0.81x | 520 | 520 | faster |
| HttpMediaTypeBenchmarks.Format(Scenario=Plain) | 19.7 ns | 9.9 ns | 0.50x | 160 | 56 | faster |
| HttpMediaTypeBenchmarks.Parse_AspNetCore(Scenario=Charset) | 85.9 ns | 90.5 ns | 1.05x | 224 | 224 |  |
| HttpMediaTypeBenchmarks.Parse_AspNetCore(Scenario=Escaped) | 195.4 ns | 190.5 ns | 0.98x | 336 | 336 |  |
| HttpMediaTypeBenchmarks.Parse_AspNetCore(Scenario=Multiple) | 120.7 ns | 125.1 ns | 1.04x | 280 | 280 |  |
| HttpMediaTypeBenchmarks.Parse_AspNetCore(Scenario=Plain) | 36.0 ns | 42.8 ns | 1.19x | 48 | 48 | REGRESSION |
| HttpMediaTypeBenchmarks.Parse(Scenario=Charset) | 143.1 ns | 113.4 ns | 0.79x | 296 | 176 | faster |
| HttpMediaTypeBenchmarks.Parse(Scenario=Escaped) | 240.5 ns | 252.2 ns | 1.05x | 424 | 424 |  |
| HttpMediaTypeBenchmarks.Parse(Scenario=Multiple) | 212.1 ns | 202.7 ns | 0.96x | 424 | 424 |  |
| HttpMediaTypeBenchmarks.Parse(Scenario=Plain) | 66.1 ns | 64.5 ns | 0.98x | 80 | 80 |  |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=1&Reversed=False) | 11.5 ns | 17.2 ns | 1.49x | 0 | 0 | REGRESSION |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=1&Reversed=True) | 10.5 ns | 17.9 ns | 1.70x | 0 | 0 | REGRESSION |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=2&Reversed=False) | 19.3 ns | 26.6 ns | 1.38x | 0 | 0 | REGRESSION |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=2&Reversed=True) | 17.4 ns | 33.1 ns | 1.91x | 0 | 0 | REGRESSION |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=65&Reversed=False) | 5.43 us | 1.43 us | 0.26x | 0 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=65&Reversed=True) | 5.45 us | 7.97 us | 1.46x | 0 | 0 | REGRESSION |

6 regression(s): time ratio over 1.1 or allocations over 1 x.
