```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method                              | Valid | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0    | Allocated | Alloc Ratio |
|------------------------------------ |------ |------------:|----------:|----------:|------:|--------:|--------:|----------:|------------:|
| **ValidateCollection**                  | **False** | **15,479.8 ns** | **515.61 ns** | **228.93 ns** |  **1.00** |    **0.03** | **20.8740** |  **85.25 KB** |        **2.27** |
| ValidateCollection_FluentValidation | False | 15,502.7 ns | 824.18 ns | 431.06 ns |  1.00 |    0.04 |  9.1858 |  37.54 KB |        1.00 |
|                                     |       |             |           |           |       |         |         |           |             |
| **ValidateCollection**                  | **True**  |  **8,719.5 ns** | **573.93 ns** | **300.18 ns** |  **2.13** |    **0.11** | **11.8866** |  **48.56 KB** |        **5.88** |
| ValidateCollection_FluentValidation | True  |  4,102.4 ns | 333.63 ns | 174.49 ns |  1.00 |    0.06 |  2.0218 |   8.26 KB |        1.00 |
|                                     |       |             |           |           |       |         |         |           |             |
| **ValidateNested**                      | **False** |  **1,997.1 ns** | **163.78 ns** |  **85.66 ns** |  **1.06** |    **0.05** |  **2.8687** |  **11.73 KB** |        **2.31** |
| ValidateNested_FluentValidation     | False |  1,892.7 ns | 120.30 ns |  62.92 ns |  1.00 |    0.05 |  1.2398 |   5.07 KB |        1.00 |
|                                     |       |             |           |           |       |         |         |           |             |
| **ValidateNested**                      | **True**  |  **1,217.9 ns** |  **36.69 ns** |  **19.19 ns** |  **2.45** |    **0.06** |  **1.8101** |    **7.4 KB** |        **5.74** |
| ValidateNested_FluentValidation     | True  |    497.8 ns |  21.03 ns |  11.00 ns |  1.00 |    0.03 |  0.3147 |   1.29 KB |        1.00 |
