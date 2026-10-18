using System;

namespace Assimalign.Cohesion.Benchmarks.External;

/// <summary>
/// The container runtime for <see cref="ExternalTargetKind.Container"/> targets: the
/// <c>COHESION_BENCHMARK_CONTAINER_RUNTIME</c> environment variable if set, otherwise docker, then podman — the first
/// whose <c>info</c> succeeds (so an installed runtime whose daemon or machine is stopped does not count).
/// </summary>
public static class ContainerRuntime
{
    private static readonly Lazy<string?> Detected = new(Detect);

    /// <summary>The runtime executable (<c>docker</c> or <c>podman</c>), or null when none is running.</summary>
    public static string? Current => Detected.Value;

    private static string? Detect()
    {
        string? configured = Environment.GetEnvironmentVariable("COHESION_BENCHMARK_CONTAINER_RUNTIME");
        foreach (string candidate in configured is { Length: > 0 } ? new[] { configured } : new[] { "docker", "podman" })
        {
            if (ExecutableLocator.Find(candidate) is null)
            {
                continue;
            }

            try
            {
                if (ExternalCommand.Run(candidate, ["info"], timeout: TimeSpan.FromSeconds(30), throwOnFailure: false) == 0)
                {
                    return candidate;
                }
            }
            catch (Exception)
            {
                // Not usable; try the next one.
            }
        }

        return null;
    }
}
