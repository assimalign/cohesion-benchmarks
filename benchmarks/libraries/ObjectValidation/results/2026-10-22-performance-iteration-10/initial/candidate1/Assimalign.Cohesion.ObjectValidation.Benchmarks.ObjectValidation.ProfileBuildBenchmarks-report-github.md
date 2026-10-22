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
| **Build**                  | **False**             | **2.048 μs** | **0.0432 μs** | **0.0192 μs** |  **1.17** |    **0.01** | **1.4496** |   **5.93 KB** |        **0.81** |
| Build_FluentValidation | False             | 1.754 μs | 0.0119 μs | 0.0043 μs |  1.00 |    0.00 | 1.7853 |    7.3 KB |        1.00 |
|                        |                   |          |           |           |       |         |        |           |             |
| **Build**                  | **True**              | **4.087 μs** | **0.1451 μs** | **0.0644 μs** |  **1.08** |    **0.02** | **2.6093** |  **10.66 KB** |        **0.75** |
| Build_FluentValidation | True              | 3.793 μs | 0.1026 μs | 0.0455 μs |  1.00 |    0.02 | 3.4981 |   14.3 KB |        1.00 |
