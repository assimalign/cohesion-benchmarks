```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                 | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| MapExisting            |  32.28 ns | 0.617 ns | 0.482 ns |  0.56 |    0.01 |      - |         - |          NA |
| MapExisting_AutoMapper |  57.27 ns | 0.862 ns | 0.673 ns |  1.00 |    0.02 |      - |         - |          NA |
|                        |           |          |          |       |         |        |           |             |
| MapRuntime             | 126.64 ns | 2.017 ns | 1.575 ns |  2.00 |    0.05 | 0.0134 |      56 B |        1.00 |
| MapRuntime_AutoMapper  |  63.48 ns | 1.906 ns | 1.488 ns |  1.00 |    0.03 | 0.0134 |      56 B |        1.00 |
