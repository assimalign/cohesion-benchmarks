```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=20  
LaunchCount=1  WarmupCount=5  

```
| Method                     | Graph | Mean            | Error         | StdDev        | Ratio | RatioSD | Gen0    | Gen1    | Gen2   | Allocated | Alloc Ratio |
|--------------------------- |------ |----------------:|--------------:|--------------:|------:|--------:|--------:|--------:|-------:|----------:|------------:|
| **Configure**                  | **False** |        **99.47 ns** |      **1.602 ns** |      **1.845 ns** | **0.001** |    **0.00** |  **0.1109** |       **-** |      **-** |     **464 B** |        **0.01** |
| Configure_AutoMapper       | False |    84,847.97 ns |    985.768 ns |  1,135.211 ns | 1.000 |    0.02 |  9.1553 |       - |      - |   38762 B |        1.00 |
|                            |       |                 |               |               |       |         |         |         |        |           |             |
| **Configure**                  | **True**  |       **280.86 ns** |      **6.773 ns** |      **7.799 ns** | **0.002** |    **0.00** |  **0.2923** |       **-** |      **-** |    **1224 B** |        **0.01** |
| Configure_AutoMapper       | True  |   172,473.35 ns |  2,709.674 ns |  3,120.464 ns | 1.000 |    0.02 | 19.5313 |  0.2441 |      - |   82068 B |        1.00 |
|                            |       |                 |               |               |       |         |         |         |        |           |             |
| **ConfigureAndMap**            | **False** |       **155.71 ns** |      **4.143 ns** |      **4.771 ns** | **0.001** |    **0.00** |  **0.1452** |       **-** |      **-** |     **608 B** |       **0.009** |
| ConfigureAndMap_AutoMapper | False |   262,780.40 ns | 16,284.356 ns | 18,100.018 ns | 1.004 |    0.09 | 16.1133 | 15.1367 | 0.4883 |   68552 B |       1.000 |
|                            |       |                 |               |               |       |         |         |         |        |           |             |
| **ConfigureAndMap**            | **True**  |     **1,740.83 ns** |     **33.383 ns** |     **38.444 ns** | **0.001** |    **0.00** |  **0.9346** |       **-** |      **-** |    **3912 B** |        **0.02** |
| ConfigureAndMap_AutoMapper | True  | 1,747,992.20 ns | 47,288.949 ns | 50,598.645 ns | 1.001 |    0.04 | 54.6875 | 52.7344 | 3.9063 |  234814 B |        1.00 |
