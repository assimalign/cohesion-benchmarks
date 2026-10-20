using System;

using Assimalign.Cohesion.OpenApi.Benchmarks.Fixtures;
using Assimalign.Cohesion.OpenApi.Integration;
using Assimalign.Cohesion.OpenApi.Serialization;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Microsoft.OpenApi.Reader;

using MicrosoftApi = global::Microsoft.OpenApi;

namespace Assimalign.Cohesion.OpenApi.Benchmarks.OpenApi.Integration;

/// <summary>
/// Management import and export through the consumer integration contracts. Export includes retargeting
/// 3.1 to 3.0 with diagnostics, not just a serializer call. Microsoft.OpenApi has no equivalent retarget
/// result contract, so retargeted export stays standalone. Import compares syntax-to-model work with
/// Microsoft's semantic validation disabled, as in the raw serialization comparisons.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class OpenApiExportBenchmarks
{
    private OpenApiDocument _document = null!;
    private string _content = null!;
    private IOpenApiDocumentImporter _importer = null!;
    private IOpenApiDocumentExporter _exporter = null!;
    private OpenApiReaderSettings _settings = null!;
    private string _microsoftFormat = null!;

    [Params(OpenApiFormat.Json, OpenApiFormat.Yaml)]
    public OpenApiFormat Format { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _document = DescriptionFixture.ReadDocument();
        _content = Format == OpenApiFormat.Json ? DescriptionFixture.ReadJson() : OpenApiYaml.Serialize(_document);
        _importer = OpenApiIntegration.CreateImporter();
        _exporter = OpenApiIntegration.CreateExporter();
        _settings = new OpenApiReaderSettings { RuleSet = MicrosoftApi.ValidationRuleSet.GetEmptyRuleSet() };
        _settings.AddYamlReader();
        _microsoftFormat = Format == OpenApiFormat.Json ? "json" : "yaml";
        if (Import().Paths!.Items.Count != _document.Paths!.Items.Count ||
            Import_MicrosoftOpenApi().Document?.Paths.Count != _document.Paths.Items.Count ||
            ExportRetargeted().Content.Length == 0)
        {
            throw new InvalidOperationException("Integration did not preserve the document.");
        }
    }

    [Benchmark]
    [BenchmarkCategory("Import")]
    public OpenApiDocument Import() => _importer.Import(_content, Format);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Import")]
    public ReadResult Import_MicrosoftOpenApi() => MicrosoftApi.OpenApiDocument.Parse(_content, _microsoftFormat, _settings);

    [Benchmark]
    [BenchmarkCategory("Export")]
    public OpenApiExportResult ExportRetargeted() => _exporter.Export(_document, Format, OpenApiSpecVersion.V3_0);
}
