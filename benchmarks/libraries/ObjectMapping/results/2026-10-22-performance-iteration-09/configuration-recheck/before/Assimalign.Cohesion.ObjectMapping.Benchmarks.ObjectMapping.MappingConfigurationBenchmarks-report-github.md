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
| **Configure**            | **False** |     **124.2 ns** |     **6.40 ns** |     **7.37 ns** | **0.001** |    **0.00** |  **0.1109** |      **-** |     **464 B** |        **0.01** |
| Configure_AutoMapper | False | 101,042.4 ns | 2,294.43 ns | 2,642.27 ns | 1.001 |    0.04 |  9.0332 | 0.2441 |   38739 B |        1.00 |
|                      |       |              |             |             |       |         |         |        |           |             |
| **Configure**            | **True**  |     **327.2 ns** |    **10.52 ns** |    **12.12 ns** | **0.002** |    **0.00** |  **0.2923** |      **-** |    **1224 B** |        **0.01** |
| Configure_AutoMapper | True  | 147,928.5 ns | 2,988.66 ns | 3,321.89 ns | 1.000 |    0.03 | 19.5313 |      - |   81752 B |        1.00 |
