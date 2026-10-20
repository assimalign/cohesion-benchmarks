using System;
using System.Linq;

using AM = AutoMapper;
using static Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping.MapperBenchmarks;

namespace Assimalign.Cohesion.ObjectMapping.Benchmarks.ObjectMapping;

internal static class MappingFixtures
{
    internal static IMapper CreateCohesion(bool graph = true)
    {
        var builder = new MapperBuilder()
            .AddProfile<FlatTarget, FlatSource>(profile => profile
                .MapMember(static source => source.Str, static (target, value) => target.Str = value)
                .MapMember(static source => source.Num1, static (target, value) => target.Num1 = value)
                .MapMember(static source => source.Num2, static (target, value) => target.Num2 = value)
                .MapMember(static source => source.Num3, static (target, value) => target.Num3 = value));
        if (graph)
        {
            builder
                .AddProfile<OrderTarget, OrderSource>(profile => profile
                    .MapMember(static source => source.Id, static (target, value) => target.Id = value)
                    .MapMember(static source => source.PlacedOn, static (target, value) => target.PlacedOn = value)
                    .MapMemberTypes(static source => source.Customer, static target => target.Customer,
                        static (target, value) => target.Customer = value!)
                    .MapMemberEnumerables(static source => source.Items, static target => target.Items,
                        static (target, items) => target.Items = items.ToList()))
                .AddProfile<CustomerTarget, CustomerSource>(profile => profile
                    .MapMember(static source => source.Name, static (target, value) => target.Name = value)
                    .MapMember(static source => source.Email, static (target, value) => target.Email = value))
                .AddProfile<LineItemTarget, LineItemSource>(profile => profile
                    .MapMember(static source => source.Sku, static (target, value) => target.Sku = value)
                    .MapMember(static source => source.Quantity, static (target, value) => target.Quantity = value)
                    .MapMember(static source => source.Price, static (target, value) => target.Price = value));
        }
        return builder.Build();
    }

    internal static AM.MapperConfiguration CreateAutoMapper(bool graph = true) => new(configuration =>
    {
        configuration.CreateMap<FlatSource, FlatTarget>();
        if (graph)
        {
            configuration.CreateMap<OrderSource, OrderTarget>();
            configuration.CreateMap<CustomerSource, CustomerTarget>();
            configuration.CreateMap<LineItemSource, LineItemTarget>();
        }
    });

    internal static FlatSource Flat(int index = 1) => new()
    {
        Str = $"value-{index}", Num1 = 7, Num2 = index, Num3 = index * 100L,
    };

    internal static OrderSource Order(int count) => new()
    {
        Id = 42,
        PlacedOn = new DateTime(2026, 10, 18, 0, 0, 0, DateTimeKind.Utc),
        Customer = new CustomerSource { Name = "Ada", Email = "ada@example.com" },
        Items = [.. Enumerable.Range(1, count).Select(i => new LineItemSource
        {
            Sku = $"SKU-{i:000}", Quantity = i, Price = i * 1.25m,
        })],
    };

    internal static void Verify(FlatTarget actual, FlatSource source)
    {
        if (actual.Str != source.Str || actual.Num1 != source.Num1
            || actual.Num2 != source.Num2 || actual.Num3 != source.Num3)
        {
            throw new InvalidOperationException("Flat mapping differs from source.");
        }
    }

    internal static void Verify(OrderTarget actual, OrderSource source)
    {
        if (actual.Id != source.Id || actual.PlacedOn != source.PlacedOn
            || actual.Customer.Name != source.Customer.Name || actual.Customer.Email != source.Customer.Email
            || actual.Items.Count != source.Items.Count)
        {
            throw new InvalidOperationException("Nested mapping differs from source.");
        }
        for (int i = 0; i < source.Items.Count; i++)
        {
            if (actual.Items[i].Sku != source.Items[i].Sku || actual.Items[i].Quantity != source.Items[i].Quantity
                || actual.Items[i].Price != source.Items[i].Price)
            {
                throw new InvalidOperationException("Nested item mapping differs from source.");
            }
        }
    }
}
