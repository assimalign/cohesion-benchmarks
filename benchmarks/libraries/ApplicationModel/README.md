# ApplicationModel benchmarks

42 cases across the four packages, including the four original cases with their names unchanged.
Graphs contain 4 or 24 resources: a configuration store, a stateful database, and dependent
services with both fan-out and chain edges. Application sets contain two graphs of those sizes.

| Package | Cases | Work measured |
| --- | ---: | --- |
| ApplicationModel | 24 | Manifest parsing, validation and hashing; generic planning and plan validation; application Build; model document creation, parsing and reconstruction; application-set resolution and provider binding |
| ApplicationModel.Gateway | 8 | Graph validation, mount input resolution, repeated direct commands and replacement command dispatch |
| ApplicationModel.Gateway.ControlPlane | 6 | The shipped five-route matcher's literal, command PUT/DELETE, HEAD, method refusal and missing-path paths |
| ApplicationModel.Gateway.InProcess | 4 | Executable binding validation and ambient host-plan rendering |

There is no third-party comparison: these operations enforce Cohesion's own composition contract.
System.Text.Json alone would skip manifest, graph, plan and provider validation and would not
produce equivalent results.

Inputs and application builders are prepared in setup. Build measures the public validation and
planning call, including provider snapshots and bound store validation. Its diagnostic text is
formatted but sent to a null writer configured in setup, excluding terminal I/O.

Gateway resolution uses the real protected input-resolution extension seam with contexts captured
from startup. Store reads, key persistence, artifact gathering, platform mutation and export
publication are in-memory fixtures. It covers provider-backed configuration, parameter secrets,
literal configuration and source-free volume mounts; setup checks successful resolution.

Command cases use public reconciliation and the shared direct control-plane delivery branch used
by InProcessGateway, including real ownership, replay and runtime-handler dispatch. They replace
the host's control-plane lookup with a fixture. Replacement alternates two prebuilt models so each
call withdraws and applies a command. This does not measure executable invocation, host supervision,
or InProcessGateway's host lookup. The actual InProcessGateway cases remain offline.

The control-plane router is internal and only reachable publicly behind a server. Setup constructs
the shipped router and compiles a direct delegate; measured calls include no reflection or boxing.
This isolates matching, excluding sockets, authentication, HTTP framing and request handlers.

Not covered: process/container startup, remote gateway calls, TLS/certificate issuance, trust
rotation, export file I/O, background observation and failure/retry budgets. No listening ports,
external services or files outside suite-created temporary directories are used.

Validate with:

```powershell
dotnet build benchmarks/libraries/ApplicationModel/benchmarks/Assimalign.Cohesion.ApplicationModel.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area ApplicationModel -Profile dry
```
