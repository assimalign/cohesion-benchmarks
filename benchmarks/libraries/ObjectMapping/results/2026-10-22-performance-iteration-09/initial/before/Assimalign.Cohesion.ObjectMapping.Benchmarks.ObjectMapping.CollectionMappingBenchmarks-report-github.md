```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=20  
LaunchCount=1  WarmupCount=5  

```
| Method                   | Count | Mean        | Error      | StdDev     | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------ |------------:|-----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| **MapCollection**            | **1**     |    **98.06 ns** |   **3.832 ns** |   **4.413 ns** |  **1.44** |    **0.07** | **0.0573** |     **240 B** |        **1.67** |
| MapCollection_AutoMapper | 1     |    68.07 ns |   1.566 ns |   1.804 ns |  1.00 |    0.04 | 0.0343 |     144 B |        1.00 |
|                          |       |             |            |            |       |         |        |           |             |
| **MapCollection**            | **100**   | **6,894.95 ns** | **114.544 ns** | **131.909 ns** |  **6.53** |    **0.33** | **3.6545** |   **15288 B** |        **1.96** |
| MapCollection_AutoMapper | 100   | 1,057.99 ns |  44.556 ns |  51.311 ns |  1.00 |    0.07 | 1.8616 |    7792 B |        1.00 |
