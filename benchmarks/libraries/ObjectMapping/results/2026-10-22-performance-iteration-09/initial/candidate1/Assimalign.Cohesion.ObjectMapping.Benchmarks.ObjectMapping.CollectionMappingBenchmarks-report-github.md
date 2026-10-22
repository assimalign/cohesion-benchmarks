```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method                   | Count | Mean        | Error      | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |------ |------------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MapCollection**            | **1**     |    **58.93 ns** |   **3.929 ns** |  **2.055 ns** |  **1.00** |    **0.04** | **0.0362** |     **152 B** |        **1.06** |
| MapCollection_AutoMapper | 1     |    59.17 ns |   2.988 ns |  1.327 ns |  1.00 |    0.03 | 0.0343 |     144 B |        1.00 |
|                          |       |             |            |           |       |         |        |           |             |
| **MapCollection**            | **100**   | **3,892.89 ns** | **121.220 ns** | **53.822 ns** |  **4.28** |    **0.07** | **1.5488** |    **6488 B** |        **0.83** |
| MapCollection_AutoMapper | 100   |   909.59 ns |  24.114 ns | 10.707 ns |  1.00 |    0.02 | 1.8625 |    7792 B |        1.00 |
