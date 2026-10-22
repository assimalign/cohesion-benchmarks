```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method              | Items | Mean        | Error      | StdDev     | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |------ |------------:|-----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| **MapGraph**            | **1**     |   **412.04 ns** |  **12.352 ns** |   **9.643 ns** |  **4.36** |    **0.12** | **0.1798** |     **752 B** |        **2.69** |
| MapGraph_AutoMapper | 1     |    94.51 ns |   2.161 ns |   1.687 ns |  1.00 |    0.02 | 0.0669 |     280 B |        1.00 |
|                     |       |             |            |            |       |         |        |           |             |
| **MapGraph**            | **100**   | **7,541.85 ns** | **223.682 ns** | **174.636 ns** |  **6.71** |    **0.20** | **4.0894** |   **17112 B** |        **2.40** |
| MapGraph_AutoMapper | 100   | 1,124.38 ns |  30.372 ns |  23.713 ns |  1.00 |    0.03 | 1.7052 |    7136 B |        1.00 |
