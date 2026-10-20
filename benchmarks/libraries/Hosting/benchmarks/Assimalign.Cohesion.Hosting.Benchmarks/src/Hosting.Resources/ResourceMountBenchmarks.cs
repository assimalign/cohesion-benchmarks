using System;
using System.IO;

using BenchmarkDotNet.Attributes;

using Assimalign.Cohesion.Hosting.Resources;

namespace Assimalign.Cohesion.Hosting.Benchmarks.Hosting.Resources;

/// <summary>
/// Reads an in-process configuration/secret mount as owned bytes or a caller-owned plaintext stream.
/// The context resolves the mount on every read. No raw file or MemoryStream baseline is used because neither
/// implements the resource mount's ownership and cross-platform protected-file contract.
/// </summary>
public class ResourceMountBenchmarks
{
    private ResourceContext _context = null!;
    private byte[] _buffer = [];
    private string _fallbackPath = "";

    [Params(256, 65536)]
    public int ByteCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        byte[] payload = new byte[ByteCount];
        for (int index = 0; index < payload.Length; index++)
        {
            payload[index] = (byte)(index % 251);
        }

        _buffer = new byte[ByteCount];
        _fallbackPath = Path.Combine(AppContext.BaseDirectory, "unused-mount");
        _context = new ResourceContext(environmentName: "Benchmark", mounts:
            new System.Collections.Generic.Dictionary<string, ResourceMount>
            {
                ["settings"] = ResourceMount.FromBytes(payload)
            });
        if (!ReadAllBytes().AsSpan().SequenceEqual(payload) || ReadStream() != ByteCount
            || !_buffer.AsSpan().SequenceEqual(payload))
        {
            throw new InvalidOperationException("The mount payload did not round-trip.");
        }
    }

    [Benchmark]
    public byte[] ReadAllBytes() => _context.GetMount("settings", _fallbackPath).ReadAllBytes();

    [Benchmark]
    public int ReadStream()
    {
        using Stream stream = _context.GetMount("settings", _fallbackPath).OpenRead();
        stream.ReadExactly(_buffer);
        return _buffer.Length;
    }
}
