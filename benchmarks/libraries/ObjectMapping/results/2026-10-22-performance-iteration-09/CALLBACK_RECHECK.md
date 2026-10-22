| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=ContextCallback) | 60.7 ns | 47.9 ns | 0.79x | 0 | 0 | faster |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=CustomProfile) | 51.4 ns | 47.7 ns | 0.93x | 0 | 0 |  |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=OverriddenProfile) | 51.3 ns | 49.2 ns | 0.96x | 0 | 0 |  |
| MappingCallbackBenchmarks.MapCallback_AutoMapper(Mode=TypedCallback) | 45.6 ns | 48.1 ns | 1.05x | 0 | 0 |  |
| MappingCallbackBenchmarks.MapCallback(Mode=ContextCallback) | 30.1 ns | 17.1 ns | 0.57x | 88 | 48 | faster |
| MappingCallbackBenchmarks.MapCallback(Mode=CustomProfile) | 39.6 ns | 31.3 ns | 0.79x | 80 | 80 | faster |
| MappingCallbackBenchmarks.MapCallback(Mode=OverriddenProfile) | 36.8 ns | 36.0 ns | 0.98x | 80 | 80 |  |
| MappingCallbackBenchmarks.MapCallback(Mode=TypedCallback) | 27.3 ns | 7.9 ns | 0.29x | 88 | 0 | faster |

0 regression(s): time ratio over 1.1 or allocations over 1 x.
