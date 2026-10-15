using System.Collections.Generic;

namespace Assimalign.Cohesion.Benchmarks.Database.Harness;

/// <summary>Connection, local storage and load settings supplied by the CLI. Credentials come from the catalog.</summary>
public sealed record TargetOptions
{
    public string? Endpoint { get; init; }
    public string Database { get; init; } = "benchmark";
    public string StorageDirectory { get; init; } = "/engine-data";
    public string? Container { get; init; }
    public int BatchSize { get; init; } = 1000;
    public int TcpIngestSampleRowsPerTable { get; init; } = 1000;
    public long BufferPoolBytes { get; init; } = 3072L * 1024 * 1024;
    public IReadOnlyDictionary<string, string> Settings { get; init; } = new Dictionary<string, string>();
}
