using System;
using System.IO;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Assimalign.Cohesion.FileSystem.Benchmarks.FileSystem.Physical;

/// <summary>
/// Resolve an existing deep file and recursively enumerate a tree of 128 files in eight directories.
/// Lookup returns an existence result on both sides; enumeration materializes file-system info objects on
/// both sides and counts them. System.IO does not enforce a virtual root: containment is provider overhead.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class PhysicalDirectoryBenchmarks
{
    private string _root = "";
    private string _fullPath = "";
    private PhysicalFileSystem _fileSystem = null!;
    private DirectoryInfo _directory = null!;
    private readonly FileSystemEnumerationOptions _options = new() { Recurse = true, AttributesToSkip = 0 };
    private readonly EnumerationOptions _systemOptions = new() { RecurseSubdirectories = true, AttributesToSkip = 0 };
    private const string RelativePath = "data/part7/file15.txt";

    [GlobalSetup]
    public void Setup()
    {
        _root = Directory.CreateTempSubdirectory("cohesion-fs-directory-").FullName;
        for (int directory = 0; directory < 8; directory++)
        {
            string path = System.IO.Path.Combine(_root, "data", $"part{directory}");
            Directory.CreateDirectory(path);
            for (int file = 0; file < 16; file++)
            {
                File.WriteAllText(System.IO.Path.Combine(path, $"file{file}.txt"), "payload");
            }
        }
        _fullPath = System.IO.Path.Combine(_root, RelativePath);
        _directory = new DirectoryInfo(_root);
        _fileSystem = new PhysicalFileSystem(new PhysicalFileSystemOptions { Root = _root, IsReadOnly = true });
        if (!ResolveFile() || !ResolveFile_SystemIO() || Enumerate() != Enumerate_SystemIO())
        {
            throw new InvalidOperationException("Directory operations disagree.");
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _fileSystem.Dispose();
        Directory.Delete(_root, recursive: true);
    }

    [Benchmark, BenchmarkCategory("Resolve")]
    public bool ResolveFile() => _fileSystem.GetFile(RelativePath) is not null;

    [Benchmark(Baseline = true), BenchmarkCategory("Resolve")]
    public bool ResolveFile_SystemIO() => new FileInfo(_fullPath).Exists;

    [Benchmark, BenchmarkCategory("Enumerate")]
    public int Enumerate()
    {
        int length = 0;
        foreach (IFileSystemInfo info in _fileSystem.EnumerateFileSystem(_options))
        {
            length++;
        }
        return length;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Enumerate")]
    public int Enumerate_SystemIO()
    {
        int length = 0;
        foreach (FileSystemInfo info in _directory.EnumerateFileSystemInfos("*", _systemOptions))
        {
            length++;
        }
        return length;
    }
}
