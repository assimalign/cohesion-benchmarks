using System.Collections.Generic;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>The single registration seam implemented by each model suite.</summary>
public interface IBenchmarkModel
{
    string Id { get; }
    DatasetModel DatasetModel { get; }
    IReadOnlyList<OperationDescriptor> Operations { get; }
    IReadOnlyList<TargetDescriptor> Targets { get; }
    IBenchmarkTarget CreateTarget(string id, TargetOptions options);
    IParameterStream CreateParameterStream(DatasetManifest manifest, ulong seed, string operation, int clientIndex,
        ParameterStreamContext context);
}
