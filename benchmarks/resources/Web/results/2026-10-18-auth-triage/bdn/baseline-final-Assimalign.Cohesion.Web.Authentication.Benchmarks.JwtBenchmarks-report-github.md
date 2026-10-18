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
| Method              | Mean       | Error    | StdDev   | Median     | Gen0   | Gen1   | Allocated |
|-------------------- |-----------:|---------:|---------:|-----------:|-------:|-------:|----------:|
| C_ValidateLibrary   | 3,046.7 ns | 47.78 ns | 66.98 ns | 3,018.1 ns | 0.8507 | 0.0038 |    8608 B |
| A_ValidatePrincipal | 2,480.2 ns | 42.39 ns | 59.43 ns | 2,451.8 ns | 0.6218 | 0.0038 |    6496 B |
| C_SplitDecode       |   157.0 ns |  4.71 ns |  6.90 ns |   154.2 ns | 0.0770 | 0.0017 |     960 B |
| C_SignatureBuffers  |   113.9 ns | 10.30 ns | 15.10 ns |   106.9 ns | 0.1314 |      - |    1328 B |
| C_Rules             |   134.2 ns |  2.34 ns |  3.43 ns |   132.4 ns | 0.0346 |      - |     352 B |
