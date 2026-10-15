using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Assimalign.Cohesion.Benchmarks.Database.Harness;
using Assimalign.Cohesion.Connections.Tcp;
using Assimalign.Cohesion.Database.Client;
using Assimalign.Cohesion.Database.Sql;
using Assimalign.Cohesion.Database.Storage;
using Microsoft.Data.SqlClient;
using Npgsql;
using WireClient = Assimalign.Cohesion.Database.Sql.Client.SqlClient;
using TsqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace Assimalign.Cohesion.Benchmarks.Database.Sql;

/// <summary>Commerce storage lifecycle, durable ingestion and session creation for all four SQL targets.</summary>
public sealed class SqlTarget : IBenchmarkTarget
{
    private readonly TargetOptions options;
    private SqlDatabaseEngine? engine;
    private WireClient? wire;
    private WireClient? scratchWire;
    private int loadConcurrency = 1;
    public TargetDescriptor Descriptor { get; }
    public IReadOnlyDictionary<string, OperationSupport> Support { get; }
    internal SqlDialect Dialect { get; }
    private bool IsTcp => Descriptor.Id == "cohesion-sql-tcp";
    private string Scratch => options.Database + "_tcp_ingest";

    public SqlTarget(TargetDescriptor descriptor, TargetOptions options, IReadOnlyDictionary<string, OperationSupport> support)
    {
        Descriptor = descriptor; this.options = options; Support = support;
        SqlSchema.Identifier(options.Database);
        if (options.BatchSize < 1) throw new ArgumentOutOfRangeException(nameof(options));
        Dialect = descriptor.Id switch { "postgresql" => SqlDialect.PostgreSql, "sqlserver" => SqlDialect.SqlServer, _ => SqlDialect.Cohesion };
    }

    private async ValueTask InitializeAsync(CancellationToken token)
    {
        if (Dialect != SqlDialect.Cohesion) return;
        if (IsTcp)
        {
            wire ??= Client(options.Database);
            scratchWire ??= Client(Scratch);
        }
        else if (engine == null)
        {
            var builder = SqlDatabaseEngine.CreateBuilder("benchmark-sql");
            builder.Options.RootPath = Path.GetFullPath(options.StorageDirectory);
            builder.Options.BufferPoolCapacity = options.BufferPoolBytes;
            builder.Options.Durability = StorageCommitDurability.Synchronous;
            builder.AddDatabase(options.Database);
            builder.AddDatabase(Scratch);
            engine = await builder.BuildAsync(token).ConfigureAwait(false);
        }
    }

    private WireClient Client(string database)
    {
        var settings = DatabaseConnectionSettings.For(new Uri(options.Endpoint!), database, options.Settings.GetValueOrDefault("principal", "dbbench"));
        settings.MaxPoolSize = 64;
        return WireClient.Create(new() { Settings = settings, ConnectionFactory = new TcpConnectionFactory() });
    }

    internal async ValueTask<SqlSession> OpenSessionAsync(CancellationToken token, bool scratch = false, bool administrative = false)
    {
        await InitializeAsync(token).ConfigureAwait(false);
        if (IsTcp) return new(await (scratch ? scratchWire! : wire!).ConnectAsync(token).ConfigureAwait(false));
        if (engine != null)
        {
            if (!engine.TryGetDatabase(scratch ? Scratch : options.Database, out SqlDatabase? database)) throw new InvalidOperationException("SQL database was not opened.");
            return new(await database.CreateSessionAsync(cancellationToken: token).ConfigureAwait(false));
        }
        var endpoint = new Uri(options.Endpoint!);
        string username = options.Settings["username"], password = options.Settings["password"];
        if (Dialect == SqlDialect.PostgreSql)
        {
            var connection = new NpgsqlConnection(new NpgsqlConnectionStringBuilder
            { Host = endpoint.Host, Port = endpoint.Port, Username = username, Password = password,
                Database = administrative ? "postgres" : options.Database, MaxPoolSize = 128, CommandTimeout = 600 }.ConnectionString);
            try { await connection.OpenAsync(token).ConfigureAwait(false); return new(connection); }
            catch { await connection.DisposeAsync().ConfigureAwait(false); throw; }
        }
        else
        {
            var connection = new TsqlConnection(new SqlConnectionStringBuilder
            { DataSource = endpoint.Host + "," + endpoint.Port.ToString(CultureInfo.InvariantCulture), UserID = username, Password = password,
                InitialCatalog = administrative ? "master" : options.Database, Encrypt = false, TrustServerCertificate = true,
                MaxPoolSize = 128, ConnectTimeout = 30 }.ConnectionString);
            try { await connection.OpenAsync(token).ConfigureAwait(false); return new(connection); }
            catch { await connection.DisposeAsync().ConfigureAwait(false); throw; }
        }
    }

    public async ValueTask PrepareAsync(DatasetManifest manifest, CancellationToken cancellationToken)
    {
        if (Dialect != SqlDialect.Cohesion)
        {
            await using var admin = await OpenSessionAsync(cancellationToken, administrative: true);
            string name = options.Database;
            if (Dialect == SqlDialect.PostgreSql)
                await admin.ExecuteAsync($"DROP DATABASE IF EXISTS {name} WITH (FORCE)", cancellationToken);
            else
                await admin.ExecuteAsync($"IF DB_ID('{name}') IS NOT NULL BEGIN ALTER DATABASE {name} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE {name}; END", cancellationToken);
            await admin.ExecuteAsync(SqlSchema.CreateDatabase(name, Dialect), cancellationToken);
            if (Dialect == SqlDialect.SqlServer)
                await admin.ExecuteAsync($"ALTER DATABASE {name} SET RECOVERY SIMPLE; ALTER DATABASE {name} SET DELAYED_DURABILITY = DISABLED", cancellationToken);
        }
        await using var session = await OpenSessionAsync(cancellationToken, scratch: IsTcp);
        foreach (SchemaTable table in CommerceSchema.Tables)
        {
            if (IsTcp && table.Name is not ("orders" or "order_items")) continue;
            await session.ExecuteAsync("DROP TABLE IF EXISTS " + table.Name, cancellationToken);
            await session.ExecuteAsync(SqlSchema.CreateTable(table, Dialect), cancellationToken);
        }
    }

    public async ValueTask<IReadOnlyList<LoadMetric>> LoadAsync(DatasetManifest manifest, string directory, IProgress<LoadProgress>? progress, CancellationToken cancellationToken)
    {
        var metrics = new List<LoadMetric>();
        if (Dialect == SqlDialect.Cohesion && !IsTcp)
            await SelectConcurrencyAsync(manifest, directory, metrics, cancellationToken);
        foreach (DatasetTableManifest table in manifest.Tables)
        {
            if (IsTcp && table.Name is not ("orders" or "order_items")) continue;
            var clock = Stopwatch.StartNew();
            long rows;
            string path;
            if (Dialect == SqlDialect.Cohesion)
            {
                rows = await LoadCohesionAsync(manifest, directory, table.Name, table.Name, IsTcp ? 100000 : long.MaxValue, IsTcp ? 1 : loadConcurrency, IsTcp, cancellationToken);
                path = $"multi-row INSERT; {Math.Min(options.BatchSize, 1000)} rows/statement; 4 statements/transaction; {(IsTcp ? 1 : loadConcurrency)} sessions";
            }
            else
            {
                await Parallel.ForEachAsync(DatasetFiles.EnumerateParts(directory, manifest, table.Name), new ParallelOptions
                { MaxDegreeOfParallelism = 4, CancellationToken = cancellationToken }, async (file, token) =>
                {
                    await using var session = await OpenSessionAsync(token);
                    if (session.Ado is NpgsqlConnection pg)
                    {
                        await using var copy = await pg.BeginTextImportAsync($"COPY {table.Name} FROM STDIN (FORMAT csv, HEADER true)", token);
                        await using var input = File.OpenRead(file);
                        try { await input.CopyToAsync(copy.BaseStream, 65536, token); }
                        catch { await copy.CancelAsync(); throw; }
                    }
                    else
                    {
                        using var input = new CsvDataReader(File.OpenRead(file), CommerceSchema.Table(table.Name));
                        using var copy = new SqlBulkCopy((TsqlConnection)session.Ado!, SqlBulkCopyOptions.TableLock, null)
                        { DestinationTableName = table.Name, BatchSize = Math.Max(10000, options.BatchSize), EnableStreaming = true, BulkCopyTimeout = 600 };
                        await copy.WriteToServerAsync(input, token);
                    }
                });
                rows = table.Rows;
                path = Dialect == SqlDialect.PostgreSql ? "COPY CSV HEADER; unmodified part bytes; up to 4 concurrent parts" : "SqlBulkCopy IDataReader; TableLock; batches >=10000; up to 4 concurrent parts";
            }
            metrics.Add(new(table.Name, IsTcp ? "tcp-ingest" : "ingest", path, rows, IsTcp ? 0 : table.Bytes, clock.Elapsed.TotalSeconds, IsTcp));
        }
        return metrics;
    }

    private async Task SelectConcurrencyAsync(DatasetManifest manifest, string directory, List<LoadMetric> metrics, CancellationToken token)
    {
        double best = double.MaxValue;
        foreach (int clients in new[] { 1, 4 })
        {
            await using (var session = await OpenSessionAsync(token, scratch: true))
            {
                await session.ExecuteAsync("DROP TABLE IF EXISTS orders", token);
                await session.ExecuteAsync(SqlSchema.CreateTable(CommerceSchema.Table("orders"), SqlDialect.Cohesion), token);
            }
            var clock = Stopwatch.StartNew();
            long rows = await LoadCohesionAsync(manifest, directory, "orders", "orders", 10000, clients, true, token);
            double seconds = clock.Elapsed.TotalSeconds;
            metrics.Add(new("orders", "concurrency-trial", $"{clients} sessions; scratch database; selection only", rows, 0, seconds, true));
            if (seconds < best) { best = seconds; loadConcurrency = clients; }
        }
        await using var cleanup = await OpenSessionAsync(token, scratch: true);
        await cleanup.ExecuteAsync("DROP TABLE orders", token);
    }

    private async Task<long> LoadCohesionAsync(DatasetManifest manifest, string directory, string table, string destination, long maximum, int workers, bool scratch, CancellationToken token)
    {
        var channel = Channel.CreateBounded<string>(workers * 8);
        using var cancel = CancellationTokenSource.CreateLinkedTokenSource(token);
        var tasks = new Task[workers + 1];
        long rows = 0;
        tasks[0] = Task.Run(async () =>
        {
            try
            {
                foreach (string path in DatasetFiles.EnumerateParts(directory, manifest, table))
                {
                    foreach (var statement in CohesionInsert.Statements(path, CommerceSchema.Table(table), destination, Math.Min(1000, options.BatchSize), maximum - rows))
                    {
                        await channel.Writer.WriteAsync(statement.Text, cancel.Token); rows += statement.Rows;
                    }
                    if (rows >= maximum) break;
                }
                channel.Writer.TryComplete();
            }
            catch (Exception ex) { channel.Writer.TryComplete(ex); await cancel.CancelAsync(); throw; }
        }, CancellationToken.None);
        for (int i = 1; i < tasks.Length; i++) tasks[i] = ConsumeAsync();
        await Task.WhenAll(tasks);
        return rows;

        async Task ConsumeAsync()
        {
            await using var session = await OpenSessionAsync(cancel.Token, scratch);
            bool transaction = false;
            try
            {
                int statements = 0;
                await foreach (string sql in channel.Reader.ReadAllAsync(cancel.Token))
                {
                    if (!transaction) { await session.ExecuteAsync("BEGIN", cancel.Token); transaction = true; }
                    await session.ExecuteAsync(sql, cancel.Token);
                    if (++statements == 4) { await session.ExecuteAsync("COMMIT", cancel.Token); transaction = false; statements = 0; }
                }
                if (transaction) { await session.ExecuteAsync("COMMIT", cancel.Token); transaction = false; }
            }
            catch
            {
                await cancel.CancelAsync();
                if (transaction) await session.ExecuteAsync("ROLLBACK", CancellationToken.None);
                throw;
            }
        }
    }

    public async ValueTask<IReadOnlyList<LoadMetric>> FinalizeLoadAsync(CancellationToken cancellationToken)
    {
        if (IsTcp) return Array.Empty<LoadMetric>();
        var metrics = new List<LoadMetric>();
        await using var session = await OpenSessionAsync(cancellationToken);
        foreach (SchemaTable table in CommerceSchema.Tables)
        {
            var clock = Stopwatch.StartNew();
            foreach (string sql in SqlSchema.CreateIndexes(table)) await session.ExecuteAsync(sql, cancellationToken);
            if (Dialect != SqlDialect.Cohesion) await session.ExecuteAsync((Dialect == SqlDialect.PostgreSql ? "ANALYZE " : "UPDATE STATISTICS ") + table.Name, cancellationToken);
            metrics.Add(new(table.Name, "indexes-and-statistics", Dialect == SqlDialect.Cohesion ? "CREATE INDEX; no statistics API (SQL DESIGN:1583-1591)" : "CREATE INDEX + statistics", 0, 0, clock.Elapsed.TotalSeconds));
        }
        return metrics;
    }

    public async ValueTask<EngineDescription> DescribeAsync(CancellationToken cancellationToken)
    {
        await using var session = await OpenSessionAsync(cancellationToken);
        var settings = new Dictionary<string, string>();
        string version;
        if (session.Ado != null)
        {
            version = session.Ado.ServerVersion;
            if (Dialect == SqlDialect.PostgreSql)
            {
                foreach (string key in new[] { "lc_collate", "lc_ctype", "shared_buffers", "work_mem", "maintenance_work_mem", "synchronous_commit", "fsync" })
                { await using var command = session.Ado.CreateCommand(); command.CommandText = "SHOW " + key; settings[key] = Convert.ToString(await command.ExecuteScalarAsync(cancellationToken), CultureInfo.InvariantCulture)!; }
            }
            else
            {
                await using var command = session.Ado.CreateCommand();
                command.CommandText = "SELECT collation_name, recovery_model_desc, delayed_durability_desc FROM sys.databases WHERE name = DB_NAME()";
                await using var reader = await command.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken)) for (int i = 0; i < reader.FieldCount; i++) settings[reader.GetName(i)] = reader.GetString(i);
            }
        }
        else
        {
            version = typeof(SqlDatabaseEngine).Assembly.GetName().Version!.ToString();
            settings["Durability"] = "Synchronous"; settings["BufferPoolCapacity"] = options.BufferPoolBytes.ToString(CultureInfo.InvariantCulture);
            settings["collation"] = "binary (explicit columns)";
        }
        return new(version, settings);
    }

    public async ValueTask<long> MeasureStorageAsync(CancellationToken cancellationToken)
    {
        if (Dialect == SqlDialect.Cohesion)
        {
            if (IsTcp) return 0; // Host-side volume inspection records the served baseline's storage.
            long bytes = 0;
            foreach (string file in Directory.EnumerateFiles(Path.Combine(options.StorageDirectory, options.Database.ToLowerInvariant()), "*", SearchOption.AllDirectories)) bytes += new FileInfo(file).Length;
            return bytes;
        }
        await using var session = await OpenSessionAsync(cancellationToken);
        await using var command = session.Ado!.CreateCommand();
        command.CommandText = Dialect == SqlDialect.PostgreSql ? "SELECT pg_database_size(current_database())" : "SELECT SUM(CAST(size AS BIGINT)) * 8192 FROM sys.database_files";
        return Convert.ToInt64(await command.ExecuteScalarAsync(cancellationToken), CultureInfo.InvariantCulture);
    }

    public async ValueTask<IBenchmarkClient> OpenClientAsync(int clientIndex, CancellationToken cancellationToken)
        => new SqlBenchmarkClient(await OpenSessionAsync(cancellationToken), Dialect);
    public async ValueTask PingAsync(CancellationToken cancellationToken) { await using var session = await OpenSessionAsync(cancellationToken); }
    public async ValueTask DisposeAsync()
    {
        if (wire != null) await wire.DisposeAsync();
        if (scratchWire != null) await scratchWire.DisposeAsync();
        if (engine != null) await engine.DisposeAsync();
    }
}
