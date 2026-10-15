using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Neo4j.Driver;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal sealed class Neo4jBenchmarkClient : IBenchmarkClient
{
    private readonly IDriver driver;
    private readonly QueryConfig config;
    private readonly List<GraphParameters> cleanup = new();
    internal Neo4jBenchmarkClient(IDriver driver, string database) { this.driver = driver; config = new QueryConfig(database: database); }

    public async ValueTask ExecuteAsync(OperationDescriptor operation, object parameters, IVerificationSink? sink, CancellationToken cancellationToken)
    {
        var p = (GraphParameters)parameters;
        if (p.CleanupCypher is not null) cleanup.Add(p);
        var result = await driver.ExecutableQuery(p.Cypher).WithParameters(p.Values).WithConfig(config).ExecuteAsync(cancellationToken);
        if (p.CleanupCypher is not null && result.Result.Count != 1) throw new InvalidOperationException("Graph write did not return exactly one affected logical row.");
        if (sink is not null)
        {
            var row = new object?[result.Keys.Length];
            foreach (var record in result.Result)
            {
                for (int i = 0; i < row.Length; i++) row[i] = record[result.Keys[i]];
                sink.AddRow(row);
            }
        }
    }
    public async ValueTask DisposeAsync()
    {
        foreach (var p in cleanup)
            await driver.ExecutableQuery(p.CleanupCypher!).WithParameters(p.Values).WithConfig(config).ExecuteAsync(CancellationToken.None);
    }
}
