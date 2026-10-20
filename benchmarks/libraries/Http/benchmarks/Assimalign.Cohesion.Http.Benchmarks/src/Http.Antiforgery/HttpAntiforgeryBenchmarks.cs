using System;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http.Benchmarks.Fixtures;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Http.Benchmarks.Http.Antiforgery;

/// <summary>
/// Issuing a request token with an existing cookie secret and validating an unsafe request's
/// signed cookie/header pair. Cookie parsing is warmed as middleware normally does; token
/// verification still performs cryptographic work every call. ASP.NET Core antiforgery uses
/// Data Protection and identity/additional-data binding with different guarantees, so no ratio
/// is claimed against Cohesion's built-in HMAC protector.
/// </summary>
public class HttpAntiforgeryBenchmarks
{
    private HttpAntiforgery _service = null!;
    private BenchmarkHttpContext _context = null!;

    [GlobalSetup]
    public async Task Setup()
    {
        _service = HttpAntiforgery.Create(new HttpAntiforgeryOptions
        {
            Key = new byte[32],
            CookieName = "csrf",
            HeaderName = "X-CSRF-TOKEN",
        });
        await using BenchmarkHttpContext issuer = new();
        HttpAntiforgeryTokenSet tokens = _service.GetTokens(issuer);
        _context = new BenchmarkHttpContext();
        _context.Request.Headers[HttpHeaderKey.Cookie] = "csrf=" + tokens.CookieToken;
        _context.Request.Headers["X-CSRF-TOKEN"] = tokens.RequestToken!;
        if (!await Validate()) throw new InvalidOperationException("Antiforgery fixture was rejected.");
    }

    [GlobalCleanup]
    public ValueTask Cleanup() => _context.DisposeAsync();

    [Benchmark]
    public HttpAntiforgeryTokenSet Issue() => _service.GetTokens(_context);

    [Benchmark]
    public Task<bool> Validate() => _service.IsRequestValidAsync(_context);
}
