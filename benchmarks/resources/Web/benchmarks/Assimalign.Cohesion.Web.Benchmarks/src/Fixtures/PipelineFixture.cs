using System;
using System.Linq;

using Assimalign.Cohesion.Http;
using Assimalign.Cohesion.Web.Hosting;

namespace Assimalign.Cohesion.Web.Benchmarks.Fixtures;

/// <summary>Builds the production pipeline, including feature stamping, without starting the host or a listener.</summary>
internal sealed class PipelineFixture : IDisposable
{
    public PipelineFixture(params IHttpFeature[] features)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        foreach (IHttpFeature feature in features)
        {
            ((IWebApplicationBuilder)builder).AddFeature(feature);
        }
        Application = builder.Build();
    }

    public WebApplication Application { get; }

    public IWebApplicationPipelineBuilder Builder => Application;

    public IWebApplicationPipeline Build() => Builder.Build();

    public BenchmarkHttpContext CreateContext()
    {
        // Match the server's capacity hint so feature population does not repeatedly grow its dictionary.
        return BenchmarkHttpContext.Create(Application.Context.Features.Count());
    }

    public void Dispose() => ((IAsyncDisposable)Application).DisposeAsync().AsTask().GetAwaiter().GetResult();
}
