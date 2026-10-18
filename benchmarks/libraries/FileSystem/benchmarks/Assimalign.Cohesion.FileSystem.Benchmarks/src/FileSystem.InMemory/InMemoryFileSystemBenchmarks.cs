using System;
using System.IO;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.FileSystem.Benchmarks.FileSystem.InMemory;

/// <summary>
/// Opening and reading a 4 KiB file from an <see cref="InMemoryFileSystem"/> by path: path resolution through the
/// directory tree, the open-handle registration that enforces <see cref="FileShare"/>, and the copy out of the file's
/// buffer. Tests, fixtures and short-lived caches read through this path.
/// </summary>
/// <remarks>There is no in-memory file-system counterpart in .NET, so there is no baseline.</remarks>
public class InMemoryFileSystemBenchmarks
{
    private const string FilePath = "/data/reports/2026/payload.bin";

    private readonly byte[] _buffer = new byte[4096];
    private InMemoryFileSystem _fileSystem = null!;

    [GlobalSetup]
    public void Setup()
    {
        _fileSystem = new InMemoryFileSystem(new InMemoryFileSystemOptions());

        byte[] payload = new byte[_buffer.Length];
        new Random(7).NextBytes(payload);

        _fileSystem.CreateDirectory("/data/reports/2026");
        using (Stream stream = _fileSystem.CreateFile(FilePath).Open(FileMode.Open, FileAccess.Write))
        {
            stream.Write(payload);
        }

        if (ReadFile() != payload.Length || !_buffer.AsSpan().SequenceEqual(payload))
        {
            throw new InvalidOperationException("The payload was not read back in full.");
        }
    }

    [GlobalCleanup]
    public void Cleanup() => _fileSystem.Dispose();

    [Benchmark]
    public int ReadFile()
    {
        using Stream stream = _fileSystem.GetFile(FilePath).Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        stream.ReadExactly(_buffer);
        return _buffer.Length;
    }
}
