using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

/// <summary>Registers the social graph workload and its three engine access paths.</summary>
public static class ModelRegistration
{
    public static IBenchmarkModel Create() => new GraphModel();
}
