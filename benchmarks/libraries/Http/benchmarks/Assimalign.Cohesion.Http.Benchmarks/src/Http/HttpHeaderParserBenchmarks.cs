using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using AspNetHeaders = Microsoft.Net.Http.Headers;
using NetHeaders = System.Net.Http.Headers;

namespace Assimalign.Cohesion.Http.Benchmarks.Http;

/// <summary>
/// Typed Content-Type and byte-range parsing on the request path. Each parser validates the
/// same valid input and returns its typed representation, including parameters or range bounds.
/// These inputs use the common grammar supported by all three implementations; this does not
/// compare their broader acceptance policies or Cohesion's subsequent range selection.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class HttpHeaderParserBenchmarks
{
    private string _contentType = null!;
    private string _range = null!;

    [GlobalSetup]
    public void Setup()
    {
        _contentType = "multipart/form-data; boundary=----bench-boundary; charset=utf-8";
        _range = "bytes=0-499,1000-1499,-500";
        if (ParseRange().Count != 3 || ParseRange_AspNetCore().Ranges.Count != 3
            || ParseRange_SystemNetHttp().Ranges.Count != 3
            || ParseContentType().Parameters.Count != 2
            || ParseContentType_AspNetCore().Parameters.Count != 2
            || ParseContentType_SystemNetHttp().Parameters.Count != 2)
        {
            throw new InvalidOperationException("Header parsers did not preserve the expected parameters and ranges.");
        }
    }

    [Benchmark, BenchmarkCategory("ContentType")]
    public HttpMediaType ParseContentType() => HttpMediaType.Parse(_contentType);

    [Benchmark(Baseline = true), BenchmarkCategory("ContentType")]
    public AspNetHeaders.MediaTypeHeaderValue ParseContentType_AspNetCore()
        => AspNetHeaders.MediaTypeHeaderValue.Parse(_contentType);

    [Benchmark, BenchmarkCategory("ContentType")]
    public NetHeaders.MediaTypeHeaderValue ParseContentType_SystemNetHttp()
        => NetHeaders.MediaTypeHeaderValue.Parse(_contentType);

    [Benchmark, BenchmarkCategory("Range")]
    public HttpRangeHeader ParseRange() => HttpRangeHeader.Parse(_range);

    [Benchmark(Baseline = true), BenchmarkCategory("Range")]
    public AspNetHeaders.RangeHeaderValue ParseRange_AspNetCore() => AspNetHeaders.RangeHeaderValue.Parse(_range);

    [Benchmark, BenchmarkCategory("Range")]
    public NetHeaders.RangeHeaderValue ParseRange_SystemNetHttp() => NetHeaders.RangeHeaderValue.Parse(_range);
}
