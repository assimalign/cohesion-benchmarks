using System;
using System.Collections.Generic;
using System.Net.Http;

using Assimalign.Cohesion.OpenApi.Fluent;
using Assimalign.Cohesion.OpenApi.Validation;

using BenchmarkDotNet.Attributes;

using MicrosoftApi = global::Microsoft.OpenApi;

namespace Assimalign.Cohesion.OpenApi.Benchmarks.OpenApi.Fluent;

/// <summary>
/// Builds fresh documents with paths, required path parameters and responses. The counterpart uses
/// Microsoft.OpenApi's public object model to produce the same fields. Neither side validates or
/// serializes during measurement; this comparison is deliberately limited to common model authoring.
/// </summary>
public class OpenApiBuildBenchmarks
{
    private string[] _paths = null!;
    private string[] _operationIds = null!;

    [Params(4, 32)]
    public int PathCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _paths = new string[PathCount];
        _operationIds = new string[PathCount];
        for (int index = 0; index < PathCount; index++)
        {
            _paths[index] = "/orders" + index + "/{id}";
            _operationIds[index] = "getOrder" + index;
        }

        if (!Build().Validate().IsValid || Build_MicrosoftOpenApi().Paths.Count != PathCount)
        {
            throw new InvalidOperationException("Building lost document paths.");
        }
    }

    [Benchmark]
    public OpenApiDocument Build()
    {
        var builder = OpenApiDocumentBuilder.Create(OpenApiSpecVersion.V3_1, "Orders", "1.0.0");
        for (int index = 0; index < PathCount; index++)
        {
            string operationId = _operationIds[index];
            builder.Path(_paths[index], path => path.Operation(OperationType.Get, operation => operation
                .OperationId(operationId)
                .Parameter("id", ParameterLocation.Path, parameter => parameter.Required().Schema(schema => schema.Type(SchemaType.Integer)))
                .Response("200", response => response.Description("Order"))));
        }

        return builder.Build();
    }

    [Benchmark(Baseline = true)]
    public MicrosoftApi.OpenApiDocument Build_MicrosoftOpenApi()
    {
        var document = new MicrosoftApi.OpenApiDocument
        {
            Info = new MicrosoftApi.OpenApiInfo { Title = "Orders", Version = "1.0.0" },
            Paths = new MicrosoftApi.OpenApiPaths(),
        };
        for (int index = 0; index < PathCount; index++)
        {
            document.Paths[_paths[index]] = new MicrosoftApi.OpenApiPathItem
            {
                Operations = new Dictionary<HttpMethod, MicrosoftApi.OpenApiOperation>
                {
                    [HttpMethod.Get] = new MicrosoftApi.OpenApiOperation
                    {
                        OperationId = _operationIds[index],
                        Parameters = [new MicrosoftApi.OpenApiParameter
                        {
                            Name = "id", In = MicrosoftApi.ParameterLocation.Path, Required = true,
                            Schema = new MicrosoftApi.OpenApiSchema { Type = MicrosoftApi.JsonSchemaType.Integer },
                        }],
                        Responses = new MicrosoftApi.OpenApiResponses
                        {
                            ["200"] = new MicrosoftApi.OpenApiResponse { Description = "Order" },
                        },
                    },
                },
            };
        }

        return document;
    }
}
