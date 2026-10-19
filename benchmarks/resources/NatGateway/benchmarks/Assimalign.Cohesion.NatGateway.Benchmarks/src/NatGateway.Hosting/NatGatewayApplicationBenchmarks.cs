using System.Threading;
using System.Threading.Tasks;

using Assimalign.Cohesion.Hosting;
using Assimalign.Cohesion.NatGateway.Hosting;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.NatGateway.Benchmarks.NatGateway.Hosting;

/// <summary>
/// The NAT gateway resource's composition root, end to end: create the builder, register background services, build
/// (container, provider, hosted-service snapshot), start, stop, dispose. This is the area's only executable code
/// today: translation, sessions and port allocation are still a placeholder data plane, so there is no packet or
/// lookup hot path to measure yet. Run outside an orchestrated resource context, so no control plane or listener
/// is composed.
/// </summary>
public class NatGatewayApplicationBenchmarks
{
    private static readonly string[] Arguments = [];

    [Params(1, 16)]
    public int ServiceCount { get; set; }

    [Benchmark]
    public async Task BuildStartStop()
    {
        NatGatewayApplicationBuilder builder = NatGatewayApplication.CreateBuilder(Arguments);
        for (int index = 0; index < ServiceCount; index++)
        {
            builder.AddService(new WaitForStopService());
        }

        await using NatGatewayApplication application = builder.Build();
        IHost host = application;
        await host.StartAsync().ConfigureAwait(false);
        await host.StopAsync().ConfigureAwait(false);
    }

    /// <summary>Waits for its stop token without throwing, so the measurement is lifecycle orchestration.</summary>
    private sealed class WaitForStopService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            TaskCompletionSource stopped = new(TaskCreationOptions.RunContinuationsAsynchronously);
            cancellationToken.Register(static state => ((TaskCompletionSource)state!).TrySetResult(), stopped);
            return stopped.Task;
        }
    }
}
