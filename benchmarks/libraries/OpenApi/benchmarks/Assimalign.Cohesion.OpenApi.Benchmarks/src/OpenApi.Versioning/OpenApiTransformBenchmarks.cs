using System;

using Assimalign.Cohesion.OpenApi.Benchmarks.Fixtures;
using Assimalign.Cohesion.OpenApi.Versioning;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.OpenApi.Benchmarks.OpenApi.Versioning;

/// <summary>
/// Retargets an imported 3.1 document to 3.0 or 3.2, including a deep copy, schema conversion and
/// loss diagnostics. The package exposes transformation rather than HTTP version negotiation.
/// Microsoft versioned serialization does not provide this copy-and-diagnostics contract.
/// </summary>
public class OpenApiTransformBenchmarks
{
    private OpenApiDocument _document = null!;

    [Params(OpenApiSpecVersion.V3_0, OpenApiSpecVersion.V3_2)]
    public OpenApiSpecVersion Target { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _document = DescriptionFixture.ReadDocument();
        _document.Components!.Schemas["Attachment"] = new OpenApiSchema
        {
            Type = SchemaType.String, ContentEncoding = "base64", Nullable = true,
        };
        OpenApiTransformResult result = Transform();
        if (result.Document.SpecVersion != Target || ReferenceEquals(result.Document, _document) ||
            _document.SpecVersion != OpenApiSpecVersion.V3_1)
        {
            throw new InvalidOperationException("Transformation did not preserve the source and retarget the copy.");
        }
    }

    [Benchmark]
    public OpenApiTransformResult Transform() => OpenApiVersionTransformer.Transform(_document, Target);
}
