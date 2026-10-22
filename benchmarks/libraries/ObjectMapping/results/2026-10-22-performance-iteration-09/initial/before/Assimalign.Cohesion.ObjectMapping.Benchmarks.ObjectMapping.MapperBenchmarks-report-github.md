```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=20  
LaunchCount=1  WarmupCount=5  

```
| Method               | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| MapFlat_AutoMapper   |    52.96 ns |  1.385 ns |  1.595 ns |  1.00 |    0.04 | 0.0134 |      56 B |        1.00 |
| MapFlat              |    99.24 ns |  1.945 ns |  2.240 ns |  1.88 |    0.07 | 0.0343 |     144 B |        2.57 |
|                      |             |           |           |       |         |        |           |             |
| MapNested_AutoMapper |   226.16 ns |  3.363 ns |  3.873 ns |  1.00 |    0.02 | 0.2275 |     952 B |        1.00 |
| MapNested            | 1,038.38 ns | 18.279 ns | 21.051 ns |  4.59 |    0.12 | 0.5455 |    2288 B |        2.40 |
