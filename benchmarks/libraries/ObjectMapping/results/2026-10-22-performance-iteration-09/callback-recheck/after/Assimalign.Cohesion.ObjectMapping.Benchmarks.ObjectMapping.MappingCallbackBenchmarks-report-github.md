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
| **MapCallback**            | **ContextCallback**   | **17.148 ns** | **0.4029 ns** | **0.3145 ns** |  **0.36** |    **0.01** | **0.0115** |      **48 B** |          **NA** |
| MapCallback_AutoMapper | ContextCallback   | 47.899 ns | 0.6847 ns | 0.4529 ns |  1.00 |    0.01 |      - |         - |          NA |
|                        |                   |           |           |           |       |         |        |           |             |
| **MapCallback**            | **CustomProfile**     | **31.549 ns** | **0.8038 ns** | **0.5812 ns** |  **0.66** |    **0.01** | **0.0191** |      **80 B** |          **NA** |
| MapCallback_AutoMapper | CustomProfile     | 47.843 ns | 0.8735 ns | 0.5778 ns |  1.00 |    0.02 |      - |         - |          NA |
|                        |                   |           |           |           |       |         |        |           |             |
| **MapCallback**            | **OverriddenProfile** | **35.572 ns** | **0.9619 ns** | **0.7510 ns** |  **0.71** |    **0.05** | **0.0191** |      **80 B** |          **NA** |
| MapCallback_AutoMapper | OverriddenProfile | 50.524 ns | 4.4732 ns | 3.4924 ns |  1.00 |    0.09 |      - |         - |          NA |
|                        |                   |           |           |           |       |         |        |           |             |
| **MapCallback**            | **TypedCallback**     |  **7.963 ns** | **0.1692 ns** | **0.1321 ns** |  **0.16** |    **0.01** |      **-** |         **-** |          **NA** |
| MapCallback_AutoMapper | TypedCallback     | 48.753 ns | 3.5292 ns | 2.7554 ns |  1.00 |    0.08 |      - |         - |          NA |
