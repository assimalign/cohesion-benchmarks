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
| Method              | Mean       | Error    | StdDev    | Gen0   | Gen1   | Allocated |
|-------------------- |-----------:|---------:|----------:|-------:|-------:|----------:|
| C_ValidateLibrary   | 2,954.6 ns | 79.74 ns | 116.88 ns | 0.7668 |      - |   7.63 KB |
| A_ValidatePrincipal | 2,546.4 ns | 57.14 ns |  81.95 ns | 0.6256 |      - |   6.34 KB |
| C_MapPrincipal      |   302.9 ns |  7.88 ns |  11.31 ns | 0.1149 |      - |   1.19 KB |
| C_Service           | 3,249.5 ns | 57.33 ns |  84.03 ns | 1.0147 | 0.0076 |   10.4 KB |
