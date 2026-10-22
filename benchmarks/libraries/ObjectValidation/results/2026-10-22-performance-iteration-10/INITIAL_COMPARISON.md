| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| CollectionValidationBenchmarks.Validate_FluentValidation(Count=32&Valid=False) | 10.10 us | 7.92 us | 0.78x | 19008 | 19008 | faster |
| CollectionValidationBenchmarks.Validate_FluentValidation(Count=32&Valid=True) | 5.03 us | 4.06 us | 0.81x | 8824 | 8824 | faster |
| CollectionValidationBenchmarks.Validate_FluentValidation(Count=4&Valid=False) | 1.60 us | 1.34 us | 0.84x | 3408 | 3408 | faster |
| CollectionValidationBenchmarks.Validate_FluentValidation(Count=4&Valid=True) | 852.2 ns | 772.1 ns | 0.91x | 1880 | 1880 | faster |
| CollectionValidationBenchmarks.Validate(Count=32&Valid=False) | 25.19 us | 10.81 us | 0.43x | 157240 | 30331 | faster |
| CollectionValidationBenchmarks.Validate(Count=32&Valid=True) | 25.92 us | 8.92 us | 0.34x | 157424 | 21746 | faster |
| CollectionValidationBenchmarks.Validate(Count=4&Valid=False) | 3.27 us | 1.68 us | 0.51x | 21464 | 5601 | faster |
| CollectionValidationBenchmarks.Validate(Count=4&Valid=True) | 3.34 us | 1.61 us | 0.48x | 21472 | 4512 | faster |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=False) | 16.87 us | 15.52 us | 0.92x | 38440 | 38440 |  |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=True) | 4.57 us | 4.01 us | 0.88x | 8456 | 8456 | faster |
| NestedValidationBenchmarks.ValidateCollection(Valid=False) | 23.03 us | 15.56 us | 0.68x | 137304 | 87297 | faster |
| NestedValidationBenchmarks.ValidateCollection(Valid=True) | 15.88 us | 8.66 us | 0.55x | 126040 | 49725 | faster |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=False) | 2.18 us | 1.91 us | 0.88x | 5192 | 5192 | faster |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=True) | 628.0 ns | 495.3 ns | 0.79x | 1320 | 1320 | faster |
| NestedValidationBenchmarks.ValidateNested(Valid=False) | 2.38 us | 2.04 us | 0.85x | 17200 | 12009 | faster |
| NestedValidationBenchmarks.ValidateNested(Valid=True) | 2.21 us | 1.22 us | 0.55x | 16056 | 7577 | faster |
| ProfileBuildBenchmarks.Build_FluentValidation(IncludeCollection=False) | 2.15 us | 1.75 us | 0.81x | 7472 | 7472 | faster |
| ProfileBuildBenchmarks.Build_FluentValidation(IncludeCollection=True) | 4.59 us | 3.79 us | 0.83x | 14640 | 14640 | faster |
| ProfileBuildBenchmarks.Build(IncludeCollection=False) | 2.57 us | 2.04 us | 0.80x | 6112 | 6072 | faster |
| ProfileBuildBenchmarks.Build(IncludeCollection=True) | 5.08 us | 4.10 us | 0.81x | 10952 | 10912 | faster |
| ValidatorBenchmarks.Validate | 3.69 us | 1.73 us | 0.47x | 23992 | 4913 | faster |
| ValidatorBenchmarks.Validate_FluentValidation | 994.9 ns | 863.8 ns | 0.87x | 1624 | 1624 | faster |
| ValidatorBenchmarks.ValidateComparisons | 3.24 us | 1.99 us | 0.61x | 21128 | 8553 | faster |
| ValidatorBenchmarks.ValidateComparisons_FluentValidation | 3.95 us | 3.31 us | 0.84x | 8584 | 8584 | faster |
| ValidatorBenchmarks.ValidateInvalid | 3.19 us | 1.99 us | 0.62x | 21672 | 8001 | faster |
| ValidatorBenchmarks.ValidateInvalid_FluentValidation | 3.52 us | 3.30 us | 0.94x | 8456 | 8456 |  |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
