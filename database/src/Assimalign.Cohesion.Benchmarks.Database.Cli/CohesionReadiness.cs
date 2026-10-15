using System;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Connections.Tcp;
using Assimalign.Cohesion.Database.Client;
using Assimalign.Cohesion.Database.Graph.Client;
using Assimalign.Cohesion.Database.Sql.Client;

namespace Assimalign.Cohesion.Benchmarks.Database.Cli;

// Readiness adapters only; deliberately not benchmark workload registrations.
internal static class CohesionReadiness
{
    public static async Task PingAsync(string target, string endpoint, string database, bool smoke, CancellationToken token)
    {
        var settings = DatabaseConnectionSettings.For(new Uri(endpoint), database, "dbbench");
        settings.MaxPoolSize = 1;
        if (target == "cohesion-sql-tcp")
        {
            await using var client = SqlClient.Create(new() { Settings = settings, ConnectionFactory = new TcpConnectionFactory() });
            await using var connection = await client.ConnectAsync(token);
            if (smoke)
            {
                await connection.ExecuteAsync("CREATE TABLE IF NOT EXISTS dbbench_smoke (id BIGINT PRIMARY KEY)", cancellationToken: token);
                await connection.ExecuteAsync("DELETE FROM dbbench_smoke", cancellationToken: token);
                await connection.ExecuteAsync("INSERT INTO dbbench_smoke (id) VALUES (1)", cancellationToken: token);
                var rows = await connection.QueryAsync("SELECT id FROM dbbench_smoke WHERE id = 1", cancellationToken: token);
                if (rows.Count != 1 || rows[0].GetInt64(0) != 1) throw new InvalidOperationException("SQL smoke returned the wrong row.");
                Console.WriteLine("SQL smoke: SELECT id FROM dbbench_smoke WHERE id = 1 => 1");
            }
            Console.WriteLine("PING cohesion-sql-tcp: authenticated ConnectAsync handshake succeeded.");
        }
        else if (target == "cohesion-graph-tcp")
        {
            await using var client = GraphClient.Create(new() { Settings = settings, ConnectionFactory = new TcpConnectionFactory() });
            await using var connection = await client.ConnectAsync(token);
            var rows = await connection.QueryAsync("MATCH (n:DbbenchReadiness) RETURN n.id", cancellationToken: token);
            Console.WriteLine($"PING cohesion-graph-tcp: authenticated handshake; MATCH (n:DbbenchReadiness) RETURN n.id => {rows.Count} rows.");
        }
        else throw new NotSupportedException($"No readiness adapter for {target}; use the catalog container probe.");
    }
}
