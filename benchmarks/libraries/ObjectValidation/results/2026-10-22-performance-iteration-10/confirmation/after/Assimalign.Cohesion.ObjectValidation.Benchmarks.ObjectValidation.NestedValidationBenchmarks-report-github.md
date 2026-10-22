```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                              | Valid | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------------------ |------ |------------:|------------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
| **ValidateCollection**                  | **False** | **15,738.6 ns** |   **670.65 ns** | **484.92 ns** |  **0.88** |    **0.05** | **20.3247** | **0.0305** |  **83.11 KB** |        **2.21** |
| ValidateCollection_FluentValidation | False | 17,928.8 ns | 1,085.73 ns | 847.67 ns |  1.00 |    0.06 |  9.1858 |      - |  37.54 KB |        1.00 |
|                                     |       |             |             |           |       |         |         |        |           |             |
| **ValidateCollection**                  | **True**  |  **7,943.6 ns** |   **360.22 ns** | **281.24 ns** |  **1.57** |    **0.06** |  **7.1716** |      **-** |  **29.29 KB** |        **3.55** |
| ValidateCollection_FluentValidation | True  |  5,069.4 ns |    98.07 ns |  76.57 ns |  1.00 |    0.02 |  2.0218 |      - |   8.26 KB |        1.00 |
|                                     |       |             |             |           |       |         |         |        |           |             |
| **ValidateNested**                      | **False** |  **2,377.7 ns** |   **155.73 ns** | **121.58 ns** |  **1.07** |    **0.09** |  **2.8687** |      **-** |  **11.73 KB** |        **2.31** |
| ValidateNested_FluentValidation     | False |  2,229.6 ns |   187.66 ns | 146.51 ns |  1.00 |    0.09 |  1.2398 |      - |   5.07 KB |        1.00 |
|                                     |       |             |             |           |       |         |         |        |           |             |
| **ValidateNested**                      | **True**  |  **1,265.8 ns** |    **42.98 ns** |  **28.43 ns** |  **2.12** |    **0.07** |  **1.2875** |      **-** |   **5.26 KB** |        **4.08** |
| ValidateNested_FluentValidation     | True  |    596.7 ns |    18.00 ns |  14.05 ns |  1.00 |    0.03 |  0.3147 |      - |   1.29 KB |        1.00 |
