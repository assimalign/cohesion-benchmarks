using System;
using System.Collections.Generic;

using BenchmarkDotNet.Attributes;

using AM = AutoMapper;
using static Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping.MapperBenchmarks;

namespace Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping;

/// <summary>
/// Existing-target mappings through custom extension points. Each copies the same four members as
/// AutoMapper. These controls keep built-in dispatch changes honest about custom-profile and callback cost.
/// Context callbacks may retain the context; the mapper cannot reuse one across invocations.
/// </summary>
public class MappingCallbackBenchmarks
{
    private IMapper _cohesion = null!;
    private AM.IMapper _autoMapper = null!;
    private FlatSource _source = null!;
    private FlatTarget _target = null!;
    private FlatTarget _autoTarget = null!;

    [Params("TypedCallback", "ContextCallback", "CustomProfile", "OverriddenProfile")]
    public string Mode { get; set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        var builder = new MapperBuilder();
        switch (Mode)
        {
            case "TypedCallback":
                builder.AddProfile<FlatTarget, FlatSource>(d => d.MapAction(Copy));
                break;
            case "ContextCallback":
                builder.AddProfile<FlatTarget, FlatSource>(d => d.MapAction(CopyContext));
                break;
            case "CustomProfile":
                builder.AddProfile(new CustomProfile());
                break;
            case "OverriddenProfile":
                builder.AddProfile(new OverriddenProfile());
                break;
        }

        _cohesion = builder.Build();
        _autoMapper = MappingFixtures.CreateAutoMapper(graph: false).CreateMapper();
        _source = MappingFixtures.Flat();
        _target = new FlatTarget();
        _autoTarget = new FlatTarget();
        MappingFixtures.Verify(MapCallback(), _source);
        MappingFixtures.Verify(MapCallback_AutoMapper(), _source);
    }

    [Benchmark]
    public FlatTarget MapCallback() => _cohesion.Map<FlatTarget, FlatSource>(_target, _source);

    [Benchmark(Baseline = true)]
    public FlatTarget MapCallback_AutoMapper() => _autoMapper.Map(_source, _autoTarget);

    private static void Copy(FlatTarget target, FlatSource source)
    {
        target.Str = source.Str;
        target.Num1 = source.Num1;
        target.Num2 = source.Num2;
        target.Num3 = source.Num3;
    }

    private static void CopyContext(IMapperContext context) => Copy((FlatTarget)context.Target, (FlatSource)context.Source);

    private sealed class Callback : IMapperAction
    {
        public void Invoke(IMapperContext context) => CopyContext(context);
    }

    private sealed class CustomProfile : IMapperProfile
    {
        public Type TargetType => typeof(FlatTarget);
        public Type SourceType => typeof(FlatSource);
        public IReadOnlyList<IMapperAction> MapActions { get; } = new IMapperAction[] { new Callback() };
    }

    private sealed class OverriddenProfile : MapperProfile<FlatTarget, FlatSource>, IMapperProfile
    {
        private readonly IMapperAction[] _actions = new IMapperAction[] { new Callback() };
        IReadOnlyList<IMapperAction> IMapperProfile.MapActions => _actions;
        protected override void Configure(MapperProfileDescriptor<FlatTarget, FlatSource> descriptor) { }
    }
}
