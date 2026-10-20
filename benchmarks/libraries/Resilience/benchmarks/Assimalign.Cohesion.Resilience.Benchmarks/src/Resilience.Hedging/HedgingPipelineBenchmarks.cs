using System;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Polly;

namespace Assimalign.Cohesion.Resilience.Benchmarks.Resilience.Hedging;

/// <summary>
/// Successful primary attempt with one spare hedge and a one-second hedge budget. The callback completes
/// synchronously, so no delay is awaited and no extra attempt runs. Cohesion counts the primary in
/// MaxHedgedAttempts; Polly counts only extra attempts. Both cancel any pending hedge when the primary wins.
/// </summary>
public class HedgingPipelineBenchmarks
{
    private IResiliencePipeline<int> _cohesion = null!;
    private Polly.ResiliencePipeline<int> _polly = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new ResiliencePipelineBuilder<int>().UseHedging(static options =>
        {
            options.MaxHedgedAttempts = 2;
            options.Delay = TimeSpan.FromSeconds(1);
        }).Build();
        _polly = new Polly.ResiliencePipelineBuilder<int>().AddHedging(new Polly.Hedging.HedgingStrategyOptions<int>
        {
            MaxHedgedAttempts = 1,
            Delay = TimeSpan.FromSeconds(1),
        }).Build();
    }

    [Benchmark]
    public ValueTask<int> Execute() => _cohesion.ExecuteAsync<int, object?>(static (_, _) => ValueTask.FromResult(42));

    [Benchmark(Baseline = true)]
    public ValueTask<int> Execute_Polly() => _polly.ExecuteAsync(static _ => ValueTask.FromResult(42), CancellationToken.None);
}
