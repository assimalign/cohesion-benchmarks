| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=False) | 12.35 us | 14.08 us | 1.14x | 38008 | 38008 | REGRESSION |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=True) | 3.27 us | 3.20 us | 0.98x | 8024 | 8024 |  |
| NestedValidationBenchmarks.ValidateCollection(Valid=False) | 14.45 us | 14.40 us | 1.00x | 137304 | 132184 |  |
| NestedValidationBenchmarks.ValidateCollection(Valid=True) | 12.33 us | 14.13 us | 1.15x | 126040 | 126040 | REGRESSION |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=False) | 1.62 us | 1.64 us | 1.01x | 5144 | 5144 |  |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=True) | 430.8 ns | 433.4 ns | 1.01x | 1272 | 1272 |  |
| NestedValidationBenchmarks.ValidateNested(Valid=False) | 1.97 us | 1.84 us | 0.94x | 17200 | 16528 |  |
| NestedValidationBenchmarks.ValidateNested(Valid=True) | 1.61 us | 1.72 us | 1.07x | 16056 | 16056 |  |

2 regression(s): time ratio over 1.1 or allocations over 1 x.
