using System;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Connections.Benchmarks.Connections;

/// <summary>
/// Named and typed optional-feature resolution through shallow/deep wrapper collections, including
/// local shadowing and tombstones. The matching feature is at the innermost level. There is no
/// dictionary baseline: a dictionary does not implement inherited, name-shadowed typed resolution.
/// The pinned connection contract exposes Features rather than a separate connection-items bag.
/// </summary>
public class ConnectionFeatureLookupBenchmarks
{
    private IConnectionFeatureCollection _features = null!;
    private string _targetName = null!;
    private string _missingName = null!;

    [Params(1, 4)]
    public int Layers { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _targetName = "Protocol";
        _missingName = "Hidden";
        var root = new ConnectionFeatureCollection();
        root.Set(new SelectedFeature(_targetName));
        root.Set(new MissingFeature(_missingName));
        root.Set(new NamedFeature("Transport"));
        _features = root;
        for (int layer = 0; layer < Layers; layer++)
        {
            var wrapper = new ConnectionFeatureCollection(_features);
            wrapper.Set(new NamedFeature("Transport"));
            wrapper.Set(new NamedFeature("Layer" + layer));
            wrapper.Remove(_missingName);
            _features = wrapper;
        }

        if (NamedHit() != 32 || TypedHit() != 32 || NamedMiss() != 0 || TypedMiss() != 0)
            throw new InvalidOperationException("Feature visibility differs from the fixture.");
    }

    [Benchmark(OperationsPerInvoke = 32)]
    public int NamedHit()
    {
        int found = 0;
        for (int i = 0; i < 32; i++) if (_features.Get(_targetName) is not null) found++;
        return found;
    }

    [Benchmark(OperationsPerInvoke = 32)]
    public int TypedHit()
    {
        int found = 0;
        for (int i = 0; i < 32; i++) if (_features.Get<SelectedFeature>() is not null) found++;
        return found;
    }

    [Benchmark(OperationsPerInvoke = 32)]
    public int NamedMiss()
    {
        int found = 0;
        for (int i = 0; i < 32; i++) if (_features.Get(_missingName) is not null) found++;
        return found;
    }

    [Benchmark(OperationsPerInvoke = 32)]
    public int TypedMiss()
    {
        int found = 0;
        for (int i = 0; i < 32; i++) if (_features.Get<MissingFeature>() is not null) found++;
        return found;
    }

    private sealed record SelectedFeature(string Name) : IConnectionFeature;
    private sealed record MissingFeature(string Name) : IConnectionFeature;
    private sealed record NamedFeature(string Name) : IConnectionFeature;
}
