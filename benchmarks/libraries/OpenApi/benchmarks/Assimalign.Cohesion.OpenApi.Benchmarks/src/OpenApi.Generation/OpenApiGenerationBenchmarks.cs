using System;

using Assimalign.Cohesion.OpenApi.Attributes;
using Assimalign.Cohesion.OpenApi.Generation;
using Assimalign.Cohesion.OpenApi.Validation;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.OpenApi.Benchmarks.OpenApi.Generation;

/// <summary>
/// Generates schema components and endpoint documents from already collected metadata. This package
/// does not infer schemas from arbitrary CLR types. Microsoft.OpenApi provides models rather than this
/// metadata generation pipeline, so no counterpart is paired with it.
/// </summary>
public class OpenApiGenerationBenchmarks
{
    private OpenApiGenerationInput _schemas = null!;
    private OpenApiGenerationInput _endpoints = null!;
    private OpenApiGenerationOptions _options = null!;

    [Params(4, 32)]
    public int ItemCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var schemas = new OpenApiSchemaMetadata[ItemCount];
        var operations = new OpenApiOperationMetadata[ItemCount];
        for (int index = 0; index < ItemCount; index++)
        {
            string name = "Order" + index;
            schemas[index] = new OpenApiSchemaMetadata
            {
                Name = name, Type = SchemaType.Object,
                Properties =
                [
                    new OpenApiSchemaPropertyMetadata { Name = "id", SchemaType = SchemaType.Integer, Format = "int64", Required = true },
                    new OpenApiSchemaPropertyMetadata { Name = "name", SchemaType = SchemaType.String, Required = true },
                    new OpenApiSchemaPropertyMetadata { Name = "description", SchemaType = SchemaType.String, Nullable = true },
                    new OpenApiSchemaPropertyMetadata { Name = "total", SchemaType = SchemaType.Number, Format = "double" },
                ],
            };
            operations[index] = new OpenApiOperationMetadata
            {
                Method = OperationType.Get, Path = "/orders" + index + "/{id}", OperationId = "getOrder" + index,
                Tags = ["orders"],
                Parameters = [new OpenApiParameterMetadata { Name = "id", In = ParameterLocation.Path, Required = true, SchemaType = SchemaType.Integer }],
                Responses = [new OpenApiResponseMetadata { StatusCode = "200", Description = "Order", ContentType = "application/json", SchemaReference = "#/components/schemas/" + name }],
            };
        }

        _options = new OpenApiGenerationOptions { Title = "Orders", ApiVersion = "1.0.0" };
        _schemas = new OpenApiGenerationInput { Schemas = schemas };
        _endpoints = new OpenApiGenerationInput { Schemas = schemas, Operations = operations };
        if (GenerateSchemas().Components!.Schemas.Count != ItemCount || !GenerateDocument().Validate().IsValid)
        {
            throw new InvalidOperationException("Generation produced an invalid document.");
        }
    }

    [Benchmark]
    public OpenApiDocument GenerateSchemas() => OpenApiDocumentGenerator.Generate(_schemas, _options);

    [Benchmark]
    public OpenApiDocument GenerateDocument() => OpenApiDocumentGenerator.Generate(_endpoints, _options);
}
