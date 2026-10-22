| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| BmffWritePlanBenchmarks.Save(Containers=1&Prefixed=False) | 252.8 ns | 232.6 ns | 0.92x | 1800 | 1800 |  |
| BmffWritePlanBenchmarks.Save(Containers=1&Prefixed=True) | 358.5 ns | 293.4 ns | 0.82x | 2424 | 1864 | faster |
| BmffWritePlanBenchmarks.Save(Containers=16&Prefixed=False) | 3.14 us | 3.05 us | 0.97x | 21800 | 21800 |  |
| BmffWritePlanBenchmarks.Save(Containers=16&Prefixed=True) | 4.92 us | 4.21 us | 0.86x | 31784 | 22824 | faster |
| CollectionMappingBenchmarks.MapCollection_AutoMapper(Count=1) | 95.2 ns | 92.0 ns | 0.97x | 144 | 144 |  |
| CollectionMappingBenchmarks.MapCollection_AutoMapper(Count=100) | 1.03 us | 1.03 us | 0.99x | 7792 | 7792 |  |
| CollectionMappingBenchmarks.MapCollection(Count=1) | 104.1 ns | 96.5 ns | 0.93x | 264 | 240 |  |
| CollectionMappingBenchmarks.MapCollection(Count=100) | 8.02 us | 7.53 us | 0.94x | 16624 | 15288 |  |
| DnsCompressionBenchmarks.ParseCompressed(AnswerCount=32) | 4.30 us | 4.32 us | 1.01x | 12248 | 12248 |  |
| DnsCompressionBenchmarks.ParseCompressed(AnswerCount=4) | 549.8 ns | 553.7 ns | 1.01x | 1720 | 1720 |  |
| DnsCompressionBenchmarks.ParseLiteral(AnswerCount=32) | 4.36 us | 4.08 us | 0.94x | 12248 | 12248 |  |
| DnsCompressionBenchmarks.ParseLiteral(AnswerCount=4) | 575.3 ns | 544.9 ns | 0.95x | 1720 | 1720 |  |
| DnsCompressionBenchmarks.WriteDistinctSuffixes(AnswerCount=32) | 16.07 us | 13.50 us | 0.84x | 49904 | 24304 | faster |
| DnsCompressionBenchmarks.WriteDistinctSuffixes(AnswerCount=4) | 1.90 us | 1.56 us | 0.82x | 5984 | 2784 | faster |
| DnsCompressionBenchmarks.WriteSharedSuffixes(AnswerCount=32) | 9.48 us | 7.87 us | 0.83x | 28952 | 13424 | faster |
| DnsCompressionBenchmarks.WriteSharedSuffixes(AnswerCount=4) | 1.32 us | 1.07 us | 0.81x | 4040 | 1952 | faster |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=False) | 13.98 us | 14.49 us | 1.04x | 38008 | 38008 |  |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=True) | 3.74 us | 3.58 us | 0.96x | 8024 | 8024 |  |
| NestedValidationBenchmarks.ValidateCollection(Valid=False) | 15.89 us | 16.03 us | 1.01x | 137304 | 132184 |  |
| NestedValidationBenchmarks.ValidateCollection(Valid=True) | 13.41 us | 14.11 us | 1.05x | 126040 | 126040 |  |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=False) | 1.81 us | 1.88 us | 1.04x | 5144 | 5144 |  |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=True) | 459.7 ns | 479.2 ns | 1.04x | 1272 | 1272 |  |
| NestedValidationBenchmarks.ValidateNested(Valid=False) | 2.13 us | 2.25 us | 1.05x | 17200 | 16528 |  |
| NestedValidationBenchmarks.ValidateNested(Valid=True) | 1.80 us | 1.91 us | 1.06x | 16056 | 16056 |  |
| TokenValidationBenchmarks.Validate(Expired=False) | 5.6 ns | 2.6 ns | 0.47x | 32 | 0 | faster |
| TokenValidationBenchmarks.Validate(Expired=True) | 74.7 ns | 77.1 ns | 1.03x | 432 | 432 |  |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
