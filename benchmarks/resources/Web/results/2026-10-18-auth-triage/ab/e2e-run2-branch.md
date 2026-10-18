# Cohesion Web vs ASP.NET Core

Run 2026-10-18T13:08:48.3474779-04:00 (feature/web-auth-perf).

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
| Core request path | 3 | 0.77× | 0.76× | 0 of 3 |
| Web packages | 3 | 0.95× | 0.94× | 0 of 3 |
| All | 6 | 0.85× | 0.85× | 0 of 6 |

## Throughput and latency

Requests per second over the measured phase (WebSocket: messages per second). † marks a server below 85% CPU, where the load generator may have set the pace; compare efficiency there instead.

| Scenario | Cohesion req/s | ASP.NET Core req/s | Ratio | Cohesion p50 / p99 (ms) | ASP.NET Core p50 / p99 (ms) |
| --- | ---: | ---: | ---: | ---: | ---: |
| `plaintext` | 112,464 | 125,175 | 0.90× | 0.53 / 1.08 | 0.48 / 0.94 |
| `plaintext-pipelined` | 782,034 | 1,453,483 | 0.54× | 1.17 / 3.24 | 0.70 / 1.35 |
| `plaintext-routed` | 112,825 | 121,792 | 0.93× | 0.53 / 1.13 | 0.50 / 0.89 |
| `auth-bearer` | 93,511 | 100,514 | 0.93× | 0.61 / 1.79 | 0.58 / 2.08 |
| `auth-cookie` | 90,397 | 97,791 | 0.92× | 0.65 / 1.45 | 0.62 / 1.91 |
| `authorization` | 91,952 | 93,253 | 0.99× | 0.63 / 1.55 | 0.62 / 2.84 |

## Efficiency and footprint

CPU is the server's share of its pinned cores. Requests per CPU-second divides completed requests by the server's CPU time, so it holds even when a run is client-bound. Startup is process start to the first HTTP response. Working set is measured after the run.

| Scenario | CPU % (C / A) | Req per CPU-s (C / A) | Ratio | Startup ms (C / A) | Working set MB (C / A) |
| --- | ---: | ---: | ---: | ---: | ---: |
| `plaintext` | 93 / 91 | 30,330 / 34,213 | 0.89× | 556 / 548 | 77.0 / 47.2 |
| `plaintext-pipelined` | 95 / 95 | 206,187 / 384,179 | 0.54× | 552 / 534 | 71.3 / 47.3 |
| `plaintext-routed` | 93 / 94 | 30,195 / 32,323 | 0.93× | 557 / 571 | 77.2 / 51.1 |
| `auth-bearer` | 93 / 94 | 25,076 / 26,694 | 0.94× | 573 / 569 | 81.6 / 95.7 |
| `auth-cookie` | 94 / 94 | 23,936 / 26,130 | 0.92× | 569 / 561 | 82.4 / 95.0 |
| `authorization` | 94 / 93 | 24,437 / 24,941 | 0.98× | 569 / 552 | 85.2 / 101.6 |

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
Date: Sun, 18 Oct 2026 17:10:14 GMT
Content-Type: text/plain
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 13
Content-Type: text/plain
Date: Sun, 18 Oct 2026 17:09:57 GMT
Server: Kestrel

Hello, World!
```

### `plaintext-pipelined`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 17:10:30 GMT
Content-Type: text/plain
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 13
Content-Type: text/plain
Date: Sun, 18 Oct 2026 17:11:03 GMT
Server: Kestrel

Hello, World!
```

### `plaintext-routed`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 17:13:33 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 17:13:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

Hello, World!
```

### `auth-bearer`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 17:13:50 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 17:14:06 GMT
Server: Kestrel
Transfer-Encoding: chunked

alice
```

### `auth-cookie`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 17:16:04 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 17:16:37 GMT
Server: Kestrel
Transfer-Encoding: chunked

alice
```

### `authorization`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 17:18:17 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 17:18:33 GMT
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

