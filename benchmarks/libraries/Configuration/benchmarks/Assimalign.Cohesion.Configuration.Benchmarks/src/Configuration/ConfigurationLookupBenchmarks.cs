using System;

using BenchmarkDotNet.Attributes;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;
using ExtensionsConfiguration = Microsoft.Extensions.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks.Configuration;

/// <summary>
/// Reading one value by a three-segment key from a built configuration, against
/// Microsoft.Extensions.Configuration (the baseline). This is what every <c>configuration["A:B:C"]</c> call pays
/// after startup.
/// </summary>
/// <remarks>
/// Both sides load the same JSON document through their JSON stream provider; the measurement is the lookup alone.
/// The Cohesion indexer takes a <see cref="CohesionConfiguration.Path"/>, so a string key is parsed on every call
/// (<see cref="PathBenchmarks"/> measures that parse on its own).
/// </remarks>
public class ConfigurationLookupBenchmarks
{
    private CohesionConfiguration.IConfiguration _cohesion = null!;
    private ExtensionsConfiguration.IConfigurationRoot _extensions = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = SampleSettings.BuildCohesion();
        _extensions = SampleSettings.BuildExtensions();

        if (GetValue() != SampleSettings.ExpectedValue || GetValue_MicrosoftExtensions() != SampleSettings.ExpectedValue)
        {
            throw new InvalidOperationException($"'{SampleSettings.ValueKey}' did not resolve on both sides.");
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        (_cohesion as IDisposable)?.Dispose();
        (_extensions as IDisposable)?.Dispose();
    }

    [Benchmark(Baseline = true)]
    public string? GetValue_MicrosoftExtensions() => _extensions[SampleSettings.ValueKey];

    [Benchmark]
    public string? GetValue() => _cohesion[SampleSettings.ValueKey];
}
