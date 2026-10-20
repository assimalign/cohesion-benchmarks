# ObjectPool benchmarks

27 cases for Assimalign.Cohesion.ObjectPool: three existing cases plus 24 additions.

| Added workload | Cases | Shape |
| --- | ---: | --- |
| Uncontended round trip, retained burst, exhaustion | 12 | Retain 1/16 objects; exhaust with 32 outstanding rentals |
| Reset and rejection policies | 8 | Append 64/4096 characters; clear retained state or reject capacity above 8192 |
| Contention | 4 | 2/4 dedicated workers, 1024 round trips each |

All additions compare with the existing Microsoft.Extensions.ObjectPool dependency. Both sides have
the same factories, retention limits and return/reset policies. Exhaustion means allocating beyond
the retained slots, not waiting for a bounded pool. Outstanding-rental buffers and worker threads are
created in setup. Contention reports batch latency including identical barriers on both sides;
it is not a single-rental latency measurement. Setup primes pools and checks the reset/output contract.

Not covered: argument-taking factories, CancellationTokenSourcePool, disposable-object ownership,
and contention above four workers. No additional counterpart package was needed.

Validation:

```powershell
dotnet build benchmarks/libraries/ObjectPool/benchmarks/Assimalign.Cohesion.ObjectPool.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area ObjectPool -Profile dry
```
