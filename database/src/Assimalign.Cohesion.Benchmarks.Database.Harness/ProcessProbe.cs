using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Bounded, shell-free diagnostic commands. Missing utilities do not prevent development runs.</summary>
public static class ProcessProbe
{
    public static async Task<string?> ReadAsync(string executable, string[] arguments, CancellationToken token = default)
    {
        try
        {
            using var process = new Process { StartInfo = new ProcessStartInfo(executable)
                { RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true } };
            foreach (string argument in arguments) process.StartInfo.ArgumentList.Add(argument);
            process.Start();
            Task<string> output = process.StandardOutput.ReadToEndAsync(token);
            Task<string> error = process.StandardError.ReadToEndAsync(token);
            using var deadline = CancellationTokenSource.CreateLinkedTokenSource(token);
            deadline.CancelAfter(TimeSpan.FromSeconds(4));
            try { await process.WaitForExitAsync(deadline.Token).ConfigureAwait(false); }
            catch (OperationCanceledException) { process.Kill(true); await process.WaitForExitAsync(CancellationToken.None).ConfigureAwait(false); throw; }
            await error.ConfigureAwait(false);
            return process.ExitCode == 0 ? (await output.ConfigureAwait(false)).Trim() : null;
        }
        catch (Exception ex) when (ex is not OutOfMemoryException && !token.IsCancellationRequested) { return null; }
    }
}
