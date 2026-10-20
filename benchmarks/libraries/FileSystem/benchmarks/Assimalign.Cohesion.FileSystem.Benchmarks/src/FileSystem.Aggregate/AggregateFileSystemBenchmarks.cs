using System;
using System.IO;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.FileSystem.Benchmarks.FileSystem.Aggregate;

/// <summary>
/// Route to the last of four or sixteen mounted in-memory providers, reject an unmounted path, and enumerate
/// the synthetic root and mounted contents. The pinned in-memory recursive enumerator emits files only, so
/// enumeration returns one mount directory and one file per source. Aggregate is a mount router, not an overlay.
/// No counterpart is used: System.IO does not implement this virtual mount contract.
/// </summary>
public class AggregateFileSystemBenchmarks
{
    private AggregateFileSystem _fileSystem = null!;
    private FileSystemPath _hit;
    private readonly FileSystemEnumerationOptions _options = new() { Recurse = true };

    [Params(4, 16)]
    public int Sources { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var builder = new AggregateFileSystemBuilder();
        for (int i = 0; i < Sources; i++)
        {
            var provider = new InMemoryFileSystem(new InMemoryFileSystemOptions());
            provider.CreateDirectory("/data");
            provider.CreateFile("/data/payload.bin");
            builder.Mount($"/source{i}", provider, ownsFileSystem: true);
        }
        _fileSystem = builder.Build();
        _hit = FileSystemPath.Parse($"/source{Sources - 1}/data/payload.bin");
        if (Lookup() is null || ExistsMiss() || Enumerate() != Sources * 2)
        {
            throw new InvalidOperationException("Aggregate routing or enumeration failed.");
        }
    }

    [GlobalCleanup]
    public void Cleanup() => _fileSystem.Dispose();

    [Benchmark]
    public IFileSystemFile Lookup() => _fileSystem.GetFile(_hit);

    [Benchmark]
    public bool ExistsMiss() => _fileSystem.Exists("/missing/data/payload.bin");

    [Benchmark]
    public int Enumerate()
    {
        int count = 0;
        foreach (IFileSystemInfo _ in _fileSystem.EnumerateFileSystem(_options))
        {
            count++;
        }
        return count;
    }
}
