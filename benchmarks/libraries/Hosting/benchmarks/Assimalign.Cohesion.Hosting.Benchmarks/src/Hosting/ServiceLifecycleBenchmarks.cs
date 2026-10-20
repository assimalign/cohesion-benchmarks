using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.Benchmarks.External;
using Microsoft.Extensions.DependencyInjection;

using ExtensionsHosting = Microsoft.Extensions.Hosting;

namespace Assimalign.Cohesion.Hosting.Benchmarks.Hosting;

/// <summary>
/// Starts and stops prebuilt hosts with identical service and lifecycle callbacks, in sequential and concurrent modes.
/// Each callback completes immediately and records its execution; there are no timers, background loops or I/O.
/// Host/container construction and disposal belong to iteration setup/cleanup, so this comparison isolates orchestration
/// from the different default host composition measured by the existing HostLifecycleBenchmarks.
/// </summary>
[ExternalWorkload]
public class ServiceLifecycleBenchmarks
{
    private IHost _cohesion = null!;
    private ExtensionsHosting.IHost _extensions = null!;
    private LifecycleService[] _cohesionServices = [];
    private LifecycleService[] _extensionsServices = [];

    [Params(1, 16)]
    public int ServiceCount { get; set; }

    [Params(false, true)]
    public bool Concurrent { get; set; }

    [IterationSetup]
    public void Setup()
    {
        _cohesionServices = CreateServices();
        _extensionsServices = CreateServices();
        _cohesion = new BenchmarkHost(_cohesionServices, Concurrent);
        ExtensionsHosting.HostApplicationBuilder builder = ExtensionsHosting.Host.CreateEmptyApplicationBuilder(
            new ExtensionsHosting.HostApplicationBuilderSettings { EnvironmentName = "Benchmark", ApplicationName = "LifecycleBenchmarks" });
        builder.Services.Configure<ExtensionsHosting.HostOptions>(options =>
        {
            options.ServicesStartConcurrently = Concurrent;
            options.ServicesStopConcurrently = Concurrent;
            options.StartupTimeout = Timeout.InfiniteTimeSpan;
            options.ShutdownTimeout = Timeout.InfiniteTimeSpan;
        });
        foreach (LifecycleService service in _extensionsServices)
        {
            builder.Services.AddSingleton<ExtensionsHosting.IHostedService>(service);
        }

        _extensions = builder.Build();
    }

    [IterationCleanup]
    public void Cleanup()
    {
        _cohesion.Dispose();
        _extensions.Dispose();
    }

    private LifecycleService[] CreateServices()
    {
        LifecycleService[] services = new LifecycleService[ServiceCount];
        for (int index = 0; index < services.Length; index++)
        {
            services[index] = new LifecycleService();
        }

        return services;
    }

    [Benchmark]
    public async Task<int> StartStop()
    {
        await _cohesion.StartAsync().ConfigureAwait(false);
        await _cohesion.StopAsync().ConfigureAwait(false);
        return VerifyCallbacks(_cohesionServices);
    }

    [Benchmark(Baseline = true)]
    public async Task<int> StartStop_MicrosoftExtensions()
    {
        await _extensions.StartAsync().ConfigureAwait(false);
        await _extensions.StopAsync().ConfigureAwait(false);
        return VerifyCallbacks(_extensionsServices);
    }

    private static int VerifyCallbacks(LifecycleService[] services)
    {
        int count = 0;
        foreach (LifecycleService service in services)
        {
            if (service.CallbackCount != 6)
            {
                throw new InvalidOperationException("A lifecycle callback was skipped or repeated.");
            }

            count += service.CallbackCount;
        }

        return count;
    }

    private sealed class LifecycleService : IHostLifecycleService, ExtensionsHosting.IHostedLifecycleService
    {
        public ServiceId Id { get; } = ServiceId.New();
        public int CallbackCount { get; private set; }

        private Task Record()
        {
            CallbackCount++;
            return Task.CompletedTask;
        }

        public Task StartingAsync(CancellationToken cancellationToken) => Record();
        public Task StartAsync(CancellationToken cancellationToken) => Record();
        public Task StartedAsync(CancellationToken cancellationToken) => Record();
        public Task StoppingAsync(CancellationToken cancellationToken) => Record();
        public Task StopAsync(CancellationToken cancellationToken) => Record();
        public Task StoppedAsync(CancellationToken cancellationToken) => Record();
    }

    private sealed class BenchmarkContext : HostContext
    {
        public BenchmarkContext(IReadOnlyList<IHostService> services) => HostedServices = services;
        public override IHostEnvironment Environment { get; } = new HostEnvironment("Benchmark");
        public override IEnumerable<IHostService> HostedServices { get; }
    }

    private sealed class BenchmarkOptions : HostOptions<BenchmarkContext>
    {
    }

    private sealed class BenchmarkHost : Host<BenchmarkContext>
    {
        public BenchmarkHost(IReadOnlyList<IHostService> services, bool concurrent)
            : base(new BenchmarkOptions
            {
                StartServicesConcurrently = concurrent,
                StopServicesConcurrently = concurrent,
                StartupTimeout = Timeout.InfiniteTimeSpan,
                ShutdownTimeout = Timeout.InfiniteTimeSpan,
                Environment = "Benchmark"
            }) => Context = new BenchmarkContext(services);

        public override BenchmarkContext Context { get; }
    }
}
