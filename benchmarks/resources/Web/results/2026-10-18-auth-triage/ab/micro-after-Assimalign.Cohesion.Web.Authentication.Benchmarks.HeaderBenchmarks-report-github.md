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
| Method     | Shape        | Mean     | Error    | StdDev   | Gen0   | Gen1   | Gen2   | Allocated |
|----------- |------------- |---------:|---------:|---------:|-------:|-------:|-------:|----------:|
| **C_ReadLine** | **bearer**       | **205.8 ns** |  **7.27 ns** | **10.65 ns** | **0.0625** |      **-** |      **-** |     **632 B** |
| **C_ReadLine** | **cookie**       | **246.2 ns** | **11.20 ns** | **16.77 ns** | **0.0834** |      **-** |      **-** |     **992 B** |
| **C_ReadLine** | **inert-bearer** | **210.5 ns** | **10.11 ns** | **15.13 ns** | **0.0625** |      **-** |      **-** |     **632 B** |
| **C_ReadLine** | **tiny**         | **154.0 ns** |  **0.36 ns** |  **0.52 ns** | **0.0033** | **0.0002** | **0.0002** |         **-** |
