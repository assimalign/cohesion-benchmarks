using System;
using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;

using AspNetHeaders = Microsoft.Net.Http.Headers;

namespace Assimalign.Cohesion.Http.Benchmarks.Http;

/// <summary>
/// Parsing a browser's <c>Accept</c> header into media ranges ordered by preference: the first step of
/// content negotiation on every request that negotiates a representation. The baseline is ASP.NET Core's
/// <c>MediaTypeHeaderValue.ParseList</c> followed by the same quality-then-specificity ordering
/// (<c>MediaTypeHeaderValueComparer.QualityComparer</c>), which is what <see cref="HttpAcceptParser.ParseAccept"/>
/// returns.
/// </summary>
public class HttpAcceptParserBenchmarks
{
    // Chrome's navigation Accept header: seven ranges, three weighted, one with an extra parameter.
    private const string accept =
        "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";

    private string[] _acceptLines = null!;

    [GlobalSetup]
    public void Setup()
    {
        _acceptLines = [accept];

        int cohesion = HttpAcceptParser.ParseAccept(accept).Count;
        int aspNetCore = AspNetHeaders.MediaTypeHeaderValue.ParseList(_acceptLines).Count;
        if (cohesion != aspNetCore)
        {
            throw new InvalidOperationException($"The parsers disagree on the Accept header: {cohesion} vs {aspNetCore} ranges.");
        }
    }

    [Benchmark(Baseline = true)]
    public AspNetHeaders.MediaTypeHeaderValue[] ParseAccept_AspNetCore()
        => AspNetHeaders.MediaTypeHeaderValue.ParseList(_acceptLines)
            .OrderByDescending(mediaType => mediaType, AspNetHeaders.MediaTypeHeaderValueComparer.QualityComparer)
            .ToArray();

    [Benchmark]
    public IReadOnlyList<HttpMediaTypeQuality> ParseAccept() => HttpAcceptParser.ParseAccept(accept);
}
