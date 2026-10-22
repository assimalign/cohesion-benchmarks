using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http;
using Assimalign.Cohesion.Web.Benchmarks.Fixtures;
using Assimalign.Cohesion.Web.Serialization;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.Serialization;

/// <summary>Registry traversal with the matching registration last; no body serialization.</summary>
public class ContentRegistryBenchmarks
{
    private IContentSerializationFeature _feature = null!;
    private BenchmarkContentCodec _expected = null!;

    [Params(1, 8)]
    public int Registrations { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        ContentSerializationBuilder builder = new();
        for (int i = 1; i < Registrations; i++)
        {
            BenchmarkContentCodec other = new(HttpMediaType.Parse($"application/x-format-{i}"));
            builder.AddReader(other).AddWriter(other);
        }
        _expected = new BenchmarkContentCodec(HttpMediaType.ApplicationJson);
        _feature = builder.AddReader(_expected).AddWriter(_expected).Build();
        if (!ReferenceEquals(Reader(), _expected) || !ReferenceEquals(Writer(), _expected))
        {
            throw new InvalidOperationException("Registry selected the wrong registration.");
        }
    }

    [Benchmark]
    public IContentReader? Reader() => _feature.GetReader(HttpMediaType.ApplicationJson);

    [Benchmark]
    public IContentWriter? Writer() => _feature.GetWriter(HttpMediaType.ApplicationJson);
}

/// <summary>Exact selection, suffix fallback and refusal through the public negotiation seam.</summary>
public class ContentNegotiationBenchmarks
{
    private IContentSerializationFeature _feature = null!;
    private string? _accept;

    [Params(1, 8)]
    public int Registrations { get; set; }

    [Params("None", "Exact", "Suffix", "Rejected")]
    public string Scenario { get; set; } = "None";

    [GlobalSetup]
    public void Setup()
    {
        ContentSerializationBuilder builder = new();
        for (int i = 1; i < Registrations; i++)
        {
            builder.AddWriter(new BenchmarkContentCodec(HttpMediaType.Parse($"application/x-format-{i}")));
        }
        HttpMediaType target = HttpMediaType.Parse(Scenario == "Suffix" ? "application/problem+json" : "application/json");
        _feature = builder.AddWriter(new BenchmarkContentCodec(target)).Build();
        _accept = Scenario switch
        {
            "None" => null,
            "Rejected" => "*/*;q=0",
            _ => "application/json"
        };
        bool selected = _feature.TryNegotiate(_accept, out HttpMediaType actual);
        HttpMediaType expected = Scenario == "None" && Registrations > 1
            ? HttpMediaType.Parse("application/x-format-1") : target;
        if (selected != (Scenario != "Rejected") || (selected && actual != expected))
        {
            throw new InvalidOperationException("Negotiation selected an unexpected representation.");
        }
    }

    [Benchmark]
    public bool Negotiate() => _feature.TryNegotiate(_accept, out _);
}

/// <summary>Negotiated writes using a completed no-op writer to isolate headers and registry work.</summary>
public class NegotiatedWriteBenchmarks
{
    private BenchmarkHttpContext _context = null!;
    private readonly object _value = new();

    [Params("", "Origin", "Accept", "Origin, Accept-Encoding, ACCEPT", "Origin, *", "Origin, Accept-Encoding")]
    public string Vary { get; set; } = "";

    [GlobalSetup]
    public void Setup()
    {
        _context = BenchmarkHttpContext.Create();
        _context.Features.Set<IContentSerializationFeature>(new ContentSerializationBuilder()
            .AddWriter(new BenchmarkContentCodec(HttpMediaType.ApplicationJson)).Build());
        _context.Request.Headers[HttpHeaderKey.Accept] = "application/json";
        if (!Write().GetAwaiter().GetResult())
        {
            throw new InvalidOperationException("Negotiated write failed.");
        }
        string expected = Vary.Length == 0 ? "Accept"
            : Vary is "Origin" or "Origin, Accept-Encoding" ? Vary + ", Accept" : Vary;
        if (_context.Response.Headers.GetValue(HttpHeaderKey.Vary) != expected)
        {
            throw new InvalidOperationException("Negotiated write changed Vary tokens.");
        }
    }

    [Benchmark]
    public Task<bool> Write()
    {
        _context.Response.Headers[HttpHeaderKey.Vary] = Vary;
        return _context.WriteNegotiatedContentAsync(_value, CancellationToken.None);
    }

    [GlobalCleanup]
    public void Cleanup() => _context.Response.Body.Dispose();
}

internal sealed class BenchmarkContentCodec : IContentReader, IContentWriter
{
    internal BenchmarkContentCodec(HttpMediaType mediaType) => MediaTypes = new[] { mediaType };

    public IReadOnlyList<HttpMediaType> MediaTypes { get; }
    public bool CanRead(Type type) => true;
    public bool CanWrite(Type type) => true;
    public ValueTask<object?> ReadAsync(IHttpRequest request, Type type, CancellationToken cancellationToken = default)
        => ValueTask.FromResult<object?>(null);
    public Task WriteAsync(IHttpResponse response, object? value, Type type, HttpMediaType contentType, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
