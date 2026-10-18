using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Assimalign.Cohesion.Web.Benchmarks;
using Assimalign.Cohesion.Web.Benchmarks.Runner;
using Assimalign.Cohesion.Web.Benchmarks.Runner.Load;
using Assimalign.Cohesion.Web.Benchmarks.Runner.Reporting;
using Assimalign.Cohesion.Web.Benchmarks.Runner.Scenarios;
using Assimalign.Cohesion.Web.Benchmarks.Runner.Servers;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

RunnerOptions options;
try
{
    options = RunnerOptions.Parse(args);
}
catch (ArgumentException exception)
{
    Console.Error.WriteLine(exception.Message);
    Console.Error.WriteLine(RunnerOptions.Usage);
    return 2;
}

if (options.ShowHelp)
{
    Console.WriteLine(RunnerOptions.Usage);
    return 0;
}

if (options.List)
{
    foreach (LoadScenario scenario in ScenarioCatalog.All)
    {
        Console.WriteLine($"{scenario.Id,-22} {scenario.Category,-8} {scenario.Description}");
    }
    return 0;
}

IReadOnlyList<LoadScenario> scenarios = ScenarioCatalog.Select(options.Scenarios);
int processors = Environment.ProcessorCount;
// On loopback the load generator spends roughly 1.5-2x the server's CPU per request (it does the same
// socket work without the framework to amortize it), so the server gets a third of the machine by default.
// On a 12-core Snapdragon X Elite that is one 4-core cluster for the server and two for the client.
int serverCores = options.ServerCores ?? Math.Max(1, processors / 3);
int clientCores = Math.Max(1, processors - serverCores);
long serverMask = options.Affinity ? Mask(0, serverCores) : 0;
long clientMask = options.Affinity ? Mask(serverCores, clientCores) : 0;
if (clientMask != 0 && (OperatingSystem.IsWindows() || OperatingSystem.IsLinux()))
{
    Process.GetCurrentProcess().ProcessorAffinity = (nint)clientMask;
}

var environment = EnvironmentInfo.Capture(serverCores, clientCores, options);
Directory.CreateDirectory(options.Output);

Console.WriteLine($"Cohesion Web benchmarks: {scenarios.Count} scenarios x {options.Frameworks.Count} frameworks x {options.Iterations} iteration(s)");
Console.WriteLine($"  machine     {environment.Cpu}, {processors} logical processors, {environment.Os}");
Console.WriteLine($"  cores       server {serverCores} (mask 0x{serverMask:X}), client {clientCores} (mask 0x{clientMask:X})");
Console.WriteLine($"  load        {options.Connections} connections, {options.Warmup.TotalSeconds:0}s warmup, {options.Duration.TotalSeconds:0}s measured, GC {options.Gc}");
Console.WriteLine($"  output      {Path.GetFullPath(options.Output)}");
Console.WriteLine();

// The first launch of a freshly published executable pays for disk reads and on-access antivirus scans,
// which would land in whichever scenario runs first. Launch each server once, unmeasured.
foreach (string framework in options.Frameworks)
{
    int primePort = ServerProcess.GetFreePort();
    var primeEndpoint = new IPEndPoint(IPAddress.Loopback, primePort);
    using ServerProcess prime = ServerProcess.Start(options.Executables[framework], ServerScenarios.Plaintext, primePort, serverCores, serverMask, options.Gc);
    try
    {
        byte[] probe = new Assimalign.Cohesion.Web.Benchmarks.Runner.Http.HttpRequestTemplate("GET", "/__ready").Encode(primeEndpoint.ToString());
        double coldStart = await prime.WaitUntilReadyAsync(primeEndpoint, probe, TimeSpan.FromSeconds(60));
        Console.WriteLine($"  primed      {framework} (cold start {coldStart:0} ms, not reported)");
    }
    catch (Exception exception)
    {
        Console.WriteLine($"  primed      {framework} failed: {exception.GetBaseException().Message.Split(Environment.NewLine)[0]}");
    }
}
Console.WriteLine();

var records = new List<RunRecord>();
int step = 0;
int total = scenarios.Count * options.Frameworks.Count * options.Iterations;
for (int s = 0; s < scenarios.Count; s++)
{
    LoadScenario scenario = scenarios[s];
    for (int iteration = 1; iteration <= options.Iterations; iteration++)
    {
        // Alternate which framework goes first so neither always runs on a warmer (or more throttled) machine.
        IEnumerable<string> order = (s + iteration) % 2 == 0 ? options.Frameworks : options.Frameworks.AsEnumerable().Reverse();
        foreach (string framework in order)
        {
            step++;
            RunRecord record = await RunOneAsync(scenario, framework, iteration);
            records.Add(record);
            Console.WriteLine($"[{step,3}/{total}] {Describe(record)}");
            await Task.Delay(options.Cooldown);
        }
    }
}

var report = new RunReport(environment, records, scenarios.Select(ScenarioInfo.From).ToList());
string jsonPath = Path.Combine(options.Output, "results.json");
await File.WriteAllTextAsync(jsonPath, JsonSerializer.Serialize(report, ReportJson.Options));
string markdownPath = Path.Combine(options.Output, "results.md");
await File.WriteAllTextAsync(markdownPath, MarkdownReport.Render(report, options.ValidateOnly));

Console.WriteLine();
Console.WriteLine($"Wrote {jsonPath}");
Console.WriteLine($"Wrote {markdownPath}");
return records.Any(record => !record.Passed) ? 1 : 0;

async Task<RunRecord> RunOneAsync(LoadScenario scenario, string framework, int iteration)
{
    string executable = options.Executables[framework];
    int port = ServerProcess.GetFreePort();
    var endpoint = new IPEndPoint(IPAddress.Loopback, port);
    var record = new RunRecord(scenario.Id, framework, iteration);

    using ServerProcess server = ServerProcess.Start(executable, scenario.Server, port, serverCores, serverMask, options.Gc);
    try
    {
        byte[] probe = new Assimalign.Cohesion.Web.Benchmarks.Runner.Http.HttpRequestTemplate("GET", "/__ready").Encode(endpoint.ToString());
        record.StartupMilliseconds = await server.WaitUntilReadyAsync(endpoint, probe, TimeSpan.FromSeconds(30));
        record.ServerInfo = await server.GetInfoAsync(TimeSpan.FromSeconds(5));

        using var validationTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        ValidationResult validation = await LoadGenerator.ValidateAsync(scenario, endpoint, validationTimeout.Token);
        record.Validation = new ValidationSummary(validation.Status, validation.HeadLength, validation.BodyLength, validation.Head, validation.BodyPreview);
        if (!validation.Passed)
        {
            record.Passed = false;
            record.Failure = "validation: " + validation.Failure;
            return record;
        }

        if (options.ValidateOnly)
        {
            record.Passed = true;
            return record;
        }

        TimeSpan serverCpuStart = default, clientCpuStart = default;
        TimeSpan serverCpuEnd = default, clientCpuEnd = default;
        Process self = Process.GetCurrentProcess();
        LoadResult result = await LoadGenerator.RunAsync(
            scenario,
            endpoint,
            options.Connections,
            options.Warmup,
            options.Duration,
            onMeasureStart: () =>
            {
                serverCpuStart = server.TotalProcessorTime;
                self.Refresh();
                clientCpuStart = self.TotalProcessorTime;
            },
            onMeasureStop: () =>
            {
                serverCpuEnd = server.TotalProcessorTime;
                self.Refresh();
                clientCpuEnd = self.TotalProcessorTime;
            });

        (long workingSet, long peakWorkingSet, long privateBytes) = server.Memory;
        double serverCpuSeconds = (serverCpuEnd - serverCpuStart).TotalSeconds;
        double clientCpuSeconds = (clientCpuEnd - clientCpuStart).TotalSeconds;

        record.Passed = result.Requests > 0 && result.Errors == 0;
        record.Failure = result.Requests == 0 ? "no responses during the measured phase" : result.Errors > 0 ? $"{result.Errors} socket errors" : null;
        record.RequestsPerSecond = result.RequestsPerSecond;
        record.Requests = result.Requests;
        record.ElapsedSeconds = result.ElapsedSeconds;
        record.Errors = result.Errors;
        record.Reconnects = result.Reconnects;
        record.StatusClasses = result.StatusClasses;
        record.MegabytesPerSecond = result.BytesPerSecond / (1024 * 1024);
        record.Latency = LatencySummary.From(result.Latency);
        record.ServerCpuPercent = serverCpuSeconds / (result.ElapsedSeconds * serverCores) * 100;
        record.ClientCpuPercent = clientCpuSeconds / (result.ElapsedSeconds * clientCores) * 100;
        record.RequestsPerCpuSecond = serverCpuSeconds > 0 ? result.Requests / serverCpuSeconds : null;
        record.WorkingSetMegabytes = workingSet / (1024.0 * 1024);
        record.PeakWorkingSetMegabytes = peakWorkingSet / (1024.0 * 1024);
        record.PrivateMegabytes = privateBytes / (1024.0 * 1024);

        long unexpected = 0;
        for (int statusClass = 1; statusClass < result.StatusClasses.Length; statusClass++)
        {
            if (!scenario.ExpectedStatus.Any(expected => expected / 100 == statusClass) && scenario.Kind == LoadKind.Http)
            {
                unexpected += result.StatusClasses[statusClass];
            }
        }
        if (unexpected > 0)
        {
            record.Passed = false;
            record.Failure = $"{unexpected} responses outside the expected status class";
        }

        return record;
    }
    catch (Exception exception)
    {
        record.Passed = false;
        record.Failure = exception.GetBaseException().Message.Split(Environment.NewLine)[0];
        return record;
    }
}

static string Describe(RunRecord record)
{
    string head = $"{record.Scenario,-22} {record.Framework,-10}";
    if (!record.Passed)
    {
        return $"{head} FAILED  {record.Failure}";
    }

    if (record.RequestsPerSecond is null)
    {
        return $"{head} ok      status {record.Validation?.Status}, head {record.Validation?.HeadBytes} B, body {record.Validation?.BodyBytes} B";
    }

    return $"{head} {record.RequestsPerSecond,12:N0} rps  p50 {record.Latency!.P50,7:0.000} ms  p99 {record.Latency.P99,7:0.000} ms  " +
           $"cpu {record.ServerCpuPercent,5:0}%  client {record.ClientCpuPercent,5:0}%  start {record.StartupMilliseconds,6:0} ms  ws {record.WorkingSetMegabytes,6:0.0} MB";
}

static long Mask(int first, int count)
{
    long mask = 0;
    for (int i = first; i < first + count && i < 63; i++)
    {
        mask |= 1L << i;
    }
    return mask;
}
