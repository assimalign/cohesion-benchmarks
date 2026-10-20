# Rezolvr benchmarks

**DNS resolution is not implemented in the pinned build.** `ResolverEndpointService.ExecuteAsync`
contains a TODO and waits for cancellation. There is no authoritative/recursive lookup, cache,
forwarder, or answer path to benchmark. This suite does not label generic dictionary work as resolution.

24 standalone cases measure the functionality that does ship:

- `Rezolvr.ApplicationModel`: public A/CNAME declaration, DNS/address/TTL validation, canonical
  payload serialization and command identity generation (1/64 records).
- `Rezolvr.Hosting`: persisted A/CNAME record snapshots, validated command replay, delete/restore,
  missing-record deletion, and initial recovery from `records.json` (16/256 records).

The repository/handler are internal. Setup constructs the actual packaged types and binds delegates;
measured calls use the public command-handler interface or a pre-bound repository delegate. The
suite starts no server/container. All disk data is seeded in unique temporary directories and cleaned
up. Recovery includes file I/O but does not flush the OS cache; mutation includes atomic file replacement.

No counterpart is introduced. DNS clients such as DnsClient perform network queries, which Rezolvr
cannot yet answer; a generic dictionary/database does not provide the same declaration ownership,
conflict validation, payload and persistence semantics. Not covered: resolution (unimplemented),
network protocol, authentication/control-plane transport, concurrent mutations, rejected ownership
conflicts, host lifecycle, or full model planning/deployment. The root package is contracts/scaffolding.

Run: `pwsh ./scripts/Invoke-Benchmarks.ps1 -Area Rezolvr -Profile dry`
