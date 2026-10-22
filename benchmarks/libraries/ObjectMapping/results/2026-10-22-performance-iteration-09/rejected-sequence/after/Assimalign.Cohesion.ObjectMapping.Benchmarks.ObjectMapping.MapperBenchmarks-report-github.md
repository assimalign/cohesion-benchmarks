```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method               | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| MapFlat_AutoMapper   |  82.80 ns |  3.021 ns |  2.184 ns |  1.00 |    0.04 | 0.0134 |      56 B |        1.00 |
| MapFlat              |  96.31 ns |  4.655 ns |  3.634 ns |  1.16 |    0.05 | 0.0134 |      56 B |        1.00 |
|                      |           |           |           |       |         |        |           |             |
| MapNested_AutoMapper | 273.33 ns | 18.420 ns | 14.381 ns |  1.00 |    0.07 | 0.2275 |     952 B |        1.00 |
| MapNested            | 795.27 ns |  9.979 ns |  7.215 ns |  2.92 |    0.15 | 0.2270 |     952 B |        1.00 |
