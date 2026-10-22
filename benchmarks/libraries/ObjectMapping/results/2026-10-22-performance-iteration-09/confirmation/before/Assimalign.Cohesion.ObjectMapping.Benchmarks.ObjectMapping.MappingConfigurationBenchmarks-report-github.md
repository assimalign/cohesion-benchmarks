```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                     | Graph | Mean            | Error          | StdDev        | Ratio | RatioSD | Gen0    | Gen1    | Gen2   | Allocated | Alloc Ratio |
|--------------------------- |------ |----------------:|---------------:|--------------:|------:|--------:|--------:|--------:|-------:|----------:|------------:|
| **Configure**                  | **False** |        **96.39 ns** |       **3.514 ns** |      **2.744 ns** | **0.001** |    **0.00** |  **0.1109** |       **-** |      **-** |     **464 B** |        **0.01** |
| Configure_AutoMapper       | False |    84,981.88 ns |   2,187.493 ns |  1,707.850 ns | 1.000 |    0.03 |  9.1553 |       - |      - |   38762 B |        1.00 |
|                            |       |                 |                |               |       |         |         |         |        |           |             |
| **Configure**                  | **True**  |       **282.96 ns** |       **9.268 ns** |      **7.236 ns** | **0.002** |    **0.00** |  **0.2923** |       **-** |      **-** |    **1224 B** |        **0.01** |
| Configure_AutoMapper       | True  |   174,484.73 ns |   4,610.399 ns |  3,599.496 ns | 1.000 |    0.03 | 19.5313 |       - |      - |   81800 B |        1.00 |
|                            |       |                 |                |               |       |         |         |         |        |           |             |
| **ConfigureAndMap**            | **False** |       **165.72 ns** |       **5.819 ns** |      **4.543 ns** | **0.001** |    **0.00** |  **0.1452** |       **-** |      **-** |     **608 B** |       **0.009** |
| ConfigureAndMap_AutoMapper | False |   267,887.46 ns |   8,699.495 ns |  6,290.305 ns | 1.000 |    0.03 | 16.1133 | 15.1367 | 0.4883 |   68558 B |       1.000 |
|                            |       |                 |                |               |       |         |         |         |        |           |             |
| **ConfigureAndMap**            | **True**  |     **1,923.00 ns** |     **131.842 ns** |    **102.934 ns** | **0.001** |    **0.00** |  **0.9346** |       **-** |      **-** |    **3912 B** |        **0.02** |
| ConfigureAndMap_AutoMapper | True  | 1,805,919.42 ns | 100,216.095 ns | 78,242.135 ns | 1.002 |    0.06 | 54.6875 | 52.7344 | 1.9531 |  234275 B |        1.00 |
