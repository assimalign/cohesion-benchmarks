using System.Threading.Tasks;

using Assimalign.Cohesion.Web.Benchmarks.Fixtures;

using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using AspNetCoreAuthentication = Microsoft.AspNetCore.Authentication;
using CohesionAuthentication = Assimalign.Cohesion.Web.Authentication;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.Authentication.Bearer;

/// <summary>
/// What one request pays to authenticate an HS256 bearer token: a fresh request, the authentication service, the
/// handler (signature, issuer, audience and lifetime checked), and the principal. ASP.NET Core is the baseline; its
/// side includes the per-request service scope its handlers are resolved from.
/// </summary>
public class JwtBearerAuthenticationBenchmarks
{
    private AuthenticationFixture _fixture = null!;

    [GlobalSetup]
    public void Setup() => _fixture = new AuthenticationFixture();

    [GlobalCleanup]
    public void Cleanup() => _fixture.Dispose();

    [Benchmark(Baseline = true)]
    public async Task<AspNetCoreAuthentication.AuthenticateResult> Authenticate_AspNetCore()
    {
        using IServiceScope scope = _fixture.AspNetCoreServices.CreateScope();
        AspNetCoreAuthentication.IAuthenticationService service = scope.ServiceProvider.GetRequiredService<AspNetCoreAuthentication.IAuthenticationService>();
        return await service.AuthenticateAsync(_fixture.CreateAspNetCoreBearerContext(scope.ServiceProvider), "Bearer");
    }

    [Benchmark]
    public Task<CohesionAuthentication.AuthenticateResult> Authenticate()
        => _fixture.CohesionBearerService.AuthenticateAsync(_fixture.CreateCohesionBearerContext(), "Bearer");
}
