using System;

using CohesionDI = Assimalign.Cohesion.DependencyInjection;
using ExtensionsDI = Microsoft.Extensions.DependencyInjection;

namespace Assimalign.Cohesion.DependencyInjection.Benchmarks.DependencyInjection;

/// <summary>
/// The service graph both containers resolve: a singleton, a scoped service that depends on it, and a transient
/// handler that depends on both. Implementation-type registrations, so each container builds its own call sites.
/// </summary>
public static class ServiceGraph
{
    public static IServiceProvider BuildCohesion()
    {
        CohesionDI.IServiceProviderBuilder builder = new CohesionDI.ServiceProviderBuilder();
        builder.AddSingleton<IClock, Clock>();
        builder.AddScoped<IRepository, Repository>();
        builder.AddTransient<IHandler, Handler>();
        return builder.Build();
    }

    public static ExtensionsDI.ServiceProvider BuildExtensions()
    {
        var services = new ExtensionsDI.ServiceCollection();
        ExtensionsDI.ServiceCollectionServiceExtensions.AddSingleton<IClock, Clock>(services);
        ExtensionsDI.ServiceCollectionServiceExtensions.AddScoped<IRepository, Repository>(services);
        ExtensionsDI.ServiceCollectionServiceExtensions.AddTransient<IHandler, Handler>(services);
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
