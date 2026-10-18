using System;
using System.IO;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.FileSystem.Benchmarks.FileSystem.Physical;

/// <summary>
/// Opening and reading a 4 KiB file through <see cref="PhysicalFileSystem"/> by a root-relative path, against
/// <see cref="File.OpenRead(string)"/> on the full path (the baseline). The difference is what the abstraction costs
/// per file a caller reads through it: path parsing, root containment, and the file-info object.
/// </summary>
/// <remarks>The file lives in a fresh temporary directory, so after the first read it is served from the OS cache.</remarks>
public class PhysicalFileSystemBenchmarks
{
    private const string RelativePath = "data/payload.bin";

    private readonly byte[] _buffer = new byte[4096];
    private string _directory = "";
    private string _fullPath = "";
    private PhysicalFileSystem _fileSystem = null!;

    [GlobalSetup]
    public void Setup()
    {
        _directory = Directory.CreateTempSubdirectory("cohesion-fs-bench-").FullName;
        _fullPath = System.IO.Path.Combine(_directory, "data", "payload.bin");
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_fullPath)!);
        File.WriteAllBytes(_fullPath, CreatePayload(_buffer.Length));

        _fileSystem = new PhysicalFileSystem(new PhysicalFileSystemOptions { Root = _directory, IsReadOnly = true });

        if (ReadFile() != _buffer.Length || ReadFile_SystemIO() != _buffer.Length)
        {
            throw new InvalidOperationException("The payload was not read in full.");
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _fileSystem.Dispose();
        Directory.Delete(_directory, recursive: true);
    }

    [Benchmark(Baseline = true)]
    public int ReadFile_SystemIO()
    {
        using FileStream stream = File.OpenRead(_fullPath);
        stream.ReadExactly(_buffer);
        return _buffer.Length;
    }

    [Benchmark]
    public int ReadFile()
    {
        using Stream stream = _fileSystem.GetFile(RelativePath).Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        stream.ReadExactly(_buffer);
        return _buffer.Length;
    }

    private static byte[] CreatePayload(int length)
    {
        byte[] payload = new byte[length];
        new Random(7).NextBytes(payload);
        return payload;
    }
}
