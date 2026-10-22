using System;
using System.Collections.Generic;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Http.Benchmarks.Http;

/// <summary>Preference sorting at the short-list boundary and on larger client lists.</summary>
public class HttpAcceptOrderingBenchmarks
{
    private string _media = null!;
    private string _tokens = null!;

    [Params(1, 8, 9, 16, 17, 64)]
    public int Entries { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var media = new string[Entries];
        var tokens = new string[Entries];
        for (int i = 0; i < Entries; i++)
        {
            // Increasing quality forces moves; pairs of equal ranks exercise stable ties.
            string quality = (i / 2 % 10).ToString(System.Globalization.CultureInfo.InvariantCulture);
            media[i] = $"application/type{i};q=0.{quality}";
            tokens[i] = $"coding{i};q=0.{quality}";
        }
        _media = string.Join(',', media);
        _tokens = string.Join(',', tokens);

        IReadOnlyList<HttpMediaTypeQuality> parsedMedia = ParseMedia();
        IReadOnlyList<HttpQualityValue> parsedTokens = ParseTokens();
        if (parsedMedia.Count != Entries || parsedTokens.Count != Entries)
        {
            throw new InvalidOperationException("Accept parsers lost benchmark entries.");
        }
        for (int i = 1; i < Entries; i++)
        {
            if (parsedMedia[i - 1].Quality.PerMille < parsedMedia[i].Quality.PerMille
                || parsedTokens[i - 1].Quality.PerMille < parsedTokens[i].Quality.PerMille)
            {
                throw new InvalidOperationException("Accept parsers did not order by descending quality.");
            }
        }
    }

    [Benchmark]
    public IReadOnlyList<HttpMediaTypeQuality> ParseMedia() => HttpAcceptParser.ParseAccept(_media);

    [Benchmark]
    public IReadOnlyList<HttpQualityValue> ParseTokens() => HttpAcceptParser.ParseAcceptEncoding(_tokens);
}
