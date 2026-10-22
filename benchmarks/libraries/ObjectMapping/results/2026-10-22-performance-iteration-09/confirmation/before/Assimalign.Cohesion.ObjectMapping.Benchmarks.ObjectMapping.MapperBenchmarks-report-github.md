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
| MapFlat_AutoMapper   |  49.48 ns |  3.530 ns |  2.756 ns |  1.00 |    0.07 | 0.0134 |      56 B |        1.00 |
| MapFlat              |  91.98 ns |  2.224 ns |  1.737 ns |  1.86 |    0.10 | 0.0343 |     144 B |        2.57 |
|                      |           |           |           |       |         |        |           |             |
| MapNested_AutoMapper | 203.86 ns |  6.169 ns |  4.081 ns |  1.00 |    0.03 | 0.2275 |     952 B |        1.00 |
| MapNested            | 961.64 ns | 17.501 ns | 13.664 ns |  4.72 |    0.11 | 0.5465 |    2288 B |        2.40 |
