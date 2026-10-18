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
| Method        | Mean       | Error    | StdDev   | Median     | Gen0   | Gen1   | Gen2   | Allocated |
|-------------- |-----------:|---------:|---------:|-----------:|-------:|-------:|-------:|----------:|
| C_Deserialize |   247.4 ns |  7.69 ns | 11.03 ns |   241.2 ns | 0.0978 |      - |      - |   1.09 KB |
| C_Format      | 4,216.7 ns |  8.20 ns | 10.66 ns | 4,214.9 ns | 0.2213 | 0.0076 | 0.0076 |  13.07 KB |
| A_Format      | 4,091.1 ns | 13.01 ns | 18.66 ns | 4,087.8 ns | 0.1984 |      - |      - |   2.02 KB |
| C_Service     | 4,749.7 ns | 54.82 ns | 76.84 ns | 4,726.3 ns | 0.4196 | 0.0076 | 0.0076 |  30.09 KB |
