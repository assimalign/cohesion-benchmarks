using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.SecretStore.Hosting;
using Assimalign.Cohesion.Security.DataProtection;

namespace Assimalign.Cohesion.SecretStore.Benchmarks;

/// <summary>
/// The shipped repository is internal and file-backed, with no public in-process engine facade.
/// Bind typed delegates once in setup to measure the engine itself without starting the HTTP host.
/// Reads include file I/O and authenticated decryption of the seeded, protected secret file.
/// Standalone: a dictionary or raw BCL AES lacks these guarantees.
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
    private Func<string, CancellationToken, Task<ReadOnlyMemory<byte>?>> _read = null!;

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
        await Bind<Func<CancellationToken, Task>>("InitializeAsync")(default);
        if ((await _read("seed/key", default)) is not { } seed || !seed.Span.SequenceEqual(_value)
            || await _read("missing/key", default) is not null)
        {
            throw new InvalidOperationException("Repository fixtures did not round-trip.");
        }
    }

    private T Bind<T>(string name) where T : Delegate =>
        _repository.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)!.CreateDelegate<T>(_repository);

    [Benchmark] public Task<ReadOnlyMemory<byte>?> ReadSeededSecret() => _read("seed/key", default);
    [Benchmark] public Task<ReadOnlyMemory<byte>?> ReadMissingSecret() => _read("missing/key", default);

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
