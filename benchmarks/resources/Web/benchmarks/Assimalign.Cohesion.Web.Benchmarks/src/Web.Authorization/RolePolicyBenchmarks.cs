using System.Threading.Tasks;

using Assimalign.Cohesion.Web.Benchmarks.Fixtures;

using BenchmarkDotNet.Attributes;

using AspNetCoreAuthorization = Microsoft.AspNetCore.Authorization;
using CohesionAuthorization = Assimalign.Cohesion.Web.Authorization;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.Authorization;

/// <summary>
/// Evaluating a <c>RequireRole("admin")</c> policy against an authenticated principal, as
/// <c>RequireAuthorization("admins")</c> does per request. ASP.NET Core's authorization service is the baseline.
/// </summary>
public class RolePolicyBenchmarks
{
    private AuthenticationFixture _fixture = null!;
    private BenchmarkHttpContext _context = null!;

    [GlobalSetup]
    public void Setup()
    {
        _fixture = new AuthenticationFixture();
        _context = BenchmarkHttpContext.Create();
    }

    [GlobalCleanup]
    public void Cleanup() => _fixture.Dispose();

    [Benchmark(Baseline = true)]
    public Task<AspNetCoreAuthorization.AuthorizationResult> Evaluate_AspNetCore()
        => _fixture.AspNetCoreAuthorizationService.AuthorizeAsync(_fixture.Principal, _context, _fixture.AspNetCoreRolePolicy.Requirements);

    [Benchmark]
    public ValueTask<bool> Evaluate()
        => _fixture.CohesionRolePolicy.EvaluateAsync(new CohesionAuthorization.AuthorizationContext(_context, _fixture.Principal));
}
