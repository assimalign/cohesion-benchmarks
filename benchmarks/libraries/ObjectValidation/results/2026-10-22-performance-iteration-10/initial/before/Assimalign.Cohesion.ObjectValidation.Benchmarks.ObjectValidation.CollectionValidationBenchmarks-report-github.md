```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method                    | Count | Valid | Mean        | Error       | StdDev    | Ratio | RatioSD | Gen0    | Allocated | Alloc Ratio |
|-------------------------- |------ |------ |------------:|------------:|----------:|------:|--------:|--------:|----------:|------------:|
| **Validate**                  | **4**     | **False** |  **3,275.2 ns** |   **186.26 ns** |  **97.42 ns** |  **2.08** |    **0.09** |  **5.1308** |  **20.96 KB** |        **6.30** |
| Validate_FluentValidation | 4     | False |  1,579.2 ns |    95.15 ns |  49.77 ns |  1.00 |    0.04 |  0.8144 |   3.33 KB |        1.00 |
|                           |       |       |             |             |           |       |         |         |           |             |
| **Validate**                  | **4**     | **True**  |  **3,350.4 ns** |   **121.14 ns** |  **63.36 ns** |  **3.96** |    **0.13** |  **5.1308** |  **20.97 KB** |       **11.42** |
| Validate_FluentValidation | 4     | True  |    846.7 ns |    43.40 ns |  22.70 ns |  1.00 |    0.04 |  0.4492 |   1.84 KB |        1.00 |
|                           |       |       |             |             |           |       |         |         |           |             |
| **Validate**                  | **32**    | **False** | **25,214.2 ns** |   **440.64 ns** | **230.47 ns** |  **2.60** |    **0.24** | **37.5977** | **153.55 KB** |        **8.27** |
| Validate_FluentValidation | 32    | False |  9,755.3 ns | 1,644.65 ns | 860.18 ns |  1.01 |    0.12 |  4.5319 |  18.56 KB |        1.00 |
|                           |       |       |             |             |           |       |         |         |           |             |
| **Validate**                  | **32**    | **True**  | **25,797.3 ns** |   **677.89 ns** | **354.55 ns** |  **5.14** |    **0.18** | **37.6282** | **153.73 KB** |       **17.84** |
| Validate_FluentValidation | 32    | True  |  5,026.4 ns |   328.45 ns | 171.78 ns |  1.00 |    0.05 |  2.1057 |   8.62 KB |        1.00 |
