# Connections benchmarks

The suite has 41 cases: the original five plus 36 additions. Cases count benchmark
methods multiplied by their parameter combinations.

| Package | Added cases | Work measured |
| --- | ---: | --- |
| Connections | 16 | Duplex stream reads/writes; named and typed feature lookup through wrappers, including shadowing and hidden features |
| Connections.InMemory | 14 | Batched buffer writes, segmented copies, async/TryRead consumption, multiplexed echo, stream open/accept/disposal |
| Connections.StreamAbort | 2 | Rejected local reads and coded peer reset reads after directional abort |
| Connections.StreamLimits | 2 | Optional allowance discovery and stream ordinal checks, feature present/absent |
| Connections.Tls | 2 | Optional TLS feature discovery and ALPN dispatch, feature present/absent |

System.IO.Pipelines remains the in-box counterpart (`SystemIOPipelines`): its stream
adapters perform identical copies/flushes, and its raw pipes transfer identical bytes
with the in-memory driver's non-pausing options. These compare successful I/O only;
raw pipes do not reproduce connection lifecycle or coded directional abort guarantees.
No new third-party NuGet packages are needed. Setup validates payloads, lookup decisions,
abort results, and the multi-segment shape of the large buffer-transfer fixture.

There is no dictionary baseline for feature lookup: plain dictionaries lack inherited
features, name shadowing and tombstones during typed resolution. StreamLimits uses a
fixed allowance registered through the public feature API, not a simulated QUIC credit
implementation. TLS feature dispatch similarly uses fixed negotiated facts; it does
not measure a TLS handshake or encryption.

## Remaining gaps

- Connections.NamedPipes, Connections.Tcp and Connections.Udp: no OS transport I/O,
  socket pumps, backpressure, accept contention, or raw sockets/System.IO.Pipes comparisons.
  The added byte-path workloads use Connections.InMemory to stay socketless on sandboxed runners.
- Connections.Quic and Connections.Security: no QUIC or TLS handshakes, encrypted
  transfers, certificate validation, or handshake scheduling. Certificate provisioning
  and platform TLS/QUIC support are outside these deterministic pipe workloads.
- StreamLimits: no real transport credit enforcement. StreamAbort: no pending-read
  races, post-abort write failures or timed abort transitions; successful per-operation
  guards run in multiplexed echo, and failure reads run on streams aborted during setup.
- The pinned connection API exposes Features, not a separate connection-items bag.

Validation: Release area build, all 41 cases under the dry profile, and the full Release
solution build. Dry results verify execution; they are not performance conclusions.
