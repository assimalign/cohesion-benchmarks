```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                              | Valid | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0    | Allocated | Alloc Ratio |
|------------------------------------ |------ |------------:|----------:|----------:|------:|--------:|--------:|----------:|------------:|
| **ValidateCollection**                  | **False** | **14,403.7 ns** | **846.26 ns** | **660.70 ns** |  **0.96** |    **0.05** | **19.7754** |  **80.84 KB** |        **2.15** |
| ValidateCollection_FluentValidation | False | 14,937.4 ns | 365.39 ns | 264.20 ns |  1.00 |    0.02 |  9.1858 |  37.54 KB |        1.00 |
|                                     |       |             |           |           |       |         |         |           |             |
| **ValidateCollection**                  | **True**  |  **5,900.1 ns** | **210.74 ns** | **164.53 ns** |  **1.53** |    **0.05** |  **6.4316** |  **26.27 KB** |        **3.18** |
| ValidateCollection_FluentValidation | True  |  3,845.5 ns |  91.62 ns |  71.53 ns |  1.00 |    0.03 |  2.0218 |   8.26 KB |        1.00 |
|                                     |       |             |           |           |       |         |         |           |             |
| **ValidateNested**                      | **False** |  **2,058.0 ns** | **113.94 ns** |  **88.96 ns** |  **0.94** |    **0.04** |  **2.8114** |  **11.49 KB** |        **2.27** |
| ValidateNested_FluentValidation     | False |  2,193.0 ns |  63.54 ns |  45.94 ns |  1.00 |    0.03 |  1.2398 |   5.07 KB |        1.00 |
|                                     |       |             |           |           |       |         |         |           |             |
| **ValidateNested**                      | **True**  |  **1,151.9 ns** |  **96.20 ns** |  **75.11 ns** |  **2.05** |    **0.13** |  **1.2035** |   **4.92 KB** |        **3.82** |
| ValidateNested_FluentValidation     | True  |    561.4 ns |  10.01 ns |   7.81 ns |  1.00 |    0.02 |  0.3147 |   1.29 KB |        1.00 |
