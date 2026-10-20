using System;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Polly;

namespace Assimalign.Cohesion.Resilience.Benchmarks.Resilience.Fallback;

/// <summary>
/// The success-path predicate check and a handled exception returning a constant fallback. Both libraries
/// handle only InvalidOperationException and produce the same integer. The exception is prepared in setup;
/// propagating its failed ValueTask and dispatching the fallback are part of the measured work.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class FallbackPipelineBenchmarks
{
    private readonly InvalidOperationException _failure = new("Dependency unavailable.");
    private IResiliencePipeline<int> _cohesion = null!;
    private Polly.ResiliencePipeline<int> _polly = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new ResiliencePipelineBuilder<int>().UseFallback(static options =>
        {
            options.ShouldHandle = static args => ValueTask.FromResult(
                args.Outcome.IsFailure(out Exception? exception) && exception is InvalidOperationException);
            options.FallbackAction = static _ => ValueTask.FromResult(42);
        }).Build();
        _polly = new Polly.ResiliencePipelineBuilder<int>().AddFallback(new Polly.Fallback.FallbackStrategyOptions<int>
        {
            ShouldHandle = static args => ValueTask.FromResult(args.Outcome.Exception is InvalidOperationException),
            FallbackAction = static _ => Polly.Outcome.FromResultAsValueTask(42),
        }).Build();
    }

    [Benchmark, BenchmarkCategory("Success")]
    public ValueTask<int> Success() => _cohesion.ExecuteAsync<int, object?>(static (_, _) => ValueTask.FromResult(42));

    [Benchmark(Baseline = true), BenchmarkCategory("Success")]
    public ValueTask<int> Success_Polly() => _polly.ExecuteAsync(static _ => ValueTask.FromResult(42), CancellationToken.None);

    [Benchmark, BenchmarkCategory("HandledFailure")]
    public ValueTask<int> HandledFailure() => _cohesion.ExecuteAsync<int, InvalidOperationException>(
        static (_, failure) => ValueTask.FromException<int>(failure!), _failure);

    [Benchmark(Baseline = true), BenchmarkCategory("HandledFailure")]
    public ValueTask<int> HandledFailure_Polly() => _polly.ExecuteAsync(
        static (failure, _) => ValueTask.FromException<int>(failure), _failure, CancellationToken.None);
}
