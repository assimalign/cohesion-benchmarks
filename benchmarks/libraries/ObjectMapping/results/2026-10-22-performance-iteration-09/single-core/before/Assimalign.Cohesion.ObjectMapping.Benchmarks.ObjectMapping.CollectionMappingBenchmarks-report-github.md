```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                   | Count | Mean        | Error       | StdDev      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------ |------------:|------------:|------------:|------:|--------:|-------:|----------:|------------:|
| **MapCollection**            | **1**     |    **782.9 ns** |    **48.74 ns** |    **38.05 ns** |  **1.90** |    **0.19** | **0.0572** |     **240 B** |        **1.67** |
| MapCollection_AutoMapper | 1     |    414.5 ns |    49.38 ns |    38.55 ns |  1.01 |    0.13 | 0.0343 |     144 B |        1.00 |
|                          |       |             |             |             |       |         |        |           |             |
| **MapCollection**            | **100**   | **91,194.0 ns** | **1,753.11 ns** | **1,267.61 ns** | **55.62** |    **2.89** | **3.5400** |   **15288 B** |        **1.96** |
| MapCollection_AutoMapper | 100   |  1,643.8 ns |   113.72 ns |    88.78 ns |  1.00 |    0.07 | 1.8616 |    7792 B |        1.00 |
