using System.Text.Json;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModel;

/// <summary>
/// The gateway's per-resource manifest work: reading a <c>resource.json</c> (deserialize and validate against the
/// manifest contract) and computing its canonical content hash, which drives change detection on every deploy.
/// Both scale with the number of resources an application set describes.
/// </summary>
public class ResourceManifestBenchmarks
{
    private string _json = null!;
    private ResourceManifest _manifest = null!;

    [GlobalSetup]
    public void Setup()
    {
        _manifest = SampleManifests.CreateWebApi().Validate();
        _json = JsonSerializer.Serialize(_manifest, ResourceManifestJsonContext.Default.ResourceManifest);
    }

    [Benchmark]
    public ResourceManifest Parse() => ResourceManifest.Parse(_json);

    [Benchmark]
    public string ComputeHash() => ResourceManifestCanonicalizer.ComputeHash(_manifest);
}
