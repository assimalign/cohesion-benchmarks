using System;
using System.IO;

using Assimalign.Cohesion.Content.Binary;
using Assimalign.Cohesion.Content.Bmff;
using Assimalign.Cohesion.Content.Mpeg;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Mpeg;

/// <summary>
/// Loads a progressive MP4 with one video track and sixteen tiny placeholder samples, producing
/// track metadata and a lazy sample index. No codec decoding. Generic metadata extractors do not
/// build this sample index, so they would perform less work and are not used as counterparts.
/// </summary>
public class Mp4FileBenchmarks
{
    private byte[] _bytes = null!;

    [GlobalSetup]
    public void Setup()
    {
        _bytes = Mp4Input.Create();
        if (Load() != 1) { throw new InvalidOperationException("Expected one MP4 track."); }
    }

    [Benchmark]
    public int Load()
    {
        using MemoryStream source = new(_bytes, writable: false);
        using Mp4File file = Mp4File.Load(source);
        return file.Tracks.Count;
    }
}

// Original, in-code ISO BMFF layout. Payload bytes describe no real codec bitstream.
internal static class Mp4Input
{
    internal static byte[] Create()
    {
        FileTypeBox brand = new(0)
        {
            MajorBrand = FourCC.Parse("mp42").Value,
            CompatibleBrands = [FourCC.Parse("isom").Value, FourCC.Parse("mp42").Value],
        };
        using MemoryStream output = new();
        using BmffWriter writer = BmffWriter.Create(output);
        writer.Write(brand);
        uint dataOffset = (uint)output.Position + 8;
        byte[] payload = new byte[48];
        for (int index = 0; index < payload.Length; index++) { payload[index] = (byte)(index + 1); }
        writer.Write(new MediaDataBox(0, 0) { Payload = BinarySegment.Create(payload) });
        SampleTableBox table = Add(new SampleTableBox(0, 0),
            Add(new SampleDescriptionBox(0, 0),
                Add(new Avc1SampleEntryBox { Width = 640, Height = 360 },
                    new AvcConfigurationBox { Data = BinarySegment.Create(new byte[] { 1, 0x64, 0, 0x1f }) })),
            new SampleDecodingTimeBox(0, 0) { Entries = [new() { SampleCount = 16, SampleDelta = 1000 }] },
            new SampleToChunkBox(0, 0) { Entries = [new() { FirstChunk = 1, SamplesPerChunk = 16, SampleDescriptionIndex = 1 }] },
            new SampleSizeBox(0, 0) { SampleSize = 3, SampleCount = 16 },
            new ChunkOffsetBox(0, 0) { Entries = [dataOffset] },
            new SyncSampleTableBox(0, 0) { Entries = [1, 5, 9, 13] });
        TrackBox track = Add(new TrackBox(0, 0),
            new TrackHeaderBox(0, 0) { TrackId = 1, Flags = 3, Duration = 16000 },
            Add(new MediaBox(0, 0),
                new MediaHeaderBox(0, 0) { TimeScale = 1000, Duration = 16000, PackedLanguage = 0x15c7 },
                new HandlerBox(0, 0) { HandlerType = FourCC.Parse("vide") },
                Add(new MediaInfoBox(0, 0),
                    Add(new DataInfoBox(0, 0), Add(new DataReferenceBox(0, 0), new DataEntryUrlBox { Flags = 1 })), table)));
        writer.Write(Add(new MovieBox(0, 0), new MovieHeaderBox(0, 0) { TimeScale = 1000, Duration = 16000 }, track));
        return output.ToArray();
    }

    private static T Add<T>(T parent, params BmffBox[] children) where T : BmffBoxComposite
    {
        foreach (BmffBox child in children) { parent.Children.Add(child); }
        return parent;
    }
}
