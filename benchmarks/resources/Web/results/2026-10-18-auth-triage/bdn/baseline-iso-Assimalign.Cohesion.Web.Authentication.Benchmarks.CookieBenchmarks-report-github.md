```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host]           : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a
  Default-Isolated : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=Default-Isolated  Affinity=1111  Server=True  
Arguments=/p:CohesionRoot=<feature/web-auth-perf checkout>  

```
| Method           | Mean        | Error      | StdDev    | Median      | Gen0   | Gen1   | Gen2   | Allocated |
|----------------- |------------:|-----------:|----------:|------------:|-------:|-------:|-------:|----------:|
| C_KeyLookup      |    39.47 ns |   0.040 ns |  0.033 ns |    39.46 ns |      - |      - |      - |         - |
| C_Hkdf           | 1,157.12 ns |   2.457 ns |  2.178 ns | 1,156.44 ns | 0.0267 |      - |      - |     288 B |
| C_AesGcm         | 2,695.10 ns |   2.961 ns |  2.312 ns | 2,694.84 ns | 0.0381 | 0.0038 | 0.0038 |    2207 B |
| C_Base64         |    82.02 ns |   1.690 ns |  3.916 ns |    79.64 ns | 0.0337 |      - |      - |     368 B |
| A_Base64         |    59.83 ns |   1.249 ns |  3.112 ns |    58.64 ns | 0.0243 |      - |      - |     256 B |
| C_Unprotect      | 3,914.90 ns |   6.460 ns |  5.727 ns | 3,914.13 ns | 0.0610 |      - |      - |     656 B |
| A_Unprotect      | 3,510.77 ns |   7.109 ns |  5.936 ns | 3,509.52 ns | 0.0534 |      - |      - |     560 B |
| A_GcmUnprotect   | 5,967.17 ns |  21.312 ns | 16.639 ns | 5,965.27 ns | 0.0534 |      - |      - |     560 B |
| A_GcmFormat      | 6,643.60 ns |  49.826 ns | 44.169 ns | 6,631.74 ns | 0.2136 |      - |      - |    2192 B |
| C_Deserialize    |   375.38 ns |   7.434 ns | 16.162 ns |   374.34 ns | 0.1497 |      - |      - |    1712 B |
| A_Deserialize    |   415.35 ns |   8.011 ns |  7.868 ns |   417.39 ns | 0.1349 |      - |      - |    1400 B |
| C_Format         | 4,479.81 ns |  40.468 ns | 37.853 ns | 4,470.52 ns | 0.2670 |      - |      - |    2736 B |
| A_Format         | 4,089.83 ns |  18.295 ns | 15.277 ns | 4,086.36 ns | 0.1984 |      - |      - |    2072 B |
| C_Handler        | 5,007.43 ns |  78.935 ns | 69.974 ns | 4,980.08 ns | 0.7935 |      - |      - |    6040 B |
| C_Service        | 5,142.20 ns |  79.666 ns | 70.622 ns | 5,137.29 ns | 0.6409 |      - |      - |    6480 B |
| A_HandlerService | 5,835.85 ns | 104.807 ns | 98.037 ns | 5,826.14 ns | 0.7019 |      - |      - |    7104 B |
