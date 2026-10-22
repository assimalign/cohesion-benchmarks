```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                   | Count | Mean        | Error      | StdDev     | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------ |------------:|-----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| **MapCollection**            | **1**     |    **91.26 ns** |   **1.551 ns** |   **1.211 ns** |  **1.39** |    **0.03** | **0.0573** |     **240 B** |        **1.67** |
| MapCollection_AutoMapper | 1     |    65.56 ns |   1.899 ns |   1.482 ns |  1.00 |    0.03 | 0.0343 |     144 B |        1.00 |
|                          |       |             |            |            |       |         |        |           |             |
| **MapCollection**            | **100**   | **6,792.13 ns** | **139.230 ns** | **108.702 ns** |  **6.62** |    **0.29** | **3.6545** |   **15288 B** |        **1.96** |
| MapCollection_AutoMapper | 100   | 1,027.45 ns |  57.450 ns |  44.853 ns |  1.00 |    0.06 | 1.8616 |    7792 B |        1.00 |
