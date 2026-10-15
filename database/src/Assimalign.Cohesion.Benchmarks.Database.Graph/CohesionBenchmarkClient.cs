using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Assimalign.Cohesion.Database.Execution;
using Assimalign.Cohesion.Database.Graph;
using Assimalign.Cohesion.Database.Graph.Client;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

internal sealed class CohesionBenchmarkClient : IBenchmarkClient
{
    private readonly GraphDatabaseSession? session;
    private readonly GraphClient? tcp;
    private GraphConnection? connection;
    private readonly List<string> cleanup = new();
    internal CohesionBenchmarkClient(GraphDatabaseSession session) => this.session = session;
    internal CohesionBenchmarkClient(GraphClient tcp, GraphConnection connection) { this.tcp = tcp; this.connection = connection; }

    public async ValueTask ExecuteAsync(OperationDescriptor operation, object parameters, IVerificationSink? sink, CancellationToken cancellationToken)
    {
        var p = (GraphParameters)parameters;
        // A canceled path exchange discards its connection. Reconnect for the next logical operation,
        // never retry the failed request or substitute a smaller graph pattern.
        if (connection is not null && !connection.IsOpen)
        {
            await connection.DisposeAsync();
            connection = await tcp!.ConnectAsync(cancellationToken);
        }
        if (p.CleanupGql is not null) cleanup.Add(p.CleanupGql);
        var rows = new List<object?[]>();
        GraphDatabaseTransaction? transaction = null;
        try
        {
            if (p.Gql.Length > 1)
            {
                if (session is null) throw new NotSupportedException("Multiple graph statements require an explicit transaction, unavailable over TCP.");
                transaction = await session.BeginTransactionAsync(cancellationToken);
            }
            for (int part = 0; part < p.Gql.Length; part++)
            {
                string text = p.Gql[part];
                if (p.PathRows)
                {
                    if (session is not null)
                    {
                        var result = (GraphPathsQueryResult)await session.ExecuteAsync(GraphPathsQueryRequest.FromGql(text), cancellationToken);
                        foreach (var path in result.Paths) AddPath(path);
                    }
                    else await foreach (var path in connection!.QueryPathsAsync(text, cancellationToken: cancellationToken)) AddPath(path);
                }
                else if (session is not null)
                {
                    await using var result = (QueryResultSet)await session.ExecuteAsync(text, cancellationToken: cancellationToken);
                    await foreach (var row in result.GetRowsAsync(cancellationToken))
                    {
                        var values = new object?[row.FieldCount + (operation.Id == "graph.friends-within-3-by-name" ? 1 : 0)];
                        for (int i = 0; i < row.FieldCount; i++) values[i] = row.GetValue(i);
                        if (values.Length != row.FieldCount) values[^1] = (long)part + 1;
                        rows.Add(values);
                    }
                }
                else
                {
                    var result = await connection!.QueryAsync(text, cancellationToken: cancellationToken);
                    foreach (var row in result)
                    {
                        var values = new object?[row.Count];
                        for (int i = 0; i < values.Length; i++) values[i] = row[i];
                        rows.Add(values);
                    }
                }
            }
            if (transaction is not null) await transaction.CommitAsync(cancellationToken);
            if (p.CleanupGql is not null && rows.Count != 1) throw new InvalidOperationException("Graph write did not return exactly one affected logical row.");
            GraphCompletion.Emit(operation.Id, rows, sink);
            cancellationToken.ThrowIfCancellationRequested();
        }
        finally { if (transaction is not null) await transaction.DisposeAsync(); }

        void AddPath(GraphPath path)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var message = path.Nodes[^1];
            var author = path.Nodes[^2];
            string kind = "Comment";
            foreach (string label in message.Labels) if (label == "Post") { kind = "Post"; break; }
            rows.Add([kind, message.Properties["id"], message.Properties["creationDate"], author.Properties["id"], message.Properties["content"]]);
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (connection is not null && !connection.IsOpen && cleanup.Count != 0)
            {
                await connection.DisposeAsync();
                connection = await tcp!.ConnectAsync(CancellationToken.None);
            }
            foreach (string text in cleanup)
            {
                if (session is not null) await session.ExecuteAsync(text, cancellationToken: CancellationToken.None);
                else await connection!.ExecuteAsync(text, cancellationToken: CancellationToken.None);
            }
        }
        finally
        {
            if (session is not null) await session.DisposeAsync();
            if (connection is not null) await connection.DisposeAsync();
        }
    }
}
