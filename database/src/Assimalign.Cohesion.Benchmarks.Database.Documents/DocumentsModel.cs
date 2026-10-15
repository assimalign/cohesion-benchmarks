using System;
using System.Collections.Generic;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

internal sealed class DocumentsModel : IBenchmarkModel
{
    public string Id => "documents";
    public DatasetModel DatasetModel => DatasetModel.Documents;
    public IReadOnlyList<OperationDescriptor> Operations => DocumentWorkload.Operations;
    public IReadOnlyList<TargetDescriptor> Targets { get; } = new TargetDescriptor[]
    {
        new("cohesion-documents-embedded", "Cohesion Documents", true, "in-process", "cohesion"),
        new("mongodb", "MongoDB 8.0", true, "MongoDB.Driver", "mongodb"),
        new("ravendb", "RavenDB 7.1", true, "static indexes, non-stale", "ravendb"),
        new("cosmos-emulator", "Cosmos DB emulator", false, "functional parity only; gateway", "cosmos-emulator")
    };
    public IBenchmarkTarget CreateTarget(string id, TargetOptions options)
    {
        TargetDescriptor? descriptor = null;
        foreach (var candidate in Targets) if (candidate.Id == id) descriptor = candidate;
        if (descriptor == null) throw new ArgumentException("Unknown document target: " + id);
        return id switch
        {
            "cohesion-documents-embedded" => new CohesionDocumentsTarget(descriptor, options),
            "mongodb" => new MongoDocumentsTarget(descriptor, options),
            "ravendb" => new RavenDocumentsTarget(descriptor, options),
            "cosmos-emulator" => new CosmosDocumentsTarget(descriptor, options),
            _ => throw new ArgumentException("Unknown document target: " + id)
        };
    }
    public IParameterStream CreateParameterStream(DatasetManifest manifest, ulong seed, string operation,
        int clientIndex, ParameterStreamContext context) => throw new NotImplementedException("Parameter streams land with operations.");
}
