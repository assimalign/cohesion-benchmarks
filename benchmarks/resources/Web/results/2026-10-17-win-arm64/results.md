# Cohesion Web vs ASP.NET Core

Run 2026-10-17T21:56:17.3053725-04:00 (Surface Laptop 7 (Snapdragon X Elite X1E80100), Windows 11 26200, AC power, Balanced plan, loopback).

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
| Core request path | 10 | 0.60× | 0.61× | 0 of 10 |
| Web packages | 27 | 0.78× | 0.78× | 2 of 27 |
| All | 37 | 0.72× | 0.73× | 2 of 37 |

## Throughput and latency

Requests per second over the measured phase (WebSocket: messages per second). † marks a server below 85% CPU, where the load generator may have set the pace; compare efficiency there instead.

| Scenario | Cohesion req/s | ASP.NET Core req/s | Ratio | Cohesion p50 / p99 (ms) | ASP.NET Core p50 / p99 (ms) |
| --- | ---: | ---: | ---: | ---: | ---: |
| `plaintext` | 101,991 | 124,172 | 0.82× | 0.57 / 1.33 | 0.48 / 1.10 |
| `plaintext-pipelined` | 348,984 | 1,457,723 | 0.24× | 2.53 / 6.05 | 0.64 / 2.68 |
| `plaintext-routed` | 96,124 | 124,325 | 0.77× | 0.62 / 1.24 | 0.49 / 0.87 |
| `json` | 93,482 | 124,646 | 0.75× | 0.63 / 1.31 | 0.49 / 0.87 |
| `routing-table` | 74,570 | 122,489 | 0.61× | 0.78 / 1.88 | 0.50 / 0.88 |
| `query-binding` | 90,905 | 125,197 | 0.73× | 0.64 / 1.41 | 0.49 / 0.86 |
| `json-post` | 67,392 | 102,115 | 0.66× | 0.84 / 2.22 | 0.58 / 1.12 |
| `large-response` | 29,925 † | 77,887 | 0.38× | 1.42 / 16.64 | 0.79 / 1.56 |
| `middleware-chain` | 95,833 | 125,302 | 0.76× | 0.60 / 1.33 | 0.49 / 0.88 |
| `full-stack` | 56,217 | 95,056 | 0.59× | 0.97 / 3.04 | 0.63 / 1.24 |
| `security-headers` | 91,653 | 125,503 | 0.73× | 0.65 / 1.35 | 0.49 / 0.87 |
| `forwarded-headers` | 79,711 | 119,647 | 0.67× | 0.74 / 1.53 | 0.51 / 0.92 |
| `host-filtering` | 97,112 | 122,404 | 0.79× | 0.61 / 1.23 | 0.50 / 0.86 |
| `https-redirection` | 95,613 | 124,287 | 0.77× | 0.62 / 1.26 | 0.50 / 0.85 |
| `error-handling` | 84,165 | 102,934 | 0.82× | 0.70 / 1.48 | 0.57 / 1.64 |
| `rewrite` | 88,632 | 120,177 | 0.74× | 0.65 / 1.54 | 0.51 / 0.91 |
| `cookie-policy` | 91,360 | 120,611 | 0.76× | 0.64 / 1.42 | 0.51 / 0.91 |
| `compression-gzip` | 44,973 | 68,244 | 0.66× | 1.16 / 4.90 | 0.81 / 3.55 |
| `compression-brotli` | 73,007 | 94,618 | 0.77× | 0.79 / 1.80 | 0.64 / 1.26 |
| `request-decompression` | 62,618 | 87,880 | 0.71× | 0.94 / 2.11 | 0.71 / 1.86 |
| `static-file` | 12,879 † | 22,445 † | 0.57× | 4.80 / 9.86 | 2.68 / 5.39 |
| `auth-bearer` | 51,912 | 103,854 | 0.50× | 1.07 / 2.81 | 0.56 / 1.29 |
| `auth-cookie` | 41,997 | 101,627 | 0.41× | 1.31 / 4.13 | 0.58 / 1.28 |
| `authorization` | 49,961 | 97,963 | 0.51× | 1.10 / 3.14 | 0.58 / 1.58 |
| `antiforgery` | 57,311 | 94,642 | 0.61× | 0.99 / 2.50 | 0.64 / 1.24 |
| `cors-preflight` | 81,754 | 127,805 | 0.64× | 0.72 / 1.46 | 0.48 / 0.85 |
| `cors-request` | 84,182 | 122,280 | 0.69× | 0.70 / 1.44 | 0.50 / 0.87 |
| `request-timeouts` | 96,556 | 122,763 | 0.79× | 0.61 / 1.22 | 0.50 / 0.91 |
| `rate-limiting` | 94,928 | 121,674 | 0.78× | 0.63 / 1.21 | 0.50 / 0.91 |
| `forms` | 68,387 | 118,226 | 0.58× | 0.81 / 2.43 | 0.51 / 0.94 |
| `output-cache` | 93,426 | 118,805 | 0.79× | 0.64 / 1.30 | 0.51 / 0.93 |
| `sessions` | 83,012 | 102,038 | 0.81× | 0.70 / 1.56 | 0.58 / 1.14 |
| `health` | 93,261 | 116,156 | 0.80× | 0.63 / 1.37 | 0.51 / 1.08 |
| `openapi` | 77,757 | 3,567 | 21.80× | 0.70 / 2.43 | 18.79 / 35.74 |
| `validation-valid` | 69,598 | 116,941 | 0.60× | 0.79 / 2.63 | 0.51 / 0.92 |
| `validation-invalid` | 66,380 | 109,873 | 0.60× | 0.84 / 2.32 | 0.54 / 1.05 |
| `websocket-echo` | 129,928 | 129,664 | 1.00× | 0.45 / 0.96 | 0.48 / 0.82 |

## Efficiency and footprint

CPU is the server's share of its pinned cores. Requests per CPU-second divides completed requests by the server's CPU time, so it holds even when a run is client-bound. Startup is process start to the first HTTP response. Working set is measured after the run.

| Scenario | CPU % (C / A) | Req per CPU-s (C / A) | Ratio | Startup ms (C / A) | Working set MB (C / A) |
| --- | ---: | ---: | ---: | ---: | ---: |
| `plaintext` | 90 / 92 | 28,207 / 33,904 | 0.83× | 572 / 620 | 77.5 / 47.2 |
| `plaintext-pipelined` | 93 / 89 | 94,173 / 410,840 | 0.23× | 755 / 657 | 69.0 / 47.3 |
| `plaintext-routed` | 94 / 95 | 25,683 / 32,610 | 0.79× | 673 / 650 | 81.0 / 50.0 |
| `json` | 91 / 92 | 25,634 / 33,934 | 0.76× | 633 / 622 | 82.7 / 88.4 |
| `routing-table` | 94 / 95 | 19,932 / 32,355 | 0.62× | 633 / 672 | 81.7 / 85.6 |
| `query-binding` | 94 / 96 | 24,086 / 32,552 | 0.74× | 647 / 587 | 82.9 / 85.4 |
| `json-post` | 95 / 94 | 17,795 / 27,113 | 0.66× | 663 / 595 | 90.1 / 88.0 |
| `large-response` | 77 / 95 | 9,758 / 20,541 | 0.48× | 705 / 656 | 306.0 / 87.5 |
| `middleware-chain` | 91 / 95 | 26,415 / 33,003 | 0.80× | 657 / 635 | 81.5 / 48.5 |
| `full-stack` | 94 / 95 | 14,884 / 25,144 | 0.59× | 652 / 606 | 88.0 / 110.0 |
| `security-headers` | 93 / 94 | 24,765 / 33,490 | 0.74× | 794 / 674 | 77.4 / 51.6 |
| `forwarded-headers` | 94 / 93 | 21,294 / 32,152 | 0.66× | 600 / 591 | 81.5 / 85.2 |
| `host-filtering` | 93 / 94 | 26,029 / 32,515 | 0.80× | 573 / 658 | 81.2 / 49.6 |
| `https-redirection` | 92 / 95 | 25,911 / 32,578 | 0.80× | 653 / 732 | 80.4 / 79.9 |
| `error-handling` | 93 / 92 | 22,669 / 28,089 | 0.81× | 618 / 587 | 82.3 / 88.1 |
| `rewrite` | 94 / 95 | 23,687 / 31,711 | 0.75× | 711 / 659 | 83.3 / 85.4 |
| `cookie-policy` | 93 / 94 | 24,628 / 32,014 | 0.77× | 615 / 639 | 82.0 / 85.1 |
| `compression-gzip` | 93 / 93 | 12,057 / 18,296 | 0.66× | 605 / 624 | 77.0 / 84.6 |
| `compression-brotli` | 94 / 94 | 19,324 / 25,078 | 0.77× | 702 / 581 | 79.4 / 91.2 |
| `request-decompression` | 94 / 94 | 16,605 / 23,447 | 0.71× | 669 / 647 | 76.4 / 86.5 |
| `static-file` | 80 / 70 | 4,049 / 7,966 | 0.51× | 599 / 635 | 92.3 / 85.9 |
| `auth-bearer` | 95 / 94 | 13,665 / 27,615 | 0.49× | 629 / 683 | 76.7 / 93.7 |
| `auth-cookie` | 94 / 94 | 11,214 / 26,931 | 0.42× | 677 / 693 | 77.3 / 97.8 |
| `authorization` | 95 / 94 | 13,198 / 26,047 | 0.51× | 633 / 659 | 77.9 / 96.5 |
| `antiforgery` | 93 / 96 | 15,325 / 24,727 | 0.62× | 782 / 659 | 76.1 / 99.9 |
| `cors-preflight` | 93 / 95 | 22,066 / 33,771 | 0.65× | 633 / 646 | 80.8 / 86.1 |
| `cors-request` | 94 / 95 | 22,307 / 32,134 | 0.69× | 630 / 654 | 82.4 / 86.2 |
| `request-timeouts` | 94 / 93 | 25,587 / 32,827 | 0.78× | 716 / 566 | 80.5 / 85.8 |
| `rate-limiting` | 93 / 93 | 25,422 / 32,806 | 0.77× | 720 / 688 | 80.8 / 88.8 |
| `forms` | 94 / 93 | 18,187 / 31,872 | 0.57× | 616 / 615 | 83.8 / 84.3 |
| `output-cache` | 93 / 95 | 25,133 / 31,316 | 0.80× | 580 / 629 | 79.7 / 85.2 |
| `sessions` | 93 / 95 | 22,361 / 26,899 | 0.83× | 662 / 681 | 78.2 / 100.3 |
| `health` | 92 / 92 | 25,378 / 31,706 | 0.80× | 734 / 592 | 82.0 / 84.4 |
| `openapi` | 93 / 95 | 20,833 / 943 | 22.09× | 657 / 612 | 98.1 / 103.2 |
| `validation-valid` | 93 / 94 | 18,710 / 30,939 | 0.60× | 612 / 666 | 85.7 / 90.7 |
| `validation-invalid` | 95 / 93 | 17,456 / 29,404 | 0.59× | 648 / 801 | 91.0 / 90.2 |
| `websocket-echo` | 93 / 93 | 34,837 / 34,942 | 1.00× | 668 / 665 | 78.2 / 49.0 |

## Response parity

The single validation exchange each server answered before load: status, response-head bytes and body bytes. Differences in head size are framework header choices (for example Kestrel's `Server` header).

| Scenario | Cohesion | ASP.NET Core |
| --- | --- | --- |
| `plaintext` | 200, head 102 B, body 13 B | 200, head 119 B, body 13 B |
| `plaintext-pipelined` | 200, head 102 B, body 13 B | 200, head 119 B, body 13 B |
| `plaintext-routed` | 200, head 117 B, body 13 B | 200, head 142 B, body 13 B |
| `json` | 200, head 137 B, body 27 B | 200, head 148 B, body 27 B |
| `routing-table` | 200, head 116 B, body 3 B | 200, head 142 B, body 3 B |
| `query-binding` | 200, head 136 B, body 2 B | 200, head 148 B, body 2 B |
| `json-post` | 200, head 138 B, body 598 B | 200, head 148 B, body 598 B |
| `large-response` | 200, head 105 B, body 65536 B | 200, head 122 B, body 65536 B |
| `middleware-chain` | 200, head 102 B, body 13 B | 200, head 119 B, body 13 B |
| `full-stack` | 200, head 317 B, body 27 B | 200, head 303 B, body 27 B |
| `security-headers` | 200, head 271 B, body 2 B | 200, head 297 B, body 2 B |
| `forwarded-headers` | 200, head 117 B, body 17 B | 200, head 142 B, body 17 B |
| `host-filtering` | 200, head 116 B, body 2 B | 200, head 142 B, body 2 B |
| `https-redirection` | 307, head 130 B, body 0 B | 307, head 147 B, body 0 B |
| `error-handling` | 500, head 135 B, body 67 B | 500, head 225 B, body 168 B |
| `rewrite` | 200, head 116 B, body 7 B | 200, head 142 B, body 7 B |
| `cookie-policy` | 200, head 171 B, body 2 B | 200, head 197 B, body 2 B |
| `compression-gzip` | 200, head 165 B, body 175 B | 200, head 189 B, body 175 B |
| `compression-brotli` | 200, head 162 B, body 90 B | 200, head 187 B, body 179 B |
| `request-decompression` | 200, head 116 B, body 4 B | 200, head 142 B, body 4 B |
| `static-file` | 200, head 202 B, body 4096 B | 200, head 214 B, body 4096 B |
| `auth-bearer` | 200, head 116 B, body 5 B | 200, head 142 B, body 5 B |
| `auth-cookie` | 200, head 116 B, body 5 B | 200, head 142 B, body 5 B |
| `authorization` | 200, head 116 B, body 5 B | 200, head 142 B, body 5 B |
| `antiforgery` | 200, head 116 B, body 8 B | 200, head 133 B, body 8 B |
| `cors-preflight` | 204, head 128 B, body 0 B | 204, head 131 B, body 0 B |
| `cors-request` | 200, head 180 B, body 8 B | 200, head 192 B, body 8 B |
| `request-timeouts` | 200, head 116 B, body 2 B | 200, head 142 B, body 2 B |
| `rate-limiting` | 200, head 116 B, body 2 B | 200, head 142 B, body 2 B |
| `forms` | 200, head 117 B, body 10 B | 200, head 142 B, body 10 B |
| `output-cache` | 200, head 116 B, body 6 B | 200, head 142 B, body 6 B |
| `sessions` | 200, head 116 B, body 1 B | 200, head 142 B, body 1 B |
| `health` | 200, head 166 B, body 159 B | 200, head 226 B, body 110 B |
| `openapi` | 200, head 199 B, body 8165 B | 200, head 147 B, body 5820 B |
| `validation-valid` | 200, head 117 B, body 11 B | 200, head 101 B, body 11 B |
| `validation-invalid` | 400, head 126 B, body 337 B | 400, head 157 B, body 153 B |
| `websocket-echo` | 101, head 0 B, body 32 B | 101, head 0 B, body 32 B |

<details><summary>Response heads</summary>

### `plaintext`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 01:56:52 GMT
Content-Type: text/plain
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 13
Content-Type: text/plain
Date: Sun, 18 Oct 2026 01:57:25 GMT
Server: Kestrel

Hello, World!
```

### `plaintext-pipelined`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 01:58:49 GMT
Content-Type: text/plain
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 13
Content-Type: text/plain
Date: Sun, 18 Oct 2026 01:58:15 GMT
Server: Kestrel

Hello, World!
```

### `plaintext-routed`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:01:02 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:00:29 GMT
Server: Kestrel
Transfer-Encoding: chunked

Hello, World!
```

### `json`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:02:09 GMT
Vary: Accept
Content-Type: application/json; charset=utf-8
Content-Length: 27

{"message":"Hello, World!"}
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Sun, 18 Oct 2026 02:01:35 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"message":"Hello, World!"}
```

### `routing-table`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:04:23 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 3

0:1
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:02:59 GMT
Server: Kestrel
Transfer-Encoding: chunked

0:1
```

### `query-binding`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:04:40 GMT
Vary: Accept
Content-Type: application/json; charset=utf-8
Content-Length: 2

42
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Sun, 18 Oct 2026 02:06:03 GMT
Server: Kestrel
Transfer-Encoding: chunked

42
```

### `json-post`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:07:44 GMT
Vary: Accept
Content-Type: application/json; charset=utf-8
Content-Length: 598

{"id":1042,"customer":"Ada Lovelace","createdAt":"2026-10-17T12:00:00+00:00","lines":[{"sku":"SKU-0001","description":"Analytical engine punch cards","quantity"...
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Sun, 18 Oct 2026 02:07:27 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"id":1042,"customer":"Ada Lovelace","createdAt":"2026-10-17T12:00:00+00:00","lines":[{"sku":"SKU-0001","description":"Analytical engine punch cards","quantity"...
```

### `large-response`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:09:08 GMT
Content-Type: text/plain
Content-Length: 65536

The quick brown fox jumps over the lazy dog. 0123456789
The quick brown fox jumps over the lazy dog. 0123456789
The quick brown fox jumps over the lazy dog. 012...
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 65536
Content-Type: text/plain
Date: Sun, 18 Oct 2026 02:08:17 GMT
Server: Kestrel

The quick brown fox jumps over the lazy dog. 0123456789
The quick brown fox jumps over the lazy dog. 0123456789
The quick brown fox jumps over the lazy dog. 012...
```

### `middleware-chain`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:09:58 GMT
Content-Type: text/plain
Content-Length: 13

Hello, World!
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 13
Content-Type: text/plain
Date: Sun, 18 Oct 2026 02:10:31 GMT
Server: Kestrel

Hello, World!
```

### `full-stack`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:12:28 GMT
Vary: Origin, Accept, Accept-Encoding
Content-Type: application/json; charset=utf-8
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
Content-Security-Policy: frame-ancestors 'none'
Referrer-Policy: strict-origin-when-cross-origin
Content-Length: 27

{"message":"Hello, World!"}
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Sun, 18 Oct 2026 02:11:38 GMT
Server: Kestrel
Transfer-Encoding: chunked
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
Content-Security-Policy: frame-ancestors 'none'
Referrer-Policy: strict-origin-when-cross-origin

{"message":"Hello, World!"}
```

### `security-headers`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:13:19 GMT
Content-Type: text/plain; charset=utf-8
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
Content-Security-Policy: frame-ancestors 'none'
Referrer-Policy: strict-origin-when-cross-origin
Content-Length: 2

ok
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:13:52 GMT
Server: Kestrel
Transfer-Encoding: chunked
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
Content-Security-Policy: frame-ancestors 'none'
Referrer-Policy: strict-origin-when-cross-origin

ok
```

### `forwarded-headers`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:14:42 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 17

Https|203.0.113.7
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:16:05 GMT
Server: Kestrel
Transfer-Encoding: chunked

https|203.0.113.7
```

### `host-filtering`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:16:39 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 2

ok
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:16:22 GMT
Server: Kestrel
Transfer-Encoding: chunked

ok
```

### `https-redirection`

Cohesion:

```http
HTTP/1.1 307 Temporary Redirect
Date: Sun, 18 Oct 2026 02:19:09 GMT
Location: https://127.0.0.1/cart?id=7
Content-Length: 0
```

ASP.NET Core:

```http
HTTP/1.1 307 Temporary Redirect
Content-Length: 0
Date: Sun, 18 Oct 2026 02:19:26 GMT
Server: Kestrel
Location: https://127.0.0.1/cart?id=7
```

### `error-handling`

Cohesion:

```http
HTTP/1.1 500 Internal Server Error
Content-Type: application/problem+json
Content-Length: 67
Date: Sun, 18 Oct 2026 02:20:16 GMT

{"type":"about:blank","title":"Internal Server Error","status":500}
```

ASP.NET Core:

```http
HTTP/1.1 500 Internal Server Error
Content-Type: application/problem+json
Date: Sun, 18 Oct 2026 02:20:33 GMT
Server: Kestrel
Cache-Control: no-cache,no-store
Expires: -1
Pragma: no-cache
Transfer-Encoding: chunked

{"type":"https://tools.ietf.org/html/rfc9110#section-15.6.1","title":"An error occurred while processing your request.","status":500,"traceId":"0HNPCNPHNVH50:00...
```

### `rewrite`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:22:30 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 7

Item 42
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:21:40 GMT
Server: Kestrel
Transfer-Encoding: chunked

Item 42
```

### `cookie-policy`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:23:37 GMT
Set-Cookie: tracking=1; Path=/; Secure; SameSite=None
Content-Type: text/plain; charset=utf-8
Content-Length: 2

ok
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:23:03 GMT
Server: Kestrel
Set-Cookie: tracking=1; path=/; secure; samesite=none
Transfer-Encoding: chunked

ok
```

### `compression-gzip`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:25:51 GMT
Content-Type: text/plain; charset=utf-8
Vary: Accept-Encoding
Content-Encoding: gzip
Content-Length: 175
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:25:00 GMT
Server: Kestrel
Content-Encoding: gzip
Transfer-Encoding: chunked
Vary: Accept-Encoding
```

### `compression-brotli`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:27:48 GMT
Content-Type: text/plain; charset=utf-8
Vary: Accept-Encoding
Content-Encoding: br
Content-Length: 90
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:27:14 GMT
Server: Kestrel
Content-Encoding: br
Transfer-Encoding: chunked
Vary: Accept-Encoding
```

### `request-decompression`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:28:55 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 4

8192
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:28:21 GMT
Server: Kestrel
Transfer-Encoding: chunked

8192
```

### `static-file`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:30:02 GMT
Content-Type: text/plain
Content-Length: 4096
Accept-Ranges: bytes
ETag: "8df2cb7e769b01d-1000"
Last-Modified: Sun, 18 Oct 2026 01:34:08 GMT

The quick brown fox jumps over the lazy dog. 0123456789
The quick brown fox jumps over the lazy dog. 0123456789
The quick brown fox jumps over the lazy dog. 012...
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 4096
Content-Type: text/plain
Date: Sun, 18 Oct 2026 02:29:45 GMT
Server: Kestrel
Accept-Ranges: bytes
ETag: "1dd5ea0c4c4f800"
Last-Modified: Sun, 18 Oct 2026 01:34:08 GMT

The quick brown fox jumps over the lazy dog. 0123456789
The quick brown fox jumps over the lazy dog. 0123456789
The quick brown fox jumps over the lazy dog. 012...
```

### `auth-bearer`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:32:33 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:32:49 GMT
Server: Kestrel
Transfer-Encoding: chunked

alice
```

### `auth-cookie`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:33:23 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:33:56 GMT
Server: Kestrel
Transfer-Encoding: chunked

alice
```

### `authorization`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:35:37 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 5

alice
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:36:10 GMT
Server: Kestrel
Transfer-Encoding: chunked

alice
```

### `antiforgery`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:37:01 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 8

accepted
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Length: 8
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:37:34 GMT
Server: Kestrel

accepted
```

### `cors-preflight`

Cohesion:

```http
HTTP/1.1 204 No Content
Date: Sun, 18 Oct 2026 02:39:15 GMT
Access-Control-Allow-Origin: https://app.example
Vary: Origin
```

ASP.NET Core:

```http
HTTP/1.1 204 No Content
Date: Sun, 18 Oct 2026 02:39:31 GMT
Server: Kestrel
Access-Control-Allow-Origin: https://app.example
```

### `cors-request`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:41:12 GMT
Vary: Origin
Content-Type: text/plain; charset=utf-8
Access-Control-Allow-Origin: https://app.example
Content-Length: 8

products
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:39:48 GMT
Server: Kestrel
Access-Control-Allow-Origin: https://app.example
Transfer-Encoding: chunked

products
```

### `request-timeouts`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:41:29 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 2

ok
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:41:45 GMT
Server: Kestrel
Transfer-Encoding: chunked

ok
```

### `rate-limiting`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:43:26 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 2

ok
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:43:09 GMT
Server: Kestrel
Transfer-Encoding: chunked

ok
```

### `forms`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:45:56 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 10

Ada:csharp
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:45:06 GMT
Server: Kestrel
Transfer-Encoding: chunked

Ada:csharp
```

### `output-cache`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:46:46 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 6

cached
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:46:30 GMT
Server: Kestrel
Transfer-Encoding: chunked

cached
```

### `sessions`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:49:17 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 1

2
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: text/plain; charset=utf-8
Date: Sun, 18 Oct 2026 02:48:27 GMT
Server: Kestrel
Transfer-Encoding: chunked

2
```

### `health`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:50:24 GMT
Cache-Control: no-store, no-cache
Content-Type: application/health+json; charset=utf-8
Content-Length: 159

{"status":"Healthy","totalDurationMs":2.2832,"entries":{"self":{"status":"Healthy","durationMs":0.5377,"description":"The application is running.","tags":[]}}}
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: application/json
Date: Sun, 18 Oct 2026 02:49:50 GMT
Server: Kestrel
Cache-Control: no-store, no-cache
Expires: Thu, 01 Jan 1970 00:00:00 GMT
Pragma: no-cache
Transfer-Encoding: chunked

{"status":"Healthy","checks":[{"name":"self","status":"Healthy","description":"The application is running."}]}
```

### `openapi`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:52:38 GMT
ETag: "9c52172be44509c7b8465dbf86041e7e863ae7441530f5f3d711ce4f6de23426"
Content-Type: application/json; charset=utf-8
Content-Length: 8165

{
  "openapi": "3.1.2",
  "info": {
    "title": "cohesion-server",
    "version": "1.0.0"
  },
  "paths": {
    "/customers": {
      "post": {
      ...
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Content-Type: application/json;charset=utf-8
Date: Sun, 18 Oct 2026 02:52:54 GMT
Server: Kestrel
Transfer-Encoding: chunked

{
  "openapi": "3.1.1",
  "info": {
    "title": "aspnetcore-server | v1",
    "version": "1.0.0"
  },
  "servers": [
    {
      "url": "http://127.0.0.1:55257...
```

### `validation-valid`

Cohesion:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:53:28 GMT
Content-Type: text/plain; charset=utf-8
Content-Length: 11

created Ada
```

ASP.NET Core:

```http
HTTP/1.1 200 OK
Date: Sun, 18 Oct 2026 02:54:01 GMT
Server: Kestrel
Transfer-Encoding: chunked

created Ada
```

### `validation-invalid`

Cohesion:

```http
HTTP/1.1 400 Bad Request
Date: Sun, 18 Oct 2026 02:55:42 GMT
Content-Type: application/problem+json
Content-Length: 337

{"type":"about:blank","title":"Bad Request","status":400,"detail":"One or more validation errors occurred.","errors":{"name":["The following expression: \u0027c...
```

ASP.NET Core:

```http
HTTP/1.1 400 Bad Request
Content-Type: application/json; charset=utf-8
Date: Sun, 18 Oct 2026 02:55:08 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"title":"One or more validation errors occurred.","errors":{"Name":["The Name field is required."],"Age":["The field Age must be between 18 and 150."]}}
```

### `websocket-echo`

Cohesion:

```http
HTTP/1.1 101 Switching Protocols

cohesion-benchmarks websocket ok
```

ASP.NET Core:

```http
HTTP/1.1 101 Switching Protocols

cohesion-benchmarks websocket ok
```

</details>

## Scenarios

| Scenario | Server pipeline | What it measures |
| --- | --- | --- |
| `plaintext` | `plaintext` | Terminal middleware writes 13 bytes with Content-Length; no routing. |
| `plaintext-pipelined` | `plaintext` | The plaintext pipeline with 16 requests pipelined per connection (TechEmpower's plaintext depth). |
| `plaintext-routed` | `plaintext-routed` | MapGet returning a string, through the router. |
| `json` | `json` | MapGet returning a record serialized with a source-generated JSON context. |
| `routing-table` | `routing-table` | 100 parameterized routes with an int constraint; requests rotate across 10 of them. |
| `query-binding` | `query-binding` | Two int parameters bound from the query string; the sum written as JSON. |
| `json-post` | `json-post` | A 598-byte JSON order bound from the body and echoed back. |
| `large-response` | `large-response` | A 64 KiB body written in one call with Content-Length. |
| `middleware-chain` | `middleware-chain` | 10 pass-through Use((context, next) => next(context)) ahead of the plaintext terminal. |
| `full-stack` | `full-stack` | Nineteen middleware in MIDDLEWARE_ORDER.md order ahead of the json endpoint; every one takes its pass-through path. |
| `security-headers` | `security-headers` | Web.SecurityHeaders default policy. *ASP.NET Core has no security-headers middleware; its side is a hand-written OnStarting middleware writing the same headers.* |
| `forwarded-headers` | `forwarded-headers` | X-Forwarded-For/Proto from a loopback proxy applied to the effective client. |
| `host-filtering` | `host-filtering` | Host header checked against a two-entry allowlist. |
| `https-redirection` | `https-redirection` | Plaintext request redirected 307 to https. |
| `error-handling` | `error-handling` | Handler throws; the exception boundary writes a 500 problem-details body. *Dominated by the cost of throwing and catching an exception in either framework.* |
| `rewrite` | `rewrite` | A regex rewrite (/legacy/items/42 to /items/42) ahead of routing. |
| `cookie-policy` | `cookie-policy` | An appended SameSite=None cookie passes through the cookie policy. |
| `compression-gzip` | `compression` | An 8 KiB text response gzip-compressed at CompressionLevel.Fastest. |
| `compression-brotli` | `compression` | An 8 KiB text response Brotli-compressed at CompressionLevel.Fastest. |
| `request-decompression` | `request-decompression` | A gzip request body (158 bytes, 8 KiB decompressed) read to the end. |
| `static-file` | `static-files` | A 4 KiB file from wwwroot. |
| `auth-bearer` | `auth-bearer` | An HS256 JWT validated (signature, issuer, audience, lifetime) on every request. |
| `auth-cookie` | `auth-cookie` | A data-protected authentication cookie decrypted on every request; each connection signs in once. |
| `authorization` | `authorization` | JWT authentication plus a role policy on the endpoint. |
| `antiforgery` | `antiforgery` | A POST validated against the antiforgery cookie and header token; each connection fetches its own pair. |
| `cors-preflight` | `cors` | A CORS preflight answered by the middleware. |
| `cors-request` | `cors` | A cross-origin GET that gets Access-Control-Allow-Origin. |
| `request-timeouts` | `request-timeouts` | A 30-second default request timeout armed and disarmed per request. |
| `rate-limiting` | `rate-limiting` | A global fixed-window limiter (int.MaxValue permits/minute) consulted on every request. *Both use System.Threading.RateLimiting's FixedWindowRateLimiter; the difference is the middleware around it.* |
| `forms` | `forms` | A 173-byte urlencoded form with 10 fields parsed; two read. |
| `output-cache` | `output-cache` | A response served from the in-memory output cache. |
| `sessions` | `sessions` | An in-memory session loaded, an int read and written, and committed per request; one session per connection. |
| `health` | `health` | A JSON health report with one healthy check. *ASP.NET Core's side uses UseHealthChecks with a JSON writer, since Cohesion's MapHealthChecks needs no router and writes JSON.* |
| `openapi` | `openapi` | The OpenAPI document for five typed endpoints. *Measures each framework's document-serving strategy as shipped (cached or regenerated); the documents are not byte-identical.* |
| `validation-valid` | `validation` | A JSON body bound and validated (3 rules) and accepted. *Cohesion validates with an ObjectValidation profile; ASP.NET Core with .NET 10's DataAnnotations validation.* |
| `validation-invalid` | `validation` | A JSON body that fails validation, answered 400 with problem details. |
| `websocket-echo` | `websockets` | 32-byte text messages echoed over one WebSocket per connection. *Throughput is messages per second (one round trip each), not HTTP requests.* |

