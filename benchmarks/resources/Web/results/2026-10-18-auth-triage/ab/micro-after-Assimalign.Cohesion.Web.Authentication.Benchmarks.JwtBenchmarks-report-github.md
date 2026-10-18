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
| Method         | Mean       | Error    | StdDev   | Median     | Gen0   | Gen1   | Allocated |
|--------------- |-----------:|---------:|---------:|-----------:|-------:|-------:|----------:|
| C_MapPrincipal |   300.9 ns |  6.70 ns |  9.40 ns |   297.4 ns | 0.1144 |      - |   1.19 KB |
| C_Service      | 3,213.3 ns | 63.50 ns | 95.05 ns | 3,150.2 ns | 1.0109 | 0.0076 |   10.4 KB |
