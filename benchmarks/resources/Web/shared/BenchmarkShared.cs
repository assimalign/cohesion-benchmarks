// Compiled into the Cohesion server, the ASP.NET Core server and the runner (linked, not referenced), so
// every side agrees on scenario names, payloads, keys and the startup handshake without a project
// reference across SDK types.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Assimalign.Cohesion.Web.Benchmarks;

/// <summary>
/// The server pipelines. A server composes exactly one of them per process, so every measurement is
/// "the minimal application plus this feature", never a shared pipeline where unrelated middleware
/// runs on every request.
/// </summary>
public static class ServerScenarios
{
    public const string Plaintext = "plaintext";
    public const string PlaintextRouted = "plaintext-routed";
    public const string Json = "json";
    public const string RoutingTable = "routing-table";
    public const string QueryBinding = "query-binding";
    public const string JsonPost = "json-post";
    public const string LargeResponse = "large-response";
    public const string MiddlewareChain = "middleware-chain";
    public const string FullStack = "full-stack";
    public const string SecurityHeaders = "security-headers";
    public const string ForwardedHeaders = "forwarded-headers";
    public const string HostFiltering = "host-filtering";
    public const string HttpsRedirection = "https-redirection";
    public const string ErrorHandling = "error-handling";
    public const string Rewrite = "rewrite";
    public const string CookiePolicy = "cookie-policy";
    public const string Compression = "compression";
    public const string RequestDecompression = "request-decompression";
    public const string StaticFiles = "static-files";
    public const string AuthenticationBearer = "auth-bearer";
    public const string AuthenticationCookie = "auth-cookie";
    public const string Authorization = "authorization";
    public const string Antiforgery = "antiforgery";
    public const string Cors = "cors";
    public const string RequestTimeouts = "request-timeouts";
    public const string RateLimiting = "rate-limiting";
    public const string Forms = "forms";
    public const string OutputCache = "output-cache";
    public const string Sessions = "sessions";
    public const string Health = "health";
    public const string OpenApi = "openapi";
    public const string Validation = "validation";
    public const string WebSockets = "websockets";
}

public static class BenchmarkPayloads
{
    public const string HelloWorld = "Hello, World!";

    public static readonly byte[] HelloWorldBytes = Encoding.UTF8.GetBytes(HelloWorld);

    /// <summary>64 KiB of text, written in one call.</summary>
    public static readonly byte[] Large = CreateText(64 * 1024);

    /// <summary>8 KiB of repetitive text that compresses well.</summary>
    public static readonly string Compressible = Encoding.UTF8.GetString(CreateText(8 * 1024));

    public const string JwtIssuer = "https://bench.cohesion.local";
    public const string JwtAudience = "bench-api";
    public const string JwtSubject = "alice";
    public const string JwtRole = "admin";

    /// <summary>The HS256 key both servers validate with and the runner signs with.</summary>
    public static readonly byte[] JwtKey = SHA256.HashData(Encoding.UTF8.GetBytes("cohesion-benchmarks/jwt"));

    public const string CorsOrigin = "https://app.example";
    public const string AntiforgeryHeader = "X-CSRF-TOKEN";
    public const string AdminPolicy = "admins";

    /// <summary>The number of routes the routing-table scenario registers.</summary>
    public const int RouteTableSize = 100;

    /// <summary>The number of pass-through middleware the middleware-chain scenario registers.</summary>
    public const int MiddlewareChainLength = 10;

    private static byte[] CreateText(int size)
    {
        const string line = "The quick brown fox jumps over the lazy dog. 0123456789\n";
        var builder = new StringBuilder(size + line.Length);
        while (builder.Length < size)
        {
            builder.Append(line);
        }
        builder.Length = size;
        return Encoding.UTF8.GetBytes(builder.ToString());
    }
}

public sealed record JsonMessage(string Message);

public sealed record OrderLine(string Sku, string Description, int Quantity, decimal UnitPrice);

public sealed record Order(int Id, string Customer, DateTimeOffset CreatedAt, List<OrderLine> Lines, decimal Total, string? Notes);

/// <summary>
/// The validation scenario's body. The DataAnnotations attributes drive ASP.NET Core's validation; the
/// Cohesion server declares the same two rules in a validation profile and ignores the attributes.
/// </summary>
public sealed record Customer(
    [property: Required, MinLength(1)] string Name,
    [property: Range(18, 150)] int Age);

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(JsonMessage))]
[JsonSerializable(typeof(Order))]
[JsonSerializable(typeof(Customer))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(string))]
public sealed partial class BenchmarkJsonContext : JsonSerializerContext;

/// <summary>The command line both servers accept and the one line they print once they are composed.</summary>
public static class BenchmarkHost
{
    public const string InfoPrefix = "BENCH-INFO ";

    public static (string Scenario, int Port) ParseArguments(string[] args)
    {
        string? scenario = null;
        int port = 5000;
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "--scenario")
            {
                scenario = args[i + 1];
            }
            else if (args[i] == "--port")
            {
                port = int.Parse(args[i + 1]);
            }
        }

        return (scenario ?? throw new ArgumentException("--scenario is required."), port);
    }

    /// <summary>
    /// Prints the runtime facts the runner records beside each result, so a comparison can be checked
    /// for an unequal GC mode, processor count or code-generation mode after the fact.
    /// </summary>
    public static void WriteInfo(string framework, string frameworkVersion, string scenario)
    {
        var json = new StringBuilder();
        json.Append('{');
        Append(json, "framework", framework).Append(',');
        Append(json, "frameworkVersion", frameworkVersion).Append(',');
        Append(json, "scenario", scenario).Append(',');
        Append(json, "runtime", RuntimeInformation.FrameworkDescription).Append(',');
        Append(json, "os", RuntimeInformation.OSDescription).Append(',');
        Append(json, "architecture", RuntimeInformation.ProcessArchitecture.ToString()).Append(',');
        Append(json, "serverGc", GCSettings.IsServerGC).Append(',');
        Append(json, "gcLatencyMode", GCSettings.LatencyMode.ToString()).Append(',');
        Append(json, "processorCount", Environment.ProcessorCount).Append(',');
        Append(json, "dynamicCode", RuntimeFeature.IsDynamicCodeCompiled).Append(',');
        Append(json, "processId", Environment.ProcessId);
        json.Append('}');
        Console.Out.WriteLine(InfoPrefix + json);
        Console.Out.Flush();
    }

    public static string GetInformationalVersion(Type type)
    {
        foreach (object attribute in type.Assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute), false))
        {
            string version = ((System.Reflection.AssemblyInformationalVersionAttribute)attribute).InformationalVersion;
            int plus = version.IndexOf('+');
            return plus < 0 ? version : version[..plus];
        }

        return type.Assembly.GetName().Version?.ToString() ?? "unknown";
    }

    private static StringBuilder Append(StringBuilder json, string name, string value) =>
        json.Append('"').Append(name).Append("\":\"").Append(value.Replace("\\", "\\\\").Replace("\"", "\\\"")).Append('"');

    private static StringBuilder Append(StringBuilder json, string name, bool value) =>
        json.Append('"').Append(name).Append("\":").Append(value ? "true" : "false");

    private static StringBuilder Append(StringBuilder json, string name, int value) =>
        json.Append('"').Append(name).Append("\":").Append(value);
}
