using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

using Assimalign.Cohesion.Web.Benchmarks.Runner.Load;
using Assimalign.Cohesion.Web.Benchmarks.Runner.Scenarios;

namespace Assimalign.Cohesion.Web.Benchmarks.Runner.Reporting;

internal sealed class RunRecord(string scenario, string framework, int iteration)
{
    public string Scenario { get; } = scenario;
    public string Framework { get; } = framework;
    public int Iteration { get; } = iteration;
    public bool Passed { get; set; }
    public string? Failure { get; set; }
    public double? RequestsPerSecond { get; set; }
    public long? Requests { get; set; }
    public double? ElapsedSeconds { get; set; }
    public long? Errors { get; set; }
    public long? Reconnects { get; set; }
    public long[]? StatusClasses { get; set; }
    public double? MegabytesPerSecond { get; set; }
    public LatencySummary? Latency { get; set; }
    public double? ServerCpuPercent { get; set; }
    public double? ClientCpuPercent { get; set; }
    public double? RequestsPerCpuSecond { get; set; }
    public double? StartupMilliseconds { get; set; }
    public double? WorkingSetMegabytes { get; set; }
    public double? PeakWorkingSetMegabytes { get; set; }
    public double? PrivateMegabytes { get; set; }
    public ValidationSummary? Validation { get; set; }
    public JsonElement? ServerInfo { get; set; }
}

internal sealed record LatencySummary(double Mean, double P50, double P75, double P90, double P99, double P999, double Max)
{
    public static LatencySummary From(LatencyHistogram histogram) => new(
        histogram.MeanMilliseconds,
        histogram.PercentileMilliseconds(50),
        histogram.PercentileMilliseconds(75),
        histogram.PercentileMilliseconds(90),
        histogram.PercentileMilliseconds(99),
        histogram.PercentileMilliseconds(99.9),
        histogram.MaxMilliseconds);
}

internal sealed record ValidationSummary(int Status, int HeadBytes, long BodyBytes, string Head, string BodyPreview);

internal sealed record ScenarioInfo(string Id, string Server, string Category, string Description, int PipelineDepth, string Kind, string? Note)
{
    public static ScenarioInfo From(LoadScenario scenario) =>
        new(scenario.Id, scenario.Server, scenario.Category, scenario.Description, scenario.PipelineDepth, scenario.Kind.ToString(), scenario.Note);
}

internal sealed record EnvironmentInfo(
    string Timestamp,
    string MachineName,
    string Cpu,
    int LogicalProcessors,
    int ServerCores,
    int ClientCores,
    string Os,
    string Architecture,
    string RunnerRuntime,
    double TotalMemoryGigabytes,
    int Connections,
    double WarmupSeconds,
    double DurationSeconds,
    int Iterations,
    string GcMode,
    bool Affinity,
    string? Label,
    string? CohesionCommit)
{
    public static EnvironmentInfo Capture(int serverCores, int clientCores, RunnerOptions options) => new(
        DateTimeOffset.Now.ToString("O"),
        Environment.MachineName,
        ReadCpuName(),
        Environment.ProcessorCount,
        serverCores,
        clientCores,
        RuntimeInformation.OSDescription,
        RuntimeInformation.OSArchitecture.ToString(),
        RuntimeInformation.FrameworkDescription,
        Math.Round(GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / (1024.0 * 1024 * 1024), 1),
        options.Connections,
        options.Warmup.TotalSeconds,
        options.Duration.TotalSeconds,
        options.Iterations,
        options.Gc.ToString(),
        options.Affinity,
        options.Label,
        options.CohesionCommit);

    private static string ReadCpuName()
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                using Microsoft.Win32.RegistryKey? key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
                if (key?.GetValue("ProcessorNameString") is string name)
                {
                    return name.Trim();
                }
            }
            else if (File.Exists("/proc/cpuinfo"))
            {
                string? line = File.ReadLines("/proc/cpuinfo").FirstOrDefault(l => l.StartsWith("model name", StringComparison.Ordinal));
                if (line is not null)
                {
                    return line[(line.IndexOf(':') + 1)..].Trim();
                }
            }
        }
        catch
        {
        }

        return RuntimeInformation.ProcessArchitecture.ToString();
    }
}

internal sealed record RunReport(EnvironmentInfo Environment, List<RunRecord> Runs, List<ScenarioInfo> Scenarios);

internal static class ReportJson
{
    public static JsonSerializerOptions Options { get; } = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
    };
}
