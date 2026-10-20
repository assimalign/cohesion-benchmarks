using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using C = Assimalign.Cohesion.DependencyInjection;
using M = Microsoft.Extensions.DependencyInjection;

namespace Assimalign.Cohesion.DependencyInjection.Benchmarks.DependencyInjection;

/// <summary>
/// Warm scoped/instance lookup, empty scope churn, and first scoped activation with disposal tracking.
/// Both sides register the same factories/instances; Cohesion's dynamic compilation is disabled.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ScopeLifetimeBenchmarks
{
    private C.ServiceProvider _cohesion = null!;
    private M.ServiceProvider _extensions = null!;
    private C.IServiceScope _cohesionScope = null!;
    private M.IServiceScope _extensionsScope = null!;

    [GlobalSetup]
    public void Setup()
    {
        C.IServiceProviderBuilder builder = new C.ServiceProviderBuilder(new C.ServiceProviderOptions { EnableDynamicCode = false });
        var instance = new ServiceGraph.Clock();
        builder.AddSingleton<ServiceGraph.IClock>(instance);
        builder.AddScoped<Resource>(static _ => new Resource());
        _cohesion = (C.ServiceProvider)builder.Build();
        ((IDisposable)builder).Dispose();
        var services = new M.ServiceCollection();
        M.ServiceCollectionServiceExtensions.AddSingleton<ServiceGraph.IClock>(services, instance);
        M.ServiceCollectionServiceExtensions.AddScoped<Resource>(services, static _ => new Resource());
        _extensions = M.ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
        _cohesionScope = C.ServiceProviderExtensions.CreateScope(_cohesion);
        _extensionsScope = M.ServiceProviderServiceExtensions.CreateScope(_extensions);
        if (!ReferenceEquals(ResolveScoped(), ResolveScoped()) ||
            !ReferenceEquals(ResolveScoped_MicrosoftExtensions(), ResolveScoped_MicrosoftExtensions()) ||
            !ReferenceEquals(ResolveInstance(), ResolveInstance_MicrosoftExtensions()))
            throw new InvalidOperationException("Lifetime mismatch.");
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _cohesionScope.Dispose(); _extensionsScope.Dispose();
        _cohesion.Dispose(); _extensions.Dispose();
    }

    [Benchmark, BenchmarkCategory("Scoped")]
    public Resource ResolveScoped() => C.ServiceProviderExtensions.GetRequiredService<Resource>(_cohesionScope.ServiceProvider);
    [Benchmark(Baseline = true), BenchmarkCategory("Scoped")]
    public Resource ResolveScoped_MicrosoftExtensions() => M.ServiceProviderServiceExtensions.GetRequiredService<Resource>(_extensionsScope.ServiceProvider);

    [Benchmark, BenchmarkCategory("Instance")]
    public ServiceGraph.IClock ResolveInstance() => C.ServiceProviderExtensions.GetRequiredService<ServiceGraph.IClock>(_cohesion);
    [Benchmark(Baseline = true), BenchmarkCategory("Instance")]
    public ServiceGraph.IClock ResolveInstance_MicrosoftExtensions() => M.ServiceProviderServiceExtensions.GetRequiredService<ServiceGraph.IClock>(_extensions);

    [Benchmark, BenchmarkCategory("EmptyScope")]
    public void CreateAndDisposeScope() { using C.IServiceScope scope = C.ServiceProviderExtensions.CreateScope(_cohesion); }
    [Benchmark(Baseline = true), BenchmarkCategory("EmptyScope")]
    public void CreateAndDisposeScope_MicrosoftExtensions() { using M.IServiceScope scope = M.ServiceProviderServiceExtensions.CreateScope(_extensions); }

    [Benchmark, BenchmarkCategory("OwnedScope")]
    public Resource ActivateAndDisposeScope()
    {
        using C.IServiceScope scope = C.ServiceProviderExtensions.CreateScope(_cohesion);
        return C.ServiceProviderExtensions.GetRequiredService<Resource>(scope.ServiceProvider);
    }
    [Benchmark(Baseline = true), BenchmarkCategory("OwnedScope")]
    public Resource ActivateAndDisposeScope_MicrosoftExtensions()
    {
        using M.IServiceScope scope = M.ServiceProviderServiceExtensions.CreateScope(_extensions);
        return M.ServiceProviderServiceExtensions.GetRequiredService<Resource>(scope.ServiceProvider);
    }

    public sealed class Resource : IDisposable
    {
        public bool Disposed { get; private set; }
        public void Dispose() => Disposed = true;
    }
}
