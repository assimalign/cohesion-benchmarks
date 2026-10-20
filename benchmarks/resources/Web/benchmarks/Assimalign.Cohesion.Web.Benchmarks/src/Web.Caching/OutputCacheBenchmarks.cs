using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http;
using Assimalign.Cohesion.Web.Benchmarks.Fixtures;
using Assimalign.Cohesion.Web.Caching;

using BenchmarkDotNet.Attributes;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.Caching;

/// <summary>
/// A warmed anonymous GET output-cache hit or Authorization-header bypass with the same 1 KiB body and one-day TTL.
/// Both middleware select a base policy, build the key, consult their in-memory store and replay status/headers/body.
/// Setup delivers successful response completion to populate Cohesion's cache, as the server would after sending.
/// This excludes fill/eviction and concurrent request coalescing; no handler work is hidden in the hit measurement.
/// </summary>
public class OutputCacheBenchmarks
{
    private PipelineFixture _fixture = null!;
    private IWebApplicationPipeline _pipeline = null!;
    private BenchmarkHttpContext _context = null!;
    private ServiceProvider _services = null!;
    private RequestDelegate _aspNetCore = null!;
    private DefaultHttpContext _aspContext = null!;
    private InMemoryOutputCacheStore _store = null!;
    private readonly byte[] _body = new byte[1024];
    private int _calls;
    private int _aspCalls;

    [Params(false, true)]
    public bool Bypass { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _store = new InMemoryOutputCacheStore();
        _fixture = new PipelineFixture();
        _fixture.Builder.UseOutputCache(_store, options => options.AddBasePolicy(policy => policy.Duration = TimeSpan.FromDays(1)));
        _fixture.Builder.Run(async context =>
        {
            _calls++;
            context.Response.Headers[HttpHeaderKey.ContentType] = "application/octet-stream";
            await context.Response.Body.WriteAsync(_body);
        });
        _pipeline = _fixture.Build();
        _context = _fixture.CreateContext();
        CompletionFeature completion = new();
        _context.Features.Set<IWebResponseCompletionFeature>(completion);
        _context.Request.Host = new HttpHost("bench.example");
        if (Bypass)
        {
            _context.Request.Headers[HttpHeaderKey.Authorization] = "Bearer opaque";
        }

        ServiceCollection services = new();
        services.AddLogging();
        services.AddOutputCache(options => options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromDays(1))));
        _services = services.BuildServiceProvider();
        ApplicationBuilder builder = new(_services);
        builder.UseOutputCache();
        builder.Run(async context =>
        {
            _aspCalls++;
            context.Response.ContentType = "application/octet-stream";
            await context.Response.Body.WriteAsync(_body);
        });
        _aspNetCore = builder.Build();
        _aspContext = new DefaultHttpContext { RequestServices = _services };
        _aspContext.Request.Scheme = "http";
        _aspContext.Request.Host = new HostString("bench.example");
        _aspContext.Request.Method = "GET";
        _aspContext.Request.Path = "/";
        _aspContext.Response.Body = new MemoryStream(2048);
        if (Bypass)
        {
            _aspContext.Request.Headers.Authorization = "Bearer opaque";
        }

        Serve().GetAwaiter().GetResult();
        completion.CompleteAsync().AsTask().GetAwaiter().GetResult();
        Serve_AspNetCore().GetAwaiter().GetResult();
        int calls = _calls;
        int aspCalls = _aspCalls;
        long cohesionLength = Serve().GetAwaiter().GetResult();
        long aspLength = Serve_AspNetCore().GetAwaiter().GetResult();
        if (cohesionLength != _body.Length || aspLength != _body.Length
            || _calls - calls != (Bypass ? 1 : 0) || _aspCalls - aspCalls != (Bypass ? 1 : 0))
        {
            throw new InvalidOperationException($"Output cache mismatch: bypass={Bypass}, lengths={cohesionLength}/{aspLength}, handler deltas={_calls - calls}/{_aspCalls - aspCalls}.");
        }
    }

    [Benchmark]
    public async Task<long> Serve()
    {
        _context.Response.Body.SetLength(0);
        _context.Response.Headers.Clear();
        await _pipeline.ExecuteAsync(_context);
        return _context.Response.Body.Length;
    }

    [Benchmark(Baseline = true)]
    public async Task<long> Serve_AspNetCore()
    {
        _aspContext.Response.Body.SetLength(0);
        _aspContext.Response.Headers.Clear();
        await _aspNetCore(_aspContext);
        return _aspContext.Response.Body.Length;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _context.Response.Body.Dispose();
        _aspContext.Response.Body.Dispose();
        _fixture.Dispose();
        _store.Dispose();
        _services.Dispose();
    }

    // The in-memory exchange has no transport to report successful transmission. Only setup needs
    // this server notification; measured hits and authenticated bypasses register no completion work.
    private sealed class CompletionFeature : IWebResponseCompletionFeature
    {
        private readonly List<Func<ValueTask>> _callbacks = new();

        public string Name => nameof(IWebResponseCompletionFeature);

        public void Register(Func<ValueTask> callback) => _callbacks.Add(callback);

        public async ValueTask CompleteAsync()
        {
            foreach (Func<ValueTask> callback in _callbacks)
            {
                await callback();
            }
            _callbacks.Clear();
        }
    }
}
