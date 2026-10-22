using System;

using BenchmarkDotNet.Attributes;

using CohesionConfiguration = Assimalign.Cohesion.Configuration;

namespace Assimalign.Cohesion.Configuration.Benchmarks.Configuration;

/// <summary>
/// Valid key construction and nullable value equality. Configuration providers construct one key per path
/// segment; these cases separate validation and string ownership from path parsing and provider lookup.
/// </summary>
public class KeyBenchmarks
{
    private readonly string _value = "RequestTimeout";
    private readonly CohesionConfiguration.Key? _left = new CohesionConfiguration.Key("RequestTimeout");
    private readonly CohesionConfiguration.Key? _right = new CohesionConfiguration.Key("RequestTimeout");

    [Benchmark]
    public CohesionConfiguration.Key FromString() => new(_value);

    [Benchmark]
    public CohesionConfiguration.Key FromSpan() => new(_value.AsSpan());

    [Benchmark]
    public bool EqualNullable() => _left == _right;
}
