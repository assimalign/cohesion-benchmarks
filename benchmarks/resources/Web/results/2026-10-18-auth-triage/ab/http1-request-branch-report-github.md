```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=20  
LaunchCount=1  WarmupCount=5  

```
| Method       | Header | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------- |------- |---------:|----------:|----------:|-------:|----------:|
| **ServeRequest** | **bearer** | **2.033 μs** | **0.1176 μs** | **0.1354 μs** | **1.1406** |   **4.66 KB** |
| **ServeRequest** | **cookie** | **1.823 μs** | **0.0390 μs** | **0.0450 μs** | **1.3123** |   **5.37 KB** |
| **ServeRequest** | **none**   | **1.404 μs** | **0.0128 μs** | **0.0126 μs** | **0.7820** |    **3.2 KB** |
