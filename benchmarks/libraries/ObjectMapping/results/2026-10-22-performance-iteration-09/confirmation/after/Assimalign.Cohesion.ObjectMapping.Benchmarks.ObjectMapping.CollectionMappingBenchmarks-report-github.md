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
| **MapCollection**            | **1**     |    **60.97 ns** |  **2.521 ns** |  **1.968 ns** |  **0.96** |    **0.04** | **0.0362** |     **152 B** |        **1.06** |
| MapCollection_AutoMapper | 1     |    63.25 ns |  2.653 ns |  1.918 ns |  1.00 |    0.04 | 0.0343 |     144 B |        1.00 |
|                          |       |             |           |           |       |         |        |           |             |
| **MapCollection**            | **100**   | **4,359.08 ns** | **85.228 ns** | **56.373 ns** |  **4.65** |    **0.22** | **1.5488** |    **6488 B** |        **0.83** |
| MapCollection_AutoMapper | 100   |   939.41 ns | 60.045 ns | 46.879 ns |  1.00 |    0.07 | 1.8625 |    7792 B |        1.00 |
