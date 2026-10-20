using System;
using System.Globalization;
using System.IO;
using System.Text;

using Assimalign.Cohesion.Content.Pdf;

using BenchmarkDotNet.Attributes;

using PdfPigDocument = UglyToad.PdfPig.PdfDocument;
using PdfPigOptions = UglyToad.PdfPig.ParsingOptions;

namespace Assimalign.Cohesion.Content.Benchmarks.Content.Pdf;

/// <summary>
/// Opens a classic-xref PDF and resolves every page's inherited geometry. PdfPig (Apache-2.0) runs
/// with lenient parsing disabled. Empty pages deliberately avoid content operators/text extraction,
/// which Cohesion does not implement. Both sides return the same sum of page widths and heights;
/// this compares the structural/page-tree use case, not the libraries' full PDF feature sets.
/// </summary>
public class PdfPageBenchmarks
{
    private byte[] _bytes = null!;
    private PdfReader _reader = null!;
    private PdfPigOptions _options = null!;

    [Params(1, 8)]
    public int Pages { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _reader = new PdfReader();
        _options = new PdfPigOptions { UseLenientParsing = false };
        using MemoryStream output = new();
        Write(output, "%PDF-1.7\n");
        long[] offsets = new long[Pages + 2];
        StringBuilder kids = new();
        for (int index = 0; index < Pages; index++) { kids.Append(index + 3).Append(" 0 R "); }
        for (int index = 0; index < offsets.Length; index++)
        {
            offsets[index] = output.Position;
            string body = index switch
            {
                0 => "<< /Type /Catalog /Pages 2 0 R >>",
                1 => $"<< /Type /Pages /Kids [{kids}] /Count {Pages} /MediaBox [0 0 612 792] /Resources <<>> >>",
                _ => "<< /Type /Page /Parent 2 0 R >>",
            };
            Write(output, $"{index + 1} 0 obj\n{body}\nendobj\n");
        }
        long xref = output.Position;
        Write(output, $"xref\n0 {offsets.Length + 1}\n0000000000 65535 f \n");
        foreach (long offset in offsets) { Write(output, offset.ToString("D10", CultureInfo.InvariantCulture) + " 00000 n \n"); }
        Write(output, $"trailer\n<< /Size {offsets.Length + 1} /Root 1 0 R >>\nstartxref\n{xref}\n%%EOF\n");
        _bytes = output.ToArray();
        if (ReadGeometry() != Pages * 1404 || ReadGeometry_PdfPig() != Pages * 1404)
        {
            throw new InvalidOperationException("Expected identical inherited page dimensions.");
        }
    }

    [Benchmark]
    public double ReadGeometry()
    {
        using MemoryStream source = new(_bytes, writable: false);
        using PdfDocument document = _reader.Read(source);
        double sum = 0;
        foreach (PdfPage page in document.Pages)
        {
            sum += ((PdfInteger)page.MediaBox[2]).Value - ((PdfInteger)page.MediaBox[0]).Value;
            sum += ((PdfInteger)page.MediaBox[3]).Value - ((PdfInteger)page.MediaBox[1]).Value;
        }
        return sum;
    }

    [Benchmark(Baseline = true)]
    public double ReadGeometry_PdfPig()
    {
        using MemoryStream source = new(_bytes, writable: false);
        using PdfPigDocument document = PdfPigDocument.Open(source, _options);
        double sum = 0;
        foreach (var page in document.GetPages()) { sum += page.Width + page.Height; }
        return sum;
    }

    private static void Write(Stream output, string text) => output.Write(Encoding.ASCII.GetBytes(text));
}
