```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method              | Items | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |------ |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MapGraph**            | **1**     |   **308.9 ns** |  **13.08 ns** |  **10.22 ns** |  **2.91** |    **0.17** | **0.0896** |     **376 B** |        **1.34** |
| MapGraph_AutoMapper | 1     |   106.5 ns |   7.45 ns |   5.82 ns |  1.00 |    0.07 | 0.0669 |     280 B |        1.00 |
|                     |       |            |           |           |       |         |        |           |             |
| **MapGraph**            | **100**   | **5,627.8 ns** | **181.90 ns** | **142.02 ns** |  **4.70** |    **0.19** | **1.6022** |    **6712 B** |        **0.94** |
| MapGraph_AutoMapper | 100   | 1,199.3 ns |  53.48 ns |  41.75 ns |  1.00 |    0.05 | 1.7052 |    7136 B |        1.00 |
