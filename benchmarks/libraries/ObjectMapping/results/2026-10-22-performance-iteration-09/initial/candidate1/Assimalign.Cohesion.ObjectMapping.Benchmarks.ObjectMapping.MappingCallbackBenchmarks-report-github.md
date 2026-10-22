```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method                 | Mode              | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |------------------ |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| **MapCallback**            | **ContextCallback**   | **18.33 ns** | **1.022 ns** | **0.534 ns** |  **0.40** |    **0.03** | **0.0115** |      **48 B** |          **NA** |
| MapCallback_AutoMapper | ContextCallback   | 46.17 ns | 6.687 ns | 3.498 ns |  1.00 |    0.10 |      - |         - |          NA |
|                        |                   |          |          |          |       |         |        |           |             |
| **MapCallback**            | **CustomProfile**     | **38.17 ns** | **4.139 ns** | **2.165 ns** |  **0.71** |    **0.04** | **0.0191** |      **80 B** |          **NA** |
| MapCallback_AutoMapper | CustomProfile     | 53.64 ns | 2.610 ns | 1.365 ns |  1.00 |    0.03 |      - |         - |          NA |
|                        |                   |          |          |          |       |         |        |           |             |
| **MapCallback**            | **OverriddenProfile** | **40.78 ns** | **3.945 ns** | **2.063 ns** |  **0.72** |    **0.04** | **0.0191** |      **80 B** |          **NA** |
| MapCallback_AutoMapper | OverriddenProfile | 56.26 ns | 1.648 ns | 0.862 ns |  1.00 |    0.02 |      - |         - |          NA |
|                        |                   |          |          |          |       |         |        |           |             |
| **MapCallback**            | **TypedCallback**     | **25.28 ns** | **0.549 ns** | **0.287 ns** |  **0.48** |    **0.01** | **0.0115** |      **48 B** |          **NA** |
| MapCallback_AutoMapper | TypedCallback     | 53.09 ns | 1.653 ns | 0.734 ns |  1.00 |    0.02 |      - |         - |          NA |
