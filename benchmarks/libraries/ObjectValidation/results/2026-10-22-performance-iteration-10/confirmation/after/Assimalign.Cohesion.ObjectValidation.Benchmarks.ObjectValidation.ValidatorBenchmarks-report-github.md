```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=12  
LaunchCount=1  WarmupCount=5  

```
| Method                               | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------- |-----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| ValidateComparisons_FluentValidation | 3,236.8 ns | 119.47 ns | 86.38 ns |  1.00 |    0.04 | 2.0523 |   8.38 KB |        1.00 |
| ValidateComparisons                  | 1,813.5 ns |  64.90 ns | 50.67 ns |  0.56 |    0.02 | 2.0447 |   8.35 KB |        1.00 |
|                                      |            |           |          |       |         |        |           |             |
| ValidateInvalid_FluentValidation     | 2,939.8 ns |  73.94 ns | 57.72 ns |  1.00 |    0.03 | 2.0218 |   8.26 KB |        1.00 |
| ValidateInvalid                      | 1,801.7 ns |  75.58 ns | 59.01 ns |  0.61 |    0.02 | 1.9131 |   7.81 KB |        0.95 |
|                                      |            |           |          |       |         |        |           |             |
| Validate_FluentValidation            |   791.8 ns |  16.63 ns | 12.03 ns |  1.00 |    0.02 | 0.3881 |   1.59 KB |        1.00 |
| Validate                             | 1,589.0 ns |  95.85 ns | 74.83 ns |  2.01 |    0.10 | 1.1730 |    4.8 KB |        3.03 |
