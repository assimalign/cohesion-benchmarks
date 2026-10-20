using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http;
using Assimalign.Cohesion.Web.Benchmarks.Fixtures;
using Assimalign.Cohesion.Web.Serialization;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using Microsoft.AspNetCore.Http;

namespace Assimalign.Cohesion.Web.Benchmarks.Web.Serialization;

/// <summary>
/// Read/write UTF-8 JSON through the HTTP content APIs, with the same source-generated web contracts and streams.
/// Both validate JSON Content-Type on reads and stamp it on writes; serializer-only calls would omit that work.
/// Stream rewind/truncation is included equally to bound memory over repeated invocations.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class JsonContentBenchmarks
{
    private BenchmarkHttpContext _context = null!;
    private DefaultHttpContext _aspContext = null!;
    private JsonPayload _value = null!;
    private MemoryStream _input = null!;
    private MemoryStream _output = null!;

    [Params(32, 4096)]
    public int TextLength { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _value = new JsonPayload(42, new string('x', TextLength));
        _input = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(_value, ContentJsonContext.Default.JsonPayload));
        _output = new MemoryStream(TextLength + 256);
        _context = BenchmarkHttpContext.Create();
        _context.Features.Set(new ContentSerializationBuilder().AddJson(ContentJsonContext.Default).Build());
        _context.Request.Headers[HttpHeaderKey.ContentType] = "application/json";
        _context.Request.Body = _input;
        _context.Response.Body = _output;
        _aspContext = new DefaultHttpContext();
        _aspContext.Request.ContentType = "application/json";
        _aspContext.Request.Body = _input;
        _aspContext.Response.Body = _output;
        if (Read().GetAwaiter().GetResult() != _value || Read_AspNetCore().GetAwaiter().GetResult() != _value)
        {
            throw new InvalidOperationException("JSON request round trip failed.");
        }
        Write().GetAwaiter().GetResult();
        byte[] expected = _output.ToArray();
        Write_AspNetCore().GetAwaiter().GetResult();
        if (!expected.AsSpan().SequenceEqual(_output.ToArray()))
        {
            throw new InvalidOperationException("The JSON response bytes differ.");
        }
    }

    [Benchmark, BenchmarkCategory("Read")]
    public ValueTask<JsonPayload?> Read()
    {
        _input.Position = 0;
        return _context.Request.ReadContentAsync<JsonPayload>();
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Read")]
    public ValueTask<JsonPayload?> Read_AspNetCore()
    {
        _input.Position = 0;
        return _aspContext.Request.ReadFromJsonAsync(ContentJsonContext.Default.JsonPayload);
    }

    [Benchmark, BenchmarkCategory("Write")]
    public async Task<long> Write()
    {
        _output.SetLength(0);
        await _context.Response.WriteContentAsync(_value);
        return _output.Length;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Write")]
    public async Task<long> Write_AspNetCore()
    {
        _output.SetLength(0);
        await _aspContext.Response.WriteAsJsonAsync(_value, ContentJsonContext.Default.JsonPayload);
        return _output.Length;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _input.Dispose();
        _output.Dispose();
    }
}

public sealed record JsonPayload(int Id, string Text);

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(JsonPayload))]
internal partial class ContentJsonContext : JsonSerializerContext;
