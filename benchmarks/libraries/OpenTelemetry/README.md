# OpenTelemetry benchmarks

20 cases cover the single transport-only package, adding 16 to the original four without renaming them:
direct exports and enqueue/flush bursts, nested attribute encoding, instrumentation-scope grouping,
JSON acknowledgements, partial and permanent rejection, multi-request draining, overflow/drain, and
producer writes to a saturated drop-oldest queue. Export inputs and responses are prebuilt in setup.
New handlers consume the request bytes and return deterministic responses without retaining payloads.
No network is used, and the one-day background flush interval leaves drains under benchmark control.

No counterpart was added. The pinned package implements only OTLP/HTTP JSON log export and buffering;
it has no activity or metric recording API. The OpenTelemetry SDK adds collection/processing and its
[OTLP exporter protocols](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/OtlpExportProtocol.cs)
are gRPC and HTTP/protobuf. Comparing these complete pipelines would mix different work and wire
encodings. A custom SDK exporter would instead measure a benchmark implementation.

Not covered: timed background drains, concurrent producers/drainers, shutdown latency, cancellation,
retry/backoff timing, real collectors, or network throughput. The retry path deliberately stays out
of this microbenchmark suite because its latency is governed by timer delays. Identifier constructors
and property getters are not treated as hot paths. Event timestamps are fixed; observed timestamps
remain part of the public exporter's encoding work, with no clock-dependent assertions.

Run `pwsh ./scripts/Invoke-Benchmarks.ps1 -Area OpenTelemetry -Profile dry` from the repository root.
