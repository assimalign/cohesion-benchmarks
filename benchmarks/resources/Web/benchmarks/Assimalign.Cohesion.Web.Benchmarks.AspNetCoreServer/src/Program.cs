using System;
using System.Buffers;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

using Assimalign.Cohesion.Web.Benchmarks;

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

(string scenario, int port) = BenchmarkHost.ParseArguments(args);

// CreateSlimBuilder is ASP.NET Core's lean builder: no host filtering, HTTPS or IIS defaults, which is
// the closest match to Cohesion's CreateBuilder. Logging providers are cleared because Cohesion
// registers none; a console sink on the request path would measure the console, not the framework.
WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
});
builder.Logging.ClearProviders();
builder.WebHost.ConfigureKestrel(kestrel =>
{
    kestrel.AddServerHeader = true;
    kestrel.Listen(IPAddress.Loopback, port, listen => listen.Protocols = HttpProtocols.Http1);
});

AspNetCoreScenarios.Register(scenario, builder);

await using WebApplication app = builder.Build();

AspNetCoreScenarios.Compose(scenario, app);

BenchmarkHost.WriteInfo("aspnetcore", BenchmarkHost.GetInformationalVersion(typeof(WebApplication)), scenario);

await app.RunAsync();

/// <summary>
/// One pipeline per scenario, mirroring the Cohesion server's scenario of the same name.
/// </summary>
static class AspNetCoreScenarios
{
    public static void Register(string scenario, WebApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;
        switch (scenario)
        {
            case ServerScenarios.Plaintext:
            case ServerScenarios.MiddlewareChain:
                break;

            case ServerScenarios.Json:
            case ServerScenarios.QueryBinding:
            case ServerScenarios.JsonPost:
                services.AddRouting();
                AddJson(services);
                break;

            case ServerScenarios.FullStack:
                services.AddRouting();
                AddJson(services);
                services.AddProblemDetails();
                services.AddHostFiltering(options => options.AllowedHosts = ["localhost", "127.0.0.1"]);
                services.AddResponseCompression(ConfigureCompression);
                services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
                services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
                services.AddRequestDecompression();
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(ConfigureJwtBearer);
                services.AddAuthorization(options => options.AddPolicy(BenchmarkPayloads.AdminPolicy, policy => policy.RequireRole(BenchmarkPayloads.JwtRole)));
                services.AddDistributedMemoryCache();
                services.AddSession();
                services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins(BenchmarkPayloads.CorsOrigin)));
                services.AddRequestTimeouts(options => options.DefaultPolicy = new RequestTimeoutPolicy { Timeout = TimeSpan.FromSeconds(30) });
                services.AddRateLimiter(options => options.GlobalLimiter = UnlimitedLimiter());
                services.AddAntiforgery(options => options.HeaderName = BenchmarkPayloads.AntiforgeryHeader);
                services.AddOutputCache();
                break;

            case ServerScenarios.HostFiltering:
                services.AddRouting();
                services.AddHostFiltering(options => options.AllowedHosts = ["localhost", "127.0.0.1"]);
                break;

            case ServerScenarios.HttpsRedirection:
                services.AddRouting();
                services.AddHttpsRedirection(options =>
                {
                    options.HttpsPort = 443;
                    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                });
                break;

            case ServerScenarios.ErrorHandling:
                services.AddRouting();
                services.AddProblemDetails();
                break;

            case ServerScenarios.Compression:
                services.AddRouting();
                services.AddResponseCompression(ConfigureCompression);
                services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
                services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
                break;

            case ServerScenarios.RequestDecompression:
                services.AddRouting();
                services.AddRequestDecompression();
                break;

            case ServerScenarios.AuthenticationBearer:
                services.AddRouting();
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(ConfigureJwtBearer);
                break;

            case ServerScenarios.AuthenticationCookie:
                services.AddRouting();
                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
                break;

            case ServerScenarios.Authorization:
                services.AddRouting();
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(ConfigureJwtBearer);
                services.AddAuthorization(options => options.AddPolicy(BenchmarkPayloads.AdminPolicy, policy => policy.RequireRole(BenchmarkPayloads.JwtRole)));
                break;

            case ServerScenarios.Antiforgery:
                services.AddRouting();
                services.AddAntiforgery(options => options.HeaderName = BenchmarkPayloads.AntiforgeryHeader);
                break;

            case ServerScenarios.Cors:
                services.AddRouting();
                services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins(BenchmarkPayloads.CorsOrigin)));
                break;

            case ServerScenarios.RequestTimeouts:
                services.AddRouting();
                services.AddRequestTimeouts(options => options.DefaultPolicy = new RequestTimeoutPolicy { Timeout = TimeSpan.FromSeconds(30) });
                break;

            case ServerScenarios.RateLimiting:
                services.AddRouting();
                services.AddRateLimiter(options => options.GlobalLimiter = UnlimitedLimiter());
                break;

            case ServerScenarios.OutputCache:
                services.AddRouting();
                services.AddOutputCache(options => options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromMinutes(5))));
                break;

            case ServerScenarios.Sessions:
                services.AddRouting();
                services.AddDistributedMemoryCache();
                services.AddSession();
                break;

            case ServerScenarios.Health:
                services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy("The application is running."));
                break;

            case ServerScenarios.OpenApi:
                services.AddRouting();
                AddJson(services);
                services.AddOpenApi();
                break;

            case ServerScenarios.Validation:
                services.AddRouting();
                AddJson(services);
                services.AddValidation();
                break;

            default:
                services.AddRouting();
                break;
        }
    }

    public static void Compose(string scenario, WebApplication app)
    {
        switch (scenario)
        {
            case ServerScenarios.Plaintext:
                app.Run(WritePlaintext);
                break;

            case ServerScenarios.PlaintextRouted:
                app.UseRouting();
                app.MapGet("/plaintext", () => BenchmarkPayloads.HelloWorld);
                break;

            case ServerScenarios.Json:
                app.UseRouting();
                app.MapGet("/json", () => new JsonMessage(BenchmarkPayloads.HelloWorld));
                break;

            case ServerScenarios.RoutingTable:
                app.UseRouting();
                for (int i = 0; i < BenchmarkPayloads.RouteTableSize; i++)
                {
                    int resource = i;
                    app.MapGet($"/api/v1/resource{i}/{{id:int}}", (int id) => $"{resource}:{id}");
                }
                break;

            case ServerScenarios.QueryBinding:
                app.UseRouting();
                app.MapGet("/add", (int a, int b) => a + b);
                break;

            case ServerScenarios.JsonPost:
                app.UseRouting();
                app.MapPost("/echo", (Order order) => order);
                break;

            case ServerScenarios.LargeResponse:
                app.UseRouting();
                app.MapGet("/large", static (HttpContext context) =>
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    context.Response.ContentType = "text/plain";
                    context.Response.ContentLength = BenchmarkPayloads.Large.Length;
                    return context.Response.Body.WriteAsync(BenchmarkPayloads.Large, context.RequestAborted).AsTask();
                });
                break;

            case ServerScenarios.MiddlewareChain:
                for (int i = 0; i < BenchmarkPayloads.MiddlewareChainLength; i++)
                {
                    app.Use(static (context, next) => next(context));
                }
                app.Run(WritePlaintext);
                break;

            case ServerScenarios.FullStack:
                app.Use(SecurityHeaders.Middleware);
                app.UseForwardedHeaders(ForwardedOptions());
                app.UseHostFiltering();
                app.UseExceptionHandler();
                app.UseRewriter(new RewriteOptions().AddRewrite(@"^legacy/items/(\d+)$", "items/$1", skipRemainingRules: true));
                app.UseCookiePolicy(CookiePolicy());
                app.UseResponseCompression();
                app.UseRequestDecompression();
                app.UseStaticFiles();
                app.UseAuthentication();
                app.UseSession();
                app.UseRouting();
                app.UseCors();
                app.UseAuthorization();
                app.UseRequestTimeouts();
                app.UseRateLimiter();
                app.UseAntiforgery();
                app.UseOutputCache();
                app.UseWebSockets();
                app.MapGet("/json", () => new JsonMessage(BenchmarkPayloads.HelloWorld));
                break;

            case ServerScenarios.SecurityHeaders:
                // ASP.NET Core ships no security-headers middleware. This one writes the header set Cohesion's
                // default policy writes, from a response-starting callback as Cohesion does.
                app.Use(SecurityHeaders.Middleware);
                app.UseRouting();
                app.MapGet("/", () => "ok");
                break;

            case ServerScenarios.ForwardedHeaders:
                app.UseForwardedHeaders(ForwardedOptions());
                app.UseRouting();
                app.MapGet("/whoami", static (HttpContext context) => $"{context.Request.Scheme}|{context.Connection.RemoteIpAddress}");
                break;

            case ServerScenarios.HostFiltering:
                app.UseHostFiltering();
                app.UseRouting();
                app.MapGet("/", () => "ok");
                break;

            case ServerScenarios.HttpsRedirection:
                app.UseHttpsRedirection();
                app.UseRouting();
                app.MapGet("/cart", () => "ok");
                break;

            case ServerScenarios.ErrorHandling:
                app.UseExceptionHandler();
                app.UseRouting();
                app.MapGet("/boom", static (HttpContext context) => Fault());
                break;

            case ServerScenarios.Rewrite:
                app.UseRewriter(new RewriteOptions()
                    .AddRedirect("^old-greeting$", "greeting", StatusCodes.Status301MovedPermanently)
                    .AddRewrite(@"^legacy/items/(\d+)$", "items/$1", skipRemainingRules: true));
                app.UseRouting();
                app.MapGet("/items/{id:int}", (int id) => $"Item {id}");
                break;

            case ServerScenarios.CookiePolicy:
                app.UseCookiePolicy(CookiePolicy());
                app.UseRouting();
                app.MapGet("/cookies", static (HttpContext context) =>
                {
                    context.Response.Cookies.Append("tracking", "1", new CookieOptions { SameSite = SameSiteMode.None });
                    return "ok";
                });
                break;

            case ServerScenarios.Compression:
                app.UseResponseCompression();
                app.UseRouting();
                app.MapGet("/compressible", () => BenchmarkPayloads.Compressible);
                break;

            case ServerScenarios.RequestDecompression:
                app.UseRequestDecompression();
                app.UseRouting();
                app.MapPost("/upload", static async (HttpContext context) =>
                {
                    long length = await DrainAsync(context.Request.Body, context.RequestAborted);
                    return length.ToString();
                });
                break;

            case ServerScenarios.StaticFiles:
                app.UseStaticFiles();
                break;

            case ServerScenarios.AuthenticationBearer:
            case ServerScenarios.AuthenticationCookie:
                app.UseAuthentication();
                app.UseRouting();
                app.MapPost("/login", static async (string user, HttpContext context) =>
                {
                    ClaimsIdentity identity = new([new Claim(ClaimTypes.Name, user)], CookieAuthenticationDefaults.AuthenticationScheme);
                    await context.SignInAsync(new ClaimsPrincipal(identity));
                    return "ok";
                });
                app.MapGet("/me", static (HttpContext context) => context.User.Identity is { IsAuthenticated: true } identity ? identity.Name ?? "unnamed" : "anonymous");
                break;

            case ServerScenarios.Authorization:
                app.UseAuthentication();
                app.UseRouting();
                app.UseAuthorization();
                app.MapGet("/admin", static (HttpContext context) => context.User.Identity?.Name ?? "unnamed")
                    .RequireAuthorization(BenchmarkPayloads.AdminPolicy);
                break;

            case ServerScenarios.Antiforgery:
                // ASP.NET Core's antiforgery middleware validates and records the outcome; only form binding
                // rejects on it. A header-token endpoint therefore reads the recorded outcome itself, which is
                // what Cohesion's RequireAntiforgery enforces in its middleware.
                app.UseRouting();
                app.UseAntiforgery();
                app.MapGet("/antiforgery/token", static (HttpContext context, IAntiforgery antiforgery) =>
                    antiforgery.GetAndStoreTokens(context).RequestToken ?? string.Empty);
                app.MapPost("/antiforgery/submit", static (HttpContext context) =>
                    context.Features.Get<IAntiforgeryValidationFeature>() is { IsValid: true }
                        ? Results.Text("accepted")
                        : Results.BadRequest())
                    .WithMetadata(new RequireAntiforgeryTokenAttribute(true));
                break;

            case ServerScenarios.Cors:
                app.UseRouting();
                app.UseCors();
                app.MapGet("/products", () => "products");
                break;

            case ServerScenarios.RequestTimeouts:
                app.UseRouting();
                app.UseRequestTimeouts();
                app.MapGet("/fast", () => "ok");
                break;

            case ServerScenarios.RateLimiting:
                app.UseRouting();
                app.UseRateLimiter();
                app.MapGet("/limited", () => "ok");
                break;

            case ServerScenarios.Forms:
                app.UseRouting();
                app.MapPost("/signup", static async (HttpContext context) =>
                {
                    IFormCollection form = await context.Request.ReadFormAsync(context.RequestAborted);
                    return $"{form["name"]}:{form["language"]}";
                });
                break;

            case ServerScenarios.OutputCache:
                app.UseRouting();
                app.UseOutputCache();
                app.MapGet("/cached", () => "cached");
                break;

            case ServerScenarios.Sessions:
                app.UseSession();
                app.UseRouting();
                app.MapGet("/session", static async (HttpContext context) =>
                {
                    ISession session = context.Session;
                    await session.LoadAsync(context.RequestAborted);
                    int visits = (session.GetInt32("visits") ?? 0) + 1;
                    session.SetInt32("visits", visits);
                    return visits.ToString();
                });
                break;

            case ServerScenarios.Health:
                // Middleware rather than an endpoint, as Cohesion's MapHealthChecks needs no router; the
                // writer emits a JSON report like Cohesion's instead of the bare "Healthy" default.
                app.UseHealthChecks("/healthz", new HealthCheckOptions { ResponseWriter = HealthReportWriter.WriteAsync });
                break;

            case ServerScenarios.OpenApi:
                app.UseRouting();
                app.MapGet("/orders/{id:int}", (int id) => new Order(id, "Ada", DateTimeOffset.UnixEpoch, [], 0m, null));
                app.MapPost("/orders", (Order order) => order);
                app.MapGet("/customers/{name}", (string name) => new Customer(name, 36));
                app.MapPost("/customers", (Customer customer) => customer);
                app.MapGet("/messages", () => new JsonMessage(BenchmarkPayloads.HelloWorld));
                app.MapOpenApi();
                break;

            case ServerScenarios.Validation:
                app.UseRouting();
                app.MapPost("/customers", (Customer customer) => $"created {customer.Name}");
                break;

            case ServerScenarios.WebSockets:
                app.UseRouting();
                app.UseWebSockets();
                app.Map("/ws", EchoAsync);
                break;

            default:
                throw new ArgumentException($"Unknown scenario '{scenario}'.");
        }
    }

    private static void AddJson(IServiceCollection services) =>
        services.ConfigureHttpJsonOptions(options => options.SerializerOptions.TypeInfoResolverChain.Insert(0, BenchmarkJsonContext.Default));

    private static Task WritePlaintext(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "text/plain";
        context.Response.ContentLength = BenchmarkPayloads.HelloWorldBytes.Length;
        return context.Response.Body.WriteAsync(BenchmarkPayloads.HelloWorldBytes, context.RequestAborted).AsTask();
    }

    private static string Fault() => throw new InvalidOperationException("Benchmark fault.");

    private static void ConfigureCompression(ResponseCompressionOptions options)
    {
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
    }

    private static void ConfigureJwtBearer(JwtBearerOptions options)
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
    }

    // Cohesion's default cookie policy upgrades a SameSite=None cookie to Secure; Secure = Always writes
    // the same Set-Cookie for the scenario's cookie.
    private static CookiePolicyOptions CookiePolicy() => new() { Secure = CookieSecurePolicy.Always };

    private static ForwardedHeadersOptions ForwardedOptions() => new()
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost,
    };

    private static PartitionedRateLimiter<HttpContext> UnlimitedLimiter() =>
        PartitionedRateLimiter.Create<HttpContext, string>(static _ => RateLimitPartition.GetFixedWindowLimiter(
            "global",
            static _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = int.MaxValue,
                Window = TimeSpan.FromMinutes(1),
            }));

    private static async Task<long> DrainAsync(Stream body, CancellationToken cancellationToken)
    {
        byte[] buffer = ArrayPool<byte>.Shared.Rent(16 * 1024);
        try
        {
            long total = 0;
            int read;
            while ((read = await body.ReadAsync(buffer, cancellationToken)) > 0)
            {
                total += read;
            }
            return total;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private static async Task EchoAsync(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        using WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();
        byte[] buffer = new byte[4096];
        while (true)
        {
            ValueWebSocketReceiveResult received = await socket.ReceiveAsync(buffer.AsMemory(), context.RequestAborted);
            if (received.MessageType == WebSocketMessageType.Close)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                return;
            }
            await socket.SendAsync(buffer.AsMemory(0, received.Count), received.MessageType, received.EndOfMessage, context.RequestAborted);
        }
    }
}

static class SecurityHeaders
{
    // Cohesion's default policy as observed on the wire (see README, "Parity").
    private static readonly (string Name, string Value)[] Headers =
    [
        ("X-Content-Type-Options", "nosniff"),
        ("X-Frame-Options", "DENY"),
        ("Content-Security-Policy", "frame-ancestors 'none'"),
        ("Referrer-Policy", "strict-origin-when-cross-origin"),
    ];

    public static Task Middleware(HttpContext context, RequestDelegate next)
    {
        context.Response.OnStarting(static state =>
        {
            IHeaderDictionary headers = ((HttpResponse)state).Headers;
            foreach ((string name, string value) in Headers)
            {
                headers[name] = value;
            }
            return Task.CompletedTask;
        }, context.Response);
        return next(context);
    }
}

static class HealthReportWriter
{
    public static Task WriteAsync(HttpContext context, HealthReport report)
    {
        var json = new StringBuilder();
        json.Append("{\"status\":\"").Append(report.Status).Append("\",\"checks\":[");
        bool first = true;
        foreach ((string name, HealthReportEntry entry) in report.Entries)
        {
            if (!first)
            {
                json.Append(',');
            }
            first = false;
            json.Append("{\"name\":\"").Append(name).Append("\",\"status\":\"").Append(entry.Status)
                .Append("\",\"description\":\"").Append(entry.Description).Append("\"}");
        }
        json.Append("]}");
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(json.ToString(), context.RequestAborted);
    }
}
