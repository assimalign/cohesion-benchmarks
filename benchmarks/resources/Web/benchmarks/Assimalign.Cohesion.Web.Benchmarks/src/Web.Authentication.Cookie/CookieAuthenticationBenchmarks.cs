using System.Threading.Tasks;

using Assimalign.Cohesion.Web.Benchmarks.Fixtures;

using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using AspNetCoreAuthentication = Microsoft.AspNetCore.Authentication;
using CohesionAuthentication = Assimalign.Cohesion.Web.Authentication;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.Authentication.Cookie;

/// <summary>
/// What one request pays to authenticate a data-protected cookie: parse the Cookie header, unprotect the ticket
/// (key-ring lookup, key derivation, authenticated decryption), deserialize it and build the principal. ASP.NET
/// Core (its default data protection algorithms, the ticket it issues itself) is the baseline.
/// </summary>
public class CookieAuthenticationBenchmarks
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
        return await service.AuthenticateAsync(_fixture.CreateAspNetCoreCookieContext(scope.ServiceProvider), "Cookies");
    }

    [Benchmark]
    public Task<CohesionAuthentication.AuthenticateResult> Authenticate()
        => _fixture.CohesionCookieService.AuthenticateAsync(_fixture.CreateCohesionCookieContext(), "Cookies");
}
