using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.ObjectValidation.Benchmarks.ObjectValidation;

/// <summary>
/// Build reusable validators separately from validation. Both construct a parent rule graph with
/// a nested address validator and optionally a collection child validator. Setup verifies the
/// resulting graphs are executable and enforce equivalent rules; no object is validated in Build.
/// </summary>
public class ProfileBuildBenchmarks
{
    [Params(false, true)] public bool IncludeCollection { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var order = new NestedValidationBenchmarks.Order
        {
            Shipping = NestedValidationBenchmarks.Address.Create(false),
            Stops = [NestedValidationBenchmarks.Address.Create(false)],
        };
        NestedValidationBenchmarks.Check(Build().Validate(order).Errors.Count(),
            Build_FluentValidation().Validate(order).Errors.Count, IncludeCollection ? 4 : 2);
    }

    [Benchmark] public IValidator Build() => NestedValidationBenchmarks.CreateCohesion(IncludeCollection);
    [Benchmark(Baseline = true)]
    public NestedValidationBenchmarks.OrderValidator Build_FluentValidation() => new(IncludeCollection);
}
