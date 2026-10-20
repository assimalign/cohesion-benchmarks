using System;
using System.IO;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Assimalign.Cohesion.FileSystem.Benchmarks.FileSystem.Physical;

/// <summary>
/// Cached OS reads and buffered overwrites at 4 KiB and 1 MiB. ReadFile includes lookup and path parsing;
/// ReadCachedFile reuses the file-info object to isolate opening/copying from lookup allocations.
/// System.IO uses the same buffers, modes, access and sharing. Writes close the stream without a durable flush.
/// The original PhysicalFileSystemBenchmarks.ReadFile remains the unchanged 4 KiB history series.
/// </summary>
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class PhysicalFileOperationsBenchmarks
{
    private string _root = "";
    private string _fullPath = "";
    private PhysicalFileSystem _fileSystem = null!;
    private IFileSystemFile _file = null!;
    private byte[] _payload = [];
    private byte[] _buffer = [];
    private const string RelativePath = "data/reports/2026/payload.bin";

    [Params(4096, 1048576)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _root = Directory.CreateTempSubdirectory("cohesion-fs-operations-").FullName;
        _fullPath = System.IO.Path.Combine(_root, RelativePath);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_fullPath)!);
        _payload = new byte[Size];
        _buffer = new byte[Size];
        new Random(7).NextBytes(_payload);
        File.WriteAllBytes(_fullPath, _payload);
        _fileSystem = new PhysicalFileSystem(new PhysicalFileSystemOptions { Root = _root });
        _file = _fileSystem.GetFile(RelativePath);
        WriteFile();
        ReadFile_SystemIO();
        Verify();
        WriteFile_SystemIO();
        ReadFile();
        Verify();
        ReadCachedFile();
        Verify();
    }

    private void Verify()
    {
        if (!_buffer.AsSpan().SequenceEqual(_payload))
        {
            throw new InvalidOperationException("File contents differ.");
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _fileSystem.Dispose();
        Directory.Delete(_root, recursive: true);
    }

    [Benchmark, BenchmarkCategory("Read")]
    public int ReadFile()
    {
        using Stream stream = _fileSystem.GetFile(RelativePath).Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        stream.ReadExactly(_buffer);
        return _buffer.Length;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Read")]
    public int ReadFile_SystemIO()
    {
        using FileStream stream = File.Open(_fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        stream.ReadExactly(_buffer);
        return _buffer.Length;
    }

    [Benchmark, BenchmarkCategory("CachedRead")]
    public int ReadCachedFile()
    {
        using Stream stream = _file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        stream.ReadExactly(_buffer);
        return _buffer.Length;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CachedRead")]
    public int ReadCachedFile_SystemIO() => ReadFile_SystemIO();

    [Benchmark, BenchmarkCategory("Write")]
    public int WriteFile()
    {
        using Stream stream = _fileSystem.GetFile(RelativePath).Open(FileMode.Create, FileAccess.Write, FileShare.None);
        stream.Write(_payload);
        return _payload.Length;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Write")]
    public int WriteFile_SystemIO()
    {
        using FileStream stream = File.Open(_fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
        stream.Write(_payload);
        return _payload.Length;
    }
}
