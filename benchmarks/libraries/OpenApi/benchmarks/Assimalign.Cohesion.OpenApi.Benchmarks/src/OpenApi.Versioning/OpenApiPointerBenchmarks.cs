using System;

using Assimalign.Cohesion.OpenApi.Versioning;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.OpenApi.Benchmarks.OpenApi.Versioning;

/// <summary>Public transform traversal with nested item pointers; same-version copying is a control.</summary>
public class OpenApiPointerBenchmarks
{
    private OpenApiDocument _document = null!;

    [Params(1, 16)]
    public int Depth { get; set; }

    [Params(false, true)]
    public bool SameVersion { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        OpenApiSchema schema = new() { Type = SchemaType.String, Format = "binary" };
        for (int i = 0; i < Depth; i++)
        {
            schema = new OpenApiSchema { Type = SchemaType.Array, Items = schema };
        }
        _document = new OpenApiDocument
        {
            SpecVersion = OpenApiSpecVersion.V3_0,
            Info = new OpenApiInfo { Title = "Pointers", Version = "1.0" },
            Components = new OpenApiComponents()
        };
        _document.Components.Schemas["root/~"] = schema;
        OpenApiTransformResult result = Transform();
        if (ReferenceEquals(result.Document, _document) || result.Document.SpecVersion != Target)
        {
            throw new InvalidOperationException("Transform copy or version changed.");
        }
    }

    private OpenApiSpecVersion Target => SameVersion ? OpenApiSpecVersion.V3_0 : OpenApiSpecVersion.V3_1;

    [Benchmark]
    public OpenApiTransformResult Transform() => OpenApiVersionTransformer.Transform(_document, Target);
}
