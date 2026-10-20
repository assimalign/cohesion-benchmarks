using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using AM = AutoMapper;
using static Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping.MapperBenchmarks;

namespace Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping;

/// <summary>
/// Map into a reused flat target, and create a new target through runtime Type dispatch. Both sides overwrite
/// the same four members with the same null handling. Warmup and target/input allocation happen in setup.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class MappingDispatchBenchmarks
{
    private IMapper _cohesion = null!;
    private AM.IMapper _autoMapper = null!;
    private FlatSource _source = null!;
    private FlatTarget _cohesionTarget = null!;
    private FlatTarget _autoTarget = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = MappingFixtures.CreateCohesion(graph: false);
        _autoMapper = MappingFixtures.CreateAutoMapper(graph: false).CreateMapper();
        _source = MappingFixtures.Flat();
        _source.Str = null;
        _source.Num2 = null;
        _cohesionTarget = new FlatTarget { Str = "old", Num2 = 42 };
        _autoTarget = new FlatTarget { Str = "old", Num2 = 42 };
        MappingFixtures.Verify(MapExisting(), _source);
        MappingFixtures.Verify(MapExisting_AutoMapper(), _source);
        MappingFixtures.Verify((FlatTarget)MapRuntime(), _source);
        MappingFixtures.Verify((FlatTarget)MapRuntime_AutoMapper(), _source);
    }

    [Benchmark, BenchmarkCategory("Existing")]
    public FlatTarget MapExisting() => _cohesion.Map<FlatTarget, FlatSource>(_cohesionTarget, _source);

    [Benchmark(Baseline = true), BenchmarkCategory("Existing")]
    public FlatTarget MapExisting_AutoMapper() => _autoMapper.Map(_source, _autoTarget);

    [Benchmark, BenchmarkCategory("Runtime")]
    public object MapRuntime() => _cohesion.Map(_source, typeof(FlatTarget), typeof(FlatSource));

    [Benchmark(Baseline = true), BenchmarkCategory("Runtime")]
    public object MapRuntime_AutoMapper() => _autoMapper.Map(_source, typeof(FlatSource), typeof(FlatTarget));
}
