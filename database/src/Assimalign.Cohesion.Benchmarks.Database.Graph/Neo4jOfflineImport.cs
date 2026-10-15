using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal static class Neo4jOfflineImport
{
    internal static async Task<List<LoadMetric>> RunAsync(TargetOptions options, DatasetManifest manifest, string directory, CancellationToken token)
    {
        if (!OperatingSystem.IsWindows()) throw new NotSupportedException("Full Neo4j import is host-orchestrated: run dbbench load on the Podman host, then use the network driver for verification/runs.");
        string output = Path.Combine(Path.GetFullPath(directory), ".neo4j-import");
        Directory.CreateDirectory(output);
        string cancellationFile = Path.Combine(output, "cancel-import");
        if (File.Exists(cancellationFile)) File.Delete(cancellationFile);
        var watch = Stopwatch.StartNew();
        var arguments = new List<string> { "database", "import", "full", options.Database == "benchmark" ? "neo4j" : options.Database,
            "--overwrite-destination=true", "--id-type=integer", "--multiline-fields=true" };
        long count = 0, bytes = 0;
        foreach (var node in SocialSchema.Nodes)
        {
            File.WriteAllText(Path.Combine(output, node.File + ".header.csv"), string.Join(',', GraphMapping.Neo4jHeader(node)) + "\n", new UTF8Encoding(false));
            string path = Path.Combine(output, node.File + ".csv");
            using (var writer = new StreamWriter(path, false, new UTF8Encoding(false), 65536))
            foreach (var properties in GraphData.Nodes(manifest, directory, node, long.MaxValue, token))
            {
                for (int i = 0; i < node.Properties.Count; i++)
                {
                    if (i != 0) writer.Write(',');
                    Field(writer, properties.GetValueOrDefault(node.Properties[i].Name));
                }
                writer.Write('\n'); count++;
            }
            bytes += new FileInfo(path).Length;
            arguments.Add($"--nodes={node.Label}=/import/{node.File}.header.csv,/import/{node.File}.csv");
        }
        foreach (var relation in SocialSchema.Relationships)
        {
            File.WriteAllText(Path.Combine(output, relation.File + ".header.csv"), string.Join(',', GraphMapping.Neo4jHeader(relation)) + "\n", new UTF8Encoding(false));
            string path = Path.Combine(output, relation.File + ".csv");
            using (var writer = new StreamWriter(path, false, new UTF8Encoding(false), 65536))
            foreach (var edge in GraphData.Edges(manifest, directory, relation, false, token))
            {
                Field(writer, edge.Source); writer.Write(','); Field(writer, edge.Target);
                foreach (var column in relation.Properties) { writer.Write(','); Field(writer, edge.Properties.GetValueOrDefault(column.Name)); }
                writer.Write('\n'); count++;
            }
            bytes += new FileInfo(path).Length;
            arguments.Add($"--relationships={relation.Type}=/import/{relation.File}.header.csv,/import/{relation.File}.csv");
        }
        await File.WriteAllTextAsync(Path.Combine(output, "arguments.json"), JsonSerializer.Serialize(arguments), token);
        watch.Stop();
        var metrics = new List<LoadMetric> { new("social", "import-conversion", "SocialSchema typed CSV; epoch milliseconds", count, bytes, watch.Elapsed.TotalSeconds) };
        string script = Path.Combine("database", "engines", "neo4j", "Import-Neo4j.ps1");
        var start = new ProcessStartInfo("pwsh") { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true };
        foreach (string argument in new[] { "-NoProfile", "-File", script, "-Container", options.Container ?? throw new ArgumentException("Neo4j import needs --container."), "-InputDirectory", output, "-CancellationFile", cancellationFile }) start.ArgumentList.Add(argument);
        using var process = Process.Start(start) ?? throw new InvalidOperationException("Could not launch offline importer.");
        // Drain output even during cancellation so the script can always reach its cleanup/restart finally.
        var stdout = process.StandardOutput.ReadToEndAsync();
        var stderr = process.StandardError.ReadToEndAsync();
        try { await process.WaitForExitAsync(token); }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            await File.WriteAllTextAsync(cancellationFile, "cancel", CancellationToken.None);
            var stop = new ProcessStartInfo("podman") { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true };
            foreach (string argument in new[] { "stop", "--time", "5", options.Container!.Replace("dbbench-neo4j", "dbbench-neo4j-import", StringComparison.Ordinal) }) stop.ArgumentList.Add(argument);
            using var stopper = Process.Start(stop)!;
            var stopOutput = stopper.StandardOutput.ReadToEndAsync();
            var stopError = stopper.StandardError.ReadToEndAsync();
            await stopper.WaitForExitAsync(CancellationToken.None);
            await stopOutput; await stopError;
            await process.WaitForExitAsync(CancellationToken.None);
        }
        string log = await stdout + await stderr;
        await File.WriteAllTextAsync(Path.Combine(output, "import.log"), log, CancellationToken.None);
        token.ThrowIfCancellationRequested();
        if (process.ExitCode != 0) throw new InvalidOperationException("Offline import failed: " + log);
        using var receipt = JsonDocument.Parse(await File.ReadAllTextAsync(Path.Combine(output, "receipt.json"), token));
        metrics.Add(new("social", "import-staging", "copy converted CSV into named dataset volume", count, bytes, receipt.RootElement.GetProperty("stagingSeconds").GetDouble()));
        metrics.Add(new("social", "ingest", "neo4j-admin database import full; named volumes", count, bytes, receipt.RootElement.GetProperty("seconds").GetDouble()));
        return metrics;
    }

    private static void Field(TextWriter writer, object? value)
    {
        if (value is null) return;
        if (value is string s)
        {
            bool quote = s.Length == 0 || s.AsSpan().IndexOfAny(",\"\r\n") >= 0;
            if (!quote) { writer.Write(s); return; }
            writer.Write('"'); writer.Write(s.Replace("\"", "\"\"", StringComparison.Ordinal)); writer.Write('"');
        }
        else writer.Write(Convert.ToString(value, CultureInfo.InvariantCulture));
    }
}
