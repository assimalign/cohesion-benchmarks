```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                 | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| MapExisting            | 66.92 ns | 2.311 ns | 1.804 ns |  1.18 |    0.04 | 0.0210 |      88 B |          NA |
| MapExisting_AutoMapper | 56.65 ns | 1.246 ns | 0.973 ns |  1.00 |    0.02 |      - |         - |          NA |
|                        |          |          |          |       |         |        |           |             |
| MapRuntime             | 74.09 ns | 2.439 ns | 1.904 ns |  1.27 |    0.03 | 0.0343 |     144 B |        2.57 |
| MapRuntime_AutoMapper  | 58.50 ns | 0.705 ns | 0.466 ns |  1.00 |    0.01 | 0.0134 |      56 B |        1.00 |
