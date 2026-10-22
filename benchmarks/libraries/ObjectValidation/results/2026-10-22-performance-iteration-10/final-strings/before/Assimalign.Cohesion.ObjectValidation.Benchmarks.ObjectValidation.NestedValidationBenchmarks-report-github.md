```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                              | Valid | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------------------ |------ |------------:|----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| **ValidateCollection**                  | **False** | **17,358.9 ns** | **553.54 ns** | **432.17 ns** |  **1.18** |    **0.04** | **32.8064** |      **-** | **134.09 KB** |        **3.57** |
| ValidateCollection_FluentValidation | False | 14,712.3 ns | 438.34 ns | 289.94 ns |  1.00 |    0.03 |  9.1858 |      - |  37.54 KB |        1.00 |
|                                     |       |             |           |           |       |         |         |        |           |             |
| **ValidateCollection**                  | **True**  | **16,807.2 ns** | **722.23 ns** | **563.87 ns** |  **3.38** |    **0.13** | **30.1208** | **0.0305** | **123.09 KB** |       **14.91** |
| ValidateCollection_FluentValidation | True  |  4,970.6 ns | 135.23 ns | 105.58 ns |  1.00 |    0.03 |  2.0218 |      - |   8.26 KB |        1.00 |
|                                     |       |             |           |           |       |         |         |        |           |             |
| **ValidateNested**                      | **False** |  **2,564.6 ns** | **164.43 ns** | **128.38 ns** |  **1.27** |    **0.06** |  **4.1122** |      **-** |   **16.8 KB** |        **3.31** |
| ValidateNested_FluentValidation     | False |  2,021.7 ns |  30.14 ns |  23.53 ns |  1.00 |    0.02 |  1.2398 |      - |   5.07 KB |        1.00 |
|                                     |       |             |           |           |       |         |         |        |           |             |
| **ValidateNested**                      | **True**  |  **1,889.5 ns** | **149.20 ns** | **116.48 ns** |  **3.25** |    **0.21** |  **3.8376** |      **-** |  **15.68 KB** |       **12.16** |
| ValidateNested_FluentValidation     | True  |    581.5 ns |  19.09 ns |  14.91 ns |  1.00 |    0.03 |  0.3147 |      - |   1.29 KB |        1.00 |
