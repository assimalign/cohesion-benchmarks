using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assimalign.Cohesion.Benchmarks.External;

/// <summary>Runs a process to completion. The unit of work for <see cref="ExternalTargetKind.Command"/> benchmarks.</summary>
public static class ExternalCommand
{
    /// <summary>Runs <paramref name="fileName"/> and waits for it to exit.</summary>
    /// <param name="fileName">The executable, a name on PATH or a full path.</param>
    /// <param name="arguments">The arguments, passed without shell interpretation.</param>
    /// <param name="workingDirectory">The working directory, or null for the current one.</param>
    /// <param name="environment">Extra environment variables, or null.</param>
    /// <param name="timeout">How long the run may take before it is killed; 60 seconds by default.</param>
    /// <param name="throwOnFailure">Throw when the exit code is not 0 (with the tail of its output).</param>
    /// <param name="onOutputLine">Receives each line written to standard output or error, or null.</param>
    /// <returns>The exit code.</returns>
    public static int Run(
        string fileName,
        IEnumerable<string> arguments,
        string? workingDirectory = null,
        IReadOnlyDictionary<string, string>? environment = null,
        TimeSpan? timeout = null,
        bool throwOnFailure = true,
        Action<string>? onOutputLine = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentNullException.ThrowIfNull(arguments);

        ProcessStartInfo start = new(fileName)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };

        foreach (string argument in arguments)
        {
            start.ArgumentList.Add(argument);
        }

        if (workingDirectory is not null)
        {
            start.WorkingDirectory = workingDirectory;
        }

        if (environment is not null)
        {
            foreach ((string key, string value) in environment)
            {
                start.Environment[key] = value;
            }
        }

        OutputTail output = new(lines: 40);
        using Process process = new() { StartInfo = start };
        process.OutputDataReceived += (_, e) => Receive(e.Data);
        process.ErrorDataReceived += (_, e) => Receive(e.Data);
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        TimeSpan limit = timeout ?? TimeSpan.FromSeconds(60);
        if (!process.WaitForExit(limit))
        {
            try
            {
                process.Kill(entireProcessTree: true);
            }
            catch (InvalidOperationException)
            {
                // Exited between the timeout and the kill.
            }

            throw new TimeoutException($"'{fileName}' did not exit within {limit}.{output.Format()}");
        }

        process.WaitForExit(); // flush the asynchronous output readers
        if (throwOnFailure && process.ExitCode != 0)
        {
            throw new InvalidOperationException($"'{fileName}' exited with {process.ExitCode}.{output.Format()}");
        }

        return process.ExitCode;

        void Receive(string? line)
        {
            output.Add(line);
            if (line is not null)
            {
                onOutputLine?.Invoke(line);
            }
        }
    }
}
