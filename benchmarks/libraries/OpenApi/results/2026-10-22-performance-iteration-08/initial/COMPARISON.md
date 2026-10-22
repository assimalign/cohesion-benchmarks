| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| HttpFormBenchmarks.DecodeMultipart_AspNetCore(Fields=32) | 58.98 us | 77.84 us | 1.32x | 374465 | 374466 | REGRESSION |
| HttpFormBenchmarks.DecodeMultipart_AspNetCore(Fields=4) | 8.55 us | 8.74 us | 1.02x | 51824 | 51824 |  |
| HttpFormBenchmarks.DecodeMultipart(Fields=32) | 36.82 us | 39.31 us | 1.07x | 117881 | 103192 |  |
| HttpFormBenchmarks.DecodeMultipart(Fields=4) | 5.17 us | 4.82 us | 0.93x | 19105 | 17313 |  |
| HttpFormBenchmarks.DecodeUrlEncoded_AspNetCore(Fields=32) | 7.32 us | 7.62 us | 1.04x | 9568 | 9568 |  |
| HttpFormBenchmarks.DecodeUrlEncoded_AspNetCore(Fields=4) | 1.43 us | 1.56 us | 1.10x | 2128 | 2128 |  |
| HttpFormBenchmarks.DecodeUrlEncoded(Fields=32) | 11.75 us | 12.43 us | 1.06x | 25776 | 25600 |  |
| HttpFormBenchmarks.DecodeUrlEncoded(Fields=4) | 1.86 us | 2.08 us | 1.12x | 10000 | 9824 | REGRESSION |
| OpenApiPointerBenchmarks.Transform(Depth=1&SameVersion=False) | 7.27 us | 7.18 us | 0.99x | 32776 | 24320 |  |
| OpenApiPointerBenchmarks.Transform(Depth=1&SameVersion=True) | 4.18 us | 4.64 us | 1.11x | 12400 | 12400 | REGRESSION |
| OpenApiPointerBenchmarks.Transform(Depth=16&SameVersion=False) | 40.11 us | 32.92 us | 0.82x | 242545 | 125129 | faster |
| OpenApiPointerBenchmarks.Transform(Depth=16&SameVersion=True) | 19.56 us | 21.89 us | 1.12x | 57008 | 57008 | REGRESSION |
| SignedTokenParsingBenchmarks.Parse_MicrosoftIdentityModel(Algorithm=HS256) | 1.04 us | 1.08 us | 1.03x | 2616 | 2616 |  |
| SignedTokenParsingBenchmarks.Parse_MicrosoftIdentityModel(Algorithm=RS256) | 1.03 us | 1.11 us | 1.08x | 2616 | 2616 |  |
| SignedTokenParsingBenchmarks.Parse(Algorithm=HS256) | 1.86 us | 1.84 us | 0.99x | 5672 | 5624 |  |
| SignedTokenParsingBenchmarks.Parse(Algorithm=RS256) | 1.78 us | 1.96 us | 1.10x | 6272 | 6224 | REGRESSION |

5 regression(s): time ratio over 1.1 or allocations over 1 x.
