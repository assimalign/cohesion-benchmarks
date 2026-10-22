```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                 | Mode              | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |------------------ |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **MapCallback**            | **ContextCallback**   |  **22.132 ns** | **2.2982 ns** | **1.7943 ns** |  **0.21** |    **0.02** | **0.0115** |      **48 B** |          **NA** |
| MapCallback_AutoMapper | ContextCallback   | 104.569 ns | 2.3689 ns | 1.7129 ns |  1.00 |    0.02 |      - |         - |          NA |
|                        |                   |            |           |           |       |         |        |           |             |
| **MapCallback**            | **CustomProfile**     |  **51.476 ns** | **2.4294 ns** | **1.8967 ns** |  **0.87** |    **0.04** | **0.0191** |      **80 B** |          **NA** |
| MapCallback_AutoMapper | CustomProfile     |  59.197 ns | 2.3978 ns | 1.7338 ns |  1.00 |    0.04 |      - |         - |          NA |
|                        |                   |            |           |           |       |         |        |           |             |
| **MapCallback**            | **OverriddenProfile** |  **35.968 ns** | **0.7850 ns** | **0.6129 ns** |  **0.67** |    **0.02** | **0.0191** |      **80 B** |          **NA** |
| MapCallback_AutoMapper | OverriddenProfile |  53.442 ns | 1.8459 ns | 1.4412 ns |  1.00 |    0.04 |      - |         - |          NA |
|                        |                   |            |           |           |       |         |        |           |             |
| **MapCallback**            | **TypedCallback**     |   **8.089 ns** | **0.0993 ns** | **0.0776 ns** |  **0.16** |    **0.01** |      **-** |         **-** |          **NA** |
| MapCallback_AutoMapper | TypedCallback     |  51.847 ns | 2.8145 ns | 2.1973 ns |  1.00 |    0.06 |      - |         - |          NA |
