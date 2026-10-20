using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.Hosting.Resources;
using Assimalign.Cohesion.SecretStore.Hosting;
using Assimalign.Cohesion.Security.DataProtection;

namespace Assimalign.Cohesion.SecretStore.Benchmarks;

/// <summary>
/// The shipped repository is internal and file-backed, with no public in-process engine facade.
/// Bind typed delegates once in setup to measure the engine itself without starting the HTTP host.
/// Reads include authenticated decryption; writes include framing, encryption and atomic durable
/// replacement. Command methods additionally measure payload validation and base64 decoding through
/// IResourceCommandHandler. Standalone: a dictionary or raw BCL AES lacks these guarantees.
/// Protect/Unprotect isolate the exact purpose-scoped protector used by the store, with a warm key
/// ring; plain AES-GCM lacks its key lookup, HKDF derivation and versioned authenticated envelope.
/// </summary>
public class StoreBenchmarks
{
    private DirectoryInfo _directory = null!;
    private object _repository = null!;
    private IDataProtector _protector = null!;
    private byte[] _value = null!;
    private byte[] _protected = null!;
    private ResourceCommand _declared = null!;
    private ResourceCommand _transient = null!;
    private IResourceCommandHandler _handler = null!;
    private Func<string, CancellationToken, Task<ReadOnlyMemory<byte>?>> _read = null!;
    private Func<ResourceCommand, string, ReadOnlyMemory<byte>, CancellationToken, Task> _store = null!;
    private Func<ResourceCommand, CancellationToken, Task> _delete = null!;

    [Params(32, 4096)]
    public int ValueBytes { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        _directory = Directory.CreateTempSubdirectory("cohesion-secret-benchmarks-");
        _value = new byte[ValueBytes];
        for (int i = 0; i < _value.Length; i++) { _value[i] = (byte)(i * 31); }
        _protector = DataProtectionProvider.Create(
                FileSystemKeyRepository.Create(Path.Combine(_directory.FullName, "key-ring")),
                options => options.ApplicationDiscriminator = "SecretStore:benchmarks:secrets")
            .CreateProtector("secret-store", "secrets", "v1");
        _protected = _protector.Protect(_value);
        if (!_protector.Unprotect(_protected).AsSpan().SequenceEqual(_value))
        {
            throw new InvalidOperationException("Protection did not round-trip.");
        }

        Assembly assembly = typeof(SecretStoreApplication).Assembly;
        Type type = assembly.GetType("Assimalign.Cohesion.SecretStore.Hosting.Internal.SecretStoreRepository", true)!;
        _repository = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.NonPublic, null,
            new object[] { _directory.FullName, _protector,
                new Dictionary<string, ReadOnlyMemory<byte>> { ["seed/key"] = _value } }, null)!;
        _read = Bind<Func<string, CancellationToken, Task<ReadOnlyMemory<byte>?>>>("ReadAsync");
        _store = Bind<Func<ResourceCommand, string, ReadOnlyMemory<byte>, CancellationToken, Task>>("StoreCommandSecretAsync");
        _delete = Bind<Func<ResourceCommand, CancellationToken, Task>>("DeleteCommandSecretAsync");
        await Bind<Func<CancellationToken, Task>>("InitializeAsync")(default);
        _declared = Command("declared/key");
        _transient = Command("transient/key");
        await _store(_declared, "parameter:credential", _value, default);

        Type handlerType = assembly.GetType("Assimalign.Cohesion.SecretStore.Hosting.Internal.SecretStoreResourceCommandHandler", true)!;
        // The certificate authority collaborator is unused by the add-secret handler.
        _handler = (IResourceCommandHandler)Activator.CreateInstance(handlerType,
            new object?[] { "secretstore.add-secret", _repository, null })!;
        if ((await _read("seed/key", default)) is not { } seed || !seed.Span.SequenceEqual(_value)
            || (await _read("declared/key", default)) is not { } declared || !declared.Span.SequenceEqual(_value)
            || await _read("missing/key", default) is not null)
        {
            throw new InvalidOperationException("Repository fixtures did not round-trip.");
        }
        await _handler.ExecuteAsync(_transient);
        await _handler.DeleteAsync(_transient);
        if (await _read(_transient.Key, default) is not null)
        {
            throw new InvalidOperationException("Command deletion did not remove the secret.");
        }
    }

    private ResourceCommand Command(string key) => new("benchmark-" + key, "secretstore.add-secret", "benchmarks", key,
        JsonSerializer.SerializeToUtf8Bytes(new { path = key, source = "parameter:credential", resolvedValue = Convert.ToBase64String(_value) }));

    private T Bind<T>(string name) where T : Delegate =>
        _repository.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)!.CreateDelegate<T>(_repository);

    [Benchmark] public Task<ReadOnlyMemory<byte>?> ReadSeededSecret() => _read("seed/key", default);
    [Benchmark] public Task<ReadOnlyMemory<byte>?> ReadDeclaredSecret() => _read(_declared.Key, default);
    [Benchmark] public Task<ReadOnlyMemory<byte>?> ReadMissingSecret() => _read("missing/key", default);
    [Benchmark] public Task ReplayDeclaration() => _store(_declared, "parameter:credential", _value, default);

    [Benchmark]
    public async Task<int> StoreAndDeleteSecret()
    {
        await _store(_transient, "parameter:credential", _value, default);
        await _delete(_transient, default);
        return _value.Length;
    }

    [Benchmark] public ValueTask<ReadOnlyMemory<byte>> ReplayAddSecretCommand() => _handler.ExecuteAsync(_declared);

    [Benchmark]
    public async Task<int> ExecuteAndDeleteSecretCommand()
    {
        await _handler.ExecuteAsync(_transient);
        await _handler.DeleteAsync(_transient);
        return _value.Length;
    }

    [Benchmark] public byte[] ProtectSecretValue() => _protector.Protect(_value);
    [Benchmark] public byte[] UnprotectSecretValue() => _protector.Unprotect(_protected);

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
