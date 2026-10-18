```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=20  
LaunchCount=1  WarmupCount=5  

```
| Method       | Header | Mean      | Error     | StdDev    | Gen0   | Allocated |
|------------- |------- |----------:|----------:|----------:|-------:|----------:|
| **ServeRequest** | **bearer** | **34.127 μs** | **1.7189 μs** | **1.9795 μs** | **5.1880** |  **21.39 KB** |
| **ServeRequest** | **cookie** | **49.699 μs** | **2.0248 μs** | **2.3318 μs** | **6.7139** |  **27.63 KB** |
| **ServeRequest** | **none**   |  **5.842 μs** | **0.2911 μs** | **0.3352 μs** | **2.3117** |   **9.47 KB** |
