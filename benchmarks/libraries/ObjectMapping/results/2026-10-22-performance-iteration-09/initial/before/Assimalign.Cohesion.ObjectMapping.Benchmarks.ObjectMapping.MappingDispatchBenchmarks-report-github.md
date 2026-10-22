```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=20  
LaunchCount=1  WarmupCount=5  

```
| Method                 | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| MapExisting            | 61.92 ns | 0.926 ns | 1.029 ns |  1.16 |    0.03 | 0.0210 |      88 B |          NA |
| MapExisting_AutoMapper | 53.43 ns | 0.698 ns | 0.804 ns |  1.00 |    0.02 |      - |         - |          NA |
|                        |          |          |          |       |         |        |           |             |
| MapRuntime             | 68.29 ns | 2.223 ns | 2.560 ns |  1.23 |    0.10 | 0.0343 |     144 B |        2.57 |
| MapRuntime_AutoMapper  | 56.03 ns | 4.029 ns | 4.639 ns |  1.01 |    0.11 | 0.0134 |      56 B |        1.00 |
