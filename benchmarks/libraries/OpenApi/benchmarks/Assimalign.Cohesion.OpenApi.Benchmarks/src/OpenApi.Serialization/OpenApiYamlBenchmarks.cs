using System;
using System.Globalization;
using System.IO;

using Assimalign.Cohesion.OpenApi.Benchmarks.Fixtures;
using Assimalign.Cohesion.OpenApi.Serialization;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Microsoft.OpenApi.Reader;

using MicrosoftApi = global::Microsoft.OpenApi;

namespace Assimalign.Cohesion.OpenApi.Benchmarks.OpenApi.Serialization;

/// <summary>
/// Parses and publishes the same Tic Tac Toe document as YAML. YAML is derived from the attributed
/// embedded JSON in setup; both parsers receive identical text. Microsoft semantic validation is disabled:
/// this measures syntax-to-model parsing, since Cohesion's reader does not run its validation pipeline.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class OpenApiYamlBenchmarks
{
    private string _yaml = null!;
    private OpenApiDocument _document = null!;
    private MicrosoftApi.OpenApiDocument _microsoftDocument = null!;
    private OpenApiReaderSettings _settings = null!;

    [GlobalSetup]
    public void Setup()
    {
        _document = DescriptionFixture.ReadDocument();
        _yaml = OpenApiYaml.Serialize(_document);
        _settings = new OpenApiReaderSettings { RuleSet = MicrosoftApi.ValidationRuleSet.GetEmptyRuleSet() };
        _settings.AddYamlReader();
        _microsoftDocument = Parse_MicrosoftOpenApi().Document
            ?? throw new InvalidOperationException("Microsoft.OpenApi could not parse the YAML.");
        if (Parse().Paths!.Items.Count != _microsoftDocument.Paths.Count ||
            Serialize().Length == 0 || Serialize_MicrosoftOpenApi().Length == 0)
        {
            throw new InvalidOperationException("The YAML document was not preserved.");
        }
    }

    [Benchmark]
    [BenchmarkCategory("Parse")]
    public OpenApiDocument Parse() => OpenApiYaml.Parse(_yaml);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Parse")]
    public ReadResult Parse_MicrosoftOpenApi() => MicrosoftApi.OpenApiDocument.Parse(_yaml, "yaml", _settings);

    [Benchmark]
    [BenchmarkCategory("Serialize")]
    public string Serialize() => OpenApiYaml.Serialize(_document);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Serialize")]
    public string Serialize_MicrosoftOpenApi()
    {
        using StringWriter writer = new(CultureInfo.InvariantCulture);
        _microsoftDocument.SerializeAsV31(new MicrosoftApi.OpenApiYamlWriter(writer));
        return writer.ToString();
    }
}
