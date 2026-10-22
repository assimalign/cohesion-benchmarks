```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                               | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| ValidateComparisons_FluentValidation | 3,240.3 ns |  97.27 ns |  75.94 ns |  1.00 |    0.03 | 2.0523 |   8.38 KB |        1.00 |
| ValidateComparisons                  | 2,663.7 ns | 110.76 ns |  86.47 ns |  0.82 |    0.03 | 5.0507 |  20.63 KB |        2.46 |
|                                      |            |           |           |       |         |        |           |             |
| ValidateInvalid_FluentValidation     | 3,079.8 ns |  83.89 ns |  65.50 ns |  1.00 |    0.03 | 2.0218 |   8.26 KB |        1.00 |
| ValidateInvalid                      | 2,725.5 ns |  67.81 ns |  52.94 ns |  0.89 |    0.02 | 5.1804 |  21.16 KB |        2.56 |
|                                      |            |           |           |       |         |        |           |             |
| Validate_FluentValidation            |   822.3 ns |  18.26 ns |  13.21 ns |  1.00 |    0.02 | 0.3881 |   1.59 KB |        1.00 |
| Validate                             | 3,067.7 ns | 163.51 ns | 127.66 ns |  3.73 |    0.16 | 5.7335 |  23.43 KB |       14.77 |
