using System;
using System.Buffers;
using System.IO;
using System.IO.Compression;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http;
using Assimalign.Cohesion.IdentityModel.Token.JsonWebToken;
using Assimalign.Cohesion.ObjectValidation;
using Assimalign.Cohesion.Web;
using Assimalign.Cohesion.Web.Antiforgery;
using Assimalign.Cohesion.Web.Authentication;
using Assimalign.Cohesion.Web.Authentication.Bearer;
using Assimalign.Cohesion.Web.Authentication.Cookie;
using Assimalign.Cohesion.Web.Authorization;
using Assimalign.Cohesion.Web.Benchmarks;
using Assimalign.Cohesion.Web.Caching;
using Assimalign.Cohesion.Web.Compression;
using Assimalign.Cohesion.Web.CookiePolicy;
using Assimalign.Cohesion.Web.Cors;
using Assimalign.Cohesion.Web.ErrorHandling;
using Assimalign.Cohesion.Web.ForwardedHeaders;
using Assimalign.Cohesion.Web.Health;
using Assimalign.Cohesion.Web.HostFiltering;
using Assimalign.Cohesion.Web.Hosting;
using Assimalign.Cohesion.Web.HttpsPolicy;
using Assimalign.Cohesion.Web.OpenApi;
using Assimalign.Cohesion.Web.RateLimiting;
using Assimalign.Cohesion.Web.RequestTimeouts;
using Assimalign.Cohesion.Web.Rewrite;
using Assimalign.Cohesion.Web.Routing;
using Assimalign.Cohesion.Web.SecurityHeaders;
using Assimalign.Cohesion.Web.Serialization;
using Assimalign.Cohesion.Web.Sessions;
using Assimalign.Cohesion.Web.StaticFiles;
using Assimalign.Cohesion.Web.Validation;
using Assimalign.Cohesion.Web.WebSockets;

(string scenario, int port) = BenchmarkHost.ParseArguments(args);

// The listener is configuration, exactly as a deployed application declares it. HTTP/1.1 on loopback.
WebApplicationBuilder builder = WebApplication.CreateBuilder(
[
    "--Http:Endpoints:Main:Protocol=Http1",
    "--Http:Endpoints:Main:Host=127.0.0.1",
    $"--Http:Endpoints:Main:Port={port}",
]);

CohesionScenarios.Register(scenario, builder);

await using WebApplication application = builder.Build();

CohesionScenarios.Compose(scenario, application);

BenchmarkHost.WriteInfo("cohesion", BenchmarkHost.GetInformationalVersion(typeof(WebApplication)), scenario);

await application.RunAsync();

/// <summary>
/// One pipeline per scenario. Each mirrors the scenario of the same name in the ASP.NET Core server;
/// when the two differ in anything but spelling, the difference is called out here and in the README.
/// </summary>
static class CohesionScenarios
{
    public static void Register(string scenario, WebApplicationBuilder builder)
    {
        switch (scenario)
        {
            case ServerScenarios.Plaintext:
            case ServerScenarios.MiddlewareChain:
            case ServerScenarios.Health:
                break;

            case ServerScenarios.Json:
            case ServerScenarios.QueryBinding:
            case ServerScenarios.JsonPost:
                builder.Services
                    .AddRouting()
                    .AddJsonSerialization(BenchmarkJsonContext.Default);
                break;

            case ServerScenarios.FullStack:
                builder.Services
                    .AddRouting()
                    .AddJsonSerialization(BenchmarkJsonContext.Default)
                    .AddErrorHandling()
                    .AddAuthentication(authentication =>
                    {
                        authentication.Options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        authentication.AddJwtBearer(ConfigureJwtBearer);
                    })
                    .AddAuthorization(authorization => authorization.Options.AddPolicy(BenchmarkPayloads.AdminPolicy, policy => policy.RequireRole(BenchmarkPayloads.JwtRole)))
                    .AddAntiforgery();
                break;

            case ServerScenarios.ErrorHandling:
                builder.Services
                    .AddRouting()
                    .AddErrorHandling();
                break;

            case ServerScenarios.AuthenticationBearer:
                builder.Services
                    .AddRouting()
                    .AddAuthentication(authentication =>
                    {
                        authentication.Options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        authentication.AddJwtBearer(ConfigureJwtBearer);
                    });
                break;

            case ServerScenarios.AuthenticationCookie:
                builder.Services
                    .AddRouting()
                    .AddAuthentication(authentication =>
                    {
                        authentication.Options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        authentication.AddCookie();
                    });
                break;

            case ServerScenarios.Authorization:
                builder.Services
                    .AddRouting()
                    .AddAuthentication(authentication =>
                    {
                        authentication.Options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        authentication.AddJwtBearer(ConfigureJwtBearer);
                    })
                    .AddAuthorization(authorization => authorization.Options.AddPolicy(BenchmarkPayloads.AdminPolicy, policy => policy.RequireRole(BenchmarkPayloads.JwtRole)));
                break;

            case ServerScenarios.Antiforgery:
                builder.Services
                    .AddRouting()
                    .AddAntiforgery();
                break;

            case ServerScenarios.OpenApi:
                builder.Services
                    .AddRouting()
                    .AddJsonSerialization(BenchmarkJsonContext.Default)
                    .AddOpenApi();
                break;

            case ServerScenarios.Validation:
                builder.Services
                    .AddRouting()
                    .AddJsonSerialization(BenchmarkJsonContext.Default)
                    .AddValidation(validation => validation.Options.AddProfile(new CustomerProfile()));
                break;

            default:
                builder.Services.AddRouting();
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
                app.MapGet("/large", static (IHttpContext context) =>
                {
                    context.Response.StatusCode = HttpStatusCode.Ok;
                    context.Response.Headers[HttpHeaderKey.ContentType] = "text/plain";
                    context.Response.Headers[HttpHeaderKey.ContentLength] = "65536";
                    return context.Response.Body.WriteAsync(BenchmarkPayloads.Large, context.RequestCancelled).AsTask();
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
                // Every middleware in docs/resources/Web/MIDDLEWARE_ORDER.md that an internet-facing JSON API
                // would plausibly compose, in that order, serving the json scenario's endpoint. The request
                // carries no credentials, cookies, origin or compression headers, so every middleware takes
                // its pass-through path: this is the price of composition on an ordinary request.
                app.UseSecurityHeaders();
                app.UseForwardedHeaders(options => options.Headers = ForwardedHeaderNames.XForwarded);
                app.UseHostFiltering(options =>
                {
                    options.AllowedHosts.Add("localhost");
                    options.AllowedHosts.Add("127.0.0.1");
                });
                app.UseErrorHandling();
                app.UseRewrite(rules => rules.AddRewrite(@"^/legacy/items/(\d+)$", "/items/$1"));
                app.UseCookiePolicy();
                app.UseResponseCompression();
                app.UseRequestDecompression();
                app.UseStaticFiles();
                app.UseAuthentication();
                app.UseSessions();
                app.UseRouting();
                app.UseCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins(BenchmarkPayloads.CorsOrigin)));
                app.UseAuthorization();
                app.UseRequestTimeouts(TimeSpan.FromSeconds(30));
                app.UseRateLimiting(options => options.GlobalPolicy = UnlimitedPolicy());
                app.UseAntiforgery();
                app.UseOutputCache();
                app.UseWebSockets();
                app.MapGet("/json", () => new JsonMessage(BenchmarkPayloads.HelloWorld));
                break;

            case ServerScenarios.SecurityHeaders:
                app.UseSecurityHeaders();
                app.UseRouting();
                app.MapGet("/", () => "ok");
                break;

            case ServerScenarios.ForwardedHeaders:
                app.UseForwardedHeaders(options => options.Headers = ForwardedHeaderNames.XForwarded);
                app.UseRouting();
                app.MapGet("/whoami", static (IHttpContext context) => $"{context.EffectiveScheme}|{context.EffectiveRemoteIp}");
                break;

            case ServerScenarios.HostFiltering:
                app.UseHostFiltering(options =>
                {
                    options.AllowedHosts.Add("localhost");
                    options.AllowedHosts.Add("127.0.0.1");
                });
                app.UseRouting();
                app.MapGet("/", () => "ok");
                break;

            case ServerScenarios.HttpsRedirection:
                // Defaults: 307 to port 443, matching the ASP.NET Core server's explicit options.
                app.UseHttpsRedirection();
                app.UseRouting();
                app.MapGet("/cart", () => "ok");
                break;

            case ServerScenarios.ErrorHandling:
                app.UseErrorHandling();
                app.UseRouting();
                app.MapGet("/boom", static context => throw new InvalidOperationException("Benchmark fault."));
                break;

            case ServerScenarios.Rewrite:
                app.UseRewrite(rules => rules
                    .AddRedirect("^/old-greeting$", "/greeting", HttpStatusCode.MovedPermanently)
                    .AddRewrite(@"^/legacy/items/(\d+)$", "/items/$1"));
                app.UseRouting();
                app.MapGet("/items/{id:int}", (int id) => $"Item {id}");
                break;

            case ServerScenarios.CookiePolicy:
                app.UseCookiePolicy();
                app.UseRouting();
                app.MapGet("/cookies", static (IHttpContext context) =>
                {
                    context.Response.Cookies.Add(new HttpCookie("tracking", "1", new HttpCookieOptions { SameSite = HttpCookieSameSiteMode.None }));
                    return "ok";
                });
                break;

            case ServerScenarios.Compression:
                // Gzip and Brotli both offered at the fastest level; the request's Accept-Encoding picks one.
                app.UseResponseCompression(options => options.Level = CompressionLevel.Fastest);
                app.UseRouting();
                app.MapGet("/compressible", () => BenchmarkPayloads.Compressible);
                break;

            case ServerScenarios.RequestDecompression:
                app.UseRequestDecompression();
                app.UseRouting();
                app.MapPost("/upload", static async (IHttpContext context) =>
                {
                    long length = await DrainAsync(context.Request.Body, context.RequestCancelled);
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
                app.MapPost("/login", static async (string user, IHttpContext context) =>
                {
                    ClaimsIdentity identity = new([new Claim(ClaimTypes.Name, user)], CookieAuthenticationDefaults.AuthenticationScheme);
                    await context.SignInAsync(new ClaimsPrincipal(identity), cancellationToken: context.RequestCancelled);
                    return "ok";
                });
                app.MapGet("/me", static (IHttpContext context) => context.User.Identity is { IsAuthenticated: true } identity ? identity.Name ?? "unnamed" : "anonymous");
                break;

            case ServerScenarios.Authorization:
                app.UseAuthentication();
                app.UseRouting();
                app.UseAuthorization();
                app.MapGet("/admin", static (IHttpContext context) => context.User.Identity?.Name ?? "unnamed")
                    .RequireAuthorization(BenchmarkPayloads.AdminPolicy);
                break;

            case ServerScenarios.Antiforgery:
                app.UseRouting();
                app.UseAntiforgery();
                app.MapGet("/antiforgery/token", static (IHttpContext context) =>
                {
                    HttpAntiforgeryTokenSet tokens = context.RequireAntiforgery.GetAndStoreTokens(context);
                    return tokens.RequestToken ?? string.Empty;
                });
                app.MapPost("/antiforgery/submit", () => "accepted")
                    .RequireAntiforgery();
                break;

            case ServerScenarios.Cors:
                app.UseRouting();
                app.UseCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins(BenchmarkPayloads.CorsOrigin)));
                app.MapGet("/products", () => "products");
                break;

            case ServerScenarios.RequestTimeouts:
                app.UseRouting();
                app.UseRequestTimeouts(TimeSpan.FromSeconds(30));
                app.MapGet("/fast", () => "ok");
                break;

            case ServerScenarios.RateLimiting:
                app.UseRouting();
                app.UseRateLimiting(options => options.GlobalPolicy = UnlimitedPolicy());
                app.MapGet("/limited", () => "ok");
                break;

            case ServerScenarios.Forms:
                app.UseRouting();
                app.UseForms();
                app.MapPost("/signup", static (IHttpContext context) =>
                {
                    IHttpFormCollection form = context.Request.Form;
                    return $"{form["name"].Value}:{form["language"].Value}";
                });
                break;

            case ServerScenarios.OutputCache:
                app.UseRouting();
                app.UseOutputCache(options => options.AddBasePolicy(policy => policy.Duration = TimeSpan.FromMinutes(5)));
                app.MapGet("/cached", () => "cached");
                break;

            case ServerScenarios.Sessions:
                app.UseSessions();
                app.UseRouting();
                app.MapGet("/session", static async (IHttpContext context) =>
                {
                    IHttpSession session = await context.LoadSessionAsync(context.RequestCancelled);
                    int visits = (session.GetInt32("visits") ?? 0) + 1;
                    session.SetInt32("visits", visits);
                    return visits.ToString();
                });
                break;

            case ServerScenarios.Health:
                app.MapHealthChecks(HealthChecks.CreateBuilder()
                    .AddCheck("self", () => HealthCheckResult.Healthy("The application is running."))
                    .Build());
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
                app.MapWebSocket("/ws", EchoAsync);
                break;

            default:
                throw new ArgumentException($"Unknown scenario '{scenario}'.");
        }
    }

    private static Task WritePlaintext(IHttpContext context)
    {
        context.Response.StatusCode = HttpStatusCode.Ok;
        context.Response.Headers[HttpHeaderKey.ContentType] = "text/plain";
        context.Response.Headers[HttpHeaderKey.ContentLength] = "13";
        return context.Response.Body.WriteAsync(BenchmarkPayloads.HelloWorldBytes, context.RequestCancelled).AsTask();
    }

    private static void ConfigureJwtBearer(JwtBearerOptions options)
    {
        options.SigningKeys.Add(HmacJwtSignatureVerifier.Create(BenchmarkPayloads.JwtKey));
        options.ValidIssuers.Add(BenchmarkPayloads.JwtIssuer);
        options.ValidAudiences.Add(BenchmarkPayloads.JwtAudience);
        // The token carries a single "role" claim; Cohesion's default role claim type is "roles".
        options.RoleClaimType = "role";
    }

    // One partition, a window that never runs out: the limiter is consulted on every request and never rejects.
    private static RateLimitingPolicy UnlimitedPolicy() => RateLimitingPolicy.Create(
        static (IHttpContext _) => "global",
        static (string _) => new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
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

    private static async Task EchoAsync(IHttpContext context, WebSocket socket)
    {
        byte[] buffer = new byte[4096];
        while (true)
        {
            ValueWebSocketReceiveResult received = await socket.ReceiveAsync(buffer.AsMemory(), context.RequestCancelled);
            if (received.MessageType == WebSocketMessageType.Close)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                return;
            }
            await socket.SendAsync(buffer.AsMemory(0, received.Count), received.MessageType, received.EndOfMessage, context.RequestCancelled);
        }
    }
}

sealed class CustomerProfile : ValidationProfile<Customer>
{
    public override void Configure(IValidationRuleDescriptor<Customer> descriptor)
    {
        descriptor.RuleFor(customer => customer.Name).NotEmpty();
        descriptor.RuleFor(customer => customer.Age).GreaterThanOrEqualTo(18).LessThanOrEqualTo(150);
    }
}
