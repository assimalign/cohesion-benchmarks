using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Resilience.Benchmarks.Resilience;

/// <summary>
/// Dispatch through the public builder's ordered strategy chain. In this build a successful strategy
/// short-circuits the chain; a failed strategy forwards to the next strategy and invokes the callback again.
/// Polly nests strategies around one callback, so it is not an equivalent composition baseline.
/// FirstSuccess measures normal dispatch; LastSuccess traverses the chain with immediate failures, no delays.
/// </summary>
public class CompositionBenchmarks
{
    private static readonly InvalidOperationException Failure = new("Dependency unavailable.");
    private IResiliencePipeline<int> _pipeline = null!;
    private readonly AttemptState _state = new();

    [Params(2, 4)]
    public int StrategyCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        IResiliencePipelineBuilder<int> builder = new ResiliencePipelineBuilder<int>();
        for (int i = 0; i < StrategyCount - 1; i++)
        {
            builder.UseRetry(static options =>
            {
                options.Delay = TimeSpan.Zero;
                options.Retry = static _ => ValueTask.FromResult(false);
            });
        }

        _pipeline = builder.UseFallback(static options =>
            options.FallbackAction = static _ => ValueTask.FromResult(42)).Build();
    }

    [Benchmark]
    public ValueTask<int> FirstSuccess() => _pipeline.ExecuteAsync<int, object?>(
        static (_, _) => ValueTask.FromResult(42));

    [Benchmark]
    public ValueTask<int> LastSuccess()
    {
        _state.RemainingFailures = StrategyCount - 1;
        return _pipeline.ExecuteAsync<int, AttemptState>(static (_, state) =>
            state!.RemainingFailures-- > 0
                ? ValueTask.FromException<int>(Failure)
                : ValueTask.FromResult(42), _state);
    }

    private sealed class AttemptState
    {
        public int RemainingFailures;
    }
}
