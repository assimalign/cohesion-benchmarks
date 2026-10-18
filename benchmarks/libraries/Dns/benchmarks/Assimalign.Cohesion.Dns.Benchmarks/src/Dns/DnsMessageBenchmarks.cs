using System;
using System.Net;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Dns.Benchmarks.Dns;

/// <summary>
/// The DNS wire format in both directions, the per-lookup cost under every transport (UDP, TCP, DoT, DoH, DoQ):
/// encoding a stub resolver's query (one question plus an EDNS OPT record), decoding a typical response (a CNAME
/// chain to four A records, with name compression pointers), and encoding that response as a server would. The
/// .NET base library has no public DNS message codec to compare against, so these track Cohesion against itself.
/// </summary>
public class DnsMessageBenchmarks
{
    private DnsMessage _query = null!;
    private DnsMessage _response = null!;
    private byte[] _responseBytes = null!;
    private byte[] _buffer = null!;

    [GlobalSetup]
    public void Setup()
    {
        DnsName name = "www.example.com";
        DnsName edge = "www.example.com.edgekey.net";
        DnsQuestion question = new(name, DnsRecordType.A);
        DnsOptRecord edns = new(1232);

        _query = new DnsMessage(
            new DnsHeader(0x2f1a, DnsHeaderFlags.RecursionDesired, DnsOpCode.Query, DnsResponseCode.NoError, 1, 0, 0, 1),
            [question],
            [],
            [],
            [edns]);

        _response = new DnsMessage(
            new DnsHeader(
                0x2f1a,
                DnsHeaderFlags.Response | DnsHeaderFlags.RecursionDesired | DnsHeaderFlags.RecursionAvailable,
                DnsOpCode.Query,
                DnsResponseCode.NoError,
                1,
                5,
                0,
                1),
            [question],
            [
                new DnsCnameRecord(name, edge, 300),
                new DnsARecord(edge, IPAddress.Parse("93.184.215.14"), 60),
                new DnsARecord(edge, IPAddress.Parse("93.184.215.15"), 60),
                new DnsARecord(edge, IPAddress.Parse("93.184.215.16"), 60),
                new DnsARecord(edge, IPAddress.Parse("93.184.215.17"), 60),
            ],
            [],
            [edns]);

        _buffer = new byte[1232];
        int length = _response.WriteTo(_buffer);
        _responseBytes = _buffer.AsSpan(0, length).ToArray();

        if (ParseResponse().Answers.Count != 5)
        {
            throw new InvalidOperationException("The response did not round-trip.");
        }
    }

    [Benchmark]
    public int WriteQuery() => _query.WriteTo(_buffer);

    [Benchmark]
    public DnsMessage ParseResponse() => DnsMessage.Parse(_responseBytes);

    [Benchmark]
    public int WriteResponse() => _response.WriteTo(_buffer);
}
