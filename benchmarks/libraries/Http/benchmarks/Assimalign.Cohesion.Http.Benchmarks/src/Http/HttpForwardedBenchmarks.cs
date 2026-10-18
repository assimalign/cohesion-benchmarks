using System;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Http.Benchmarks.Http;

/// <summary>
/// Parsing the proxy headers a forwarded-headers trust evaluator reads on every request behind a load balancer:
/// the RFC 7239 <c>Forwarded</c> element list, and the de-facto <c>X-Forwarded-For</c> list with each entry
/// parsed as a node (address, optional port), as the trust middleware does. No .NET counterpart parses
/// <c>Forwarded</c> (ASP.NET Core's middleware reads only the <c>X-Forwarded-*</c> headers, internally), so these
/// track Cohesion against itself.
/// </summary>
public class HttpForwardedBenchmarks
{
    // Three hops: IPv4 with a by= node, a quoted bracketed IPv6 node with a port, and a host= parameter.
    private const string forwarded =
        "for=192.0.2.60;proto=https;by=203.0.113.43, for=\"[2001:db8:cafe::17]:4711\";proto=https, for=198.51.100.17;host=example.com;proto=https";

    private const string xForwardedFor = "203.0.113.195, 2001:db8:85a3:8d3:1319:8a2e:370:7348, 198.51.100.178";

    [GlobalSetup]
    public void Setup()
    {
        if (ParseForwarded().Count != 3 || ParseXForwardedFor().IsEmpty)
        {
            throw new InvalidOperationException("The forwarded-header inputs did not parse as expected.");
        }
    }

    [Benchmark]
    public HttpForwardedElementCollection ParseForwarded() => HttpForwardedElementCollection.Parse(forwarded);

    [Benchmark]
    public HttpForwardedNode ParseXForwardedFor()
    {
        HttpForwardedValues values = HttpForwardedValues.Parse(xForwardedFor);
        HttpForwardedNode nearest = default;
        foreach (string entry in values.AsSpan())
        {
            if (!HttpForwardedNode.TryParse(entry.AsSpan(), out nearest))
            {
                throw new InvalidOperationException($"'{entry}' is not a forwarded node.");
            }
        }

        return nearest;
    }
}
