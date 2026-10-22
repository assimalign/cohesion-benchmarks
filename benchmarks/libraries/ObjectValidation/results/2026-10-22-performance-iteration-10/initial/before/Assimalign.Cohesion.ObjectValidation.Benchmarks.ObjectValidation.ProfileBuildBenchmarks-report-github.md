```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method                 | IncludeCollection | Mean     | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |------------------ |---------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Build**                  | **False**             | **2.573 μs** | **0.2020 μs** | **0.1057 μs** |  **1.19** |    **0.06** | **1.4610** |   **5.97 KB** |        **0.82** |
| Build_FluentValidation | False             | 2.162 μs | 0.1367 μs | 0.0715 μs |  1.00 |    0.04 | 1.7853 |    7.3 KB |        1.00 |
|                        |                   |          |           |           |       |         |        |           |             |
| **Build**                  | **True**              | **5.090 μs** | **0.1821 μs** | **0.0809 μs** |  **1.11** |    **0.02** | **2.6169** |   **10.7 KB** |        **0.75** |
| Build_FluentValidation | True              | 4.588 μs | 0.1495 μs | 0.0782 μs |  1.00 |    0.02 | 3.4943 |   14.3 KB |        1.00 |
