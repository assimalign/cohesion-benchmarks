using System;
using System.IO;

using Assimalign.Cohesion.Content.Ebml;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Ebml;

/// <summary>
/// Scans typed EBML elements and loads their editable tree. The synthetic schema includes a master
/// containing repeated integer/string records, exercising VINT, boundary and parent validation.
/// No credible counterpart offering the generic schema-aware EBML contract is used.
/// </summary>
public class EbmlReaderBenchmarks
{
    private byte[] _bytes = null!;
    private EbmlSchema _schema = null!;

    [GlobalSetup]
    public void Setup()
    {
        VInt master = VInt.MakeId(0xa0);
        _schema = new EbmlSchemaBuilder().Include(EbmlSchema.Header)
            .Add(EbmlElementDefinition.Create(master, "Records", EbmlElementType.MasterElement))
            .Add(EbmlElementDefinition.Create(VInt.MakeId(0x81), "Number", EbmlElementType.UnsignedInteger, [master]))
            .Add(EbmlElementDefinition.Create(VInt.MakeId(0x82), "Label", EbmlElementType.Utf8String, [master]))
            .Build();
        using MemoryStream output = new();
        using (EbmlWriter writer = EbmlWriter.Create(output, _schema))
        {
            writer.WriteElement(new EbmlHeader { DocType = "bench" }.Element);
            writer.BeginMaster(master, sizeLength: 4, backPatch: true);
            for (ulong index = 0; index < 16; index++)
            {
                writer.WriteUnsignedInteger(VInt.MakeId(0x81), index);
                writer.WriteUtf8String(VInt.MakeId(0x82), "record café");
            }
            writer.EndMaster();
        }
        _bytes = output.ToArray();
        if (ReadValues() == 0 || Load() != 1) { throw new InvalidOperationException("Invalid EBML fixture."); }
    }

    [Benchmark]
    public int ReadValues()
    {
        using MemoryStream source = new(_bytes, writable: false);
        using EbmlReader reader = EbmlReader.Create(source, _schema);
        return Scan(reader);
    }

    [Benchmark]
    public int Load()
    {
        using MemoryStream source = new(_bytes, writable: false);
        using EbmlDocument document = EbmlDocument.Load(source, _schema);
        return document.Body.Elements.Count;
    }

    private static int Scan(EbmlReader reader)
    {
        int count = 0;
        while (reader.ReadNext())
        {
            if (reader.Current.Type == EbmlElementType.MasterElement)
            {
                reader.EnterMaster();
                count += Scan(reader);
                reader.LeaveMaster();
            }
            else
            {
                _ = reader.ReadValue();
                count++;
            }
        }
        return count;
    }
}
