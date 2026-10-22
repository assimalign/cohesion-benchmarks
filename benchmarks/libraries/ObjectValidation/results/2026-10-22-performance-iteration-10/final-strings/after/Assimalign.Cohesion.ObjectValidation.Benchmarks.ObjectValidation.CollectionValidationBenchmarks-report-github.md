```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                    | Count | Valid | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------- |------ |------ |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Validate**                  | **4**     | **False** | **1,113.8 ns** | **123.19 ns** |  **96.18 ns** |  **1.14** |    **0.11** | **1.0872** |   **4.45 KB** |        **1.34** |
| Validate_FluentValidation | 4     | False |   978.8 ns |  54.46 ns |  42.52 ns |  1.00 |    0.06 | 0.8144 |   3.33 KB |        1.00 |
|                           |       |       |            |           |           |       |         |        |           |             |
| **Validate**                  | **4**     | **True**  |   **930.4 ns** | **113.26 ns** |  **88.42 ns** |  **1.56** |    **0.15** | **0.7725** |   **3.16 KB** |        **1.72** |
| Validate_FluentValidation | 4     | True  |   596.0 ns |  13.73 ns |  10.72 ns |  1.00 |    0.02 | 0.4492 |   1.84 KB |        1.00 |
|                           |       |       |            |           |           |       |         |        |           |             |
| **Validate**                  | **32**    | **False** | **7,071.0 ns** | **640.68 ns** | **500.20 ns** |  **0.84** |    **0.07** | **5.1498** |  **21.03 KB** |        **1.13** |
| Validate_FluentValidation | 32    | False | 8,411.7 ns | 511.11 ns | 399.04 ns |  1.00 |    0.06 | 4.5319 |  18.56 KB |        1.00 |
|                           |       |       |            |           |           |       |         |        |           |             |
| **Validate**                  | **32**    | **True**  | **7,066.8 ns** | **207.34 ns** | **161.88 ns** |  **1.43** |    **0.08** | **2.6245** |  **10.72 KB** |        **1.24** |
| Validate_FluentValidation | 32    | True  | 4,951.5 ns | 307.00 ns | 239.68 ns |  1.00 |    0.07 | 2.1057 |   8.62 KB |        1.00 |
