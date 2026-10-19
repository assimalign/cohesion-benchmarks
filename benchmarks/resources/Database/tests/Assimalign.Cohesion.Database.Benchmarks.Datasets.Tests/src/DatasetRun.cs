using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Assimalign.Cohesion.Database.Benchmarks.Datasets;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets.Tests;

internal sealed class DatasetRun : IDisposable
{
    private static int next;
    private readonly string root;
    internal DatasetGenerationOptions Options { get; }
    internal DatasetManifest Manifest { get; private set; } = null!;
    internal string Directory => DatasetFiles.DirectoryPath(Options);
    internal double Seconds { get; private set; }
    internal long Bytes { get { long result=0; foreach(var table in Manifest.Tables) result+=table.Bytes; return result; } }

    private DatasetRun(DatasetModel model,double scale,int threads,ulong seed)
    {
        string scratch=Environment.GetEnvironmentVariable("DBBENCH_DATA_SCRATCH") ?? @"C:\cw\bench\data-scratch";
        root=Path.Combine(scratch,"r1b-"+Environment.ProcessId+"-"+Interlocked.Increment(ref next));
        if(System.IO.Directory.Exists(root)) throw new IOException("Scratch directory already exists: "+root);
        Options=new DatasetGenerationOptions { Model=model,Scale=scale,Threads=threads,Seed=seed,OutputDirectory=root };
    }

    internal static async Task<DatasetRun> Generate(DatasetModel model,double scale=0.01,int threads=2,ulong seed=DatasetGenerationOptions.DefaultSeed)
    {
        var run=new DatasetRun(model,scale,threads,seed);
        try
        {
            var watch=Stopwatch.StartNew();
            run.Manifest=await DatasetGenerators.Get(model).GenerateAsync(run.Options,null,CancellationToken.None);
            run.Seconds=watch.Elapsed.TotalSeconds;
            return run;
        }
        catch { run.Dispose(); throw; }
    }

    internal IEnumerable<Dictionary<string,string?>> Csv(string name)
    {
        foreach(string path in DatasetFiles.EnumerateParts(Directory,Manifest,name))
        {
            using var stream=File.OpenRead(path); using var reader=new CsvReader(stream);
            while(reader.Read())
            {
                var row=new Dictionary<string,string?>(StringComparer.Ordinal);
                foreach(string header in reader.Headers) row.Add(header,reader.GetString(header));
                yield return row;
            }
        }
    }

    internal IEnumerable<JsonDocument> Json(string name)
    {
        foreach(string path in DatasetFiles.EnumerateParts(Directory,Manifest,name))
        {
            using var stream=File.OpenRead(path); using var reader=new JsonLinesReader(stream);
            while(reader.Read()) yield return JsonDocument.Parse(reader.Current);
        }
    }

    public void Dispose()
    {
        string full=Path.GetFullPath(root);
        if(!Path.GetFileName(full).StartsWith("r1b-",StringComparison.Ordinal)) throw new IOException("Unexpected scratch path.");
        if(System.IO.Directory.Exists(full)) System.IO.Directory.Delete(full,true);
    }
    internal static long Long(Dictionary<string,string?> row,string name) => long.Parse(row[name]!,CultureInfo.InvariantCulture);
    internal static decimal Money(Dictionary<string,string?> row,string name) => decimal.Parse(row[name]!,CultureInfo.InvariantCulture);
    internal static DateTime Time(Dictionary<string,string?> row,string name) => DateTime.ParseExact(row[name]!,"yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture,DateTimeStyles.AssumeUniversal|DateTimeStyles.AdjustToUniversal);
}
