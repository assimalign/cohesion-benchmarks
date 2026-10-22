# Http benchmarks

The suite has 82 cases, including 35 added request-path cases, twelve Accept ordering cases,
sixteen media-type parsing/rendering cases and eight media-type equality controls.
Existing benchmark names are retained.
Inputs are prepared in global setup, which also checks decoded values or exercises the transport.

Accept sorting evidence, including the rejected initial cutoff and unchanged parser controls,
is in [performance iteration 4](results/2026-10-21-performance-iteration-04/README.md).
Media-type allocation and equality evidence is in
[performance iteration 5](results/2026-10-21-performance-iteration-05/README.md).

| Package | Added cases | Work |
| --- | ---: | --- |
| Http | 6 | Content-Type and byte-range parsing against ASP.NET Core and System.Net.Http |
| Http | 12 | Media/token preference ordering at 1, 8, 9, 16, 17 and 64 entries |
| Http | 16 | Media-type parsing/rendering for plain, charset, multiple and escaped inputs, against ASP.NET Core |
| Http | 8 | Unique-name parameter equality in original/reversed order at 1, 2, 65 and 4,097 parameters |
| Http.Antiforgery | 2 | Request-token issue with an existing cookie and signed cookie/header validation |
| Http.DigestFields | 5 | Digest and preference parsing, canonical formatting, SHA-256 generation, dual-hash verification |
| Http.Forms | 8 | URL-encoded and multipart text decoding, repeated fields and UTF-8, against ASP.NET Core FormFeature |
| Http.RequestLimits | 2 | Chunked request consumption with a per-request cap enabled or disabled |
| Http.ServerSentEvents | 6 | Array/buffer formatting plus stream-batch formatting against System.Net.ServerSentEvents |
| Http.Sessions | 4 | Binary store-frame serialization/deserialization for four or 32 entries |
| Http.Streaming | 2 | A 4 KiB response in one or sixteen flushed writes |

Counterparts use the existing ASP.NET Core framework reference or .NET 10's framework assemblies;
no additional NuGet counterpart packages are required. Header inputs use shared grammar and both
sides return typed values with parameters/ranges. Form comparisons use complete feature parsing
to multivalue field collections, with matching values and arrival order checked in setup. SSE
comparison adapts both libraries to an async event sequence and a stream; parsing the output checks
equivalent events despite field-order differences. The BCL SSE suffix is `SystemNetServerSentEvents`
because that is the assembly implementing the formatter, distinct from `System.Net.Http`.

Request-limit and streaming benchmarks use the real public HTTP/1.1 transport over one in-memory
connection created in setup. Their measurements include framing and dispatch, so they are not
isolated feature costs. Kestrel's in-memory transport is not a public counterpart. The request cap
cases cover accepted requests; rejection closes the connection and is not measured. Clock-based
minimum data rates are also excluded.

Gaps remain deliberate and visible:

- `Http.ClientFactory`: no named-client/handler-pool lookup or redirect pipeline benchmark yet;
  network requests and connection creation are excluded.
- `Http.Forwarded`: this package exposes identity feature accessors. The existing `Http` benchmarks
  already measure the RFC 7239 and X-Forwarded-For parsers; proxy trust policy lives above this area.
- `Http.ExtendedConnect`, `Http.ProtocolUpgrade`, `Http.InterimResponses`: tunnel takeover,
  upgrade negotiation and informational response handling are not covered by these additions.
- `Http.Tls`: session metadata access is not a useful repeated-work benchmark; handshakes are excluded.
- `Http.WebSockets`: the HTTP package negotiates an opening handshake, then delegates frame
  encoding/decoding to `System.Net.WebSockets.WebSocket`; it has no independent frame codec to compare.
- Multipart file uploads/spill, malformed-input corpora, digest rejection, session stores,
  streaming backpressure, HTTP/2 and HTTP/3 are not covered by these additions.

Standalone digest fields have no typed counterpart in ASP.NET Core or System.Net.Http. A bare BCL
hash omits field construction/validation. ASP.NET Core's session serializer is internal and uses
different framing; a generic serializer would change the wire contract. Its antiforgery service
uses Data Protection and identity/additional-data binding, so it is not compared with Cohesion's
built-in HMAC protector. Those alternatives were considered and rejected as unequal work.
