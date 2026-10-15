using System;
using System.Collections.Generic;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Explicit registration placeholder for a model session; it never claims operations or targets exist.</summary>
public sealed class EmptyBenchmarkModel(string id, DatasetModel datasetModel) : IBenchmarkModel
{
    public string Id => id;
    public DatasetModel DatasetModel => datasetModel;
    public IReadOnlyList<OperationDescriptor> Operations => [];
    public IReadOnlyList<TargetDescriptor> Targets => [];
    public IBenchmarkTarget CreateTarget(string id, TargetOptions options)
        => throw new NotSupportedException($"Model {Id} has no workload adapters yet (requested {id}).");
    public IParameterStream CreateParameterStream(DatasetManifest manifest, ulong seed, string operation, int clientIndex,
        ParameterStreamContext context)
        => throw new NotSupportedException($"Model {Id} has no parameter streams yet.");
}
