using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Closed-loop suite executor. Only ExecuteAsync is timed; parameter construction, deadlines, and accounting are outside.</summary>
public sealed class BenchmarkRunner(TimeProvider? clock = null)
{
    private readonly TimeProvider clock = clock ?? TimeProvider.System;

    public async Task<IReadOnlyList<SuiteMeasurement>> RunAsync(IBenchmarkModel model, IBenchmarkTarget target,
        DatasetManifest manifest, IReadOnlyList<OperationDescriptor> operations, RunnerOptions options,
        CancellationToken cancellationToken = default)
    {
        options.Validate();
        if (operations.Count == 0) throw new ArgumentException("No operations selected.");
        var operationIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var operation in operations)
        {
            if (!operationIds.Add(operation.Id) || operation.ReadHeavyWeight < 0 || operation.BalancedWeight < 0)
                throw new ArgumentException("Operation IDs must be unique and mix weights nonnegative.");
            if (!target.Support.TryGetValue(operation.Id, out var support))
                throw new InvalidOperationException($"Target {target.Descriptor.Id} has no support declaration for {operation.Id}.");
            support.Validate();
        }
        var passes = new List<SuiteMeasurement>();
        for (int repetition = 1; repetition <= options.Repeat; repetition++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var started = clock.GetUtcNow();
            var results = new List<OperationMeasurement>();
            string? blockReason = null;
            var sampler = new ResourceSampler(options.Container);
            try
            {
                var supported = new List<OperationDescriptor>();
                foreach (var operation in operations)
                {
                    var support = target.Support[operation.Id];
                    if (support.Level == SupportLevel.Unsupported)
                        results.Add(new() { Operation = operation.Id, Support = support });
                    else supported.Add(operation);
                }
                if (options.Suite == "mixed")
                {
                    // Preserve unsupported evidence while refusing to silently change the logical workload.
                    foreach (var operation in operations)
                        if (Weight(operation, options) > 0 && target.Support[operation.Id].Level == SupportLevel.Unsupported)
                            blockReason = $"Mixed workload contains unsupported operation {operation.Id}; choose an explicit comparable subset.";
                    supported.RemoveAll(o => Weight(o, options) == 0);
                    if (blockReason == null)
                    {
                        if (supported.Count == 0) throw new ArgumentException("The chosen mix has no weighted operations.");
                        foreach (int clients in options.Clients)
                            results.AddRange(await GroupAsync(model, target, manifest, supported, clients, repetition, options, cancellationToken).ConfigureAwait(false));
                    }
                }
                else
                {
                    foreach (var operation in supported)
                    {
                        int[] counts = options.Suite == "latency" ? [1] : options.Clients;
                        foreach (int clients in counts)
                            results.AddRange(await GroupAsync(model, target, manifest, [operation], clients, repetition, options, cancellationToken).ConfigureAwait(false));
                    }
                }
            }
            finally { await sampler.DisposeAsync().ConfigureAwait(false); }
            passes.Add(new(repetition, started, clock.GetUtcNow(), results, sampler.Samples, blockReason));
        }
        return passes;
    }

    private async Task<List<OperationMeasurement>> GroupAsync(IBenchmarkModel model, IBenchmarkTarget target,
        DatasetManifest manifest, IReadOnlyList<OperationDescriptor> operations, int clients, int repetition, RunnerOptions options,
        CancellationToken token)
    {
        bool latency = options.Suite == "latency";
        bool analytics = operations.Count == 1 && operations[0].TimeoutClass == TimeoutClass.Analytics;
        TimeSpan warmup = options.Warmup ?? TimeSpan.FromSeconds(latency ? 10 : 15);
        TimeSpan duration = options.Duration ?? (latency && analytics ? TimeSpan.MaxValue : TimeSpan.FromSeconds(latency ? 30 : 60));
        int limit = latency ? options.Iterations ?? (analytics ? 5 : 5000) : int.MaxValue;
        int warmupLimit = latency && analytics && options.Warmup == null ? 1 : int.MaxValue;
        int weightTotal = 0;
        foreach (var operation in operations) weightTotal = checked(weightTotal + (options.Suite == "mixed" ? Weight(operation, options) : 1));
        var ready = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var start = new TaskCompletionSource<long>(TaskCreationOptions.RunContinuationsAsynchronously);
        int arrivals = 0;
        using var stop = CancellationTokenSource.CreateLinkedTokenSource(token);
        var tasks = new Task<ClientResult>[clients];
        for (int client = 0; client < clients; client++)
        {
            int index = client;
            tasks[client] = Task.Run(async () =>
            {
                try
                {
                    await using var connection = await target.OpenClientAsync(index, stop.Token).ConfigureAwait(false);
                    var stats = new Counters[operations.Count];
                    for (int i = 0; i < stats.Length; i++) stats[i] = new();
                    await PhaseAsync(false, clock.GetTimestamp(), warmup, warmupLimit).ConfigureAwait(false);
                    if (Interlocked.Increment(ref arrivals) == clients) ready.TrySetResult();
                    long begun = await start.Task.WaitAsync(stop.Token).ConfigureAwait(false);
                    await PhaseAsync(true, begun, duration, limit).ConfigureAwait(false);
                    // Exclude disposal from throughput duration.
                    return new ClientResult(stats, clock.GetTimestamp());

                    async Task PhaseAsync(bool measured, long begun, TimeSpan budget, int iterations)
                    {
                        var streams = new IParameterStream[operations.Count];
                        ulong seed = measured ? options.Seed : options.Seed ^ 0x8A5CD789635D2DFFUL;
                        var context = new ParameterStreamContext(options.Suite == "mixed" ? "mixed/" + options.Mix : options.Suite,
                            repetition, clients, !measured);
                        for (int i = 0; i < streams.Length; i++)
                            streams[i] = model.CreateParameterStream(manifest, seed, operations[i].Id, index, context);
                        var selector = new DeterministicSequence(seed, "mix:" + options.Mix, index);
                        for (int iteration = 0; iteration < iterations && clock.GetElapsedTime(begun) < budget; iteration++)
                        {
                            stop.Token.ThrowIfCancellationRequested();
                            int selected = 0;
                            if (operations.Count > 1)
                            {
                                int pick = (int)(selector.Next() % (ulong)weightTotal);
                                while (pick >= Weight(operations[selected], options)) pick -= Weight(operations[selected++], options);
                            }
                            var operation = operations[selected];
                            var counters = stats[selected];
                            object parameters = streams[selected].Next();
                            TimeSpan timeout = operation.TimeoutClass == TimeoutClass.Point ? options.PointTimeout : options.AnalyticsTimeout;
                            using var deadline = CancellationTokenSource.CreateLinkedTokenSource(stop.Token);
                            deadline.CancelAfter(timeout);
                            bool success = false;
                            bool timedOut = false;
                            string? failure = null;
                            long elapsedTicks;
                            long before = clock.GetTimestamp();
                            try
                            {
                                await connection.ExecuteAsync(operation, parameters, null, deadline.Token).ConfigureAwait(false);
                                elapsedTicks = clock.GetTimestamp() - before;
                                success = true;
                            }
                            catch (Exception ex) when (ex is not OutOfMemoryException)
                            {
                                elapsedTicks = clock.GetTimestamp() - before;
                                stop.Token.ThrowIfCancellationRequested();
                                timedOut = deadline.IsCancellationRequested || ex is TimeoutException;
                                failure = ex.GetType().Name + ": " + ex.Message;
                            }
                            TimeSpan elapsed = clock.GetElapsedTime(0, elapsedTicks);
                            if (elapsed >= timeout || deadline.IsCancellationRequested) { success = false; timedOut = true; }
                            if (elapsed.TotalMicroseconds > LatencyHistogram.HighestTrackableValue)
                            { success = false; failure = "Operation exceeded histogram's one-hour bound."; }
                            if (!measured)
                            {
                                counters.WarmupAttempts++;
                                if (!success) counters.WarmupErrors++;
                            }
                            else if (success) counters.Histogram.Record(Math.Max(1, (long)Math.Ceiling(elapsed.TotalMicroseconds)));
                            else
                            {
                                counters.Errors++;
                                if (timedOut) counters.Timeouts++;
                            }
                            if (!success && counters.Examples.Count < 3)
                                counters.Examples.Add((measured ? "" : "warmup: ") + (timedOut ? "timeout: " : "") + (failure ?? "Deadline exceeded."));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ready.TrySetException(ex);
                    start.TrySetException(ex);
                    await stop.CancelAsync().ConfigureAwait(false);
                    throw;
                }
            }, CancellationToken.None);
        }
        long started;
        ClientResult[] completed;
        try
        {
            await ready.Task.WaitAsync(token).ConfigureAwait(false);
            started = clock.GetTimestamp();
            start.TrySetResult(started);
            completed = await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        catch
        {
            await stop.CancelAsync().ConfigureAwait(false);
            start.TrySetCanceled(stop.Token);
            try { await Task.WhenAll(tasks).ConfigureAwait(false); } catch { }
            throw;
        }
        long ended = started;
        foreach (var client in completed) ended = Math.Max(ended, client.Ended);
        double seconds = clock.GetElapsedTime(started, ended).TotalSeconds;
        var result = new List<OperationMeasurement>();
        for (int i = 0; i < operations.Count; i++)
        {
            var merged = new Counters();
            foreach (var client in completed)
            {
                var source = client.Stats[i];
                merged.Histogram.Merge(source.Histogram);
                merged.Errors += source.Errors;
                merged.Timeouts += source.Timeouts;
                merged.WarmupAttempts += source.WarmupAttempts;
                merged.WarmupErrors += source.WarmupErrors;
                foreach (string example in source.Examples) if (merged.Examples.Count < 3) merged.Examples.Add(example);
            }
            var h = merged.Histogram;
            result.Add(new()
            {
                Operation = operations[i].Id, Support = target.Support[operations[i].Id], Clients = clients,
                Successes = h.Count, Errors = merged.Errors, Timeouts = merged.Timeouts, Seconds = seconds,
                WarmupAttempts = merged.WarmupAttempts, WarmupErrors = merged.WarmupErrors,
                P50Us = h.Percentile(50), P90Us = h.Percentile(90), P95Us = h.Percentile(95), P99Us = h.Percentile(99),
                P999Us = h.Percentile(99.9), MaxUs = h.Maximum, Histogram = h.Encode(), ErrorExamples = merged.Examples,
            });
        }
        return result;
    }

    private static int Weight(OperationDescriptor operation, RunnerOptions options)
        => options.Mix == "balanced" ? operation.BalancedWeight : operation.ReadHeavyWeight;
    private sealed class Counters
    {
        public readonly LatencyHistogram Histogram = new();
        public long Errors, Timeouts, WarmupAttempts, WarmupErrors;
        public readonly List<string> Examples = [];
    }
    private sealed record ClientResult(Counters[] Stats, long Ended);
}
