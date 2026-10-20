using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.ApplicationModel.Gateway.ControlPlane;
using Assimalign.Cohesion.Http;

namespace Assimalign.Cohesion.ApplicationModel.Benchmarks.ApplicationModelGatewayControlPlane;

/// <summary>
/// The gateway control plane's fixed five-route matcher: literal, captured command, HEAD,
/// method refusal and missing path. The subject is the internal router itself; the public factory
/// exposes it only behind a listening server. Reflection constructs the shipped types in setup,
/// then a compiled delegate calls Match directly without reflection, boxing or network in the
/// measurement. The table matches GatewayControlPlaneServer. No equivalent composition counterpart.
/// </summary>
public class ControlPlaneRoutingBenchmarks
{
    private Func<HttpMethod, HttpPath, int> _match = null!;
    private readonly HttpPath _application = "/cohesion/v1/application";
    private readonly HttpPath _command = "/cohesion/v1/resources/service-23/commands/configuration-001";
    private readonly HttpPath _missing = "/cohesion/v1/unknown/service-23";

    [GlobalSetup]
    public void Setup()
    {
        Assembly assembly = typeof(GatewayControlPlane).Assembly;
        const string prefix = "Assimalign.Cohesion.ApplicationModel.Gateway.ControlPlane.Internal.";
        Type routerType = assembly.GetType(prefix + "ControlPlaneRouter", throwOnError: true)!;
        Type routeType = assembly.GetType(prefix + "ControlPlaneRoute", throwOnError: true)!;
        Type handlerType = assembly.GetType(prefix + "ControlPlaneRouteHandler", throwOnError: true)!;
        Delegate handler = Delegate.CreateDelegate(handlerType,
            typeof(ControlPlaneRoutingBenchmarks).GetMethod(nameof(Handle), BindingFlags.Static | BindingFlags.NonPublic)!);
        (HttpMethod Method, string Path)[] definitions =
        [
            (HttpMethod.Get, "/cohesion/v1/application"),
            (HttpMethod.Get, "/cohesion/v1/resources/{name}"),
            (HttpMethod.Get, "/cohesion/v1/resources/{name}/commands"),
            (HttpMethod.Put, "/cohesion/v1/resources/{name}/commands/{id}"),
            (HttpMethod.Delete, "/cohesion/v1/resources/{name}/commands/{id}"),
        ];
        Array routes = Array.CreateInstance(routeType, definitions.Length);
        for (int index = 0; index < definitions.Length; index++)
            routes.SetValue(Activator.CreateInstance(routeType, definitions[index].Method, definitions[index].Path, handler), index);
        object router = Activator.CreateInstance(routerType, new object[] { routes })!;
        ParameterExpression method = Expression.Parameter(typeof(HttpMethod));
        ParameterExpression path = Expression.Parameter(typeof(HttpPath));
        MethodCallExpression call = Expression.Call(Expression.Constant(router), routerType.GetMethod("Match")!, method, path);
        _match = Expression.Lambda<Func<HttpMethod, HttpPath, int>>(
            Expression.Convert(Expression.Property(call, "Status"), typeof(int)), method, path).Compile();
        if (MatchApplication() != 1 || MatchCommand() != 1 || MatchHead() != 1 || RejectMethod() != 2 || MissPath() != 0)
            throw new InvalidOperationException("Control-plane fixture did not exercise the expected route outcomes.");
    }

    [Benchmark]
    public int MatchApplication() => _match(HttpMethod.Get, _application);

    [Benchmark]
    public int MatchCommand() => _match(HttpMethod.Put, _command);

    [Benchmark]
    public int MatchCommandDeletion() => _match(HttpMethod.Delete, _command);

    [Benchmark]
    public int MatchHead() => _match(HttpMethod.Head, _application);

    [Benchmark]
    public int RejectMethod() => _match(HttpMethod.Post, _command);

    [Benchmark]
    public int MissPath() => _match(HttpMethod.Get, _missing);

    private static Task Handle(IHttpContext context, IReadOnlyDictionary<string, string> values,
        CancellationToken cancellationToken) => Task.CompletedTask;
}
