using System.Linq;
using BenchmarkDotNet.Attributes;
using FluentValidation;
using FV = FluentValidation;

namespace Assimalign.Cohesion.ObjectValidation.Benchmarks.ObjectValidation;

/// <summary>
/// Scalar collection validation and indexed error allocation with small/large collections.
/// Both validators apply the same not-empty and maximum-length chain, evaluating both rules even after a
/// failure per element. Every fourth element fails in the invalid fixture.
/// </summary>
public class CollectionValidationBenchmarks
{
    private IValidator _cohesion = null!;
    private TagsValidator _fluent = null!;
    private TagsModel _model = null!;

    [Params(4, 32)] public int Count { get; set; }
    [Params(true, false)] public bool Valid { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = Validator.Create(builder =>
        {
            builder.AddOptions(options => options.ContinueThroughValidationChain = true);
            builder.AddProfile(profiles => profiles.CreateProfile<TagsModel>(rules =>
                rules.RuleForEach(model => model.Tags).NotEmpty().MaxLength(32)));
        });
        _fluent = new TagsValidator();
        _model = new TagsModel
        {
            Tags = Enumerable.Range(0, Count).Select(i => !Valid && i % 4 == 0 ? "" : $"tag-{i}").ToArray(),
        };
        NestedValidationBenchmarks.Check(Validate().Errors.Count(), Validate_FluentValidation().Errors.Count, Valid ? 0 : Count / 4);
    }

    [Benchmark] public ValidationResult Validate() => _cohesion.Validate(_model);
    [Benchmark(Baseline = true)] public FV.Results.ValidationResult Validate_FluentValidation() => _fluent.Validate(_model);

    public sealed class TagsModel
    {
        public string[] Tags { get; set; } = [];
    }

    private sealed class TagsValidator : FV.AbstractValidator<TagsModel>
    {
        public TagsValidator()
        {
            RuleLevelCascadeMode = FV.CascadeMode.Continue;
            RuleForEach(model => model.Tags).NotEmpty().MaximumLength(32);
        }
    }
}
