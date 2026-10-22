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
| **MapCollection**            | **1**     |    **82.54 ns** |   **3.104 ns** |   **2.423 ns** |  **1.12** |    **0.05** | **0.0362** |     **152 B** |        **1.06** |
| MapCollection_AutoMapper | 1     |    73.80 ns |   3.387 ns |   2.240 ns |  1.00 |    0.04 | 0.0343 |     144 B |        1.00 |
|                          |       |             |            |            |       |         |        |           |             |
| **MapCollection**            | **100**   | **4,977.86 ns** | **366.945 ns** | **286.486 ns** |  **4.49** |    **0.30** | **1.5488** |    **6488 B** |        **0.83** |
| MapCollection_AutoMapper | 100   | 1,110.98 ns |  53.587 ns |  41.837 ns |  1.00 |    0.05 | 1.8616 |    7792 B |        1.00 |
