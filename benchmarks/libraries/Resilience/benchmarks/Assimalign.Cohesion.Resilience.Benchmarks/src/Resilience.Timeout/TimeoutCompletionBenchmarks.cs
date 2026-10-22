using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Resilience.Benchmarks.Resilience.Timeout;

/// <summary>
/// Covers the void synchronous path and a callback that suspends once. Task.Yield includes scheduler cost;
/// compare these workloads across package versions rather than treating them as pure timeout-strategy timings.
/// </summary>
[BenchmarkCategory("Timeout")]
public class TimeoutCompletionBenchmarks
{
    private static readonly Func<IResilienceContext, object?, ValueTask> _voidCallback =
        static (_, _) => ValueTask.CompletedTask;

    private static readonly Func<IResilienceContext, object?, ValueTask<int>> _asyncCallback =
        static async (_, _) =>
        {
            await Task.Yield();
            return 42;
        };

    private IResiliencePipeline _voidPipeline = null!;
    private IResiliencePipeline<int> _valuePipeline = null!;

    [GlobalSetup]
    public void Setup()
    {
        _voidPipeline = new ResiliencePipelineBuilder()
            .UseTimeout(static options => options.Timeout = TimeSpan.FromSeconds(1))
            .Build();
        _valuePipeline = new ResiliencePipelineBuilder<int>()
            .UseTimeout(static options => options.Timeout = TimeSpan.FromSeconds(1))
            .Build();
    }

    [Benchmark]
    public ValueTask ExecuteVoid() => _voidPipeline.ExecuteAsync<object?>(_voidCallback);

    [Benchmark]
    public ValueTask<int> ExecuteAsyncValue() => _valuePipeline.ExecuteAsync<int, object?>(_asyncCallback);
}
