# Rezolvr benchmarks

No benchmark suite at the moment. Nothing Rezolvr ships at the pinned build does measurable work:

- **DNS resolution is not implemented.** `ResolverEndpointService.ExecuteAsync` contains a TODO and
  waits for cancellation. There is no authoritative/recursive lookup, cache, forwarder or answer path.
- **Record storage is unreachable.** The internal `RezolvrRecordRepository` (A/CNAME declarations
  persisted to `records.json`) was driven by the resource command path that cohesion removed in its
  application-model rebuild (assimalign/cohesion#1759). At the pinned build nothing constructs it, and
  its read and mutate methods have no caller outside its own tests.

The previous suite's 20 cases (record snapshots, declaration replay, delete/restore, missing-record
deletion, and recovery from `records.json`) measured that repository, so they were removed rather
than kept as measurements of code no shipped path runs. A generic dictionary or DNS client would not
measure Rezolvr either, so no counterpart replaces them.

A new suite belongs here once Rezolvr answers queries or its records are reachable through a shipped
path, such as a resource action of `Rezolvr.Hosting.ApplicationModel`. It starts new history series;
nothing carries over from the removed cases.
