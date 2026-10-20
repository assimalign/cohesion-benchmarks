using System;
using System.Linq;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http;
using Assimalign.Cohesion.Web.Benchmarks.Fixtures;
using Assimalign.Cohesion.Web.Routing;

using BenchmarkDotNet.Attributes;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.Api;

/// <summary>
/// Invoke a typed endpoint that binds an int route value, an int query value and the exchange, then sets status.
/// Route selection is prepared in setup on both sides: this isolates binding from the separate routing suite.
/// Cohesion's MapGet source-generated thunk is compared with ASP.NET Core's public RequestDelegateFactory thunk;
/// both perform required-value parsing and call the same-shaped Task-returning handler, without serialization.
/// </summary>
public class ParameterBindingBenchmarks
{
    private PipelineFixture _fixture = null!;
    private IRouterRouteHandler _handler = null!;
    private BenchmarkHttpContext _context = null!;
    private ServiceProvider _services = null!;
    private RequestDelegate _aspHandler = null!;
    private DefaultHttpContext _aspContext = null!;

    [GlobalSetup]
    public void Setup()
    {
        IRouterFeature routing = new RoutingBuilder().Build();
        _fixture = new PipelineFixture(routing);
        _fixture.Application.MapGet("/items/{id}", (int id, int page, IHttpContext context) =>
        {
            context.Response.StatusCode = id + page == 45 ? HttpStatusCode.NoContent : HttpStatusCode.BadRequest;
            return Task.CompletedTask;
        });
        IRouterRoute route = routing.Router.Routes.Single();
        _handler = route.Handler;
        _context = _fixture.CreateContext();
        _context.SetRouteMatch(route, new Assimalign.Cohesion.Web.Routing.RouteValueDictionary { ["id"] = "42" });
        _context.Request.Query.Add("page", "3");

        _services = new ServiceCollection().AddLogging().BuildServiceProvider();
        _aspHandler = RequestDelegateFactory.Create((int id, int page, Microsoft.AspNetCore.Http.HttpContext context) =>
        {
            context.Response.StatusCode = id + page == 45 ? 204 : 400;
            return Task.CompletedTask;
        }, new RequestDelegateFactoryOptions { ServiceProvider = _services, RouteParameterNames = ["id"] }).RequestDelegate;
        _aspContext = new DefaultHttpContext { RequestServices = _services };
        _aspContext.Request.RouteValues["id"] = "42";
        _aspContext.Request.QueryString = new QueryString("?page=3");
        if (Bind().GetAwaiter().GetResult() != 204 || Bind_AspNetCore().GetAwaiter().GetResult() != 204)
        {
            throw new InvalidOperationException("Typed endpoint parameters were not bound correctly.");
        }
    }

    [Benchmark]
    public async Task<int> Bind()
    {
        _context.Response.StatusCode = HttpStatusCode.Ok;
        await _handler.InvokeAsync(_context);
        return (int)_context.Response.StatusCode;
    }

    [Benchmark(Baseline = true)]
    public async Task<int> Bind_AspNetCore()
    {
        _aspContext.Response.StatusCode = 200;
        await _aspHandler(_aspContext);
        return _aspContext.Response.StatusCode;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _fixture.Dispose();
        _services.Dispose();
    }
}
