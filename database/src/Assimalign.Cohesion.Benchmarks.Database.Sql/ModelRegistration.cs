using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

/// <summary>Model-owned registration; the workload implementation replaces this empty registration.</summary>
public static class ModelRegistration
{
    public static IBenchmarkModel Create() => new SqlModel();
}
