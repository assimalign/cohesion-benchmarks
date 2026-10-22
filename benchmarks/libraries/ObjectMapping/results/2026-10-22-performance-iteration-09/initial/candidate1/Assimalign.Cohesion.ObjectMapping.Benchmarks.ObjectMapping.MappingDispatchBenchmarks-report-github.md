```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method                 | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| MapExisting            | 24.51 ns | 0.207 ns | 0.092 ns |  0.55 |    0.00 |      - |         - |          NA |
| MapExisting_AutoMapper | 44.21 ns | 0.090 ns | 0.032 ns |  1.00 |    0.00 |      - |         - |          NA |
|                        |          |          |          |       |         |        |           |             |
| MapRuntime             | 36.86 ns | 0.182 ns | 0.065 ns |  0.79 |    0.05 | 0.0134 |      56 B |        1.00 |
| MapRuntime_AutoMapper  | 46.93 ns | 6.185 ns | 3.235 ns |  1.00 |    0.09 | 0.0134 |      56 B |        1.00 |
