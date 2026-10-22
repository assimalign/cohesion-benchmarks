```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=20  
LaunchCount=1  WarmupCount=5  

```
| Method                              | Valid | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------------------------------------ |------ |------------:|----------:|----------:|------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **ValidateCollection**                  | **False** | **13,682.0 ns** | **625.90 ns** | **720.79 ns** | **13,692.9 ns** |  **1.07** |    **0.06** | **20.3400** | **0.0153** |  **83.11 KB** |        **2.24** |
| ValidateCollection_FluentValidation | False | 12,765.9 ns | 274.08 ns | 304.64 ns | 12,630.3 ns |  1.00 |    0.03 |  9.0790 |      - |  37.12 KB |        1.00 |
|                                     |       |             |           |           |             |       |         |         |        |           |             |
| **ValidateCollection**                  | **True**  |  **5,834.2 ns** | **257.49 ns** | **275.51 ns** |  **5,691.9 ns** |  **1.86** |    **0.10** |  **7.1716** |      **-** |  **29.29 KB** |        **3.74** |
| ValidateCollection_FluentValidation | True  |  3,143.0 ns |  76.67 ns |  85.22 ns |  3,085.9 ns |  1.00 |    0.04 |  1.9150 |      - |   7.84 KB |        1.00 |
|                                     |       |             |           |           |             |       |         |         |        |           |             |
| **ValidateNested**                      | **False** |  **1,633.8 ns** |  **62.11 ns** |  **69.03 ns** |  **1,600.5 ns** |  **1.05** |    **0.04** |  **2.8706** |      **-** |  **11.73 KB** |        **2.33** |
| ValidateNested_FluentValidation     | False |  1,552.5 ns |  15.90 ns |  15.62 ns |  1,548.8 ns |  1.00 |    0.01 |  1.2283 |      - |   5.02 KB |        1.00 |
|                                     |       |             |           |           |             |       |         |         |        |           |             |
| **ValidateNested**                      | **True**  |    **913.3 ns** |  **41.11 ns** |  **47.34 ns** |    **905.2 ns** |  **2.19** |    **0.12** |  **1.2875** |      **-** |   **5.26 KB** |        **4.23** |
| ValidateNested_FluentValidation     | True  |    416.9 ns |   7.67 ns |   8.21 ns |    413.3 ns |  1.00 |    0.03 |  0.3037 |      - |   1.24 KB |        1.00 |
