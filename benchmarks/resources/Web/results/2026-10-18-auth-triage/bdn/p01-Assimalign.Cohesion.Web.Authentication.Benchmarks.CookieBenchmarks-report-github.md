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
| Method      | Mean       | Error    | StdDev   | Median     | Gen0   | Gen1   | Gen2   | Allocated |
|------------ |-----------:|---------:|---------:|-----------:|-------:|-------:|-------:|----------:|
| C_Unprotect |   368.8 ns |  1.35 ns |  1.85 ns |   368.7 ns | 0.0329 | 0.0005 | 0.0005 |    1756 B |
| A_Unprotect | 3,508.4 ns |  8.85 ns | 12.70 ns | 3,506.8 ns | 0.0534 |      - |      - |     560 B |
| C_Format    |   841.9 ns | 20.45 ns | 30.60 ns |   829.6 ns | 0.2365 |      - |      - |    2408 B |
| C_Service   | 1,397.0 ns | 32.65 ns | 48.87 ns | 1,366.8 ns | 0.5512 | 0.0019 |      - |    6152 B |
