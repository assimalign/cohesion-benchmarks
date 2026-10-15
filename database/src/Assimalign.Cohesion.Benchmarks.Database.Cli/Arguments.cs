using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Assimalign.Cohesion.Benchmarks.Database.Cli;

internal sealed class Arguments
{
    private readonly Dictionary<string, string> values = new(StringComparer.Ordinal);
    public string Command { get; }
    public Arguments(string[] args)
    {
        Command = args.Length == 0 ? "help" : args[0];
        for (int i = 1; i < args.Length; i++)
        {
            string key = args[i];
            if (!key.StartsWith("--", StringComparison.Ordinal)) throw new ArgumentException($"Expected an option, got {key}.");
            key = key[2..];
            string value = key is "official" or "overwrite" or "smoke" or "json" ? "true" :
                ++i < args.Length && !args[i].StartsWith("--", StringComparison.Ordinal) ? args[i] :
                throw new ArgumentException($"Missing value for --{key}.");
            if (!values.TryAdd(key, value)) throw new ArgumentException($"Repeated option --{key}.");
        }
    }
    public void Allow(params string[] names)
    {
        foreach (string key in values.Keys) if (!names.Contains(key, StringComparer.Ordinal)) throw new ArgumentException($"Unknown option --{key} for {Command}.");
    }
    public bool Has(string key) => values.ContainsKey(key);
    public string? Optional(string key) => values.GetValueOrDefault(key);
    public string Required(string key) => Optional(key) ?? throw new ArgumentException($"--{key} is required.");
    public string Get(string key, string fallback) => Optional(key) ?? fallback;
    public int Int(string key, int fallback) => int.Parse(Get(key, fallback.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
    public double Double(string key, double fallback) => double.Parse(Get(key, fallback.ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
    public ulong Seed => ulong.Parse(Get("seed", "20261015"), CultureInfo.InvariantCulture);
    public TimeSpan? Duration(string key)
    {
        string? text = Optional(key);
        if (text == null) return null;
        if (text.EndsWith("ms", StringComparison.Ordinal)) return TimeSpan.FromMilliseconds(double.Parse(text[..^2], CultureInfo.InvariantCulture));
        if (text.EndsWith('s')) return TimeSpan.FromSeconds(double.Parse(text[..^1], CultureInfo.InvariantCulture));
        if (text.EndsWith('m')) return TimeSpan.FromMinutes(double.Parse(text[..^1], CultureInfo.InvariantCulture));
        return TimeSpan.Parse(text, CultureInfo.InvariantCulture);
    }
}
