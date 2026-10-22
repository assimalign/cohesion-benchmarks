```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method               | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| MapFlat_AutoMapper   |   403.8 ns |  28.21 ns |  22.03 ns |  1.00 |    0.07 | 0.0134 |      56 B |        1.00 |
| MapFlat              |   141.5 ns |   3.03 ns |   2.37 ns |  0.35 |    0.02 | 0.0343 |     144 B |        2.57 |
|                      |            |           |           |       |         |        |           |             |
| MapNested_AutoMapper |   254.2 ns |  19.52 ns |  15.24 ns |  1.00 |    0.08 | 0.2275 |     952 B |        1.00 |
| MapNested            | 4,751.7 ns | 203.03 ns | 158.52 ns | 18.75 |    1.22 | 0.5417 |    2288 B |        2.40 |
