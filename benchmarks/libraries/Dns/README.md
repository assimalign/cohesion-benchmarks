# DNS benchmarks

35 cases, all in-process wire work with deterministic inputs and no DNS resolution or sockets:

- The original query/response message cases remain unchanged (3 cases).
- Every supported record codec: A, AAAA, CNAME, NS, PTR, MX, TXT, SOA, SRV, OPT and unknown RDATA,
  encoded and parsed through `DnsMessage` (22 cases). OPT includes cookie, client subnet, extended error
  and unknown options. Setup checks the concrete parsed types and exact re-encoding.
- Compression table reuse for shared/disjoint suffixes, plus decompression of equivalent compressed
  and literal-name answer packets at 4 and 32 answers (8 cases).
- Question/EDNS and authoritative answer/glue construction followed by serialization (2 cases).

Only `Assimalign.Cohesion.Dns` is measured. `Dns.Client` resolver, cache, retry and transport behavior is
outside this wire-only suite. `Dns.Client.Doh`, `Dns.Client.Doq` and `Dns.Client.Dot` at the pinned source
revision have no implementation. No transport throughput, TLS, HTTP, QUIC, socket or real lookup is measured.
Malformed packet rejection, compression cycles and maximum-size messages are also not measured.

## Counterpart assessment

DnsClient.NET was considered. Its public
[`DnsDatagramReader`](https://github.com/MichaCo/DnsClient.NET/blob/1.8.0/src/DnsClient/DnsDatagramReader.cs)
exposes primitive and name reading, but the
[`DnsRecordFactory`](https://github.com/MichaCo/DnsClient.NET/blob/1.8.0/src/DnsClient/DnsRecordFactory.cs)
and
[`DnsMessageHandler`](https://github.com/MichaCo/DnsClient.NET/blob/1.8.0/src/DnsClient/DnsMessageHandler.cs)
that materialize records and read/write complete messages are internal. A reader-only baseline would do
less work than `DnsMessage.Parse`; custom record dispatch or reflection would measure an adapter or
non-consumer API. It is therefore not added. The checked source is Apache-2.0 licensed.

## Verify

```powershell
dotnet build benchmarks/libraries/Dns/benchmarks/Assimalign.Cohesion.Dns.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area Dns -Profile dry
```
