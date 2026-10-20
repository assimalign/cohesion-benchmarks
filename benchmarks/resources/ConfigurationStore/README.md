# ConfigurationStore benchmarks

34 cases (17 methods across two namespace sizes), running entirely in process against the pinned
Cohesion packages. No server, socket, container or external service is started.

| Package | Cases | Work measured |
| --- | ---: | --- |
| `Assimalign.Cohesion.ConfigurationStore.Hosting` | 20 | Namespace snapshots and missing lookup, sorted namespace listing, snapshot plus key lookup, durable set/remove cycles, namespace declaration replay/create/delete, and set/remove command dispatch |
| `Assimalign.Cohesion.ConfigurationStore.Client` | 12 | Namespace/list decoding with read-only results, command request encoding, and applied/rejected/deleted observations |
| `Assimalign.Cohesion.ConfigurationStore.ApplicationModel.Orchestration` | 2 | Public configuration-mount source resolution, connection validation, endpoint normalization, per-call transport lifetime and namespace decoding |

Fixtures contain 8 or 128 entries with nullable and Unicode string values. Hosting also varies the
number of namespaces by that count to exercise sorted listing. The actual engine holds live values
in memory and persists mutations as atomic JSON replacements. Reads include the shipped defensive
snapshot copy; `ReadAndLookupKey` includes that snapshot and a successful key lookup rather than
timing a bare dictionary accessor. Missing namespace lookup is an invariant-cost control across the
two store fixtures. Writes stay bounded: sets overwrite a fixed key and create/remove cycles restore
the initial state on each invocation. Every repository owns and cleans up a unique temp directory.

The repository has no public in-process facade. Setup binds typed delegates to the packaged internal
repository; measured calls do not use reflection. Commands use `IResourceCommandHandler`, including
payload parsing and validation. Public clients use caller-owned `HttpMessageInvoker` transports;
handlers consume encoded command bodies and wrap setup-built response bytes in fresh responses.
The source provider's internal transport-factory constructor is used only in setup because its public
constructor always creates a socket transport; its measured entry point is public. Setup checks
namespace contents, nullable values, command updates/deletion, request payload bytes, bearer headers,
routes and observation statuses.

All cases are standalone. No counterpart package was added. A BCL dictionary was rejected because
it lacks snapshot isolation, sorted enumeration, persistence and declaration ownership. Raw
`System.Text.Json` encoding/decoding was also rejected as a substitute for client calls, which include
HTTP envelopes, read-only result wrappers and command observations.

Not covered: HTTP endpoint dispatch and authentication, trust-issuer persistence, contention,
startup/recovery, ownership rejection inside the engine, and ApplicationModel planning or
Refs/Runtime SDK packaging. The root package supplies application contracts rather than a separate
read/write engine. The pinned store/client has no watch or change-notification contract, so no
notification benchmark is invented. No network or server throughput is claimed.

```powershell
dotnet build benchmarks/resources/ConfigurationStore/benchmarks/Assimalign.Cohesion.ConfigurationStore.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area ConfigurationStore -Profile dry
```
