using System;
using System.Collections.Generic;
using System.IO;

using Assimalign.Cohesion.Web.Benchmarks.Runner.Servers;

namespace Assimalign.Cohesion.Web.Benchmarks.Runner;

internal sealed class RunnerOptions
{
    public const string Usage = """
        web-bench [options]

          --cohesion <exe>        Cohesion server apphost (default: artifacts/publish/cohesion/cohesion-server)
          --aspnetcore <exe>      ASP.NET Core server apphost (default: artifacts/publish/aspnetcore/aspnetcore-server)
          --frameworks <list>     cohesion,aspnetcore (default: both)
          --scenarios <list>      all | core | feature | comma-separated ids (default: all)
          --connections <n>       keep-alive connections (default: 64)
          --warmup <seconds>      unmeasured warmup per run (default: 5)
          --duration <seconds>    measured phase per run (default: 10)
          --iterations <n>        runs per scenario and framework; the report uses the median (default: 1)
          --server-cores <n>      cores pinned to the server; the rest go to the load generator (default: a third)
          --no-affinity           do not pin processes to cores
          --gc <mode>             server | workstation | default: GC mode forced on both servers (default: server)
          --output <dir>          results directory (default: results/<timestamp>)
          --label <text>          free-form label recorded in the report (machine, power plan, ...)
          --cohesion-commit <sha> cohesion commit the packages were built from, recorded in the report
          --validate-only         start each server, check its response, record the head; no load
          --list                  list the scenarios
        """;

    public string Scenarios { get; private set; } = "all";

    public List<string> Frameworks { get; } = ["cohesion", "aspnetcore"];

    public Dictionary<string, string> Executables { get; } = new(StringComparer.Ordinal);

    public int Connections { get; private set; } = 64;

    public TimeSpan Warmup { get; private set; } = TimeSpan.FromSeconds(5);

    public TimeSpan Duration { get; private set; } = TimeSpan.FromSeconds(10);

    public TimeSpan Cooldown { get; private set; } = TimeSpan.FromSeconds(1);

    public int Iterations { get; private set; } = 1;

    public int? ServerCores { get; private set; }

    public bool Affinity { get; private set; } = true;

    public GcMode Gc { get; private set; } = GcMode.Server;

    public string Output { get; private set; } = string.Empty;

    public string? Label { get; private set; }

    public string? CohesionCommit { get; private set; }

    public bool ValidateOnly { get; private set; }

    public bool List { get; private set; }

    public bool ShowHelp { get; private set; }

    public static RunnerOptions Parse(string[] args)
    {
        var options = new RunnerOptions();
        string? cohesion = null, aspnetcore = null;
        for (int i = 0; i < args.Length; i++)
        {
            string Value() => i + 1 < args.Length ? args[++i] : throw new ArgumentException($"{args[i]} needs a value.");
            switch (args[i])
            {
                case "--cohesion": cohesion = Value(); break;
                case "--aspnetcore": aspnetcore = Value(); break;
                case "--frameworks":
                    options.Frameworks.Clear();
                    options.Frameworks.AddRange(Value().Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
                    break;
                case "--scenarios": options.Scenarios = Value(); break;
                case "--connections": options.Connections = int.Parse(Value()); break;
                case "--warmup": options.Warmup = TimeSpan.FromSeconds(double.Parse(Value())); break;
                case "--duration": options.Duration = TimeSpan.FromSeconds(double.Parse(Value())); break;
                case "--cooldown": options.Cooldown = TimeSpan.FromSeconds(double.Parse(Value())); break;
                case "--iterations": options.Iterations = int.Parse(Value()); break;
                case "--server-cores": options.ServerCores = int.Parse(Value()); break;
                case "--no-affinity": options.Affinity = false; break;
                case "--gc": options.Gc = Enum.Parse<GcMode>(Value(), ignoreCase: true); break;
                case "--output": options.Output = Value(); break;
                case "--label": options.Label = Value(); break;
                case "--cohesion-commit": options.CohesionCommit = Value(); break;
                case "--validate-only": options.ValidateOnly = true; break;
                case "--list": options.List = true; break;
                case "-h" or "--help": options.ShowHelp = true; break;
                default: throw new ArgumentException($"Unknown option '{args[i]}'.");
            }
        }

        string root = FindBenchmarksRoot();
        string extension = OperatingSystem.IsWindows() ? ".exe" : string.Empty;
        options.Executables["cohesion"] = Path.GetFullPath(cohesion ?? Path.Combine(root, "artifacts", "publish", "cohesion", "cohesion-server" + extension));
        options.Executables["aspnetcore"] = Path.GetFullPath(aspnetcore ?? Path.Combine(root, "artifacts", "publish", "aspnetcore", "aspnetcore-server" + extension));

        foreach (string framework in options.Frameworks)
        {
            if (!options.Executables.TryGetValue(framework, out string? executable))
            {
                throw new ArgumentException($"Unknown framework '{framework}'.");
            }
            if (!options.ShowHelp && !options.List && !File.Exists(executable))
            {
                throw new ArgumentException($"The {framework} server was not found at {executable}. Run ./run.ps1, or publish it and pass --{framework}.");
            }
        }

        if (string.IsNullOrEmpty(options.Output))
        {
            options.Output = Path.Combine(root, "results", DateTime.Now.ToString("yyyyMMdd-HHmmss"));
        }

        return options;
    }

    /// <summary>The benchmarks/Web folder: the nearest ancestor of the runner that holds global.json.</summary>
    private static string FindBenchmarksRoot()
    {
        for (DirectoryInfo? directory = new(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "global.json")) && Directory.Exists(Path.Combine(directory.FullName, "shared")))
            {
                return directory.FullName;
            }
        }

        return Directory.GetCurrentDirectory();
    }
}
