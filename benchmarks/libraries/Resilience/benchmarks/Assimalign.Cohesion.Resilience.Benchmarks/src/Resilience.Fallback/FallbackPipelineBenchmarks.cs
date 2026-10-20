using System;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Polly;

namespace Assimalign.Cohesion.Resilience.Benchmarks.Resilience.Fallback;

/// <summary>
/// The success-path predicate check and a handled exception returning a constant fallback. Both libraries
/// handle only InvalidOperationException and produce the same integer. Propagating the failed ValueTask and
/// dispatching the fallback are the measured work.
/// </summary>
/// <remarks>
/// The failure is constructed per operation, not once in setup. A single exception instance rethrown by every
/// operation accumulates stack-trace state, so the cost climbs with every invocation: the first version of this
/// class measured 3.3 ms on its first iteration and 6.2 ms by its eighteenth, which is not a number about
/// resilience at all, and it eventually tripped BenchmarkDotNet's "takes too long for an in-process toolchain"
/// guard and failed the whole suite. Both sides allocate the same way, so the comparison stays fair.
/// </remarks>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class FallbackPipelineBenchmarks
{
    private static InvalidOperationException Failure() => new("Dependency unavailable.");
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
        static (_, failure) => ValueTask.FromException<int>(failure!), Failure());

    [Benchmark(Baseline = true), BenchmarkCategory("HandledFailure")]
    public ValueTask<int> HandledFailure_Polly() => _polly.ExecuteAsync(
        static (failure, _) => ValueTask.FromException<int>(failure), Failure(), CancellationToken.None);
}
