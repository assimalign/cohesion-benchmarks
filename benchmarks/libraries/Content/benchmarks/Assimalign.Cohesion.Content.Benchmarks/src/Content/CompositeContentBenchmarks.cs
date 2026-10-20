using System;
using System.IO;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content;

/// <summary>
/// Enumerates a logical content group and drains its eight in-memory parts through their public
/// streams. CompositeContent has no serialized byte representation: consumers enumerate its items.
/// No third-party equivalent with the same content ownership and enumeration contract is used.
/// </summary>
public class CompositeContentBenchmarks
{
    private CompositeContent _content = null!;
    private byte[] _buffer = null!;

    [GlobalSetup]
    public async Task Setup()
    {
        IContent[] parts = new IContent[8];
        for (int index = 0; index < parts.Length; index++)
        {
            byte[] bytes = new byte[257];
            Array.Fill(bytes, (byte)index);
            parts[index] = MemoryContent.Create(bytes);
        }
        _content = CompositeContent.Create(parts);
        _buffer = new byte[128];
        if (await ReadParts() != 2056) { throw new InvalidOperationException("Expected all eight content parts."); }
    }

    [GlobalCleanup]
    public void Cleanup() => _content.Dispose();

    [Benchmark]
    public async Task<int> ReadParts()
    {
        int length = 0;
        await foreach (IContent item in _content.GetItemsAsync())
        {
            using Stream source = item.OpenRead();
            int count;
            while ((count = source.Read(_buffer)) != 0) { length += count; }
        }
        return length;
    }
}
