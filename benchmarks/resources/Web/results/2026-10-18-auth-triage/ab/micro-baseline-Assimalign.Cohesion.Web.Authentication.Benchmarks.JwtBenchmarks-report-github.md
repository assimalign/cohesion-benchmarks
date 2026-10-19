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
| Method         | Mean       | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|--------------- |-----------:|---------:|---------:|-------:|-------:|----------:|
| C_MapPrincipal |   466.5 ns | 11.28 ns | 16.18 ns | 0.1812 |      - |   1.97 KB |
| C_Service      | 3,412.9 ns | 63.72 ns | 95.37 ns | 1.0872 | 0.0076 |  11.18 KB |
