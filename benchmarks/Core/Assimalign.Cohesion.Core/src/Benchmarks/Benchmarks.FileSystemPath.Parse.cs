using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Exporters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Columns;
using System.Reflection;
using BenchmarkDotNet.Exporters.Json;

namespace Assimalign.Cohesion.Benchmarks;

[CohesionJob(RuntimeMoniker.Net80)]
public class FileSystemPathBenchmarks
{
    private static char[] _invalidChars = [.. System.IO.Path.GetInvalidPathChars(), '*', '?', '!', '<', '>', '^'];
    private string _value = "//test/users\\aas/fasdf\\myfile.txt";

    public FileSystemPath Path { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Path = _value;
    }

    public FileSystemPath Par

    [Benchmark]
    [Arguments("//test/users\\aas/fasdf\\myfile.txt")]
    [Arguments("C:/test/users/aas/fasdf/myfile.txt")]
    public FileSystemPath Parse(string value)
    {
        return FileSystemPath.Parse(value);
    }

    [Benchmark()]
    public string[] GetSegments()
    {
        return Path.GetSegments();
    }

    //private class Config : ManualConfig
    //{
    //    private readonly Type _type = typeof(FileSystemPath);
    //    public Config()
    //    {
    //        AddJob(new Job("SimpleJob1", RunMode.Dry, new EnvironmentMode(CoreRuntime.Core60)));
    //        AddJob(new Job("SimpleJob2", RunMode.Dry, new EnvironmentMode(CoreRuntime.Core80)));
    //        AddJob(new Job("SimpleJob3", RunMode.Dry, new EnvironmentMode(CoreRuntime.Core90)));


    //        // AddColumn(new TagColumn("Fqdn", ProcessMethod));
    //        AddExporter(new JsonExporter("test", excludeMeasurements: true));
    //    }



    //    private string ProcessMethod(string name)
    //    {
    //        var methodInfo = _type
    //            .GetMethod(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

    //        var fullName = _type.FullName;

    //        if (methodInfo is not null)
    //        {
    //            return string.Join('.', fullName, methodInfo.Name);
    //        }

    //        return string.Join(" - ", fullName, name);
    //    }
    //}
}
