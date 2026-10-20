using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Resilience.Benchmarks.Resilience.CircuitBreaker;

/// <summary>
/// Repeatedly reject calls through an already-open circuit, including public API exception propagation.
/// A frozen clock keeps the circuit open without sleeping. No Polly ratio: Cohesion wraps a broken circuit
/// in strategy/pipeline aggregate exceptions, whereas Polly throws BrokenCircuitException directly; those
/// output shapes and their allocation costs are materially different. Closed success has a paired benchmark.
/// </summary>
public class OpenCircuitBenchmarks
{
    private IResiliencePipeline<int> _pipeline = null!;

    [GlobalSetup]
    public async Task Setup()
    {
        _pipeline = new ResiliencePipelineBuilder<int>().UseCircuitBreaker(static options =>
        {
            options.FailureThreshold = 1;
            options.TimeProvider = new FrozenTimeProvider();
        }).Build();

        try
        {
            await _pipeline.ExecuteAsync<int, object?>(static (_, _) =>
                ValueTask.FromException<int>(new InvalidOperationException("Open the circuit.")));
        }
        catch (ResilienceException)
        {
            // The first failure opens the circuit; no failure escapes the benchmark harness.
        }

        Exception? rejection = await Reject();
        if (rejection is null || !ContainsBrokenCircuit(rejection))
            throw new InvalidOperationException("Circuit did not open.");
    }

    [Benchmark]
    public async ValueTask<Exception?> Reject()
    {
        try
        {
            await _pipeline.ExecuteAsync<int, object?>(static (_, _) => ValueTask.FromResult(42));
            return null;
        }
        catch (ResilienceException exception)
        {
            return exception;
        }
    }

    private static bool ContainsBrokenCircuit(Exception exception)
    {
        if (exception is BrokenCircuitException) return true;
        if (exception is AggregateException aggregate)
        {
            foreach (Exception inner in aggregate.InnerExceptions)
                if (ContainsBrokenCircuit(inner)) return true;
            return false;
        }

        return exception.InnerException is not null && ContainsBrokenCircuit(exception.InnerException);
    }

    private sealed class FrozenTimeProvider : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => DateTimeOffset.UnixEpoch;
    }
}
