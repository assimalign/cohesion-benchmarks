# NatGateway benchmarks

4 cases total, including the existing 2 lifecycle cases. `Build` and `BuildStartStop`, each with 1 or 16 hosted services.
The suite references `Assimalign.Cohesion.NatGateway.Hosting`; composition also exercises the
root application contracts and the transitive dependency-injection and hosting packages.

`Build` includes builder creation, service registration, container construction and validation,
host-service resolution, and async disposal. `BuildStartStop` also includes ordered startup,
cancellation and shutdown. The existing lifecycle case retains its caller-owned service registrations; the new build case uses application-owned factories.
Services wait only for their stop token, so there are no timers, sockets or external dependencies.
The two service counts expose the change from a small host to a longer lifecycle-service list.

Standalone only: no third-party counterpart was introduced. A generic host or raw DI container
would omit this resource's application context, registration closure and host-service snapshot,
so neither is used as a substitute comparison.

Not covered: packet translation, session lookup or port allocation; the pinned area does not implement
those operations. ApplicationModel manifest/planner metadata, Refs/Runtime packaging, ambient
orchestrator control-plane endpoints and telemetry are outside this standalone composition suite.
No operational throughput or data-plane performance is claimed.

```powershell
dotnet build benchmarks/resources/NatGateway/benchmarks/Assimalign.Cohesion.NatGateway.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area NatGateway -Profile dry
```
