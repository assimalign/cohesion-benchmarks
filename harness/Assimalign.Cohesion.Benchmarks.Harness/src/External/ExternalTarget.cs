using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Assimalign.Cohesion.Benchmarks.External;

/// <summary>How an external target runs.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<ExternalTargetKind>))]
public enum ExternalTargetKind
{
    /// <summary>A long-running local process (a server) that benchmarks talk to.</summary>
    Process,

    /// <summary>A long-running container (docker or podman) that benchmarks talk to.</summary>
    Container,

    /// <summary>A command that runs to completion; the benchmark measures one run.</summary>
    Command,
}

/// <summary>
/// One application in an area's <c>targets/catalog.json</c>: something that is not code in the benchmark process
/// (a non-.NET server, a container, a published .NET app, a command-line tool) that benchmarks start and measure.
/// </summary>
/// <remarks>
/// The field names follow the database suite's engine catalog (id, kind, image, ports, environment, arguments,
/// readiness, endpointTemplate). String values may use placeholders, resolved at start:
/// <c>{host}</c> (127.0.0.1), <c>{port}</c> (the first published port, a free port when the catalog says 0),
/// <c>{port:N}</c> (the published port for internal port N), <c>{repoRoot}</c>, <c>{cohesionVersion}</c> (the measured cohesion package version),
/// <c>{catalogDir}</c>, and <c>{exe}</c> (".exe" on Windows, empty elsewhere).
/// </remarks>
public sealed class ExternalTarget
{
    /// <summary>The identifier benchmarks select the target by (also the <c>[Params]</c> value).</summary>
    public required string Id { get; init; }

    /// <summary>How the target runs.</summary>
    public ExternalTargetKind Kind { get; init; } = ExternalTargetKind.Process;

    /// <summary>What the target is, for reports.</summary>
    public string? Description { get; init; }

    /// <summary>For processes and commands: the executable, a name on PATH or a path (placeholders allowed).</summary>
    public string? Command { get; init; }

    /// <summary>Arguments for the process, the command, or the container entry point.</summary>
    public IReadOnlyList<string> Arguments { get; init; } = [];

    /// <summary>The working directory for processes and commands.</summary>
    public string? WorkingDirectory { get; init; }

    /// <summary>For containers: the image, fully qualified (docker.io/library/nginx:1.27-alpine).</summary>
    public string? Image { get; init; }

    /// <summary>Ports. For processes, <c>internal</c> is ignored and <c>published</c> 0 allocates a free port.</summary>
    public IReadOnlyList<ExternalTargetPort> Ports { get; init; } = [];

    /// <summary>Environment variables for the process or container.</summary>
    public IReadOnlyDictionary<string, string> Environment { get; init; } = new Dictionary<string, string>();

    /// <summary>
    /// For containers: the CPU budget (<c>--cpus</c>). Unset leaves the container unlimited.
    /// </summary>
    /// <remarks>
    /// Comparing two engines is only meaningful when both get the same hardware, so an area that compares
    /// server products gives every target the same budget and records it with the results.
    /// </remarks>
    public double? Cpus { get; init; }

    /// <summary>
    /// For containers: the memory budget (<c>--memory</c>), for example <c>6g</c>. Swap is pinned to the same
    /// value, so a target that exceeds its budget fails instead of quietly swapping.
    /// </summary>
    public string? Memory { get; init; }

    /// <summary>
    /// For containers: volume mounts, each <c>source:/path/inside</c> (a named volume, or a host path with
    /// placeholders). Database engines keep their data files in named volumes rather than the container layer.
    /// </summary>
    public IReadOnlyList<string> Volumes { get; init; } = [];

    /// <summary>How to tell the target is ready. Without one, a target with a port waits for a TCP connection.</summary>
    public ExternalTargetReadiness? Readiness { get; init; }

    /// <summary>The address benchmarks connect to, e.g. <c>http://{host}:{port}/</c>.</summary>
    public string? EndpointTemplate { get; init; }

    /// <summary>Operating systems the target runs on (<c>windows</c>, <c>linux</c>, <c>osx</c>); empty means any.</summary>
    public IReadOnlyList<string> Platforms { get; init; } = [];

    /// <summary>Executables that must be on PATH (for example <c>node</c>); otherwise the target is skipped.</summary>
    public IReadOnlyList<string> Requires { get; init; } = [];

    /// <summary>Files that must exist (placeholders allowed), for example a published server; otherwise skipped.</summary>
    public IReadOnlyList<string> RequiresFiles { get; init; } = [];
}

/// <summary>A port mapping.</summary>
public sealed class ExternalTargetPort
{
    /// <summary>The port inside the container. Ignored for processes.</summary>
    public int Internal { get; init; }

    /// <summary>The port on 127.0.0.1; 0 picks a free one.</summary>
    public int Published { get; init; }
}

/// <summary>A readiness probe.</summary>
public sealed class ExternalTargetReadiness
{
    /// <summary><c>tcp</c>, <c>http</c>, <c>log</c>, <c>exec</c>, or <c>none</c>.</summary>
    public string Kind { get; init; } = "tcp";

    /// <summary>For <c>http</c>: the path requested on the endpoint; any status below 500 is ready.</summary>
    public string? Path { get; init; }

    /// <summary>For <c>log</c>: a regular expression matched against standard output and error.</summary>
    public string? Pattern { get; init; }

    /// <summary>For <c>exec</c>: a command that exits 0 once ready (run inside the container for containers).</summary>
    public IReadOnlyList<string> Command { get; init; } = [];

    /// <summary>How long to wait before giving up.</summary>
    public double TimeoutSeconds { get; init; } = 60;
}
