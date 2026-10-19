```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9711/25H2/2025Update/HudsonValley2)
Snapdragon X 12-core X1E80100 3.40 GHz (Max: 3.42GHz), 1 CPU, 12 logical and 12 physical cores
.NET SDK 10.0.402
  [Host]           : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a
  Default-Isolated : .NET 10.0.13 (10.0.13, 10.0.1326.50504), Arm64 RyuJIT armv8.0-a

Job=Default-Isolated  Affinity=1111  Server=True  
Arguments=/p:CohesionRoot=<feature/web-auth-perf checkout>  

```
| Method                 | Mean       | Error    | StdDev    | Median      | Gen0   | Gen1   | Allocated |
|----------------------- |-----------:|---------:|----------:|------------:|-------:|-------:|----------:|
| C_ValidateLibrary      | 3,199.7 ns | 62.85 ns |  72.38 ns | 3,173.88 ns | 0.8430 | 0.0038 |    8608 B |
| A_ValidateLazy         | 1,964.3 ns | 38.26 ns |  62.87 ns | 1,959.89 ns | 0.3738 |      - |    3776 B |
| A_ValidatePrincipal    | 2,526.5 ns | 49.20 ns |  67.34 ns | 2,511.63 ns | 0.6256 |      - |    6496 B |
| C_SplitDecode          |   170.5 ns |  3.46 ns |   9.58 ns |   169.17 ns | 0.0839 |      - |    1000 B |
| C_JsonToDescriptor     | 1,329.2 ns | 25.84 ns |  26.53 ns | 1,332.44 ns | 0.3014 |      - |    3064 B |
| C_SnapshotModel        |   188.2 ns |  3.72 ns |   8.09 ns |   187.57 ns | 0.0777 |      - |     888 B |
| C_Parse                | 1,669.5 ns | 29.88 ns |  43.79 ns | 1,660.83 ns | 0.4845 | 0.0019 |    4912 B |
| A_Parse                | 1,008.8 ns | 19.97 ns |  29.27 ns | 1,004.46 ns | 0.2270 |      - |    2312 B |
| C_Hmac                 |   563.3 ns |  1.10 ns |   0.97 ns |   563.26 ns |      - |      - |         - |
| A_Hmac                 |   325.4 ns |  3.87 ns |   3.43 ns |   323.53 ns | 0.0162 |      - |     168 B |
| Candidate_RetainedHmac |   353.2 ns |  0.36 ns |   0.28 ns |   353.10 ns |      - |      - |         - |
| C_SignatureBuffers     |   111.1 ns |  2.59 ns |   7.60 ns |   107.11 ns | 0.1348 |      - |    1360 B |
| C_Rules                |   125.2 ns |  2.50 ns |   3.16 ns |   124.09 ns | 0.0348 |      - |     352 B |
| C_MapPrincipal         |   468.1 ns |  9.33 ns |  12.46 ns |   469.13 ns | 0.1822 |      - |    2016 B |
| C_FreshContext         |   101.1 ns |  2.84 ns |   8.10 ns |    97.98 ns | 0.1205 | 0.0001 |    1216 B |
| A_FreshContext         |   260.2 ns |  5.25 ns |  14.80 ns |   254.68 ns | 0.1884 | 0.0005 |    1904 B |
| C_Handler              | 3,342.9 ns | 65.83 ns | 118.71 ns | 3,328.42 ns | 1.0376 | 0.0076 |   10800 B |
| C_Service              | 3,440.0 ns | 67.20 ns |  74.70 ns | 3,433.56 ns | 1.0834 | 0.0076 |   11448 B |
| A_HandlerService       | 3,932.7 ns | 37.62 ns |  31.41 ns | 3,921.07 ns | 1.1673 | 0.0153 |   12216 B |
