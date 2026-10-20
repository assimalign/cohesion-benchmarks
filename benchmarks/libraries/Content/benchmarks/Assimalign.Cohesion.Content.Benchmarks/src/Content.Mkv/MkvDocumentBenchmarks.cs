using System;
using System.IO;
using System.Threading.Tasks;

using Assimalign.Cohesion.Content.Binary;
using Assimalign.Cohesion.Content.Ebml;
using Assimalign.Cohesion.Content.Media;
using Assimalign.Cohesion.Content.Mkv;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Mkv;

/// <summary>
/// Loads track metadata and scans samples across four clusters in a synthetic Matroska container.
/// The second case reuses an open file, measuring the cluster/block walker behind Content.Media's
/// async sample enumeration. No codec decoding or materialized sample list; no honest counterpart.
/// </summary>
public class MkvDocumentBenchmarks
{
    private byte[] _bytes = null!;
    private MemoryStream _source = null!;
    private MkvDocument _document = null!;

    [GlobalSetup]
    public async Task Setup()
    {
        using MemoryStream output = new();
        using (EbmlWriter writer = EbmlWriter.Create(output, MkvSchema.Matroska))
        {
            writer.WriteElement(new EbmlHeader { DocType = "matroska", DocTypeVersion = 4, DocTypeReadVersion = 4 }.Element);
            Start(writer, 0x18538067);
            Start(writer, 0x1549a966);
            writer.WriteUtf8String(Id(0x4d80), "benchmark");
            writer.WriteUtf8String(Id(0x5741), "benchmark");
            writer.WriteFloat(Id(0x4489), 4000, 8);
            writer.EndMaster();
            Start(writer, 0x1654ae6b);
            Start(writer, 0xae);
            writer.WriteUnsignedInteger(Id(0xd7), 1);
            writer.WriteUnsignedInteger(Id(0x73c5), 1);
            writer.WriteUnsignedInteger(Id(0x83), 1);
            writer.WriteAsciiString(Id(0x86), "V_VP9");
            Start(writer, 0xe0);
            writer.WriteUnsignedInteger(Id(0xb0), 64);
            writer.WriteUnsignedInteger(Id(0xba), 64);
            writer.EndMaster();
            writer.EndMaster();
            writer.EndMaster();
            for (ulong cluster = 0; cluster < 4; cluster++)
            {
                Start(writer, 0x1f43b675);
                writer.WriteUnsignedInteger(Id(0xe7), cluster * 1000);
                writer.WriteBinary(Id(0xa3), BinarySegment.Create(new byte[] { 0x81, 0, 0, 0x80, 1, 2, 3 }));
                writer.WriteBinary(Id(0xa3), BinarySegment.Create(new byte[] { 0x81, 1, 0xf4, 0, 4, 5, 6 }));
                writer.EndMaster();
            }
            writer.EndMaster();
        }
        _bytes = output.ToArray();
        _source = new MemoryStream(_bytes, writable: false);
        _document = MkvDocument.Load(_source);
        if (Load() != 1 || await ReadSamples() != 24) { throw new InvalidOperationException("Expected one track with eight three-byte MKV samples."); }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _document.Dispose();
        _source.Dispose();
    }

    [Benchmark]
    public int Load()
    {
        using MemoryStream source = new(_bytes, writable: false);
        using MkvDocument document = MkvDocument.Load(source);
        return document.Tracks.Count;
    }

    [Benchmark]
    public async Task<long> ReadSamples()
    {
        long length = 0;
        await foreach (MediaSample sample in _document.Tracks[0].Samples.ReadSamplesAsync())
        {
            length += sample.ByteLength;
        }
        return length;
    }

    private static VInt Id(ulong value) => VInt.FromEncoded(value);
    private static void Start(EbmlWriter writer, ulong id) => writer.BeginMaster(Id(id), sizeLength: 4, backPatch: true);
}
