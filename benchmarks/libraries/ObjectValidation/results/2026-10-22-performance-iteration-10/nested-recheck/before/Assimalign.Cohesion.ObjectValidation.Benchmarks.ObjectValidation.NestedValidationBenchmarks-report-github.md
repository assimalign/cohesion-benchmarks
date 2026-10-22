```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=20  
LaunchCount=1  WarmupCount=5  

```
| Method                              | Valid | Mean        | Error       | StdDev      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------------------ |------ |------------:|------------:|------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **ValidateCollection**                  | **False** | **17,682.2 ns** | **1,030.88 ns** | **1,187.17 ns** |  **1.31** |    **0.09** | **32.8064** |      **-** | **134.09 KB** |        **3.61** |
| ValidateCollection_FluentValidation | False | 13,517.1 ns |   291.79 ns |   336.03 ns |  1.00 |    0.03 |  9.0790 |      - |  37.12 KB |        1.00 |
|                                     |       |             |             |             |       |         |         |        |           |             |
| **ValidateCollection**                  | **True**  | **12,950.6 ns** |   **639.11 ns** |   **736.00 ns** |  **4.11** |    **0.23** | **30.1361** | **0.0153** | **123.09 KB** |       **15.71** |
| ValidateCollection_FluentValidation | True  |  3,150.6 ns |    41.91 ns |    43.04 ns |  1.00 |    0.02 |  1.9150 |      - |   7.84 KB |        1.00 |
|                                     |       |             |             |             |       |         |         |        |           |             |
| **ValidateNested**                      | **False** |  **1,861.2 ns** |    **28.86 ns** |    **30.88 ns** |  **1.15** |    **0.03** |  **4.1122** |      **-** |   **16.8 KB** |        **3.34** |
| ValidateNested_FluentValidation     | False |  1,613.4 ns |    30.98 ns |    33.15 ns |  1.00 |    0.03 |  1.2283 |      - |   5.02 KB |        1.00 |
|                                     |       |             |             |             |       |         |         |        |           |             |
| **ValidateNested**                      | **True**  |  **1,611.2 ns** |   **103.85 ns** |   **119.59 ns** |  **3.79** |    **0.28** |  **3.8376** |      **-** |  **15.68 KB** |       **12.62** |
| ValidateNested_FluentValidation     | True  |    425.7 ns |     2.80 ns |     2.75 ns |  1.00 |    0.01 |  0.3037 |      - |   1.24 KB |        1.00 |
