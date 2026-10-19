using System;

using BenchmarkDotNet.Attributes;

using CohesionDI = Assimalign.Cohesion.DependencyInjection;
using ExtensionsDI = Microsoft.Extensions.DependencyInjection;

namespace Assimalign.Cohesion.DependencyInjection.Benchmarks.DependencyInjection;

/// <summary>
/// Building a provider and resolving the first transient from a first scope, against
/// Microsoft.Extensions.DependencyInjection (the baseline). This is the container's share of host startup: provider
/// construction plus the first, uncached call-site build for each service in the graph.
/// </summary>
public class ServiceProviderBuildBenchmarks
{
    [Benchmark(Baseline = true)]
    public ServiceGraph.IHandler BuildAndResolve_MicrosoftExtensions()
    {
        using ExtensionsDI.ServiceProvider provider = ServiceGraph.BuildExtensions();
        using ExtensionsDI.IServiceScope scope = ExtensionsDI.ServiceProviderServiceExtensions.CreateScope(provider);
        return ExtensionsDI.ServiceProviderServiceExtensions.GetRequiredService<ServiceGraph.IHandler>(scope.ServiceProvider);
    }

    [Benchmark]
    public ServiceGraph.IHandler BuildAndResolve()
    {
        IServiceProvider provider = ServiceGraph.BuildCohesion();
        try
        {
            using CohesionDI.IServiceScope scope = CohesionDI.ServiceProviderExtensions.CreateScope(provider);
            return CohesionDI.ServiceProviderExtensions.GetRequiredService<ServiceGraph.IHandler>(scope.ServiceProvider);
        }
        finally
        {
            (provider as IDisposable)?.Dispose();
        }
    }
}
