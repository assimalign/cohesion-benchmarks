```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                 | Mode              | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |------------------ |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MapCallback**            | **ContextCallback**   | **17.652 ns** | **0.1797 ns** | **0.1189 ns** |  **0.38** |    **0.03** | **0.0115** |      **48 B** |          **NA** |
| MapCallback_AutoMapper | ContextCallback   | 47.012 ns | 4.3366 ns | 3.3857 ns |  1.00 |    0.10 |      - |         - |          NA |
|                        |                   |           |           |           |       |         |        |           |             |
| **MapCallback**            | **CustomProfile**     | **31.430 ns** | **0.1894 ns** | **0.1253 ns** |  **0.68** |    **0.03** | **0.0191** |      **80 B** |          **NA** |
| MapCallback_AutoMapper | CustomProfile     | 46.442 ns | 3.0490 ns | 2.3805 ns |  1.00 |    0.07 |      - |         - |          NA |
|                        |                   |           |           |           |       |         |        |           |             |
| **MapCallback**            | **OverriddenProfile** | **32.680 ns** | **1.0831 ns** | **0.7164 ns** |  **0.65** |    **0.03** | **0.0191** |      **80 B** |          **NA** |
| MapCallback_AutoMapper | OverriddenProfile | 50.130 ns | 3.1711 ns | 2.4758 ns |  1.00 |    0.07 |      - |         - |          NA |
|                        |                   |           |           |           |       |         |        |           |             |
| **MapCallback**            | **TypedCallback**     |  **7.922 ns** | **0.1502 ns** | **0.1086 ns** |  **0.14** |    **0.01** |      **-** |         **-** |          **NA** |
| MapCallback_AutoMapper | TypedCallback     | 58.120 ns | 4.3242 ns | 3.3760 ns |  1.00 |    0.08 |      - |         - |          NA |
