```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host]           : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a
  Default-Isolated : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=Default-Isolated  Affinity=1111  Server=True  
Arguments=/p:CohesionRoot=<feature/web-auth-perf checkout>  

```
| Method       | Mean      | Error    | StdDev   | Median    | Gen0   | Gen1   | Gen2   | Allocated |
|------------- |----------:|---------:|---------:|----------:|-------:|-------:|-------:|----------:|
| C_RolePolicy |  44.44 ns | 0.863 ns | 0.807 ns |  44.27 ns | 0.0089 | 0.0001 | 0.0001 |         - |
| A_RolePolicy | 208.45 ns | 4.170 ns | 6.368 ns | 205.10 ns | 0.0679 |      - |      - |     704 B |
