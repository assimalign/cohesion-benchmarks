using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using FluentValidation;
using FV = FluentValidation;

namespace Assimalign.Cohesion.ObjectValidation.Benchmarks.ObjectValidation;

/// <summary>
/// Reusable nested profiles and a collection of eight child objects. Both libraries evaluate every
/// rule in each member chain and continue across members/items; setup checks error counts.
/// An invalid address fails City and Country, including indexed error construction in collections.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class NestedValidationBenchmarks
{
    private IValidator _cohesion = null!;
    private OrderValidator _fluent = null!;
    private Order _single = null!;
    private Order _collection = null!;

    [Params(true, false)] public bool Valid { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = CreateCohesion(true);
        _fluent = new OrderValidator(true);
        _single = new Order { Shipping = Address.Create(Valid) };
        _collection = new Order { Stops = Enumerable.Range(0, 8).Select(_ => Address.Create(Valid)).ToArray() };
        Check(ValidateNested().Errors.Count(), ValidateNested_FluentValidation().Errors.Count, Valid ? 0 : 2);
        Check(ValidateCollection().Errors.Count(), ValidateCollection_FluentValidation().Errors.Count, Valid ? 0 : 16);
    }

    internal static void Check(int cohesion, int fluent, int expected)
    {
        if (cohesion != expected || fluent != expected)
            throw new InvalidOperationException($"Expected {expected} errors, got Cohesion={cohesion}, FluentValidation={fluent}.");
    }

    internal static IValidator CreateCohesion(bool collection) => Validator.Create(builder =>
    {
        builder.AddOptions(options => options.ContinueThroughValidationChain = true);
        builder.AddProfile(profiles => profiles.CreateProfile<Order>(rules =>
        {
            rules.RuleFor(order => order.Shipping).UseProfile(new AddressProfile());
            if (collection) rules.RuleForEach(order => order.Stops).UseProfile(new AddressProfile());
        }));
    });

    [Benchmark, BenchmarkCategory("Nested")]
    public ValidationResult ValidateNested() => _cohesion.Validate(_single);
    [Benchmark(Baseline = true), BenchmarkCategory("Nested")]
    public FV.Results.ValidationResult ValidateNested_FluentValidation() => _fluent.Validate(_single);
    [Benchmark, BenchmarkCategory("Collection")]
    public ValidationResult ValidateCollection() => _cohesion.Validate(_collection);
    [Benchmark(Baseline = true), BenchmarkCategory("Collection")]
    public FV.Results.ValidationResult ValidateCollection_FluentValidation() => _fluent.Validate(_collection);

    public sealed class Address
    {
        public string City { get; set; } = "London";
        public string Country { get; set; } = "GB";
        public static Address Create(bool valid) => valid ? new Address() : new Address { City = "", Country = "GBR" };
    }

    public sealed class Order
    {
        public Address Shipping { get; set; } = new();
        public Address[] Stops { get; set; } = [];
    }

    private sealed class AddressProfile : ValidationProfile<Address>
    {
        public override void Configure(IValidationRuleDescriptor<Address> rules)
        {
            rules.RuleFor(address => address.City).NotEmpty().MaxLength(100);
            rules.RuleFor(address => address.Country).Length(2);
        }
    }

    public sealed class AddressValidator : FV.AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleLevelCascadeMode = FV.CascadeMode.Continue;
            RuleFor(address => address.City).NotEmpty().MaximumLength(100);
            RuleFor(address => address.Country).Length(2);
        }
    }

    public sealed class OrderValidator : FV.AbstractValidator<Order>
    {
        public OrderValidator(bool collection)
        {
            RuleLevelCascadeMode = FV.CascadeMode.Continue;
            RuleFor(order => order.Shipping).SetValidator(new AddressValidator());
            if (collection) RuleForEach(order => order.Stops).SetValidator(new AddressValidator());
        }
    }
}
