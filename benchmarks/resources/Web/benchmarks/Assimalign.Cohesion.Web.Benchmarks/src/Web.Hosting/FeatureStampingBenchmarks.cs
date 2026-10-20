using System;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http;
using Assimalign.Cohesion.Web.Benchmarks.Fixtures;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.Hosting;

/// <summary>
/// The production pipeline stamps application features onto each exchange, then a handler resolves the aggregate.
/// A fresh context charges dictionary population rather than replacement of already-installed singleton features.
/// Standalone: ASP.NET Core has no equivalent public application-feature registration/stamping pipeline contract;
/// comparing only its FeatureCollection.Set would omit the hosting dispatch and aggregate consumption measured here.
/// </summary>
public class FeatureStampingBenchmarks
{
    private PipelineFixture _fixture = null!;
    private IWebApplicationPipeline _pipeline = null!;
    private string[] _names = null!;

    [Params(4, 16)]
    public int FeatureCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        IHttpFeature[] features = new IHttpFeature[FeatureCount];
        _names = new string[FeatureCount];
        for (int i = 0; i < features.Length; i++)
        {
            _names[i] = "benchmark-feature-" + i;
            features[i] = new ApplicationFeature(_names[i], i + 1);
        }
        _fixture = new PipelineFixture(features);
        _fixture.Builder.Run(context =>
        {
            int total = 0;
            foreach (string name in _names)
            {
                total += ((ApplicationFeature)context.Features.Get(name)!).Value;
            }
            context.Items["feature-total"] = total;
            return Task.CompletedTask;
        });
        _pipeline = _fixture.Build();
        if (Dispatch().GetAwaiter().GetResult() != FeatureCount * (FeatureCount + 1) / 2)
        {
            throw new InvalidOperationException("The application feature aggregate was not stamped on the exchange.");
        }
    }

    [Benchmark]
    public async Task<int> Dispatch()
    {
        BenchmarkHttpContext context = BenchmarkHttpContext.Create(featureCapacity: FeatureCount);
        await _pipeline.ExecuteAsync(context);
        return (int)context.Items["feature-total"]!;
    }

    [GlobalCleanup]
    public void Cleanup() => _fixture.Dispose();

    private sealed record ApplicationFeature(string Name, int Value) : IHttpFeature;
}
