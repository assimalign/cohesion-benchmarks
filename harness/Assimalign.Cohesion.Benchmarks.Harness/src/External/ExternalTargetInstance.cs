using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Benchmarks.External;

/// <summary>
/// A running process or container target. Start it in <c>[GlobalSetup]</c> (or per invocation for startup
/// benchmarks) and dispose it in <c>[GlobalCleanup]</c>; disposal kills the process tree or removes the container.
/// </summary>
public sealed class ExternalTargetInstance : IDisposable
{
    private readonly Process? _process;
    private readonly string? _containerName;
    private readonly OutputTail _output;
    private int _disposed;

    private ExternalTargetInstance(ExternalTarget target, Process? process, string? containerName, OutputTail output, IReadOnlyList<(int Internal, int Published)> ports, Uri? endpoint)
    {
        Target = target;
        _process = process;
        _containerName = containerName;
        _output = output;
        Ports = ports;
        Endpoint = endpoint;
    }

    /// <summary>The catalog entry.</summary>
    public ExternalTarget Target { get; }

    /// <summary>The host the target listens on.</summary>
    public string Host => Placeholders.Host;

    /// <summary>The first published port, or 0 when the target publishes none.</summary>
    public int Port => Ports.Count > 0 ? Ports[0].Published : 0;

    /// <summary>The port mappings (internal, published).</summary>
    public IReadOnlyList<(int Internal, int Published)> Ports { get; }

    /// <summary>The resolved <see cref="ExternalTarget.EndpointTemplate"/>, if the entry has one.</summary>
    public Uri? Endpoint { get; }

    /// <summary>The address to connect a socket to.</summary>
    public IPEndPoint IPEndPoint => new(IPAddress.Loopback, Port);

    /// <summary>How long the target took from launch to ready.</summary>
    public TimeSpan StartupTime { get; private set; }

    internal static ExternalTargetInstance Start(ExternalTarget target, ExternalTargetCatalog catalog)
    {
        List<(int Internal, int Published)> ports = target.Ports
            .Select(p => (p.Internal, p.Published > 0 ? p.Published : FreePort()))
            .ToList();
        Placeholders placeholders = new(catalog, ports);
        Uri? endpoint = target.EndpointTemplate is null ? null : new Uri(placeholders.Resolve(target.EndpointTemplate));
        Dictionary<string, string> environment = target.Environment.ToDictionary(e => e.Key, e => placeholders.Resolve(e.Value));
        OutputTail output = new(lines: 200);

        Stopwatch clock = Stopwatch.StartNew();
        ExternalTargetInstance instance = target.Kind switch
        {
            ExternalTargetKind.Process => new(target, StartProcess(target, placeholders, environment, output), null, output, ports, endpoint),
            ExternalTargetKind.Container => new(target, null, StartContainer(target, placeholders, environment, ports, output), output, ports, endpoint),
            _ => throw new InvalidOperationException($"External target '{target.Id}' is a {target.Kind}; run it with ExternalTargetCatalog.Run."),
        };

        try
        {
            instance.WaitUntilReady(placeholders);
            instance.StartupTime = clock.Elapsed;
            return instance;
        }
        catch
        {
            instance.Dispose();
            throw;
        }
    }

    /// <summary>Stops the target.</summary>
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        if (_process is not null)
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
                // Already gone.
            }

            _process.Dispose();
        }

        if (_containerName is not null && ContainerRuntime.Current is { } runtime)
        {
            ExternalCommand.Run(runtime, ["rm", "--force", _containerName], timeout: TimeSpan.FromSeconds(60), throwOnFailure: false);
        }
    }

    private static Process StartProcess(ExternalTarget target, Placeholders placeholders, Dictionary<string, string> environment, OutputTail output)
    {
        ProcessStartInfo start = new(placeholders.ResolveCommand(target.Command!))
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WorkingDirectory = target.WorkingDirectory is null ? string.Empty : placeholders.ResolvePath(target.WorkingDirectory),
        };

        foreach (string argument in target.Arguments)
        {
            start.ArgumentList.Add(placeholders.Resolve(argument));
        }

        foreach ((string key, string value) in environment)
        {
            start.Environment[key] = value;
        }

        Process process = new() { StartInfo = start };
        process.OutputDataReceived += (_, e) => output.Add(e.Data);
        process.ErrorDataReceived += (_, e) => output.Add(e.Data);
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        return process;
    }

    private static string StartContainer(ExternalTarget target, Placeholders placeholders, Dictionary<string, string> environment, List<(int Internal, int Published)> ports, OutputTail output)
    {
        string runtime = ContainerRuntime.Current ?? throw new InvalidOperationException("No container runtime is running.");
        string name = $"cohbench-{target.Id}-{Guid.NewGuid():N}"[..40];

        List<string> arguments = ["run", "--detach", "--rm", "--name", name];
        foreach ((int inside, int published) in ports)
        {
            arguments.AddRange(["--publish", $"{Placeholders.Host}:{published}:{inside}"]);
        }

        foreach ((string key, string value) in environment)
        {
            arguments.AddRange(["--env", $"{key}={value}"]);
        }

        arguments.Add(target.Image!);
        arguments.AddRange(target.Arguments.Select(placeholders.Resolve));

        // Pulling the image is part of starting it the first time; allow for it.
        ExternalCommand.Run(runtime, arguments, timeout: TimeSpan.FromMinutes(10));
        output.Add($"started container {name} ({target.Image})");
        return name;
    }

    private void WaitUntilReady(Placeholders placeholders)
    {
        ExternalTargetReadiness readiness = Target.Readiness ?? new ExternalTargetReadiness { Kind = Ports.Count > 0 ? "tcp" : "none" };
        TimeSpan timeout = TimeSpan.FromSeconds(readiness.TimeoutSeconds);
        using CancellationTokenSource deadline = new(timeout);

        Task logMatched = readiness.Kind == "log"
            ? _output.WaitForAsync(readiness.Pattern ?? throw new InvalidOperationException($"'{Target.Id}': a log probe needs a pattern."))
            : Task.CompletedTask;

        while (true)
        {
            if (_process is { HasExited: true })
            {
                throw new InvalidOperationException($"External target '{Target.Id}' exited with {_process.ExitCode} before it was ready.{_output.Format()}");
            }

            if (Probe(readiness, logMatched, placeholders))
            {
                return;
            }

            if (deadline.IsCancellationRequested)
            {
                throw new TimeoutException($"External target '{Target.Id}' was not ready within {timeout} ({readiness.Kind} probe).{_output.Format()}{ContainerLogs()}");
            }

            Thread.Sleep(50);
        }
    }

    private bool Probe(ExternalTargetReadiness readiness, Task logMatched, Placeholders placeholders)
    {
        try
        {
            switch (readiness.Kind)
            {
                case "none":
                    return true;

                case "tcp":
                    using (TcpClient client = new())
                    {
                        return client.ConnectAsync(IPAddress.Loopback, Port).Wait(TimeSpan.FromSeconds(1)) && client.Connected;
                    }

                case "http":
                    Uri address = new(Endpoint ?? throw new InvalidOperationException($"'{Target.Id}': an http probe needs an endpointTemplate."), readiness.Path ?? "/");
                    using (HttpClient http = new() { Timeout = TimeSpan.FromSeconds(2) })
                    {
                        return (int)http.GetAsync(address).GetAwaiter().GetResult().StatusCode < 500;
                    }

                case "log":
                    if (_containerName is not null && ContainerRuntime.Current is { } runtime)
                    {
                        // A container writes to its own log, not to this process: read it back each poll.
                        Regex pattern = new(readiness.Pattern!, RegexOptions.CultureInvariant);
                        bool seen = false;
                        ExternalCommand.Run(runtime, ["logs", _containerName], timeout: TimeSpan.FromSeconds(10), throwOnFailure: false,
                            onOutputLine: line => seen |= pattern.IsMatch(line));
                        return seen;
                    }

                    return logMatched.IsCompleted;

                case "exec":
                    List<string> command = readiness.Command.Select(placeholders.Resolve).ToList();
                    if (command.Count == 0)
                    {
                        throw new InvalidOperationException($"'{Target.Id}': an exec probe needs a command.");
                    }

                    return _containerName is not null && ContainerRuntime.Current is { } execRuntime
                        ? ExternalCommand.Run(execRuntime, ["exec", _containerName, .. command], timeout: TimeSpan.FromSeconds(10), throwOnFailure: false) == 0
                        : ExternalCommand.Run(command[0], command.Skip(1), timeout: TimeSpan.FromSeconds(10), throwOnFailure: false) == 0;

                default:
                    throw new InvalidOperationException($"'{Target.Id}': unknown readiness kind '{readiness.Kind}'.");
            }
        }
        catch (Exception exception) when (exception is SocketException or HttpRequestException or TaskCanceledException or AggregateException or TimeoutException)
        {
            return false;
        }
    }

    private string ContainerLogs()
    {
        if (_containerName is null || ContainerRuntime.Current is not { } runtime)
        {
            return string.Empty;
        }

        OutputTail logs = new(lines: 40);
        try
        {
            ExternalCommand.Run(runtime, ["logs", "--tail", "40", _containerName], timeout: TimeSpan.FromSeconds(10), throwOnFailure: false,
                onOutputLine: logs.Add);
        }
        catch (Exception exception)
        {
            logs.Add(exception.Message);
        }

        return logs.Format();
    }

    private static int FreePort()
    {
        using TcpListener listener = new(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }
}
