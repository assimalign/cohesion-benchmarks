using System;
using System.IO;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Core.Benchmarks.Core;

/// <summary>
/// Parsing and splitting <see cref="FileSystemPath"/>, the path type every FileSystem provider, Configuration file
/// source and glob filter takes. Every string handed to an <c>IFileSystem</c> member is parsed through the implicit
/// conversion, so <see cref="Parse(string)"/> is paid once per file-system call.
/// </summary>
/// <remarks>
/// Ported from the legacy <c>FileSystemPathBenchmarks</c>. There is no .NET counterpart that normalizes separators,
/// roots and UNC shares the same way, so there is no baseline.
/// </remarks>
public class FileSystemPathBenchmarks
{
    private FileSystemPath _path;
    private FileSystemPath _merge;

    [GlobalSetup]
    public void Setup()
    {
        _path = FileSystemPath.Parse("C:/test/users/aas/fasdf/myfile.txt");
        _merge = FileSystemPath.Parse("fasdf/myfile.txt/child");
    }

    /// <summary>A UNC-style path with mixed separators, and a drive path that is already normalized.</summary>
    [Benchmark]
    [Arguments("//test/users\\aas/fasdf\\myfile.txt")]
    [Arguments("C:/test/users/aas/fasdf/myfile.txt")]
    public FileSystemPath Parse(string value) => FileSystemPath.Parse(value);

    [Benchmark]
    public string[] GetSegments() => _path.GetSegments();

    [Benchmark]
    public FileSystemPath ParseSpan() => FileSystemPath.Parse("C:/test/users/aas/fasdf/myfile.txt".AsSpan());

    [Benchmark]
    public FileSystemPath Subpath() => _path.Subpath(1, 3);

    [Benchmark]
    public FileSystemPath Merge() => _path.Merge(_merge);
}
