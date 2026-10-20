using System;
using System.Net;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Dns.Benchmarks.Dns;

/// <summary>
/// Each supported RDATA codec through the public message API, including the enclosing record header and
/// name compression table. Each fixture contains one record; OPT belongs in the additional section.
/// DnsClient.NET's public datagram reader does not expose its internal record factory or full message codec,
/// so comparing it with message parsing would omit record materialization and validation. No counterpart is used.
/// </summary>
public class DnsRecordBenchmarks
{
    private DnsMessage[] _messages = null!;
    private byte[][] _wire = null!;
    private byte[] _buffer = null!;

    [GlobalSetup]
    public void Setup()
    {
        DnsName owner = "example.test";
        DnsRecord[] records =
        [
            new DnsARecord(owner, IPAddress.Parse("192.0.2.42"), 300),
            new DnsAaaaRecord(owner, IPAddress.Parse("2001:db8::42"), 300),
            new DnsCnameRecord(owner, "edge.example.test", 300),
            new DnsNsRecord(owner, "ns1.example.test", 3600),
            new DnsPtrRecord("42.2.0.192.in-addr.arpa", owner, 300),
            new DnsMxRecord(owner, 10, "mail.example.test", 300),
            new DnsTxtRecord(owner, ["v=spf1 include:mail.example.test -all", "verification=benchmark-42"], 300),
            new DnsSoaRecord(owner, "ns1.example.test", "hostmaster.example.test", 2026101901, 3600, 600, 86400, 300, 3600),
            new DnsSrvRecord("_amqp._tcp.example.test", 10, 20, 5672, "broker.example.test", 300),
            new DnsOptRecord(1232, 0, 0, DnsEdnsFlags.None,
            [
                new DnsEdnsCookieOption(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, new byte[] { 8, 7, 6, 5, 4, 3, 2, 1 }),
                new DnsEdnsClientSubnetOption(IPAddress.Parse("192.0.2.0"), 24),
                new DnsEdnsExtendedErrorOption(3, "stale cached answer"),
                new DnsEdnsUnknownOption((DnsEdnsOptionCode)65001, new byte[] { 1, 2, 3, 4 }),
            ]),
            new DnsUnknownRecord(owner, (DnsRecordType)65280, DnsClass.IN, 300, new byte[] { 0, 1, 127, 128, 255 }),
        ];

        _messages = new DnsMessage[records.Length];
        _wire = new byte[records.Length][];
        _buffer = new byte[4096];
        for (int i = 0; i < records.Length; i++)
        {
            bool opt = records[i] is DnsOptRecord;
            _messages[i] = new DnsMessage(
                new DnsHeader(42, DnsHeaderFlags.Response, DnsOpCode.Query, DnsResponseCode.NoError, 0, 0, 0, 0),
                [], opt ? [] : [records[i]], [], opt ? [records[i]] : []);
            int length = _messages[i].WriteTo(_buffer);
            _wire[i] = _buffer.AsSpan(0, length).ToArray();
            DnsMessage parsed = DnsMessage.Parse(_wire[i]);
            DnsRecord result = opt ? parsed.Additionals[0] : parsed.Answers[0];
            int roundTripLength = parsed.WriteTo(_buffer);
            if (result.GetType() != records[i].GetType() ||
                !_wire[i].AsSpan().SequenceEqual(_buffer.AsSpan(0, roundTripLength)))
            {
                throw new InvalidOperationException($"The {records[i].Type} record did not round-trip.");
            }
        }
    }

    [Benchmark] public int WriteA() => _messages[0].WriteTo(_buffer);
    [Benchmark] public DnsMessage ParseA() => DnsMessage.Parse(_wire[0]);
    [Benchmark] public int WriteAaaa() => _messages[1].WriteTo(_buffer);
    [Benchmark] public DnsMessage ParseAaaa() => DnsMessage.Parse(_wire[1]);
    [Benchmark] public int WriteCname() => _messages[2].WriteTo(_buffer);
    [Benchmark] public DnsMessage ParseCname() => DnsMessage.Parse(_wire[2]);
    [Benchmark] public int WriteNs() => _messages[3].WriteTo(_buffer);
    [Benchmark] public DnsMessage ParseNs() => DnsMessage.Parse(_wire[3]);
    [Benchmark] public int WritePtr() => _messages[4].WriteTo(_buffer);
    [Benchmark] public DnsMessage ParsePtr() => DnsMessage.Parse(_wire[4]);
    [Benchmark] public int WriteMx() => _messages[5].WriteTo(_buffer);
    [Benchmark] public DnsMessage ParseMx() => DnsMessage.Parse(_wire[5]);
    [Benchmark] public int WriteTxt() => _messages[6].WriteTo(_buffer);
    [Benchmark] public DnsMessage ParseTxt() => DnsMessage.Parse(_wire[6]);
    [Benchmark] public int WriteSoa() => _messages[7].WriteTo(_buffer);
    [Benchmark] public DnsMessage ParseSoa() => DnsMessage.Parse(_wire[7]);
    [Benchmark] public int WriteSrv() => _messages[8].WriteTo(_buffer);
    [Benchmark] public DnsMessage ParseSrv() => DnsMessage.Parse(_wire[8]);
    [Benchmark] public int WriteEdnsOptions() => _messages[9].WriteTo(_buffer);
    [Benchmark] public DnsMessage ParseEdnsOptions() => DnsMessage.Parse(_wire[9]);
    [Benchmark] public int WriteUnknown() => _messages[10].WriteTo(_buffer);
    [Benchmark] public DnsMessage ParseUnknown() => DnsMessage.Parse(_wire[10]);
}
