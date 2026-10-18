using System;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Polly;

using PollyBreakerOptions = Polly.CircuitBreaker.CircuitBreakerStrategyOptions<int>;
using PollyPipeline = Polly.ResiliencePipeline<int>;
using PollyPipelineBuilder = Polly.ResiliencePipelineBuilder<int>;

namespace Assimalign.Cohesion.Resilience.Benchmarks.Resilience.CircuitBreaker;

/// <summary>
/// What a closed circuit breaker costs a successful call: the state check before the callback and the success
/// bookkeeping after it, paid on every call to a healthy dependency. Polly.Core is the baseline, so the Ratio
/// column reads "Cohesion relative to the counterpart".
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[BenchmarkCategory("CircuitBreaker")]
public class CircuitBreakerPipelineBenchmarks
{
    private static readonly Func<IResilienceContext, object?, ValueTask<int>> Callback =
        static (_, _) => ValueTask.FromResult(42);

    private IResiliencePipeline<int> _cohesion = null!;
    private PollyPipeline _polly = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new ResiliencePipelineBuilder<int>()
            .UseCircuitBreaker(static _ => { })
            .Build();

        _polly = new PollyPipelineBuilder()
            .AddCircuitBreaker(new PollyBreakerOptions())
            .Build();
    }

    [Benchmark(Baseline = true)]
    public ValueTask<int> Execute_Polly() => _polly.ExecuteAsync(static _ => ValueTask.FromResult(42), CancellationToken.None);

    [Benchmark]
    public ValueTask<int> Execute() => _cohesion.ExecuteAsync<int, object?>(Callback);
}
