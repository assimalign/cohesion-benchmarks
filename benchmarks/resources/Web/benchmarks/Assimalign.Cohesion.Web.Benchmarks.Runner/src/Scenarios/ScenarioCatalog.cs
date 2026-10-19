using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using Assimalign.Cohesion.Web.Benchmarks.Runner.Http;

namespace Assimalign.Cohesion.Web.Benchmarks.Runner.Scenarios;

/// <summary>
/// Every measured workload. The Web packages with no ASP.NET Core counterpart or no plaintext HTTP/1.1
/// shape (Query, Authentication.Certificate, HSTS, HTTP logging, Testing, ApplicationModel) are listed
/// in the README with the reason they are not here.
/// </summary>
internal static class ScenarioCatalog
{
    private const string Core = "core";
    private const string Feature = "feature";

    public static IReadOnlyList<LoadScenario> All { get; } = Create();

    public static IReadOnlyList<LoadScenario> Select(string selection)
    {
        if (string.IsNullOrWhiteSpace(selection) || selection == "all")
        {
            return All;
        }

        if (selection is Core or Feature)
        {
            return All.Where(scenario => scenario.Category == selection).ToList();
        }

        var selected = new List<LoadScenario>();
        foreach (string id in selection.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            selected.Add(All.FirstOrDefault(scenario => scenario.Id == id)
                ?? throw new ArgumentException($"Unknown scenario '{id}'. Known: {string.Join(", ", All.Select(s => s.Id))}"));
        }
        return selected;
    }

    private static IReadOnlyList<LoadScenario> Create()
    {
        string bearer = "Bearer " + Jwt.CreateHs256(BenchmarkPayloads.JwtKey, DateTimeOffset.UtcNow);
        byte[] orderJson = JsonSerializer.SerializeToUtf8Bytes(SampleOrder(), BenchmarkJsonContext.Default.Order);
        byte[] gzippedText = Gzip(Encoding.UTF8.GetBytes(BenchmarkPayloads.Compressible));
        byte[] form = Encoding.ASCII.GetBytes(
            "name=Ada&language=csharp&email=ada%40example.com&country=UK&city=London" +
            "&company=Analytical+Engines&title=Countess&phone=%2B44-20-0000-0000&newsletter=true&referrer=benchmark");

        static HttpRequestTemplate Get(string path) => new("GET", path);

        return
        [
            // ---- core request path -------------------------------------------------------------------
            new(ServerScenarios.Plaintext, ServerScenarios.Plaintext, Core, "Terminal middleware writes 13 bytes with Content-Length; no routing.")
            {
                Requests = _ => [Get("/plaintext")],
                ExpectBodyContains = BenchmarkPayloads.HelloWorld,
            },
            new("plaintext-pipelined", ServerScenarios.Plaintext, Core, "The plaintext pipeline with 16 requests pipelined per connection (TechEmpower's plaintext depth).")
            {
                Requests = _ => [Get("/plaintext")],
                ExpectBodyContains = BenchmarkPayloads.HelloWorld,
                PipelineDepth = 16,
            },
            new(ServerScenarios.PlaintextRouted, ServerScenarios.PlaintextRouted, Core, "MapGet returning a string, through the router.")
            {
                Requests = _ => [Get("/plaintext")],
                ExpectBodyContains = BenchmarkPayloads.HelloWorld,
            },
            new(ServerScenarios.Json, ServerScenarios.Json, Core, "MapGet returning a record serialized with a source-generated JSON context.")
            {
                Requests = _ => [Get("/json")],
                ExpectBodyContains = BenchmarkPayloads.HelloWorld,
            },
            new(ServerScenarios.RoutingTable, ServerScenarios.RoutingTable, Core, $"{BenchmarkPayloads.RouteTableSize} parameterized routes with an int constraint; requests rotate across 10 of them.")
            {
                Requests = _ => [.. new[] { 0, 11, 23, 37, 49, 58, 66, 74, 88, 99 }.Select(i => Get($"/api/v1/resource{i}/{i * 7 + 1}"))],
                ExpectBodyContains = "0:1",
            },
            new(ServerScenarios.QueryBinding, ServerScenarios.QueryBinding, Core, "Two int parameters bound from the query string; the sum written as JSON.")
            {
                Requests = _ => [Get("/add?a=17&b=25")],
                ExpectBodyContains = "42",
            },
            new(ServerScenarios.JsonPost, ServerScenarios.JsonPost, Core, $"A {orderJson.Length}-byte JSON order bound from the body and echoed back.")
            {
                Requests = _ => [new HttpRequestTemplate("POST", "/echo") { Headers = [("Content-Type", "application/json")], Body = orderJson }],
                ExpectBodyContains = "\"customer\"",
            },
            new(ServerScenarios.LargeResponse, ServerScenarios.LargeResponse, Core, "A 64 KiB body written in one call with Content-Length.")
            {
                Requests = _ => [Get("/large")],
            },
            new(ServerScenarios.MiddlewareChain, ServerScenarios.MiddlewareChain, Core, $"{BenchmarkPayloads.MiddlewareChainLength} pass-through Use((context, next) => next(context)) ahead of the plaintext terminal.")
            {
                Requests = _ => [Get("/plaintext")],
                ExpectBodyContains = BenchmarkPayloads.HelloWorld,
            },
            new(ServerScenarios.FullStack, ServerScenarios.FullStack, Core, "Nineteen middleware in MIDDLEWARE_ORDER.md order ahead of the json endpoint; every one takes its pass-through path.")
            {
                Requests = _ => [Get("/json")],
                ExpectBodyContains = BenchmarkPayloads.HelloWorld,
            },

            // ---- one Web package per scenario --------------------------------------------------------
            new(ServerScenarios.SecurityHeaders, ServerScenarios.SecurityHeaders, Feature, "Web.SecurityHeaders default policy.")
            {
                Requests = _ => [Get("/")],
                ExpectHeaders = ["X-Content-Type-Options"],
                Note = "ASP.NET Core has no security-headers middleware; its side is a hand-written OnStarting middleware writing the same headers.",
            },
            new(ServerScenarios.ForwardedHeaders, ServerScenarios.ForwardedHeaders, Feature, "X-Forwarded-For/Proto from a loopback proxy applied to the effective client.")
            {
                Requests = _ => [Get("/whoami").WithHeader("X-Forwarded-For", "203.0.113.7").WithHeader("X-Forwarded-Proto", "https")],
                ExpectBodyContains = "203.0.113.7",
            },
            new(ServerScenarios.HostFiltering, ServerScenarios.HostFiltering, Feature, "Host header checked against a two-entry allowlist.")
            {
                Requests = _ => [Get("/")],
                ExpectBodyContains = "ok",
            },
            new(ServerScenarios.HttpsRedirection, ServerScenarios.HttpsRedirection, Feature, "Plaintext request redirected 307 to https.")
            {
                Requests = _ => [Get("/cart?id=7")],
                ExpectedStatus = [307],
                ExpectHeaders = ["Location"],
            },
            new(ServerScenarios.ErrorHandling, ServerScenarios.ErrorHandling, Feature, "Handler throws; the exception boundary writes a 500 problem-details body.")
            {
                Requests = _ => [Get("/boom")],
                ExpectedStatus = [500],
                ExpectBodyContains = "500",
                Note = "Dominated by the cost of throwing and catching an exception in either framework.",
            },
            new(ServerScenarios.Rewrite, ServerScenarios.Rewrite, Feature, "A regex rewrite (/legacy/items/42 to /items/42) ahead of routing.")
            {
                Requests = _ => [Get("/legacy/items/42")],
                ExpectBodyContains = "Item 42",
            },
            new(ServerScenarios.CookiePolicy, ServerScenarios.CookiePolicy, Feature, "An appended SameSite=None cookie passes through the cookie policy.")
            {
                Requests = _ => [Get("/cookies")],
                ExpectHeaders = ["Set-Cookie"],
            },
            new("compression-gzip", ServerScenarios.Compression, Feature, "An 8 KiB text response gzip-compressed at CompressionLevel.Fastest.")
            {
                Requests = _ => [Get("/compressible").WithHeader("Accept-Encoding", "gzip")],
                ExpectHeaders = ["Content-Encoding"],
            },
            new("compression-brotli", ServerScenarios.Compression, Feature, "An 8 KiB text response Brotli-compressed at CompressionLevel.Fastest.")
            {
                Requests = _ => [Get("/compressible").WithHeader("Accept-Encoding", "br")],
                ExpectHeaders = ["Content-Encoding"],
            },
            new(ServerScenarios.RequestDecompression, ServerScenarios.RequestDecompression, Feature, $"A gzip request body ({gzippedText.Length} bytes, 8 KiB decompressed) read to the end.")
            {
                Requests = _ => [new HttpRequestTemplate("POST", "/upload") { Headers = [("Content-Type", "text/plain"), ("Content-Encoding", "gzip")], Body = gzippedText }],
                ExpectBodyContains = "8192",
            },
            new("static-file", ServerScenarios.StaticFiles, Feature, "A 4 KiB file from wwwroot.")
            {
                Requests = _ => [Get("/hello-4k.txt")],
            },
            new(ServerScenarios.AuthenticationBearer, ServerScenarios.AuthenticationBearer, Feature, "An HS256 JWT validated (signature, issuer, audience, lifetime) on every request.")
            {
                Requests = _ => [Get("/me").WithHeader("Authorization", bearer)],
                ExpectBodyContains = BenchmarkPayloads.JwtSubject,
            },
            new(ServerScenarios.AuthenticationCookie, ServerScenarios.AuthenticationCookie, Feature, "A data-protected authentication cookie decrypted on every request; each connection signs in once.")
            {
                Setup = new HttpRequestTemplate("POST", "/login?user=" + BenchmarkPayloads.JwtSubject),
                Requests = setup => [Get("/me").WithHeader("Cookie", setup.CookieHeader ?? string.Empty)],
                ExpectBodyContains = BenchmarkPayloads.JwtSubject,
            },
            new(ServerScenarios.Authorization, ServerScenarios.Authorization, Feature, "JWT authentication plus a role policy on the endpoint.")
            {
                Requests = _ => [Get("/admin").WithHeader("Authorization", bearer)],
                ExpectBodyContains = BenchmarkPayloads.JwtSubject,
            },
            new(ServerScenarios.Antiforgery, ServerScenarios.Antiforgery, Feature, "A POST validated against the antiforgery cookie and header token; each connection fetches its own pair.")
            {
                Setup = Get("/antiforgery/token"),
                SetupBodyIsToken = true,
                Requests = setup => [new HttpRequestTemplate("POST", "/antiforgery/submit")
                {
                    Headers = [("Cookie", setup.CookieHeader ?? string.Empty), (BenchmarkPayloads.AntiforgeryHeader, setup.Token ?? string.Empty)],
                }],
                ExpectBodyContains = "accepted",
            },
            new("cors-preflight", ServerScenarios.Cors, Feature, "A CORS preflight answered by the middleware.")
            {
                Requests = _ => [new HttpRequestTemplate("OPTIONS", "/products")
                {
                    Headers = [("Origin", BenchmarkPayloads.CorsOrigin), ("Access-Control-Request-Method", "GET")],
                }],
                ExpectedStatus = [200, 204],
                ExpectHeaders = ["Access-Control-Allow-Origin"],
            },
            new("cors-request", ServerScenarios.Cors, Feature, "A cross-origin GET that gets Access-Control-Allow-Origin.")
            {
                Requests = _ => [Get("/products").WithHeader("Origin", BenchmarkPayloads.CorsOrigin)],
                ExpectHeaders = ["Access-Control-Allow-Origin"],
            },
            new(ServerScenarios.RequestTimeouts, ServerScenarios.RequestTimeouts, Feature, "A 30-second default request timeout armed and disarmed per request.")
            {
                Requests = _ => [Get("/fast")],
            },
            new(ServerScenarios.RateLimiting, ServerScenarios.RateLimiting, Feature, "A global fixed-window limiter (int.MaxValue permits/minute) consulted on every request.")
            {
                Requests = _ => [Get("/limited")],
                Note = "Both use System.Threading.RateLimiting's FixedWindowRateLimiter; the difference is the middleware around it.",
            },
            new(ServerScenarios.Forms, ServerScenarios.Forms, Feature, $"A {form.Length}-byte urlencoded form with 10 fields parsed; two read.")
            {
                Requests = _ => [new HttpRequestTemplate("POST", "/signup") { Headers = [("Content-Type", "application/x-www-form-urlencoded")], Body = form }],
                ExpectBodyContains = "Ada:csharp",
            },
            new(ServerScenarios.OutputCache, ServerScenarios.OutputCache, Feature, "A response served from the in-memory output cache.")
            {
                Requests = _ => [Get("/cached")],
                ExpectBodyContains = "cached",
            },
            new(ServerScenarios.Sessions, ServerScenarios.Sessions, Feature, "An in-memory session loaded, an int read and written, and committed per request; one session per connection.")
            {
                Setup = Get("/session"),
                Requests = setup => [Get("/session").WithHeader("Cookie", setup.CookieHeader ?? string.Empty)],
            },
            new(ServerScenarios.Health, ServerScenarios.Health, Feature, "A JSON health report with one healthy check.")
            {
                Requests = _ => [Get("/healthz")],
                ExpectBodyContains = "ealthy",
                Note = "ASP.NET Core's side uses UseHealthChecks with a JSON writer, since Cohesion's MapHealthChecks needs no router and writes JSON.",
            },
            new(ServerScenarios.OpenApi, ServerScenarios.OpenApi, Feature, "The OpenAPI document for five typed endpoints.")
            {
                Requests = _ => [Get("/openapi/v1.json")],
                ExpectBodyContains = "openapi",
                Note = "Measures each framework's document-serving strategy as shipped (cached or regenerated); the documents are not byte-identical.",
            },
            new("validation-valid", ServerScenarios.Validation, Feature, "A JSON body bound and validated (3 rules) and accepted.")
            {
                Requests = _ => [new HttpRequestTemplate("POST", "/customers") { Headers = [("Content-Type", "application/json")], Body = """{"name":"Ada","age":36}"""u8.ToArray() }],
                ExpectBodyContains = "created Ada",
                Note = "Cohesion validates with an ObjectValidation profile; ASP.NET Core with .NET 10's DataAnnotations validation.",
            },
            new("validation-invalid", ServerScenarios.Validation, Feature, "A JSON body that fails validation, answered 400 with problem details.")
            {
                Requests = _ => [new HttpRequestTemplate("POST", "/customers") { Headers = [("Content-Type", "application/json")], Body = """{"name":"","age":12}"""u8.ToArray() }],
                ExpectedStatus = [400],
            },
            new("websocket-echo", ServerScenarios.WebSockets, Feature, "32-byte text messages echoed over one WebSocket per connection.")
            {
                Kind = LoadKind.WebSocket,
                WebSocketPath = "/ws",
                WebSocketMessageSize = 32,
                Note = "Throughput is messages per second (one round trip each), not HTTP requests.",
            },
        ];
    }

    private static Order SampleOrder() => new(
        1042,
        "Ada Lovelace",
        new DateTimeOffset(2026, 10, 17, 12, 0, 0, TimeSpan.Zero),
        [
            new OrderLine("SKU-0001", "Analytical engine punch cards", 12, 1.25m),
            new OrderLine("SKU-0002", "Brass gear assembly", 3, 48.00m),
            new OrderLine("SKU-0003", "Difference engine manual", 1, 19.99m),
            new OrderLine("SKU-0004", "Steam regulator", 2, 72.50m),
            new OrderLine("SKU-0005", "Ink, black, 1 litre", 4, 6.75m),
        ],
        322.99m,
        "Deliver to the workshop entrance; ring twice.");

    private static byte[] Gzip(byte[] data)
    {
        using var output = new MemoryStream();
        using (var gzip = new GZipStream(output, CompressionLevel.Optimal, leaveOpen: true))
        {
            gzip.Write(data);
        }
        return output.ToArray();
    }
}

/// <summary>Signs the HS256 token both servers validate.</summary>
internal static class Jwt
{
    public static string CreateHs256(byte[] key, DateTimeOffset now)
    {
        long issuedAt = now.ToUnixTimeSeconds();
        string header = Base64Url("""{"alg":"HS256","typ":"JWT"}"""u8);
        string payload = Base64Url(Encoding.UTF8.GetBytes(
            $$"""{"iss":"{{BenchmarkPayloads.JwtIssuer}}","aud":"{{BenchmarkPayloads.JwtAudience}}","sub":"{{BenchmarkPayloads.JwtSubject}}","name":"{{BenchmarkPayloads.JwtSubject}}","role":"{{BenchmarkPayloads.JwtRole}}","iat":{{issuedAt}},"nbf":{{issuedAt - 60}},"exp":{{issuedAt + 86400}}}"""));
        string signingInput = header + "." + payload;
        byte[] signature = HMACSHA256.HashData(key, Encoding.ASCII.GetBytes(signingInput));
        return signingInput + "." + Base64Url(signature);
    }

    private static string Base64Url(ReadOnlySpan<byte> data) =>
        Convert.ToBase64String(data).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
