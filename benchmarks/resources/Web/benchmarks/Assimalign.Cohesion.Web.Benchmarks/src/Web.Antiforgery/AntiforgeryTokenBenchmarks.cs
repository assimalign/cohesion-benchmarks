using System;
using System.IO;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http;
using Assimalign.Cohesion.Web.Antiforgery;
using Assimalign.Cohesion.Web.Benchmarks.Fixtures;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using CohesionProtection = Assimalign.Cohesion.Security.DataProtection;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.Antiforgery;

/// <summary>
/// Anonymous double-submit token-pair issue and header-token validation through the public services.
/// Both use purpose-bound data protection, the same header/cookie names, and their own protected token format.
/// Fresh exchanges on both sides include request creation and prevent ASP.NET Core's per-exchange token cache
/// from turning repeated calls into cached results. Authenticated identity binding and form parsing are excluded.
/// Key-ring creation and initial key loading happen in setup; no host or listener is started.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class AntiforgeryTokenBenchmarks
{
    private const string CookieName = "csrf";
    private const string HeaderName = "X-CSRF-TOKEN";
    private readonly string _keyDirectory = Path.Combine(Path.GetTempPath(), "cohesion-web-antiforgery", Guid.NewGuid().ToString("N"));
    private IHttpAntiforgery _service = null!;
    private IAntiforgery _aspService = null!;
    private ServiceProvider _services = null!;
    private string _cookie = null!;
    private string _token = null!;
    private string _aspCookie = null!;
    private string _aspToken = null!;

    [GlobalSetup]
    public void Setup()
    {
        AntiforgeryBuilder builder = new();
        builder.Options.CookieName = CookieName;
        builder.Options.HeaderName = HeaderName;
        builder.UseDataProtection(CohesionProtection.DataProtectionProvider.Create(
            CohesionProtection.FileSystemKeyRepository.Create(_keyDirectory)));
        _service = builder.Build().Antiforgery;

        ServiceCollection services = new();
        services.AddLogging();
        services.AddDataProtection().UseEphemeralDataProtectionProvider();
        services.AddAntiforgery(options =>
        {
            options.Cookie.Name = CookieName;
            options.HeaderName = HeaderName;
        });
        _services = services.BuildServiceProvider();
        _aspService = _services.GetRequiredService<IAntiforgery>();
        HttpAntiforgeryTokenSet tokens = Issue();
        AntiforgeryTokenSet aspTokens = Issue_AspNetCore();
        _cookie = CookieName + "=" + tokens.CookieToken;
        _token = tokens.RequestToken!;
        _aspCookie = CookieName + "=" + aspTokens.CookieToken;
        _aspToken = aspTokens.RequestToken!;
        if (!Validate().GetAwaiter().GetResult() || !Validate_AspNetCore().GetAwaiter().GetResult())
        {
            throw new InvalidOperationException("The issued antiforgery token pair did not validate.");
        }
        BenchmarkHttpContext invalid = CreateValidationContext();
        invalid.Request.Headers[new HttpHeaderKey(HeaderName)] = "invalid";
        DefaultHttpContext aspInvalid = CreateAspValidationContext();
        aspInvalid.Request.Headers[HeaderName] = "invalid";
        if (_service.IsRequestValidAsync(invalid).GetAwaiter().GetResult()
            || _aspService.IsRequestValidAsync(aspInvalid).GetAwaiter().GetResult())
        {
            throw new InvalidOperationException("Antiforgery accepted an invalid token.");
        }
    }

    [Benchmark, BenchmarkCategory("Issue")]
    public HttpAntiforgeryTokenSet Issue() => _service.GetTokens(BenchmarkHttpContext.Create());

    [Benchmark(Baseline = true), BenchmarkCategory("Issue")]
    public AntiforgeryTokenSet Issue_AspNetCore() => _aspService.GetTokens(new DefaultHttpContext { RequestServices = _services });

    [Benchmark, BenchmarkCategory("Validate")]
    public Task<bool> Validate() => _service.IsRequestValidAsync(CreateValidationContext());

    [Benchmark(Baseline = true), BenchmarkCategory("Validate")]
    public Task<bool> Validate_AspNetCore() => _aspService.IsRequestValidAsync(CreateAspValidationContext());

    private BenchmarkHttpContext CreateValidationContext()
    {
        BenchmarkHttpContext context = BenchmarkHttpContext.Create();
        context.Request.Method = Http.HttpMethod.Post;
        context.Request.Headers[HttpHeaderKey.Cookie] = _cookie;
        context.Request.Headers[new HttpHeaderKey(HeaderName)] = _token;
        return context;
    }

    private DefaultHttpContext CreateAspValidationContext()
    {
        DefaultHttpContext context = new() { RequestServices = _services };
        context.Request.Method = "POST";
        context.Request.Headers.Cookie = _aspCookie;
        context.Request.Headers[HeaderName] = _aspToken;
        return context;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _services.Dispose();
        string expectedParent = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "cohesion-web-antiforgery"));
        if (!string.Equals(Directory.GetParent(Path.GetFullPath(_keyDirectory))?.FullName, expectedParent, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The key directory is outside this benchmark's temporary directory.");
        }
        Directory.Delete(_keyDirectory, recursive: true);
    }
}
