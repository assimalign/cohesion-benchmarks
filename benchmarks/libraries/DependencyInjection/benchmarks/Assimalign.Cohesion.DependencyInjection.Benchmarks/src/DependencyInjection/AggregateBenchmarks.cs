using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using C = Assimalign.Cohesion.DependencyInjection;
using M = Microsoft.Extensions.DependencyInjection;

namespace Assimalign.Cohesion.DependencyInjection.Benchmarks.DependencyInjection;

/// <summary>Resolve and enumerate transient pipeline contributors from warmed providers, using explicit factories.</summary>
public class AggregateBenchmarks
{
    private C.ServiceProvider _cohesion = null!;
    private M.ServiceProvider _extensions = null!;

    [Params(4, 16)] public int Contributors { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        C.IServiceProviderBuilder builder = new C.ServiceProviderBuilder(new C.ServiceProviderOptions { EnableDynamicCode = false });
        var services = new M.ServiceCollection();
        for (int i = 0; i < Contributors; i++)
        {
            int id = i;
            builder.AddTransient<Contributor>(_ => new Contributor(id));
            M.ServiceCollectionServiceExtensions.AddTransient<Contributor>(services, _ => new Contributor(id));
        }
        _cohesion = (C.ServiceProvider)builder.Build();
        ((IDisposable)builder).Dispose();
        _extensions = M.ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
        int expected = Contributors * (Contributors - 1) / 2;
        if (ResolveAggregate() != expected || ResolveAggregate_MicrosoftExtensions() != expected)
            throw new InvalidOperationException("Aggregate mismatch.");
    }

    [GlobalCleanup] public void Cleanup() { _cohesion.Dispose(); _extensions.Dispose(); }

    [Benchmark]
    public int ResolveAggregate() => C.ServiceProviderExtensions.GetRequiredService<IEnumerable<Contributor>>(_cohesion).Sum(static item => item.Id);

    [Benchmark(Baseline = true)]
    public int ResolveAggregate_MicrosoftExtensions() => M.ServiceProviderServiceExtensions.GetRequiredService<IEnumerable<Contributor>>(_extensions).Sum(static item => item.Id);

    public sealed record Contributor(int Id);
}
