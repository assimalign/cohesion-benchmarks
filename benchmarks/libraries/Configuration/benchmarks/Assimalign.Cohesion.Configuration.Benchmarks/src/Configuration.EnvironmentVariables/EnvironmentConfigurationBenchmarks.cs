using System;

using BenchmarkDotNet.Attributes;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;
using Extensions = Microsoft.Extensions.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks.Configuration.EnvironmentVariables;

/// <summary>
/// Enumerates the process environment, filters a private prefix, and normalizes double-underscore nested keys.
/// Both providers scan the same ambient environment; setup creates only the suite's variables and cleanup removes them.
/// </summary>
public class EnvironmentConfigurationBenchmarks
{
    private ProviderSettings _settings = null!;
    private string _prefix = "";
    private string[] _names = [];

    [Params(8, 128)]
    public int KeyCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _settings = new ProviderSettings(KeyCount);
        _prefix = $"COHESION_BENCH_{Guid.NewGuid():N}_";
        _names = new string[KeyCount];
        for (int index = 0; index < KeyCount; index++)
        {
            _names[index] = $"{_prefix}Service__Item{index}";
            Environment.SetEnvironmentVariable(_names[index], $"value{index}");
        }

        _settings.Verify(Build(), Build_MicrosoftExtensions());
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        foreach (string name in _names)
        {
            Environment.SetEnvironmentVariable(name, null);
        }
    }

    [Benchmark]
    public string? Build()
    {
        CohesionConfiguration.IConfiguration configuration = CohesionConfiguration.ConfigurationBuilderExtensions
            .AddEnvironmentVariables(new CohesionConfiguration.ConfigurationBuilder(), _prefix).Build();
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
        Extensions.IConfigurationRoot configuration = Extensions.EnvironmentVariablesExtensions
            .AddEnvironmentVariables(new Extensions.ConfigurationBuilder(), _prefix).Build();
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
