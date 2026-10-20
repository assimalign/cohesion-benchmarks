using System;

using BenchmarkDotNet.Attributes;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;
using CohesionCommandLine = Assimalign.Cohesion.Configuration.CommandLine;
using Extensions = Microsoft.Extensions.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks.Configuration.CommandLine;

/// <summary>Builds nested settings from identical --key=value switches with both public command-line providers.</summary>
public class CommandLineConfigurationBenchmarks
{
    private ProviderSettings _settings = null!;

    [Params(8, 128)]
    public int KeyCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _settings = new ProviderSettings(KeyCount);
        _settings.Verify(Build(), Build_MicrosoftExtensions());
    }

    [Benchmark]
    public string? Build()
    {
        CohesionConfiguration.IConfiguration configuration = CohesionCommandLine.ConfigurationBuilderExtensions
            .AddCommandLine(new CohesionConfiguration.ConfigurationBuilder(), _settings.Arguments).Build();
        try
        {
            return configuration[_settings.LastKey];
        }
        finally
        {
            ((CohesionConfiguration.Configuration)configuration).Dispose();
        }
    }

    [Benchmark(Baseline = true)]
    public string? Build_MicrosoftExtensions()
    {
        Extensions.IConfigurationRoot configuration = Extensions.CommandLineConfigurationExtensions
            .AddCommandLine(new Extensions.ConfigurationBuilder(), _settings.Arguments).Build();
        try
        {
            return configuration[_settings.LastKey];
        }
        finally
        {
            ((IDisposable)configuration).Dispose();
        }
    }
}
