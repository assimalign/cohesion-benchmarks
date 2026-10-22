using System;

using Assimalign.Cohesion.Http;
using Assimalign.Cohesion.Web.Benchmarks.Fixtures;
using Assimalign.Cohesion.Web.Routing;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.Routing;

/// <summary>
/// One public route path match, including path splitting and capture allocation. Isolates the matcher
/// from pipeline dispatch; repeated and trailing separators exercise its empty-segment convention.
/// </summary>
public class RoutePathBenchmarks
{
    private Route _route = null!;
    private BenchmarkHttpContext _context = null!;

    [Params("literal", "parameter", "catchall")]
    public string Shape { get; set; } = "literal";

    [GlobalSetup]
    public void Setup()
    {
        (string template, string path, int count) = Shape switch
        {
            "literal" => ("/items/status", "/items/status/", 0),
            "parameter" => ("/items/{id:int}", "/items/42", 1),
            "catchall" => ("/files/{**path}", "/files//one/two/three/", 1),
            _ => throw new InvalidOperationException("Unknown route shape.")
        };
        _route = new Route(HttpMethod.Get, template);
        _context = BenchmarkHttpContext.Create();
        _context.Request.Path = new HttpPath(path);
        if (!_route.TryMatchPath(_context, out var values) || values.Count != count)
        {
            throw new InvalidOperationException("The prepared path did not match its route.");
        }
    }

    [Benchmark]
    public RouteValueDictionary Match()
    {
        _route.TryMatchPath(_context, out var values);
        return values;
    }
}
