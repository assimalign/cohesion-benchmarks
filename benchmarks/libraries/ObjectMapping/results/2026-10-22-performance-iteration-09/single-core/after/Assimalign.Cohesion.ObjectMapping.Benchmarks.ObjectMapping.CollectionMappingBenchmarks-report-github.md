```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                   | Count | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------ |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MapCollection**            | **1**     |    **458.8 ns** |   **2.57 ns** |   **1.86 ns** |  **1.43** |    **0.01** | **0.0362** |     **152 B** |        **1.06** |
| MapCollection_AutoMapper | 1     |    321.1 ns |   3.58 ns |   2.80 ns |  1.00 |    0.01 | 0.0343 |     144 B |        1.00 |
|                          |       |             |           |           |       |         |        |           |             |
| **MapCollection**            | **100**   | **21,273.6 ns** | **942.63 ns** | **735.94 ns** | **17.21** |    **0.64** | **1.5259** |    **6488 B** |        **0.83** |
| MapCollection_AutoMapper | 100   |  1,236.2 ns |  27.19 ns |  21.22 ns |  1.00 |    0.02 | 1.8616 |    7792 B |        1.00 |
