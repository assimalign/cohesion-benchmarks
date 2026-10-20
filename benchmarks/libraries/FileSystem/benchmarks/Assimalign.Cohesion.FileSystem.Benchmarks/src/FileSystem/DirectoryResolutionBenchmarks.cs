using System;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.FileSystem.Benchmarks.FileSystem;

/// <summary>
/// Resolve a path relative to an IFileSystemDirectory via the root package's public Exists extension.
/// Uses an in-memory provider to expose merge and tree-walk costs without disk access. No direct System.IO
/// comparison: that API has no provider-relative directory abstraction.
/// </summary>
public class DirectoryResolutionBenchmarks
{
    private InMemoryFileSystem _fileSystem = null!;
    private IFileSystemDirectory _directory = null!;

    [Params("payload.bin", "nested/reports/payload.bin")]
    public string RelativePath { get; set; } = "";

    [GlobalSetup]
    public void Setup()
    {
        _fileSystem = new InMemoryFileSystem(new InMemoryFileSystemOptions());
        _directory = _fileSystem.CreateDirectory("/data");
        _fileSystem.CreateDirectory("/data/nested/reports");
        _fileSystem.CreateFile("/data/" + RelativePath);
        if (!Resolve())
        {
            throw new InvalidOperationException("Relative directory lookup failed.");
        }
    }

    [GlobalCleanup]
    public void Cleanup() => _fileSystem.Dispose();

    [Benchmark]
    public bool Resolve() => _directory.Exists(RelativePath);
}
