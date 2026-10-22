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
| ValidateComparisons_FluentValidation | 3,793.2 ns | 178.94 ns | 139.70 ns |  1.00 |    0.05 | 2.0523 |   8.38 KB |        1.00 |
| ValidateComparisons                  | 2,271.9 ns |  74.25 ns |  57.97 ns |  0.60 |    0.03 | 2.0447 |   8.35 KB |        1.00 |
|                                      |            |           |           |       |         |        |           |             |
| ValidateInvalid_FluentValidation     | 3,705.2 ns | 149.01 ns | 116.34 ns |  1.00 |    0.04 | 2.0218 |   8.26 KB |        1.00 |
| ValidateInvalid                      | 1,842.7 ns |  73.43 ns |  57.33 ns |  0.50 |    0.02 | 1.6594 |   6.78 KB |        0.82 |
|                                      |            |           |           |       |         |        |           |             |
| Validate_FluentValidation            |   948.1 ns |  21.46 ns |  15.52 ns |  1.00 |    0.02 | 0.3872 |   1.59 KB |        1.00 |
| Validate                             | 1,416.2 ns |  61.28 ns |  47.85 ns |  1.49 |    0.05 | 0.8430 |   3.45 KB |        2.17 |
