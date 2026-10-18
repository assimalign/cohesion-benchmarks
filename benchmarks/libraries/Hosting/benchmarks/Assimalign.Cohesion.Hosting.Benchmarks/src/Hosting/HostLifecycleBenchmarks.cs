using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using ExtensionsHosting = Microsoft.Extensions.Hosting;

namespace Assimalign.Cohesion.Hosting.Benchmarks.Hosting;

/// <summary>
/// Standing a host up and tearing it down: build it with a set of background services, start it (every service's
/// work begins), stop it (every service observes cancellation and is joined), dispose it. Resource executables pay
/// this once per process; test factories and in-process gateways pay it per host. Microsoft.Extensions.Hosting's
/// empty application builder is the baseline, so the Ratio column reads "Cohesion relative to the counterpart".
/// </summary>
/// <remarks>
/// Each service waits for its stop token without throwing, so the measurement is lifecycle orchestration, not
/// exception unwinding. The counterpart's builder always composes a service container, configuration, logging and
/// a console lifetime; the base Cohesion host composes none of them (an area's hosting module adds its container,
/// which the resource suites measure), so the ratio reads as "what each host costs by default", not as a like-for-
/// like comparison of the same features.
/// </remarks>
public class HostLifecycleBenchmarks
{
    [Params(1, 16)]
    public int ServiceCount { get; set; }

    [Benchmark(Baseline = true)]
    public async Task BuildStartStop_MicrosoftExtensions()
    {
        ExtensionsHosting.HostApplicationBuilder builder =
            ExtensionsHosting.Host.CreateEmptyApplicationBuilder(new ExtensionsHosting.HostApplicationBuilderSettings());

        for (int index = 0; index < ServiceCount; index++)
        {
            builder.Services.AddSingleton<ExtensionsHosting.IHostedService>(new ExtensionsWaitForStopService());
        }

        using ExtensionsHosting.IHost host = builder.Build();
        await host.StartAsync().ConfigureAwait(false);
        await host.StopAsync().ConfigureAwait(false);
    }

    [Benchmark]
    public async Task BuildStartStop()
    {
        IHostService[] services = new IHostService[ServiceCount];
        for (int index = 0; index < services.Length; index++)
        {
            services[index] = new WaitForStopService();
        }

        await using BenchmarkHost host = new(services);
        IHost lifecycle = host;
        await lifecycle.StartAsync().ConfigureAwait(false);
        await lifecycle.StopAsync().ConfigureAwait(false);
    }

    private static Task WaitForStop(CancellationToken stoppingToken)
    {
        TaskCompletionSource stopped = new(TaskCreationOptions.RunContinuationsAsynchronously);
        stoppingToken.Register(static state => ((TaskCompletionSource)state!).TrySetResult(), stopped);
        return stopped.Task;
    }

    private sealed class WaitForStopService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken cancellationToken) => WaitForStop(cancellationToken);
    }

    private sealed class ExtensionsWaitForStopService : ExtensionsHosting.BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken) => WaitForStop(stoppingToken);
    }

    private sealed class BenchmarkHostContext : HostContext
    {
        public BenchmarkHostContext(IReadOnlyList<IHostService> services)
        {
            HostedServices = services;
            Environment = new HostEnvironment("Benchmark");
        }

        public override IHostEnvironment Environment { get; }

        public override IEnumerable<IHostService> HostedServices { get; }
    }

    private sealed class BenchmarkHostOptions : HostOptions<BenchmarkHostContext>
    {
    }

    private sealed class BenchmarkHost : Host<BenchmarkHostContext>
    {
        public BenchmarkHost(IReadOnlyList<IHostService> services)
            : base(new BenchmarkHostOptions())
        {
            Context = new BenchmarkHostContext(services);
        }

        public override BenchmarkHostContext Context { get; }
    }
}
