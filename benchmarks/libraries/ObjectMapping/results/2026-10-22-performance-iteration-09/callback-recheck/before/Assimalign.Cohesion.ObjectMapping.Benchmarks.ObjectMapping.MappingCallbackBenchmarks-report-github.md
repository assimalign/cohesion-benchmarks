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
| **MapCallback**            | **ContextCallback**   | **30.60 ns** | **2.807 ns** | **2.192 ns** |  **0.51** |    **0.04** | **0.0210** |      **88 B** |          **NA** |
| MapCallback_AutoMapper | ContextCallback   | 60.60 ns | 3.907 ns | 2.825 ns |  1.00 |    0.06 |      - |         - |          NA |
|                        |                   |          |          |          |       |         |        |           |             |
| **MapCallback**            | **CustomProfile**     | **39.60 ns** | **2.074 ns** | **1.619 ns** |  **0.77** |    **0.03** | **0.0191** |      **80 B** |          **NA** |
| MapCallback_AutoMapper | CustomProfile     | 51.41 ns | 0.386 ns | 0.301 ns |  1.00 |    0.01 |      - |         - |          NA |
|                        |                   |          |          |          |       |         |        |           |             |
| **MapCallback**            | **OverriddenProfile** | **37.01 ns** | **1.532 ns** | **1.013 ns** |  **0.73** |    **0.03** | **0.0191** |      **80 B** |          **NA** |
| MapCallback_AutoMapper | OverriddenProfile | 51.08 ns | 1.722 ns | 1.344 ns |  1.00 |    0.04 |      - |         - |          NA |
|                        |                   |          |          |          |       |         |        |           |             |
| **MapCallback**            | **TypedCallback**     | **27.24 ns** | **0.175 ns** | **0.115 ns** |  **0.59** |    **0.01** | **0.0210** |      **88 B** |          **NA** |
| MapCallback_AutoMapper | TypedCallback     | 45.96 ns | 1.333 ns | 0.964 ns |  1.00 |    0.03 |      - |         - |          NA |
