using System;
using System.Buffers;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Assimalign.Cohesion.Database.Execution;
using Assimalign.Cohesion.Database.Sql;
using WireConnection = Assimalign.Cohesion.Database.Sql.Client.SqlConnection;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

internal sealed class SqlSession : IAsyncDisposable
{
    internal DbConnection? Ado { get; }
    private readonly SqlDatabaseSession? embedded;
    private readonly WireConnection? wire;
    internal SqlSession(DbConnection connection) => Ado = connection;
    internal SqlSession(SqlDatabaseSession session) => embedded = session;
    internal SqlSession(WireConnection connection) => wire = connection;

    internal async ValueTask<long> ExecuteAsync(SqlStatement statement, CancellationToken token)
    {
        if (Ado != null) return await Command(statement).ExecuteNonQueryAsync(token).ConfigureAwait(false);
        if (wire != null) return await wire.ExecuteAsync(statement.Cohesion, token).ConfigureAwait(false);
        QueryResult result = await embedded!.ExecuteAsync(statement.Text, statement.Parameters, token).ConfigureAwait(false);
        Check(result);
        if (result is QueryResultSet rows) await rows.DisposeAsync().ConfigureAwait(false);
        return result.AffectedCount;
    }

    internal async ValueTask ReadAsync(SqlStatement statement, IVerificationSink? sink, CancellationToken token)
    {
        object?[]? buffer = null;
        try
        {
            if (Ado != null)
            {
                await using var reader = await Command(statement).ExecuteReaderAsync(token).ConfigureAwait(false);
                if (sink != null) buffer = ArrayPool<object?>.Shared.Rent(reader.FieldCount);
                while (await reader.ReadAsync(token).ConfigureAwait(false))
                {
                    if (sink == null) continue;
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        object? value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        // DATE is a logical DateOnly; TIMESTAMP/DATETIME2 remain DateTime.
                        if (value is DateTime date && reader.GetDataTypeName(i).Equals("date", StringComparison.OrdinalIgnoreCase)) value = DateOnly.FromDateTime(date);
                        buffer![i] = value;
                    }
                    sink.AddRow(buffer.AsSpan(0, reader.FieldCount));
                }
            }
            else if (wire != null)
            {
                var result = await wire.QueryAsync(statement.Cohesion, token).ConfigureAwait(false);
                for (int r = 0; r < result.Count; r++)
                {
                    token.ThrowIfCancellationRequested();
                    var row = result[r];
                    if (sink == null) continue;
                    buffer ??= ArrayPool<object?>.Shared.Rent(row.FieldCount);
                    for (int i = 0; i < row.FieldCount; i++) buffer[i] = row.GetValue(i);
                    sink.AddRow(buffer.AsSpan(0, row.FieldCount));
                }
            }
            else
            {
                QueryResult result = await embedded!.ExecuteAsync(statement.Text, statement.Parameters, token).ConfigureAwait(false);
                Check(result);
                await using var rows = (QueryResultSet)result;
                await foreach (var row in rows.GetRowsAsync(token).ConfigureAwait(false))
                {
                    if (sink == null) continue;
                    buffer ??= ArrayPool<object?>.Shared.Rent(row.FieldCount);
                    for (int i = 0; i < row.FieldCount; i++) buffer[i] = row.GetValue(i);
                    sink.AddRow(buffer.AsSpan(0, row.FieldCount));
                }
            }
        }
        finally { if (buffer != null) ArrayPool<object?>.Shared.Return(buffer, true); }
    }

    private DbCommand Command(SqlStatement statement)
    {
        DbCommand command = Ado is Npgsql.NpgsqlConnection ? statement.PostgreSql : statement.SqlServer;
        command.Connection = Ado;
        return command;
    }

    private static void Check(QueryResult result)
    {
        if (result.Status == QueryResultStatus.Success) return;
        var text = new StringBuilder();
        if (result.Diagnostics != null) foreach (var diagnostic in result.Diagnostics) text.Append(diagnostic.Code).Append(": ").AppendLine(diagnostic.Message);
        throw new InvalidOperationException(text.ToString());
    }

    internal async ValueTask<long> ExecuteAsync(string text, CancellationToken token)
    {
        using var statement = new SqlStatement(text);
        return await ExecuteAsync(statement, token).ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (Ado != null) await Ado.DisposeAsync().ConfigureAwait(false);
        if (embedded != null) await embedded.DisposeAsync().ConfigureAwait(false);
        if (wire != null) await wire.DisposeAsync().ConfigureAwait(false);
    }
}
