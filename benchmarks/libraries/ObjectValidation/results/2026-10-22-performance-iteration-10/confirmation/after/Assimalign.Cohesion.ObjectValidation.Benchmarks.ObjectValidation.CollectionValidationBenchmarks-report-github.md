```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                    | Count | Valid | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|-------------------------- |------ |------ |------------:|----------:|----------:|------:|--------:|-------:|-------:|----------:|------------:|
| **Validate**                  | **4**     | **False** |  **1,603.2 ns** |  **87.34 ns** |  **68.19 ns** |  **1.33** |    **0.06** | **1.3390** |      **-** |   **5.47 KB** |        **1.64** |
| Validate_FluentValidation | 4     | False |  1,205.3 ns |  24.22 ns |  17.51 ns |  1.00 |    0.02 | 0.8144 |      - |   3.33 KB |        1.00 |
|                           |       |       |             |           |           |       |         |        |        |           |             |
| **Validate**                  | **4**     | **True**  |  **1,311.2 ns** |  **34.10 ns** |  **24.65 ns** |  **1.86** |    **0.06** | **1.0777** |      **-** |   **4.41 KB** |        **2.40** |
| Validate_FluentValidation | 4     | True  |    704.0 ns |  22.86 ns |  17.85 ns |  1.00 |    0.03 | 0.4492 |      - |   1.84 KB |        1.00 |
|                           |       |       |             |           |           |       |         |        |        |           |             |
| **Validate**                  | **32**    | **False** | **10,446.7 ns** | **425.25 ns** | **332.01 ns** |  **1.33** |    **0.04** | **7.2479** | **0.0305** |  **29.62 KB** |        **1.60** |
| Validate_FluentValidation | 32    | False |  7,833.9 ns | 116.28 ns |  76.91 ns |  1.00 |    0.01 | 4.5319 |      - |  18.56 KB |        1.00 |
|                           |       |       |             |           |           |       |         |        |        |           |             |
| **Validate**                  | **32**    | **True**  |  **9,574.1 ns** | **590.49 ns** | **461.02 ns** |  **2.27** |    **0.12** | **5.1880** |      **-** |  **21.24 KB** |        **2.46** |
| Validate_FluentValidation | 32    | True  |  4,216.8 ns | 159.57 ns | 124.58 ns |  1.00 |    0.04 | 2.1057 |      - |   8.62 KB |        1.00 |
