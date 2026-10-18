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
| Method        | Mean     | Error     | StdDev    | Gen0   | Allocated |
|-------------- |---------:|----------:|----------:|-------:|----------:|
| C_Own297Bytes | 3.920 μs | 0.0072 μs | 0.0104 μs | 0.0610 |     656 B |
| C_Asp137Bytes | 3.880 μs | 0.0049 μs | 0.0072 μs | 0.0458 |     496 B |
