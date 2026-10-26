using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using BenchmarkDotNet.Attributes;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;
using ExtensionsConfiguration = Microsoft.Extensions.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks.Configuration;

/// <summary>
/// Binding a configuration section to an options object (scalars, a nested object, a list of strings and a list of
/// objects), against Microsoft.Extensions.Configuration.Binder's reflection binder (the baseline). Every options type
/// an application reads from configuration pays this at startup, and again on every reload.
/// </summary>
/// <remarks>
/// Both sides are reflection binders (each <c>Get</c> is <c>[RequiresUnreferencedCode]</c>), so a NativeAOT build only
/// binds what it kept: <see cref="ServiceOptions"/>' own members (Cohesion's <c>Get&lt;T&gt;</c> asks for them), not
/// those of the types under it. <see cref="Setup"/> keeps them the way an AOT application binding these types has to;
/// without that both binders leave <c>Timeouts</c>, <c>Features</c> and <c>Endpoints</c> empty.
/// </remarks>
// Opted into NativeAOT runs: reflection costs differently once the JIT is gone, so it is a real question for a binder.
[BenchmarkCategory(global::Assimalign.Cohesion.Benchmarks.BenchmarkRuntimes.Category)]
public class ConfigurationBindingBenchmarks
{
    private const string Section = "Service";

    private CohesionConfiguration.IConfiguration _cohesion = null!;
    private ExtensionsConfiguration.IConfigurationRoot _extensions = null!;
    private CohesionConfiguration.Path _section;

    [GlobalSetup]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TimeoutOptions))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(EndpointOptions))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(List<string>))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(List<EndpointOptions>))]
    public void Setup()
    {
        _cohesion = SampleSettings.BuildCohesion();
        _extensions = SampleSettings.BuildExtensions();
        _section = CohesionConfiguration.Path.Parse(Section);

        ServiceOptions cohesion = Bind();
        ServiceOptions extensions = Bind_MicrosoftExtensions();

        // The baseline has to bind the whole document first, or two equally empty results would compare equal.
        if (extensions.Endpoints.Count != 2)
        {
            throw new InvalidOperationException("The baseline binder left the nested members unbound.");
        }

        if (!cohesion.SameAs(extensions))
        {
            throw new InvalidOperationException("The two binders produced different options.");
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        (_cohesion as CohesionConfiguration.Configuration)?.Dispose();
        (_extensions as IDisposable)?.Dispose();
    }

    [Benchmark(Baseline = true)]
    public ServiceOptions Bind_MicrosoftExtensions()
        => ExtensionsConfiguration.ConfigurationBinder.Get<ServiceOptions>(_extensions.GetSection(Section))!;

    [Benchmark]
    public ServiceOptions Bind()
        => CohesionConfiguration.ConfigurationBinder.Get<ServiceOptions>(_cohesion, _section);

    public sealed class ServiceOptions
    {
        public string Name { get; set; } = "";

        public int Port { get; set; }

        public TimeoutOptions Timeouts { get; set; } = new();

        public List<string> Features { get; set; } = [];

        public List<EndpointOptions> Endpoints { get; set; } = [];

        internal bool SameAs(ServiceOptions other)
        {
            if (Name != other.Name || Port != other.Port
                || Timeouts.Connect != other.Timeouts.Connect
                || Timeouts.Request != other.Timeouts.Request
                || Timeouts.Idle != other.Timeouts.Idle
                || Features.Count != other.Features.Count
                || Endpoints.Count != other.Endpoints.Count)
            {
                return false;
            }

            for (int i = 0; i < Features.Count; i++)
            {
                if (Features[i] != other.Features[i])
                {
                    return false;
                }
            }

            for (int i = 0; i < Endpoints.Count; i++)
            {
                if (Endpoints[i].Name != other.Endpoints[i].Name || Endpoints[i].Url != other.Endpoints[i].Url)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public sealed class TimeoutOptions
    {
        public TimeSpan Connect { get; set; }

        public TimeSpan Request { get; set; }

        public TimeSpan Idle { get; set; }
    }

    public sealed class EndpointOptions
    {
        public string Name { get; set; } = "";

        public string Url { get; set; } = "";
    }
}
