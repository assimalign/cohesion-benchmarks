# 2026-10-18: why authentication cost twice what ASP.NET Core's did

The 2026-10-17 results had Cohesion at 0.50x of ASP.NET Core for JWT bearer, 0.51x with a role policy and 0.41x for
cookie authentication. Measured against `plaintext-routed`, authentication added 34 µs (bearer) and 50 µs (cookie) of
server CPU per request, against 5.5 and 6.5 µs for ASP.NET Core.

**Cause: the HTTP/1.1 request head was read one octet at a time** (an asynchronous pipe read, a one-byte array and a
`MemoryStream` write per octet, about 80 ns each), so the 307-byte `Authorization` line cost 24 µs and the 484-byte
`Cookie` line 37 µs before authentication ran. The JWT and data-protection libraries were within 1.1–1.2x of
ASP.NET Core.

| File | |
| --- | --- |
| [triage-report.md](triage-report.md) | The investigation: fairness check, per-stage costs, ranked causes, proposed fixes |
| [ab/](ab/) | Before/after for [assimalign/cohesion#1663](https://github.com/assimalign/cohesion/pull/1663): `e2e-run{1,2}-{main,branch}.md` are the end-to-end runs (`main` at `74976cdc5`, branch at `40d670fa8`, run 1 branch first, run 2 main first); `http1-request-*` is the HTTP/1.1 request cycle; `micro-*` are the header-reader and auth micro-benchmarks from the first fix |
| [bdn/](bdn/) | BenchmarkDotNet reports from the investigation, including the prototype patches |
| [patches/](patches/) | `01` pooled AES-GCM handles in DataProtection and `02` a compact cookie format (both need an owner decision), `03` claims built on their identity (landed) |

The investigation's stage-level harness reached into Cohesion internals by reflection and was not kept; its
reproduction commands refer to that harness. The forward-looking comparisons now live in the Web suite
(`benchmarks/Assimalign.Cohesion.Web.Benchmarks`), the Http suite (the request-head reader) and the IdentityModel
and Security suites.
