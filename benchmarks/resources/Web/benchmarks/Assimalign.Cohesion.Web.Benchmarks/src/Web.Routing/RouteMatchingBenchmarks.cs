using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http;
using Assimalign.Cohesion.Web.Benchmarks.Fixtures;
using Assimalign.Cohesion.Web.Routing;

using BenchmarkDotNet.Attributes;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.Routing;

/// <summary>
/// Endpoint selection and route-value capture for GET routes with an int constraint, including a constraint miss.
/// Both use their public routing middleware and stop before endpoint execution. No typed handler binding is included.
/// Reused contexts have their selection cleared each call so ASP.NET Core cannot skip matching a cached endpoint.
/// </summary>
public class RouteMatchingBenchmarks
{
    private PipelineFixture _fixture = null!;
    private IWebApplicationPipeline _pipeline = null!;
    private BenchmarkHttpContext _context = null!;
    private ServiceProvider _services = null!;
    private RequestDelegate _aspNetCore = null!;
    private DefaultHttpContext _aspContext = null!;

    [Params(1, 100)]
    public int RouteCount { get; set; }

    [Params(true, false)]
    public bool Match { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        IRouterFeature routing = new RoutingBuilder().Build();
        _fixture = new PipelineFixture(routing);
        IRouterBuilder routes = _fixture.Application.UseRouting();
        DiagnosticListener diagnostics = new("WebRoutingBenchmarks");
        _services = new ServiceCollection().AddLogging().AddRouting()
            .AddSingleton(diagnostics).AddSingleton<DiagnosticSource>(diagnostics).BuildServiceProvider();
        ApplicationBuilder builder = new(_services);
        builder.UseRouting();
        // Register endpoints below, but stop after selection rather than dispatching them.
        builder.Run(_ => Task.CompletedTask);
        builder.UseEndpoints(endpoints =>
        {
            for (int i = 0; i < RouteCount; i++)
            {
                string template = $"/items{i}/{{id:int}}";
                routes.Map(Http.HttpMethod.Get, template, new RouterRouteHandler(_ => Task.CompletedTask));
                endpoints.MapGet(template, (RequestDelegate)(_ => Task.CompletedTask));
            }
        });
        _fixture.Builder.Run(_ => Task.CompletedTask);
        _pipeline = _fixture.Build();
        _aspNetCore = builder.Build();
        string path = $"/items{RouteCount - 1}/{(Match ? "42" : "invalid")}";
        _context = _fixture.CreateContext();
        _context.Request.Path = new HttpPath(path);
        _aspContext = new DefaultHttpContext { RequestServices = _services };
        _aspContext.Request.Method = "GET";
        _aspContext.Request.Path = path;
        int expected = Match ? 1 : 0;
        if (Select().GetAwaiter().GetResult() != expected || Select_AspNetCore().GetAwaiter().GetResult() != expected)
        {
            throw new InvalidOperationException("Route selection did not produce the expected constrained match.");
        }
        if (Match && (_context.GetRouteMatch()?.Values?["id"]?.ToString() != "42"
            || _aspContext.Request.RouteValues["id"]?.ToString() != "42"))
        {
            throw new InvalidOperationException("Route selection captured the wrong parameter value.");
        }
    }

    [Benchmark]
    public async Task<int> Select()
    {
        _context.Features.Set<IRouterEndpointFeature>(null);
        await _pipeline.ExecuteAsync(_context);
        return _context.Features.Get<IRouteMatchFeature>()?.Values?.Count ?? 0;
    }

    [Benchmark(Baseline = true)]
    public async Task<int> Select_AspNetCore()
    {
        _aspContext.SetEndpoint(null);
        _aspContext.Request.RouteValues.Clear();
        await _aspNetCore(_aspContext);
        return _aspContext.Request.RouteValues.Count;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _fixture.Dispose();
        _services.Dispose();
    }
}
