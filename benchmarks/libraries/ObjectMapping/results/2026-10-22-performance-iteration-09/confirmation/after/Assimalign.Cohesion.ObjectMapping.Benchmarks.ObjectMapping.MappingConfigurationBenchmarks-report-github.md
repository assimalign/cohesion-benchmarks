```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                     | Graph | Mean           | Error         | StdDev        | Ratio | RatioSD | Gen0    | Gen1    | Gen2   | Allocated | Alloc Ratio |
|--------------------------- |------ |---------------:|--------------:|--------------:|------:|--------:|--------:|--------:|-------:|----------:|------------:|
| **Configure**                  | **False** |       **114.4 ns** |       **4.85 ns** |       **3.79 ns** | **0.001** |    **0.00** |  **0.1109** |       **-** |      **-** |     **464 B** |        **0.01** |
| Configure_AutoMapper       | False |   101,907.9 ns |   2,268.48 ns |   1,771.08 ns | 1.000 |    0.02 |  9.2773 |  0.1221 |      - |   38803 B |        1.00 |
|                            |       |                |               |               |       |         |         |         |        |           |             |
| **Configure**                  | **True**  |       **340.2 ns** |      **10.19 ns** |       **7.96 ns** | **0.002** |    **0.00** |  **0.2923** |       **-** |      **-** |    **1224 B** |        **0.01** |
| Configure_AutoMapper       | True  |   187,711.6 ns |   6,616.04 ns |   5,165.37 ns | 1.001 |    0.04 | 19.5313 |       - |      - |   81832 B |        1.00 |
|                            |       |                |               |               |       |         |         |         |        |           |             |
| **ConfigureAndMap**            | **False** |       **124.1 ns** |       **2.88 ns** |       **2.25 ns** | **0.000** |    **0.00** |  **0.1242** |       **-** |      **-** |     **520 B** |       **0.008** |
| ConfigureAndMap_AutoMapper | False |   260,285.7 ns |   6,224.21 ns |   4,859.45 ns | 1.000 |    0.03 | 16.1133 | 15.1367 | 0.4883 |   68595 B |       1.000 |
|                            |       |                |               |               |       |         |         |         |        |           |             |
| **ConfigureAndMap**            | **True**  |     **1,160.0 ns** |       **8.32 ns** |       **5.50 ns** | **0.001** |    **0.00** |  **0.5512** |       **-** |      **-** |    **2312 B** |       **0.010** |
| ConfigureAndMap_AutoMapper | True  | 1,647,456.3 ns | 181,742.63 ns | 141,892.69 ns | 1.007 |    0.12 | 54.6875 | 52.7344 | 1.9531 |  234769 B |       1.000 |
