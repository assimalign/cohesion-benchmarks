```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                 | IncludeCollection | Mean     | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |------------------ |---------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **Build**                  | **False**             | **2.260 μs** | **0.0974 μs** | **0.0704 μs** |  **1.24** |    **0.04** | **1.4610** |   **5.97 KB** |        **0.82** |
| Build_FluentValidation | False             | 1.816 μs | 0.0490 μs | 0.0382 μs |  1.00 |    0.03 | 1.7853 |    7.3 KB |        1.00 |
|                        |                   |          |           |           |       |         |        |           |             |
| **Build**                  | **True**              | **4.110 μs** | **0.1799 μs** | **0.1190 μs** |  **1.12** |    **0.04** | **2.6169** |   **10.7 KB** |        **0.75** |
| Build_FluentValidation | True              | 3.665 μs | 0.1015 μs | 0.0672 μs |  1.00 |    0.02 | 3.4981 |   14.3 KB |        1.00 |
