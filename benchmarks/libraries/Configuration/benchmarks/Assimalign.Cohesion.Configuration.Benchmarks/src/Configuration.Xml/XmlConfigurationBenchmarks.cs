using System;
using System.IO;

using BenchmarkDotNet.Attributes;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;
using CohesionXml = Assimalign.Cohesion.Configuration.Xml;
using Extensions = Microsoft.Extensions.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks.Configuration.Xml;

/// <summary>
/// Parses the same XML element hierarchy into nested settings and disposes the resulting configuration.
/// Neither side decrypts XML or performs file I/O; both expose the same keys and string values.
/// </summary>
public class XmlConfigurationBenchmarks
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
        using MemoryStream stream = ProviderSettings.Open(_settings.Xml);
        CohesionConfiguration.IConfiguration configuration = CohesionXml.ConfigurationBuilderExtensions
            .AddXmlStream(new CohesionConfiguration.ConfigurationBuilder(), stream).Build();
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
        using MemoryStream stream = ProviderSettings.Open(_settings.Xml);
        Extensions.IConfigurationRoot configuration = Extensions.XmlConfigurationExtensions
            .AddXmlStream(new Extensions.ConfigurationBuilder(), stream).Build();
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
