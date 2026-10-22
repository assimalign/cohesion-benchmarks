```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method                               | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| ValidateComparisons_FluentValidation | 3,330.0 ns | 309.67 ns | 161.96 ns |  1.00 |    0.06 | 2.0523 |   8.38 KB |        1.00 |
| ValidateComparisons                  | 1,975.3 ns |  72.34 ns |  32.12 ns |  0.59 |    0.03 | 2.0447 |   8.35 KB |        1.00 |
|                                      |            |           |           |       |         |        |           |             |
| ValidateInvalid_FluentValidation     | 3,307.6 ns |  61.69 ns |  27.39 ns |  1.00 |    0.01 | 2.0218 |   8.26 KB |        1.00 |
| ValidateInvalid                      | 1,990.8 ns |  22.80 ns |  10.12 ns |  0.60 |    0.01 | 1.9112 |   7.81 KB |        0.95 |
|                                      |            |           |           |       |         |        |           |             |
| Validate_FluentValidation            |   862.9 ns |  21.03 ns |   9.34 ns |  1.00 |    0.01 | 0.3881 |   1.59 KB |        1.00 |
| Validate                             | 1,733.7 ns |  49.21 ns |  21.85 ns |  2.01 |    0.03 | 1.1730 |    4.8 KB |        3.03 |
