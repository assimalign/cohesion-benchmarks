using System;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Polly;

namespace Assimalign.Cohesion.Resilience.Benchmarks.Resilience.Retry;

/// <summary>
/// Result-predicate retries with an immediately completing predicate and zero backoff. Both pipelines
/// retry the same negative result, then return 42; includes the first-attempt success fast path.
/// Mutable attempt state is reused and reset per invocation so there is no per-call fixture allocation.
/// </summary>
public class ImmediateRetryBenchmarks
{
    private IResiliencePipeline<int> _cohesion = null!;
    private Polly.ResiliencePipeline<int> _polly = null!;
    private readonly AttemptState _state = new();

    [Params(0, 1, 3)]
    public int Failures { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new ResiliencePipelineBuilder<int>().UseRetry(static options =>
        {
            options.MaxRetryAttempts = 3;
            options.Delay = TimeSpan.Zero;
            options.Retry = static args => ValueTask.FromResult(
                args.Outcome.IsSuccess(out int result) && result < 0);
        }).Build();

        _polly = new Polly.ResiliencePipelineBuilder<int>().AddRetry(new Polly.Retry.RetryStrategyOptions<int>
        {
            MaxRetryAttempts = 3,
            Delay = TimeSpan.Zero,
            ShouldHandle = static args => ValueTask.FromResult(args.Outcome.Result < 0),
        }).Build();
    }

    [Benchmark]
    public ValueTask<int> Execute()
    {
        _state.RemainingFailures = Failures;
        return _cohesion.ExecuteAsync<int, AttemptState>(static (_, state) =>
            ValueTask.FromResult(state!.RemainingFailures-- > 0 ? -1 : 42), _state);
    }

    [Benchmark(Baseline = true)]
    public ValueTask<int> Execute_Polly()
    {
        _state.RemainingFailures = Failures;
        return _polly.ExecuteAsync(static (state, _) =>
            ValueTask.FromResult(state.RemainingFailures-- > 0 ? -1 : 42), _state, CancellationToken.None);
    }

    private sealed class AttemptState
    {
        public int RemainingFailures;
    }
}
