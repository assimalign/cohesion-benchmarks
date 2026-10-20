using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using AM = AutoMapper;
using static Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping.MapperBenchmarks;

namespace Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping;

/// <summary>
/// Configuration with one flat profile or four graph profiles. Configure only registers/builds definitions;
/// ConfigureAndMap additionally executes each registered mapping, including AutoMapper's first-use compilation.
/// Cohesion's delegate profiles need no runtime expression compilation. These are separate from warm per-map
/// cost and compare the lifecycle boundary of registering equivalent maps and making their first calls.
/// Input objects are prepared in setup; configuring the mapper is intentionally measured here.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class MappingConfigurationBenchmarks
{
    private FlatSource _flat = null!;
    private OrderSource _order = null!;

    [Params(false, true)]
    public bool Graph { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _flat = MappingFixtures.Flat();
        _order = MappingFixtures.Order(10);
        AM.MapperConfiguration configuration = MappingFixtures.CreateAutoMapper(Graph);
        configuration.AssertConfigurationIsValid();
        Configure();
        Configure_AutoMapper();
        ConfigureAndMap();
        ConfigureAndMap_AutoMapper();
    }

    [Benchmark, BenchmarkCategory("Configure")]
    public IMapper Configure() => MappingFixtures.CreateCohesion(Graph);

    [Benchmark(Baseline = true), BenchmarkCategory("Configure")]
    public AM.IMapper Configure_AutoMapper() => MappingFixtures.CreateAutoMapper(Graph).CreateMapper();

    [Benchmark, BenchmarkCategory("FirstMap")]
    public object ConfigureAndMap()
    {
        IMapper mapper = MappingFixtures.CreateCohesion(Graph);
        FlatTarget flat = mapper.Map<FlatTarget, FlatSource>(_flat);
        if (!Graph) return flat;
        mapper.Map<CustomerTarget, CustomerSource>(_order.Customer);
        mapper.Map<LineItemTarget, LineItemSource>(_order.Items[0]);
        return mapper.Map<OrderTarget, OrderSource>(_order);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("FirstMap")]
    public object ConfigureAndMap_AutoMapper()
    {
        AM.IMapper mapper = MappingFixtures.CreateAutoMapper(Graph).CreateMapper();
        FlatTarget flat = mapper.Map<FlatTarget>(_flat);
        if (!Graph) return flat;
        mapper.Map<CustomerTarget>(_order.Customer);
        mapper.Map<LineItemTarget>(_order.Items[0]);
        return mapper.Map<OrderTarget>(_order);
    }
}
