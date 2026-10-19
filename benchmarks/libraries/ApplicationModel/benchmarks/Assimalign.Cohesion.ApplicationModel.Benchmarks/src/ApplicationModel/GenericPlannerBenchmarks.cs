using System.Collections.Generic;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModel;

/// <summary>
/// Turning a manifest into the platform-neutral realization plan (workload, container, ports, probes, services,
/// exposures) and validating that plan against its manifest: the step every gateway runs per resource before a
/// platform compiler sees it.
/// </summary>
public class GenericPlannerBenchmarks
{
    private PlanContext _context = null!;
    private ResourcePlan _plan = null!;

    [GlobalSetup]
    public void Setup()
    {
        _context = new PlanContext(
            SampleManifests.CreateWebApi().Validate(),
            new ResourceOptions(),
            SampleManifests.Environment,
            new Dictionary<string, ResourceManifest>());
        _plan = GenericPlanner.CreatePlan(_context);
    }

    [Benchmark]
    public ResourcePlan CreatePlan() => GenericPlanner.CreatePlan(_context);

    [Benchmark]
    public ResourcePlan ValidatePlan()
    {
        ResourcePlanValidator.Validate(_plan, _context);
        return _plan;
    }
}
