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
| **MapGraph**            | **1**     |   **228.35 ns** |   **6.008 ns** |  **4.344 ns** |  **2.74** |    **0.15** | **0.0899** |     **376 B** |        **1.34** |
| MapGraph_AutoMapper | 1     |    83.65 ns |   5.899 ns |  4.606 ns |  1.00 |    0.07 | 0.0669 |     280 B |        1.00 |
|                     |       |             |            |           |       |         |        |           |             |
| **MapGraph**            | **100**   | **4,065.91 ns** | **114.613 ns** | **82.873 ns** |  **4.18** |    **0.10** | **1.6022** |    **6712 B** |        **0.94** |
| MapGraph_AutoMapper | 100   |   973.33 ns |  18.473 ns | 12.219 ns |  1.00 |    0.02 | 1.7052 |    7136 B |        1.00 |
