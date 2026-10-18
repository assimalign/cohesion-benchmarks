using System;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Cohesion.Web.Benchmarks.Runner.Http;

/// <summary>
/// One request, encoded once and replayed byte for byte. Only a Host header is added, as wrk and
/// bombardier do; anything else a scenario needs it declares.
/// </summary>
internal sealed record HttpRequestTemplate(string Method, string Path)
{
    public IReadOnlyList<(string Name, string Value)> Headers { get; init; } = [];

    public byte[]? Body { get; init; }

    public bool IsHead => Method == "HEAD";

    public HttpRequestTemplate WithHeader(string name, string value) =>
        this with { Headers = [.. Headers, (name, value)] };

    public byte[] Encode(string authority)
    {
        var head = new StringBuilder(256);
        head.Append(Method).Append(' ').Append(Path).Append(" HTTP/1.1\r\n");
        head.Append("Host: ").Append(authority).Append("\r\n");
        foreach ((string name, string value) in Headers)
        {
            head.Append(name).Append(": ").Append(value).Append("\r\n");
        }

        if (Body is not null || Method is "POST" or "PUT" or "PATCH")
        {
            head.Append("Content-Length: ").Append(Body?.Length ?? 0).Append("\r\n");
        }

        head.Append("\r\n");

        byte[] headBytes = Encoding.Latin1.GetBytes(head.ToString());
        if (Body is null || Body.Length == 0)
        {
            return headBytes;
        }

        byte[] request = new byte[headBytes.Length + Body.Length];
        headBytes.CopyTo(request, 0);
        Body.CopyTo(request, headBytes.Length);
        return request;
    }
}
