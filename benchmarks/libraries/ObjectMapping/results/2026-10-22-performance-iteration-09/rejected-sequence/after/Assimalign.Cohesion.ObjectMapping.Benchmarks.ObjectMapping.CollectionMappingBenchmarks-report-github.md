```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                   | Count | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------ |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MapCollection**            | **1**     |    **83.21 ns** |   **1.139 ns** |  **0.889 ns** |  **0.89** |    **0.01** | **0.0286** |     **120 B** |        **0.83** |
| MapCollection_AutoMapper | 1     |    93.47 ns |   1.249 ns |  0.975 ns |  1.00 |    0.01 | 0.0343 |     144 B |        1.00 |
|                          |       |             |            |           |       |         |        |           |             |
| **MapCollection**            | **100**   | **7,381.86 ns** | **117.212 ns** | **91.512 ns** |  **5.30** |    **0.11** | **1.5411** |    **6456 B** |        **0.83** |
| MapCollection_AutoMapper | 100   | 1,393.95 ns |  32.994 ns | 23.857 ns |  1.00 |    0.02 | 1.8616 |    7792 B |        1.00 |
