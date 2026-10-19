using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using CohesionDI = Assimalign.Cohesion.DependencyInjection;
using ExtensionsDI = Microsoft.Extensions.DependencyInjection;

namespace Assimalign.Cohesion.DependencyInjection.Benchmarks.DependencyInjection;

/// <summary>
/// Resolving services from a built provider, against Microsoft.Extensions.DependencyInjection (the baseline in
/// each category): a singleton from the root, a transient with a scoped and a singleton dependency from a live
/// scope, and the per-request pattern of creating a scope, resolving from it and disposing it.
/// </summary>
/// <remarks>
/// Both providers use their defaults, so both may switch a hot call site to a compiled resolver after the first
/// resolutions; the measured steady state is what a long-running host pays. Each side is called through its own
/// <c>GetRequiredService</c> and <c>CreateScope</c> helpers.
/// </remarks>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ServiceResolutionBenchmarks
{
    private IServiceProvider _cohesion = null!;
    private ExtensionsDI.ServiceProvider _extensions = null!;
    private CohesionDI.IServiceScope _cohesionScope = null!;
    private ExtensionsDI.IServiceScope _extensionsScope = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = ServiceGraph.BuildCohesion();
        _extensions = ServiceGraph.BuildExtensions();
        _cohesionScope = CohesionDI.ServiceProviderExtensions.CreateScope(_cohesion);
        _extensionsScope = ExtensionsDI.ServiceProviderServiceExtensions.CreateScope(_extensions);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _cohesionScope.Dispose();
        _extensionsScope.Dispose();
        (_cohesion as IDisposable)?.Dispose();
        _extensions.Dispose();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Singleton")]
    public ServiceGraph.IClock ResolveSingleton_MicrosoftExtensions()
        => ExtensionsDI.ServiceProviderServiceExtensions.GetRequiredService<ServiceGraph.IClock>(_extensions);

    [Benchmark]
    [BenchmarkCategory("Singleton")]
    public ServiceGraph.IClock ResolveSingleton()
        => CohesionDI.ServiceProviderExtensions.GetRequiredService<ServiceGraph.IClock>(_cohesion);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Transient")]
    public ServiceGraph.IHandler ResolveTransient_MicrosoftExtensions()
        => ExtensionsDI.ServiceProviderServiceExtensions.GetRequiredService<ServiceGraph.IHandler>(_extensionsScope.ServiceProvider);

    [Benchmark]
    [BenchmarkCategory("Transient")]
    public ServiceGraph.IHandler ResolveTransient()
        => CohesionDI.ServiceProviderExtensions.GetRequiredService<ServiceGraph.IHandler>(_cohesionScope.ServiceProvider);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Scope")]
    public ServiceGraph.IHandler CreateScopeAndResolve_MicrosoftExtensions()
    {
        using ExtensionsDI.IServiceScope scope = ExtensionsDI.ServiceProviderServiceExtensions.CreateScope(_extensions);
        return ExtensionsDI.ServiceProviderServiceExtensions.GetRequiredService<ServiceGraph.IHandler>(scope.ServiceProvider);
    }

    [Benchmark]
    [BenchmarkCategory("Scope")]
    public ServiceGraph.IHandler CreateScopeAndResolve()
    {
        using CohesionDI.IServiceScope scope = CohesionDI.ServiceProviderExtensions.CreateScope(_cohesion);
        return CohesionDI.ServiceProviderExtensions.GetRequiredService<ServiceGraph.IHandler>(scope.ServiceProvider);
    }
}
