| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| HttpFormBenchmarks.DecodeMultipart_AspNetCore(Fields=32) | 56.64 us | 56.39 us | 1.00x | 374465 | 374466 | REGRESSION |
| HttpFormBenchmarks.DecodeMultipart_AspNetCore(Fields=4) | 7.78 us | 7.85 us | 1.01x | 51824 | 51824 |  |
| HttpFormBenchmarks.DecodeMultipart(Fields=32) | 37.78 us | 32.86 us | 0.87x | 117889 | 103192 | faster |
| HttpFormBenchmarks.DecodeMultipart(Fields=4) | 5.31 us | 4.51 us | 0.85x | 19105 | 17313 | faster |
| HttpFormBenchmarks.DecodeUrlEncoded_AspNetCore(Fields=32) | 7.47 us | 7.32 us | 0.98x | 9568 | 9568 |  |
| HttpFormBenchmarks.DecodeUrlEncoded_AspNetCore(Fields=4) | 1.45 us | 1.31 us | 0.91x | 2128 | 2128 | faster |
| HttpFormBenchmarks.DecodeUrlEncoded(Fields=32) | 12.09 us | 12.00 us | 0.99x | 25776 | 25600 |  |
| HttpFormBenchmarks.DecodeUrlEncoded(Fields=4) | 1.91 us | 1.74 us | 0.91x | 10000 | 9824 |  |
| OpenApiPointerBenchmarks.Transform(Depth=1&SameVersion=False) | 7.07 us | 6.24 us | 0.88x | 32776 | 24320 | faster |
| OpenApiPointerBenchmarks.Transform(Depth=1&SameVersion=True) | 3.96 us | 3.98 us | 1.00x | 12400 | 12400 |  |
| OpenApiPointerBenchmarks.Transform(Depth=16&SameVersion=False) | 37.98 us | 28.16 us | 0.74x | 242545 | 125128 | faster |
| OpenApiPointerBenchmarks.Transform(Depth=16&SameVersion=True) | 18.72 us | 18.91 us | 1.01x | 57008 | 57008 |  |
| SignedTokenParsingBenchmarks.Parse_MicrosoftIdentityModel(Algorithm=HS256) | 990.6 ns | 982.2 ns | 0.99x | 2616 | 2616 |  |
| SignedTokenParsingBenchmarks.Parse_MicrosoftIdentityModel(Algorithm=RS256) | 1.01 us | 987.2 ns | 0.98x | 2616 | 2616 |  |
| SignedTokenParsingBenchmarks.Parse(Algorithm=HS256) | 1.73 us | 1.78 us | 1.03x | 5672 | 5624 |  |
| SignedTokenParsingBenchmarks.Parse(Algorithm=RS256) | 1.74 us | 1.72 us | 0.99x | 6272 | 6224 |  |

1 regression(s): time ratio over 1.1 or allocations over 1 x.
