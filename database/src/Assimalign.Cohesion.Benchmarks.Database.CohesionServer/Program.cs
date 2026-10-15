using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Connections.Tcp;
using Assimalign.Cohesion.Database;
using Assimalign.Cohesion.Database.Graph;
using Assimalign.Cohesion.Database.Security;
using Assimalign.Cohesion.Database.Sql;
using Assimalign.Cohesion.Database.Storage;

namespace Assimalign.Cohesion.Benchmarks.Database.CohesionServer;

internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        using var stop = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; stop.Cancel(); };
        using var terminate = OperatingSystem.IsWindows() ? null : PosixSignalRegistration.Create(PosixSignal.SIGTERM,
            context => { context.Cancel = true; stop.Cancel(); });
        try
        {
            if (args.Length == 1 && args[0] == "--help")
            {
                Console.WriteLine("cohesion-db-server --model sql|graph --data DIR [--port 5740] [--database benchmark] [--buffer-pool-mb 3072] [--durability synchronous] [--engine-name NAME]");
                return 0;
            }
            var options = new Dictionary<string, string>(StringComparer.Ordinal);
            for (int i = 0; i < args.Length; i += 2)
            {
                if (i + 1 >= args.Length || args[i] is not ("--model" or "--data" or "--port" or "--database" or "--buffer-pool-mb" or "--durability" or "--engine-name") ||
                    !options.TryAdd(args[i], args[i + 1])) throw new ArgumentException("Unknown, repeated or incomplete server option: " + args[i]);
            }
            string model = options.GetValueOrDefault("--model") ?? throw new ArgumentException("--model is required.");
            string data = Path.GetFullPath(options.GetValueOrDefault("--data") ?? throw new ArgumentException("--data is required."));
            int port = int.Parse(options.GetValueOrDefault("--port", "5740"), CultureInfo.InvariantCulture);
            if (port is < 1 or > 65535) throw new ArgumentOutOfRangeException(nameof(port));
            string database = options.GetValueOrDefault("--database", "benchmark");
            string name = options.GetValueOrDefault("--engine-name", "benchmark-" + model);
            long pool = checked(long.Parse(options.GetValueOrDefault("--buffer-pool-mb", "3072"), CultureInfo.InvariantCulture) * 1024 * 1024);
            var durability = Enum.Parse<StorageCommitDurability>(options.GetValueOrDefault("--durability", "synchronous"), true);
            if (!Enum.IsDefined(durability)) throw new ArgumentException("Unknown durability.");
            if (model == "sql")
            {
                var builder = SqlDatabaseEngine.CreateBuilder(name);
                builder.Options.RootPath = data;
                builder.Options.BufferPoolCapacity = pool;
                builder.Options.Durability = durability;
                builder.AddDatabase(database);
                await using var engine = await builder.BuildAsync(stop.Token);
                var serverOptions = new SqlDatabaseServerOptions { Authenticator = DatabaseAuthenticator.AllowAll };
                serverOptions.Listen(new Uri($"tcp://0.0.0.0:{port}"));
                await using var server = SqlDatabaseServer.Create(engine, serverOptions);
                await ServeAsync(server, model, port, stop.Token);
            }
            else if (model == "graph")
            {
                var builder = GraphDatabaseEngine.CreateBuilder(name);
                builder.Options.RootPath = data;
                builder.Options.BufferPoolCapacity = pool;
                builder.Options.Durability = durability;
                builder.AddDatabase(database);
                await using var engine = await builder.BuildAsync(stop.Token);
                var listener = new TcpConnectionListener(new() { EndPoint = new IPEndPoint(IPAddress.Any, port) });
                await using var server = GraphDatabaseServer.Create(engine, new()
                    { Listener = listener, Authenticator = DatabaseAuthenticator.AllowAll });
                await ServeAsync(server, model, port, stop.Token);
            }
            else throw new ArgumentException("--model must be sql or graph.");
            Console.WriteLine("STOPPED: server drained and engine disposed.");
            return 0;
        }
        catch (OperationCanceledException) when (stop.IsCancellationRequested) { return 0; }
        catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
    }

    private static async Task ServeAsync(DatabaseServer server, string model, int port, CancellationToken token)
    {
        await server.StartAsync(token);
        Console.WriteLine($"READY {model} port={port}");
        try { await Task.Delay(Timeout.InfiniteTimeSpan, token); }
        catch (OperationCanceledException) when (token.IsCancellationRequested) { }
        // Stop is deliberately not canceled by the termination signal: it drains accepted sessions.
        await server.StopAsync(CancellationToken.None);
    }
}
