using System;
using System.Net;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Dns.Benchmarks.Dns;

/// <summary>
/// Construct and serialize the per-request objects a resolver or authority needs: a question with EDNS,
/// or an authoritative answer with glue. Includes name validation, section allocation and wire encoding,
/// rather than timing empty constructors. No client or socket is created. No public DnsClient.NET equivalent.
/// </summary>
public class DnsConstructionBenchmarks
{
    private byte[] _buffer = null!;
    private IPAddress _answerAddress = null!;
    private IPAddress _nameServerAddress = null!;

    [GlobalSetup]
    public void Setup()
    {
        _buffer = new byte[1232];
        _answerAddress = IPAddress.Parse("192.0.2.42");
        _nameServerAddress = IPAddress.Parse("192.0.2.53");
        int queryLength = ConstructQuery();
        DnsMessage query = DnsMessage.Parse(_buffer.AsSpan(0, queryLength));
        int answerLength = ConstructAnswer();
        DnsMessage answer = DnsMessage.Parse(_buffer.AsSpan(0, answerLength));
        if (query.Questions.Count != 1 || query.Edns?.UdpPayloadSize != 1232 ||
            answer.Answers.Count != 1 || answer.Authorities.Count != 1 || answer.Additionals.Count != 1)
        {
            throw new InvalidOperationException("Constructed messages did not round-trip.");
        }
    }

    [Benchmark]
    public int ConstructQuery()
    {
        DnsQuestion question = new(new DnsName("www.example.test"), DnsRecordType.A);
        DnsMessage message = new(
            new DnsHeader(42, DnsHeaderFlags.RecursionDesired, DnsOpCode.Query, DnsResponseCode.NoError, 0, 0, 0, 0),
            [question], [], [], [new DnsOptRecord(1232)]);
        return message.WriteTo(_buffer);
    }

    [Benchmark]
    public int ConstructAnswer()
    {
        DnsName owner = new("www.example.test");
        DnsName zone = new("example.test");
        DnsName nameServer = new("ns1.example.test");
        DnsMessage message = new(
            new DnsHeader(42, DnsHeaderFlags.Response | DnsHeaderFlags.AuthoritativeAnswer,
                DnsOpCode.Query, DnsResponseCode.NoError, 0, 0, 0, 0),
            [new DnsQuestion(owner, DnsRecordType.A)],
            [new DnsARecord(owner, _answerAddress, 300)],
            [new DnsNsRecord(zone, nameServer, 3600)],
            [new DnsARecord(nameServer, _nameServerAddress, 3600)]);
        return message.WriteTo(_buffer);
    }
}
