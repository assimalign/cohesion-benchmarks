using System;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Connections.Benchmarks.Connections;

/// <summary>
/// Named lookup of an absent slot through built-in layers. Unlike a tombstone hit, this visits every
/// layer. A dictionary does not provide the inherited feature visibility contract measured here.
/// </summary>
public class ConnectionNamedMissBenchmarks
{
    private IConnectionFeatureCollection _features = null!;

    [Params(0, 1, 4, 16)]
    public int Layers { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _features = new ConnectionFeatureCollection();
        _features.Set(new NamedFeature("Root"));
        for (int index = 0; index < Layers; index++)
        {
            var wrapper = new ConnectionFeatureCollection(_features);
            wrapper.Set(new NamedFeature("Layer" + index));
            _features = wrapper;
        }
        if (NamedAbsent() != 0)
        {
            throw new InvalidOperationException("The absent feature became visible.");
        }
    }

    [Benchmark(OperationsPerInvoke = 32)]
    public int NamedAbsent()
    {
        int found = 0;
        for (int index = 0; index < 32; index++)
        {
            if (_features.Get("Absent") is not null) found++;
        }
        return found;
    }

    private sealed record NamedFeature(string Name) : IConnectionFeature;
}
