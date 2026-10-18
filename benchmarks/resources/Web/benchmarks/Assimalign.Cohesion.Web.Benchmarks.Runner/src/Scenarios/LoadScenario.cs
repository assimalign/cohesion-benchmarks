using System;
using System.Collections.Generic;

using Assimalign.Cohesion.Web.Benchmarks.Runner.Http;

namespace Assimalign.Cohesion.Web.Benchmarks.Runner.Scenarios;

internal enum LoadKind
{
    Http,
    WebSocket,
}

/// <summary>
/// What a connection learned from its setup request: the cookies the server set and, when the
/// scenario asks for it, the response body as a token.
/// </summary>
internal sealed record SetupResult(string? CookieHeader, string? Token);

/// <summary>
/// A measured workload: which server pipeline it runs against, the request(s) every connection replays,
/// an optional per-connection setup request, and what a correct response looks like. The expectation is
/// checked once per framework before load starts, so a server that answers the wrong thing quickly is
/// reported as failed rather than fast.
/// </summary>
internal sealed record LoadScenario(string Id, string Server, string Category, string Description)
{
    public LoadKind Kind { get; init; } = LoadKind.Http;

    /// <summary>Builds the requests a connection replays, round-robin, given its setup result.</summary>
    public Func<SetupResult, IReadOnlyList<HttpRequestTemplate>> Requests { get; init; } = _ => [];

    /// <summary>A request each connection sends once before load, for cookies or a token.</summary>
    public HttpRequestTemplate? Setup { get; init; }

    public bool SetupBodyIsToken { get; init; }

    public int[] ExpectedStatus { get; init; } = [200];

    public string? ExpectBodyContains { get; init; }

    public string[] ExpectHeaders { get; init; } = [];

    /// <summary>Requests in flight per connection. 1 is request/response; 16 is TechEmpower's plaintext depth.</summary>
    public int PipelineDepth { get; init; } = 1;

    /// <summary>The WebSocket path and message size, for <see cref="LoadKind.WebSocket"/> scenarios.</summary>
    public string WebSocketPath { get; init; } = "/ws";

    public int WebSocketMessageSize { get; init; } = 32;

    /// <summary>Anything a reader must know to interpret the comparison.</summary>
    public string? Note { get; init; }
}
