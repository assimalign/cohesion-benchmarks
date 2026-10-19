# Cohesion Web vs ASP.NET Core

Run 2026-10-18T12:48:34.6245848-04:00 (74976cdc5).

| | |
| --- | --- |
| Machine | Snapdragon(R) X 12-core X1E80100 @ 3.40 GHz, 12 logical processors, 31.6 GB |
| OS | Microsoft Windows 10.0.26200 (Arm64) |
| Cores | server 4, load generator 8, pinned |
| Load | 64 keep-alive connections, 5s warmup, 10s measured, 3 iteration(s), median reported |
| GC | Server (forced on both servers) |
| Cohesion | 10.0.0-preview.1.local on .NET 10.0.13, server GC True, 4 processors, JIT True |
| ASP.NET Core | 10.0.13 on .NET 10.0.13, server GC True, 4 processors, JIT True |
| Cohesion commit | `74976cdc5` |

## Summary

Geometric mean of the per-scenario Cohesion ÷ ASP.NET Core ratio, over scenarios both frameworks passed. Above 1 favors Cohesion.

| Scenarios | Count | Throughput ratio | Efficiency ratio (req/CPU-s) | Cohesion faster in |
| --- | ---: | ---: | ---: | ---: |
| Core request path | 3 | 0.52× | 0.53× | 0 of 3 |
| Web packages | 3 | 0.46× | 0.45× | 0 of 3 |
| All | 6 | 0.49× | 0.49× | 0 of 6 |

## Throughput and latency

Requests per second over the measured phase (WebSocket: messages per second). † marks a server below 85% CPU, where the load generator may have set the pace; compare efficiency there instead.

| Scenario | Cohesion req/s | ASP.NET Core req/s | Ratio | Cohesion p50 / p99 (ms) | ASP.NET Core p50 / p99 (ms) |
| --- | ---: | ---: | ---: | ---: | ---: |
| `plaintext` | 97,982 | 126,460 | 0.77× | 0.61 / 1.27 | 0.48 / 0.87 |
| `plaintext-pipelined` | 338,672 | 1,464,644 | 0.23× | 2.68 / 6.39 | 0.68 / 2.02 |
| `plaintext-routed` | 97,843 | 123,723 | 0.79× | 0.61 / 1.35 | 0.49 / 0.86 |
| `auth-bearer` | 47,333 | 100,749 | 0.47× | 1.17 / 4.54 | 0.58 / 2.12 |
| `auth-cookie` | 38,741 | 97,795 | 0.40× | 1.44 / 5.31 | 0.63 / 1.46 |
| `authorization` | 48,345 | 95,226 | 0.51× | 1.17 / 3.78 | 0.61 / 2.69 |

## Efficiency and footprint

CPU is the server's share of its pinned cores. Requests per CPU-second divides completed requests by the server's CPU time, so it holds even when a run is client-bound. Startup is process start to the first HTTP response. Working set is measured after the run.

| Scenario | CPU % (C / A) | Req per CPU-s (C / A) | Ratio | Startup ms (C / A) | Working set MB (C / A) |
| --- | ---: | ---: | ---: | ---: | ---: |
| `plaintext` | 93 / 95 | 26,372 / 33,439 | 0.79× | 567 / 552 | 79.5 / 48.5 |
| `plaintext-pipelined` | 94 / 95 | 89,945 / 385,336 | 0.23× | 564 / 547 | 69.5 / 48.8 |
| `plaintext-routed` | 93 / 94 | 26,164 / 32,743 | 0.80× | 568 / 554 | 78.1 / 50.0 |
| `auth-bearer` | 95 / 94 | 12,426 / 26,757 | 0.46× | 568 / 565 | 76.6 / 95.7 |
| `auth-cookie` | 95 / 95 | 10,164 / 25,759 | 0.39× | 1077 / 555 | 78.5 / 93.8 |
| `authorization` | 95 / 94 | 12,745 / 25,249 | 0.50× | 568 / 560 | 77.0 / 96.4 |

## Response parity

The single validation exchange each server answered before load: status, response-head bytes and body bytes. Differences in head size are framework header choices (for example Kestrel's `Server` header).

| Scenario | Cohesion | ASP.NET Core |
| --- | --- | --- |
| `plaintext` | 200, head 102 B, body 13 B | 200, head 119 B, body 13 B |
| `plaintext-pipelined` | 200, head 102 B, body 13 B | 200, head 119 B, body 13 B |
| `plaintext-routed` | 200, head 117 B, body 13 B | 200, head 142 B, body 13 B |
| `auth-bearer` | 200, head 116 B, body 5 B | 200, head 142 B, body 5 B |
| `auth-cookie` | 200, head 116 B, body 5 B | 200, head 142 B, body 5 B |
| `authorization` | 200, head 116 B, body 5 B | 200, head 142 B, body 5 B |

<details><summary>Response heads</summary>

### `plaintext`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 16:48:55 GMT
Content-Type: text/plain
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 13
Content-Type: text/plain
Date: Sun, 18 Oct 2026 16:49:28 GMT
Server: Kestrel

Hello, World!
```

### `plaintext-pipelined`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 16:50:18 GMT
Content-Type: text/plain
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 13
Content-Type: text/plain
Date: Sun, 18 Oct 2026 16:50:35 GMT
Server: Kestrel

Hello, World!
```

### `plaintext-routed`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 16:53:21 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 16:51:57 GMT
Server: Kestrel
Transfer-Encoding: chunked

Hello, World!
```

### `auth-bearer`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 16:54:44 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 16:55:01 GMT
Server: Kestrel
Transfer-Encoding: chunked

alice
```

### `auth-cookie`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 16:56:41 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 16:56:24 GMT
Server: Kestrel
Transfer-Encoding: chunked

alice
```

### `authorization`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 16:58:05 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 16:57:14 GMT
Server: Kestrel
Transfer-Encoding: chunked

alice
```

</details>

## Scenarios

| Scenario | Server pipeline | What it measures |
| --- | --- | --- |
| `plaintext` | `plaintext` | Terminal middleware writes 13 bytes with Content-Length; no routing. |
| `plaintext-pipelined` | `plaintext` | The plaintext pipeline with 16 requests pipelined per connection (TechEmpower's plaintext depth). |
| `plaintext-routed` | `plaintext-routed` | MapGet returning a string, through the router. |
| `auth-bearer` | `auth-bearer` | An HS256 JWT validated (signature, issuer, audience, lifetime) on every request. |
| `auth-cookie` | `auth-cookie` | A data-protected authentication cookie decrypted on every request; each connection signs in once. |
| `authorization` | `authorization` | JWT authentication plus a role policy on the endpoint. |

