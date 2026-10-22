```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                               | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| ValidateComparisons_FluentValidation | 3,074.9 ns | 116.76 ns |  91.16 ns |  1.00 |    0.04 | 2.0523 |   8.38 KB |        1.00 |
| ValidateComparisons                  | 2,604.8 ns | 140.44 ns | 109.65 ns |  0.85 |    0.04 | 5.0507 |  20.63 KB |        2.46 |
|                                      |            |           |           |       |         |        |           |             |
| ValidateInvalid_FluentValidation     | 2,897.9 ns |  98.77 ns |  77.12 ns |  1.00 |    0.04 | 2.0218 |   8.26 KB |        1.00 |
| ValidateInvalid                      | 2,826.6 ns | 174.51 ns | 136.25 ns |  0.98 |    0.05 | 5.1804 |  21.16 KB |        2.56 |
|                                      |            |           |           |       |         |        |           |             |
| Validate_FluentValidation            |   792.5 ns |  20.66 ns |  16.13 ns |  1.00 |    0.03 | 0.3881 |   1.59 KB |        1.00 |
| Validate                             | 2,861.4 ns | 150.32 ns | 117.36 ns |  3.61 |    0.16 | 5.7335 |  23.43 KB |       14.77 |
