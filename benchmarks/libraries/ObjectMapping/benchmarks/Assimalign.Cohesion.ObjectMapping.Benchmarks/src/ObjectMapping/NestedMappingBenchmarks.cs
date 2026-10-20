using BenchmarkDotNet.Attributes;

using AM = AutoMapper;
using static Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping.MapperBenchmarks;

namespace Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping;

/// <summary>
/// Warm mapping of an order/customer graph with one or one hundred line items. Both create a new graph and
/// a List of new line-item targets; delegate profiles match the existing AOT-safe benchmark configuration.
/// </summary>
public class NestedMappingBenchmarks
{
    private IMapper _cohesion = null!;
    private AM.IMapper _autoMapper = null!;
    private OrderSource _source = null!;

    [Params(1, 100)]
    public int Items { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = MappingFixtures.CreateCohesion();
        _autoMapper = MappingFixtures.CreateAutoMapper().CreateMapper();
        _source = MappingFixtures.Order(Items);
        MappingFixtures.Verify(MapGraph(), _source);
        MappingFixtures.Verify(MapGraph_AutoMapper(), _source);
    }

    [Benchmark]
    public OrderTarget MapGraph() => _cohesion.Map<OrderTarget, OrderSource>(_source);

    [Benchmark(Baseline = true)]
    public OrderTarget MapGraph_AutoMapper() => _autoMapper.Map<OrderTarget>(_source);
}
