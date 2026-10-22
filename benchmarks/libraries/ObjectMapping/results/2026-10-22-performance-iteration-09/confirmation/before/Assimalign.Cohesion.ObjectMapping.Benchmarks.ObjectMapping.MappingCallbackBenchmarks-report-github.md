```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                 | Mode              | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |------------------ |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| **MapCallback**            | **ContextCallback**   | **25.75 ns** | **0.571 ns** | **0.446 ns** |  **0.48** |    **0.01** | **0.0210** |      **88 B** |          **NA** |
| MapCallback_AutoMapper | ContextCallback   | 53.25 ns | 0.834 ns | 0.603 ns |  1.00 |    0.02 |      - |         - |          NA |
|                        |                   |          |          |          |       |         |        |           |             |
| **MapCallback**            | **CustomProfile**     | **41.85 ns** | **2.890 ns** | **2.257 ns** |  **0.76** |    **0.04** | **0.0191** |      **80 B** |          **NA** |
| MapCallback_AutoMapper | CustomProfile     | 54.98 ns | 0.868 ns | 0.677 ns |  1.00 |    0.02 |      - |         - |          NA |
|                        |                   |          |          |          |       |         |        |           |             |
| **MapCallback**            | **OverriddenProfile** | **39.39 ns** | **1.149 ns** | **0.897 ns** |  **0.74** |    **0.02** | **0.0191** |      **80 B** |          **NA** |
| MapCallback_AutoMapper | OverriddenProfile | 53.15 ns | 0.464 ns | 0.335 ns |  1.00 |    0.01 |      - |         - |          NA |
|                        |                   |          |          |          |       |         |        |           |             |
| **MapCallback**            | **TypedCallback**     | **28.80 ns** | **0.896 ns** | **0.700 ns** |  **0.55** |    **0.01** | **0.0210** |      **88 B** |          **NA** |
| MapCallback_AutoMapper | TypedCallback     | 52.60 ns | 0.831 ns | 0.649 ns |  1.00 |    0.02 |      - |         - |          NA |
