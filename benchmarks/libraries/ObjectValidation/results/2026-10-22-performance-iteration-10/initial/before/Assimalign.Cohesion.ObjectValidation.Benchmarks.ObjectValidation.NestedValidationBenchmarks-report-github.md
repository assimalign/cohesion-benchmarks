```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method                              | Valid | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------------------ |------ |------------:|------------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| **ValidateCollection**                  | **False** | **23,061.9 ns** |   **976.90 ns** | **510.94 ns** |  **1.35** |    **0.06** | **32.8064** |      **-** | **134.09 KB** |        **3.57** |
| ValidateCollection_FluentValidation | False | 17,141.8 ns | 1,331.83 ns | 696.57 ns |  1.00 |    0.05 |  9.1858 |      - |  37.54 KB |        1.00 |
|                                     |       |             |             |           |       |         |         |        |           |             |
| **ValidateCollection**                  | **True**  | **15,667.0 ns** | **1,135.61 ns** | **593.94 ns** |  **3.44** |    **0.13** | **30.1208** | **0.0305** | **123.09 KB** |       **14.91** |
| ValidateCollection_FluentValidation | True  |  4,549.2 ns |   145.27 ns |  51.81 ns |  1.00 |    0.02 |  2.0218 |      - |   8.26 KB |        1.00 |
|                                     |       |             |             |           |       |         |         |        |           |             |
| **ValidateNested**                      | **False** |  **2,400.9 ns** |    **85.54 ns** |  **44.74 ns** |  **1.10** |    **0.02** |  **4.1122** |      **-** |   **16.8 KB** |        **3.31** |
| ValidateNested_FluentValidation     | False |  2,178.8 ns |    61.88 ns |  27.48 ns |  1.00 |    0.02 |  1.2398 |      - |   5.07 KB |        1.00 |
|                                     |       |             |             |           |       |         |         |        |           |             |
| **ValidateNested**                      | **True**  |  **2,178.8 ns** |   **221.84 ns** | **116.03 ns** |  **3.48** |    **0.18** |  **3.8376** |      **-** |  **15.68 KB** |       **12.16** |
| ValidateNested_FluentValidation     | True  |    625.8 ns |    20.66 ns |  10.80 ns |  1.00 |    0.02 |  0.3147 |      - |   1.29 KB |        1.00 |
