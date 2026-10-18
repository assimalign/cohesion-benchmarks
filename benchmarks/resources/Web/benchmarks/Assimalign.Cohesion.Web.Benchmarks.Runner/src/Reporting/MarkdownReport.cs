using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assimalign.Cohesion.Web.Benchmarks.Runner.Reporting;

/// <summary>
/// Renders the run as Markdown: medians across iterations, the Cohesion-to-ASP.NET Core ratio per
/// scenario, the efficiency view (requests per server CPU-second) that survives a client-bound run,
/// and the validation exchange each server answered, so parity can be checked from the report alone.
/// </summary>
internal static class MarkdownReport
{
    private const string Cohesion = "cohesion";
    private const string AspNetCore = "aspnetcore";

    /// <summary>Below this server CPU utilization the load generator, not the server, probably set the pace.</summary>
    private const double ClientBoundThreshold = 85;

    public static string Render(RunReport report, bool validateOnly)
    {
        var markdown = new StringBuilder();
        EnvironmentInfo environment = report.Environment;
        markdown.AppendLine("# Cohesion Web vs ASP.NET Core");
        markdown.AppendLine();
        markdown.AppendLine($"Run {environment.Timestamp}{(environment.Label is null ? string.Empty : $" ({environment.Label})")}.");
        markdown.AppendLine();
        markdown.AppendLine("| | |");
        markdown.AppendLine("| --- | --- |");
        markdown.AppendLine($"| Machine | {environment.Cpu}, {environment.LogicalProcessors} logical processors, {environment.TotalMemoryGigabytes} GB |");
        markdown.AppendLine($"| OS | {environment.Os} ({environment.Architecture}) |");
        markdown.AppendLine($"| Cores | server {environment.ServerCores}, load generator {environment.ClientCores}{(environment.Affinity ? ", pinned" : ", not pinned")} |");
        markdown.AppendLine($"| Load | {environment.Connections} keep-alive connections, {environment.WarmupSeconds:0}s warmup, {environment.DurationSeconds:0}s measured, {environment.Iterations} iteration(s), median reported |");
        markdown.AppendLine($"| GC | {environment.GcMode} (forced on both servers) |");
        foreach (string framework in new[] { Cohesion, AspNetCore })
        {
            RunRecord? sample = report.Runs.FirstOrDefault(run => run.Framework == framework && run.ServerInfo is not null);
            if (sample?.ServerInfo is { } info)
            {
                markdown.AppendLine($"| {Name(framework)} | {Str(info, "frameworkVersion")} on {Str(info, "runtime")}, server GC {Str(info, "serverGc")}, {Str(info, "processorCount")} processors, JIT {Str(info, "dynamicCode")} |");
            }
        }
        if (environment.CohesionCommit is not null)
        {
            markdown.AppendLine($"| Cohesion commit | `{environment.CohesionCommit}` |");
        }
        markdown.AppendLine();

        Dictionary<(string Scenario, string Framework), RunRecord> medians = Medians(report.Runs);
        List<ScenarioInfo> scenarios = report.Scenarios;

        if (!validateOnly)
        {
            RenderSummary(markdown, scenarios, medians);
            RenderThroughput(markdown, scenarios, medians);
            RenderEfficiency(markdown, scenarios, medians);
        }

        RenderParity(markdown, scenarios, medians);
        RenderFailures(markdown, report.Runs);
        RenderNotes(markdown, scenarios);
        return markdown.ToString();
    }

    private static void RenderSummary(StringBuilder markdown, List<ScenarioInfo> scenarios, Dictionary<(string, string), RunRecord> medians)
    {
        markdown.AppendLine("## Summary");
        markdown.AppendLine();
        markdown.AppendLine("Geometric mean of the per-scenario Cohesion ÷ ASP.NET Core ratio, over scenarios both frameworks passed. Above 1 favors Cohesion.");
        markdown.AppendLine();
        markdown.AppendLine("| Scenarios | Count | Throughput ratio | Efficiency ratio (req/CPU-s) | Cohesion faster in |");
        markdown.AppendLine("| --- | ---: | ---: | ---: | ---: |");
        foreach ((string label, Func<ScenarioInfo, bool> filter) in new (string, Func<ScenarioInfo, bool>)[]
        {
            ("Core request path", s => s.Category == "core"),
            ("Web packages", s => s.Category == "feature"),
            ("All", _ => true),
        })
        {
            var throughput = new List<double>();
            var efficiency = new List<double>();
            foreach (ScenarioInfo scenario in scenarios.Where(filter))
            {
                if (TryPair(medians, scenario.Id, out RunRecord cohesion, out RunRecord aspnet))
                {
                    throughput.Add(cohesion.RequestsPerSecond!.Value / aspnet.RequestsPerSecond!.Value);
                    if (cohesion.RequestsPerCpuSecond is > 0 && aspnet.RequestsPerCpuSecond is > 0)
                    {
                        efficiency.Add(cohesion.RequestsPerCpuSecond.Value / aspnet.RequestsPerCpuSecond.Value);
                    }
                }
            }
            markdown.AppendLine($"| {label} | {throughput.Count} | {GeoMean(throughput):0.00}× | {GeoMean(efficiency):0.00}× | {throughput.Count(r => r > 1)} of {throughput.Count} |");
        }
        markdown.AppendLine();
    }

    private static void RenderThroughput(StringBuilder markdown, List<ScenarioInfo> scenarios, Dictionary<(string, string), RunRecord> medians)
    {
        markdown.AppendLine("## Throughput and latency");
        markdown.AppendLine();
        markdown.AppendLine($"Requests per second over the measured phase (WebSocket: messages per second). † marks a server below {ClientBoundThreshold:0}% CPU, where the load generator may have set the pace; compare efficiency there instead.");
        markdown.AppendLine();
        markdown.AppendLine("| Scenario | Cohesion req/s | ASP.NET Core req/s | Ratio | Cohesion p50 / p99 (ms) | ASP.NET Core p50 / p99 (ms) |");
        markdown.AppendLine("| --- | ---: | ---: | ---: | ---: | ---: |");
        foreach (ScenarioInfo scenario in scenarios)
        {
            medians.TryGetValue((scenario.Id, Cohesion), out RunRecord? cohesion);
            medians.TryGetValue((scenario.Id, AspNetCore), out RunRecord? aspnet);
            string ratio = TryPair(medians, scenario.Id, out RunRecord c, out RunRecord a)
                ? $"{c.RequestsPerSecond!.Value / a.RequestsPerSecond!.Value:0.00}×"
                : "–";
            markdown.AppendLine($"| `{scenario.Id}` | {Rps(cohesion)} | {Rps(aspnet)} | {ratio} | {Latency(cohesion)} | {Latency(aspnet)} |");
        }
        markdown.AppendLine();
    }

    private static void RenderEfficiency(StringBuilder markdown, List<ScenarioInfo> scenarios, Dictionary<(string, string), RunRecord> medians)
    {
        markdown.AppendLine("## Efficiency and footprint");
        markdown.AppendLine();
        markdown.AppendLine("CPU is the server's share of its pinned cores. Requests per CPU-second divides completed requests by the server's CPU time, so it holds even when a run is client-bound. Startup is process start to the first HTTP response. Working set is measured after the run.");
        markdown.AppendLine();
        markdown.AppendLine("| Scenario | CPU % (C / A) | Req per CPU-s (C / A) | Ratio | Startup ms (C / A) | Working set MB (C / A) |");
        markdown.AppendLine("| --- | ---: | ---: | ---: | ---: | ---: |");
        foreach (ScenarioInfo scenario in scenarios)
        {
            medians.TryGetValue((scenario.Id, Cohesion), out RunRecord? c);
            medians.TryGetValue((scenario.Id, AspNetCore), out RunRecord? a);
            string ratio = c?.RequestsPerCpuSecond is > 0 && a?.RequestsPerCpuSecond is > 0 && c.Passed && a.Passed
                ? $"{c.RequestsPerCpuSecond.Value / a.RequestsPerCpuSecond.Value:0.00}×"
                : "–";
            markdown.AppendLine(
                $"| `{scenario.Id}` | {Num(c?.ServerCpuPercent, "0")} / {Num(a?.ServerCpuPercent, "0")} | " +
                $"{Num(c?.RequestsPerCpuSecond, "N0")} / {Num(a?.RequestsPerCpuSecond, "N0")} | {ratio} | " +
                $"{Num(c?.StartupMilliseconds, "0")} / {Num(a?.StartupMilliseconds, "0")} | " +
                $"{Num(c?.WorkingSetMegabytes, "0.0")} / {Num(a?.WorkingSetMegabytes, "0.0")} |");
        }
        markdown.AppendLine();
    }

    private static void RenderParity(StringBuilder markdown, List<ScenarioInfo> scenarios, Dictionary<(string, string), RunRecord> medians)
    {
        markdown.AppendLine("## Response parity");
        markdown.AppendLine();
        markdown.AppendLine("The single validation exchange each server answered before load: status, response-head bytes and body bytes. Differences in head size are framework header choices (for example Kestrel's `Server` header).");
        markdown.AppendLine();
        markdown.AppendLine("| Scenario | Cohesion | ASP.NET Core |");
        markdown.AppendLine("| --- | --- | --- |");
        foreach (ScenarioInfo scenario in scenarios)
        {
            medians.TryGetValue((scenario.Id, Cohesion), out RunRecord? c);
            medians.TryGetValue((scenario.Id, AspNetCore), out RunRecord? a);
            markdown.AppendLine($"| `{scenario.Id}` | {Parity(c)} | {Parity(a)} |");
        }
        markdown.AppendLine();
        markdown.AppendLine("<details><summary>Response heads</summary>");
        markdown.AppendLine();
        foreach (ScenarioInfo scenario in scenarios)
        {
            markdown.AppendLine($"### `{scenario.Id}`");
            markdown.AppendLine();
            foreach (string framework in new[] { Cohesion, AspNetCore })
            {
                if (medians.TryGetValue((scenario.Id, framework), out RunRecord? record) && record.Validation is { } validation)
                {
                    markdown.AppendLine($"{Name(framework)}:");
                    markdown.AppendLine();
                    markdown.AppendLine("```http");
                    markdown.AppendLine(validation.Head.TrimEnd());
                    if (validation.BodyPreview.Length > 0 && !validation.Head.Contains("Content-Encoding", StringComparison.OrdinalIgnoreCase))
                    {
                        markdown.AppendLine();
                        markdown.AppendLine(validation.BodyPreview.Replace("```", "'''"));
                    }
                    markdown.AppendLine("```");
                    markdown.AppendLine();
                }
            }
        }
        markdown.AppendLine("</details>");
        markdown.AppendLine();
    }

    private static void RenderFailures(StringBuilder markdown, List<RunRecord> runs)
    {
        List<RunRecord> failures = runs.Where(run => !run.Passed).ToList();
        if (failures.Count == 0)
        {
            return;
        }

        markdown.AppendLine("## Failures");
        markdown.AppendLine();
        markdown.AppendLine("| Scenario | Framework | Iteration | Failure |");
        markdown.AppendLine("| --- | --- | ---: | --- |");
        foreach (RunRecord run in failures)
        {
            markdown.AppendLine($"| `{run.Scenario}` | {Name(run.Framework)} | {run.Iteration} | {run.Failure?.Replace("|", "\\|")} |");
        }
        markdown.AppendLine();
    }

    private static void RenderNotes(StringBuilder markdown, List<ScenarioInfo> scenarios)
    {
        markdown.AppendLine("## Scenarios");
        markdown.AppendLine();
        markdown.AppendLine("| Scenario | Server pipeline | What it measures |");
        markdown.AppendLine("| --- | --- | --- |");
        foreach (ScenarioInfo scenario in scenarios)
        {
            string note = scenario.Note is null ? string.Empty : $" *{scenario.Note}*";
            markdown.AppendLine($"| `{scenario.Id}` | `{scenario.Server}` | {scenario.Description}{note} |");
        }
        markdown.AppendLine();
    }

    private static Dictionary<(string, string), RunRecord> Medians(List<RunRecord> runs)
    {
        var medians = new Dictionary<(string, string), RunRecord>();
        foreach (IGrouping<(string Scenario, string Framework), RunRecord> group in runs.GroupBy(run => (run.Scenario, run.Framework)))
        {
            List<RunRecord> passed = group.Where(run => run.Passed).OrderBy(run => run.RequestsPerSecond ?? 0).ToList();
            medians[group.Key] = passed.Count > 0 ? passed[passed.Count / 2] : group.First();
        }
        return medians;
    }

    private static bool TryPair(Dictionary<(string, string), RunRecord> medians, string scenario, out RunRecord cohesion, out RunRecord aspnet)
    {
        cohesion = null!;
        aspnet = null!;
        if (medians.TryGetValue((scenario, Cohesion), out RunRecord? c) && medians.TryGetValue((scenario, AspNetCore), out RunRecord? a) &&
            c.Passed && a.Passed && c.RequestsPerSecond is > 0 && a.RequestsPerSecond is > 0)
        {
            cohesion = c;
            aspnet = a;
            return true;
        }
        return false;
    }

    private static double GeoMean(List<double> values) =>
        values.Count == 0 ? double.NaN : Math.Exp(values.Sum(Math.Log) / values.Count);

    private static string Rps(RunRecord? record) => record switch
    {
        null => "–",
        { Passed: false } => "failed",
        { RequestsPerSecond: null } => "–",
        _ => $"{record.RequestsPerSecond:N0}{(record.ServerCpuPercent < ClientBoundThreshold ? " †" : string.Empty)}",
    };

    private static string Latency(RunRecord? record) =>
        record is { Passed: true, Latency: { } latency } ? $"{latency.P50:0.00} / {latency.P99:0.00}" : "–";

    private static string Parity(RunRecord? record) => record?.Validation is { } validation
        ? $"{validation.Status}, head {validation.HeadBytes} B, body {validation.BodyBytes} B{(record.Passed ? string.Empty : " (failed)")}"
        : record is null ? "–" : $"no response ({record.Failure})";

    private static string Num(double? value, string format) => value is null ? "–" : value.Value.ToString(format);

    private static string Name(string framework) => framework switch
    {
        Cohesion => "Cohesion",
        AspNetCore => "ASP.NET Core",
        _ => framework,
    };

    private static string Str(System.Text.Json.JsonElement info, string property) =>
        info.TryGetProperty(property, out System.Text.Json.JsonElement value) ? value.ToString() : "?";
}
