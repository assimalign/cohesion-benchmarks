using System;

using Assimalign.Cohesion.OpenApi.Benchmarks.Fixtures;
using Assimalign.Cohesion.OpenApi.Validation;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.OpenApi.Benchmarks.OpenApi.Validation;

/// <summary>
/// Revalidates an imported description, optionally with missing required info and a dangling schema
/// reference. Measures default structural/version/semantic rules and the optional official-schema stage.
/// Microsoft.OpenApi's rule set has different guarantees; treating it as equivalent would hide work.
/// </summary>
public class OpenApiValidationBenchmarks
{
    private OpenApiDocument _document = null!;
    private IOpenApiValidator _default = null!;
    private IOpenApiValidator _official = null!;

    [Params(false, true)]
    public bool Invalid { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _document = DescriptionFixture.ReadDocument();
        if (Invalid)
        {
            _document.Info.Title = string.Empty;
            _document.Components!.Schemas["MissingReference"] = new OpenApiSchema
            {
                Reference = new OpenApiReference { Ref = "#/components/schemas/DoesNotExist" },
            };
        }

        _default = OpenApiValidation.CreateDefault();
        _official = OpenApiValidation.Create([.. OpenApiValidation.DefaultRules(), OpenApiValidation.CreateOfficialSchemaRule()]);
        if (Validate().IsValid == Invalid || ValidateWithOfficialSchema().IsValid == Invalid)
        {
            throw new InvalidOperationException("The validation fixture has unexpected validity.");
        }
    }

    [Benchmark]
    public OpenApiValidationResult Validate() => _default.Validate(_document);

    [Benchmark]
    public OpenApiValidationResult ValidateWithOfficialSchema() => _official.Validate(_document);
}
