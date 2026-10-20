using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModel;

/// <summary>A store, a stateful database, and dependent APIs/workers, with fan-out and chain edges.</summary>
internal sealed class ApplicationGraph
{
    public ApplicationGraph(int count, ApplicationName application)
    {
        Manifests = new ResourceManifest[count];
        Contexts = new PlanContext[count];
        Plans = new ResourcePlan[count];
        var references = new Dictionary<string, ResourceManifest>(StringComparer.Ordinal);
        for (int index = 0; index < count; index++)
        {
            string name = index == 0 ? "settings" : index == 1 ? "database" : $"service-{index}";
            var dependencies = new List<ResourceManifestReference>();
            if (index > 0)
            {
                dependencies.Add(new ResourceManifestReference
                {
                    Resource = "settings", Application = application, Endpoints = ["http"], Manifest = "Example.ConfigurationStore",
                });
            }
            if (index > 1)
            {
                dependencies.Add(new ResourceManifestReference
                {
                    Resource = Manifests[index - 1].Name, Application = application, Endpoints = ["http"],
                    Manifest = $"Example.Resource{index - 1}",
                });
            }
            ResourceManifest sample = SampleManifests.CreateWebApi();
            Manifests[index] = (sample with
            {
                Name = name,
                Application = application,
                Kind = index == 0 ? "ConfigurationStore" : index == 1 ? "Database" : "Web",
                Properties = index < 2 ? new Dictionary<string, string>() : sample.Properties,
                Endpoints = [sample.Endpoints[0]],
                Probes = new ResourceManifestProbes
                {
                    Readiness = new ResourceManifestProbe { Endpoint = "http", Http = "/readyz" },
                    Liveness = new ResourceManifestProbe { Endpoint = "http", Tcp = true },
                },
                Mounts = index == 0 ? [] : index == 1
                    ? [new ResourceManifestMount { Name = "data", Kind = ResourceMountKind.Volume, ContainerPath = "/data", Size = "10Gi" }]
                    : [new ResourceManifestMount { Name = "configuration", Kind = ResourceMountKind.Configuration,
                        ContainerPath = "/config", Source = "settings:application" },
                       new ResourceManifestMount { Name = "api-key", Kind = ResourceMountKind.Secret,
                        ContainerPath = "/secrets/api-key", Source = "parameter:api-key" },
                       new ResourceManifestMount { Name = "features", Kind = ResourceMountKind.Configuration,
                        ContainerPath = "/features", Source = "literal:{\"search\":true}" }],
                Lifecycle = sample.Lifecycle with { Workload = index == 1 ? WorkloadKind.StatefulSet : WorkloadKind.Deployment },
                References = dependencies,
            }).Validate();
            Contexts[index] = new PlanContext(Manifests[index], new ResourceOptions(), SampleManifests.Environment,
                new Dictionary<string, ResourceManifest>(references, StringComparer.Ordinal));
            Plans[index] = GenericPlanner.CreatePlan(Contexts[index]);
            references.Add(name, Manifests[index]);
        }
    }

    public ResourceManifest[] Manifests { get; }
    public PlanContext[] Contexts { get; }
    public ResourcePlan[] Plans { get; }

    public IApplicationBuilder CreateBuilder(IApplicationGateway gateway, string environment = "Production")
    {
        IApplicationBuilder builder = Application.CreateBuilder(Manifests[0].Application,
            ["--environment", environment]).UseGateway(gateway);
        foreach (ResourceManifest manifest in Manifests)
        {
            builder.AddResource(manifest);
        }
        builder.Providers.Sources.Add("settings", ConfigurationProvider.Instance);
        return builder;
    }

    internal sealed class ConfigurationProvider : IResourceSourceProvider
    {
        public static ConfigurationProvider Instance { get; } = new();
        private static readonly IReadOnlyDictionary<string, string?> _values = new Dictionary<string, string?>
        {
            ["ConnectionStrings:Main"] = "cohesion-db://database:5740/orders",
            ["Features:Search"] = "true",
            ["Logging:Level"] = "Information",
        };
        public string? ResourceKind => "ConfigurationStore";
        public ValueTask<IReadOnlyDictionary<string, string?>> ReadConfigurationAsync(ResourceSourceRequest request,
            CancellationToken cancellationToken = default) => ValueTask.FromResult(_values);
    }
}

/// <summary>Consumes resolved models without platform work; isolates composition and validation costs.</summary>
internal sealed class CompositionGateway : IMultiModelApplicationGateway
{
    public ResourceName Name => "benchmark";
    public IReadOnlyList<IApplicationModel> Models { get; private set; } = [];
    public void Validate(IApplicationModel model) { }
    public void Validate(IReadOnlyList<IApplicationModel> models) { }
    public Task StartAsync(IApplicationModel model, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task StartAsync(IReadOnlyList<IApplicationModel> models, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task ReconcileAsync(IApplicationModel model, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task ReconcileAsync(IReadOnlyList<IApplicationModel> models, CancellationToken cancellationToken = default)
    {
        Models = models;
        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task UninstallAsync(IApplicationModel model, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task UninstallAsync(IReadOnlyList<IApplicationModel> models, CancellationToken cancellationToken = default) => Task.CompletedTask;
}

/// <summary>Build emits planner diagnostics; discard only that output during the measured composition call.</summary>
internal sealed class PlannerDiagnostics : IDisposable
{
    private readonly TextWriter _previous = Console.Error;
    public PlannerDiagnostics() => Console.SetError(TextWriter.Null);
    public void Dispose() => Console.SetError(_previous);
}
