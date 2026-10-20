using System;
using System.IO;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Assimalign.Cohesion.FileSystem;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;
using CohesionJson = Assimalign.Cohesion.Configuration.Json;
using Extensions = Microsoft.Extensions.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks.Configuration.FileSystem;

/// <summary>
/// Exercises the shared file-provider base through its public JSON implementation: open, parse, build or reload.
/// Both sides read the same suite-owned temp file with watching disabled. Reload explicitly rereads an unchanged
/// file and rebuilds entries; it measures neither debounce timers nor a filesystem write.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class FileConfigurationBenchmarks
{
    private ProviderSettings _settings = null!;
    private string _directory = "";
    private string _file = "";
    private PhysicalFileSystem _fileSystem = null!;
    private CohesionConfiguration.IConfiguration _cohesion = null!;
    private Extensions.IConfigurationRoot _extensions = null!;

    [Params(8, 128)]
    public int KeyCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _settings = new ProviderSettings(KeyCount);
        _directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"cohesion-configuration-{Guid.NewGuid():N}");
        Directory.CreateDirectory(_directory);
        _file = System.IO.Path.Combine(_directory, "settings.json");
        File.WriteAllBytes(_file, _settings.Json);
        _fileSystem = new PhysicalFileSystem(_directory);
        _cohesion = CreateCohesion();
        _extensions = CreateExtensions();
        _settings.Verify(Build(), Build_MicrosoftExtensions());
        _settings.Verify(Reload().GetAwaiter().GetResult(), Reload_MicrosoftExtensions());
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        (_cohesion as CohesionConfiguration.Configuration)?.Dispose();
        (_extensions as IDisposable)?.Dispose();
        _fileSystem?.Dispose();
        if (Directory.Exists(_directory))
        {
            // This exact directory was created by this setup and contains only its settings file.
            File.Delete(_file);
            Directory.Delete(_directory);
        }
    }

    private CohesionConfiguration.IConfiguration CreateCohesion() => CohesionJson.ConfigurationBuilderExtensions
        .AddJsonFile(new CohesionConfiguration.ConfigurationBuilder(), _fileSystem, "settings.json", optional: false, reloadOnChange: false)
        .Build();

    private Extensions.IConfigurationRoot CreateExtensions() => Extensions.JsonConfigurationExtensions
        .AddJsonFile(new Extensions.ConfigurationBuilder(), _file, optional: false, reloadOnChange: false).Build();

    [Benchmark]
    [BenchmarkCategory("Build")]
    public string? Build()
    {
        CohesionConfiguration.IConfiguration configuration = CreateCohesion();
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
    [BenchmarkCategory("Build")]
    public string? Build_MicrosoftExtensions()
    {
        Extensions.IConfigurationRoot configuration = CreateExtensions();
        try
        {
            return configuration[_settings.LastKey];
        }
        finally
        {
            ((IDisposable)configuration).Dispose();
        }
    }

    [Benchmark]
    [BenchmarkCategory("Reload")]
    public async Task<string?> Reload()
    {
        await CohesionConfiguration.ConfigurationExtensions.ReloadAsync(_cohesion).ConfigureAwait(false);
        return _cohesion[_settings.LastKey];
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Reload")]
    public string? Reload_MicrosoftExtensions()
    {
        _extensions.Reload();
        return _extensions[_settings.LastKey];
    }
}
