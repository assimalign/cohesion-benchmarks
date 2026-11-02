using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.ConfigurationStore.Hosting;

namespace Assimalign.Cohesion.ConfigurationStore.Benchmarks;

/// <summary>
/// Measures the internal file-backed repository because the shipped service has no public engine
/// facade that can run without the HTTP host. Reflection and delegate binding happen only in setup.
/// Reads return defensive snapshots and lists are sorted. A BCL dictionary or JSON encoder does not
/// perform the same work, so these are standalone benchmarks. Only the paths the shipped endpoint
/// service calls (initialize, read, list) are measured: the repository's set/remove methods have no
/// caller at the pinned build, since the resource command path that drove them was removed.
/// </summary>
public class StoreBenchmarks
{
    private DirectoryInfo _directory = null!;
    private object _repository = null!;
    private Func<string, CancellationToken, Task<IReadOnlyDictionary<string, string?>?>> _read = null!;
    private Func<CancellationToken, Task<IReadOnlyList<string>>> _list = null!;

    [Params(8, 128)]
    public int EntryCount { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        _directory = Directory.CreateTempSubdirectory("cohesion-configuration-benchmarks-");
        var values = new Dictionary<string, string?>(StringComparer.Ordinal);
        var seeds = new Dictionary<string, IReadOnlyDictionary<string, string?>>(StringComparer.Ordinal);
        for (int i = 0; i < EntryCount; i++)
        {
            values.Add($"Key{i:D4}", i % 4 == 0 ? null : $"value-{i:D4}-\u03bb");
            seeds.Add($"namespace-{i:D4}", new Dictionary<string, string?> { ["Mode"] = "active" });
        }
        seeds.Add("settings", values);
        Assembly assembly = typeof(ConfigurationStoreApplication).Assembly;
        Type type = assembly.GetType("Assimalign.Cohesion.ConfigurationStore.Hosting.Internal.ConfigurationStoreRepository", true)!;
        _repository = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.NonPublic, null,
            new object[] { _directory.FullName, seeds }, null)!;
        _read = Bind<Func<string, CancellationToken, Task<IReadOnlyDictionary<string, string?>?>>>("ReadAsync");
        _list = Bind<Func<CancellationToken, Task<IReadOnlyList<string>>>>("ListAsync");
        await Bind<Func<CancellationToken, Task>>("InitializeAsync")(default);
        if ((await _read("settings", default))?.Count != EntryCount || await _read("missing", default) is not null
            || (await _list(default)).Count != EntryCount + 1)
        {
            throw new InvalidOperationException("Repository fixtures have unexpected contents.");
        }
        if ((await _read("settings", default))!["Key0001"] != values["Key0001"])
        {
            throw new InvalidOperationException("Namespace values were not seeded.");
        }
    }

    private T Bind<T>(string name) where T : Delegate =>
        _repository.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)!.CreateDelegate<T>(_repository);

    [Benchmark] public Task<IReadOnlyDictionary<string, string?>?> ReadNamespace() => _read("settings", default);
    [Benchmark] public Task<IReadOnlyDictionary<string, string?>?> ReadMissingNamespace() => _read("missing", default);
    [Benchmark] public Task<IReadOnlyList<string>> ListNamespaces() => _list(default);

    [Benchmark]
    public async Task<string?> ReadAndLookupKey() => (await _read("settings", default))!["Key0001"];

    [GlobalCleanup]
    public void Cleanup()
    {
        if (_repository is not null)
        {
            ((SemaphoreSlim)_repository.GetType().GetField("_gate", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(_repository)!).Dispose();
        }
        _directory?.Delete(recursive: true);
    }
}
