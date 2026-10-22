```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method              | Items | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |------ |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MapGraph**            | **1**     |   **273.80 ns** | **12.401 ns** |  **6.486 ns** |  **3.26** |    **0.17** | **0.1106** |     **464 B** |        **1.66** |
| MapGraph_AutoMapper | 1     |    84.25 ns |  8.148 ns |  4.261 ns |  1.00 |    0.07 | 0.0669 |     280 B |        1.00 |
|                     |       |             |           |           |       |         |        |           |             |
| **MapGraph**            | **100**   | **4,306.16 ns** | **28.778 ns** | **12.777 ns** |  **4.25** |    **0.08** | **1.6251** |    **6800 B** |        **0.95** |
| MapGraph_AutoMapper | 100   | 1,013.47 ns | 38.904 ns | 20.348 ns |  1.00 |    0.03 | 1.7052 |    7136 B |        1.00 |
