```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method               | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| MapFlat_AutoMapper   |  46.40 ns |  3.763 ns |  1.968 ns |  1.00 |    0.06 | 0.0134 |      56 B |        1.00 |
| MapFlat              |  52.90 ns |  3.003 ns |  1.571 ns |  1.14 |    0.05 | 0.0134 |      56 B |        1.00 |
|                      |           |           |           |       |         |        |           |             |
| MapNested_AutoMapper | 190.55 ns | 19.314 ns | 10.101 ns |  1.00 |    0.07 | 0.2275 |     952 B |        1.00 |
| MapNested            | 616.77 ns | 16.664 ns |  7.399 ns |  3.24 |    0.17 | 0.2480 |    1040 B |        1.09 |
