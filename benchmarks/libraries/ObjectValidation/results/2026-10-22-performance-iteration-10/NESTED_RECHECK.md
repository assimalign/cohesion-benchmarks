| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=False) | 13.41 us | 12.63 us | 0.94x | 38008 | 38008 |  |
| NestedValidationBenchmarks.ValidateCollection_FluentValidation(Valid=True) | 3.14 us | 3.09 us | 0.98x | 8024 | 8024 |  |
| NestedValidationBenchmarks.ValidateCollection(Valid=False) | 17.80 us | 13.69 us | 0.77x | 137304 | 85105 | faster |
| NestedValidationBenchmarks.ValidateCollection(Valid=True) | 12.60 us | 5.69 us | 0.45x | 126040 | 29995 | faster |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=False) | 1.60 us | 1.55 us | 0.97x | 5144 | 5144 |  |
| NestedValidationBenchmarks.ValidateNested_FluentValidation(Valid=True) | 424.7 ns | 413.3 ns | 0.97x | 1272 | 1272 |  |
| NestedValidationBenchmarks.ValidateNested(Valid=False) | 1.85 us | 1.60 us | 0.87x | 17200 | 12009 | faster |
| NestedValidationBenchmarks.ValidateNested(Valid=True) | 1.55 us | 905.2 ns | 0.58x | 16056 | 5385 | faster |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
