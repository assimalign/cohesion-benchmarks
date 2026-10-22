using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using AspNetHeaders = Microsoft.Net.Http.Headers;

namespace Assimalign.Cohesion.Http.Benchmarks.Http;

/// <summary>
/// Media-type parsing and rendering with zero, one, or several parameters. Formatting starts
/// from parsed values in both libraries; a library may retain and reuse its original text.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class HttpMediaTypeBenchmarks
{
    private string _text = null!;
    private HttpMediaType _mediaType;
    private AspNetHeaders.MediaTypeHeaderValue _aspNetCore = null!;

    [Params("Plain", "Charset", "Multiple", "Escaped")]
    public string Scenario { get; set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        (_text, int count) = Scenario switch
        {
            "Plain" => ("application/json", 0),
            "Charset" => ("text/plain; charset=utf-8", 1),
            "Multiple" => ("multipart/form-data; boundary=bench-boundary; charset=utf-8", 2),
            "Escaped" => ("text/plain; note=\"a,b;c\"; empty=\"\"; escape=\"a\\\"b\\\\c\"", 3),
            _ => throw new InvalidOperationException("Unknown media-type scenario."),
        };
        _mediaType = Parse();
        _aspNetCore = Parse_AspNetCore();
        if (_mediaType.Parameters.Count != count || _aspNetCore.Parameters.Count != count
            || !string.Equals(Format(), _text, StringComparison.Ordinal)
            || !string.Equals(Format_AspNetCore(), _text, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Media-type parsers or formatters changed the canonical input.");
        }
    }

    [Benchmark, BenchmarkCategory("Parse")]
    public HttpMediaType Parse() => HttpMediaType.Parse(_text);

    [Benchmark(Baseline = true), BenchmarkCategory("Parse")]
    public AspNetHeaders.MediaTypeHeaderValue Parse_AspNetCore() => AspNetHeaders.MediaTypeHeaderValue.Parse(_text);

    [Benchmark, BenchmarkCategory("Format")]
    public string Format() => _mediaType.ToString();

    [Benchmark(Baseline = true), BenchmarkCategory("Format")]
    public string Format_AspNetCore() => _aspNetCore.ToString();
}
