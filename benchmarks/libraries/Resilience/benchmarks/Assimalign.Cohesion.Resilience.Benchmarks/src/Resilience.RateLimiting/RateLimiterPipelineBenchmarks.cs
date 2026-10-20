using System;
using System.Threading;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Assimalign.Cohesion.Resilience.Benchmarks.Resilience.RateLimiting;

/// <summary>
/// Acquire an available concurrency permit, invoke the same constant callback, then release the lease.
/// The direct System.Threading.RateLimiting baseline performs the same acquire/check/callback/dispose work;
/// comparisons are restricted to uncontended success because rejection exception contracts differ.
/// Both async and synchronous consumer entry points are measured. Queueing and timer replenishment are excluded.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class RateLimiterPipelineBenchmarks
{
    private ConcurrencyLimiter _cohesionLimiter = null!;
    private ConcurrencyLimiter _directLimiter = null!;
    private IResiliencePipeline<int> _cohesion = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesionLimiter = new(new ConcurrencyLimiterOptions { PermitLimit = 1, QueueLimit = 0 });
        _directLimiter = new(new ConcurrencyLimiterOptions { PermitLimit = 1, QueueLimit = 0 });
        _cohesion = new ResiliencePipelineBuilder<int>()
            .UseRateLimiter(options => options.RateLimiter = _cohesionLimiter).Build();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _cohesionLimiter.Dispose();
        _directLimiter.Dispose();
    }

    private static ValueTask<int> Callback() => ValueTask.FromResult(42);

    [Benchmark, BenchmarkCategory("Async")]
    public ValueTask<int> Execute() => _cohesion.ExecuteAsync<int, object?>(static (_, _) => Callback());

    [Benchmark(Baseline = true), BenchmarkCategory("Async")]
    public async ValueTask<int> Execute_SystemThreadingRateLimiting()
    {
        using RateLimitLease lease = await _directLimiter.AcquireAsync(1, CancellationToken.None);
        if (!lease.IsAcquired) throw new InvalidOperationException("Expected an available permit.");
        return await Callback();
    }

    [Benchmark, BenchmarkCategory("Sync")]
    public int ExecuteSync() => _cohesion.Execute<int, object?>(static (_, _) => Callback().GetAwaiter().GetResult());

    [Benchmark(Baseline = true), BenchmarkCategory("Sync")]
    public int ExecuteSync_SystemThreadingRateLimiting() => Execute_SystemThreadingRateLimiting().GetAwaiter().GetResult();
}
