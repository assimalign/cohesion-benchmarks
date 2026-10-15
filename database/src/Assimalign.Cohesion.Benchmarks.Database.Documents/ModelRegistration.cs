using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Documents;

/// <summary>Registers the marketplace document workload and its four access paths.</summary>
public static class ModelRegistration
{
    public static IBenchmarkModel Create() => new DocumentsModel();
}
