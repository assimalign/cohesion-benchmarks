using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;
using CohesionJson = Assimalign.Cohesion.Configuration.Json;
using Extensions = Microsoft.Extensions.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks.Configuration;

/// <summary>
/// Resolves and enumerates a populated section, or looks up a missing leaf in that section, on prebuilt roots.
/// Enumeration consumes every child and returns its count. Both sides expose identical child keys;
/// order is deliberately not part of the comparison because the providers have different ordering contracts.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class SectionEnumerationBenchmarks
{
    private CohesionConfiguration.IConfiguration _cohesion = null!;
    private Extensions.IConfigurationRoot _extensions = null!;

    [Params(8, 128)]
    public int KeyCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        ProviderSettings settings = new(KeyCount);
        _cohesion = CohesionJson.ConfigurationBuilderExtensions
            .AddJsonStream(new CohesionConfiguration.ConfigurationBuilder(), ProviderSettings.Open(settings.Json)).Build();
        _extensions = Extensions.JsonConfigurationExtensions
            .AddJsonStream(new Extensions.ConfigurationBuilder(), ProviderSettings.Open(settings.Json)).Build();
        if (Enumerate() != KeyCount || Enumerate_MicrosoftExtensions() != KeyCount
            || GetMissingValue() is not null || GetMissingValue_MicrosoftExtensions() is not null)
        {
            throw new InvalidOperationException("The section contents do not match the fixture.");
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        (_cohesion as CohesionConfiguration.Configuration)?.Dispose();
        (_extensions as IDisposable)?.Dispose();
    }

    [Benchmark]
    [BenchmarkCategory("Enumerate")]
    public int Enumerate()
    {
        CohesionConfiguration.IConfigurationSection section = (CohesionConfiguration.IConfigurationSection)_cohesion.GetEntry("Service")!;
        int count = 0;
        foreach (CohesionConfiguration.IConfigurationEntry child in section.GetChildren())
        {
            count++;
        }

        return count;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Enumerate")]
    public int Enumerate_MicrosoftExtensions()
    {
        int count = 0;
        foreach (Extensions.IConfigurationSection child in _extensions.GetSection("Service").GetChildren())
        {
            count++;
        }

        return count;
    }

    [Benchmark]
    [BenchmarkCategory("MissingValue")]
    public string? GetMissingValue() => _cohesion["Service:Missing"];

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("MissingValue")]
    public string? GetMissingValue_MicrosoftExtensions() => _extensions["Service:Missing"];
}
