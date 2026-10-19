using System;
using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

// FluentValidation's rule methods are extension methods; the alias keeps its types apart from Cohesion's.
using FluentValidation;

using FV = FluentValidation;

namespace Assimalign.Cohesion.ObjectValidation.Benchmarks.ObjectValidation;

/// <summary>
/// Validating one object with a built validator, against FluentValidation (the baseline in each category): a request
/// model that passes, the same model failing three of its five members, and the legacy suite's comparison profile.
/// Web.Validation runs a validator like this on every request body.
/// </summary>
/// <remarks>
/// <para>
/// FluentValidation is configured with <c>RuleLevelCascadeMode = Stop</c>, matching Cohesion's default of stopping a
/// member's rule chain at its first failure, so both report the same errors. Both validators are built once in
/// setup, which checks that they agree.
/// </para>
/// <para>
/// <c>Comparisons</c> ports the legacy ObjectValidation benchmark: eight <c>EqualTo</c> rules over short, int,
/// nullable long and <see cref="DateTime"/> members of a nested object, half of them failing. The legacy
/// FluentValidation side called <c>object.Equals</c> instead of the <c>Equal</c> rule, so it validated nothing; this
/// port uses <c>Equal</c>.
/// </para>
/// </remarks>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ValidatorBenchmarks
{
    private IValidator _cohesion = null!;
    private RegistrationValidator _fluentRegistration = null!;
    private ComparisonValidator _fluentComparison = null!;
    private Registration _valid = null!;
    private Registration _invalid = null!;
    private ComparisonModel _comparison = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = Validator.Create(validator => validator.AddProfile(profiles => profiles
            .CreateProfile<Registration>(descriptor =>
            {
                descriptor.RuleFor(p => p.Name).NotEmpty().MaxLength(100);
                descriptor.RuleFor(p => p.Email).NotEmpty().MaxLength(254);
                descriptor.RuleFor(p => p.Age).BetweenOrEqualTo(18, 120);
                descriptor.RuleFor(p => p.Country).Length(2);
                descriptor.RuleForEach(p => p.Tags).NotEmpty();
            })
            .CreateProfile<ComparisonModel>(descriptor =>
            {
                descriptor.RuleFor(p => p.Fields.ShortSuccess).EqualTo(ComparisonModel.Expected);
                descriptor.RuleFor(p => p.Fields.ShortFailure).EqualTo(ComparisonModel.Expected);
                descriptor.RuleFor(p => p.Fields.IntSuccess).EqualTo(ComparisonModel.Expected);
                descriptor.RuleFor(p => p.Fields.IntFailure).EqualTo(ComparisonModel.Expected);
                descriptor.RuleFor(p => p.Fields.LongSuccess).EqualTo(ComparisonModel.Expected);
                descriptor.RuleFor(p => p.Fields.LongFailure).EqualTo(ComparisonModel.Expected);
                descriptor.RuleFor(p => p.Fields.DateTimeSuccess).EqualTo(ComparisonModel.ExpectedDate);
                descriptor.RuleFor(p => p.Fields.DateTimeFailure).EqualTo(ComparisonModel.ExpectedDate);
            })));

        _fluentRegistration = new RegistrationValidator();
        _fluentComparison = new ComparisonValidator();

        _valid = new Registration
        {
            Name = "Ada Lovelace",
            Email = "ada@example.com",
            Age = 36,
            Country = "GB",
            Tags = ["analyst", "engine", "notes"],
        };
        _invalid = new Registration
        {
            Name = "",
            Email = "ada@example.com",
            Age = 12,
            Country = "GBR",
            Tags = ["analyst", "engine", "notes"],
        };
        _comparison = new ComparisonModel();

        Check("valid", Validate().Errors.Count(), Validate_FluentValidation().Errors.Count, expected: 0);
        Check("invalid", ValidateInvalid().Errors.Count(), ValidateInvalid_FluentValidation().Errors.Count, expected: 3);
        Check("comparison", ValidateComparisons().Errors.Count(), ValidateComparisons_FluentValidation().Errors.Count, expected: 4);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Valid")]
    public FV.Results.ValidationResult Validate_FluentValidation() => _fluentRegistration.Validate(_valid);

    [Benchmark]
    [BenchmarkCategory("Valid")]
    public ValidationResult Validate() => _cohesion.Validate(_valid);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Invalid")]
    public FV.Results.ValidationResult ValidateInvalid_FluentValidation() => _fluentRegistration.Validate(_invalid);

    [Benchmark]
    [BenchmarkCategory("Invalid")]
    public ValidationResult ValidateInvalid() => _cohesion.Validate(_invalid);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Comparisons")]
    public FV.Results.ValidationResult ValidateComparisons_FluentValidation() => _fluentComparison.Validate(_comparison);

    [Benchmark]
    [BenchmarkCategory("Comparisons")]
    public ValidationResult ValidateComparisons() => _cohesion.Validate(_comparison);

    private static void Check(string model, int cohesionErrors, int fluentErrors, int expected)
    {
        if (cohesionErrors != expected || fluentErrors != expected)
        {
            throw new InvalidOperationException(
                $"The {model} model: expected {expected} errors, Cohesion reported {cohesionErrors} and FluentValidation {fluentErrors}.");
        }
    }

    public sealed class Registration
    {
        public string Name { get; set; } = "";

        public string Email { get; set; } = "";

        public int Age { get; set; }

        public string Country { get; set; } = "";

        public List<string> Tags { get; set; } = [];
    }

    public sealed class ComparisonModel
    {
        public const short Expected = 10;

        public static readonly DateTime ExpectedDate = new(2021, 12, 1);

        public ComparisonFields Fields { get; set; } = new();
    }

    public sealed class ComparisonFields
    {
        public short ShortSuccess { get; set; } = 10;

        public short ShortFailure { get; set; } = 14;

        public int IntSuccess { get; set; } = 10;

        public int IntFailure { get; set; } = 14;

        public long? LongSuccess { get; set; } = 10;

        public long LongFailure { get; set; } = 14;

        public DateTime DateTimeSuccess { get; set; } = new(2021, 12, 1);

        public DateTime DateTimeFailure { get; set; } = new(2021, 12, 2);
    }

    private sealed class RegistrationValidator : FV.AbstractValidator<Registration>
    {
        public RegistrationValidator()
        {
            RuleLevelCascadeMode = FV.CascadeMode.Stop;

            RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
            RuleFor(p => p.Email).NotEmpty().MaximumLength(254);
            RuleFor(p => p.Age).InclusiveBetween(18, 120);
            RuleFor(p => p.Country).Length(2);
            RuleForEach(p => p.Tags).NotEmpty();
        }
    }

    private sealed class ComparisonValidator : FV.AbstractValidator<ComparisonModel>
    {
        public ComparisonValidator()
        {
            RuleLevelCascadeMode = FV.CascadeMode.Stop;

            RuleFor(p => p.Fields.ShortSuccess).Equal(ComparisonModel.Expected);
            RuleFor(p => p.Fields.ShortFailure).Equal(ComparisonModel.Expected);
            RuleFor(p => p.Fields.IntSuccess).Equal(ComparisonModel.Expected);
            RuleFor(p => p.Fields.IntFailure).Equal(ComparisonModel.Expected);
            RuleFor(p => p.Fields.LongSuccess).Equal(ComparisonModel.Expected);
            RuleFor(p => p.Fields.LongFailure).Equal(ComparisonModel.Expected);
            RuleFor(p => p.Fields.DateTimeSuccess).Equal(ComparisonModel.ExpectedDate);
            RuleFor(p => p.Fields.DateTimeFailure).Equal(ComparisonModel.ExpectedDate);
        }
    }
}
