namespace Assimalign.Cohesion.Benchmarks.Database.CohesionServer;

/// <summary>
/// Entry point of the Cohesion benchmark server host (placeholder until the host is implemented).
/// </summary>
internal static class Program
{
    private static int Main(string[] args)
    {
        // Touch one type from each engine so the cross-repository references are proven at build time.
        System.Type[] engines =
        [
            typeof(Assimalign.Cohesion.Database.Sql.SqlDatabaseEngine),
            typeof(Assimalign.Cohesion.Database.Graph.GraphDatabaseEngine),
            typeof(Assimalign.Cohesion.Connections.Tcp.TcpConnectionListener),
        ];
        System.Console.Error.WriteLine($"cohesion-db-server: not implemented yet ({engines.Length} engine types linked).");
        return 1;
    }
}
