using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Reports comparable verified repetitions, support/error matrices and every Cohesion deficit.</summary>
public static class ReportGenerator
{
    public static async Task<IReadOnlyList<Finding>> GenerateAsync(string input, string output, CancellationToken token = default)
    {
        var results = new List<BenchmarkResult>();
        var verificationFiles = new List<VerificationFile>();
        var failures = new List<OrchestrationFailure>();
        foreach (string path in Directory.EnumerateFiles(input, "*.json", SearchOption.AllDirectories).Order(StringComparer.Ordinal))
        {
            using var file = JsonDocument.Parse(await File.ReadAllTextAsync(path, token).ConfigureAwait(false));
            if (file.RootElement.ValueKind != JsonValueKind.Object) continue;
            if (!file.RootElement.TryGetProperty("runId", out _))
            {
                if (Path.GetFileName(path) == "phase.status.json" &&
                    file.RootElement.TryGetProperty("status", out var status) && status.GetString() == "failed")
                {
                    string phaseModel = file.RootElement.TryGetProperty("model", out var id) ? id.GetString()!
                        : Directory.GetParent(Path.GetDirectoryName(path)!)?.Parent?.Name ?? "unknown";
                    failures.Add(new(phaseModel, file.RootElement.GetProperty("target").GetString()!,
                        file.RootElement.GetProperty("phase").GetString()!, file.RootElement.GetProperty("error").GetString() ?? "Phase failed.",
                        file.RootElement.GetProperty("seconds").GetDouble()));
                }
                if (file.RootElement.ValueKind == JsonValueKind.Object && file.RootElement.TryGetProperty("parameterCount", out _))
                    verificationFiles.Add(await ResultFiles.ReadAsync(path, ResultJsonContext.Default.VerificationFile, token).ConfigureAwait(false));
                continue;
            }
            var result = await ResultFiles.ReadAsync(path, ResultJsonContext.Default.BenchmarkResult, token).ConfigureAwait(false);
            if (result.SchemaVersion != 1) throw new InvalidDataException($"Unsupported result schema in {path}.");
            // Older smoke files carried a nominal 6 GiB label. The recorded inspection/cgroup
            // evidence is authoritative for report cohorts too; missing limits stay unknown.
            results.Add(result with { Budget = ResourceBudget.Observe(result.Environment,
                result.Target.Id.EndsWith("-embedded", StringComparison.Ordinal)) });
        }
        if (results.Count == 0 && failures.Count == 0) throw new InvalidDataException("No benchmark result JSON files or failed phase receipts found.");
        for (int i = 0; i < results.Count; i++)
        {
            var result = results[i];
            if (result.Verification == null)
                results[i] = result with { Verification = verificationFiles.FirstOrDefault(v => v.Target == result.Target.Id &&
                    v.Model == result.Model && v.DatasetManifestSha256 == result.DatasetManifestSha256 && v.Seed == result.Seed) };
        }
        Directory.CreateDirectory(output);
        var findings = new List<Finding>();
        foreach (string model in results.Select(r => r.Model).Concat(failures.Select(f => f.Model)).Distinct().Order(StringComparer.Ordinal))
        {
            var text = new StringBuilder($"# {model} benchmark\n\n");
            text.AppendLine("Median across repetitions; brackets show min–max. Latency units: microseconds. † = emulated.");
            text.AppendLine("Only verified, error-free, ranked targets enter performance tables and ratios. Missing verification blocks performance.\n");
            var modelRows = results.Where(r => r.Model == model).ToArray();
            RenderFailures(model, failures.Where(f => f.Model == model).ToArray(), text, findings);
            if (modelRows.Length == 0) text.AppendLine("No benchmark metrics were produced. This is failed execution evidence, not a performance result.\n");
            foreach (var cohort in modelRows.GroupBy(Cohort))
                Render(cohort.Key, cohort.ToArray(), text, findings);
            await File.WriteAllTextAsync(Path.Combine(output, model + ".md"), text.ToString(), token).ConfigureAwait(false);
        }
        await ResultFiles.WriteAsync(Path.Combine(output, "findings.json"), findings, ResultJsonContext.Default.ListFinding, token).ConfigureAwait(false);
        return findings;
    }

    private sealed record OrchestrationFailure(string Model, string Target, string Phase, string Error, double Seconds);
    private static void RenderFailures(string model, OrchestrationFailure[] failures, StringBuilder text, List<Finding> findings)
    {
        if (failures.Length == 0) return;
        text.AppendLine("## Failed orchestration phases\n");
        text.AppendLine("These phases may have terminated before producing result JSON. See the phase console log and receipt for details.\n");
        text.AppendLine("| Target | Phase | Seconds | Error |");
        text.AppendLine("|---|---|---:|---|");
        foreach (var failure in failures)
        {
            text.AppendLine($"| {E(failure.Target)} | {E(failure.Phase)} | {N(failure.Seconds)} | {E(failure.Error)} |");
            findings.Add(new(model, "orchestration", failure.Target, "phase-error", failure.Phase, failure.Error, failure.Seconds));
        }
        text.AppendLine();
    }
    private static string Cohort(BenchmarkResult r)
        => $"{r.DatasetManifestSha256}|seed={r.Seed}|engine={r.Budget.Cpus}/{r.Budget.MemoryBytes}|official={r.Official}|benchmarks={r.Environment.BenchmarksCommit}|cohesion={string.Join(",", r.Environment.CohesionBuiltCommits)}";

    private static void Render(string cohort, BenchmarkResult[] rows, StringBuilder text, List<Finding> findings)
    {
        string model = rows[0].Model;
        text.AppendLine($"## Dataset {rows[0].DatasetManifestSha256}, SF {N(rows[0].Scale)}\n");
        text.AppendLine($"Budget: {N(rows[0].Budget.Cpus)} CPUs / {rows[0].Budget.MemoryBytes} bytes; official: {rows[0].Official}.\n");
        text.AppendLine("Observed container limits (0 = unknown or unbounded):\n");
        text.AppendLine("| Target | Engine CPUs | Engine bytes | Driver bytes |");
        text.AppendLine("|---|---:|---:|---:|");
        foreach (var run in rows.DistinctBy(r => r.Target.Id))
            text.AppendLine($"| {E(run.Target.Id)} | {N(run.Budget.Cpus)} | {run.Budget.MemoryBytes} | {run.Budget.DriverMemoryBytes} |");
        text.AppendLine();
        var targets = rows.Select(r => r.Target).DistinctBy(t => t.Id).OrderBy(t => t.Id, StringComparer.Ordinal).ToArray();
        // Suites embed the same verification file; compare identical evidence once, retaining conflicting attempts.
        var verification = rows.Where(r => r.Verification != null).Select(r => r.Verification!)
            .DistinctBy(v => JsonSerializer.Serialize(v, ResultJsonContext.Default.VerificationFile)).ToArray();
        var mismatches = VerificationRunner.Compare(verification);
        var blocked = new HashSet<string>(StringComparer.Ordinal);
        foreach (var target in targets)
        {
            var own = verification.Where(v => v.Target == target.Id).ToArray();
            bool missing = own.Length == 0 || own.Any(v => v.DatasetManifestSha256 != rows[0].DatasetManifestSha256 ||
                v.Seed != rows[0].Seed) ||
                rows.Where(r => r.Target.Id == target.Id).SelectMany(r => r.Operations)
                    .Any(o => o.Support.Level != SupportLevel.Unsupported && !own.Any(v =>
                        v.Operations.Any(x => x.Operation == o.Operation && x.Error == null && x.Cases.Count == 100)));
            if (missing)
            {
                blocked.Add(target.Id);
                text.AppendLine($"Verification blocked: {E(target.Id)} has missing or incompatible digests.\n");
                if (Cohesion(target.Id)) findings.Add(new(model, cohort, target.Id, "verification", "*", "Missing or incompatible verification digests."));
            }
        }
        foreach (var mismatch in mismatches)
        {
            blocked.Add(mismatch.LeftTarget); blocked.Add(mismatch.RightTarget);
            text.AppendLine($"Verification mismatch: {E(mismatch.Operation)} / {E(mismatch.LeftTarget)} / {E(mismatch.RightTarget)}: {E(mismatch.Evidence)}\n");
            foreach (string target in new[] { mismatch.LeftTarget, mismatch.RightTarget }.Distinct())
                if (Cohesion(target)) findings.Add(new(model, cohort, target, "verification", mismatch.Operation, mismatch.Evidence));
        }
        text.AppendLine("### Functional verification\n");
        text.AppendLine("| Target | Ranked | Verification |");
        text.AppendLine("|---|---|---|");
        foreach (var target in targets) text.AppendLine($"| {E(target.Label)} ({E(target.Id)}) | {target.Ranked} | {(blocked.Contains(target.Id) ? "BLOCKED" : "passed")} |");
        text.AppendLine();
        foreach (var run in rows)
        {
            if (run.SuiteBlockReason != null) text.AppendLine($"Suite blocked: {E(run.Target.Id)} / {E(run.Suite)}: {E(run.SuiteBlockReason)}\n");
            var high = run.Samples.Where(s => s.HostCpuPercent >= 80).ToArray();
            if (high.Length != 0) text.AppendLine($"Noise: {E(run.RunId)} repetition {run.Repetition}: host CPU ≥80% in {high.Length}/{run.Samples.Count} samples; max {N(high.Max(s => s.HostCpuPercent!.Value))}%.\n");
            if (run.Samples.Count == 0 || run.Samples.All(s => s.HostCpuPercent == null))
                text.AppendLine($"Noise observation unavailable: {E(run.RunId)} repetition {run.Repetition}.\n");
        }
        var entries = rows.SelectMany(r => r.Operations.Select(o => (Run: r, Op: o))).ToArray();
        string[] operationIds = entries.Select(x => x.Op.Operation).Concat(verification.SelectMany(v => v.Operations.Select(o => o.Operation))).Distinct().Order(StringComparer.Ordinal).ToArray();
        text.AppendLine("### Support matrix\n");
        Header(text, "Operation", targets);
        foreach (string operation in operationIds)
        {
            text.Append($"| {E(operation)} |");
            foreach (var target in targets)
            {
                var support = entries.FirstOrDefault(e => e.Run.Target.Id == target.Id && e.Op.Operation == operation).Op?.Support
                    ?? verification.Where(v => v.Target == target.Id).SelectMany(v => v.Operations).FirstOrDefault(o => o.Operation == operation)?.Support;
                text.Append($" {support?.Level.ToString().ToLowerInvariant() ?? "not recorded"} |");
                if (Cohesion(target.Id) && support != null && support.Level != SupportLevel.Native)
                    findings.Add(new(model, cohort, target.Id, support.Level.ToString().ToLowerInvariant(), operation,
                        (support.Reason ?? "") + (support.Evidence == null ? "" : " Evidence: " + support.Evidence)));
            }
            text.AppendLine();
        }
        text.AppendLine("\n### Error matrix\n");
        Header(text, "Operation", targets);
        foreach (string operation in operationIds)
        {
            text.Append($"| {E(operation)} |");
            foreach (var target in targets)
            {
                var own = entries.Where(e => e.Run.Target.Id == target.Id && e.Op.Operation == operation).ToArray();
                long errors = own.Sum(e => e.Op.Errors), timeouts = own.Sum(e => e.Op.Timeouts), warmup = own.Sum(e => e.Op.WarmupErrors);
                text.Append($" {errors} errors / {timeouts} timeouts / {warmup} warm-up |");
                if (Cohesion(target.Id) && errors + warmup > 0)
                    findings.Add(new(model, cohort, target.Id, "errors", operation,
                        $"{errors} measured errors, {timeouts} timeouts, {warmup} warm-up errors. " + string.Join("; ", own.SelectMany(e => e.Op.ErrorExamples).Distinct()), errors + warmup));
            }
            text.AppendLine();
        }
        text.AppendLine("\n### Load\n");
        text.AppendLine("| Target | Total seconds [spread] | Ratio | Stored bytes |");
        text.AppendLine("|---|---:|---:|---:|");
        var loads = rows.Where(r => r.Suite == "load" && r.Target.Ranked && !blocked.Contains(r.Target.Id) && r.Load.Any(l => !l.IsSample))
            .GroupBy(r => r.Target.Id).Select(g => new
            {
                Target = g.First().Target,
                Values = g.Select(r => r.Load.Where(l => !l.IsSample).Sum(l => l.Seconds)).ToArray(),
                Storage = g.Last().StorageBytes,
            }).ToArray();
        double bestLoad = loads.Length == 0 ? 0 : loads.Min(g => ReportStatistics.Median(g.Values));
        string? bestLoader = loads.FirstOrDefault(g => ReportStatistics.Median(g.Values) == bestLoad)?.Target.Id;
        foreach (var load in loads)
        {
            double median = ReportStatistics.Median(load.Values), ratio = ReportStatistics.Ratio(median, bestLoad);
            text.AppendLine($"| {E(load.Target.Label)} | {Spread(load.Values)} | {N(ratio)}× | {load.Storage} |");
            if (Cohesion(load.Target.Id) && ReportStatistics.IsDeficit(ratio, true))
                findings.Add(new(model, cohort, load.Target.Id, "load", "*", "Total ingest + finalization wall time, seconds.", median, bestLoad, ratio, bestLoader));
        }
        text.AppendLine("\n| Target | Table | Phase / path | Rows/s | MB/s | Seconds | Sample |");
        text.AppendLine("|---|---|---|---:|---:|---:|---|");
        foreach (var run in rows.Where(r => r.Suite == "load" && r.Target.Ranked && !blocked.Contains(r.Target.Id)))
        foreach (var metric in run.Load)
            text.AppendLine($"| {E(run.Target.Id)} | {E(metric.Table)} | {E(metric.Phase)} / {E(metric.Path)} | {N(metric.RowsPerSecond)} | {N(metric.MegabytesPerSecond)} | {N(metric.Seconds)} | {metric.IsSample} |");
        text.AppendLine();
        var ranked = targets.Where(t => t.Ranked).ToArray();
        foreach (var suite in entries.Where(e => e.Run.Suite != "load").GroupBy(e =>
            $"{e.Run.Suite}/{e.Run.Mix ?? "-"}/clients={e.Op.Clients}/duration={e.Run.Runner?.Duration}/warmup={e.Run.Runner?.Warmup}/iterations={e.Run.Runner?.Iterations}"))
        {
            bool throughput = suite.First().Run.Suite != "latency";
            text.AppendLine($"### {suite.Key}\n");
            text.AppendLine(throughput ? "Cells: operations/s [spread], ratio (best throughput / target throughput).\n" :
                "Cells: p50 [spread]; p90 / p95 / p99 / p99.9 / max (median across repetitions); ratio of p50 to best.\n");
            Header(text, "Operation", ranked);
            foreach (var op in suite.GroupBy(e => e.Op.Operation).OrderBy(g => g.Key, StringComparer.Ordinal))
            {
                var failedTargets = op.Where(e => e.Op.Errors + e.Op.WarmupErrors > 0).Select(e => e.Run.Target.Id).ToHashSet(StringComparer.Ordinal);
                var measured = op.Where(e => e.Run.Target.Ranked && !blocked.Contains(e.Run.Target.Id) && !failedTargets.Contains(e.Run.Target.Id) &&
                    e.Op.Support.Level != SupportLevel.Unsupported && e.Op.Errors == 0 && e.Op.WarmupErrors == 0 && e.Op.Successes > 0)
                    .GroupBy(e => e.Run.Target.Id).ToDictionary(g => g.Key, g => g.Select(e => e.Op).ToArray(), StringComparer.Ordinal);
                var medians = measured.ToDictionary(g => g.Key, g => ReportStatistics.Median(g.Value.Select(o => throughput ? o.OperationsPerSecond : o.P50Us)));
                double best = medians.Count == 0 ? 0 : throughput ? medians.Values.Max() : medians.Values.Min();
                string? bestTarget = medians.FirstOrDefault(p => p.Value == best).Key;
                text.Append($"| {E(op.Key)} |");
                foreach (var target in ranked)
                {
                    if (!measured.TryGetValue(target.Id, out var values)) { text.Append(" — |"); continue; }
                    double median = medians[target.Id], ratio = ReportStatistics.Ratio(median, best, throughput);
                    string mark = values.Any(o => o.Support.Level == SupportLevel.Emulated) ? " †" : "";
                    string percentiles = throughput ? "" : "; " + string.Join(" / ", new[] {
                        ReportStatistics.Median(values.Select(o => (double)o.P90Us)), ReportStatistics.Median(values.Select(o => (double)o.P95Us)),
                        ReportStatistics.Median(values.Select(o => (double)o.P99Us)), ReportStatistics.Median(values.Select(o => (double)o.P999Us)),
                        ReportStatistics.Median(values.Select(o => (double)o.MaxUs)) }.Select(N));
                    text.Append($" {Spread(values.Select(o => throughput ? o.OperationsPerSecond : o.P50Us).ToArray())}{percentiles}; {N(ratio)}×{mark} |");
                    if (Cohesion(target.Id) && ReportStatistics.IsDeficit(ratio))
                        findings.Add(new(model, cohort, target.Id, "performance", op.Key, suite.Key + (throughput ? ": operations/s" : ": p50 microseconds"), median, best, ratio, bestTarget));
                }
                text.AppendLine();
            }
            text.AppendLine();
        }
    }
    private static bool Cohesion(string id) => id.StartsWith("cohesion-", StringComparison.Ordinal);
    private static string N(double n) => n.ToString("0.###", CultureInfo.InvariantCulture);
    private static string E(string text) => text.Replace("|", "\\|", StringComparison.Ordinal).Replace("\r", "", StringComparison.Ordinal).Replace("\n", " ", StringComparison.Ordinal);
    private static string Spread(double[] values) => $"{N(ReportStatistics.Median(values))} [{N(values.Min())}–{N(values.Max())}]";
    private static void Header(StringBuilder text, string first, TargetDescriptor[] targets)
    {
        text.AppendLine("| " + first + " | " + string.Join(" | ", targets.Select(t => E(t.Label) + " (" + E(t.Id) + ")")) + " |");
        text.AppendLine("|---|" + string.Join("", targets.Select(_ => "---|")));
    }
}
