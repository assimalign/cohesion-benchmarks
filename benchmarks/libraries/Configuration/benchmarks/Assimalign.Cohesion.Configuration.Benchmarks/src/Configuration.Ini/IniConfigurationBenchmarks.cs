using System;
using System.IO;

using BenchmarkDotNet.Attributes;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;
using CohesionIni = Assimalign.Cohesion.Configuration.Ini;
using Extensions = Microsoft.Extensions.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks.Configuration.Ini;

/// <summary>
/// Parses identical INI sections and builds a configuration tree, including disposal on both sides.
/// Fresh streams over setup-owned bytes exclude disk I/O while retaining the consumer's provider/build path.
/// </summary>
public class IniConfigurationBenchmarks
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
        using MemoryStream stream = ProviderSettings.Open(_settings.Ini);
        CohesionConfiguration.IConfiguration configuration = CohesionIni.ConfigurationBuilderExtensions
            .AddIniStream(new CohesionConfiguration.ConfigurationBuilder(), stream).Build();
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
        using MemoryStream stream = ProviderSettings.Open(_settings.Ini);
        Extensions.IConfigurationRoot configuration = Extensions.IniConfigurationExtensions
            .AddIniStream(new Extensions.ConfigurationBuilder(), stream).Build();
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
