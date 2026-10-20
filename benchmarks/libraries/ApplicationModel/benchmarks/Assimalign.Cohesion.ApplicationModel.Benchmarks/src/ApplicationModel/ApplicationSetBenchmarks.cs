using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModel;

/// <summary>
/// Resolves and composes two application-set members, freezes each member's provider registrations
/// and validates mount bindings. The set has no Build API: RunAsync in Apply mode does that work.
/// An in-memory resolver and consuming gateway exclude transport and platform work. No counterpart.
/// </summary>
public class ApplicationSetBenchmarks
{
    private IApplicationSet _set = null!;
    private CompositionGateway _gateway = null!;

    [Params(4, 24)]
    public int ResourcesPerApplication { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        using var diagnostics = new PlannerDiagnostics();
        _gateway = new CompositionGateway();
        _set = Application.CreateSet(_gateway, ["--environment", "Production", "--mode", "apply"]);
        foreach (string name in new[] { "orders", "billing" })
        {
            IApplicationModel model = new ApplicationGraph(ResourcesPerApplication, name)
                .CreateBuilder(_gateway).Build().Model;
            // Imported documents intentionally carry no code providers: register each member explicitly.
            var resolver = new ModelResolver(ApplicationModelDocument.Create(model).ToModel());
            _set.AddApplication(new ApplicationDeclaration(name, resolver), member =>
                member.Providers.Sources.Add("settings", ApplicationGraph.ConfigurationProvider.Instance));
        }
    }

    [Benchmark]
    public async Task<IReadOnlyList<IApplicationModel>> ComposeApplications()
    {
        await _set.RunAsync();
        return _gateway.Models;
    }

    private sealed class ModelResolver(IApplicationModel model) : IApplicationModelResolver
    {
        public ValueTask<IApplicationModel> ResolveAsync(ApplicationModelResolutionContext context,
            CancellationToken cancellationToken = default) => ValueTask.FromResult(model);
    }
}
