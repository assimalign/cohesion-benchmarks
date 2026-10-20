using System;
using System.Buffers;

using Assimalign.Cohesion.Content.Binary;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Binary;

/// <summary>
/// Reads and writes packed container records through the checked public cursor API. Each record has
/// a four-byte identifier, a 32-bit length and a 64-bit offset. No counterpart: unchecked primitives
/// alone do not supply the cursor and range validation that this API promises.
/// </summary>
public class BinaryReaderBenchmarks
{
    private byte[] _bytes = null!;
    private ArrayBufferWriter<byte> _output = null!;

    [Params(8, 128)]
    public int Records { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _output = new ArrayBufferWriter<byte>(Records * 16);
        WriteRecords();
        _bytes = _output.WrittenSpan.ToArray();
    }

    [Benchmark]
    public ulong ReadRecords()
    {
        BinaryContentReader reader = new(_bytes);
        ulong checksum = 0;
        for (int index = 0; index < Records; index++)
        {
            checksum += reader.ReadFourCC().Value;
            checksum += reader.ReadUInt32BigEndian();
            checksum += reader.ReadUInt64BigEndian();
        }
        return checksum;
    }

    [Benchmark]
    public int WriteRecords()
    {
        _output.Clear();
        BinaryContentWriter writer = new(_output);
        for (int index = 0; index < Records; index++)
        {
            writer.WriteFourCC(FourCC.FromUInt32(0x6d646174));
            writer.WriteUInt32BigEndian(32);
            writer.WriteUInt64BigEndian((ulong)index * 32);
        }
        return _output.WrittenCount;
    }
}
