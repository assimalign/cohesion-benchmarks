using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Web.Benchmarks.Runner.Servers;

internal enum GcMode
{
    Server,
    Workstation,
    Default,
}

/// <summary>
/// One server process for one scenario. The runner owns its environment: the GC mode, the processor
/// count the runtime sizes itself for, and the cores it may run on, identically for both frameworks.
/// </summary>
internal sealed class ServerProcess : IDisposable
{
    private readonly Process _process;
    private readonly ConcurrentQueue<string> _output = new();
    private readonly TaskCompletionSource<JsonElement> _info = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly long _startedAt;

    private ServerProcess(Process process, long startedAt)
    {
        _process = process;
        _startedAt = startedAt;
    }

    public int ProcessId => _process.Id;

    public string RecentOutput => string.Join(Environment.NewLine, _output);

    public static ServerProcess Start(string executable, string scenario, int port, int serverCores, long affinityMask, GcMode gc)
    {
        var start = new ProcessStartInfo(executable)
        {
            WorkingDirectory = Path.GetDirectoryName(executable)!,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        start.ArgumentList.Add("--scenario");
        start.ArgumentList.Add(scenario);
        start.ArgumentList.Add("--port");
        start.ArgumentList.Add(port.ToString());

        // Production everywhere: no developer exception pages, no launch-profile environments.
        start.Environment["COHESION_ENVIRONMENT"] = "Production";
        start.Environment["ASPNETCORE_ENVIRONMENT"] = "Production";
        start.Environment["DOTNET_ENVIRONMENT"] = "Production";

        // The runtime sizes its thread pool, GC heaps and transport queues from the processor count it
        // sees at startup, before the affinity mask below is applied, so tell it the same number.
        start.Environment["DOTNET_PROCESSOR_COUNT"] = serverCores.ToString();
        switch (gc)
        {
            case GcMode.Server:
                start.Environment["DOTNET_gcServer"] = "1";
                start.Environment["DOTNET_GCHeapCount"] = serverCores.ToString("X"); // hexadecimal by contract
                start.Environment["DOTNET_GCNoAffinitize"] = "1";
                break;
            case GcMode.Workstation:
                start.Environment["DOTNET_gcServer"] = "0";
                break;
        }

        long startedAt = Stopwatch.GetTimestamp();
        Process process = Process.Start(start) ?? throw new InvalidOperationException($"Could not start {executable}.");
        var server = new ServerProcess(process, startedAt);

        if (affinityMask != 0 && (OperatingSystem.IsWindows() || OperatingSystem.IsLinux()))
        {
            process.ProcessorAffinity = (nint)affinityMask;
        }

        process.OutputDataReceived += (_, line) => server.OnOutput(line.Data);
        process.ErrorDataReceived += (_, line) => server.OnOutput(line.Data);
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        return server;
    }

    /// <summary>
    /// Waits until the server answers a request with any HTTP response and returns the time from process
    /// start to that response: runtime start, composition, bind and the first request's cold path.
    /// </summary>
    public async Task<double> WaitUntilReadyAsync(IPEndPoint endpoint, byte[] probe, TimeSpan timeout)
    {
        using var cancellation = new CancellationTokenSource(timeout);
        while (true)
        {
            if (_process.HasExited)
            {
                throw new InvalidOperationException($"The server exited with code {_process.ExitCode} before it answered.{Environment.NewLine}{RecentOutput}");
            }

            try
            {
                using var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
                await socket.ConnectAsync(endpoint, cancellation.Token);
                await socket.SendAsync(probe, SocketFlags.None, cancellation.Token);
                byte[] buffer = new byte[16];
                int received = await socket.ReceiveAsync(buffer, SocketFlags.None, cancellation.Token);
                if (received >= 12 && buffer.AsSpan().StartsWith("HTTP/1."u8))
                {
                    return Stopwatch.GetElapsedTime(_startedAt).TotalMilliseconds;
                }
            }
            catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
            {
                throw new TimeoutException($"The server did not answer within {timeout.TotalSeconds:0}s.{Environment.NewLine}{RecentOutput}");
            }
            catch (SocketException)
            {
            }

            await Task.Delay(10);
        }
    }

    public async Task<JsonElement?> GetInfoAsync(TimeSpan timeout)
    {
        Task completed = await Task.WhenAny(_info.Task, Task.Delay(timeout));
        return completed == _info.Task ? _info.Task.Result : null;
    }

    public TimeSpan TotalProcessorTime
    {
        get
        {
            _process.Refresh();
            return _process.TotalProcessorTime;
        }
    }

    public (long WorkingSet, long PeakWorkingSet, long PrivateBytes) Memory
    {
        get
        {
            _process.Refresh();
            return (_process.WorkingSet64, _process.PeakWorkingSet64, _process.PrivateMemorySize64);
        }
    }

    public void Dispose()
    {
        try
        {
            if (!_process.HasExited)
            {
                _process.Kill(entireProcessTree: true);
                _process.WaitForExit(10_000);
            }
        }
        catch (InvalidOperationException)
        {
        }
        _process.Dispose();
    }

    private void OnOutput(string? line)
    {
        if (line is null)
        {
            return;
        }

        if (line.StartsWith(BenchmarkHost.InfoPrefix, StringComparison.Ordinal))
        {
            try
            {
                _info.TrySetResult(JsonDocument.Parse(line[BenchmarkHost.InfoPrefix.Length..]).RootElement.Clone());
            }
            catch (JsonException)
            {
            }
            return;
        }

        _output.Enqueue(line);
        while (_output.Count > 40 && _output.TryDequeue(out _))
        {
        }
    }

    public static int GetFreePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }
}
