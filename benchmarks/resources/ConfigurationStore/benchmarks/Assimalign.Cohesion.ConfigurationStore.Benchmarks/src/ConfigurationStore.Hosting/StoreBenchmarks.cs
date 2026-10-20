using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.ConfigurationStore.Hosting;
using Assimalign.Cohesion.Hosting.Resources;

namespace Assimalign.Cohesion.ConfigurationStore.Benchmarks;

/// <summary>
/// Measures the internal file-backed repository because the shipped service has no public engine
/// facade that can run without the HTTP host. Reflection and delegate binding happen only in setup;
/// commands use the public IResourceCommandHandler contract. Reads return defensive snapshots,
/// writes copy and atomically persist JSON, and lists are sorted. A BCL dictionary or JSON encoder
/// does not perform the same work, so these are standalone benchmarks. Mutation cycles restore
/// their initial state on every invocation and never accumulate namespaces or keys.
/// </summary>
public class StoreBenchmarks
{
    private DirectoryInfo _directory = null!;
    private object _repository = null!;
    private Func<string, CancellationToken, Task<IReadOnlyDictionary<string, string?>?>> _read = null!;
    private Func<CancellationToken, Task<IReadOnlyList<string>>> _list = null!;
    private Func<string, string, string?, CancellationToken, Task<bool>> _set = null!;
    private Func<string, string, CancellationToken, Task<bool>> _remove = null!;
    private IResourceCommandHandler _namespaceHandler = null!;
    private IResourceCommandHandler _setHandler = null!;
    private IResourceCommandHandler _removeHandler = null!;
    private ResourceCommand _declared = null!;
    private ResourceCommand _transient = null!;
    private ResourceCommand _setCommand = null!;
    private ResourceCommand _removeCommand = null!;
    private bool _alternate;

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
        _set = Bind<Func<string, string, string?, CancellationToken, Task<bool>>>("SetAsync");
        _remove = Bind<Func<string, string, CancellationToken, Task<bool>>>("RemoveAsync");
        await Bind<Func<CancellationToken, Task>>("InitializeAsync")(default);
        Type namespaceHandlerType = assembly.GetType("Assimalign.Cohesion.ConfigurationStore.Hosting.Internal.ConfigurationNamespaceCommandHandler", true)!;
        _namespaceHandler = (IResourceCommandHandler)Activator.CreateInstance(namespaceHandlerType, new[] { _repository })!;
        Type commandHandlerType = assembly.GetType("Assimalign.Cohesion.ConfigurationStore.Hosting.Internal.ConfigurationResourceCommandHandler", true)!;
        _setHandler = Handler("configurationstore.set-value");
        _removeHandler = Handler("configurationstore.remove-value");
        _declared = NamespaceCommand("declared", values);
        _transient = NamespaceCommand("transient", values);
        _setCommand = new ResourceCommand("set", "configurationstore.set-value", "benchmarks", "settings/Key0001",
            """{"namespace":"settings","key":"Key0001","value":"command-value"}"""u8.ToArray());
        _removeCommand = new ResourceCommand("remove", "configurationstore.remove-value", "benchmarks", "settings/Transient",
            """{"namespace":"settings","key":"Transient"}"""u8.ToArray());
        await _namespaceHandler.ExecuteAsync(_declared);
        if ((await _read("settings", default))?.Count != EntryCount || await _read("missing", default) is not null
            || (await _list(default)).Count != EntryCount + 2)
        {
            throw new InvalidOperationException("Repository fixtures have unexpected contents.");
        }
        await _setHandler.ExecuteAsync(_setCommand);
        if ((await _read("settings", default))!["Key0001"] != "command-value")
        {
            throw new InvalidOperationException("Set command did not update the namespace.");
        }
        await _set("settings", "Key0001", values["Key0001"], default);
        await _namespaceHandler.ExecuteAsync(_transient);
        await _namespaceHandler.DeleteAsync(_transient);
        if (await _read("transient", default) is not null)
        {
            throw new InvalidOperationException("Namespace deletion failed.");
        }

        IResourceCommandHandler Handler(string kind) => (IResourceCommandHandler)Activator.CreateInstance(commandHandlerType,
            BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { (object)kind, _repository }, null)!;
    }

    private static ResourceCommand NamespaceCommand(string name, Dictionary<string, string?> values) =>
        new(name, "configurationstore.add-namespace", "benchmarks", name,
            JsonSerializer.SerializeToUtf8Bytes(new { name, seed = values }));

    private T Bind<T>(string name) where T : Delegate =>
        _repository.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)!.CreateDelegate<T>(_repository);

    [Benchmark] public Task<IReadOnlyDictionary<string, string?>?> ReadNamespace() => _read("settings", default);
    [Benchmark] public Task<IReadOnlyDictionary<string, string?>?> ReadMissingNamespace() => _read("missing", default);
    [Benchmark] public Task<IReadOnlyList<string>> ListNamespaces() => _list(default);

    [Benchmark]
    public async Task<string?> ReadAndLookupKey() => (await _read("settings", default))!["Key0001"];

    [Benchmark]
    public Task<bool> SetValue()
    {
        _alternate = !_alternate;
        return _set("settings", "Key0001", _alternate ? "enabled" : "disabled", default);
    }

    [Benchmark]
    public async Task<bool> SetAndRemoveValue()
    {
        await _set("settings", "Transient", "temporary", default);
        return await _remove("settings", "Transient", default);
    }

    [Benchmark] public ValueTask<ReadOnlyMemory<byte>> ReplayNamespaceCommand() => _namespaceHandler.ExecuteAsync(_declared);

    [Benchmark]
    public async Task<ReadOnlyMemory<byte>> CreateAndDeleteNamespaceCommand()
    {
        await _namespaceHandler.ExecuteAsync(_transient);
        return await _namespaceHandler.DeleteAsync(_transient);
    }

    [Benchmark] public ValueTask<ReadOnlyMemory<byte>> SetValueCommand() => _setHandler.ExecuteAsync(_setCommand);

    [Benchmark]
    public async Task<ReadOnlyMemory<byte>> SetAndRemoveValueCommand()
    {
        await _set("settings", "Transient", "temporary", default);
        return await _removeHandler.ExecuteAsync(_removeCommand);
    }

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
