```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method              | Items | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |------ |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MapGraph**            | **1**     | **1,445.57 ns** |  **34.326 ns** | **26.800 ns** | **14.75** |    **0.31** | **0.1793** |     **752 B** |        **2.69** |
| MapGraph_AutoMapper | 1     |    98.01 ns |   1.389 ns |  1.085 ns |  1.00 |    0.02 | 0.0669 |     280 B |        1.00 |
|                     |       |             |            |           |       |         |        |           |             |
| **MapGraph**            | **100**   | **6,996.61 ns** | **116.943 ns** | **84.558 ns** |  **7.05** |    **0.08** | **4.0894** |   **17112 B** |        **2.40** |
| MapGraph_AutoMapper | 100   |   992.96 ns |   4.147 ns |  2.743 ns |  1.00 |    0.00 | 1.7052 |    7136 B |        1.00 |
