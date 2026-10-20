using System;
using System.Buffers.Binary;
using System.Net;
using System.Text;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Dns.Benchmarks.Dns;

/// <summary>
/// Compression table reuse for repeated suffixes versus disjoint names, and decompression of identical
/// answers from compressed and literal-name packets. The public message codec owns name compression;
/// no internal calls or transport are involved. The literal packet is built only in setup.
/// DnsClient.NET has no public equivalent full message codec, so these run without a counterpart.
/// </summary>
public class DnsCompressionBenchmarks
{
    private DnsMessage _shared = null!;
    private DnsMessage _distinct = null!;
    private byte[] _compressed = null!;
    private byte[] _literal = null!;
    private byte[] _buffer = null!;

    [Params(4, 32)]
    public int AnswerCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        DnsRecord[] shared = new DnsRecord[AnswerCount];
        DnsRecord[] distinct = new DnsRecord[AnswerCount];
        for (int i = 0; i < AnswerCount; i++)
        {
            IPAddress address = new(new byte[] { 192, 0, 2, (byte)(i + 1) });
            shared[i] = new DnsARecord($"host{i:D2}.service.example.test", address, 300);
            distinct[i] = new DnsARecord($"host{i:D2}.service{i:D2}.example{i:D2}.test{i:D2}", address, 300);
        }

        DnsHeader header = new(42, DnsHeaderFlags.Response, DnsOpCode.Query, DnsResponseCode.NoError, 0, 0, 0, 0);
        _shared = new DnsMessage(header, [], shared, [], []);
        _distinct = new DnsMessage(header, [], distinct, [], []);
        _buffer = new byte[8192];
        int length = _shared.WriteTo(_buffer);
        _compressed = _buffer.AsSpan(0, length).ToArray();

        // Explicit uncompressed A records with the same header, names, addresses, class and TTL.
        _compressed.AsSpan(0, DnsHeader.WireSize).CopyTo(_buffer);
        int position = DnsHeader.WireSize;
        foreach (DnsARecord record in shared)
        {
            foreach (string label in record.Name.GetLabels())
            {
                _buffer[position++] = (byte)label.Length;
                position += Encoding.ASCII.GetBytes(label, _buffer.AsSpan(position));
            }
            _buffer[position++] = 0;
            BinaryPrimitives.WriteUInt16BigEndian(_buffer.AsSpan(position), (ushort)record.Type);
            BinaryPrimitives.WriteUInt16BigEndian(_buffer.AsSpan(position + 2), (ushort)record.Class);
            BinaryPrimitives.WriteUInt32BigEndian(_buffer.AsSpan(position + 4), record.TimeToLive);
            BinaryPrimitives.WriteUInt16BigEndian(_buffer.AsSpan(position + 8), 4);
            record.Address.GetAddressBytes().CopyTo(_buffer, position + 10);
            position += 14;
        }
        _literal = _buffer.AsSpan(0, position).ToArray();

        DnsMessage compressed = ParseCompressed();
        DnsMessage literal = ParseLiteral();
        if (_compressed.Length >= _literal.Length || compressed.Answers.Count != AnswerCount || literal.Answers.Count != AnswerCount)
        {
            throw new InvalidOperationException("Expected compressed and literal answers.");
        }
        for (int i = 0; i < AnswerCount; i++)
        {
            DnsARecord left = (DnsARecord)compressed.Answers[i];
            DnsARecord right = (DnsARecord)literal.Answers[i];
            if (left.Name != right.Name || !left.Address.Equals(right.Address) || left.TimeToLive != right.TimeToLive)
            {
                throw new InvalidOperationException("Compression changed an answer.");
            }
        }
        length = _distinct.WriteTo(_buffer);
        if (DnsMessage.Parse(_buffer.AsSpan(0, length)).Answers.Count != AnswerCount)
        {
            throw new InvalidOperationException("Distinct names did not round-trip.");
        }
    }

    [Benchmark] public int WriteSharedSuffixes() => _shared.WriteTo(_buffer);
    [Benchmark] public int WriteDistinctSuffixes() => _distinct.WriteTo(_buffer);
    [Benchmark] public DnsMessage ParseCompressed() => DnsMessage.Parse(_compressed);
    [Benchmark] public DnsMessage ParseLiteral() => DnsMessage.Parse(_literal);
}
