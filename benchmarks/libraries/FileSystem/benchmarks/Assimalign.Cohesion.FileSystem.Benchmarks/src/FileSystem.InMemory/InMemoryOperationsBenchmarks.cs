using System;
using System.IO;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.FileSystem.Benchmarks.FileSystem.InMemory;

/// <summary>
/// Buffered reads, overwrites, and create/write/delete cycles through the public provider. There is no
/// honest System.IO counterpart for the in-memory tree, handle-sharing checks and event dispatch semantics.
/// </summary>
public class InMemoryOperationsBenchmarks
{
    private InMemoryFileSystem _fileSystem = null!;
    private byte[] _payload = [];
    private byte[] _buffer = [];
    private const string Path = "/data/reports/2026/payload.bin";

    [Params(4096, 1048576)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _payload = new byte[Size];
        _buffer = new byte[Size];
        new Random(7).NextBytes(_payload);
        _fileSystem = new InMemoryFileSystem(new InMemoryFileSystemOptions());
        _fileSystem.CreateDirectory("/data/reports/2026");
        _fileSystem.CreateFile(Path);
        WriteFile();
        ReadFile();
        if (!_buffer.AsSpan().SequenceEqual(_payload) || CreateWriteDelete() != Size)
        {
            throw new InvalidOperationException("In-memory payload differs.");
        }
    }

    [GlobalCleanup]
    public void Cleanup() => _fileSystem.Dispose();

    [Benchmark]
    public int ReadFile()
    {
        using Stream stream = _fileSystem.GetFile(Path).Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        stream.ReadExactly(_buffer);
        return _buffer.Length;
    }

    [Benchmark]
    public int WriteFile()
    {
        using Stream stream = _fileSystem.GetFile(Path).Open(FileMode.Create, FileAccess.Write);
        stream.Write(_payload);
        return _payload.Length;
    }

    [Benchmark]
    public int CreateWriteDelete()
    {
        const string path = "/data/reports/2026/transient.bin";
        using (Stream stream = _fileSystem.CreateFile(path).Open(FileMode.Open, FileAccess.Write))
        {
            stream.Write(_payload);
        }
        _fileSystem.DeleteFile(path);
        return _payload.Length;
    }
}
