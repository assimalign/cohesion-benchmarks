```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method               | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| MapFlat_AutoMapper   |   291.85 ns |  6.367 ns |  4.604 ns |  1.00 |    0.02 | 0.0134 |      56 B |        1.00 |
| MapFlat              |    85.55 ns |  1.591 ns |  1.242 ns |  0.29 |    0.01 | 0.0134 |      56 B |        1.00 |
|                      |             |           |           |       |         |        |           |             |
| MapNested_AutoMapper |   216.07 ns |  4.188 ns |  3.269 ns |  1.00 |    0.02 | 0.2275 |     952 B |        1.00 |
| MapNested            | 2,886.46 ns | 82.706 ns | 64.571 ns | 13.36 |    0.35 | 0.2251 |     952 B |        1.00 |
