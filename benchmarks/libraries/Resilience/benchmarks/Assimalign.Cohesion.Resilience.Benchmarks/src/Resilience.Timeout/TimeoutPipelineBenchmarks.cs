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
/// The budget is one second rather than the 30-second default. Cohesion's strategy races the callback against an
/// uncancelled <c>Task.Delay(timeout)</c>, so every call leaves a timer pending for the full budget; at benchmark call
/// rates a 30-second budget keeps tens of millions of timers alive (gigabytes of heap), which makes the measurement
/// unstable. One second still pays the per-call timer and allocations while bounding what accumulates.
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
