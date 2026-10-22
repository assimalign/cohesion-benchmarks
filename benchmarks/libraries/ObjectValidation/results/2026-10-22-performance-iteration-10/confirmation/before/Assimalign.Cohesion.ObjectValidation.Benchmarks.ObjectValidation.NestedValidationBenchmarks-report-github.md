```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                              | Valid | Mean        | Error       | StdDev      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------------------ |------ |------------:|------------:|------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **ValidateCollection**                  | **False** | **16,484.8 ns** | **1,795.44 ns** | **1,401.76 ns** |  **1.16** |    **0.10** | **32.8064** |      **-** | **134.09 KB** |        **3.57** |
| ValidateCollection_FluentValidation | False | 14,268.3 ns |   447.65 ns |   349.50 ns |  1.00 |    0.03 |  9.1858 |      - |  37.54 KB |        1.00 |
|                                     |       |             |             |             |       |         |         |        |           |             |
| **ValidateCollection**                  | **True**  | **12,694.4 ns** |   **470.17 ns** |   **339.97 ns** |  **3.47** |    **0.11** | **30.1361** | **0.0153** | **123.09 KB** |       **14.91** |
| ValidateCollection_FluentValidation | True  |  3,663.0 ns |   102.24 ns |    79.83 ns |  1.00 |    0.03 |  2.0218 |      - |   8.26 KB |        1.00 |
|                                     |       |             |             |             |       |         |         |        |           |             |
| **ValidateNested**                      | **False** |  **2,144.9 ns** |   **191.54 ns** |   **149.54 ns** |  **1.17** |    **0.08** |  **4.1122** |      **-** |   **16.8 KB** |        **3.31** |
| ValidateNested_FluentValidation     | False |  1,834.3 ns |    68.62 ns |    53.57 ns |  1.00 |    0.04 |  1.2398 |      - |   5.07 KB |        1.00 |
|                                     |       |             |             |             |       |         |         |        |           |             |
| **ValidateNested**                      | **True**  |  **1,803.6 ns** |   **157.46 ns** |   **122.94 ns** |  **3.44** |    **0.25** |  **3.8376** |      **-** |  **15.68 KB** |       **12.16** |
| ValidateNested_FluentValidation     | True  |    524.0 ns |    20.57 ns |    16.06 ns |  1.00 |    0.04 |  0.3147 |      - |   1.29 KB |        1.00 |
