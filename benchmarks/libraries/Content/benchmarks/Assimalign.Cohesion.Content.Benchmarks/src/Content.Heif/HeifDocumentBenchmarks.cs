using System;
using System.IO;

using Assimalign.Cohesion.Content.Binary;
using Assimalign.Cohesion.Content.Bmff;
using Assimalign.Cohesion.Content.Heif;

using BenchmarkDotNet.Attributes;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Heif;

/// <summary>
/// Loads HEIC/AVIF item metadata, including item locations and image-property associations.
/// Four placeholder payload bytes are synthesized in setup, without decoding images. MetadataExtractor
/// extracts different metadata (including EXIF internals), not this editable item graph; no ratio is claimed.
/// </summary>
public class HeifDocumentBenchmarks
{
    private byte[] _bytes = null!;

    [Params("heic", "avif")]
    public string Brand { get; set; } = "heic";

    [GlobalSetup]
    public void Setup()
    {
        using BmffDocument document = new();
        document.Boxes.Add(new FileTypeBox(0)
        {
            MajorBrand = FourCC.Parse(Brand).Value,
            CompatibleBrands = [FourCC.Parse("mif1").Value],
        });
        MetaBox meta = new(0, 0);
        meta.Children.Add(new HandlerBox(0, 0) { HandlerType = FourCC.Parse("pict") });
        meta.Children.Add(new PrimaryItemReferenceBox(0, 0) { ItemId = 1 });
        ItemInfoBox info = new(0, 0);
        info.Children.Add(new ItemInfoEntryBox { ItemId = 1, ItemType = FourCC.Parse(Brand == "avif" ? "av01" : "hvc1"), ItemName = "image" });
        meta.Children.Add(info);
        meta.Children.Add(new ItemLocationBox(0, 0)
        {
            Version = 1, OffsetSize = 4, LengthSize = 4,
            Entries = [new() { ItemId = 1, ConstructionMethod = 1, Extents = [new() { Offset = 0, Length = 4 }] }],
        });
        ItemPropertyContainerBox properties = new();
        properties.Children.Add(new ImageSpatialExtentsBox { Width = 64, Height = 64 });
        properties.Children.Add(Brand == "avif"
            ? new Av1ConfigurationBox { Payload = BinarySegment.Create(new byte[] { 0x81, 0, 0, 0 }) }
            : new HevcConfigurationBox { Payload = BinarySegment.Create(new byte[] { 1, 2, 3, 4 }) });
        ItemPropertiesBox propertyBox = new();
        propertyBox.Children.Add(properties);
        propertyBox.Children.Add(new ItemPropertyAssociationBox
        {
            Entries = [new() { ItemId = 1, Associations = [new() { PropertyIndex = 1, Essential = true }, new() { PropertyIndex = 2, Essential = true }] }],
        });
        meta.Children.Add(propertyBox);
        meta.Children.Add(new ItemDataBox { Payload = BinarySegment.Create(new byte[] { 1, 2, 3, 4 }) });
        document.Boxes.Add(meta);
        using MemoryStream output = new();
        document.Save(output);
        _bytes = output.ToArray();
        if (Load() != 1) { throw new InvalidOperationException("Expected one HEIF image item."); }
    }

    [Benchmark]
    public int Load()
    {
        using MemoryStream source = new(_bytes, writable: false);
        using HeifDocument document = HeifDocument.Load(source);
        return document.Items.Count;
    }
}
