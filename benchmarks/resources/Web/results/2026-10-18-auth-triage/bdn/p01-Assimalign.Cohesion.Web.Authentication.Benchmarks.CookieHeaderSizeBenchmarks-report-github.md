```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host]          : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a
  Medium-Isolated : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=Medium-Isolated  Affinity=1111  Server=True  
Arguments=/p:CohesionRoot=<feature/web-auth-perf checkout>  IterationCount=15  LaunchCount=2  
WarmupCount=10  

```
| Method          | Mean     | Error    | StdDev   | Gen0   | Gen1   | Gen2   | Allocated |
|---------------- |---------:|---------:|---------:|-------:|-------:|-------:|----------:|
| C_CohCookieLine | 37.46 μs | 0.329 μs | 0.450 μs | 1.7090 |      - |      - |   17360 B |
| C_AspCookieLine | 25.90 μs | 0.234 μs | 0.350 μs | 1.2512 | 0.0305 | 0.0305 |         - |
