using System;
using System.Buffers.Binary;
using System.IO;

using Assimalign.Cohesion.Content.Exe;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Exe;

/// <summary>
/// Detects and inspects synthetic 64-bit PE, ELF and Mach-O headers, including one native table entry.
/// The reader produces detached metadata; it never executes these bytes. No common permissive library
/// supplies the same inspection contract across all three formats, so these are standalone cases.
/// </summary>
public class ExecutableReaderBenchmarks
{
    private byte[] _bytes = null!;
    private ExecutableReader _reader = null!;

    [Params("PE", "ELF", "MachO")]
    public string Format { get; set; } = "PE";

    [GlobalSetup]
    public void Setup()
    {
        _reader = new ExecutableReader();
        _bytes = Format switch
        {
            "PE" => CreatePe(),
            "ELF" => CreateElf(),
            _ => CreateMachO(),
        };
        if (Read().Bitness != 64) { throw new InvalidOperationException("Expected a 64-bit executable header."); }
    }

    [Benchmark]
    public IExecutableFile Read()
    {
        using MemoryStream source = new(_bytes, writable: false);
        return _reader.Read(source);
    }

    private static byte[] CreatePe()
    {
        byte[] bytes = new byte[240];
        Put16(bytes, 0, 0x5a4d);
        Put32(bytes, 60, 64);
        Put32(bytes, 64, 0x4550);
        Put16(bytes, 68, 0x8664);
        Put16(bytes, 70, 1);
        Put16(bytes, 84, 112);
        Put16(bytes, 86, 0x22);
        Put16(bytes, 88, 0x20b);
        Put32(bytes, 120, 0x1000);
        Put32(bytes, 124, 0x200);
        Put32(bytes, 144, 0x2000);
        Put32(bytes, 148, 240);
        Put16(bytes, 156, 3);
        // A zero-length .text section has no payload to read.
        ".text"u8.CopyTo(bytes.AsSpan(200));
        Put32(bytes, 212, 0x1000);
        Put32(bytes, 236, 0x60000020);
        return bytes;
    }

    private static byte[] CreateElf()
    {
        byte[] bytes = new byte[120];
        new byte[] { 0x7f, 69, 76, 70, 2, 1, 1 }.CopyTo(bytes, 0);
        Put16(bytes, 16, 2);
        Put16(bytes, 18, 62);
        Put32(bytes, 20, 1);
        Put64(bytes, 32, 64);
        Put16(bytes, 52, 64);
        Put16(bytes, 54, 56);
        Put16(bytes, 56, 1);
        Put32(bytes, 64, 1);
        Put32(bytes, 68, 5);
        Put64(bytes, 96, 120);
        Put64(bytes, 104, 120);
        Put64(bytes, 112, 0x1000);
        return bytes;
    }

    private static byte[] CreateMachO()
    {
        byte[] bytes = new byte[56];
        Put32(bytes, 0, 0xfeedfacf);
        Put32(bytes, 4, 0x1000007);
        Put32(bytes, 8, 3);
        Put32(bytes, 12, 2);
        Put32(bytes, 16, 1);
        Put32(bytes, 20, 24);
        Put32(bytes, 32, 0x1b); // LC_UUID, 24 bytes including its header.
        Put32(bytes, 36, 24);
        return bytes;
    }

    private static void Put16(byte[] bytes, int offset, ushort value) => BinaryPrimitives.WriteUInt16LittleEndian(bytes.AsSpan(offset), value);
    private static void Put32(byte[] bytes, int offset, uint value) => BinaryPrimitives.WriteUInt32LittleEndian(bytes.AsSpan(offset), value);
    private static void Put64(byte[] bytes, int offset, ulong value) => BinaryPrimitives.WriteUInt64LittleEndian(bytes.AsSpan(offset), value);
}
