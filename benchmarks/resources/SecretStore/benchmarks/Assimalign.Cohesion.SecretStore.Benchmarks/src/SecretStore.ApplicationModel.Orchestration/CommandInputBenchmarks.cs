using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.ApplicationModel;

namespace Assimalign.Cohesion.SecretStore.Benchmarks;

/// <summary>
/// Public gateway input resolver: parses and validates a source declaration, resolves an already
/// available input, then emits the delivered JSON/base64 payload. The source resolver is a fixed
/// in-process fixture so no parameter binding, network or remote store latency is included.
/// Standalone: a JSON encode alone omits the declaration validation and resolution contract.
/// </summary>
public class CommandInputBenchmarks
{
    private readonly SecretStoreAddSecretInputResolver _resolver = new();
    private IResourceSourceResolver _sources = null!;
    private ResourceCommandInput _parameter = null!;
    private ResourceCommandInput _store = null!;

    [Params(32, 4096)]
    public int ValueBytes { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        byte[] value = new byte[ValueBytes];
        for (int i = 0; i < value.Length; i++) { value[i] = (byte)(i * 31); }
        _sources = new FixedSources(ResourceMountInput.Resolved("benchmark", value));
        _parameter = new ResourceCommandInput("parameter-command", "secretstore.add-secret", "benchmarks", "orders/key",
            """{"path":"orders/key","source":"parameter:credential"}"""u8.ToArray());
        _store = new ResourceCommandInput("store-command", "secretstore.add-secret", "benchmarks", "orders/key",
            """{"path":"orders/key","source":"upstream:orders/key"}"""u8.ToArray());
        foreach (ResourceCommandInput input in new[] { _parameter, _store })
        {
            using JsonDocument document = JsonDocument.Parse(await _resolver.ResolveAsync(input, _sources));
            if (!document.RootElement.GetProperty("resolvedValue").GetBytesFromBase64().AsSpan().SequenceEqual(value)
                || document.RootElement.GetProperty("path").GetString() != input.Key)
            {
                throw new InvalidOperationException("Resolved delivery payload changed.");
            }
        }
    }

    [Benchmark] public ValueTask<ReadOnlyMemory<byte>> ResolveParameterSource() => _resolver.ResolveAsync(_parameter, _sources);
    [Benchmark] public ValueTask<ReadOnlyMemory<byte>> ResolveStoreSource() => _resolver.ResolveAsync(_store, _sources);

    private sealed class FixedSources(ResourceMountInput input) : IResourceSourceResolver
    {
        public ValueTask<ResourceMountInput> ResolveAsync(string source, ResourceMountKind kind, CancellationToken cancellationToken = default) =>
            ValueTask.FromResult(input);
    }
}
