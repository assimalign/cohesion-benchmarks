using System;
using System.IO;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModel;

namespace Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModelGateway;

/// <summary>
/// Gateway graph validation and repeated mount resolution over admitted resources: provider lookup,
/// observed store connection/credential resolution, sorted configuration JSON and literal UTF-8.
/// The protected gateway extension seam is exposed by a fixture, with real contexts captured at startup.
/// Provider reads are in-memory; this measures orchestration, not store/network latency. No counterpart.
/// </summary>
public class ResourceResolutionBenchmarks
{
    private BenchmarkGateway _gateway = null!;
    private IApplicationModel _model = null!;
    private IResourceControlContext[] _contexts = null!;
    private string _directory = null!;

    [Params(4, 24)]
    public int ResourceCount { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        _directory = Path.Combine(Path.GetTempPath(), "cohesion-resolution-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_directory);
        _gateway = new BenchmarkGateway(_directory);
        using (var diagnostics = new PlannerDiagnostics())
            _model = new ApplicationGraph(ResourceCount, "orders").CreateBuilder(_gateway, "Local").Build().Model;
        await ((IApplicationGateway)_gateway).StartAsync(_model);
        _contexts = new IResourceControlContext[ResourceCount];
        for (int index = 0; index < ResourceCount; index++)
            _contexts[index] = _gateway.Contexts[_model.Resources[index].Id];
        ResourceInputs inputs = await _gateway.Resolve(_contexts[^1]);
        if (!inputs.Mounts["configuration"].IsResolved || !inputs.Mounts["api-key"].IsResolved)
            throw new InvalidOperationException($"Resolution benchmark must measure successful inputs: {inputs.Mounts["configuration"].UnresolvedReason}; {inputs.Mounts["api-key"].UnresolvedReason}");
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await ((IApplicationGateway)_gateway).StopAsync();
        Directory.Delete(_directory, recursive: true);
    }

    [Benchmark]
    public IApplicationModel ValidateGraph()
    {
        ((IApplicationGateway)_gateway).Validate(_model);
        return _model;
    }

    [Benchmark]
    public async Task<ResourceInputs> ResolveMountSources()
    {
        ResourceInputs inputs = null!;
        foreach (IResourceControlContext context in _contexts) inputs = await _gateway.Resolve(context);
        return inputs;
    }
}
