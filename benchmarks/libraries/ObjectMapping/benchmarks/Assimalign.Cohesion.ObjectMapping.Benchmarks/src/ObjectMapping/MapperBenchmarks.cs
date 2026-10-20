using System;
using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using AM = AutoMapper;

namespace Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping;

/// <summary>
/// Mapping a new target from a source, against AutoMapper (the baseline in each category): a flat object of four
/// nullable scalars, and an order with a nested customer and ten line items.
/// </summary>
/// <remarks>
/// <para>
/// <c>Flat</c> is the port of the legacy <c>SimpleObjectToObjectMapping</c> benchmark. Its TinyMapper comparison is
/// dropped: the package is unmaintained and is not one of this repository's counterparts.
/// </para>
/// <para>
/// Cohesion profiles are written with the delegate overloads (<c>MapMember(getter, setter)</c> and friends), which is
/// what the ObjectMapping source generator emits for a class-based or intercepted inline profile, so the measured
/// path is the AOT-safe one a consumer gets with the generator. Both mappers are built once in setup.
/// </para>
/// </remarks>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
// Opted into NativeAOT runs: this path is reflection-free by design, so losing the JIT is a real question for it.
[BenchmarkCategory(global::Assimalign.Cohesion.Benchmarks.BenchmarkRuntimes.Category)]
public class MapperBenchmarks
{
    private IMapper _cohesion = null!;
    private AM.IMapper _autoMapper = null!;
    private FlatSource _flat = null!;
    private OrderSource _order = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new MapperBuilder()
            .AddProfile<FlatTarget, FlatSource>(profile => profile
                .MapMember(static source => source.Str, static (target, value) => target.Str = value)
                .MapMember(static source => source.Num1, static (target, value) => target.Num1 = value)
                .MapMember(static source => source.Num2, static (target, value) => target.Num2 = value)
                .MapMember(static source => source.Num3, static (target, value) => target.Num3 = value))
            .AddProfile<OrderTarget, OrderSource>(profile => profile
                .MapMember(static source => source.Id, static (target, value) => target.Id = value)
                .MapMember(static source => source.PlacedOn, static (target, value) => target.PlacedOn = value)
                .MapMemberTypes(
                    static source => source.Customer,
                    static target => target.Customer,
                    static (target, value) => target.Customer = value!)
                .MapMemberEnumerables(
                    static source => source.Items,
                    static target => target.Items,
                    static (target, items) => target.Items = items.ToList()))
            .AddProfile<CustomerTarget, CustomerSource>(profile => profile
                .MapMember(static source => source.Name, static (target, value) => target.Name = value)
                .MapMember(static source => source.Email, static (target, value) => target.Email = value))
            .AddProfile<LineItemTarget, LineItemSource>(profile => profile
                .MapMember(static source => source.Sku, static (target, value) => target.Sku = value)
                .MapMember(static source => source.Quantity, static (target, value) => target.Quantity = value)
                .MapMember(static source => source.Price, static (target, value) => target.Price = value))
            .Build();

        _autoMapper = new AM.MapperConfiguration(configuration =>
        {
            configuration.CreateMap<FlatSource, FlatTarget>();
            configuration.CreateMap<OrderSource, OrderTarget>();
            configuration.CreateMap<CustomerSource, CustomerTarget>();
            configuration.CreateMap<LineItemSource, LineItemTarget>();
        }).CreateMapper();

        _flat = new FlatSource { Str = "TestString1", Num1 = 1, Num2 = 2, Num3 = 3 };
        _order = new OrderSource
        {
            Id = 42,
            PlacedOn = new DateTime(2026, 10, 18, 0, 0, 0, DateTimeKind.Utc),
            Customer = new CustomerSource { Name = "Ada Lovelace", Email = "ada@example.com" },
            Items = [.. Enumerable.Range(1, 10).Select(i => new LineItemSource { Sku = $"SKU-{i:000}", Quantity = i, Price = i * 1.25m })],
        };

        OrderTarget cohesion = MapNested();
        OrderTarget autoMapper = MapNested_AutoMapper();
        if (cohesion.Items.Count != 10 || cohesion.Items[9].Sku != autoMapper.Items[9].Sku
            || cohesion.Customer.Email != autoMapper.Customer.Email || MapFlat().Num3 != MapFlat_AutoMapper().Num3)
        {
            throw new InvalidOperationException("The two mappers produced different targets.");
        }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Flat")]
    public FlatTarget MapFlat_AutoMapper() => _autoMapper.Map<FlatTarget>(_flat);

    [Benchmark]
    [BenchmarkCategory("Flat")]
    public FlatTarget MapFlat() => _cohesion.Map<FlatTarget, FlatSource>(_flat);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Nested")]
    public OrderTarget MapNested_AutoMapper() => _autoMapper.Map<OrderTarget>(_order);

    [Benchmark]
    [BenchmarkCategory("Nested")]
    public OrderTarget MapNested() => _cohesion.Map<OrderTarget, OrderSource>(_order);

    public sealed class FlatSource
    {
        public string? Str { get; set; }

        public short? Num1 { get; set; }

        public int? Num2 { get; set; }

        public long? Num3 { get; set; }
    }

    public sealed class FlatTarget
    {
        public string? Str { get; set; }

        public short? Num1 { get; set; }

        public int? Num2 { get; set; }

        public long? Num3 { get; set; }
    }

    public sealed class OrderSource
    {
        public int Id { get; set; }

        public DateTime PlacedOn { get; set; }

        public CustomerSource Customer { get; set; } = new();

        public List<LineItemSource> Items { get; set; } = [];
    }

    public sealed class OrderTarget
    {
        public int Id { get; set; }

        public DateTime PlacedOn { get; set; }

        public CustomerTarget Customer { get; set; } = new();

        public List<LineItemTarget> Items { get; set; } = [];
    }

    public sealed class CustomerSource
    {
        public string Name { get; set; } = "";

        public string Email { get; set; } = "";
    }

    public sealed class CustomerTarget
    {
        public string Name { get; set; } = "";

        public string Email { get; set; } = "";
    }

    public sealed class LineItemSource
    {
        public string Sku { get; set; } = "";

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }

    public sealed class LineItemTarget
    {
        public string Sku { get; set; } = "";

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
