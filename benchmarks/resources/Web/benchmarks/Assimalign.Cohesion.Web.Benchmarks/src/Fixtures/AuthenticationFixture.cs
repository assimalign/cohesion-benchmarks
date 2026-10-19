using System;
using System.Buffers.Text;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Assimalign.Cohesion.Http;
using Assimalign.Cohesion.Web.Authentication.Bearer;
using Assimalign.Cohesion.Web.Authentication.Cookie;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using AspNetCoreAuthentication = Microsoft.AspNetCore.Authentication;
using AspNetCoreAuthorization = Microsoft.AspNetCore.Authorization;
using AspNetCoreCookies = Microsoft.AspNetCore.Authentication.Cookies;
using CohesionAuthentication = Assimalign.Cohesion.Web.Authentication;
using CohesionAuthorization = Assimalign.Cohesion.Web.Authorization;
using CohesionBearer = Assimalign.Cohesion.Web.Authentication.Bearer;
using CohesionCookie = Assimalign.Cohesion.Web.Authentication.Cookie;
using CohesionDataProtection = Assimalign.Cohesion.Security.DataProtection;

namespace Assimalign.Cohesion.Web.Benchmarks.Fixtures;

/// <summary>
/// Cohesion and ASP.NET Core configured to do the same work as the end-to-end harness's auth scenarios: the same
/// HS256 key, issuer, audience and token (minted like the runner mints it), role claim type <c>role</c>, and a
/// cookie ticket for one identity with one name claim, issued through each framework's own sign-in.
/// </summary>
internal sealed class AuthenticationFixture : IDisposable
{
    private readonly string _keyDirectory = Path.Combine(Path.GetTempPath(), "cohesion-benchmarks", "keys", Guid.NewGuid().ToString("N"));

    public AuthenticationFixture()
    {
        Token = CreateToken();

        // Cohesion: bearer.
        CohesionAuthentication.AuthenticationBuilder bearer = new();
        bearer.Options.DefaultScheme = CohesionBearer.JwtBearerDefaults.AuthenticationScheme;
        bearer.AddJwtBearer(options =>
        {
            options.SigningKeys.Add(CohesionBearer.HmacJwtSignatureVerifier.Create(BenchmarkPayloads.JwtKey));
            options.ValidIssuers.Add(BenchmarkPayloads.JwtIssuer);
            options.ValidAudiences.Add(BenchmarkPayloads.JwtAudience);
            options.RoleClaimType = "role";
        });
        CohesionBearerService = bearer.Build();

        // Cohesion: cookie, over a file-system key ring (the default production repository) in a temp folder.
        CohesionAuthentication.AuthenticationBuilder cookie = new();
        cookie.Options.DefaultScheme = CohesionCookie.CookieAuthenticationDefaults.AuthenticationScheme;
        cookie.UseDataProtection(CohesionDataProtection.DataProtectionProvider.Create(
            CohesionDataProtection.FileSystemKeyRepository.Create(_keyDirectory)));
        cookie.AddCookie();
        CohesionCookieService = cookie.Build();
        (CohesionCookieName, CohesionCookieValue) = SignInCohesion(CohesionCookieService);

        // ASP.NET Core: the same schemes, ephemeral data protection keys.
        ServiceCollection services = new();
        services.AddLogging();
        services.AddDataProtection().UseEphemeralDataProtectionProvider();
        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = BenchmarkPayloads.JwtIssuer,
                    ValidAudience = BenchmarkPayloads.JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(BenchmarkPayloads.JwtKey),
                    NameClaimType = "name",
                    RoleClaimType = "role",
                };
            })
            .AddCookie();
        services.AddAuthorization();
        AspNetCoreServices = services.BuildServiceProvider();
        AspNetCoreCookieValue = ProtectAspNetCoreTicket(AspNetCoreServices);

        // Authorization: the principal the bearer scheme produces, and the same role policy on both sides.
        CohesionAuthentication.AuthenticateResult result = CohesionBearerService
            .AuthenticateAsync(CreateCohesionBearerContext(), CohesionBearer.JwtBearerDefaults.AuthenticationScheme)
            .GetAwaiter().GetResult();
        Principal = result.Principal ?? throw new InvalidOperationException($"The Cohesion bearer scheme rejected the fixture token: {result.Failure?.Message}");
        CohesionRolePolicy = new CohesionAuthorization.AuthorizationPolicyBuilder().RequireRole(BenchmarkPayloads.JwtRole).Build();
        AspNetCoreRolePolicy = new AspNetCoreAuthorization.AuthorizationPolicyBuilder().RequireRole(BenchmarkPayloads.JwtRole).Build();
        AspNetCoreAuthorizationService = AspNetCoreServices.GetRequiredService<AspNetCoreAuthorization.IAuthorizationService>();
    }

    public string Token { get; }

    public CohesionAuthentication.IAuthenticationService CohesionBearerService { get; }

    public CohesionAuthentication.IAuthenticationService CohesionCookieService { get; }

    public string CohesionCookieName { get; }

    public string CohesionCookieValue { get; }

    public ServiceProvider AspNetCoreServices { get; }

    public string AspNetCoreCookieValue { get; }

    public ClaimsPrincipal Principal { get; }

    public CohesionAuthorization.AuthorizationPolicy CohesionRolePolicy { get; }

    public AspNetCoreAuthorization.AuthorizationPolicy AspNetCoreRolePolicy { get; }

    public AspNetCoreAuthorization.IAuthorizationService AspNetCoreAuthorizationService { get; }

    /// <summary>A fresh request carrying the bearer token, as each request arrives.</summary>
    public BenchmarkHttpContext CreateCohesionBearerContext()
    {
        BenchmarkHttpContext context = BenchmarkHttpContext.Create();
        context.SetAuthorization("Bearer " + Token);
        return context;
    }

    /// <summary>A fresh request carrying the authentication cookie.</summary>
    public BenchmarkHttpContext CreateCohesionCookieContext()
    {
        BenchmarkHttpContext context = BenchmarkHttpContext.Create();
        context.Request.Headers[HttpHeaderKey.Cookie] = CohesionCookieName + "=" + CohesionCookieValue;
        return context;
    }

    /// <summary>A fresh ASP.NET Core request (and its request services scope) carrying the bearer token.</summary>
    public DefaultHttpContext CreateAspNetCoreBearerContext(IServiceProvider requestServices)
    {
        DefaultHttpContext context = new() { RequestServices = requestServices };
        context.Request.Headers.Authorization = "Bearer " + Token;
        return context;
    }

    /// <summary>A fresh ASP.NET Core request carrying the authentication cookie.</summary>
    public DefaultHttpContext CreateAspNetCoreCookieContext(IServiceProvider requestServices)
    {
        DefaultHttpContext context = new() { RequestServices = requestServices };
        context.Request.Headers.Cookie = AspNetCoreCookies.CookieAuthenticationDefaults.CookiePrefix + "Cookies=" + AspNetCoreCookieValue;
        return context;
    }

    public void Dispose()
    {
        AspNetCoreServices.Dispose();
        try
        {
            Directory.Delete(_keyDirectory, recursive: true);
        }
        catch (IOException)
        {
            // Best effort: a temp folder.
        }
    }

    // Exactly the runner's ScenarioCatalog JwtToken.CreateHs256: same claims, order, encoding and lifetime.
    private static string CreateToken()
    {
        long issuedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string header = Base64Url.EncodeToString("""{"alg":"HS256","typ":"JWT"}"""u8);
        string payload = Base64Url.EncodeToString(Encoding.UTF8.GetBytes(
            $$"""{"iss":"{{BenchmarkPayloads.JwtIssuer}}","aud":"{{BenchmarkPayloads.JwtAudience}}","sub":"{{BenchmarkPayloads.JwtSubject}}","name":"{{BenchmarkPayloads.JwtSubject}}","role":"{{BenchmarkPayloads.JwtRole}}","iat":{{issuedAt}},"nbf":{{issuedAt - 60}},"exp":{{issuedAt + 86400}}}"""));
        byte[] signature = HMACSHA256.HashData(BenchmarkPayloads.JwtKey, Encoding.ASCII.GetBytes(header + "." + payload));
        return header + "." + payload + "." + Base64Url.EncodeToString(signature);
    }

    private static ClaimsPrincipal CreateCookiePrincipal(string authenticationType)
        => new(new ClaimsIdentity([new Claim(ClaimTypes.Name, BenchmarkPayloads.JwtSubject)], authenticationType));

    private static (string Name, string Value) SignInCohesion(CohesionAuthentication.IAuthenticationService service)
    {
        BenchmarkHttpContext context = BenchmarkHttpContext.Create();
        DateTimeOffset now = DateTimeOffset.UtcNow;
        service.SignInAsync(
                context,
                CohesionCookie.CookieAuthenticationDefaults.AuthenticationScheme,
                CreateCookiePrincipal(CohesionCookie.CookieAuthenticationDefaults.AuthenticationScheme),
                new CohesionAuthentication.AuthenticationProperties { IssuedUtc = now, ExpiresUtc = now.AddDays(14) })
            .GetAwaiter().GetResult();

        foreach (HttpCookie issued in context.Response.Cookies)
        {
            return (issued.Name, issued.Value);
        }

        throw new InvalidOperationException("The Cohesion cookie scheme issued no cookie.");
    }

    private static string ProtectAspNetCoreTicket(IServiceProvider services)
    {
        AspNetCoreCookies.CookieAuthenticationOptions options = services
            .GetRequiredService<IOptionsMonitor<AspNetCoreCookies.CookieAuthenticationOptions>>()
            .Get(AspNetCoreCookies.CookieAuthenticationDefaults.AuthenticationScheme);
        DateTimeOffset now = DateTimeOffset.UtcNow;
        AspNetCoreAuthentication.AuthenticationTicket ticket = new(
            CreateCookiePrincipal(AspNetCoreCookies.CookieAuthenticationDefaults.AuthenticationScheme),
            new AspNetCoreAuthentication.AuthenticationProperties { IssuedUtc = now, ExpiresUtc = now.AddDays(14) },
            AspNetCoreCookies.CookieAuthenticationDefaults.AuthenticationScheme);
        return options.TicketDataFormat.Protect(ticket);
    }
}
