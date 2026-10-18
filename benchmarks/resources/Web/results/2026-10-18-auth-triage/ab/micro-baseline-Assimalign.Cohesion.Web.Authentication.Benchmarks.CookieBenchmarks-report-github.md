```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host]          : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a
  Medium-Isolated : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=Medium-Isolated  Affinity=1111  Server=True  
Arguments=/p:CohesionRoot=<main checkout>  IterationCount=15  LaunchCount=2  
WarmupCount=10  

```
| Method        | Mean       | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|-------------- |-----------:|---------:|---------:|-------:|-------:|----------:|
| C_Deserialize |   384.7 ns | 15.53 ns | 23.24 ns | 0.1531 |      - |   1.67 KB |
| C_Service     | 5,088.5 ns | 53.53 ns | 71.46 ns | 1.2665 | 0.0153 |   6.33 KB |
