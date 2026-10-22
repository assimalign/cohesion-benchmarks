```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host] : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=ci  Toolchain=InProcessEmitToolchain  IterationCount=8  
LaunchCount=1  WarmupCount=3  

```
| Method                               | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| ValidateComparisons_FluentValidation | 4,016.6 ns | 325.99 ns | 170.50 ns |  1.00 |    0.06 | 2.0523 |   8.38 KB |        1.00 |
| ValidateComparisons                  | 3,227.6 ns | 372.16 ns | 194.65 ns |  0.80 |    0.06 | 5.0507 |  20.63 KB |        2.46 |
|                                      |            |           |           |       |         |        |           |             |
| ValidateInvalid_FluentValidation     | 3,509.7 ns |  90.96 ns |  40.39 ns |  1.00 |    0.02 | 2.0218 |   8.26 KB |        1.00 |
| ValidateInvalid                      | 3,247.5 ns | 294.86 ns | 154.22 ns |  0.93 |    0.04 | 5.1804 |  21.16 KB |        2.56 |
|                                      |            |           |           |       |         |        |           |             |
| Validate_FluentValidation            |   995.2 ns | 106.58 ns |  55.74 ns |  1.00 |    0.07 | 0.3872 |   1.59 KB |        1.00 |
| Validate                             | 3,739.4 ns | 292.48 ns | 152.97 ns |  3.77 |    0.25 | 5.7335 |  23.43 KB |       14.77 |
