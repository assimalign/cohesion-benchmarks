using System;
using System.Globalization;
using System.IO;

using Assimalign.Cohesion.OpenApi.Serialization;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using MicrosoftOpenApiDocument = global::Microsoft.OpenApi.OpenApiDocument;
using MicrosoftOpenApiJsonWriter = global::Microsoft.OpenApi.OpenApiJsonWriter;
using MicrosoftReadResult = global::Microsoft.OpenApi.Reader.ReadResult;

namespace Assimalign.Cohesion.OpenApi.Benchmarks.OpenApi.Serialization;

/// <summary>
/// Reading an OpenAPI 3.1 JSON description into the document model and writing the model back out as indented
/// JSON: an API gateway or client generator importing a description, and a server publishing its own on
/// <c>/openapi.json</c>. The baseline is Microsoft.OpenApi (<c>OpenApiDocument.Parse</c>,
/// <c>SerializeAsV31</c>), the model ASP.NET Core's OpenAPI support is built on.
/// </summary>
/// <remarks>
/// The input is the OpenAPI Initiative's "Tic Tac Toe" 3.1 example (paths, operations, parameters, components,
/// security schemes and <c>$ref</c>s), embedded beside this class as <c>tictactoe.json</c> (CC-BY-4.0, see
/// NOTICE.md).
/// Microsoft semantic validation is disabled so both sides measure syntax-to-model parsing;
/// document validation has its own standalone benchmarks.
/// </remarks>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class OpenApiJsonBenchmarks
{
    private const string parseCategory = "Parse";
    private const string serializeCategory = "Serialize";

    private string _json = null!;
    private OpenApiDocument _document = null!;
    private MicrosoftOpenApiDocument _microsoftDocument = null!;
    private global::Microsoft.OpenApi.Reader.OpenApiReaderSettings _settings = null!;

    [GlobalSetup]
    public void Setup()
    {
        using Stream corpus = typeof(OpenApiJsonBenchmarks).Assembly.GetManifestResourceStream("tictactoe.json")
            ?? throw new InvalidOperationException("The embedded tictactoe.json is missing.");
        using StreamReader reader = new(corpus);
        _json = reader.ReadToEnd();

        _document = OpenApiJson.Parse(_json);
        _settings = new global::Microsoft.OpenApi.Reader.OpenApiReaderSettings
        {
            RuleSet = global::Microsoft.OpenApi.ValidationRuleSet.GetEmptyRuleSet(),
        };
        _microsoftDocument = Parse_MicrosoftOpenApi().Document
            ?? throw new InvalidOperationException("Microsoft.OpenApi could not read the description.");

        if (Serialize().Length == 0 || Serialize_MicrosoftOpenApi().Length == 0)
        {
            throw new InvalidOperationException("The description serialized to nothing.");
        }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(parseCategory)]
    public MicrosoftReadResult Parse_MicrosoftOpenApi() => MicrosoftOpenApiDocument.Parse(_json, "json", _settings);

    [Benchmark]
    [BenchmarkCategory(parseCategory)]
    public OpenApiDocument Parse() => OpenApiJson.Parse(_json);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(serializeCategory)]
    public string Serialize_MicrosoftOpenApi()
    {
        using StringWriter writer = new(CultureInfo.InvariantCulture);
        _microsoftDocument.SerializeAsV31(new MicrosoftOpenApiJsonWriter(writer));
        return writer.ToString();
    }

    [Benchmark]
    [BenchmarkCategory(serializeCategory)]
    public string Serialize() => OpenApiJson.Serialize(_document, OpenApiSpecVersion.V3_1);
}
