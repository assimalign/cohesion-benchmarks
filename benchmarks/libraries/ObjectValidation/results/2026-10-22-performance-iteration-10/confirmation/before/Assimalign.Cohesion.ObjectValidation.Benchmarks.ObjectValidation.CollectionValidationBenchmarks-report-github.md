```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                    | Count | Valid | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0    | Allocated | Alloc Ratio |
|-------------------------- |------ |------ |------------:|----------:|----------:|------:|--------:|--------:|----------:|------------:|
| **Validate**                  | **4**     | **False** |  **2,617.5 ns** | **156.44 ns** | **122.14 ns** |  **2.45** |    **0.12** |  **5.1308** |  **20.96 KB** |        **6.30** |
| Validate_FluentValidation | 4     | False |  1,068.1 ns |  21.90 ns |  15.83 ns |  1.00 |    0.02 |  0.8144 |   3.33 KB |        1.00 |
|                           |       |       |             |           |           |       |         |         |           |             |
| **Validate**                  | **4**     | **True**  |  **2,349.7 ns** |  **42.59 ns** |  **28.17 ns** |  **3.89** |    **0.09** |  **5.1308** |  **20.97 KB** |       **11.42** |
| Validate_FluentValidation | 4     | True  |    603.7 ns |  17.19 ns |  13.42 ns |  1.00 |    0.03 |  0.4492 |   1.84 KB |        1.00 |
|                           |       |       |             |           |           |       |         |         |           |             |
| **Validate**                  | **32**    | **False** | **19,208.1 ns** | **430.89 ns** | **311.56 ns** |  **2.78** |    **0.07** | **37.5977** | **153.55 KB** |        **8.27** |
| Validate_FluentValidation | 32    | False |  6,920.9 ns | 164.45 ns | 128.39 ns |  1.00 |    0.03 |  4.5395 |  18.56 KB |        1.00 |
|                           |       |       |             |           |           |       |         |         |           |             |
| **Validate**                  | **32**    | **True**  | **17,429.9 ns** | **533.51 ns** | **352.88 ns** |  **5.03** |    **0.12** | **37.6282** | **153.73 KB** |       **17.84** |
| Validate_FluentValidation | 32    | True  |  3,469.0 ns |  61.84 ns |  48.28 ns |  1.00 |    0.02 |  2.1095 |   8.62 KB |        1.00 |
