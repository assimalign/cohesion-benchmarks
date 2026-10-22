```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method              | Items | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |------ |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MapGraph**            | **1**     |   **296.15 ns** |  **7.841 ns** |  **6.122 ns** |  **2.99** |    **0.09** | **0.0896** |     **376 B** |        **1.34** |
| MapGraph_AutoMapper | 1     |    99.17 ns |  3.005 ns |  2.346 ns |  1.00 |    0.03 | 0.0669 |     280 B |        1.00 |
|                     |       |             |           |           |       |         |        |           |             |
| **MapGraph**            | **100**   | **5,389.77 ns** | **71.575 ns** | **55.881 ns** |  **5.08** |    **0.10** | **1.6022** |    **6712 B** |        **0.94** |
| MapGraph_AutoMapper | 100   | 1,061.79 ns | 24.436 ns | 17.669 ns |  1.00 |    0.02 | 1.7052 |    7136 B |        1.00 |
