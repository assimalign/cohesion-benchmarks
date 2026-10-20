using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModel;
using Assimalign.Cohesion.ApplicationModel.Gateway.InProcess;
using Assimalign.Cohesion.Hosting.Resources;

namespace Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModelGatewayInProcess;

/// <summary>
/// Offline in-process binding/entry eligibility validation and rendering of ambient host units,
/// endpoint allocation and mount carriers. The suite executable supplies an entry binding solely
/// for eligibility checks: it is never invoked. No third-party composition-model counterpart.
/// </summary>
public class InProcessPlanBenchmarks
{
    private InProcessGateway _gateway = null!;
    private IApplicationModel _model = null!;
    private IApplicationModel[] _models = null!;
    private string _directory = null!;

    [Params(4, 24)]
    public int ResourceCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _directory = Path.Combine(Path.GetTempPath(), "cohesion-inprocess-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_directory);
        _gateway = new InProcessGateway(new InProcessGatewayOptions { StateDirectory = _directory });
        Assembly assembly = typeof(Program).Assembly;
        if (!ResourceRuntime.IsEntryRegistered(assembly)) ResourceRuntime.RegisterEntry(assembly);
        var graph = new ApplicationGraph(ResourceCount, $"inprocess-{ResourceCount}");
        IApplicationBuilder builder = Application.CreateBuilder(graph.Manifests[0].Application,
            ["--environment", "Production"]).UseGateway(_gateway);
        foreach (ResourceManifest manifest in graph.Manifests)
            builder.AddResource(manifest, new ResourceOptions { Replicas = 1 }).InProcess(assembly, _directory);
        builder.Providers.Sources.Add("settings", ApplicationGraph.ConfigurationProvider.Instance);
        using var diagnostics = new PlannerDiagnostics();
        _model = builder.Build().Model;
        _models = [_model];
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await ((IApplicationGateway)_gateway).StopAsync();
        Directory.Delete(_directory, recursive: true);
    }

    [Benchmark]
    public IApplicationModel ValidateBindings()
    {
        ((IApplicationGateway)_gateway).Validate(_model);
        return _model;
    }

    [Benchmark]
    public async Task<string> RenderPlan()
    {
        using var output = new StringWriter();
        await _gateway.RenderAsync(_models, output);
        return output.ToString();
    }
}
