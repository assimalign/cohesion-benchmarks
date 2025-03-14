using System;
using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Configs;
using System.Text.Json;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using Azure.Data.Tables;
using Azure;
using BenchmarkDotNet.Columns;

namespace Assimalign.Cohesion.Benchmarks;

[SimpleJob()]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class CohesionJobAttribute : JobConfigBaseAttribute
{
    public CohesionJobAttribute(RuntimeMoniker runtimeMoniker)
    {
        Configure(config =>
        {
            // Exporters
            config.AddExporter(new RemoteExporter());

            // Loggers
            config.AddLogger(ConsoleLogger.Default);

            // Jobs
            config.AddJob(new Job(
                "SimpleJob", 
                RunMode.Dry, 
                new EnvironmentMode(FromMoniker(runtimeMoniker))));
        });
    }

    private Runtime FromMoniker(RuntimeMoniker runtime) => runtime switch
    {
        RuntimeMoniker.Net90 => CoreRuntime.Core90,
        RuntimeMoniker.Net80 => CoreRuntime.Core80,
        RuntimeMoniker.Net70 => CoreRuntime.Core70,
        RuntimeMoniker.Net60 => CoreRuntime.Core60
    };

    private void Configure(Action<ManualConfig> configure)
    {
        if (Config is ManualConfig configuration)
        {
            configure.Invoke(configuration);
        }
    }


    #region Exporters

    partial class RemoteExporter : ExporterBase
    {
        public RemoteExporter()
        {

        }


        public override void ExportToLog(Summary summary, ILogger logger)
        {
            try
            {
                var columns = summary.GetColumns();

                foreach (var benchmark in summary.BenchmarksCases)
                {
                    foreach (var column in columns)
                    {
                        var value = column.GetValue(summary, benchmark);
                        Console.WriteLine($"{column.ColumnName}: {value}");

                    }
                }
                //foreach (var column in summary.GetColumns())
                //{
                //    column.
                //}
                //Console.WriteLine(JsonSerializer.Serialize(summary, new JsonSerializerOptions()
                //{
                //    WriteIndented = true
                //}));
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occurred");
            }
        }
    }

    partial class RemoteExporterTable : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string? Title { get; }
        public string? ResultsDirectoryPath { get; }
        public string? LogFilePath { get; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }


    #endregion
}
