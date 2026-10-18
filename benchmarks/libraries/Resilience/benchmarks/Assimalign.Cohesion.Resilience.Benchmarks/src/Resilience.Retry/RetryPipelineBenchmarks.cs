using System;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Polly;

using PollyPipeline = Polly.ResiliencePipeline<int>;
using PollyPipelineBuilder = Polly.ResiliencePipelineBuilder<int>;
using PollyRetryOptions = Polly.Retry.RetryStrategyOptions<int>;

namespace Assimalign.Cohesion.Resilience.Benchmarks.Resilience.Retry;

/// <summary>
/// What a retry policy costs a call that succeeds on its first attempt, the overwhelmingly common case: one typed
/// pipeline with a default retry strategy around a synchronously completing callback. Polly.Core is the baseline,
/// so the Ratio column reads "Cohesion relative to the counterpart".
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[BenchmarkCategory("Retry")]
public class RetryPipelineBenchmarks
{
    private static readonly Func<IResilienceContext, object?, ValueTask<int>> Callback =
        static (_, _) => ValueTask.FromResult(42);

    private IResiliencePipeline<int> _cohesion = null!;
    private PollyPipeline _polly = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new ResiliencePipelineBuilder<int>()
            .UseRetry(static _ => { })
            .Build();

        _polly = new PollyPipelineBuilder()
            .AddRetry(new PollyRetryOptions())
            .Build();
    }

    [Benchmark(Baseline = true)]
    public ValueTask<int> Execute_Polly() => _polly.ExecuteAsync(static _ => ValueTask.FromResult(42), CancellationToken.None);

    [Benchmark]
    public ValueTask<int> Execute() => _cohesion.ExecuteAsync<int, object?>(Callback);
}
