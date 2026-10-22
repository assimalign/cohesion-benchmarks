```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                    | Count | Valid | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0    | Allocated | Alloc Ratio |
|-------------------------- |------ |------ |------------:|------------:|----------:|------:|--------:|--------:|----------:|------------:|
| **Validate**                  | **4**     | **False** |  **2,815.6 ns** |   **113.07 ns** |  **88.28 ns** |  **2.64** |    **0.11** |  **5.1308** |  **20.96 KB** |        **6.30** |
| Validate_FluentValidation | 4     | False |  1,068.5 ns |    40.45 ns |  29.25 ns |  1.00 |    0.04 |  0.8144 |   3.33 KB |        1.00 |
|                           |       |       |             |             |           |       |         |         |           |             |
| **Validate**                  | **4**     | **True**  |  **2,583.1 ns** |    **67.93 ns** |  **49.12 ns** |  **3.57** |    **0.15** |  **5.1308** |  **20.97 KB** |       **11.42** |
| Validate_FluentValidation | 4     | True  |    724.4 ns |    35.50 ns |  27.72 ns |  1.00 |    0.05 |  0.4492 |   1.84 KB |        1.00 |
|                           |       |       |             |             |           |       |         |         |           |             |
| **Validate**                  | **32**    | **False** | **20,062.6 ns** | **1,168.34 ns** | **912.17 ns** |  **2.59** |    **0.13** | **37.5977** | **153.55 KB** |        **8.27** |
| Validate_FluentValidation | 32    | False |  7,738.0 ns |   278.47 ns | 217.41 ns |  1.00 |    0.04 |  4.5319 |  18.56 KB |        1.00 |
|                           |       |       |             |             |           |       |         |         |           |             |
| **Validate**                  | **32**    | **True**  | **17,809.6 ns** |   **185.01 ns** | **122.37 ns** |  **4.83** |    **0.11** | **37.6282** | **153.73 KB** |       **17.84** |
| Validate_FluentValidation | 32    | True  |  3,689.6 ns |   102.36 ns |  79.91 ns |  1.00 |    0.03 |  2.1095 |   8.62 KB |        1.00 |
