using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Assimalign.Cohesion.Http.Benchmarks.Fixtures;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using AspNet = Microsoft.AspNetCore.Http;
using AspNetFeatures = Microsoft.AspNetCore.Http.Features;

namespace Assimalign.Cohesion.Http.Benchmarks.Http.Forms;

/// <summary>
/// Public form-feature decoding to a multivalue field collection, compared with ASP.NET Core's
/// FormFeature (which uses WebUtilities). Both parse the entire body, preserve repeated fields,
/// decode UTF-8/percent escapes, and apply their default limits. Multipart uses text fields only,
/// avoiding disk spill and differences in file ownership. Fresh features prevent cached reads;
/// the request carriers and seekable input streams are prepared outside measurement.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class HttpFormBenchmarks
{
    private BenchmarkHttpContext _url = null!;
    private BenchmarkHttpContext _multipart = null!;
    private AspNet.DefaultHttpContext _aspUrl = null!;
    private AspNet.DefaultHttpContext _aspMultipart = null!;

    [Params(4, 32)]
    public int Fields { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        StringBuilder url = new();
        StringBuilder multipart = new();
        for (int i = 0; i < Fields; i++)
        {
            if (i > 0) url.Append('&');
            url.Append($"field{i}=caf%C3%A9+value%26{i}&field{i}=second");
            multipart.Append($"--bench-boundary\r\nContent-Disposition: form-data; name=\"field{i}\"\r\n\r\ncafé value&{i}\r\n");
            multipart.Append($"--bench-boundary\r\nContent-Disposition: form-data; name=\"field{i}\"\r\n\r\nsecond\r\n");
        }
        multipart.Append("--bench-boundary--\r\n");
        (_url, _aspUrl) = CreateInputs(url.ToString(), "application/x-www-form-urlencoded; charset=utf-8");
        (_multipart, _aspMultipart) = CreateInputs(multipart.ToString(), "multipart/form-data; boundary=bench-boundary");
        Validate(await DecodeUrlEncoded(), await DecodeUrlEncoded_AspNetCore());
        Validate(await DecodeMultipart(), await DecodeMultipart_AspNetCore());
    }

    private void Validate(IHttpFormCollection cohesion, AspNet.IFormCollection asp)
    {
        if (cohesion.Count != Fields || asp.Count != Fields)
            throw new InvalidOperationException("Form field counts differ.");
        for (int i = 0; i < Fields; i++)
        {
            HttpQueryValue actual = cohesion[$"field{i}"];
            if (actual.Count != 2 || asp[$"field{i}"].Count != 2
                || actual[0] != $"café value&{i}" || actual[0] != asp[$"field{i}"][0]
                || actual[1] != "second" || actual[1] != asp[$"field{i}"][1])
                throw new InvalidOperationException("Form values or ordering differ.");
        }
    }

    private static (BenchmarkHttpContext, AspNet.DefaultHttpContext) CreateInputs(string body, string contentType)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(body);
        BenchmarkHttpContext cohesion = new();
        cohesion.Request.Headers[HttpHeaderKey.ContentType] = contentType;
        cohesion.Request.Body = new MemoryStream(bytes, writable: false);
        AspNet.DefaultHttpContext asp = new();
        asp.Request.ContentType = contentType;
        asp.Request.ContentLength = bytes.Length;
        asp.Request.Body = new MemoryStream(bytes, writable: false);
        return (cohesion, asp);
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _url.DisposeAsync();
        await _multipart.DisposeAsync();
        _aspUrl.Request.Body.Dispose();
        _aspMultipart.Request.Body.Dispose();
    }

    [Benchmark, BenchmarkCategory("UrlEncoded")]
    public Task<IHttpFormCollection> DecodeUrlEncoded()
    {
        _url.Request.Body.Position = 0;
        return new HttpFormFeature(_url.Request).ReadFormAsync();
    }

    [Benchmark(Baseline = true), BenchmarkCategory("UrlEncoded")]
    public Task<AspNet.IFormCollection> DecodeUrlEncoded_AspNetCore()
    {
        _aspUrl.Request.Body.Position = 0;
        return new AspNetFeatures.FormFeature(_aspUrl.Request).ReadFormAsync();
    }

    [Benchmark, BenchmarkCategory("Multipart")]
    public Task<IHttpFormCollection> DecodeMultipart()
    {
        _multipart.Request.Body.Position = 0;
        return new HttpFormFeature(_multipart.Request).ReadFormAsync();
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Multipart")]
    public Task<AspNet.IFormCollection> DecodeMultipart_AspNetCore()
    {
        _aspMultipart.Request.Body.Position = 0;
        return new AspNetFeatures.FormFeature(_aspMultipart.Request).ReadFormAsync();
    }
}
