using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace Assimalign.Cohesion.Benchmarks.External;

/// <summary>Decides whether a target can run on this machine, and if not, why.</summary>
internal static class ExternalTargetAvailability
{
    private static readonly ConcurrentDictionary<string, bool> ProbedExecutables = new(StringComparer.OrdinalIgnoreCase);

    /// <returns>Null when the target can run; otherwise the reason it cannot.</returns>
    public static string? Check(ExternalTarget target, ExternalTargetCatalog catalog)
    {
        if (target.Platforms.Count > 0 && !target.Platforms.Contains(ExternalTargetCatalog.CurrentPlatform, StringComparer.OrdinalIgnoreCase))
        {
            return $"runs on {string.Join("/", target.Platforms)} only";
        }

        foreach (string requirement in target.Requires)
        {
            if (!Runs(requirement))
            {
                return $"'{requirement.Split(' ')[0]}' is not installed (or does not run)";
            }
        }

        Placeholders placeholders = new(catalog, ports: []);
        foreach (string file in target.RequiresFiles)
        {
            string path = placeholders.ResolvePath(file);
            if (!File.Exists(path))
            {
                return $"{path} does not exist";
            }
        }

        switch (target.Kind)
        {
            case ExternalTargetKind.Process or ExternalTargetKind.Command:
                if (string.IsNullOrWhiteSpace(target.Command))
                {
                    return "the catalog entry has no command";
                }

                string command = placeholders.ResolveCommand(target.Command);
                if (Path.IsPathRooted(command) ? !File.Exists(command) : ExecutableLocator.Find(command) is null)
                {
                    return $"'{command}' was not found";
                }

                return null;

            case ExternalTargetKind.Container:
                if (string.IsNullOrWhiteSpace(target.Image))
                {
                    return "the catalog entry has no image";
                }

                return ContainerRuntime.Current is null ? "no container runtime (docker or podman) is running" : null;

            default:
                return $"unknown kind {target.Kind}";
        }
    }

    // "node" probes `node --version`; "nginx -v" probes `nginx -v`. The probe rejects Windows' app-execution-alias
    // stubs (python3 opening the Store) that are on PATH but do not run.
    private static bool Runs(string requirement) => ProbedExecutables.GetOrAdd(requirement, static value =>
    {
        string[] parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (ExecutableLocator.Find(parts[0]) is null)
        {
            return false;
        }

        try
        {
            string[] arguments = parts.Length > 1 ? parts[1..] : ["--version"];
            return ExternalCommand.Run(parts[0], arguments, timeout: TimeSpan.FromSeconds(15), throwOnFailure: false) == 0;
        }
        catch (Exception)
        {
            return false;
        }
    });
}
