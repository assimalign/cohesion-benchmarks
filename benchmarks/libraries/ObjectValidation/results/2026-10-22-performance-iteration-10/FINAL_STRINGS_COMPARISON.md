| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| CollectionValidationBenchmarks.Validate_FluentValidation(Count=32&Valid=False) | 7.69 us | 8.38 us | 1.09x | 19008 | 19008 |  |
| CollectionValidationBenchmarks.Validate_FluentValidation(Count=32&Valid=True) | 3.70 us | 5.05 us | 1.37x | 8824 | 8824 | REGRESSION |
| CollectionValidationBenchmarks.Validate_FluentValidation(Count=4&Valid=False) | 1.06 us | 960.4 ns | 0.90x | 3408 | 3408 | faster |
| CollectionValidationBenchmarks.Validate_FluentValidation(Count=4&Valid=True) | 722.1 ns | 593.6 ns | 0.82x | 1880 | 1880 | faster |
| CollectionValidationBenchmarks.Validate(Count=32&Valid=False) | 19.79 us | 7.15 us | 0.36x | 157240 | 21538 | faster |
| CollectionValidationBenchmarks.Validate(Count=32&Valid=True) | 17.83 us | 7.06 us | 0.40x | 157424 | 10977 | faster |
| CollectionValidationBenchmarks.Validate(Count=4&Valid=False) | 2.81 us | 1.15 us | 0.41x | 21464 | 4552 | faster |
| CollectionValidationBenchmarks.Validate(Count=4&Valid=True) | 2.56 us | 987.0 ns | 0.39x | 21472 | 3232 | faster |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=False) | 14.58 us | 14.78 us | 1.01x | 38440 | 38440 |  |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=True) | 4.97 us | 3.82 us | 0.77x | 8456 | 8456 | faster |
| NestedValidationBenchmarks.ValidateCollection(Valid=False) | 17.22 us | 14.35 us | 0.83x | 137304 | 82777 | faster |
| NestedValidationBenchmarks.ValidateCollection(Valid=True) | 16.83 us | 5.89 us | 0.35x | 126040 | 26899 | faster |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=False) | 2.02 us | 2.20 us | 1.08x | 5192 | 5192 |  |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=True) | 579.1 ns | 564.7 ns | 0.98x | 1320 | 1320 |  |
| NestedValidationBenchmarks.ValidateNested(Valid=False) | 2.56 us | 2.05 us | 0.80x | 17200 | 11761 | faster |
| NestedValidationBenchmarks.ValidateNested(Valid=True) | 1.85 us | 1.14 us | 0.62x | 16056 | 5041 | faster |
| ProfileBuildBenchmarks.Build_FluentValidation(IncludeCollection=False) | 1.82 us | 1.91 us | 1.05x | 7472 | 7472 |  |
| ProfileBuildBenchmarks.Build_FluentValidation(IncludeCollection=True) | 3.68 us | 4.25 us | 1.16x | 14640 | 14640 | REGRESSION |
| ProfileBuildBenchmarks.Build(IncludeCollection=False) | 2.26 us | 2.20 us | 0.97x | 6112 | 6112 |  |
| ProfileBuildBenchmarks.Build(IncludeCollection=True) | 4.12 us | 4.50 us | 1.09x | 10952 | 10952 |  |
| ValidatorBenchmarks.Validate | 3.05 us | 1.41 us | 0.46x | 23992 | 3528 | faster |
| ValidatorBenchmarks.Validate_FluentValidation | 816.7 ns | 950.3 ns | 1.16x | 1624 | 1624 | REGRESSION |
| ValidatorBenchmarks.ValidateComparisons | 2.64 us | 2.26 us | 0.86x | 21128 | 8553 | faster |
| ValidatorBenchmarks.ValidateComparisons_FluentValidation | 3.24 us | 3.76 us | 1.16x | 8584 | 8584 | REGRESSION |
| ValidatorBenchmarks.ValidateInvalid | 2.72 us | 1.83 us | 0.67x | 21672 | 6945 | faster |
| ValidatorBenchmarks.ValidateInvalid_FluentValidation | 3.08 us | 3.69 us | 1.20x | 8456 | 8456 | REGRESSION |

5 regression(s): time ratio over 1.1 or allocations over 1 x.
