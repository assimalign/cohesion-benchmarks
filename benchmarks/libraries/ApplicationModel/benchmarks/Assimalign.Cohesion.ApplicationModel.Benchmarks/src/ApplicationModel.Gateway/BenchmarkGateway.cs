using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

using Assimalign.Cohesion.ApplicationModel.Gateway;
using Assimalign.Cohesion.Hosting.Resources;

namespace Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModelGateway;

/// <summary>
/// A platform-free gateway using the real base reconciliation, input resolution and direct command
/// delivery. Only artifact gathering, platform mutation and export publication are fixtures.
/// </summary>
internal sealed class BenchmarkGateway : ApplicationGateway
{
    private readonly InMemoryResourceStateManager _state = new();
    private readonly Controller _controller = new();
    private readonly IReadOnlyList<IApplicationResourceController> _controllers;
    private readonly Dictionary<ResourceId, IResourceControlPlane> _planes = new();

    public BenchmarkGateway(string directory) : base(new ApplicationGatewayOptions
    {
        ExportDirectory = directory,
        TrustKeyRepository = new MemoryKeys(),
        Parameters = { ["api-key"] = "benchmark-api-key" },
    }) => _controllers = [_controller];

    public override ResourceName Name => "benchmark";
    protected override IApplicationResourceStateManager State => _state;
    protected override IReadOnlyList<IApplicationResourceController> Controllers => _controllers;
    public IReadOnlyDictionary<ResourceId, IResourceControlContext> Contexts => _controller.Contexts;

    public void Register(IApplicationResource resource, IResourceControlPlane plane) => _planes.Add(resource.Id, plane);

    protected override bool TryGetResourceControlPlane(IApplicationModel model, IApplicationResource resource,
        out IResourceControlPlane? controlPlane) => _planes.TryGetValue(resource.Id, out controlPlane);

    public ValueTask<ResourceInputs> Resolve(IResourceControlContext context) =>
        ResolveInputsAsync(context.Descriptor, context, CancellationToken.None);

    protected override Task<IResourceArtifact> GatherAsync(IApplicationResource resource, CancellationToken cancellationToken) =>
        Task.FromResult<IResourceArtifact>(new Artifact(resource.Id));

    protected override Task PublishApplicationExportAsync(ApplicationExportDocument document, CancellationToken cancellationToken) =>
        Task.CompletedTask;

    protected override Task RemoveApplicationExportAsync(ApplicationName application, CancellationToken cancellationToken) =>
        Task.CompletedTask;

    private sealed record Artifact(ResourceId Resource) : IResourceArtifact;

    private sealed class Controller : IApplicationResourceController
    {
        public Dictionary<ResourceId, IResourceControlContext> Contexts { get; } = new();
        public bool CanRealize(ResourcePlan plan, out string? reason) { reason = null; return true; }
        public Task ReconcileAsync(IResourceControlContext context, CancellationToken cancellationToken = default)
        {
            foreach (ResourceMountInput mount in context.Inputs.Mounts.Values)
                if (!mount.IsResolved) throw new InvalidOperationException(mount.UnresolvedReason);
            Contexts[context.Resource.Id] = context;
            context.State.SetState(context.Resource.Id, ResourceLifecycle.Running,
                observedEndpoints: [new ResourceEndpoint("http", "http", 8080, Host: "localhost")]);
            return Task.CompletedTask;
        }
        public Task StopAsync(IResourceControlContext context, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task DeleteAsync(IResourceControlContext context, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private sealed class MemoryKeys : IGatewayTrustKeyRepository
    {
        public Task<ECDsa> LoadOrCreateAsync(ApplicationName application, ResourceName gateway,
            CancellationToken cancellationToken = default) => Task.FromResult(ECDsa.Create(ECCurve.NamedCurves.nistP256));
        public Task<ECDsa> RotateAsync(ApplicationName application, ResourceName gateway,
            CancellationToken cancellationToken = default) => LoadOrCreateAsync(application, gateway, cancellationToken);
    }
}
