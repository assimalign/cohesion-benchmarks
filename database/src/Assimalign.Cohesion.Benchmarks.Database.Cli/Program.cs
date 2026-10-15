using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Cli;

internal static class Program
{
    private static readonly IBenchmarkModel[] Models =
        [Sql.ModelRegistration.Create(), Documents.ModelRegistration.Create(), Graph.ModelRegistration.Create()];

    private static async Task<int> Main(string[] args)
    {
        using var stop = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; stop.Cancel(); };
        try { return await ExecuteAsync(new Arguments(args), stop.Token); }
        catch (OperationCanceledException) { Console.Error.WriteLine("dbbench: canceled."); return 130; }
        catch (Exception ex) { Console.Error.WriteLine($"dbbench: {ex.GetType().Name}: {ex.Message}"); return 1; }
    }

    private static async Task<int> ExecuteAsync(Arguments a, CancellationToken token)
    {
        if (a.Command is "help" or "--help")
        {
            Console.WriteLine("""
                dbbench generate --model sql|documents|graph --scale N --out DIR [--seed N] [--threads N]
                dbbench load --model M --target ID --data DIR [--endpoint URI] [--batch N] [--official] --out DIR
                dbbench verify --model M --target ID --data DIR --out FILE
                dbbench verify --compare DIR [--out FILE]
                dbbench run --model M --target ID --data DIR --suite latency|throughput|mixed --out DIR
                  [--clients 1,8,32] [--duration 60s] [--warmup 15s] [--iterations N] [--repeat 3]
                  [--ops a,b] [--mix read-heavy|balanced] [--verification FILE] [--official]
                dbbench report --in DIR --out DIR
                dbbench env [--container NAME] [--out FILE]
                dbbench ping --target ID [--endpoint URI] [--database benchmark] [--suffix -r1a] [--smoke]
                dbbench list [--model M] [--catalog FILE]
                Common target options: --endpoint, --database, --storage, --container, --catalog, --suffix
                """);
            return 0;
        }
        if (a.Command == "generate")
        {
            a.Allow("model", "scale", "out", "seed", "threads", "overwrite");
            var model = Model(a.Required("model"));
            double scale = a.Double("scale", 1);
            if (!double.IsFinite(scale) || scale <= 0 || a.Int("threads", Environment.ProcessorCount) < 1)
                throw new ArgumentException("Scale and thread count must be positive.");
            var generation = new DatasetGenerationOptions { Model = model.DatasetModel, Scale = scale,
                OutputDirectory = a.Required("out"), Seed = a.Seed, Threads = a.Int("threads", Environment.ProcessorCount), Overwrite = a.Has("overwrite") };
            Console.WriteLine($"GENERATE model={model.Id} scale={scale.ToString(CultureInfo.InvariantCulture)} seed={generation.Seed} out={generation.OutputDirectory}");
            await DatasetGenerators.Get(model.DatasetModel).GenerateAsync(generation,
                new Progress<DatasetProgress>(p => Console.WriteLine($"{p.Table}: {p.RowsWritten} rows, {p.BytesWritten} bytes")), token);
            return 0;
        }
        if (a.Command == "report")
        {
            a.Allow("in", "out");
            var findings = await ReportGenerator.GenerateAsync(a.Required("in"), a.Required("out"), token);
            Console.WriteLine($"REPORT {a.Required("out")}: {findings.Count} Cohesion findings.");
            return 0;
        }
        if (a.Command == "verify" && a.Has("compare"))
        {
            a.Allow("compare", "out");
            var files = new List<VerificationFile>();
            foreach (string file in Directory.EnumerateFiles(a.Required("compare"), "*.verify.json", SearchOption.AllDirectories))
                files.Add(await ResultFiles.ReadAsync(file, ResultJsonContext.Default.VerificationFile, token));
            if (files.Select(f => f.Target).Distinct().Count() < 2) throw new ArgumentException("Comparison requires at least two target *.verify.json files.");
            var mismatches = VerificationRunner.Compare(files);
            foreach (var mismatch in mismatches) Console.WriteLine($"{mismatch.Operation}: {mismatch.LeftTarget} / {mismatch.RightTarget}: {mismatch.Evidence}");
            if (a.Optional("out") is { } output) await ResultFiles.WriteAsync(output, mismatches, ResultJsonContext.Default.ListVerificationMismatch, token);
            Console.WriteLine($"VERIFY COMPARE: {mismatches.Count} mismatches.");
            return mismatches.Count == 0 ? 0 : 2;
        }
        if (a.Command == "list")
        {
            a.Allow("model", "catalog");
            if (a.Has("model")) _ = Model(a.Required("model"));
            foreach (var model in Models.Where(m => !a.Has("model") || m.Id == a.Required("model")))
            {
                Console.WriteLine($"model {model.Id}: {model.Targets.Count} workload targets, {model.Operations.Count} operations");
                foreach (var target in model.Targets) Console.WriteLine($"  target {target.Id}: {target.Label}; ranked={target.Ranked}");
                foreach (var operation in model.Operations) Console.WriteLine($"  operation {operation.Id}: {operation.Category}, {operation.TimeoutClass}");
            }
            using var catalog = new EngineCatalog(a.Optional("catalog"));
            catalog.Print();
            return 0;
        }
        LoadEngineAssemblies();
        if (a.Command == "env")
        {
            a.Allow("container", "out");
            var environment = await EnvironmentCapture.CaptureAsync(a.Optional("container"), Environment.GetEnvironmentVariable("COHESION_ROOT"), token);
            if (a.Optional("out") is { } output) await ResultFiles.WriteAsync(output, environment, ResultJsonContext.Default.EnvironmentSnapshot, token);
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(environment, ResultJsonContext.Default.EnvironmentSnapshot));
            return 0;
        }
        if (a.Command is not ("load" or "verify" or "run" or "ping")) throw new ArgumentException("Unknown command " + a.Command);
        a.Allow("model", "target", "data", "endpoint", "database", "storage", "container", "catalog", "suffix", "out",
            "batch", "official", "suite", "clients", "duration", "warmup", "iterations", "repeat", "ops", "mix",
            "point-timeout", "analytics-timeout", "verification", "seed", "smoke", "buffer-pool-mb", "sample-rows", "environment");
        string id = a.Required("target");
        using var engines = new EngineCatalog(a.Optional("catalog"));
        var entry = engines.Find(id);
        string suffix = a.Get("suffix", "");
        string? endpoint = a.Optional("endpoint") ?? engines.Endpoint(id, suffix);
        var targetOptions = new TargetOptions { Endpoint = endpoint, Database = a.Get("database", "benchmark"),
            StorageDirectory = a.Get("storage", "/engine-data"), BatchSize = a.Int("batch", 1000),
            Container = a.Optional("container") ?? (entry.GetProperty("kind").GetString() == "embedded" ? null : entry.GetProperty("containerName").GetString() + suffix),
            TcpIngestSampleRowsPerTable = a.Int("sample-rows", 1000), BufferPoolBytes = checked(a.Int("buffer-pool-mb", 3072) * 1024L * 1024),
            Settings = engines.Settings(id) };
        if (a.Command == "ping" && id is "cohesion-sql-tcp" or "cohesion-graph-tcp")
        {
            using var deadline = CancellationTokenSource.CreateLinkedTokenSource(token);
            deadline.CancelAfter(TimeSpan.FromSeconds(20));
            await CohesionReadiness.PingAsync(id, endpoint!, targetOptions.Database, a.Has("smoke"), deadline.Token);
            return 0;
        }
        var selectedModel = Model(a.Get("model", entry.GetProperty("model").GetString()!));
        await using var targetInstance = selectedModel.CreateTarget(id, targetOptions);
        if (a.Command == "ping") { await targetInstance.PingAsync(token); Console.WriteLine("PING " + id + ": ready."); return 0; }
        string directory = a.Required("data");
        string manifestPath = Path.Combine(directory, "manifest.json");
        var manifest = await ResultFiles.ReadAsync(manifestPath, ResultJsonContext.Default.DatasetManifest, token);
        if (manifest.Model != selectedModel.DatasetModel) throw new ArgumentException("Dataset model differs from requested model.");
        string manifestHash = await ResultFiles.HashAsync(manifestPath, token);
        var operations = SelectOperations(selectedModel, a.Optional("ops"));
        if (a.Command == "verify")
        {
            var verified = await VerificationRunner.VerifyAsync(selectedModel, targetInstance, manifest, manifestHash, operations, token);
            await ResultFiles.WriteAsync(a.Required("out"), verified, ResultJsonContext.Default.VerificationFile, token);
            return VerificationRunner.Compare([verified]).Count == 0 ? 0 : 2;
        }
        var environmentSnapshot = await EnvironmentCapture.CaptureAsync(targetOptions.Container, Environment.GetEnvironmentVariable("COHESION_ROOT"), token);
        if (a.Optional("environment") is { } hostPath)
        {
            var host = await ResultFiles.ReadAsync(hostPath, ResultJsonContext.Default.EnvironmentSnapshot, token);
            environmentSnapshot = environmentSnapshot with { ContainerInspectJson = host.ContainerInspectJson, ImageDigest = host.ImageDigest,
                Diagnostics = host.Diagnostics };
        }
        if (a.Has("official"))
        {
            if (Environment.GetEnvironmentVariable("DBBENCH_NETWORK") != "1") throw new InvalidOperationException("Official runs require the driver container on the dbbench network.");
            OfficialRunGuard.Validate(environmentSnapshot, entry.GetProperty("kind").GetString() == "embedded");
        }
        var engine = await targetInstance.DescribeAsync(token);
        VerificationFile? verification = a.Optional("verification") is { } verificationPath
            ? await ResultFiles.ReadAsync(verificationPath, ResultJsonContext.Default.VerificationFile, token) : null;
        if (verification != null && (verification.Target != id || verification.DatasetManifestSha256 != manifestHash || verification.Seed != manifest.Seed))
            throw new ArgumentException("Verification belongs to a different target, dataset or seed.");
        if (a.Command == "run" && a.Has("official") && (verification == null || VerificationRunner.Compare([verification]).Count != 0))
            throw new InvalidOperationException("Official run requires a complete successful --verification file.");
        string runId = DateTimeOffset.UtcNow.ToString("yyyyMMddTHHmmssfffffffZ", CultureInfo.InvariantCulture) + "-" + id;
        BenchmarkResult Result(string suite, int repetition, DateTimeOffset started, DateTimeOffset finished) => new()
        {
            RunId = runId, Model = selectedModel.Id, Target = targetInstance.Descriptor, Suite = suite, Repetition = repetition,
            Official = a.Has("official"), StartedUtc = started, FinishedUtc = finished, Engine = engine, Environment = environmentSnapshot,
            DatasetManifestSha256 = manifestHash, Scale = manifest.Scale, Seed = manifest.Seed, Verification = verification,
            Budget = ObservedBudget(environmentSnapshot, entry.GetProperty("kind").GetString() == "embedded"),
        };
        string outputDirectory = a.Get("out", Path.Combine("database", "artifacts", "results"));
        if (a.Command == "load")
        {
            var started = DateTimeOffset.UtcNow;
            var metrics = new List<LoadMetric>();
            var sampler = new ResourceSampler(targetOptions.Container);
            try
            {
                await targetInstance.PrepareAsync(manifest, token);
                metrics.AddRange(await targetInstance.LoadAsync(manifest, directory,
                    new Progress<LoadProgress>(p => Console.WriteLine($"{p.Table}: {p.Rows} rows")), token));
                metrics.AddRange(await targetInstance.FinalizeLoadAsync(token));
            }
            finally { await sampler.DisposeAsync(); }
            long storage = await targetInstance.MeasureStorageAsync(token);
            var result = Result("load", 1, started, DateTimeOffset.UtcNow) with { Load = metrics, StorageBytes = storage, Samples = sampler.Samples };
            await ResultFiles.WriteAsync(Path.Combine(outputDirectory, runId + "-load.json"), result, ResultJsonContext.Default.BenchmarkResult, token);
            Console.WriteLine($"LOAD {id}: {metrics.Sum(m => m.Seconds):0.###} seconds; {storage} stored bytes.");
            return 0;
        }
        var options = new RunnerOptions { Suite = a.Get("suite", "latency"), Mix = a.Get("mix", "read-heavy"),
            Clients = a.Get("clients", a.Get("suite", "latency") == "mixed" ? "8,32" : "1,8,32").Split(',').Select(s => int.Parse(s, CultureInfo.InvariantCulture)).ToArray(),
            Duration = a.Duration("duration"), Warmup = a.Duration("warmup"), Iterations = a.Has("iterations") ? a.Int("iterations", 0) : null,
            Repeat = a.Int("repeat", 3), Seed = a.Has("seed") ? a.Seed : manifest.Seed,
            PointTimeout = a.Duration("point-timeout") ?? TimeSpan.FromSeconds(30),
            AnalyticsTimeout = a.Duration("analytics-timeout") ?? TimeSpan.FromSeconds(600), Container = targetOptions.Container };
        var passes = await new BenchmarkRunner().RunAsync(selectedModel, targetInstance, manifest, operations, options, token);
        foreach (var pass in passes)
        {
            var result = Result(options.Suite, pass.Repetition, pass.StartedUtc, pass.FinishedUtc) with
                { Mix = options.Suite == "mixed" ? options.Mix : null, SuiteBlockReason = pass.BlockReason,
                    Runner = options, Seed = options.Seed, Operations = pass.Operations, Samples = pass.Samples };
            await ResultFiles.WriteAsync(Path.Combine(outputDirectory, $"{runId}-{options.Suite}-{pass.Repetition}.json"), result, ResultJsonContext.Default.BenchmarkResult, token);
        }
        return passes.Any(p => p.BlockReason != null) || passes.SelectMany(p => p.Operations).Any(o => o.Errors + o.WarmupErrors > 0) ? 2 : 0;
    }

    private static ResourceBudget ObservedBudget(EnvironmentSnapshot snapshot, bool embedded)
    {
        // Zero records an unknown/unbounded native limit; never label a development container as the default 6 GiB.
        long.TryParse(snapshot.CgroupMemoryMax, CultureInfo.InvariantCulture, out long driverMemory);
        double cpus = 0;
        long engineMemory = embedded ? driverMemory : 0;
        if (embedded)
        {
            string[] quota = (snapshot.CgroupCpuMax ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (quota.Length == 2 && double.TryParse(quota[0], CultureInfo.InvariantCulture, out double amount) &&
                double.TryParse(quota[1], CultureInfo.InvariantCulture, out double period) && period > 0) cpus = amount / period;
        }
        else if (snapshot.ContainerInspectJson is { } inspection)
        {
            using var json = System.Text.Json.JsonDocument.Parse(inspection);
            var config = json.RootElement[0].GetProperty("HostConfig");
            engineMemory = config.GetProperty("Memory").GetInt64();
            cpus = config.GetProperty("NanoCpus").GetInt64() / 1_000_000_000d;
        }
        return new(cpus, engineMemory, driverMemory);
    }

    private static IBenchmarkModel Model(string id) => Array.Find(Models, m => m.Id == id) ?? throw new ArgumentException("Unknown model " + id);
    private static IReadOnlyList<OperationDescriptor> SelectOperations(IBenchmarkModel model, string? filter)
    {
        if (filter == null) return model.Operations;
        var ids = filter.Split(',');
        foreach (string id in ids) if (!model.Operations.Any(o => o.Id == id)) throw new ArgumentException("Unknown operation " + id);
        return model.Operations.Where(o => ids.Contains(o.Id, StringComparer.Ordinal)).ToArray();
    }
    private static void LoadEngineAssemblies()
    {
        var pending = new Queue<Assembly>();
        pending.Enqueue(typeof(Assimalign.Cohesion.Database.Sql.SqlDatabaseEngine).Assembly);
        pending.Enqueue(typeof(Assimalign.Cohesion.Database.Graph.GraphDatabaseEngine).Assembly);
        pending.Enqueue(typeof(Assimalign.Cohesion.Database.Documents.DocumentDatabaseEngine).Assembly);
        var visited = new HashSet<string>(StringComparer.Ordinal);
        while (pending.TryDequeue(out var assembly))
        {
            if (!visited.Add(assembly.FullName!)) continue;
            foreach (var reference in assembly.GetReferencedAssemblies())
                if (reference.Name!.StartsWith("Assimalign.Cohesion.", StringComparison.Ordinal))
                    pending.Enqueue(Assembly.Load(reference));
        }
    }
}
