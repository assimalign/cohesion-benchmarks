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
| Method     | Shape        | Mean        | Error     | StdDev    | Gen0   | Gen1   | Gen2   | Allocated |
|----------- |------------- |------------:|----------:|----------:|-------:|-------:|-------:|----------:|
| **C_ReadLine** | **bearer**       | **24,049.6 ns** | **550.07 ns** | **806.29 ns** | **1.1902** | **0.0305** | **0.0305** |         **-** |
| **C_ReadLine** | **cookie**       | **37,165.0 ns** | **168.54 ns** | **230.71 ns** | **1.7090** |      **-** |      **-** |   **17360 B** |
| **C_ReadLine** | **inert-bearer** | **24,012.6 ns** | **150.65 ns** | **225.48 ns** | **1.1597** | **0.0610** | **0.0610** |   **11336 B** |
| **C_ReadLine** | **tiny**         |    **574.5 ns** |   **5.44 ns** |   **7.98 ns** | **0.0553** |      **-** |      **-** |     **568 B** |
