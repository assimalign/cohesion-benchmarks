using System.IO;

using Assimalign.Cohesion.Content.Benchmarks.Content.Mpeg;
using Assimalign.Cohesion.Content.Bmff;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Bmff;

/// <summary>
/// Walks the box headers of a small MP4 without materializing leaves, descending through the movie,
/// track, media and sample-table containers. This is the public bounded cursor path rather than the
/// existing document-loading case. Metadata extractors perform a different operation; no counterpart.
/// </summary>
public class BmffCursorBenchmarks
{
    private byte[] _bytes = null!;

    [GlobalSetup]
    public void Setup() => _bytes = Mp4Input.Create();

    [Benchmark]
    public int WalkHeaders()
    {
        using MemoryStream source = new(_bytes, writable: false);
        using BmffReader reader = BmffReader.Create(source);
        return Walk(reader);
    }

    private static int Walk(BmffReader reader)
    {
        int count = 0;
        while (reader.Read())
        {
            count++;
            if (reader.Header.BoxType is BmffBoxType.Movie or BmffBoxType.Track or BmffBoxType.Media or BmffBoxType.MediaInfo or BmffBoxType.SampleTable)
            {
                using BmffReader children = reader.ReadChildren();
                count += Walk(children);
            }
        }
        return count;
    }
}
