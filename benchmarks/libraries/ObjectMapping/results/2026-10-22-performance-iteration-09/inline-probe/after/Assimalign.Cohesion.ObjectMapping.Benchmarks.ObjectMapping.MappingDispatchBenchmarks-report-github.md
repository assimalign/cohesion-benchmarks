```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=20  
LaunchCount=1  WarmupCount=5  

```
| Method                | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| MapRuntime            | 327.8 ns | 16.88 ns | 19.44 ns |  0.90 |    0.07 | 0.0134 |      56 B |        1.00 |
| MapRuntime_AutoMapper | 364.5 ns | 18.63 ns | 21.45 ns |  1.00 |    0.08 | 0.0134 |      56 B |        1.00 |
