using System;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

using Assimalign.Cohesion.Web.Benchmarks.Fixtures;
using Assimalign.Cohesion.Web.RateLimiting;

using BenchmarkDotNet.Attributes;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.RateLimiting;

/// <summary>
/// Global concurrency gate, including lease disposal, feature publication and middleware dispatch.
/// Both use a single BCL partition, one permit and no queue. A setup-held lease makes rejection deterministic
/// without a replenishment timer. Rejection has no Retry-After on either side for this limiter type.
/// </summary>
public class RateLimitingBenchmarks
{
    private PipelineFixture _fixture = null!;
    private IWebApplicationPipeline _pipeline = null!;
    private BenchmarkHttpContext _context = null!;
    private ServiceProvider _services = null!;
    private RequestDelegate _aspNetCore = null!;
    private DefaultHttpContext _aspContext = null!;
    private RateLimitingPolicy _policy = null!;
    private PartitionedRateLimiter<HttpContext> _aspLimiter = null!;
    private RateLimitLease? _held;
    private RateLimitLease? _aspHeld;

    [Params(false, true)]
    public bool Reject { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _policy = RateLimitingPolicy.Create(_ => RateLimitPartition.GetConcurrencyLimiter("global", _ => Options()));
        _aspLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ => RateLimitPartition.GetConcurrencyLimiter("global", _ => Options()));
        _fixture = new PipelineFixture();
        _context = _fixture.CreateContext();
        _fixture.Builder.UseRateLimiting(options => options.GlobalPolicy = _policy);
        _fixture.Builder.Run(_ => Task.CompletedTask);
        _pipeline = _fixture.Build();

        ServiceCollection services = new();
        services.AddLogging();
        services.AddMetrics();
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = _aspLimiter;
            options.RejectionStatusCode = 429;
        });
        _services = services.BuildServiceProvider();
        ApplicationBuilder builder = new(_services);
        builder.UseRateLimiter();
        builder.Run(_ => Task.CompletedTask);
        _aspNetCore = builder.Build();
        _aspContext = new DefaultHttpContext { RequestServices = _services };
        if (Reject)
        {
            _held = _policy.Limiter.AttemptAcquire(_context);
            _aspHeld = _aspLimiter.AttemptAcquire(_aspContext);
        }
        int expected = Reject ? 429 : 200;
        if (Gate().GetAwaiter().GetResult() != expected || Gate_AspNetCore().GetAwaiter().GetResult() != expected)
        {
            throw new InvalidOperationException("The concurrency gate did not produce the expected decision.");
        }
    }

    private static ConcurrencyLimiterOptions Options() => new() { PermitLimit = 1, QueueLimit = 0 };

    [Benchmark]
    public async Task<int> Gate()
    {
        await _pipeline.ExecuteAsync(_context);
        return (int)_context.Response.StatusCode;
    }

    [Benchmark(Baseline = true)]
    public async Task<int> Gate_AspNetCore()
    {
        await _aspNetCore(_aspContext);
        return _aspContext.Response.StatusCode;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _held?.Dispose();
        _aspHeld?.Dispose();
        _policy.Limiter.Dispose();
        _aspLimiter.Dispose();
        _fixture.Dispose();
        _services.Dispose();
    }
}
