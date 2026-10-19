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
| Method     | Shape        | Mean        | Error     | StdDev    | Median      | Gen0   | Allocated |
|----------- |------------- |------------:|----------:|----------:|------------:|-------:|----------:|
| **C_ReadLine** | **bearer**       | **23,825.7 ns** | **250.82 ns** | **359.72 ns** | **23,710.6 ns** | **1.0986** |   **11336 B** |
| **C_ReadLine** | **cookie**       | **37,304.2 ns** | **284.09 ns** | **407.43 ns** | **37,212.8 ns** | **2.1973** |   **17360 B** |
| **C_ReadLine** | **inert-bearer** | **23,648.2 ns** | **136.59 ns** | **204.44 ns** | **23,530.1 ns** | **1.0986** |   **11336 B** |
| **C_ReadLine** | **tiny**         |    **582.8 ns** |   **7.81 ns** |  **11.21 ns** |    **583.0 ns** | **0.0553** |     **568 B** |
