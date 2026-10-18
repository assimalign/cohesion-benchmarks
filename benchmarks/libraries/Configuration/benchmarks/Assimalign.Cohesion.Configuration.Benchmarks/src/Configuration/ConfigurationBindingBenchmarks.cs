using System;
using System.Collections.Generic;

using BenchmarkDotNet.Attributes;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;
using ExtensionsConfiguration = Microsoft.Extensions.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks.Configuration;

/// <summary>
/// Binding a configuration section to an options object (scalars, a nested object, a list of strings and a list of
/// objects), against Microsoft.Extensions.Configuration.Binder's reflection binder (the baseline). Every options type
/// an application reads from configuration pays this at startup, and again on every reload.
/// </summary>
public class ConfigurationBindingBenchmarks
{
    private const string Section = "Service";

    private CohesionConfiguration.IConfiguration _cohesion = null!;
    private ExtensionsConfiguration.IConfigurationRoot _extensions = null!;
    private CohesionConfiguration.Path _section;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = SampleSettings.BuildCohesion();
        _extensions = SampleSettings.BuildExtensions();
        _section = CohesionConfiguration.Path.Parse(Section);

        ServiceOptions cohesion = Bind();
        ServiceOptions extensions = Bind_MicrosoftExtensions();

        if (extensions.Endpoints.Count != 2 || !cohesion.SameAs(extensions))
        {
            throw new InvalidOperationException("The two binders produced different options.");
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        (_cohesion as IDisposable)?.Dispose();
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
