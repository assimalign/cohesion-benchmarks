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
| MapExisting            | 25.00 ns | 0.343 ns | 0.268 ns |  0.49 |    0.01 |      - |         - |          NA |
| MapExisting_AutoMapper | 51.49 ns | 1.136 ns | 0.887 ns |  1.00 |    0.02 |      - |         - |          NA |
|                        |          |          |          |       |         |        |           |             |
| MapRuntime             | 37.28 ns | 0.368 ns | 0.287 ns |  0.62 |    0.02 | 0.0134 |      56 B |        1.00 |
| MapRuntime_AutoMapper  | 60.38 ns | 2.379 ns | 1.857 ns |  1.00 |    0.04 | 0.0134 |      56 B |        1.00 |
