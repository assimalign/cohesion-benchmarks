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
| **MapGraph**            | **1**     |   **744.07 ns** | **27.877 ns** | **21.765 ns** |  **7.64** |    **0.23** | **0.0896** |     **376 B** |        **1.34** |
| MapGraph_AutoMapper | 1     |    97.41 ns |  1.359 ns |  0.983 ns |  1.00 |    0.01 | 0.0669 |     280 B |        1.00 |
|                     |       |             |           |           |       |         |        |           |             |
| **MapGraph**            | **100**   | **4,759.86 ns** | **91.604 ns** | **66.236 ns** |  **4.78** |    **0.07** | **1.6022** |    **6712 B** |        **0.94** |
| MapGraph_AutoMapper | 100   |   995.84 ns | 11.295 ns |  8.167 ns |  1.00 |    0.01 | 1.7052 |    7136 B |        1.00 |
