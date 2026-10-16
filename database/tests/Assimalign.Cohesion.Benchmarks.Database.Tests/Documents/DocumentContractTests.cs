using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Documents;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using MongoDB.Bson;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Documents;

/// <summary>Pins support, deterministic namespaces and lossless transport representations.</summary>
public sealed class DocumentContractTests
{
    internal static DatasetManifest Manifest => new() { Model = DatasetModel.Documents, Name = "marketplace", GeneratorVersion = 1,
        Scale = 0.01, Seed = 20261015, Tables = [] };
    [Fact]
    public void Registration_AllOperations_DeclaresExplicitSupportAndMixes()
    {
        var model = ModelRegistration.Create();
        model.Operations.Count.ShouldBe(11); model.Targets.Count.ShouldBe(3);
        foreach (var target in model.Targets)
        {
            var support = DocumentWorkload.Support(target.Id); support.Count.ShouldBe(11);
            foreach (var declaration in support.Values) declaration.Validate();
        }
        var cohesion = DocumentWorkload.Support("cohesion-documents-embedded");
        cohesion.Values.Count(s => s.Level == SupportLevel.Native).ShouldBe(3);
        cohesion.Values.Count(s => s.Level == SupportLevel.Emulated).ShouldBe(7);
        cohesion["doc.products-by-category-price"].Level.ShouldBe(SupportLevel.Unsupported);
        model.Operations.Where(o => o.Category == "write").Sum(o => o.ReadHeavyWeight).ShouldBe(10);
        model.Operations.Where(o => o.Category == "write").Sum(o => o.BalancedWeight).ShouldBe(50);
    }
    [Fact]
    public void Parameters_RepeatedSeeds_ReproduceRequestsAndDisjointWriteKeys()
    {
        var model = ModelRegistration.Create();
        foreach (var operation in model.Operations)
        {
            var first = model.CreateParameterStream(Manifest, 123, operation.Id, 0, new("verify", 1, 1, false));
            var second = model.CreateParameterStream(Manifest, 123, operation.Id, 0, new("verify", 1, 1, false));
            for (int i = 0; i < 20; i++)
            {
                var x = (DocumentParameters)first.Next(); var y = (DocumentParameters)second.Next();
                x.Key.ShouldBe(y.Key); x.Keys.ShouldBe(y.Keys); x.RavenQuery.ShouldBe(y.RavenQuery);
                x.MongoPipeline.Select(v => v.ToJson()).ShouldBe(y.MongoPipeline.Select(v => v.ToJson()));
                x.Json.ShouldBe(y.Json);
            }
        }
        var keys = new HashSet<string>(StringComparer.Ordinal);
        foreach (bool warmup in new[] { false, true }) foreach (int client in new[] { 0, 1 })
        {
            var stream = model.CreateParameterStream(Manifest, 123, "doc.insert-order", client, new("throughput", 1, 8, warmup));
            for (int i = 0; i < 50; i++) keys.Add(((DocumentParameters)stream.Next()).Key).ShouldBeTrue();
        }
        foreach (string operation in new[] { "doc.patch-order-status", "doc.adjust-variant-stock", "doc.replace-customer" })
        {
            var warmup = (DocumentParameters)model.CreateParameterStream(Manifest, 123, operation, 0, new("latency", 1, 1, true)).Next();
            var measured = (DocumentParameters)model.CreateParameterStream(Manifest, 123, operation, 0, new("latency", 1, 1, false)).Next();
            warmup.Key.ShouldNotBe(measured.Key);
        }
    }
    [Fact]
    public void RawJson_DecimalAndLogicalId_RoundTripWithoutPocoCoercion()
    {
        byte[] bytes = "{\"id\":\"products/7\",\"price\":0.10,\"stock\":42,\"nested\":{\"name\":\"Alpha\"}}"u8.ToArray();
        var bson = DocumentJson.Mongo(bytes);
        bson["price"].BsonType.ShouldBe(BsonType.Decimal128);
        using var mongo = DocumentRows.MongoJson(bson); mongo.RootElement.GetProperty("price").GetDecimal().ShouldBe(0.10m);
        using var raven = JsonDocument.Parse(DocumentJson.Rewrite(bytes, ravenCollection: "products"));
        DocumentRows.Key(raven.RootElement).ShouldBe("products/7");
        raven.RootElement.GetProperty("price").GetDecimal().ShouldBe(0.10m);
        raven.RootElement.GetProperty("@metadata").GetProperty("@collection").GetString().ShouldBe("products");
    }
}
