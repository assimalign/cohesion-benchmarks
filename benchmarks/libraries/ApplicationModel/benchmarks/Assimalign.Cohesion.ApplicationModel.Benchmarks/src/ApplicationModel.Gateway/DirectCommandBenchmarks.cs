using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModel;
using Assimalign.Cohesion.Hosting.Resources;

using CommandEnvelope = Assimalign.Cohesion.Hosting.Resources.ResourceCommand;

namespace Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModelGateway;

/// <summary>
/// Public reconciliation through ApplicationGateway's direct control-plane delivery branch used by
/// InProcessGateway. Real ownership, replay, command replacement and resource-control-plane handler
/// dispatch run against an in-memory runtime handler. The fixture replaces platform work and the
/// InProcess host's control-plane lookup, so this excludes entry invocation and host supervision.
/// Replacement alternates two prebuilt declarations, withdrawing the old one and applying the new
/// one on every call; repeated commands measure the steady-state idempotency path. No counterpart.
/// </summary>
public class DirectCommandBenchmarks
{
    private BenchmarkGateway _gateway = null!;
    private IApplicationModel[] _models = null!;
    private int _current;
    private string _directory = null!;
    private Handler _handler = null!;

    [Params(4, 24)]
    public int ResourceCount { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        _directory = Path.Combine(Path.GetTempPath(), "cohesion-commands-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_directory);
        _gateway = new BenchmarkGateway(_directory);
        var graph = new ApplicationGraph(ResourceCount, "orders");
        graph.Manifests[^1] = graph.Manifests[^1] with { Commands = [new ResourceManifestCommand("web.configure")] };
        IApplicationBuilder builder = graph.CreateBuilder(_gateway, "Local");
        using (var diagnostics = new PlannerDiagnostics())
        {
            IApplicationModel initial = builder.Build().Model;
            IApplicationResource target = initial.Resources[^1];
            _models = new IApplicationModel[2];
            for (int index = 0; index < _models.Length; index++)
            {
                IResourceCommand command = ResourceCommands.Create("web.configure", "settings", target, "orders",
                    new CommandSettings(index, "Information"), CommandJsonContext.Default.CommandSettings);
                // The wrapper changes only the desired commands; resource identity and graph stay identical.
                _models[index] = new CommandModel(initial, command);
            }
            _handler = new Handler();
            var plane = ResourceControlPlane.Create(["web.configure"]);
            plane.RegisterCommandHandler(_handler);
            _gateway.Register(target, plane);
        }
        await ((IApplicationGateway)_gateway).StartAsync(_models[0]);
        await ReplaceCommands();
        if (_handler.Applied != 2 || _handler.Deleted != 1)
            throw new InvalidOperationException("Replacement must dispatch both deletion and application.");
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await ((IApplicationGateway)_gateway).StopAsync();
        Directory.Delete(_directory, recursive: true);
    }

    [Benchmark]
    public async Task<int> ReconcileRepeatedCommands()
    {
        await ((IApplicationGateway)_gateway).ReconcileAsync(_models[_current]);
        return _handler.Applied;
    }

    [Benchmark]
    public async Task<int> ReplaceCommands()
    {
        _current ^= 1;
        await ((IApplicationGateway)_gateway).ReconcileAsync(_models[_current]);
        return _handler.Applied;
    }

    private sealed class Handler : IResourceCommandHandler
    {
        public string Kind => "web.configure";
        public int Applied { get; private set; }
        public int Deleted { get; private set; }
        public ValueTask<ReadOnlyMemory<byte>> ExecuteAsync(CommandEnvelope command, CancellationToken cancellationToken = default)
        {
            Applied++;
            return ValueTask.FromResult(command.Payload);
        }
        public ValueTask<ReadOnlyMemory<byte>> DeleteAsync(CommandEnvelope command, CancellationToken cancellationToken = default)
        {
            Deleted++;
            return ValueTask.FromResult(ReadOnlyMemory<byte>.Empty);
        }
    }

    private sealed class CommandModel(IApplicationModel model, IResourceCommand command) : IApplicationModel
    {
        public ApplicationName Name => model.Name;
        public IApplicationEnvironment Environment => model.Environment;
        public GatewayRunMode RunMode => model.RunMode;
        public ResourceName GatewayIdentity => model.GatewayIdentity;
        public string Owner => model.Owner;
        public bool Adopt => model.Adopt;
        public bool RestartOrphans => model.RestartOrphans;
        public System.Collections.Generic.IReadOnlyList<IApplicationResourceDescriptor> Descriptors => model.Descriptors;
        public System.Collections.Generic.IReadOnlyList<IApplicationResource> Resources => model.Resources;
        public System.Collections.Generic.IReadOnlyList<ResourceManifest> Manifests => model.Manifests;
        public System.Collections.Generic.IReadOnlyList<ResourcePlan> Plans => model.Plans;
        public System.Collections.Generic.IReadOnlyList<IResourceCommand> Commands { get; } = [command];
        public ApplicationProviders Providers => model.Providers;
    }
}

internal sealed record CommandSettings(int Revision, string LogLevel);

[JsonSerializable(typeof(CommandSettings))]
internal partial class CommandJsonContext : JsonSerializerContext;
