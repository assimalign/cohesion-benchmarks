using System;
using System.Collections.Generic;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using AspNetHeaders = Microsoft.Net.Http.Headers;

namespace Assimalign.Cohesion.Http.Benchmarks.Http.Cookies;

/// <summary>
/// The two cookie paths a server pays for: reading the request <c>Cookie</c> header into cookies (every request
/// that carries a session) and formatting a <c>Set-Cookie</c> value (every response that issues or refreshes one).
/// Baselines are ASP.NET Core's <c>CookieHeaderValue.ParseList</c> and <c>SetCookieHeaderValue.ToString</c>.
/// </summary>
/// <remarks>
/// Cohesion parses the request header through the public path a consumer takes: constructing an
/// <see cref="HttpCookieCollection"/> over the request headers, which validates each cookie against the
/// RFC 6265 grammar and the RFC 6265bis size limits.
/// </remarks>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
// Opted into NativeAOT runs: this path is reflection-free by design, so losing the JIT is a real question for it.
[BenchmarkCategory(global::Assimalign.Cohesion.Benchmarks.BenchmarkRuntimes.Category)]
public class HttpCookieBenchmarks
{
    private const string parseCategory = "ParseCookieHeader";
    private const string formatCategory = "FormatSetCookie";

    // Six cookies, the shape of a signed-in browser session with analytics and CSRF cookies.
    private const string cookieHeader =
        "session=8f3b2c1d9e7a4b6c0d1e2f3a4b5c6d7e; theme=dark; _ga=GA1.2.1234567890.1700000000; " +
        "_gid=GA1.2.987654321.1700000000; csrf=Zm9vYmFyYmF6cXV4; lang=en-US";

    private static readonly DateTimeOffset _expires = new(2030, 1, 1, 0, 0, 0, TimeSpan.Zero);

    private HttpHeaderCollection _requestHeaders = null!;
    private string[] _cookieLines = null!;
    private HttpCookie _cookie = null!;
    private AspNetHeaders.SetCookieHeaderValue _aspNetCookie = null!;

    [GlobalSetup]
    public void Setup()
    {
        _requestHeaders = new HttpHeaderCollection();
        _requestHeaders.Add(HttpHeaderKey.Cookie, cookieHeader);
        _cookieLines = [cookieHeader];

        _cookie = new HttpCookie("session", "8f3b2c1d9e7a4b6c0d1e2f3a4b5c6d7e", new HttpCookieOptions
        {
            Domain = "example.com",
            Path = "/",
            Expires = _expires,
            MaxAge = TimeSpan.FromHours(1),
            Secure = true,
            HttpOnly = true,
            SameSite = HttpCookieSameSiteMode.Lax,
        });

        _aspNetCookie = new AspNetHeaders.SetCookieHeaderValue("session", "8f3b2c1d9e7a4b6c0d1e2f3a4b5c6d7e")
        {
            Domain = "example.com",
            Path = "/",
            Expires = _expires,
            MaxAge = TimeSpan.FromHours(1),
            Secure = true,
            HttpOnly = true,
            SameSite = AspNetHeaders.SameSiteMode.Lax,
        };

        int cohesion = ParseCookieHeader().Count;
        int aspNetCore = ParseCookieHeader_AspNetCore().Count;
        if (cohesion != 6 || aspNetCore != 6)
        {
            throw new InvalidOperationException($"Expected six cookies from both parsers, got {cohesion} and {aspNetCore}.");
        }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(parseCategory)]
    public IList<AspNetHeaders.CookieHeaderValue> ParseCookieHeader_AspNetCore()
        => AspNetHeaders.CookieHeaderValue.ParseList(_cookieLines);

    [Benchmark]
    [BenchmarkCategory(parseCategory)]
    public HttpCookieCollection ParseCookieHeader() => new(_requestHeaders, HttpHeaderKey.Cookie);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(formatCategory)]
    public string FormatSetCookie_AspNetCore() => _aspNetCookie.ToString();

    [Benchmark]
    [BenchmarkCategory(formatCategory)]
    public string FormatSetCookie() => _cookie.ToString();
}
