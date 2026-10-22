```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=20  
LaunchCount=1  WarmupCount=5  

```
| Method              | Items | Mean        | Error      | StdDev     | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |------ |------------:|-----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| **MapGraph**            | **1**     |   **421.95 ns** |  **44.570 ns** |  **51.327 ns** |  **4.66** |    **0.56** | **0.1798** |     **752 B** |        **2.69** |
| MapGraph_AutoMapper | 1     |    90.60 ns |   1.839 ns |   2.118 ns |  1.00 |    0.03 | 0.0669 |     280 B |        1.00 |
|                     |       |             |            |            |       |         |        |           |             |
| **MapGraph**            | **100**   | **7,658.95 ns** | **249.837 ns** | **287.713 ns** |  **7.18** |    **0.31** | **4.0894** |   **17112 B** |        **2.40** |
| MapGraph_AutoMapper | 100   | 1,067.30 ns |  22.541 ns |  25.958 ns |  1.00 |    0.03 | 1.7052 |    7136 B |        1.00 |
