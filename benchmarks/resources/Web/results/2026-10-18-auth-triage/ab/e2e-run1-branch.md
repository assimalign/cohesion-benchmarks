# Cohesion Web vs ASP.NET Core

Run 2026-10-18T12:38:23.2844538-04:00 (feature/web-auth-perf).

| | |
| --- | --- |
| Machine | Snapdragon(R) X 12-core X1E80100 @ 3.40 GHz, 12 logical processors, 31.6 GB |
| OS | Microsoft Windows 10.0.26200 (Arm64) |
| Cores | server 4, load generator 8, pinned |
| Load | 64 keep-alive connections, 5s warmup, 10s measured, 3 iteration(s), median reported |
| GC | Server (forced on both servers) |
| Cohesion | 10.0.0-preview.1.local on .NET 10.0.13, server GC True, 4 processors, JIT True |
| ASP.NET Core | 10.0.13 on .NET 10.0.13, server GC True, 4 processors, JIT True |
| Cohesion commit | `40d670fa8` (feature/web-auth-perf) |

## Summary

Geometric mean of the per-scenario Cohesion ÷ ASP.NET Core ratio, over scenarios both frameworks passed. Above 1 favors Cohesion.

| Scenarios | Count | Throughput ratio | Efficiency ratio (req/CPU-s) | Cohesion faster in |
| --- | ---: | ---: | ---: | ---: |
| Core request path | 3 | 0.75× | 0.77× | 0 of 3 |
| Web packages | 3 | 0.94× | 0.95× | 1 of 3 |
| All | 6 | 0.84× | 0.86× | 1 of 6 |

## Throughput and latency

Requests per second over the measured phase (WebSocket: messages per second). † marks a server below 85% CPU, where the load generator may have set the pace; compare efficiency there instead.

| Scenario | Cohesion req/s | ASP.NET Core req/s | Ratio | Cohesion p50 / p99 (ms) | ASP.NET Core p50 / p99 (ms) |
| --- | ---: | ---: | ---: | ---: | ---: |
| `plaintext` | 112,240 | 125,974 | 0.89× | 0.53 / 1.13 | 0.49 / 0.90 |
| `plaintext-pipelined` | 778,641 | 1,503,667 | 0.52× | 1.17 / 3.93 | 0.66 / 1.34 |
| `plaintext-routed` | 111,821 | 122,019 | 0.92× | 0.53 / 1.12 | 0.50 / 0.88 |
| `auth-bearer` | 94,580 | 101,853 | 0.93× | 0.60 / 1.60 | 0.58 / 1.58 |
| `auth-cookie` | 88,895 | 98,845 | 0.90× | 0.66 / 1.58 | 0.61 / 1.77 |
| `authorization` | 94,576 | 94,220 | 1.00× | 0.61 / 1.56 | 0.61 / 2.13 |

## Efficiency and footprint

CPU is the server's share of its pinned cores. Requests per CPU-second divides completed requests by the server's CPU time, so it holds even when a run is client-bound. Startup is process start to the first HTTP response. Working set is measured after the run.

| Scenario | CPU % (C / A) | Req per CPU-s (C / A) | Ratio | Startup ms (C / A) | Working set MB (C / A) |
| --- | ---: | ---: | ---: | ---: | ---: |
| `plaintext` | 92 / 94 | 30,527 / 33,388 | 0.91× | 572 / 1051 | 78.0 / 46.7 |
| `plaintext-pipelined` | 92 / 95 | 211,859 / 396,339 | 0.53× | 572 / 547 | 68.9 / 46.9 |
| `plaintext-routed` | 93 / 94 | 29,930 / 32,301 | 0.93× | 567 / 556 | 80.7 / 50.0 |
| `auth-bearer` | 93 / 94 | 25,365 / 27,065 | 0.94× | 801 / 570 | 84.4 / 96.8 |
| `auth-cookie` | 94 / 95 | 23,678 / 25,879 | 0.91× | 569 / 564 | 85.3 / 91.5 |
| `authorization` | 94 / 94 | 25,210 / 25,029 | 1.01× | 568 / 558 | 80.8 / 95.0 |

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
Date: Sun, 18 Oct 2026 16:38:44 GMT
Content-Type: text/plain
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 13
Content-Type: text/plain
Date: Sun, 18 Oct 2026 16:39:34 GMT
Server: Kestrel

Hello, World!
```

### `plaintext-pipelined`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 16:41:14 GMT
Content-Type: text/plain
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 13
Content-Type: text/plain
Date: Sun, 18 Oct 2026 16:40:24 GMT
Server: Kestrel

Hello, World!
```

### `plaintext-routed`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 16:43:10 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 16:42:37 GMT
Server: Kestrel
Transfer-Encoding: chunked

Hello, World!
```

### `auth-bearer`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 16:43:27 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 16:44:00 GMT
Server: Kestrel
Transfer-Encoding: chunked

alice
```

### `auth-cookie`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 16:45:41 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 16:46:14 GMT
Server: Kestrel
Transfer-Encoding: chunked

alice
```

### `authorization`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 16:46:48 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 16:48:10 GMT
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

