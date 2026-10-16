using System;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Harness;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

internal sealed class SqlBenchmarkClient(SqlSession session, SqlDialect dialect) : IBenchmarkClient
{
    private readonly SqlStatement begin = new("BEGIN", tsql: "BEGIN TRANSACTION");
    private readonly SqlStatement commit = new("COMMIT");
    private readonly SqlStatement rollback = new("ROLLBACK");

    public async ValueTask ExecuteAsync(OperationDescriptor operation, object parameters, IVerificationSink? sink, CancellationToken cancellationToken)
    {
        var request = (SqlWorkloadParameters)parameters;
        bool transaction = false;
        try
        {
            if (operation.Access == OperationAccess.Read)
            {
                if (dialect == SqlDialect.Cohesion && request.Monthly != null)
                {
                    await session.ReadAsync(request.Statements[0], request.Monthly, cancellationToken);
                    request.Monthly.Complete(sink);
                }
                else await session.ReadAsync(request.Statements[0], sink, cancellationToken);
                return;
            }
            if (request.Transaction) { await session.ExecuteAsync(begin, cancellationToken); transaction = true; }
            foreach (SqlStatement statement in request.Statements) await session.ExecuteAsync(statement, cancellationToken);
            if (transaction) { await session.ExecuteAsync(commit, cancellationToken); transaction = false; }
            if (sink != null) foreach (SqlStatement statement in request.Verification) await session.ReadAsync(statement, sink, cancellationToken);
        }
        catch
        {
            if (transaction) await session.ExecuteAsync(rollback, CancellationToken.None);
            throw;
        }
        finally { request.Dispose(); }
    }

    public async ValueTask DisposeAsync()
    {
        begin.Dispose(); commit.Dispose(); rollback.Dispose();
        await session.DisposeAsync();
    }
}
