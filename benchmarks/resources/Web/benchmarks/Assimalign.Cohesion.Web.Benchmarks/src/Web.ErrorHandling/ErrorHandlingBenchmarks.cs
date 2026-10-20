using System;
using System.Threading.Tasks;

using Assimalign.Cohesion.Web.Benchmarks.Fixtures;
using Assimalign.Cohesion.Web.ErrorHandling;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.ErrorHandling;

/// <summary>
/// The production exception boundary, successful dispatch or a thrown fault rendered as 500 problem+json.
/// Standalone: ASP.NET Core's default exception handler has different re-execution and problem-details behavior.
/// The fault case includes exception construction/stack capture, as a faulting handler does on a real request.
/// </summary>
public class ErrorHandlingBenchmarks
{
    private PipelineFixture _fixture = null!;
    private IWebApplicationPipeline _pipeline = null!;
    private BenchmarkHttpContext _context = null!;

    [Params(false, true)]
    public bool Throw { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _fixture = new PipelineFixture();
        _fixture.Builder.UseErrorHandling();
        _fixture.Builder.Run(_ => Throw ? throw new InvalidOperationException("benchmark fault") : Task.CompletedTask);
        _pipeline = _fixture.Build();
        _context = _fixture.CreateContext();
        if (Execute().GetAwaiter().GetResult() != (Throw ? 500 : 200)
            || (Throw && _context.Response.Body.Length == 0))
        {
            throw new InvalidOperationException("Exception boundary did not produce the expected response.");
        }
    }

    [Benchmark]
    public async Task<int> Execute()
    {
        _context.Response.Body.SetLength(0);
        _context.Response.Headers.Clear();
        _context.Response.StatusCode = Http.HttpStatusCode.Ok;
        await _pipeline.ExecuteAsync(_context);
        return (int)_context.Response.StatusCode;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _context.Response.Body.Dispose();
        _fixture.Dispose();
    }
}
