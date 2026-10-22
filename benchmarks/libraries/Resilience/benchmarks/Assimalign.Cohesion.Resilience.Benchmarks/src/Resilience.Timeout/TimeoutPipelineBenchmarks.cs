using System;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Polly;

using PollyPipeline = Polly.ResiliencePipeline<int>;
using PollyPipelineBuilder = Polly.ResiliencePipelineBuilder<int>;

namespace Assimalign.Cohesion.Resilience.Benchmarks.Resilience.Timeout;

/// <summary>
/// What a timeout policy costs a call that completes well inside its budget: the strategy still links a
/// cancellation source and arms (then disarms) a timer per call. Polly.Core is the baseline, so the Ratio column
/// reads "Cohesion relative to the counterpart".
/// </summary>
/// <remarks>
/// The one-second budget preserves the historical workload. Before the timer-lifetime fix, successful calls left
/// an uncancelled delay timer pending for the full budget; a 30-second budget made that version unstable at benchmark
/// call rates. Keep this input unchanged when comparing versions with and without the fix.
/// </remarks>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[BenchmarkCategory("Timeout")]
public class TimeoutPipelineBenchmarks
{
    private static readonly TimeSpan Budget = TimeSpan.FromSeconds(1);

    private static readonly Func<IResilienceContext, object?, ValueTask<int>> Callback =
        static (_, _) => ValueTask.FromResult(42);

    private IResiliencePipeline<int> _cohesion = null!;
    private PollyPipeline _polly = null!;

    [GlobalSetup]
    public void Setup()
    {
        _cohesion = new ResiliencePipelineBuilder<int>()
            .UseTimeout(static options => options.Timeout = Budget)
            .Build();

        _polly = new PollyPipelineBuilder()
            .AddTimeout(Budget)
            .Build();
    }

    [Benchmark(Baseline = true)]
    public ValueTask<int> Execute_Polly() => _polly.ExecuteAsync(static _ => ValueTask.FromResult(42), CancellationToken.None);

    [Benchmark]
    public ValueTask<int> Execute() => _cohesion.ExecuteAsync<int, object?>(Callback);
}
