using System.Threading;
using System.Threading.Tasks;

using Assimalign.Cohesion.Hosting;
using Assimalign.Cohesion.EventHub.Hosting;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.EventHub.Benchmarks.EventHub.Hosting;

/// <summary>
/// Standalone composition: builder creation, service registration, validated container build,
/// host-service factory resolution and disposal; the lifecycle case also starts and stops the host.
/// Services wait only for cancellation. No orchestrated context, control-plane endpoint or data
/// plane is composed. These area-specific composition contracts have no honest third-party counterpart.
/// </summary>
public class EventHubApplicationBenchmarks
{
    private static readonly string[] Arguments = [];

    [Params(1, 16)]
    public int ServiceCount { get; set; }

    [Benchmark]
    public async Task<EventHubApplicationContext> Build()
    {
        await using EventHubApplication application = CreateBuilder().Build();
        return application.Context;
    }

    [Benchmark]
    public async Task<EventHubApplicationContext> BuildStartStop()
    {
        await using EventHubApplication application = CreateBuilder().Build();
        IHost host = application;
        await host.StartAsync().ConfigureAwait(false);
        await host.StopAsync().ConfigureAwait(false);
        return application.Context;
    }

    private EventHubApplicationBuilder CreateBuilder()
    {
        EventHubApplicationBuilder builder = EventHubApplication.CreateBuilder(Arguments);
        for (int index = 0; index < ServiceCount; index++)
        {
            builder.AddService(static _ => new WaitForStopService());
        }

        return builder;
    }

    /// <summary>Application-owned service that drains immediately when cancellation is signaled.</summary>
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
