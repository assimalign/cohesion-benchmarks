```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method                     | Graph | Mean            | Error          | StdDev        | Ratio | RatioSD | Gen0    | Gen1    | Gen2   | Allocated | Alloc Ratio |
|--------------------------- |------ |----------------:|---------------:|--------------:|------:|--------:|--------:|--------:|-------:|----------:|------------:|
| **Configure**                  | **False** |        **91.89 ns** |       **1.641 ns** |      **0.858 ns** | **0.001** |    **0.00** |  **0.1109** |       **-** |      **-** |     **464 B** |        **0.01** |
| Configure_AutoMapper       | False |    83,488.58 ns |   1,930.782 ns |  1,009.836 ns | 1.000 |    0.02 |  9.1553 |       - |      - |   38682 B |        1.00 |
|                            |       |                 |                |               |       |         |         |         |        |           |             |
| **Configure**                  | **True**  |       **273.56 ns** |      **15.734 ns** |      **8.229 ns** | **0.002** |    **0.00** |  **0.2923** |       **-** |      **-** |    **1224 B** |        **0.02** |
| Configure_AutoMapper       | True  |   174,246.55 ns |   2,430.774 ns |  1,271.342 ns | 1.000 |    0.01 | 19.2871 |       - |      - |   81512 B |        1.00 |
|                            |       |                 |                |               |       |         |         |         |        |           |             |
| **ConfigureAndMap**            | **False** |       **129.19 ns** |       **2.406 ns** |      **1.259 ns** | **0.001** |    **0.00** |  **0.1242** |       **-** |      **-** |     **520 B** |       **0.008** |
| ConfigureAndMap_AutoMapper | False |   256,523.37 ns |   7,653.038 ns |  3,397.997 ns | 1.000 |    0.02 | 16.1133 | 15.1367 |      - |   68481 B |       1.000 |
|                            |       |                 |                |               |       |         |         |         |        |           |             |
| **ConfigureAndMap**            | **True**  |     **1,170.88 ns** |      **44.295 ns** |     **23.167 ns** | **0.001** |    **0.00** |  **0.5722** |       **-** |      **-** |    **2400 B** |        **0.01** |
| ConfigureAndMap_AutoMapper | True  | 1,462,595.34 ns | 177,049.593 ns | 92,600.361 ns | 1.003 |    0.08 | 54.6875 | 52.7344 | 1.9531 |  234250 B |        1.00 |
