using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assimalign.Cohesion.Benchmarks.External;

/// <summary>Resolves the catalog placeholders documented on <see cref="ExternalTarget"/>.</summary>
internal sealed partial class Placeholders
{
    internal const string Host = "127.0.0.1";

    private readonly ExternalTargetCatalog _catalog;
    private readonly IReadOnlyList<(int Internal, int Published)> _ports;

    public Placeholders(ExternalTargetCatalog catalog, IReadOnlyList<(int Internal, int Published)> ports)
    {
        _catalog = catalog;
        _ports = ports;
    }

    public string Resolve(string value)
    {
        string result = value
            .Replace("{host}", Host, StringComparison.Ordinal)
            .Replace("{repoRoot}", Trim(_catalog.RepositoryRoot), StringComparison.Ordinal)
            .Replace("{cohesionRoot}", Trim(_catalog.CohesionRoot), StringComparison.Ordinal)
            .Replace("{catalogDir}", Trim(_catalog.CatalogDirectory), StringComparison.Ordinal)
            .Replace("{exe}", ExternalTargetCatalog.ExecutableSuffix, StringComparison.Ordinal);

        if (_ports.Count > 0)
        {
            result = result.Replace("{port}", _ports[0].Published.ToString(), StringComparison.Ordinal);
            result = PortPattern().Replace(result, match =>
            {
                int requested = int.Parse(match.Groups[1].Value);
                return _ports.FirstOrDefault(p => p.Internal == requested) is { Published: > 0 } port
                    ? port.Published.ToString()
                    : throw new InvalidOperationException($"No port maps internal port {requested}.");
            });
        }

        return result;
    }

    /// <summary>Resolves a file path; relative paths are relative to the catalog directory.</summary>
    public string ResolvePath(string value)
    {
        string resolved = Resolve(value);
        return Path.GetFullPath(Path.IsPathRooted(resolved) ? resolved : Path.Combine(_catalog.CatalogDirectory, resolved));
    }

    /// <summary>A bare name stays a name (looked up on PATH); anything with a separator becomes a full path.</summary>
    public string ResolveCommand(string command)
    {
        string resolved = Resolve(command);
        return resolved.IndexOfAny(['/', '\\']) >= 0 ? ResolvePath(resolved) : resolved;
    }

    private static string Trim(string path) => path.TrimEnd('/', '\\');

    [GeneratedRegex(@"\{port:(\d+)\}")]
    private static partial Regex PortPattern();
}
