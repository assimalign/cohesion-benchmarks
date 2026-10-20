using System;

using Assimalign.Cohesion.Content.Binary;
using Assimalign.Cohesion.Content.Mkv;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Mkv;

/// <summary>
/// Parses SimpleBlock framing into borrowed frame segments for each multi-frame lacing algorithm.
/// Codec bytes are six synthetic bytes; no media decoder or external fixture is involved.
/// No permissive counterpart with equivalent validated lace framing and lazy segments is used.
/// </summary>
public class MkvBlockBenchmarks
{
    private BinarySegment _payload;

    [Params(MkvLacing.Xiph, MkvLacing.FixedSize, MkvLacing.Ebml)]
    public MkvLacing Lacing { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        byte[] bytes = Lacing switch
        {
            MkvLacing.Xiph => [0x81, 0, 0, 0x82, 2, 2, 2, 1, 2, 3, 4, 5, 6],
            MkvLacing.FixedSize => [0x81, 0, 0, 0x84, 2, 1, 2, 3, 4, 5, 6],
            _ => [0x81, 0, 0, 0x86, 2, 0x82, 0xbf, 1, 2, 3, 4, 5, 6],
        };
        _payload = BinarySegment.Create(bytes);
        if (Parse().Frames.Count != 3) { throw new InvalidOperationException("Expected three laced frames."); }
    }

    [Benchmark]
    public MkvBlock Parse() => MkvBlock.Parse(_payload, 32);
}
