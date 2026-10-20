using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using C = Assimalign.Cohesion.DependencyInjection;
using M = Microsoft.Extensions.DependencyInjection;

namespace Assimalign.Cohesion.DependencyInjection.Benchmarks.DependencyInjection;

/// <summary>
/// Build and dispose from prepared descriptors: 32 distinct service contracts, with one or four
/// registrations per contract as in extensible pipelines. Includes instance, singleton factory,
/// scoped factory and transient factory registrations. Validation is measured independently of resolve;
/// neither container can inspect dependencies hidden inside a factory. Cohesion disables dynamic code.
/// </summary>
// Opted into NativeAOT runs: this path is reflection-free by design, so losing the JIT is a real question for it.
[BenchmarkCategory(global::Assimalign.Cohesion.Benchmarks.BenchmarkRuntimes.Category)]
public class ContainerBuildBenchmarks
{
    private C.IServiceProviderBuilder _builder = null!;
    private M.IServiceCollection _services = null!;
    private M.ServiceProviderOptions _options = null!;

    [Params(32, 128)] public int RegistrationCount { get; set; }
    [Params(false, true)] public bool Validate { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _builder = new C.ServiceProviderBuilder(new C.ServiceProviderOptions
        {
            EnableDynamicCode = false, ValidateOnBuild = Validate, ValidateScopes = Validate,
        });
        _services = new M.ServiceCollection();
        _options = new M.ServiceProviderOptions { ValidateOnBuild = Validate, ValidateScopes = Validate };
        for (int i = 0; i < RegistrationCount / 32; i++)
        {
            AddFamily<int>(); AddFamily<long>(); AddFamily<string>(); AddFamily<Guid>();
            AddFamily<short>(); AddFamily<byte>(); AddFamily<double>(); AddFamily<decimal>();
        }
        if (_builder.Container.Count != RegistrationCount || _services.Count != RegistrationCount)
            throw new InvalidOperationException("Registration count mismatch.");
    }

    private void AddFamily<T>()
    {
        var instance = new Slot<T>();
        _builder.Add(new C.ServiceDescriptor(typeof(Slot<T>), instance));
        _services.Add(new M.ServiceDescriptor(typeof(Slot<T>), instance));
        AddFactory<T[]>(System.ServiceLifetime.Singleton, M.ServiceLifetime.Singleton);
        AddFactory<List<T>>(System.ServiceLifetime.Scoped, M.ServiceLifetime.Scoped);
        AddFactory<Dictionary<string, T>>(System.ServiceLifetime.Transient, M.ServiceLifetime.Transient);
    }

    private void AddFactory<T>(System.ServiceLifetime cohesion, M.ServiceLifetime extensions)
    {
        _builder.Add(new C.ServiceDescriptor(typeof(Slot<T>), static _ => new Slot<T>(), cohesion));
        _services.Add(new M.ServiceDescriptor(typeof(Slot<T>), static _ => new Slot<T>(), extensions));
    }

    [GlobalCleanup] public void Cleanup() => ((IDisposable)_builder).Dispose();

    [Benchmark]
    public int Build()
    {
        var provider = (C.ServiceProvider)_builder.Build();
        using (provider) return RegistrationCount;
    }

    [Benchmark(Baseline = true)]
    public int Build_MicrosoftExtensions()
    {
        using M.ServiceProvider provider = M.ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(_services, _options);
        return RegistrationCount;
    }

    public sealed class Slot<T>;
}
