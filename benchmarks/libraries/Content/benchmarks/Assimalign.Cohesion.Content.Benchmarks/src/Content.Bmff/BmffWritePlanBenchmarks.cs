using System;
using System.IO;

using Assimalign.Cohesion.Content.Bmff;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Bmff;

/// <summary>Saving bounded container metadata; distinguishes zero and nonzero internal prefixes.</summary>
public class BmffWritePlanBenchmarks
{
    private BmffDocument _document = null!;
    private MemoryStream _output = null!;

    [Params(1, 16)]
    public int Containers { get; set; }

    [Params(false, true)]
    public bool Prefixed { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _document = new BmffDocument();
        for (int i = 0; i < Containers; i++)
        {
            _document.Boxes.Add(Prefixed ? new MetaBox(0, 0) : new MovieBox(0));
        }
        _output = new MemoryStream(Containers * 12);
        long expectedLength = Containers * (Prefixed ? 12 : 8);
        if (Save() != expectedLength)
        {
            throw new InvalidOperationException("Container size changed.");
        }
        using MemoryStream source = new(_output.ToArray(), writable: false);
        using BmffDocument copy = BmffDocument.Load(source);
        if (copy.Boxes.Count != Containers)
        {
            throw new InvalidOperationException("Saved containers did not round-trip.");
        }
    }

    [Benchmark]
    public long Save()
    {
        _output.SetLength(0);
        _document.Save(_output);
        return _output.Length;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _document.Dispose();
        _output.Dispose();
    }
}
