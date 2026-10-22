| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=False) | 13.47 us | 13.40 us | 0.99x | 38008 | 38008 |  |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=True) | 3.29 us | 3.82 us | 1.16x | 8024 | 8024 | REGRESSION |
| NestedValidationBenchmarks.ValidateCollection(Valid=False) | 17.68 us | 14.53 us | 0.82x | 137304 | 132184 | faster |
| NestedValidationBenchmarks.ValidateCollection(Valid=True) | 12.58 us | 14.37 us | 1.14x | 126040 | 126040 | REGRESSION |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=False) | 1.66 us | 1.72 us | 1.03x | 5144 | 5144 |  |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=True) | 462.1 ns | 514.1 ns | 1.11x | 1272 | 1272 | REGRESSION |
| NestedValidationBenchmarks.ValidateNested(Valid=False) | 1.92 us | 2.03 us | 1.06x | 17200 | 16528 |  |
| NestedValidationBenchmarks.ValidateNested(Valid=True) | 1.68 us | 1.70 us | 1.01x | 16056 | 16056 |  |
| TokenValidationBenchmarks.Validate(Expired=False) | 6.3 ns | 2.8 ns | 0.45x | 32 | 0 | faster |
| TokenValidationBenchmarks.Validate(Expired=True) | 77.5 ns | 76.9 ns | 0.99x | 432 | 432 |  |

3 regression(s): time ratio over 1.1 or allocations over 1 x.
