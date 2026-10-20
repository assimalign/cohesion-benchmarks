using System;
using System.IO;
using System.Threading.Tasks;

using Assimalign.Cohesion.Content.Benchmarks.Content.Mpeg;
using Assimalign.Cohesion.Content.Media;
using Assimalign.Cohesion.Content.Mpeg;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Media;

/// <summary>
/// Uses Content.Media's public sample contract through the real MP4 implementation: random sample
/// lookup, presentation-time lookup and safe sync-to-target range seeking. Setup loads and warms
/// the lazy index, so these measure repeated queries over an open file. Content.Media defines shared
/// contracts; no artificial implementation or counterpart is substituted for a native container index.
/// </summary>
public class MediaSampleBenchmarks
{
    private MemoryStream _source = null!;
    private Mp4File _file = null!;
    private IMediaSampleIndex _samples = null!;
    private MediaTimestamp _time;

    [GlobalSetup]
    public async Task Setup()
    {
        _source = new MemoryStream(Mp4Input.Create(), writable: false);
        _file = Mp4File.Load(_source);
        _samples = _file.Tracks[0].Samples;
        _time = new MediaTimestamp(10500, 1000);
        if (await GetSample() is null || await FindSample() is null || await Seek() is null)
        {
            throw new InvalidOperationException("Expected an indexed sample and seek range.");
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _file.Dispose();
        _source.Dispose();
    }

    [Benchmark]
    public ValueTask<MediaSample?> GetSample() => _samples.GetSampleAsync(10);

    [Benchmark]
    public ValueTask<MediaSample?> FindSample() => _samples.FindSampleAsync(_time);

    [Benchmark]
    public ValueTask<MediaSeekResult?> Seek() => _samples.SeekAsync(_time);
}
