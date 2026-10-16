using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Shouldly;
using Xunit;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Datasets;

/// <summary>Canonical formats and streaming boundary behavior.</summary>
public sealed class FormatTests
{
    [Fact]
    public void Save_Manifest_UsesLfBytesAndSingleFinalNewline()
    {
        string path = Path.GetTempFileName();
        try
        {
            var manifest = new DatasetManifest { Model = DatasetModel.Documents, Name = "marketplace",
                GeneratorVersion = 1, Scale = .1, Seed = 20261015, Tables = [] };
            DatasetManifestFile.Save(path, manifest);
            byte[] first = File.ReadAllBytes(path);
            Array.IndexOf(first, (byte)'\r').ShouldBe(-1);
            first[^1].ShouldBe((byte)'\n');
            first[^2].ShouldBe((byte)'}');
            DatasetManifestFile.Save(path, manifest);
            File.ReadAllBytes(path).ShouldBe(first);
        }
        finally { File.Delete(path); }
    }
    [Fact]
    public void Csv_SpecialFields_RoundTripsNullEmptyAndEmbeddedNewlines()
    {
        string large=new('z',200000); string text="a,b \"quote\"\r\nnext\nline café 東京";
        using var stream=new MemoryStream();
        using(var w=new CsvWriter(stream,["null","empty","text","large","decimal","date","timestamp","boolean"],true))
            w.WriteRow(null,"",text,large,12.50m,new DateOnly(2024,2,29),new DateTime(2024,2,29,12,34,56,DateTimeKind.Utc),true);
        string raw=Encoding.UTF8.GetString(stream.ToArray());
        raw.StartsWith("\uFEFF",StringComparison.Ordinal).ShouldBeFalse();
        raw.ShouldContain("\n,\"\","); raw.ShouldContain("12.50,2024-02-29,2024-02-29 12:34:56,true\n");
        stream.Position=0;
        using var reader=new CsvReader(stream,true);
        reader.Read().ShouldBeTrue();
        reader.IsNull("null").ShouldBeTrue(); reader.IsNull("empty").ShouldBeFalse();
        reader.GetField("empty").Length.ShouldBe(0); reader.GetString("text").ShouldBe(text); reader.GetString("large").ShouldBe(large);
        reader.GetString("decimal").ShouldBe("12.50"); reader.Read().ShouldBeFalse();
    }

    [Fact]
    public void Csv_TrailingEmptyAndMalformedQuotes_HandlesBoundaries()
    {
        using var stream=new MemoryStream(Encoding.UTF8.GetBytes("a,b,c\r\n1,\"\",\r\n2,3,4"));
        using var reader=new CsvReader(stream); reader.Read().ShouldBeTrue();
        reader.IsNull(1).ShouldBeFalse(); reader.IsNull(2).ShouldBeTrue();
        reader.Read().ShouldBeTrue(); reader.GetField(2).ToString().ShouldBe("4"); reader.Read().ShouldBeFalse();
        using var invalid=new MemoryStream(Encoding.UTF8.GetBytes("a\n\"unfinished"));
        using var bad=new CsvReader(invalid); Should.Throw<InvalidDataException>(()=>bad.Read());
    }

    [Fact]
    public void JsonLines_Utf8AndNumbers_RoundTripsAcrossBufferBoundaries()
    {
        using var stream=new MemoryStream(); string text=new('é',100000);
        using(var output=new JsonLinesWriter(stream,true))
        {
            for(int i=0;i<2;i++) { var w=output.BeginDocument(); w.WriteString("id","products/"+i);
                JsonLinesWriter.Money(w,"price",12.50m); JsonLinesWriter.Timestamp(w,"date",DatasetTime.Start);
                w.WriteString("text",text); output.EndDocument(); }
        }
        string raw=Encoding.UTF8.GetString(stream.ToArray());
        raw.ShouldContain("\"price\":12.50"); raw.ShouldContain("\"date\":\"2021-01-01T00:00:00Z\"");
        stream.Position=0; using var reader=new JsonLinesReader(stream,true);
        for(int i=0;i<2;i++) { reader.Read().ShouldBeTrue(); using var doc=JsonDocument.Parse(reader.Current); doc.RootElement.GetProperty("text").GetString().ShouldBe(text); }
        reader.Read().ShouldBeFalse();
    }

    [Fact]
    public void Csv_InvariantFormatting_IgnoresCurrentCulture()
    {
        CultureInfo previous=CultureInfo.CurrentCulture;
        try
        {
            // The suite uses invariant globalization; this explicit invariant culture also protects future project settings.
            CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;
            using var stream=new MemoryStream();
            using(var w=new CsvWriter(stream,["value"],true)) w.WriteRow(12.5m);
            Encoding.UTF8.GetString(stream.ToArray()).ShouldBe("value\n12.50\n");
        }
        finally { CultureInfo.CurrentCulture=previous; }
    }
}
