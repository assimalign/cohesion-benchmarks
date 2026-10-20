using System;
using System.Collections.Generic;

using Assimalign.Cohesion.OpenApi.Attributes;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.OpenApi.Benchmarks.OpenApi.Attributes;

/// <summary>
/// Maps collected authoring attributes to schema and operation metadata, including diagnostics.
/// The mapper accepts attributes rather than reflecting over a CLR type. Microsoft.OpenApi has no
/// equivalent attribute contract, so these run standalone; attribute discovery is outside measurement.
/// </summary>
public class OpenApiAttributeBenchmarks
{
    private OpenApiSchemaAttribute _schema = null!;
    private (string MemberName, OpenApiSchemaPropertyAttribute Attribute)[] _properties = null!;
    private OpenApiOperationAttribute _operation = null!;
    private OpenApiParameterAttribute[] _parameters = null!;
    private OpenApiResponseAttribute[] _responses = null!;

    [Params(4, 32)]
    public int MemberCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _schema = new OpenApiSchemaAttribute { Title = "Order", Description = "An order returned by the API" };
        _properties = new (string, OpenApiSchemaPropertyAttribute)[MemberCount];
        _parameters = new OpenApiParameterAttribute[MemberCount];
        for (int index = 0; index < MemberCount; index++)
        {
            string name = "field" + index;
            _properties[index] = (name, new OpenApiSchemaPropertyAttribute
            {
                Required = index % 2 == 0,
                SchemaType = OpenApiSchemaKind.String,
                Description = "An indexed order field",
            });
            _parameters[index] = new OpenApiParameterAttribute(name, ParameterLocation.Query)
            {
                SchemaType = OpenApiSchemaKind.String,
            };
        }

        _operation = new OpenApiOperationAttribute(OperationType.Get, "/orders")
        {
            OperationId = "listOrders", Tags = ["orders"], Summary = "Search orders",
        };
        _responses = [new OpenApiResponseAttribute(200)
        {
            Description = "Orders", ContentType = "application/json", SchemaReference = "#/components/schemas/Order",
        }];
        if (MapSchema().Properties.Count != MemberCount || MapOperation().Parameters.Count != MemberCount)
        {
            throw new InvalidOperationException("Attribute mapping lost members.");
        }
    }

    [Benchmark]
    public OpenApiSchemaMetadata MapSchema() => OpenApiAttributeMapper.MapSchema(
        _schema, "Order", _properties, new List<OpenApiMetadataDiagnostic>());

    [Benchmark]
    public OpenApiOperationMetadata MapOperation() => OpenApiAttributeMapper.MapOperation(
        _operation, _parameters, null, _responses, null, new List<OpenApiMetadataDiagnostic>());
}
