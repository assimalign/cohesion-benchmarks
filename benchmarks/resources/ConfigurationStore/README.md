# ConfigurationStore benchmarks

12 cases (6 methods across two namespace sizes), running entirely in process against the pinned
Cohesion packages. No server, socket, container or external service is started.

| Package | Cases | Work measured |
| --- | ---: | --- |
| `Assimalign.Cohesion.ConfigurationStore.Hosting` | 8 | Namespace snapshots and missing lookup, sorted namespace listing, and snapshot plus key lookup |
| `Assimalign.Cohesion.ConfigurationStore.Client` | 4 | Namespace and namespace-list reads: request construction, bearer header, decoding and read-only results |

Fixtures contain 8 or 128 entries with nullable and Unicode string values. Hosting also varies the
number of namespaces by that count to exercise sorted listing. The actual engine holds live values
in memory. Reads include the shipped defensive snapshot copy; `ReadAndLookupKey` includes that
snapshot and a successful key lookup rather than timing a bare dictionary accessor. Missing namespace
lookup is an invariant-cost control across the two store fixtures. Every repository owns and cleans
up a unique temp directory.

Only the repository paths the shipped endpoint service calls (initialize, read, list) are measured.
The repository still has set/remove methods, but nothing calls them at the pinned build: the resource
command path that drove them was removed, so they are not benchmarked.

The repository has no public in-process facade. Setup binds typed delegates to the packaged internal
repository; measured calls do not use reflection. The public client uses a caller-owned
`HttpMessageInvoker` transport whose handler wraps setup-built response bytes in fresh responses.
Setup checks namespace contents, nullable values and the bearer header.

All cases are standalone. No counterpart package was added. A BCL dictionary was rejected because
it lacks snapshot isolation, sorted enumeration and persistence. Raw `System.Text.Json` decoding was
also rejected as a substitute for client calls, which include the HTTP envelope and read-only result
wrappers.

Not covered: writes (no shipped caller), HTTP endpoint dispatch and authentication, trust-issuer
persistence, contention, startup/recovery, the resource integration
(`ConfigurationStore.Hosting.ApplicationModel`, which ships no actions of its own) and Refs/Runtime
SDK packaging. The root package supplies application
contracts rather than a separate read/write engine. The pinned store/client has no watch or
change-notification contract, so no notification benchmark is invented. No network or server
throughput is claimed.

```powershell
dotnet build benchmarks/resources/ConfigurationStore/benchmarks/Assimalign.Cohesion.ConfigurationStore.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area ConfigurationStore -Profile dry
```
