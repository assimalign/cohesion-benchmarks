using System.Threading.Tasks;

using Assimalign.Cohesion.Web.Benchmarks.Fixtures;

using BenchmarkDotNet.Attributes;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Assimalign.Cohesion.Web.Benchmarks.Web;

/// <summary>Dispatch through synchronous pass-through middleware to a status-setting terminal on both frameworks.</summary>
public class PipelineDispatchBenchmarks
{
    private PipelineFixture _fixture = null!;
    private IWebApplicationPipeline _pipeline = null!;
    private BenchmarkHttpContext _context = null!;
    private ServiceProvider _services = null!;
    private RequestDelegate _aspNetCore = null!;
    private DefaultHttpContext _aspContext = null!;

    [Params(1, 10)]
    public int MiddlewareCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _fixture = new PipelineFixture();
        _context = _fixture.CreateContext();
        _services = new ServiceCollection().BuildServiceProvider();
        ApplicationBuilder builder = new(_services);
        _aspContext = new DefaultHttpContext();
        for (int i = 0; i < MiddlewareCount; i++)
        {
            _fixture.Builder.Use(next => context => next(context));
            builder.Use(next => context => next(context));
        }
        _fixture.Builder.Run(context =>
        {
            context.Response.StatusCode = Http.HttpStatusCode.NoContent;
            return Task.CompletedTask;
        });
        builder.Run(context =>
        {
            context.Response.StatusCode = 204;
            return Task.CompletedTask;
        });
        _pipeline = _fixture.Build();
        _aspNetCore = builder.Build();
        if (Dispatch().GetAwaiter().GetResult() != 204 || Dispatch_AspNetCore().GetAwaiter().GetResult() != 204)
        {
            throw new System.InvalidOperationException("The middleware terminal did not execute.");
        }
    }

    [Benchmark]
    public async Task<int> Dispatch()
    {
        await _pipeline.ExecuteAsync(_context);
        return (int)_context.Response.StatusCode;
    }

    [Benchmark(Baseline = true)]
    public async Task<int> Dispatch_AspNetCore()
    {
        await _aspNetCore(_aspContext);
        return _aspContext.Response.StatusCode;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _fixture.Dispose();
        _services.Dispose();
    }
}
