using System.IO;

using Assimalign.Cohesion.Content.Binary;
using Assimalign.Cohesion.Content.Bmff;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Bmff;

/// <summary>
/// Loading the box tree of an MP4-shaped ISO BMFF file (<c>ftyp</c>, a two-track <c>moov</c>, a 1 MiB
/// <c>mdat</c>, <c>free</c>) from a seekable source: the first step of every inspection, fast-start or metadata
/// edit. Media stays lazy, so the cost is header and metadata parsing, not payload size. The fixture is built with
/// the package's own writer in setup. No widely used .NET BMFF library exists to compare against, so this tracks
/// Cohesion against itself.
/// </summary>
public class BmffDocumentBenchmarks
{
    private MemoryStream _source = null!;

    [GlobalSetup]
    public void Setup()
    {
        using BmffDocument document = new();
        document.Boxes.Add(new FileTypeBox(0)
        {
            MajorBrand = FourCC.Parse("isom").Value,
            MinorBrand = 0x200,
            CompatibleBrands = [FourCC.Parse("isom").Value, FourCC.Parse("iso2").Value, FourCC.Parse("mp41").Value],
        });

        MovieBox movie = new(0);
        movie.Children.Add(new MovieHeaderBox(0, 0) { TimeScale = 1000, Duration = 4000, NextTrackId = 3 });
        movie.Children.Add(CreateTrack(1, "vide"));
        movie.Children.Add(CreateTrack(2, "soun"));
        document.Boxes.Add(movie);

        byte[] media = new byte[1024 * 1024];
        for (int index = 0; index < media.Length; index++)
        {
            media[index] = (byte)index;
        }

        document.Boxes.Add(new MediaDataBox(0, 0) { Payload = BinarySegment.Create(media) });
        document.Boxes.Add(new FreeSpaceBox(0, 0) { Payload = BinarySegment.Create(new byte[16]) });

        _source = new MemoryStream();
        document.Save(_source);
    }

    [GlobalCleanup]
    public void Cleanup() => _source.Dispose();

    [Benchmark]
    public int Load()
    {
        _source.Position = 0;
        using BmffDocument document = BmffDocument.Load(_source);
        return document.Boxes.Count;
    }

    private static TrackBox CreateTrack(uint trackId, string handler)
    {
        TrackBox track = new(0, 0);
        track.Children.Add(new TrackHeaderBox(0, 0) { TrackId = trackId, Duration = 4000 });

        MediaBox media = new(0);
        media.Children.Add(new MediaHeaderBox(0, 0));
        media.Children.Add(new HandlerBox(0, 0) { HandlerType = FourCC.Parse(handler), Name = handler + " handler" });

        MediaInfoBox info = new(0);
        info.Children.Add(new SampleTableBox(0, 0));
        media.Children.Add(info);

        track.Children.Add(media);
        return track;
    }
}
