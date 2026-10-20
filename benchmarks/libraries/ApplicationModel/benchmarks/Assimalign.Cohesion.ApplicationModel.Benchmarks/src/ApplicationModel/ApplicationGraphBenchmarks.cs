using System.Text.Json;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModel;

/// <summary>
/// Whole-graph manifest and planning passes for a small application and a larger service graph.
/// Build includes manifest snapshots, graph/plan validation and registered store-source validation.
/// No third-party API implements Cohesion's composition contract. Planner diagnostic output is
/// redirected in setup, so Build still formats diagnostics but does not measure terminal I/O.
/// </summary>
public class ApplicationGraphBenchmarks
{
    private ApplicationGraph _graph = null!;
    private string[] _json = null!;
    private IApplicationBuilder _builder = null!;
    private PlannerDiagnostics _diagnostics = null!;

    [Params(4, 24)]
    public int ResourceCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _diagnostics = new PlannerDiagnostics();
        _graph = new ApplicationGraph(ResourceCount, "orders");
        _builder = _graph.CreateBuilder(new CompositionGateway());
        _json = new string[ResourceCount];
        for (int index = 0; index < ResourceCount; index++)
        {
            _json[index] = JsonSerializer.Serialize(_graph.Manifests[index], ResourceManifestJsonContext.Default.ResourceManifest);
        }
        _builder.Build();
    }

    [GlobalCleanup]
    public void Cleanup() => _diagnostics.Dispose();

    [Benchmark]
    public ResourceManifest ParseManifests()
    {
        ResourceManifest result = null!;
        foreach (string json in _json) result = ResourceManifest.Parse(json);
        return result;
    }

    [Benchmark]
    public ResourceManifest ValidateManifests()
    {
        ResourceManifest result = null!;
        foreach (ResourceManifest manifest in _graph.Manifests) result = manifest.Validate();
        return result;
    }

    [Benchmark]
    public ResourcePlan CreatePlans()
    {
        ResourcePlan result = null!;
        foreach (PlanContext context in _graph.Contexts) result = GenericPlanner.CreatePlan(context);
        return result;
    }

    [Benchmark]
    public ResourcePlan ValidatePlans()
    {
        for (int index = 0; index < ResourceCount; index++)
            ResourcePlanValidator.Validate(_graph.Plans[index], _graph.Contexts[index]);
        return _graph.Plans[^1];
    }

    [Benchmark]
    public IApplication Build() => _builder.Build();

    [Benchmark]
    public string ComputeManifestHashes()
    {
        string hash = null!;
        foreach (ResourceManifest manifest in _graph.Manifests) hash = ResourceManifestCanonicalizer.ComputeHash(manifest);
        return hash;
    }
}
