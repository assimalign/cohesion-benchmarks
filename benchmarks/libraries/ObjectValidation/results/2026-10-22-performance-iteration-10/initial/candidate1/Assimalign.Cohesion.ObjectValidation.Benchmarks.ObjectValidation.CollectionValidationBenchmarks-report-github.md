```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method                    | Count | Valid | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|-------------------------- |------ |------ |------------:|----------:|----------:|------:|--------:|-------:|-------:|----------:|------------:|
| **Validate**                  | **4**     | **False** |  **1,679.9 ns** |  **91.79 ns** |  **48.01 ns** |  **1.27** |    **0.06** | **1.3390** |      **-** |   **5.47 KB** |        **1.64** |
| Validate_FluentValidation | 4     | False |  1,327.3 ns | 115.42 ns |  60.37 ns |  1.00 |    0.06 | 0.8144 |      - |   3.33 KB |        1.00 |
|                           |       |       |             |           |           |       |         |        |        |           |             |
| **Validate**                  | **4**     | **True**  |  **1,620.2 ns** |  **84.01 ns** |  **43.94 ns** |  **2.08** |    **0.09** | **1.0777** |      **-** |   **4.41 KB** |        **2.40** |
| Validate_FluentValidation | 4     | True  |    778.4 ns |  57.83 ns |  30.24 ns |  1.00 |    0.05 | 0.4492 |      - |   1.84 KB |        1.00 |
|                           |       |       |             |           |           |       |         |        |        |           |             |
| **Validate**                  | **32**    | **False** | **10,713.0 ns** | **478.92 ns** | **212.65 ns** |  **1.37** |    **0.06** | **7.2479** | **0.0305** |  **29.62 KB** |        **1.60** |
| Validate_FluentValidation | 32    | False |  7,808.0 ns | 639.32 ns | 334.38 ns |  1.00 |    0.06 | 4.5319 |      - |  18.56 KB |        1.00 |
|                           |       |       |             |           |           |       |         |        |        |           |             |
| **Validate**                  | **32**    | **True**  |  **9,004.3 ns** | **408.44 ns** | **213.62 ns** |  **2.21** |    **0.06** | **5.1880** |      **-** |  **21.24 KB** |        **2.46** |
| Validate_FluentValidation | 32    | True  |  4,067.9 ns | 150.41 ns |  78.67 ns |  1.00 |    0.03 | 2.1057 |      - |   8.62 KB |        1.00 |
