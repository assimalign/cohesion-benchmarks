using System;
using System.Globalization;
using System.IO;

using Assimalign.Cohesion.Benchmarks;
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
/// security schemes and <c>$ref</c>s), read from the measured cohesion checkout's vendored test corpus so this
/// repository does not carry a copy.
/// </remarks>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class OpenApiJsonBenchmarks
{
    private const string parseCategory = "Parse";
    private const string serializeCategory = "Serialize";

    private const string corpusPath =
        "libraries/OpenApi/Assimalign.Cohesion.OpenApi/tests/Shared/Corpus/v3.1/tictactoe.json";

    private string _json = null!;
    private OpenApiDocument _document = null!;
    private MicrosoftOpenApiDocument _microsoftDocument = null!;

    [GlobalSetup]
    public void Setup()
    {
        string cohesionRoot = BenchmarkEnvironment.GetCohesionRoot(typeof(OpenApiJsonBenchmarks).Assembly);
        _json = File.ReadAllText(Path.Combine(cohesionRoot, corpusPath.Replace('/', Path.DirectorySeparatorChar)));

        _document = OpenApiJson.Parse(_json);
        _microsoftDocument = Parse_MicrosoftOpenApi().Document
            ?? throw new InvalidOperationException("Microsoft.OpenApi could not read the description.");

        if (Serialize().Length == 0 || Serialize_MicrosoftOpenApi().Length == 0)
        {
            throw new InvalidOperationException("The description serialized to nothing.");
        }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(parseCategory)]
    public MicrosoftReadResult Parse_MicrosoftOpenApi() => MicrosoftOpenApiDocument.Parse(_json, "json");

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
