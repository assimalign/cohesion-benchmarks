# ApplicationModel

No benchmark suite at the moment. cohesion rebuilt this area (assimalign/cohesion#1759): the manifest
parser, generic planner, application graph and sets, model document, gateway command dispatch and the
gateway control-plane router the previous suite measured no longer exist, so its 42 cases were removed
rather than renamed onto unrelated work.

The rebuilt area is seven packages: the shared identity (`ApplicationModel`), the gateway-to-instance
`Protocol`, the logical `Definition`, the instance runtime `Hosting`, the `Gateway`, and the in-process
platform pair (`Hosting.InProcess`, `Gateway.InProcess`). Candidate subjects for a new suite, each of them
work a gateway or an instance pays repeatedly rather than once at startup:

- `Protocol`: encoding and decoding an instance manifest, a publication, an action result and a state
  snapshot through the default JSON content serializer.
- `Hosting`: invoking an instance action through its control plane, including state snapshotting.
- `Definition` and `Gateway`: building a model of a few or a few dozen resources with dependencies, and
  planning a deployment of it against an in-memory store.

A new suite starts new history series; nothing here carries over from the removed cases.
