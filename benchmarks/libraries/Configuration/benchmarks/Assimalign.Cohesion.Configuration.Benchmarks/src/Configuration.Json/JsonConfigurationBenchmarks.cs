using System;

using BenchmarkDotNet.Attributes;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;
using ExtensionsConfiguration = Microsoft.Extensions.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks.Configuration.Json;

/// <summary>
/// Building a configuration from an appsettings-style JSON stream and reading one value back, against
/// Microsoft.Extensions.Configuration.Json (the baseline). This is the startup cost of loading a settings file,
/// without the file I/O.
/// </summary>
public class JsonConfigurationBenchmarks
{
    [Benchmark(Baseline = true)]
    public string? Build_MicrosoftExtensions()
    {
        ExtensionsConfiguration.IConfigurationRoot configuration = SampleSettings.BuildExtensions();
        string? value = configuration[SampleSettings.ValueKey];
        (configuration as IDisposable)?.Dispose();
        return value;
    }

    [Benchmark]
    public string? Build()
    {
        CohesionConfiguration.IConfiguration configuration = SampleSettings.BuildCohesion();
        string? value = configuration[SampleSettings.ValueKey];
        ((CohesionConfiguration.Configuration)configuration).Dispose();
        return value;
    }
}
