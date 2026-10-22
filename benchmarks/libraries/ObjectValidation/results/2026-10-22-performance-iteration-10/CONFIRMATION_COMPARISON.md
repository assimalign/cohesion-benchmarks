| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| CollectionValidationBenchmarks.Validate_FluentValidation(Count=32&Valid=False) | 6.88 us | 7.84 us | 1.14x | 19008 | 19008 | REGRESSION |
| CollectionValidationBenchmarks.Validate_FluentValidation(Count=32&Valid=True) | 3.44 us | 4.21 us | 1.22x | 8824 | 8824 | REGRESSION |
| CollectionValidationBenchmarks.Validate_FluentValidation(Count=4&Valid=False) | 1.07 us | 1.20 us | 1.13x | 3408 | 3408 | REGRESSION |
| CollectionValidationBenchmarks.Validate_FluentValidation(Count=4&Valid=True) | 599.3 ns | 701.1 ns | 1.17x | 1880 | 1880 | REGRESSION |
| CollectionValidationBenchmarks.Validate(Count=32&Valid=False) | 19.14 us | 10.53 us | 0.55x | 157240 | 30331 | faster |
| CollectionValidationBenchmarks.Validate(Count=32&Valid=True) | 17.36 us | 9.63 us | 0.55x | 157424 | 21746 | faster |
| CollectionValidationBenchmarks.Validate(Count=4&Valid=False) | 2.64 us | 1.58 us | 0.60x | 21464 | 5601 | faster |
| CollectionValidationBenchmarks.Validate(Count=4&Valid=True) | 2.35 us | 1.31 us | 0.56x | 21472 | 4512 | faster |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=False) | 14.32 us | 18.08 us | 1.26x | 38440 | 38440 | REGRESSION |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=True) | 3.66 us | 5.07 us | 1.39x | 8456 | 8456 | REGRESSION |
| NestedValidationBenchmarks.ValidateCollection(Valid=False) | 15.67 us | 15.74 us | 1.00x | 137304 | 85105 |  |
| NestedValidationBenchmarks.ValidateCollection(Valid=True) | 12.56 us | 7.88 us | 0.63x | 126040 | 29995 | faster |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=False) | 1.81 us | 2.30 us | 1.27x | 5192 | 5192 | REGRESSION |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=True) | 523.4 ns | 595.4 ns | 1.14x | 1320 | 1320 | REGRESSION |
| NestedValidationBenchmarks.ValidateNested(Valid=False) | 2.13 us | 2.38 us | 1.12x | 17200 | 12009 | REGRESSION |
| NestedValidationBenchmarks.ValidateNested(Valid=True) | 1.81 us | 1.26 us | 0.70x | 16056 | 5385 | faster |
| ProfileBuildBenchmarks.Build_FluentValidation(IncludeCollection=False) | 1.77 us | 1.99 us | 1.12x | 7472 | 7472 | REGRESSION |
| ProfileBuildBenchmarks.Build_FluentValidation(IncludeCollection=True) | 3.56 us | 3.82 us | 1.07x | 14640 | 14640 |  |
| ProfileBuildBenchmarks.Build(IncludeCollection=False) | 2.18 us | 2.29 us | 1.05x | 6112 | 6112 |  |
| ProfileBuildBenchmarks.Build(IncludeCollection=True) | 3.97 us | 4.30 us | 1.08x | 10952 | 10952 |  |
| ValidatorBenchmarks.Validate | 2.81 us | 1.57 us | 0.56x | 23992 | 4913 | faster |
| ValidatorBenchmarks.Validate_FluentValidation | 790.2 ns | 786.5 ns | 1.00x | 1624 | 1624 |  |
| ValidatorBenchmarks.ValidateComparisons | 2.65 us | 1.80 us | 0.68x | 21128 | 8553 | faster |
| ValidatorBenchmarks.ValidateComparisons_FluentValidation | 3.07 us | 3.21 us | 1.05x | 8584 | 8584 |  |
| ValidatorBenchmarks.ValidateInvalid | 2.85 us | 1.79 us | 0.63x | 21672 | 8001 | faster |
| ValidatorBenchmarks.ValidateInvalid_FluentValidation | 2.85 us | 2.92 us | 1.02x | 8456 | 8456 |  |

10 regression(s): time ratio over 1.1 or allocations over 1 x.
