using System;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http;
using Assimalign.Cohesion.ObjectValidation;
using Assimalign.Cohesion.Web.Benchmarks.Fixtures;
using Assimalign.Cohesion.Web.Validation;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.Validation;

/// <summary>
/// Public hand-bound request validation, including the errors map and 400 problem+json on invalid input.
/// Standalone: ASP.NET Core's model-validation integrations do not execute the same registered ObjectValidation
/// profiles or produce the same error contract; comparing only a rule evaluator would omit response work.
/// </summary>
public class RequestValidationBenchmarks
{
    private PipelineFixture _fixture = null!;
    private IWebApplicationPipeline _pipeline = null!;
    private BenchmarkHttpContext _context = null!;
    private Signup _value = null!;

    [Params(true, false)]
    public bool Valid { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        ValidationBuilder builder = new();
        builder.Options.AddProfile(new SignupProfile());
        _fixture = new PipelineFixture(builder.Build());
        _value = new Signup { Name = Valid ? "Ada" : "", Age = Valid ? 30 : 12 };
        _fixture.Builder.Run(async context => { await context.ValidateAsync(_value); });
        _pipeline = _fixture.Build();
        _context = _fixture.CreateContext();
        if (Validate().GetAwaiter().GetResult() != (Valid ? 200 : 400)
            || (!Valid && _context.Response.Body.Length == 0))
        {
            throw new InvalidOperationException("Request validation did not execute its registered rules.");
        }
    }

    [Benchmark]
    public async Task<int> Validate()
    {
        _context.Response.Body.SetLength(0);
        _context.Response.Headers.Clear();
        _context.Response.StatusCode = HttpStatusCode.Ok;
        await _pipeline.ExecuteAsync(_context);
        return (int)_context.Response.StatusCode;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _context.Response.Body.Dispose();
        _fixture.Dispose();
    }

    private sealed class Signup
    {
        public string Name { get; init; } = "";
        public int Age { get; init; }
    }

    private sealed class SignupProfile : ValidationProfile<Signup>
    {
        public override void Configure(IValidationRuleDescriptor<Signup> descriptor)
        {
            descriptor.RuleFor(value => value.Name).NotEmpty().MinLength(3);
            descriptor.RuleFor(value => value.Age).GreaterThanOrEqualTo(18);
        }
    }
}
