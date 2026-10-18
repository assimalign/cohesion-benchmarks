using System;
using System.IO;

namespace Assimalign.Cohesion.Benchmarks.External;

/// <summary>Finds an executable on PATH the way a shell would.</summary>
internal static class ExecutableLocator
{
    /// <returns>The full path, or null.</returns>
    public static string? Find(string name)
    {
        if (Path.IsPathRooted(name))
        {
            return File.Exists(name) ? name : null;
        }

        string[] extensions = OperatingSystem.IsWindows()
            ? [string.Empty, .. (Environment.GetEnvironmentVariable("PATHEXT") ?? ".EXE;.CMD;.BAT").Split(';', StringSplitOptions.RemoveEmptyEntries)]
            : [string.Empty];

        foreach (string directory in (Environment.GetEnvironmentVariable("PATH") ?? string.Empty).Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
        {
            foreach (string extension in extensions)
            {
                string candidate = Path.Combine(directory.Trim('"'), name + extension);
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }
        }

        return null;
    }
}
