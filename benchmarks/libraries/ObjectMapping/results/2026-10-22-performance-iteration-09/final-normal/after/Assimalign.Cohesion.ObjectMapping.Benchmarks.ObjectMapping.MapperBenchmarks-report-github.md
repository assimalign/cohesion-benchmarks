```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method               | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| MapFlat_AutoMapper   |  54.47 ns |  1.346 ns | 0.973 ns |  1.00 |    0.02 | 0.0134 |      56 B |        1.00 |
| MapFlat              |  47.73 ns |  0.303 ns | 0.219 ns |  0.88 |    0.02 | 0.0134 |      56 B |        1.00 |
|                      |           |           |          |       |         |        |           |             |
| MapNested_AutoMapper | 194.45 ns |  4.031 ns | 2.666 ns |  1.00 |    0.02 | 0.2275 |     952 B |        1.00 |
| MapNested            | 557.93 ns | 12.821 ns | 9.270 ns |  2.87 |    0.06 | 0.2270 |     952 B |        1.00 |
