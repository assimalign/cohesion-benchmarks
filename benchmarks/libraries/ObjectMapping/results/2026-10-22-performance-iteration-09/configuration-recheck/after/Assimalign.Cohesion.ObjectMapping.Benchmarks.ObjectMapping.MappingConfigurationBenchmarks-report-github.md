```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=20  
LaunchCount=1  WarmupCount=5  

```
| Method               | Graph | Mean         | Error       | StdDev      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|--------------------- |------ |-------------:|------------:|------------:|------:|--------:|--------:|-------:|----------:|------------:|
| **Configure**            | **False** |     **110.5 ns** |     **9.57 ns** |    **11.03 ns** | **0.001** |    **0.00** |  **0.1109** |      **-** |     **464 B** |        **0.01** |
| Configure_AutoMapper | False | 110,555.9 ns | 2,353.70 ns | 2,417.07 ns | 1.000 |    0.03 |  9.0332 | 0.2441 |   38699 B |        1.00 |
|                      |       |              |             |             |       |         |         |        |           |             |
| **Configure**            | **True**  |     **364.2 ns** |     **3.42 ns** |     **3.94 ns** | **0.002** |    **0.00** |  **0.2923** |      **-** |    **1224 B** |        **0.01** |
| Configure_AutoMapper | True  | 173,201.7 ns | 4,497.72 ns | 5,179.58 ns | 1.001 |    0.04 | 19.5313 | 0.2441 |   81840 B |        1.00 |
