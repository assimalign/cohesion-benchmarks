using System;

using CohesionDI = Assimalign.Cohesion.DependencyInjection;
using ExtensionsDI = Microsoft.Extensions.DependencyInjection;

namespace Assimalign.Cohesion.DependencyInjection.Benchmarks.DependencyInjection;

/// <summary>
/// The service graph both containers resolve: a singleton, a scoped service that depends on it, and a transient
/// handler that depends on both. Explicit factories on both sides; Cohesion never compiles resolvers.
/// </summary>
public static class ServiceGraph
{
    public static IServiceProvider BuildCohesion()
    {
        CohesionDI.IServiceProviderBuilder builder = new CohesionDI.ServiceProviderBuilder(
            new CohesionDI.ServiceProviderOptions { EnableDynamicCode = false });
        builder.AddSingleton<IClock>(static _ => new Clock());
        builder.AddScoped<IRepository>(static provider => new Repository(CohesionDI.ServiceProviderExtensions.GetRequiredService<IClock>(provider)));
        builder.AddTransient<IHandler>(static provider => new Handler(
            CohesionDI.ServiceProviderExtensions.GetRequiredService<IRepository>(provider),
            CohesionDI.ServiceProviderExtensions.GetRequiredService<IClock>(provider)));
        return builder.Build();
    }

    public static ExtensionsDI.ServiceProvider BuildExtensions()
    {
        var services = new ExtensionsDI.ServiceCollection();
        ExtensionsDI.ServiceCollectionServiceExtensions.AddSingleton<IClock>(services, static _ => new Clock());
        ExtensionsDI.ServiceCollectionServiceExtensions.AddScoped<IRepository>(services, static provider => new Repository(ExtensionsDI.ServiceProviderServiceExtensions.GetRequiredService<IClock>(provider)));
        ExtensionsDI.ServiceCollectionServiceExtensions.AddTransient<IHandler>(services, static provider => new Handler(
            ExtensionsDI.ServiceProviderServiceExtensions.GetRequiredService<IRepository>(provider),
            ExtensionsDI.ServiceProviderServiceExtensions.GetRequiredService<IClock>(provider)));
        return ExtensionsDI.ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
    }

    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }

    public interface IRepository
    {
        IClock Clock { get; }
    }

    public interface IHandler
    {
        IRepository Repository { get; }
    }

    public sealed class Clock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UnixEpoch;
    }

    public sealed class Repository(IClock clock) : IRepository
    {
        public IClock Clock { get; } = clock;
    }

    public sealed class Handler(IRepository repository, IClock clock) : IHandler
    {
        public IRepository Repository { get; } = repository;

        public IClock Clock { get; } = clock;
    }
}
