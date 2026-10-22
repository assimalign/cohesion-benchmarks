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
| MapExisting            | 57.93 ns | 1.346 ns | 1.050 ns |  1.09 |    0.02 | 0.0210 |      88 B |          NA |
| MapExisting_AutoMapper | 52.99 ns | 0.850 ns | 0.663 ns |  1.00 |    0.02 |      - |         - |          NA |
|                        |          |          |          |       |         |        |           |             |
| MapRuntime             | 76.97 ns | 1.868 ns | 1.351 ns |  1.31 |    0.03 | 0.0343 |     144 B |        2.57 |
| MapRuntime_AutoMapper  | 58.56 ns | 0.995 ns | 0.720 ns |  1.00 |    0.02 | 0.0134 |      56 B |        1.00 |
