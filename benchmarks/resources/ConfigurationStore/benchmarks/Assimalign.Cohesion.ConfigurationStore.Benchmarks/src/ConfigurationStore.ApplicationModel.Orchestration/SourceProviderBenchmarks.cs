using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.ApplicationModel;

namespace Assimalign.Cohesion.ConfigurationStore.Benchmarks;

/// <summary>
/// Public mount-source resolution, including connection validation, endpoint normalization,
/// per-operation client/transport lifetime and immutable namespace decoding. Setup uses the
/// internal transport-factory constructor solely to substitute an in-process handler: the public
/// constructor always creates a socket transport. No reflection occurs in the measured path.
/// Standalone because a JSON decode does not provide the gateway source-provider contract.
/// </summary>
public class SourceProviderBenchmarks
{
    private ConfigurationStoreSourceProvider _provider = null!;
    private ResourceSourceRequest _request = null!;

    [Params(8, 128)]
    public int EntryCount { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        var values = new Dictionary<string, string?>(StringComparer.Ordinal);
        for (int i = 0; i < EntryCount; i++)
        {
            values.Add($"Key{i:D4}", i % 4 == 0 ? null : $"value-{i:D4}-\u03bb");
        }
        byte[] document = JsonSerializer.SerializeToUtf8Bytes(values);
        Func<RemoteCertificateValidationCallback?, HttpMessageInvoker> factory = _ =>
            new HttpMessageInvoker(new NamespaceHandler(document));
        _provider = (ConfigurationStoreSourceProvider)Activator.CreateInstance(typeof(ConfigurationStoreSourceProvider),
            BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { factory }, null)!;
        _request = new ResourceSourceRequest("benchmarks", "consumer", "settings", ResourceMountKind.Configuration, "settings",
            new ResourceProviderConnection("benchmarks", "configuration", "ConfigurationStore",
                new Uri("https://store.invalid/custom/control"), "benchmark-opaque-token", null));
        IReadOnlyDictionary<string, string?> result = await _provider.ReadConfigurationAsync(_request);
        if (result.Count != EntryCount || result["Key0000"] is not null || result["Key0001"] != values["Key0001"])
        {
            throw new InvalidOperationException("Provider fixture did not round-trip.");
        }
    }

    [Benchmark]
    public ValueTask<IReadOnlyDictionary<string, string?>> ReadConfigurationMount() => _provider.ReadConfigurationAsync(_request);

    private sealed class NamespaceHandler(byte[] document) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(document) };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Task.FromResult(response);
        }
    }
}
