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
| MapFlat_AutoMapper   |  47.54 ns |  4.249 ns |  3.317 ns |  1.00 |    0.09 | 0.0134 |      56 B |        1.00 |
| MapFlat              |  50.79 ns |  1.702 ns |  1.329 ns |  1.07 |    0.07 | 0.0134 |      56 B |        1.00 |
|                      |           |           |           |       |         |        |           |             |
| MapNested_AutoMapper | 174.99 ns |  1.196 ns |  0.791 ns |  1.00 |    0.01 | 0.2275 |     952 B |        1.00 |
| MapNested            | 571.36 ns | 26.319 ns | 20.548 ns |  3.27 |    0.11 | 0.2270 |     952 B |        1.00 |
