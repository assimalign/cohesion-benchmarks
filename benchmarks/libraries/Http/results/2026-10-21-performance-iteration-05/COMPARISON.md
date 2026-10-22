| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| HttpHeaderParserBenchmarks.ParseContentType | 203.3 ns | 191.7 ns | 0.94x | 432 | 432 |  |
| HttpHeaderParserBenchmarks.ParseContentType_AspNetCore | 116.9 ns | 116.8 ns | 1.00x | 280 | 280 |  |
| HttpHeaderParserBenchmarks.ParseContentType_SystemNetHttp | 115.9 ns | 107.3 ns | 0.93x | 424 | 424 |  |
| HttpHeaderParserBenchmarks.ParseRange | 89.7 ns | 87.3 ns | 0.97x | 416 | 416 |  |
| HttpHeaderParserBenchmarks.ParseRange_AspNetCore | 162.3 ns | 157.4 ns | 0.97x | 304 | 304 |  |
| HttpHeaderParserBenchmarks.ParseRange_SystemNetHttp | 95.5 ns | 95.2 ns | 1.00x | 248 | 248 |  |
| HttpMediaTypeBenchmarks.Format_AspNetCore(Scenario=Charset) | 41.1 ns | 45.8 ns | 1.11x | 280 | 280 | REGRESSION |
| HttpMediaTypeBenchmarks.Format_AspNetCore(Scenario=Escaped) | 74.0 ns | 83.7 ns | 1.13x | 472 | 472 | REGRESSION |
| HttpMediaTypeBenchmarks.Format_AspNetCore(Scenario=Multiple) | 71.1 ns | 74.6 ns | 1.05x | 488 | 488 |  |
| HttpMediaTypeBenchmarks.Format_AspNetCore(Scenario=Plain) | 17.2 ns | 18.1 ns | 1.05x | 160 | 160 |  |
| HttpMediaTypeBenchmarks.Format(Scenario=Charset) | 61.8 ns | 59.2 ns | 0.96x | 376 | 376 |  |
| HttpMediaTypeBenchmarks.Format(Scenario=Escaped) | 119.5 ns | 119.7 ns | 1.00x | 584 | 584 |  |
| HttpMediaTypeBenchmarks.Format(Scenario=Multiple) | 75.6 ns | 72.9 ns | 0.96x | 520 | 520 |  |
| HttpMediaTypeBenchmarks.Format(Scenario=Plain) | 18.2 ns | 9.7 ns | 0.53x | 160 | 56 | faster |
| HttpMediaTypeBenchmarks.Parse_AspNetCore(Scenario=Charset) | 80.3 ns | 82.2 ns | 1.02x | 224 | 224 |  |
| HttpMediaTypeBenchmarks.Parse_AspNetCore(Scenario=Escaped) | 179.4 ns | 175.1 ns | 0.98x | 336 | 336 |  |
| HttpMediaTypeBenchmarks.Parse_AspNetCore(Scenario=Multiple) | 119.9 ns | 114.5 ns | 0.96x | 280 | 280 |  |
| HttpMediaTypeBenchmarks.Parse_AspNetCore(Scenario=Plain) | 37.4 ns | 35.1 ns | 0.94x | 48 | 48 |  |
| HttpMediaTypeBenchmarks.Parse(Scenario=Charset) | 121.2 ns | 108.3 ns | 0.89x | 296 | 176 | faster |
| HttpMediaTypeBenchmarks.Parse(Scenario=Escaped) | 239.4 ns | 226.2 ns | 0.95x | 424 | 424 |  |
| HttpMediaTypeBenchmarks.Parse(Scenario=Multiple) | 203.4 ns | 208.7 ns | 1.03x | 424 | 424 |  |
| HttpMediaTypeBenchmarks.Parse(Scenario=Plain) | 63.2 ns | 64.4 ns | 1.02x | 80 | 80 |  |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=1&Reversed=False) | 13.7 ns | 10.6 ns | 0.78x | 0 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=1&Reversed=True) | 12.3 ns | 10.7 ns | 0.87x | 0 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=2&Reversed=False) | 20.5 ns | 18.5 ns | 0.90x | 0 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=2&Reversed=True) | 21.1 ns | 22.1 ns | 1.05x | 0 | 0 |  |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=4097&Reversed=False) | 19.10 ms | 28.20 us | 0.00x | 74 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=4097&Reversed=True) | 18.86 ms | 16.94 ms | 0.90x | 42 | 618 | REGRESSION |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=65&Reversed=False) | 6.27 us | 324.3 ns | 0.05x | 0 | 0 | faster |
| HttpMediaTypeEqualityBenchmarks.EqualsMediaTypes(Parameters=65&Reversed=True) | 6.54 us | 4.83 us | 0.74x | 0 | 0 | faster |

3 regression(s): time ratio over 1.1 or allocations over 1 x.
