using System;
using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;

using AM = AutoMapper;
using static Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping.MapperBenchmarks;

namespace Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping;

/// <summary>
/// Map a materialized source array to a new List of new flat targets. Cohesion's public sequence overload
/// eagerly returns a List as IEnumerable; AutoMapper is explicitly asked for a List, so both do all work here.
/// Configuration and first-use compilation are outside measurement.
/// </summary>
public class CollectionMappingBenchmarks
{
    private IMapper _cohesion = null!;
    private AM.IMapper _autoMapper = null!;
    private FlatSource[] _source = [];

    [Params(1, 100)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = MappingFixtures.CreateCohesion(graph: false);
        _autoMapper = MappingFixtures.CreateAutoMapper(graph: false).CreateMapper();
        _source = [.. Enumerable.Range(1, Count).Select(MappingFixtures.Flat)];
        foreach (IEnumerable<FlatTarget> result in new[] { MapCollection(), MapCollection_AutoMapper() })
        {
            FlatTarget[] items = result.ToArray();
            if (items.Length != Count) throw new InvalidOperationException("Collection count differs.");
            for (int i = 0; i < Count; i++) MappingFixtures.Verify(items[i], _source[i]);
        }
    }

    [Benchmark]
    public List<FlatTarget> MapCollection() => (List<FlatTarget>)_cohesion.Map<FlatTarget, FlatSource>((IEnumerable<FlatSource>)_source);

    [Benchmark(Baseline = true)]
    public List<FlatTarget> MapCollection_AutoMapper() => _autoMapper.Map<List<FlatTarget>>(_source);
}
