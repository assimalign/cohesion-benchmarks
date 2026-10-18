# Cohesion Web vs ASP.NET Core

Run 2026-10-18T12:58:45.1401015-04:00 (74976cdc5).

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
| Web packages | 3 | 0.45× | 0.45× | 0 of 3 |
| All | 6 | 0.49× | 0.49× | 0 of 6 |

## Throughput and latency

Requests per second over the measured phase (WebSocket: messages per second). † marks a server below 85% CPU, where the load generator may have set the pace; compare efficiency there instead.

| Scenario | Cohesion req/s | ASP.NET Core req/s | Ratio | Cohesion p50 / p99 (ms) | ASP.NET Core p50 / p99 (ms) |
| --- | ---: | ---: | ---: | ---: | ---: |
| `plaintext` | 97,770 | 124,173 | 0.79× | 0.61 / 1.31 | 0.49 / 0.91 |
| `plaintext-pipelined` | 342,195 | 1,507,070 | 0.23× | 2.67 / 6.28 | 0.67 / 1.23 |
| `plaintext-routed` | 97,611 | 121,530 | 0.80× | 0.61 / 1.42 | 0.50 / 0.89 |
| `auth-bearer` | 48,647 | 102,266 | 0.48× | 1.14 / 4.57 | 0.57 / 1.81 |
| `auth-cookie` | 38,013 | 99,030 | 0.38× | 1.48 / 5.54 | 0.61 / 2.00 |
| `authorization` | 47,416 | 93,776 | 0.51× | 1.17 / 5.29 | 0.61 / 2.75 |

## Efficiency and footprint

CPU is the server's share of its pinned cores. Requests per CPU-second divides completed requests by the server's CPU time, so it holds even when a run is client-bound. Startup is process start to the first HTTP response. Working set is measured after the run.

| Scenario | CPU % (C / A) | Req per CPU-s (C / A) | Ratio | Startup ms (C / A) | Working set MB (C / A) |
| --- | ---: | ---: | ---: | ---: | ---: |
| `plaintext` | 93 / 94 | 26,267 / 32,935 | 0.80× | 567 / 549 | 77.5 / 48.0 |
| `plaintext-pipelined` | 94 / 94 | 91,110 / 401,589 | 0.23× | 568 / 546 | 68.7 / 47.0 |
| `plaintext-routed` | 92 / 94 | 26,468 / 32,187 | 0.82× | 564 / 558 | 81.7 / 49.4 |
| `auth-bearer` | 93 / 93 | 13,124 / 27,507 | 0.48× | 564 / 569 | 77.6 / 94.9 |
| `auth-cookie` | 94 / 95 | 10,097 / 25,952 | 0.39× | 567 / 550 | 74.4 / 93.5 |
| `authorization` | 95 / 93 | 12,518 / 25,084 | 0.50× | 568 / 547 | 75.8 / 96.3 |

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
Date: Sun, 18 Oct 2026 16:59:20 GMT
Content-Type: text/plain
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 13
Content-Type: text/plain
Date: Sun, 18 Oct 2026 16:59:37 GMT
Server: Kestrel

Hello, World!
```

### `plaintext-pipelined`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 17:00:27 GMT
Content-Type: text/plain
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 13
Content-Type: text/plain
Date: Sun, 18 Oct 2026 17:01:50 GMT
Server: Kestrel

Hello, World!
```

### `plaintext-routed`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 17:02:23 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 17:02:56 GMT
Server: Kestrel
Transfer-Encoding: chunked

Hello, World!
```

### `auth-bearer`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 17:04:36 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 17:04:03 GMT
Server: Kestrel
Transfer-Encoding: chunked

alice
```

### `auth-cookie`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 17:06:50 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 17:05:26 GMT
Server: Kestrel
Transfer-Encoding: chunked

alice
```

### `authorization`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 17:07:07 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 17:07:23 GMT
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

