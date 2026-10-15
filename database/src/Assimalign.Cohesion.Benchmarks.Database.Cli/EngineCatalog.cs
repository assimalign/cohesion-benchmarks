using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Assimalign.Cohesion.Benchmarks.Database.Cli;

internal sealed class EngineCatalog : IDisposable
{
    private readonly JsonDocument json;
    public EngineCatalog(string? path)
    {
        path ??= Environment.GetEnvironmentVariable("DBBENCH_CATALOG");
        if (path == null)
        {
            string bundled = Path.Combine(AppContext.BaseDirectory, "engines", "catalog.json");
            path = File.Exists(bundled) ? bundled : Path.Combine("database", "engines", "catalog.json");
        }
        json = JsonDocument.Parse(File.ReadAllText(path));
    }
    public JsonElement Find(string target)
    {
        foreach (var item in json.RootElement.GetProperty("targets").EnumerateArray())
            if (item.GetProperty("id").GetString() == target) return item;
        throw new ArgumentException("Unknown catalog target " + target);
    }
    public string? Endpoint(string target, string suffix)
    {
        var item = Find(target);
        if (item.GetProperty("kind").GetString() == "embedded") return null;
        bool network = Environment.GetEnvironmentVariable("DBBENCH_NETWORK") == "1";
        string host = network ? item.GetProperty("containerName").GetString()! + suffix : "localhost";
        var port = item.GetProperty("ports")[0];
        int number = port.GetProperty(network ? "internal" : "published").GetInt32();
        return item.GetProperty("endpointTemplate").GetString()!.Replace("{host}", host, StringComparison.Ordinal)
            .Replace("{port}", number.ToString(System.Globalization.CultureInfo.InvariantCulture), StringComparison.Ordinal);
    }
    public Dictionary<string, string> Settings(string target)
    {
        var settings = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var field in Find(target).GetProperty("environment").EnumerateObject()) settings[field.Name] = field.Value.GetString()!;
        return settings;
    }
    public void Print()
    {
        foreach (var item in json.RootElement.GetProperty("targets").EnumerateArray())
            Console.WriteLine($"catalog {item.GetProperty("model").GetString()} {item.GetProperty("id").GetString()} {item.GetProperty("kind").GetString()}");
    }
    public void Dispose() => json.Dispose();
}
